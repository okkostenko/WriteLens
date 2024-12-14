using System.Text.Json;
using WriteLens.Core.Application.Commands.Document;
using WriteLens.Core.Interfaces.Repositories;
using WriteLens.Core.Interfaces.Services;
using WriteLens.Core.Models.DomainModels.Document;
using WriteLens.Core.Models.Types;
using WriteLens.Shared.Models;

namespace WriteLens.Core.Application.Services;

public class DocumentService : IDocumentService
{
    private readonly IDocumentServiceCreate _createService;
    private readonly IDocumentServiceRead _readService;
    private readonly IDocumentServiceUpdate _updateService;
    private readonly IDocumentServiceDelete _deleteService;
    private readonly IDocumentScoreRepository _documentScoreRepository;
    private readonly IDocumentFlagsRepository _documentFlagsRepository;
    
    public DocumentService(
        IDocumentServiceCreate createService,
        IDocumentServiceRead readService,
        IDocumentServiceUpdate updateService,
        IDocumentServiceDelete deleteService,
        IDocumentScoreRepository documentScoreRepository,
        IDocumentFlagsRepository documentFlagsRepository)
    {
        _createService = createService;
        _readService = readService;
        _updateService = updateService;
        _deleteService = deleteService;

        _documentScoreRepository = documentScoreRepository;
        _documentFlagsRepository = documentFlagsRepository;
    }

    public async Task<Document> CreateSingleAsync(
        Guid userId, CreateDocumentCommand createDocumentCommand)
    {
        return await _createService.CreateSingleAsync(userId, createDocumentCommand);
    }

    public async Task<PaginatedList<Document>> GetManyByUserIdAsync(
        Guid userId, DocumentQueryParams queryParams)
    {
        return await _readService.GetManyByUserIdAsync(userId, queryParams);
    }

    public async Task<Document> GetSingleByIdAsync(
        Guid userId, Guid documentId)
    {
        return await _readService.GetSingleByIdAsync(userId, documentId);
    }

    public async Task DeleteSingleById(
        Guid userId, Guid documentId)
    {
        await GetSingleByIdAsync(userId, documentId);
        await _deleteService.DeleteSingleById(userId, documentId);
    }

    public async Task UpdateSingleByIdAsync(
        Guid userId,
        Guid documentId, 
        UpdateDocumentCommand updateDocumentCommand)
    {
        Document document = await GetSingleByIdAsync(userId, documentId);
        var updatedSections = await _updateService
            .UpdateSingleByIdAsync(document, updateDocumentCommand);
        
        var createdSectionsIds = await GetCreatedSectionsIds(updatedSections);
        var deletedSectionsIds = await GetDeletedSectionsIds(
            document, updatedSections.Select(s => (Guid)s.Id).ToList());

        if (
            (deletedSectionsIds is null || deletedSectionsIds.Count == 0) &&
            (createdSectionsIds is null || createdSectionsIds.Count == 0)
        )
            return;

        await Task.WhenAll(
            _documentScoreRepository.PushSectionsScoresAsync(
                documentId, createdSectionsIds),
            _documentScoreRepository.PullSectionsScoresBySectionsIdsAsync(
                documentId, deletedSectionsIds),
            _documentFlagsRepository.DeleteManyBySectionIdAsync(
                documentId, deletedSectionsIds)
        );
    }

    private async Task<List<Guid>?> GetDeletedSectionsIds(
        Document document, 
        List<Guid> updatedSectionsIds)
    {
        return document.Content?.Sections?
            .Where(s => !updatedSectionsIds.Contains(s.Id))
            .Select(s => s.Id)
            .ToList();
    }

    private async Task<List<Guid>?> GetCreatedSectionsIds(
        List<UpdateSectionCommand> updatedSections)
    {
        return updatedSections
            .Where(s => s.State == DocumentContentSectionState.Created)
            .Select(s => (Guid)s.Id)
            .ToList();
    }
}