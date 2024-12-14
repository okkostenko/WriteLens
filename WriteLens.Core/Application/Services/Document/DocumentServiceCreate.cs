using AutoMapper;
using WriteLens.Core.Application.Commands.Document;
using WriteLens.Core.Application.Validators;
using WriteLens.Core.Interfaces.Caching;
using WriteLens.Core.Interfaces.Repositories;
using WriteLens.Core.Interfaces.Services;
using WriteLens.Core.Models.DomainModels.Document;
using WriteLens.Shared.Models;


namespace WriteLens.Core.Application.Services;

public class DocumentServiceCreate : IDocumentServiceCreate
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IDocumentContentRepository _documentContentRepository;
    private readonly IDocumentTypeCache _documentTypeCache;
    private readonly IDocumentScoreService _documentScoreService;
    public readonly IMapper _mapper;
    
    public DocumentServiceCreate(
        IDocumentRepository documentRepository,
        IDocumentContentRepository documentContentRepository,
        IDocumentTypeCache documentTypeCache,
        IDocumentScoreService documentScoreService,
        IMapper mapper)
    {
        _documentRepository = documentRepository;
        _documentContentRepository = documentContentRepository;
        _documentTypeCache = documentTypeCache;
        _documentScoreService = documentScoreService;
        _mapper = mapper;
    }

    public async Task<Document> CreateSingleAsync(Guid userId, CreateDocumentCommand createDocumentCommand)
    {
        await DocumentTypeValidator.ValidateDocumentTypeExists(
            _documentTypeCache, createDocumentCommand.TypeId);
        
        var document = new Document(createDocumentCommand);

        var documentContent = new DocumentContent
        {
            Id = document.Id,
            TypeId = createDocumentCommand.TypeId,
            CreatedAt = document.CreatedAt
        };

        await _documentContentRepository.InsertSingleAsync(documentContent);
        await _documentScoreService.CreateSingleAsync(document.Id);
    
        await _documentRepository.AddSingleAsync(userId, document);

        document.Type = await _documentTypeCache
            .GetDocumentTypeByIdAsync(documentContent.TypeId);

        return document;
    }
}