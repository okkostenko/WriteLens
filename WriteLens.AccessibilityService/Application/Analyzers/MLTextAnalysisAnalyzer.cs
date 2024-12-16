using System.Text.Json;
using Microsoft.Extensions.Options;
using WriteLens.Accessibility.Models.ApplicationModels;
using WriteLens.Accessibility.Settings;
using WriteLens.Shared.Types;

namespace WriteLens.Accessibility.Application.Analyzers;

public class MLTextAnalysisAnalyzer
{
    private const int MAX_WAITING_ITERATIONS = 5;
    private readonly MLTASSettings _mltasSettings;
    private readonly HttpRequestSender _requestSender;
    private readonly ILogger<MLTextAnalysisAnalyzer> _logger;

    public MLTextAnalysisAnalyzer(MLTASSettings mltasSettings, ILoggerFactory loggerFactory)
    {
        _requestSender = new HttpRequestSender(loggerFactory);
        _mltasSettings = mltasSettings;
        _logger = loggerFactory.CreateLogger<MLTextAnalysisAnalyzer>();
    }

    public async Task<MLTextAnalysisResult> AnalyzeAsync(string text)
    {
        _logger.LogInformation($"Requesting analysis from ML Text Processing Service.");
        string processId = await RequestAnalysis(text);

        _logger.LogInformation($"Awaiting result from ML Text Processing Service.");
        await WaitUntilProcessed(processId);
        MLTextAnalysisResult result = await GetAnalysisResultAsync(processId);
        _logger.LogInformation($"Retrieved result from ML Text Processing Service.");
        return result;
    }

    private Dictionary<string, string> GetHeaders()
    {
        return new Dictionary<string, string>
        {
            {"X-API-Key", _mltasSettings.ApiKey},
            {"X-Audience", _mltasSettings.ValidAudience}
        };
    }

    private async Task<string> RequestAnalysis(string text)
    {
        HttpRequestConfig requestConfig = new() {
            Url = _mltasSettings.MLTextAnalysisServiceUrl,
            Endpoint = "/api/v1/accessibility/analyze",
            Method = HttpMethod.Post,
            Headers = GetHeaders(),
            Body = JsonContent.Create(new {
                Text = text
            })
        };

        HttpResponseMessage? response = await _requestSender.SendRequestAsync(requestConfig);
        var requestBody = await _requestSender.ParseResponse<MLTextAnalysisRequestAccepted>(response, JsonNamingPolicy.SnakeCaseLower);
        
        await Task.Delay(5000);
        return requestBody.TaskId;
    }

    private async Task WaitUntilProcessed(string taskId)
    {
        HttpRequestConfig requestConfig = new() {
            Url = _mltasSettings.MLTextAnalysisServiceUrl,
            Endpoint = $"/api/v1/accessibility/task/{taskId}/status",
            Method = HttpMethod.Get,
            Headers = GetHeaders()
        };

        string status = "PENDING";
        int iteration = 0;

        while (status != "FAILURE" && status != "SUCCESS")
        {
            HttpResponseMessage? response = await _requestSender.SendRequestAsync(requestConfig);
            var requestBody = await _requestSender.ParseResponse<MLTextAnalysisStatus>(response, JsonNamingPolicy.SnakeCaseLower);
            status = requestBody.Status;
            await Task.Delay(CalculateExponentialDelayMilliseconds(iteration));
            iteration ++;
            if (iteration == MAX_WAITING_ITERATIONS)
                throw new Exception("Max iterations exceeded"); // TODO: figure out some new error for it
        }
    }

    private int CalculateExponentialDelayMilliseconds(int iteration)
        => (int)(Math.Exp(iteration + 1) * 0.15 * 1000);

    private async Task<MLTextAnalysisResult> GetAnalysisResultAsync(string taskId)
    {
        HttpRequestConfig requestConfig = new() {
            Url = _mltasSettings.MLTextAnalysisServiceUrl,
            Endpoint = $"/api/v1/accessibility/task/{taskId}/result",
            Method = HttpMethod.Get,
            Headers = GetHeaders()
        };
        
        HttpResponseMessage? response = await _requestSender.SendRequestAsync(requestConfig);
        var responseBody = await _requestSender.ParseResponse<MLTextAnalysisResultResponse>(response, JsonNamingPolicy.SnakeCaseLower);
        return responseBody.Result;
    }
}