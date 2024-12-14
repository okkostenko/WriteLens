using AutoMapper;
using MassTransit;
using MassTransit.Middleware;
using MassTransit.SagaStateMachine;
using Microsoft.AspNetCore.Http.HttpResults;
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
    
    public ReadabilityController(IPublishEndpoint publisher, ITaskCache taskCache, IMapper mapper, IAuthService authService)
    {
        _publisher = publisher;
        _taskCache = taskCache;
        _mapper = mapper;
        _authService = authService;
    }

    [HttpPost("document/{documentId}/analyze")]
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
            return Unauthorized(exc.Message);
        }
        catch (Exception exc) when (
            exc is UnsupportedDocumentTypeException ||
            exc is NothigToAnalyzeException)
        {
            return BadRequest(exc.Message);
        }
    }

    [HttpGet("task/{taskId}/status")]
    public async Task<ActionResult<TaskStatusResponseDto>> GetTaskStatus(Guid taskId)
    {
        try
        {
            await _authService.Authorize(taskId);
            TaskModel task = await _taskCache.GetTaskAsync<TaskModel>(taskId);
            return _mapper.Map<TaskStatusResponseDto>(task);
        }
        catch (Exception exc) when (
            exc is UnauthorizedAccessException ||
            exc is AccessDeniedException
        )
        {
            return Unauthorized(exc.Message);
        }
        catch (TaskNotFoundException exc)
        {
            return NotFound(exc.Message);
        }
    }

    [HttpGet("task/{taskId}/result")]
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
                if (task.StatusCode == 400)
                    return BadRequest(task.ErrorMessage);
                else
                    return StatusCode(500, task.ErrorMessage);
            }
            return _mapper.Map<ReadabilityAnalysisResultResponseDto>(task.Result);
        }
        catch (Exception exc) when (
            exc is UnauthorizedAccessException ||
            exc is AccessDeniedException
        )
        {
            return Unauthorized(exc.Message);
        }
        catch (TaskNotFoundException exc)
        {
            return NotFound(exc.Message);
        }
    }
}