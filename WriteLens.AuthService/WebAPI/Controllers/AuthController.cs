using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WriteLens.Auth.Models.Commands;
using WriteLens.Auth.Interfaces.Services;
using WriteLens.Auth.WebAPI.DTOs.Requests;
using WriteLens.Auth.WebAPI.DTOs.Responses;

namespace WriteLens.Auth.WebAPI.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController: ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AuthController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticateResponseDto>> Login(LoginRequestDto loginDto)
    {
        try
        {
            string authToken = await _authService.AuthenticateAsync(_mapper.Map<LoginUserCommand>(loginDto));
            return new AuthenticateResponseDto(authToken);
        }
        catch (UnauthorizedAccessException exc)
        {
            return Unauthorized(exc.Message);
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticateResponseDto>> Register(RegisterRequestDto registerDto)
    {
        try
        {
            string authToken = await _authService.RegisterAsync(_mapper.Map<RegisterUserCommand>(registerDto));
            return new AuthenticateResponseDto(authToken);
        }
        catch (UnauthorizedAccessException exc)
        {
            return Unauthorized(exc.Message);
        }
    }
}