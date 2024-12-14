using AutoMapper;
using WriteLens.Core.Application.Commands.Document;
using WriteLens.Core.Application.Commands.User;
using WriteLens.Core.Infrastructure.Data.MongoDb.Entities;
using WriteLens.Core.Infrastructure.Data.PostgresDb.Entities;
using WriteLens.Core.Models.DomainModels.User;
using WriteLens.Core.Models.DomainModels.Document;
using WriteLens.Core.WebAPI.DTOs.Document.Requests;
using WriteLens.Core.WebAPI.DTOs.Document.Responses;
using WriteLens.Core.WebAPI.DTOs.User.Requests;
using WriteLens.Core.WebAPI.DTOs.User.Responses;
using WriteLens.Shared.Entities;
using WriteLens.Shared.Models;
using WriteLens.Core.WebAPI.DTOs.Pagination;

namespace WriteLens.Core.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // * User
        CreateMap<UserEntity, User>();
        CreateMap<User, UserEntity>();
        CreateMap<User, UserResponseDto>();
        CreateMap<UpdateUserRequestDto, UpdateUserCommand>();
        
        // * Document
        CreateMap<DocumentEntity, Document>();
        CreateMap<Document, DocumentEntity>();
        CreateMap<Document, DocumentResponseDto>();
        CreateMap<Document, DocumentListItemResponseDto>();
        CreateMap<CreateDocumentRequestDto, CreateDocumentCommand>();
        CreateMap<UpdateDocumentRequestDto, UpdateDocumentCommand>();

        // * Document Query Params
        CreateMap<GetDocumentsQueryParamsDto, DocumentQueryParams>();

        // * Document Type
        CreateMap<DocumentTypeEntity, DocumentType>();
        CreateMap<DocumentType, DocumentTypeResponseDto>();
        CreateMap<DocumentTypeRulesetEntity, DocumentTypeRuleset>();

        // * Document Content
        CreateMap<DocumentContentWithAnalysisDataEntity, DocumentContentWithAnalysisData>();
        CreateMap<DocumentContentWithAnalysisData, DocumentContent>();
        CreateMap<DocumentContent, DocumentContentEntity>();
        CreateMap<DocumentContent, DocumentContentResponseDto>();
        CreateMap<UpdateDocumentContentRequestDto, UpdateDocumentCommand>();

        // * Document -> Document Content Section
        CreateMap<DocumentContentSectionEntity, DocumentContentSection>();
        CreateMap<UpdateDocumentSectionRequestDto, UpdateSectionCommand>();
        CreateMap<UpdateSectionCommand, DocumentContentSectionEntity>();
        CreateMap<DocumentContentSection, DocumentContentSectionEntity>();
        CreateMap<DocumentContentSection, DocumentContentSectionResponseDto>();

        // * Document -> Document Content Flags
        CreateMap<DocumentContentFlagEntity, DocumentContentFlag>();
        CreateMap<DocumentContentFlag, DocumentContentFlagEntity>();
        CreateMap<DocumentContentFlag, DocumentContentFlagResponseDto>();

        // * Document -> Document Content Flags -> Position
        CreateMap<DocumentContentFlagPositionEntity, DocumentContentFlagPosition>();
        CreateMap<DocumentContentFlagPosition, DocumentContentFlagPositionEntity>();
        CreateMap<DocumentContentFlagPosition, DocumentContentFlagPositionResponseDto>();

        // * Document -> Document Content Flags -> Suggestion
        CreateMap<DocumentContentFlagSuggestionEntity, DocumentContentFlagSuggestion>();
        CreateMap<DocumentContentFlagSuggestion, DocumentContentFlagSuggestionEntity>();
        CreateMap<DocumentContentFlagSuggestion, DocumentContentFlagSuggestionResponseDto>();

        // * Document -> Document Content Score
        CreateMap<DocumentContentScoreEntity, DocumentContentScore>();
        CreateMap<DocumentContentScore, DocumentContentScoreEntity>();
        CreateMap<DocumentContentDocumentScore, DocumentContentDocumentScoreEntity>();
        CreateMap<DocumentContentDocumentScoreEntity, DocumentContentDocumentScore>();
        CreateMap<DocumentContentDocumentScore, DocumentContentScoreResponseDto>();

        // * Document Pagination
        CreateMap<PaginatedList<Document>, PaginatedListResponseDto<DocumentListItemResponseDto>>();
        CreateMap<PaginationInfo, PaginationInfoResponseDto>();
    }
}