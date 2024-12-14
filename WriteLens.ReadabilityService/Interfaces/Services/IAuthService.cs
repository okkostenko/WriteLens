namespace WriteLens.Readability.Interfaces.Services;

public interface IAuthService
{
    public Task Authorize(Guid documentId);
}