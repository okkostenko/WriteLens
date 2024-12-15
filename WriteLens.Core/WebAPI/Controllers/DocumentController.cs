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
    private readonly ILogger<DocumentController> _logger;

    public DocumentController(
        IDocumentService documentService,
        IDocumentTypeService documentTypeService,
        IMapper mapper,
        ILogger<DocumentController> logger)
    {
        _documentService = documentService;
        _documentTypeService = documentTypeService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Get available document types.
    /// </summary>
    /// <returns> Returns a list of document types</returns>
    /// <response code="200">Returns list of types.</response>
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
            
            _logger.LogInformation($"User {userId} created new document '{document.Id}'.");

            return CreatedAtAction(
                nameof(GetDocumentById),
                new {documentId = document.Id},
                _mapper.Map<DocumentResponseDto>(document)
            );
        }
        catch (DocumentTypeNotFoundException exc)
        {
            _logger.LogWarning($"Failed to find document type with ID '{documentDto.TypeId}'");
            return BadRequest(new
            {
                error = exc.Message,
                exceptionType = exc.GetType().Name
            });
        }
        catch (Exception exc)
        {
            _logger.LogTrace($"Failed to create new document for user '{userId}': {exc.Message}.", exc.StackTrace);
            return StatusCode(500, "An internal server error occurred.");
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
            
            _logger.LogInformation($"User '{userId}' fetched document '{documentId}'");

            return _mapper.Map<DocumentResponseDto>(document);
        }
        catch (AccessDeniedException exc)
        {
            _logger.LogWarning($"Unauthorized attempt to FETCH document '{documentId}' by user '{userId}'.");
            return Forbid(exc.Message);
        }
        catch (DocumentNotFoundException exc)
        {
            _logger.LogError($"Failed to fetch document '{documentId}' that does not exist");
            return NotFound(exc.Message);
        }
        catch (Exception exc)
        {
            _logger.LogTrace($"Failed to fetch document '{documentId}': {exc.Message}.", exc.StackTrace);
            return StatusCode(500, "An internal server error occurred.");
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
            _logger.LogWarning(
                @$"Request with invalid query parameters
                Page = {queryParams.Page}, Size = {queryParams.Size}, Search = {queryParams.Search},
                SortBy = {queryParams.SortBy}, SortDirection = {queryParams.SortDirection}");
            return BadRequest(ModelState);
        }

        var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        try
        {
            PaginatedList<Document> documents = await _documentService.GetManyByUserIdAsync(
                new Guid(userId),
                _mapper.Map<DocumentQueryParams>(queryParams)
            );

            _logger.LogInformation($"User '{userId}' fetched their documents.");
            return _mapper.Map<PaginatedListResponseDto<DocumentListItemResponseDto>>(documents);
        }
        catch (InvalidSortingFiledException exc)
        {
            _logger.LogError(@$"Failed to fetch documents with invalid sorting field
                SortBy = {queryParams.SortBy}, SortDirection = {queryParams.SortDirection}");
            return BadRequest(exc.Message);
        }
        catch (Exception exc)
        {
            _logger.LogTrace($"Failed to fetch documents of user '{userId}': {exc.Message}.", exc.StackTrace);
            return StatusCode(500, "An internal server error occurred.");
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
            
            _logger.LogInformation($"User '{userId}' updated the metadata of the document '{documentId}'");
            return Ok();
        }
        catch (AccessDeniedException exc)
        {
            _logger.LogWarning($"Unauthorized attempt to UPDATE document '{documentId}' metadata by user '{userId}'.");
            return Forbid(exc.Message);
        }
        catch (DocumentNotFoundException exc)
        {
            _logger.LogError($"Failed to update document '{documentId}' metadata that does not exist");
            return NotFound(exc.Message);
        }
        catch (Exception exc)
        {
            _logger.LogTrace($"Failed to update document '{documentId}' metadata by user '{userId}': {exc.Message}.", exc.StackTrace);
            return StatusCode(500, "An internal server error occurred.");
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

            _logger.LogInformation($"User '{userId}' updated the content of the document '{documentId}'");
            return Ok();
        }
        catch (AccessDeniedException exc)
        {
            _logger.LogWarning($"Unauthorized attempt to UPDATE document '{documentId}' content by user '{userId}'.");
            return Forbid(exc.Message);
        }
        catch (DocumentNotFoundException exc)
        {
            _logger.LogError($"Failed to update document '{documentId}' content that does not exist");
            return NotFound(exc.Message);
        } 
        catch (DocumentException exc)
        {
            _logger.LogError($"Failed to update document '{documentId}': {exc.Message}.");
            return BadRequest(exc.Message);
        }
        catch (Exception exc)
        {
            _logger.LogTrace($"Failed to update document '{documentId}' content by user '{userId}': {exc.Message}.", exc.StackTrace);
            return StatusCode(500, "An internal server error occurred.");
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

            _logger.LogInformation($"User '{userId}' deleted document '{documentId}'");
            return Ok();
        }
        catch (AccessDeniedException exc)
        {
            _logger.LogWarning($"Unauthorized attempt to DELETE document '{documentId}' by user '{userId}'.");
            return Forbid(exc.Message);
        }
        catch (DocumentNotFoundException exc)
        {
            _logger.LogError($"Failed to delete document '{documentId}' that does not exist");
            return NotFound(exc.Message);
        }
        catch (Exception exc)
        {
            _logger.LogTrace($"Failed to delete document '{documentId}' by user '{userId}': {exc.Message}.", exc.StackTrace);
            return StatusCode(500, "An internal server error occurred.");
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