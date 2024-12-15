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

    /// <summary>
    /// Get available document types.
    /// </summary>
    /// <returns> Returns a list of document types</returns>
    /// response code="200">Returns list of types.</response>
    [HttpGet("types")]
    [ProducesResponseType(typeof(List<DocumentTypeResponseDto>), 200)]
    public async Task<ActionResult<List<DocumentTypeResponseDto>>> GetDocumentTypes ()
    {
        List<DocumentType>? documentTypes = await _documentTypeService.GetAllAsync();
        return documentTypes.Select(_mapper.Map<DocumentTypeResponseDto>).ToList();
    }

    /// <summary>
    /// Create a new document.
    /// </summary>
    /// <param name="documentDto">The document to create.</param>
    /// <returns>Returns created document.</returns>
    /// <response code="201">Document created successfully.</response>
    /// <response code="400">Non-existing document type.</response>
    /// <response code="401">User is not authorized.</response>
    [HttpPost("create")]
    [ProducesResponseType(typeof(DocumentResponseDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
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

    /// <summary>
    /// Get document by ID.
    /// </summary>
    /// <param name="documentId">The ID of the document to retreive.</param>
    /// <returns>Returns the requested document.</returns>
    /// <response code="200">Returns the document.</response>
    /// <response code="401">User is not authorized.</response>
    /// <response code="403">User does not have access to the document.</response>
    /// <response code="404">Document with provided ID does not exists.</response>
    [HttpGet("{documentId}")]
    [ProducesResponseType(typeof(DocumentResponseDto), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
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

    /// <summary>
    /// Get the list of user's documents.
    /// </summary>
    /// <param name="queryParams">Request query parameters.</param>
    /// <returns>Returns a list of requested documents.</returns>
    /// <response code="200">Returns the document.</response>
    /// <response code="400">Validation error.</response>
    /// <response code="401">User is not authorized.</response>
    [HttpGet("my")]
    [ProducesResponseType(typeof(PaginatedListResponseDto<DocumentListItemResponseDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
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

    /// <summary>
    /// Update document metadata by ID.
    /// </summary>
    /// <param name="documentId">The ID of the document to update.</param>
    /// <param name="document">Document metadata to set.</param>
    /// <response code="200">Document meatadata updated successfully.</response>
    /// <response code="401">User is not authorized.</response>
    /// <response code="403">User does not have access to the document.</response>
    /// <response code="404">Document with provided ID does not exists.</response>
    [HttpPatch("{documentId}/update")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
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
    
    /// <summary>
    /// Update document content by ID.
    /// </summary>
    /// <param name="documentId">The ID of the document to update.</param>
    /// <param name="document">Document content to set.</param>
    /// <response code="200">Document content updated successfully.</response>
    /// <response code="400">Validation error.</response>
    /// <response code="401">User is not authorized.</response>
    /// <response code="403">User does not have access to the document.</response>
    /// <response code="404">Document with provided ID does not exists.</response>
    [HttpPut("{documentId}/content/update")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
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

    /// <summary>
    /// Delete document by ID.
    /// </summary>
    /// <param name="documentId">The ID for the document to delete.</param>
    /// <response code="200">Document deleted successfully.</response>
    /// <response code="401">User is not authorized.</response>
    /// <response code="403">User does not have access to the document.</response>
    /// <response code="404">Document with provided ID does not exists.</response>
    [HttpDelete("{documentId}/delete")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
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
    
    /// <summary>
    /// Check if the user has access to the document.
    /// </summary>
    /// <param name="documentId">The ID of the document to check access to.</param>
    /// <response code="200">User has access to the document with provided ID.</response>
    /// <response code="401">User is not authorized.</response>
    /// <response code="403">User does not have access to the document.</response>
    /// <response code="404">Document with provided ID does not exists.</response>
    [HttpGet("{documentId}/check-access")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
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