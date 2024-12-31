using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using bilhete24.Models;
using bilhete24.Dtos;
using bilhete24.Exceptions;
using bilhete24.Filters;
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
    [ServiceFilter(typeof(RequireActiveAuthFilter))]
    public async Task<ActionResult<List<Auth>>> Get()
    {
        return Ok(await _authService.GetByUser());
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login(AuthDto authDto)
    {
        try{
            var user = await _authService.GetLogin(authDto);
            return Ok(user);

        } catch (UserOrPassInvalidException ex){
            return BadRequest(new {
                type = "authentication",
                error = "Your credencials are invalid!",
                solution = "Use a correct login and passwrord."
            });
        } 

    }

    [HttpPost("login/oauth")]
    public async Task<ActionResult<User>> OAuth2(OAuthDto oauthDto)
    {
        try {
            var user = await _authService.GetOAuth(oauthDto);
            return Ok(user);

        } catch (UserOrPassInvalidException ex){
            return BadRequest(new {
                type = "authentication",
                error = "Your credencials are invalid!",
                solution = "Make sure you are using the correct email"
            });
        }
    }

    [HttpPost("otp")]
    [ServiceFilter(typeof(RequireActiveAuthFilter))]
    public ActionResult<User> Otp(OtpDto otpDto)
    {
        return Ok(new User());
    }

    [HttpPut("refresh"), Authorize]
    [ServiceFilter(typeof(RequireActiveAuthFilter))]
    public ActionResult<User> Geta()
    {
        return Ok("Bruno");
    }

    [HttpDelete("logout"), Authorize]
    [ServiceFilter(typeof(RequireActiveAuthFilter))]
    public async Task<ActionResult<User>> Logout()
    {
        return Ok(await _authService.Logout());
    }
}
