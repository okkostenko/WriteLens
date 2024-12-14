using WriteLens.Core.Application.Commands.User;
using WriteLens.Core.Models.DomainModels.User;

namespace WriteLens.Core.Interfaces.Services;

public interface IUserService
{
    Task<User> GetSingleByIdAsync(Guid userId);
    Task UpdateSingleByIdAsync(Guid userId, UpdateUserCommand updateUserCommand);
    Task DeleteSingleByIdAsync(Guid userId);
}
