namespace WriteLens.Accessibility.Interfaces.Services;

public interface IAuthService
{
    public Task Authorize(Guid documentId);
}