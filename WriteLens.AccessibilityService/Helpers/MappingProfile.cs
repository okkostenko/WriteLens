using AutoMapper;
using WriteLens.Accessibility.Models.ApplicationModels;
using WriteLens.Accessibility.Models.DomainModels;
using WriteLens.Accessibility.WebAPI.DTOs.Responses;
using WriteLens.Shared.Entities;
using WriteLens.Shared.Models;

namespace WriteLens.Accessibility.Helpers;

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
        CreateMap<DocumentContentDocumentScore, DocumentContentScoreResponseDto>();

        // * Document Flags
        CreateMap<DocumentContentFlagEntity, DocumentContentFlag>();
        CreateMap<DocumentContentFlag, DocumentContentFlagEntity>();
        CreateMap<DocumentContentFlag, DocumentContentFlagResponseDto>();

        // * Document Flag Position
        CreateMap<DocumentContentFlagPositionEntity, DocumentContentFlagPosition>();
        CreateMap<DocumentContentFlagPosition, DocumentContentFlagPositionEntity>();
        CreateMap<DocumentContentFlagPosition, DocumentContentFlagPositionResponseDto>();

        // * Document Flag Suggestion
        CreateMap<DocumentContentFlagSuggestionEntity, DocumentContentFlagSuggestion>();
        CreateMap<DocumentContentFlagSuggestion, DocumentContentFlagSuggestionEntity>();
        CreateMap<DocumentContentFlagSuggestion, DocumentContentFlagSuggestionResponseDto>();

        // * Document Type
        CreateMap<DocumentTypeEntity, DocumentType>();
        CreateMap<DocumentTypeRulesetEntity, DocumentTypeRuleset>();

        // * Task
        CreateMap<TaskModel, TaskStatusResponseDto>();

        // * Accessibility Analysis Reault
        CreateMap<AccessibilityAnalysisResult, AccessibilityAnalysisResultResponseDto>();
    }
}