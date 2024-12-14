using WriteLens.Auth.Models.DomainModels.User;

namespace WriteLens.Auth.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<User?> GetSingleByEmailAsync(string email);
    public Task<User> AddSingleAsync(User user);
}