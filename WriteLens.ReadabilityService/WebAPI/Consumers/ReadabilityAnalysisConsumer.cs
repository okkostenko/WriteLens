using AutoMapper;
using MassTransit;
using WriteLens.Readability.Application.Services;
using WriteLens.Shared.Exceptions.AnalysisExceptions;
using WriteLens.Shared.Exceptions.DocumentExceptions;
using WriteLens.Shared.Interfaces.Caching;
using WriteLens.Readability.Interfaces.Services;
using WriteLens.Readability.Models.DomainModels;
using WriteLens.Readability.WebAPI.DTOs.Requests;
using WriteLens.Readability.WebAPI.DTOs.Responses;
using WriteLens.Shared.Models;

namespace WriteLens.Readability.WebAPI.Consumers;

public class  ReadabilityAnalysisConsumer : IConsumer<ReadabilityAnalysisRequestDto>
{
    private readonly IReadabilityService _readabilityService;
    private readonly ITaskCache _taskCache;
    private readonly ILogger _logger;

    public ReadabilityAnalysisConsumer(IReadabilityService readabilityService, ITaskCache taskCache, ILogger<ReadabilityAnalysisConsumer> logger)
    {
        _readabilityService = readabilityService;
        _taskCache = taskCache;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ReadabilityAnalysisRequestDto> context)
    {
        try
        {
            await UpdateTaskStatusToProcessing(context.Message.TaskId);

            var documentId = context.Message.DocumentId;
            DocumentContentDocumentScore analysisResult = await _readabilityService.AnalyzeAsync(documentId);
            
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

    private async Task UpdateTaskStatusToSuccess(Guid taskId, DocumentContentDocumentScore analysisResult)
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