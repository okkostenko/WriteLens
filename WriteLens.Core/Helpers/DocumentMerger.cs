using AutoMapper;
using WriteLens.Core.Models.DomainModels.Document;
using WriteLens.Shared.Models;

namespace WriteLens.Core.Helpers;

public class DocumentMerger
{
    private readonly IMapper _mapper;

    public DocumentMerger(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public Document MergeMetadataWithContent(Document metadata, DocumentContentWithAnalysisData? content)
    {
        if (content != null)
        {
            metadata.Type = content.Type;
            metadata.Content = _mapper.Map<DocumentContent>(content);
            metadata.Score = content.Score;
            metadata.Flags = content.Flags;
        }

        return metadata;
    }
}