using WriteLens.Auth.Helpers;
using WriteLens.Auth.Models.Commands;
using WriteLens.Auth.Interfaces.Repositories;
using WriteLens.Auth.Interfaces.Services;
using WriteLens.Auth.Models.DomainModels.User;
using WriteLens.Auth.Settings;
using Microsoft.Extensions.Options;

namespace WriteLens.Auth.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IUserRepository PostgresDbUserRepository, IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = PostgresDbUserRepository;
            _jwtTokenGenerator = new JwtTokenGenerator(jwtSettings.Value);
        }

        public async Task<string> AuthenticateAsync(LoginUserCommand loginData)
        {
            var user = await _userRepository.GetSingleByEmailAsync(loginData.Email);
            
            validateUserExists(user);
            validatePasswordMatches(loginData.Password, user.PasswordHash);

            return _jwtTokenGenerator.GenerateJwtToken(user);
        }

        private void validateUserExists(User? user)
        {
            if (user != null) return;
            
            throw new UnauthorizedAccessException(
                $"User with provided email does not exist");
        }

        private void validatePasswordMatches(string password, string storedPasswordHash)
        {
            bool isPasswordMatch = PasswordEncoder.VerifyPassword(
                password,
                storedPasswordHash);

            if (! isPasswordMatch)
            {
                throw new UnauthorizedAccessException(
                    "Wrong Password");
            }
        }

        public async Task<string> RegisterAsync(RegisterUserCommand registerData)
        {
            User? dbUser = await _userRepository.GetSingleByEmailAsync(registerData.Email);
            validateUserDoesNotExist(dbUser);

            registerData.Password = PasswordEncoder.HashPassword(registerData.Password);

            var user = await _userRepository.AddSingleAsync(
                new User(registerData)
            );

            return  _jwtTokenGenerator.GenerateJwtToken(user);
        }

        private void validateUserDoesNotExist(User? user)
        {
            if (user is null) return;

            throw new UnauthorizedAccessException(
                $"User with provided email already exist");
        }

        public async Task<string> RefreshTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

    }
}