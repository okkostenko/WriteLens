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
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        IMapper mapper,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _mapper = mapper;
        _logger = logger;
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

            _logger.LogInformation($"User with email '{loginDto}' logged in successfully");
            return new AuthenticateResponseDto(authToken);
        }
        catch (UnauthorizedAccessException exc)
        {
            _logger.LogWarning(
                $"Unauthorized attempt to login by user with email '{loginDto.Email}': {exc.Message}");
            return Unauthorized(exc.Message);
        }
        catch (Exception exc)
        {
            _logger.LogTrace($"Failed to login user with email '{loginDto.Email}': {exc.Message}", exc.StackTrace);
            return StatusCode(500, "Intermal server error occured");
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

            _logger.LogInformation($"User with email '{registerDto.Email}' registered successfully");
            return new AuthenticateResponseDto(authToken);
        }
        catch (UnauthorizedAccessException exc)
        {
            _logger.LogWarning(
                $"Unauthorized attempt to register by user with email '{registerDto.Email}': {exc.Message}");
            return Unauthorized(exc.Message);
        }
        catch (Exception exc)
        {
            _logger.LogTrace(
                $"Failed to register user with email '{registerDto.Email}': {exc.Message}", exc.StackTrace);
            return StatusCode(500, "Intermal server error occured");
        }
    }
}