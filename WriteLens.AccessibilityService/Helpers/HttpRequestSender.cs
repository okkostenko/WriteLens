using System.Text.Json;
using WriteLens.Accessibility.Helpers;
using WriteLens.Accessibility.Models.ApplicationModels;

public class HttpRequestSender
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpRequestSender> _logger;
    public HttpRequestSender(ILoggerFactory loggerFactory)
    {
        _httpClient = new HttpClient();
        _logger = loggerFactory.CreateLogger<HttpRequestSender>();
    }
    public async Task<HttpResponseMessage?> SendRequestAsync(HttpRequestConfig requestConfig)
    {
        _logger.LogInformation($"Sending {requestConfig.Method} request to url {requestConfig.Uri}...");
        var request = new HttpRequestMessage(requestConfig.Method, requestConfig.Uri);
        AddBody(requestConfig.Body, request);
        AddHeaders(requestConfig.Headers, request);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        _logger.LogInformation($"Request with method {requestConfig.Method} sent successfully to url {requestConfig.Uri}.");
        return response;
    }

    private void AddBody(JsonContent? body, in HttpRequestMessage request)
    {
        if (body is null)
            return;

        request.Content = body;
    }

    private void AddHeaders(Dictionary<string, string>? headers, in HttpRequestMessage request)
    {
        if (headers is null)
            return;

        foreach (KeyValuePair<string, string> entry in headers)
        {
            request.Headers.Add(entry.Key, entry.Value);
        }
    }

    public async Task<T?> ParseResponse<T>(HttpResponseMessage response, JsonNamingPolicy namingPolicy)
    {
        var responseBody = await response.Content.ReadAsStringAsync();
        
        return JsonSerializer.Deserialize<T>(
            responseBody,
            new JsonSerializerOptions{
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = namingPolicy
            }
        );
    }
}