using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WriteLens.Core.Application.Commands.User;
using WriteLens.Core.Infrastructure.Data;
using WriteLens.Core.Infrastructure.Data.PostgresDb;
using WriteLens.Core.Infrastructure.Data.PostgresDb.Entities;
using WriteLens.Core.Helpers;
using WriteLens.Core.Interfaces.Repositories;
using WriteLens.Core.Models.DomainModels.User;

namespace WriteLens.Core.Infrastructure.Repositories;

public class PostgresDbUserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PostgresDbUserRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<User> AddSingleAsync(User user)
    {
        UserEntity dbUser = _mapper.Map<UserEntity>(user);
        var createdUser = await _context.AddAsync(dbUser);
        await _context.SaveChangesAsync();
        return _mapper.Map<User>(createdUser.Entity);
    }

    public async Task DeleteSingleByIdAsync(Guid userId)
    {
        UserEntity? user = await getSingleEntityByIdAsync(userId);
        
        if (user is null) return;

        _context.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetSingleByEmailAsync(string email)
    {
        UserEntity? user = await _context.Users
            .Where(u => u.Email == email)
            .Include(u => u.Documents)
            .FirstOrDefaultAsync();

        return _mapper.Map<User>(user);
    }

    public async Task<User?> GetSingleByIdAsync(Guid userId)
    {
        UserEntity? user = await getSingleEntityByIdAsync(userId);
        
        return _mapper.Map<User>(user);
    }

    private async Task<UserEntity?> getSingleEntityByIdAsync(Guid userId)
    {
        UserEntity? user = await _context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.Documents)
            .FirstOrDefaultAsync();
        
        return user;
    }

    public async Task UpdateSingleByIdAsync(Guid userId, UpdateUserCommand updateUserCommand)
    {
        UserEntity? user = await getSingleEntityByIdAsync(userId);

        if (user is null) return;

        if (updateUserCommand.Email != null) user.Email = updateUserCommand.Email;
        if (updateUserCommand.Name != null) user.Name = updateUserCommand.Name;

        await _context.SaveChangesAsync();
    }
}