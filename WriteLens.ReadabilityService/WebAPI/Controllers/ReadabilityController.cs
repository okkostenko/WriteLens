using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using WriteLens.Shared.Exceptions.AnalysisExceptions;
using WriteLens.Shared.Exceptions.DocumentExceptions;
using WriteLens.Shared.Exceptions.TaskExceptions;
using WriteLens.Shared.Interfaces.Caching;
using WriteLens.Readability.Interfaces.Services;
using WriteLens.Readability.Models.DomainModels;
using WriteLens.Readability.WebAPI.DTOs.Requests;
using WriteLens.Readability.WebAPI.DTOs.Responses;
using WriteLens.Shared.Exceptions;

namespace WriteLens.Readability.WebAPI.Controllers;

[ApiController]
[Route("api/v1/readability")]
public class ReadabilityController : ControllerBase
{
    private readonly IPublishEndpoint _publisher;
    private readonly ITaskCache _taskCache;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;
    private readonly ILogger<ReadabilityController> _logger;
    
    public ReadabilityController(
        IPublishEndpoint publisher,
        ITaskCache taskCache,
        IMapper mapper,
        IAuthService authService,
        ILogger<ReadabilityController> logger)
    {
        _publisher = publisher;
        _taskCache = taskCache;
        _mapper = mapper;
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Create a request to analyze the document's readability.
    /// </summary>
    /// <param name="documentId">The ID of the document.</param>
    /// <returns>Returns the ID of the analysis task to track.</returns>
    /// <response code="200">Returns the ID of the analysis task.</response>
    /// <response code="400">
    /// Nothing to analyze error:
    /// no updates were made to the document content.
    /// </response>
    /// <response code="401">User is not authorized.</response>
    [HttpPost("document/{documentId}/analyze")]
    [ProducesResponseType(typeof(RequestAcceptedResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<RequestAcceptedResponseDto>> Analyze(Guid documentId)
    {
        try
        {
            await _authService.Authorize(documentId);
            TaskModel? task = await _taskCache.GetTaskAsync<TaskModel>(documentId);
            if (task is null || task.Status != "Pending")
            {
                await _taskCache.RegisterTaskAsync(documentId);
                await _publisher.Publish(new ReadabilityAnalysisRequestDto
                {
                    TaskId = documentId,
                    DocumentId = documentId
                });
            }

            _logger.LogInformation(
                $"Readability analysis of document '{documentId}' accepted successfully.");
            return Accepted(new RequestAcceptedResponseDto
            {
                TaskId = documentId
            });
        }
        catch (Exception exc) when (
            exc is UnauthorizedAccessException ||
            exc is AccessDeniedException
        )
        {
            _logger.LogWarning(
                @$"Unauthorized attempt to analyze readabilit of the document '{documentId}': {exc.Message}");
            return Unauthorized(exc.Message);
        }
        catch (Exception exc) when (
            exc is UnsupportedDocumentTypeException ||
            exc is NothigToAnalyzeException)
        {
            _logger.LogError(
                $"Failed to analyze readability of the document '{documentId}': {exc.Message}");
            return BadRequest(exc.Message);
        }
        catch (Exception exc)
        {
            _logger.LogError(
                $"Failed to analyze readability of the document '{documentId}': {exc.Message}");
            return StatusCode(500, "Internal server error occured");
        }
    }

    /// <summary>
    /// Get the status of the analysis task.
    /// </summary>
    /// <param name="taskId">The id of the task.</param>
    /// <returns>Returns the status of the task.</returns>
    /// <response code="200">Returns the status of the analysis task.</response>
    /// <response code="400">Validation error.</response>
    /// <response code="401">User is not authorized.</response>
    /// <response code="404">Task with provided ID does not exist.</response>
    [HttpGet("task/{taskId}/status")]
    [ProducesResponseType(typeof(TaskStatusResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<TaskStatusResponseDto>> GetTaskStatus(Guid taskId)
    {
        try
        {
            await _authService.Authorize(taskId);
            TaskModel task = await _taskCache.GetTaskAsync<TaskModel>(taskId);

            _logger.LogInformation($"Task '{taskId}' status retreived successfully");
            return _mapper.Map<TaskStatusResponseDto>(task);
        }
        catch (Exception exc) when (
            exc is UnauthorizedAccessException ||
            exc is AccessDeniedException
        )
        {
            _logger.LogWarning(
                @$"Unauthorized attempt to retreive the status of task '{taskId}': {exc.Message}");
            return Unauthorized(exc.Message);
        }
        catch (TaskNotFoundException exc)
        {
            _logger.LogError(
                $"Failed to retreive the status of task '{taskId}': {exc.Message}");
            return NotFound(exc.Message);
        }
        catch (Exception exc)
        {
            _logger.LogError(
                $"Failed to retreive the status of task '{taskId}': {exc.Message}");
            return StatusCode(500, "Internal server error occured");
        }
    }

    /// <summary>
    /// Get the result of the analysis.
    /// </summary>
    /// <param name="taskId">The id of the task.</param>
    /// <returns>Returns the result of the analysis.</returns>
    /// <response code="200">Returns the result of the analysis.</response>
    /// <response code="400">The analysis is still in progress.</response>
    /// <response code="401">User is not authorized.</response>
    /// <response code="404">Task with provided ID does not exist.</response>
    /// <response code="500">Task with provided ID does not exist.</response>
    [HttpGet("task/{taskId}/result")]
    [ProducesResponseType(typeof(ReadabilityAnalysisResultResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<ReadabilityAnalysisResultResponseDto>> GetTaskResult(Guid taskId)
    {
        try
        {
            await _authService.Authorize(taskId);
            TaskModel task = await _taskCache.GetTaskAsync<TaskModel>(taskId);
            if (task.Status == "Pending" || task.Status == "Processing")
            {
                return BadRequest($"Task with id '{taskId}' is still {task.Status}");
            }
            if (task.Status == "Failed")
            {
                _logger.LogError(
                    $"Failed to retrieve task '{taskId}' analysis result: {task.ErrorMessage}");
                if (task.StatusCode == 400)
                    return BadRequest(task.ErrorMessage);
                else
                    return StatusCode(500, task.ErrorMessage);
            }

            _logger.LogInformation(
                $"Task '{taskId}' analysis result retreived successfully.");
            return _mapper.Map<ReadabilityAnalysisResultResponseDto>(task.Result);
        }
        catch (Exception exc) when (
            exc is UnauthorizedAccessException ||
            exc is AccessDeniedException
        )
        {
            _logger.LogWarning(
                @$"Unauthorized attempt to retreive the analysis result of task '{taskId}': {exc.Message}");
            return Unauthorized(exc.Message);
        }
        catch (TaskNotFoundException exc)
        {
            _logger.LogError(
                $"Failed to retreive the analysis result of task '{taskId}': {exc.Message}");
            return NotFound(exc.Message);
        }
        catch (Exception exc)
        {
            _logger.LogError(
                $"Failed to retreive the analysis result of task '{taskId}': {exc.Message}");
            return StatusCode(500, "Internal server error occured");
        }
    }
}