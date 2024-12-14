using AutoMapper;
using WriteLens.Core.Application.Validators;
using WriteLens.Core.Helpers;
using WriteLens.Core.Interfaces.Repositories;
using WriteLens.Core.Interfaces.Services;
using WriteLens.Core.Models.DomainModels.Document;
using WriteLens.Shared.Helpers;
using WriteLens.Shared.Models;

namespace WriteLens.Core.Application.Services;

public class DocumentServiceRead : IDocumentServiceRead
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IDocumentContentRepository _documentContentRepository;
    private readonly DocumentMerger _merger;
    public readonly IMapper _mapper;
    
    public DocumentServiceRead(
        IDocumentRepository documentRepository,
        IDocumentContentRepository documentContentRepository,
        IMapper mapper)
    {
        _documentRepository = documentRepository;
        _documentContentRepository = documentContentRepository;
        _mapper = mapper;
        _merger = new DocumentMerger(_mapper);
    }

    public async Task<Document> GetSingleByIdAsync(Guid userId, Guid documentId)
    {
        Document? document = await _documentRepository.GetSingleByIdAsync(documentId);

        DocumentValidator.ValidateDocumentExists(documentId, document);
        UserDocumentAccessValidator.ValidateUserHasAccessToDocument(userId, document);

        DocumentContentWithAnalysisData? documentContent = await _documentContentRepository
            .GetSingleByIdAsync(documentId);

        document = _merger.MergeMetadataWithContent(document, documentContent);

        return document;
    }

    public async Task<PaginatedList<Document>> GetManyByUserIdAsync(Guid userId, DocumentQueryParams queryParams)
    {
        List<Document>? documents = await _documentRepository
            .GetManyByUserIdAsync(userId, queryParams);

        List<Guid> documentsIds = documents.Select(d => d.Id).ToList();

        var documentsContent = await _documentContentRepository
            .GetManyByDocumentsIdsAsync(documentsIds, queryParams);
        
        documents = await MergeMetadataWithContent(documents, documentsContent);

        return Paginator<Document>.Paginate(
            documents, 
            new PaginationParams { 
                Page = queryParams.Page,
                Size = queryParams.Size
            }
        );
    }

    private async Task<List<Document>> MergeMetadataWithContent(
        List<Document>? documents,
        List<DocumentContentWithAnalysisData> documentsContent)
    {
        Dictionary<Guid, DocumentContentWithAnalysisData> documentsContentsMapping =
            documentsContent.ToDictionary(d => d.Id, d => d);

        for ( int i = 0; i < documents.Count; i ++ )
        {
            Document document = documents[i];
            DocumentContentWithAnalysisData? documentContent;
            documentsContentsMapping.TryGetValue(document.Id, out documentContent);
            documents[i] = _merger.MergeMetadataWithContent(document, documentContent);
        }

        return documents;
    }
}