using System.Net;
using Microsoft.Extensions.Options;
using WriteLens.Accessibility.Interfaces.Services;
using WriteLens.Accessibility.Settings;
using WriteLens.Shared.Exceptions;

namespace WriteLens.Accessibility.Application.Services;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _context;
    private readonly HttpClient _httpClient;
    private readonly ApplicationSettings _appSettings;

    public AuthService(IHttpContextAccessor context, IOptions<ApplicationSettings> appSettings)
    {
        _context = context;
        _httpClient = new HttpClient();
        _appSettings = appSettings.Value;
    }
    public async Task Authorize(Guid documentId)
    {
        try
        {
            var url = $"{_appSettings.CoreUrl}/api/v1/documents/{documentId}/check-access";
            var authorizationHeader = GetAuthorizationHeader();

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", authorizationHeader);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
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