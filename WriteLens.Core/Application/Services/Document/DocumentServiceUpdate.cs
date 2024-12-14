using System.Text.Json;
using AutoMapper;
using WriteLens.Core.Application.Commands.Document;
using WriteLens.Core.Exceptions;
using WriteLens.Core.Helpers;
using WriteLens.Core.Interfaces.Repositories;
using WriteLens.Core.Interfaces.Services;
using WriteLens.Core.Models.DomainModels.Document;
using WriteLens.Shared.Models;

namespace WriteLens.Core.Application.Services;

public class DocumentServiceUpdate : IDocumentServiceUpdate
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IDocumentContentRepository _documentContentRepository;
    public readonly IMapper _mapper;
    
    public DocumentServiceUpdate(
        IDocumentRepository documentRepository,
        IDocumentContentRepository documentContentRepository,
        IMapper mapper)
    {
        _documentRepository = documentRepository;
        _documentContentRepository = documentContentRepository;
        _mapper = mapper;
    }

    public async Task<List<UpdateSectionCommand>> UpdateSingleByIdAsync(Document document, UpdateDocumentCommand updateDocumentCommand)
    {
        await UpdateSingleDocumentMetadataByIdAsync(document, updateDocumentCommand);
        var updatedSectionsIds = await UpdateSingleDocumentContentByIdAsync(document, updateDocumentCommand);
        
        return updatedSectionsIds;
    }

    private async Task UpdateSingleDocumentMetadataByIdAsync(Document document, UpdateDocumentCommand updateDocumentCommand)
    {
        await _documentRepository.UpdateSingleByIdAsync(
            document.Id, updateDocumentCommand);
    }

    private async Task<List<UpdateSectionCommand>> UpdateSingleDocumentContentByIdAsync(Document document, UpdateDocumentCommand updateDocumentCommand)
    {
        if (updateDocumentCommand.Sections == null || updateDocumentCommand.Sections.Count == 0)
            return new List<UpdateSectionCommand>();

        Dictionary<Guid, DocumentContentSection> documentSections = 
            document.Content.Sections.ToDictionary(s => s.Id, s => s);

        if (updateDocumentCommand.Sections != null)
        {
            for ( int i = 0; i < updateDocumentCommand.Sections.Count; i ++ )
            {
                UpdateSectionCommand section = updateDocumentCommand.Sections[i];
                if (!documentSections.ContainsKey((Guid)section.Id))
                {
                    throw new DocumentException(
                        @$"Document Exception: \
                        Section with id {section.Id} does not exist");
                }
                
                DocumentContentSection? currentSectionData = section.Id != null
                    ? documentSections[(Guid)section.Id]
                    : null;

                section = DocumentContentSectionDataSetter.SetData(
                    section, currentSectionData);
                
                updateDocumentCommand.Sections[i] = section;
            }
        }

        await  _documentContentRepository.UpdateSingleByIdAsync(document.Id, updateDocumentCommand);
        return updateDocumentCommand.Sections;
    }
}