using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WriteLens.Auth.Infrastructure.Data;
using WriteLens.Auth.Infrastructure.Data;
using WriteLens.Auth.Infrastructure.Data.Entities;
using WriteLens.Auth.Helpers;
using WriteLens.Auth.Interfaces.Repositories;
using WriteLens.Auth.Models.DomainModels.User;

namespace WriteLens.Auth.Infrastructure.Repositories;

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

    public async Task<User?> GetSingleByEmailAsync(string email)
    {
        UserEntity? user = await _context.Users
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();

        return _mapper.Map<User>(user);
    }
}