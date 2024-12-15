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

    /// <summary>
    /// Login user by email and password.
    /// </summary>
    /// <param name="loginDto">Login data.</param>
    /// <returns>JWT Token.</returns>
    /// <response code="200">Returns JWT token for the further authentication.</response>
    /// <response code="400">Payload validation error.</response>
    /// <response code="401">
    /// Authorization error: provided email doesn't exist or the password is wrong.
    /// </response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthenticateResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
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

    /// <summary>
    /// Register new user.
    /// </summary>
    /// <param name="registerDto">Registration data.</param>
    /// <returns>JWT Token.</returns>
    /// <response code="200">Returns JWT token for the further authentication.</response>
    /// <response code="400">Payload validation error.</response>
    /// <response code="401">
    /// Authorization error: user with provided email already exists.
    /// </response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthenticateResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
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