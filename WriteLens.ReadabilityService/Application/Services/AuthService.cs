using System.Net;
using Microsoft.Extensions.Options;
using WriteLens.Readability.Interfaces.Services;
using WriteLens.Readability.Settings;
using WriteLens.Shared.Exceptions;

namespace WriteLens.Readability.Application.Services;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _context;
    private readonly HttpClient _httpClient;
    private readonly ApplicationSettings _appSettings;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IHttpContextAccessor context,
        IOptions<ApplicationSettings> appSettings,
        ILogger<AuthService> logger)
    {
        _context = context;
        _httpClient = new HttpClient();
        _appSettings = appSettings.Value;
        _logger = logger;
    }
    public async Task Authorize(Guid documentId)
    {
        try
        {
            var url = $"{_appSettings.CoreUrl}/api/v1/documents/{documentId}/check-access";
            
            var authorizationHeader = GetAuthorizationHeader();

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", authorizationHeader);

            _logger.LogInformation($"Sending request to url '{url}'");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            _logger.LogInformation(
                $"User got access to analyze document '{documentId}' successfully");
        }
        catch (HttpRequestException exc)
        {
            if (exc.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException(exc.Message);
            
            else if (exc.StatusCode == HttpStatusCode.Forbidden)
                throw new AccessDeniedException(exc.Message);
            else if (exc.StatusCode == HttpStatusCode.NotFound)
                throw new AccessDeniedException($"Document");
            else
                throw new Exception("Could not authorize user");
        }
    }

    private string GetAuthorizationHeader()
    {
        var authorizationHeader = _context.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            throw new UnauthorizedAccessException("Authorization Token is Required");
        }

        return authorizationHeader;
    }
}