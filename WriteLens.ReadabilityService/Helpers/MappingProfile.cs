using AutoMapper;
using WriteLens.Readability.Models.DomainModels;
using WriteLens.Readability.WebAPI.DTOs.Responses;
using WriteLens.Shared.Entities;
using WriteLens.Shared.Models;

namespace WriteLens.Readability.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // * Document Content
        CreateMap<DocumentContentEntity, DocumentContent>();
        CreateMap<DocumentContentSectionEntity, DocumentContentSection>();

        // * Document Score
        CreateMap<DocumentContentScoreEntity, DocumentContentScore>();
        CreateMap<DocumentContentDocumentScoreEntity, DocumentContentDocumentScore>();
        CreateMap<DocumentContentSectionScoreEntity, DocumentContentSectionScore>();
        CreateMap<DocumentContentSectionScore, DocumentContentSectionScoreEntity>();
        CreateMap<DocumentContentDocumentScore, DocumentContentDocumentScoreEntity>();
        CreateMap<DocumentContentDocumentScore, ReadabilityAnalysisResultResponseDto>();

        // * Document Type
        CreateMap<DocumentTypeEntity, DocumentType>();
        CreateMap<DocumentTypeRulesetEntity, DocumentTypeRuleset>();

        // * Task
        CreateMap<TaskModel, TaskStatusResponseDto>();
    }
}