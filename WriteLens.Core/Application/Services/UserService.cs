using WriteLens.Core.Application.Commands.User;
using WriteLens.Core.Exceptions;
using WriteLens.Core.Interfaces.Repositories;
using WriteLens.Core.Interfaces.Services;
using WriteLens.Core.Models.DomainModels.User;

namespace WriteLens.Core.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository PostgresDbUserRepository)
    {
        _userRepository = PostgresDbUserRepository;
    }

    public async Task<User> GetSingleByIdAsync(Guid userId)
    {
        User? user = await _userRepository.GetSingleByIdAsync(userId);
        validateUserExists(userId, user);
        return user;
    }

    private void validateUserExists(Guid userId, User? user)
    {
        if (user != null) return;

        throw new UserNotFoundException("id", userId);
    }

    public async Task UpdateSingleByIdAsync(Guid userId, UpdateUserCommand updateUserCommand)
    {
        await GetSingleByIdAsync(userId);
        await _userRepository.UpdateSingleByIdAsync(userId, updateUserCommand);
    }

    public async Task DeleteSingleByIdAsync(Guid userId)
    {
        await GetSingleByIdAsync(userId);
        await _userRepository.DeleteSingleByIdAsync(userId);
    }

}
