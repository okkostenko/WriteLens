using AutoMapper;
using MassTransit;
using WriteLens.Accessibility.Application.Services;
using WriteLens.Accessibility.Interfaces.Services;
using WriteLens.Accessibility.Models.ApplicationModels;
using WriteLens.Accessibility.Models.DomainModels;
using WriteLens.Accessibility.WebAPI.DTOs.Requests;
using WriteLens.Accessibility.WebAPI.DTOs.Responses;
using WriteLens.Shared.Exceptions.AnalysisExceptions;
using WriteLens.Shared.Exceptions.DocumentExceptions;
using WriteLens.Shared.Interfaces.Caching;
using WriteLens.Shared.Models;

namespace WriteLens.Accessibility.WebAPI.Consumers;

public class  AccessibilityAnalysisConsumer : IConsumer<AccessibilityAnalysisRequestDto>
{
    private readonly IAccessibilityService _accessibilityService;
    private readonly ITaskCache _taskCache;
    private readonly ILogger _logger;

    public AccessibilityAnalysisConsumer(IAccessibilityService accessibilityService, ITaskCache taskCache, ILogger<AccessibilityAnalysisConsumer> logger)
    {
        _accessibilityService = accessibilityService;
        _taskCache = taskCache;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AccessibilityAnalysisRequestDto> context)
    {
        try
        {
            await UpdateTaskStatusToProcessing(context.Message.TaskId);

            var documentId = context.Message.DocumentId;
            AccessibilityAnalysisResult analysisResult = await _accessibilityService.AnalyzeAsync(documentId);
            
            await UpdateTaskStatusToSuccess(context.Message.TaskId, analysisResult);
        }
        catch (Exception exc) when (
            exc is UnsupportedDocumentTypeException ||
            exc is NothigToAnalyzeException)
        {
            await UpdateTaskStatusToFailed(context.Message.TaskId, 400, exc.Message);
        }
        catch (Exception exc)
        {
            await UpdateTaskStatusToFailed(context.Message.TaskId, 500, exc.Message);
            _logger.LogError(exc, "An exception occured while analyzing document");
        }
    }

    private async Task UpdateTaskStatusToProcessing(Guid taskId)
    {
        await _taskCache.UpdateTaskAsync(taskId, new TaskModel
        {
            TaskId = taskId,
            Status = "Processing"
        });
    }

    private async Task UpdateTaskStatusToSuccess(Guid taskId, AccessibilityAnalysisResult analysisResult)
    {
        await _taskCache.UpdateTaskAsync(taskId, new TaskModel
        {
            TaskId = taskId,
            Status = "Success",
            StatusCode = 200,
            ErrorMessage = null,
            Result = analysisResult
        });
    }

    private async Task UpdateTaskStatusToFailed(Guid taskId, int statusCode, string errorMessage)
    {
        await _taskCache.UpdateTaskAsync(taskId, new TaskModel
        {
            TaskId = taskId,
            Status = "Failed",
            StatusCode = statusCode,
            ErrorMessage = errorMessage,
            Result = null
        });
    }
}