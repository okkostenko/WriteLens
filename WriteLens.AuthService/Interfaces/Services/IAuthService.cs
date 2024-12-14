using WriteLens.Auth.Models.Commands;
using WriteLens.Auth.Models.DomainModels.User;

namespace WriteLens.Auth.Interfaces.Services;

public interface IAuthService
{
    Task<string> AuthenticateAsync(LoginUserCommand loginData);
    Task<string> RegisterAsync(RegisterUserCommand registerData);
    Task<string> RefreshTokenAsync(string token);
}