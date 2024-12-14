
namespace WriteLens.Accessibility.Models.ApplicationModels;

public class HttpRequestConfig
{
    public string Url { get; set; }
    public string Endpoint { get; set; }
    public HttpMethod Method { get; set; }
    public Dictionary<string, string>? Headers { get; set; }
    public JsonContent? Body { get; set; }

    public string Uri
    {
        get => $"{Url}{Endpoint}";
    }
}