using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using WriteLens.Core.Application.Commands.User;
using WriteLens.Core.Exceptions;
using WriteLens.Core.Interfaces.Services;
using WriteLens.Core.Models.DomainModels.User;
using WriteLens.Core.WebAPI.DTOs.User.Requests;
using WriteLens.Core.WebAPI.DTOs.User.Responses;

namespace WriteLens.Core.WebAPI.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UserController: ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserResponseDto>> GetUserById()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        try
        {
            User user = await _userService.GetSingleByIdAsync(new Guid(userId));
            return _mapper.Map<UserResponseDto>(user);
        }
        catch (UserNotFoundException exc)
        {
            return NotFound(exc.Message);
        }
    }

    [HttpGet("{userId}")]
    [Authorize]
    public async Task<ActionResult<UserResponseDto>> GetUserById(Guid userId)
    {
        try
        {
            User user = await _userService.GetSingleByIdAsync(userId);
            return _mapper.Map<UserResponseDto>(user);
        }
        catch (UserNotFoundException exc)
        {
            return NotFound(exc.Message);
        }
    }

    [HttpPatch("update")]
    [Authorize]
    public async Task<IActionResult> UpdateUserById(UpdateUserRequestDto updateUserDto)
    {
        var userId = new Guid(User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value);
        try
        {
            vaildateUserAllowedToPerformOperation(userId, nameof(UpdateUserById));
            await _userService.UpdateSingleByIdAsync(userId, _mapper.Map<UpdateUserCommand>(updateUserDto));
            return Ok();
        }
        catch (UnauthorizedAccessException exc)
        {
            return Unauthorized(exc.Message);
        }
        catch (AccessDeniedException exc)
        {
            return StatusCode(403, new {Message = exc.Message});
        }
        catch (UserNotFoundException exc)
        {
            return NotFound(exc.Message);
        }
    }

    [HttpDelete("delete")]
    [Authorize]
    public async Task<IActionResult> DeleteUserById()
    {
        var userId = new Guid(User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value);
        try
        {
            vaildateUserAllowedToPerformOperation(userId, nameof(DeleteUserById));
            await _userService.DeleteSingleByIdAsync(userId);
            return Ok();
        }
        catch (UnauthorizedAccessException exc)
        {
            return Unauthorized(exc.Message);
        }
        catch (AccessDeniedException exc)
        {
            return Forbid(exc.Message);
        }
        catch (UserNotFoundException exc)
        {
            return NotFound(exc.Message);
        }
    }

    private void vaildateUserAllowedToPerformOperation(Guid userId, string operation)
    {
        var authorizedUserId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

        if (authorizedUserId is null)
        {
            throw new UnauthorizedAccessException($"User is not authorized");
        }

        if (authorizedUserId != userId.ToString())
        {
            throw new AccessDeniedException(
                @$"User with id {authorizedUserId} doesn't have access
                to perform operation {operation} on user with id {userId}"
            );
        }
    }
}