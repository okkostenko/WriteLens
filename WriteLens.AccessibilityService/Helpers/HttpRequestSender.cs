using System.Text.Json;
using WriteLens.Accessibility.Helpers;
using WriteLens.Accessibility.Models.ApplicationModels;

public class HttpRequestSender
{
    private readonly HttpClient _httpClient;
    public HttpRequestSender()
    {
        _httpClient = new HttpClient();
    }
    public async Task<HttpResponseMessage?> SendRequestAsync(HttpRequestConfig requestConfig)
    {
        var request = new HttpRequestMessage(requestConfig.Method, requestConfig.Uri);
        AddBody(requestConfig.Body, request);
        AddHeaders(requestConfig.Headers, request);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

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