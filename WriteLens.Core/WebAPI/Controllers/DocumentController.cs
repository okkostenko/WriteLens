using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using WriteLens.Core.Application.Commands.Document;
using WriteLens.Core.Exceptions;
using WriteLens.Core.Interfaces.Services;
using WriteLens.Core.Models.DomainModels.Document;
using WriteLens.Core.WebAPI.DTOs.Document.Requests;
using WriteLens.Core.WebAPI.DTOs.Document.Responses;
using WriteLens.Core.WebAPI.DTOs.Pagination;
using WriteLens.Shared.Models;

namespace WriteLens.Core.WebAPI.Controllers;


[ApiController]
[Route("api/v1/documents")]
public class DocumentController : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly IDocumentTypeService _documentTypeService;
    private readonly IMapper _mapper;

    public DocumentController(
        IDocumentService documentService,
        IDocumentTypeService documentTypeService,
        IMapper mapper)
    {
        _documentService = documentService;
        _documentTypeService = documentTypeService;
        _mapper = mapper;
    }

    [HttpGet("types")]
    public async Task<ActionResult<List<DocumentTypeResponseDto>>> GetDocumentTypes ()
    {
        List<DocumentType>? documentTypes = await _documentTypeService.GetAllAsync();
        return documentTypes.Select(_mapper.Map<DocumentTypeResponseDto>).ToList();
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<ActionResult<DocumentResponseDto>> CreateDocument(CreateDocumentRequestDto documentDto)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        try
        {
            Document document = await _documentService.CreateSingleAsync(
                new Guid(userId),
                _mapper.Map<CreateDocumentCommand>(documentDto));

            return CreatedAtAction(
                nameof(GetDocumentById),
                new {documentId = document.Id},
                _mapper.Map<DocumentResponseDto>(document)
            );
        }
        catch (DocumentTypeNotFoundException exc)
        {
            return BadRequest(new
            {
                error = exc.Message,
                exceptionType = exc.GetType().Name
            });
        }
    }

    [HttpGet("{documentId}")]
    [Authorize]
    public async Task<ActionResult<DocumentResponseDto>> GetDocumentById(Guid documentId)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        try
        {
            Document document = await _documentService.GetSingleByIdAsync(new Guid(userId), documentId);
            return _mapper.Map<DocumentResponseDto>(document);
        }
        catch (AccessDeniedException exc)
        {
            return Forbid(exc.Message);
        }
        catch (DocumentNotFoundException exc)
        {
            return NotFound(exc.Message);
        }
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<ActionResult<PaginatedListResponseDto<DocumentListItemResponseDto>>> GetDocumentsByUser([FromQuery] GetDocumentsQueryParamsDto queryParams)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        try
        {
            PaginatedList<Document> documents = await _documentService.GetManyByUserIdAsync(
                new Guid(userId),
                _mapper.Map<DocumentQueryParams>(queryParams)
            );

            return _mapper.Map<PaginatedListResponseDto<DocumentListItemResponseDto>>(documents);
        }
        catch (InvalidSortingFiledException exc)
        {
            return BadRequest(exc.Message);
        }
    }

    [HttpPatch("{documentId}/update")]
    [Authorize]
    public async Task<IActionResult> UpdateDocumentById(Guid documentId, UpdateDocumentRequestDto document)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        try
        {
            await _documentService.UpdateSingleByIdAsync(
                new Guid(userId),
                documentId,
                _mapper.Map<UpdateDocumentCommand>(document));
                
            return Ok();
        }
        catch (AccessDeniedException exc)
        {
            return Forbid(exc.Message);
        }
        catch (DocumentNotFoundException exc)
        {
            return NotFound(exc.Message);
        } 
    }
    
    [HttpPut("{documentId}/content/update")]
    [Authorize]
    public async Task<IActionResult> UpdateDocumentContentById(Guid documentId, UpdateDocumentContentRequestDto document)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        try
        {
            await _documentService.UpdateSingleByIdAsync(
                new Guid(userId),
                documentId,
                _mapper.Map<UpdateDocumentCommand>(document));

            return Ok();
        }
        catch (AccessDeniedException exc)
        {
            return Forbid(exc.Message);
        }
        catch (DocumentNotFoundException exc)
        {
            return NotFound(exc.Message);
        } 
        catch (DocumentException exc)
        {
            return BadRequest(exc.Message);
        }
    }

    [HttpDelete("{documentId}/delete")]
    [Authorize]
    public async Task<IActionResult> DeleteDocumentById(Guid documentId)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        try
        {
            await _documentService.DeleteSingleById(new Guid(userId), documentId);
            return Ok();
        }
        catch (AccessDeniedException exc)
        {
            return Forbid(exc.Message);
        }
        catch (DocumentNotFoundException exc)
        {
            return NotFound(exc.Message);
        } 
    }
    
    [HttpGet("{documentId}/check-access")]
    [Authorize]
    public async Task<IActionResult> CheckAccessToDocumentById(Guid documentId)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        try
        {
            await _documentService.GetSingleByIdAsync(new Guid(userId), documentId);
            return Ok();
        }
        catch (AccessDeniedException exc)
        {
            return Forbid(exc.Message);
        }
        catch (DocumentNotFoundException exc)
        {
            return NotFound(exc.Message);
        } 
    }
}