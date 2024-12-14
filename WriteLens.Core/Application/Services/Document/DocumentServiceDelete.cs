using AutoMapper;
using WriteLens.Core.Interfaces.Repositories;
using WriteLens.Core.Interfaces.Services;

namespace WriteLens.Core.Application.Services;

public class DocumentServiceDelete : IDocumentServiceDelete
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IDocumentContentRepository _documentContentRepository;
    public readonly IMapper _mapper;
    
    public DocumentServiceDelete(
        IDocumentRepository documentRepository,
        IDocumentContentRepository documentContentRepository,
        IMapper mapper)
    {
        _documentRepository = documentRepository;
        _documentContentRepository = documentContentRepository;
        _mapper = mapper;
    }

    public async Task DeleteSingleById(Guid userId, Guid documentId)
    {
        var deleteDocumentTask = _documentRepository.DeleteSingleByIdAsync(documentId);
        var deleteDocumentContentTask = _documentContentRepository.DeleteSingleByIdAsync(documentId);
        await Task.WhenAll(deleteDocumentTask, deleteDocumentContentTask);
    }
}