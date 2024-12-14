using WriteLens.Core.Application.Commands.User;
using WriteLens.Core.Models.DomainModels.User;

namespace WriteLens.Core.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<User?> GetSingleByIdAsync(Guid userId);
    public Task<User?> GetSingleByEmailAsync(string email);
    public Task<User> AddSingleAsync(User user);
    public Task UpdateSingleByIdAsync(Guid userId, UpdateUserCommand updateUserCommand);
    public Task DeleteSingleByIdAsync(Guid userId);
}