using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using bilhete24.Models;
using bilhete24.Dtos;
using bilhete24.Services.UserService;
using bilhete24.Services.AuthService;


namespace bilhete24.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class AuthController : ControllerBase
{

    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public AuthController(IUserService userService, IAuthService authService)
    {
        _authService = authService;
        _userService = userService;
    }
    [HttpGet("get"), Authorize]
    public async Task<ActionResult<List<Auth>>> Get()
    {
        return Ok(await _authService.GetByUser());
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login(AuthDto authDto)
    {
        return Ok(await _userService.GetLogin(authDto));
    }

    [HttpPost("otp")]
    public ActionResult<User> Otp(OtpDto otpDto)
    {
        return Ok(new User());
    }

    [HttpPut("refresh"), Authorize]
    public ActionResult<User> Geta()
    {
        return Ok("Bruno");
    }

    [HttpDelete("logout"), Authorize]
    public async Task<ActionResult<User>> Logout()
    {
        return Ok(await _authService.Logout());
    }
}
