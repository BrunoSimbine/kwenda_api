using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using bilhete24.Models;
using bilhete24.Dtos;
using bilhete24.Exceptions;
using bilhete24.Filters;
using bilhete24.Services.UserService;

namespace bilhete24.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("get"), Authorize]
    [ServiceFilter(typeof(RequireVerifiedUserFilter))]
    [ServiceFilter(typeof(RequireActiveUserFilter))]
    [ServiceFilter(typeof(RequireActiveAuthFilter))]
    public async Task<ActionResult<User>> Get()
    {
        var user = await _userService.Get();
        return Ok(user);
    }

    [HttpGet("get/actives"), Authorize]
    [ServiceFilter(typeof(RequireVerifiedUserFilter))]
    [ServiceFilter(typeof(RequireActiveUserFilter))]
    [ServiceFilter(typeof(RequireActiveAuthFilter))]
    public async Task<ActionResult<List<User>>> GetActives()
    {
        var users = await _userService.GetActives();
        return Ok(users);
    }

    [HttpGet("get/all"), Authorize]
    [ServiceFilter(typeof(RequireVerifiedUserFilter))]
    [ServiceFilter(typeof(RequireActiveUserFilter))]
    [ServiceFilter(typeof(RequireActiveAuthFilter))]
    public async Task<ActionResult<User>> GetAll()
    {
        var users = await _userService.GetAll();
        return Ok(users);
    }


    [HttpPost("create")]
    public async Task<ActionResult<User>> Create(UserDto userDto)
    {
        try{
            var user = await _userService.Create(userDto);
            return Ok(user);

        } catch (PhoneAlreadyExistsException ex){
            return Conflict(new {
                type = "error",
                code = 409,
                message = "This contact are already used!"
            });
        } 
    }

    [HttpPost("create/oauth")]
    public async Task<ActionResult<User>> CreateByOAuth(UserOAuthDto userOAuthDto)
    {
        try{
            var user = await _userService.CreateByOAuth(userOAuthDto);
            return Ok(user);

        } catch (PhoneAlreadyExistsException ex){
            return Conflict(new {
                type = "error",
                code = 409,
                message = "This contact are already used!"
            });
        } 
    }

    [HttpPut("upgrade/role"), Authorize]
    [ServiceFilter(typeof(RequireVerifiedUserFilter))]
    [ServiceFilter(typeof(RequireActiveUserFilter))]
    [ServiceFilter(typeof(RequireActiveAuthFilter))]
    public async Task<ActionResult<User>> Upgrade()
    {
        var user = await _userService.UpgradeRole();
        return Ok(user);
    }


    [HttpPut("verify"), Authorize]
    [ServiceFilter(typeof(RequireActiveAuthFilter))]
    public async Task<ActionResult<User>> Verify()
    {
        var user = await _userService.Verify();
        return Ok(user);
    }

    [HttpPut("recover"), Authorize]
    [ServiceFilter(typeof(RequireVerifiedUserFilter))]
    [ServiceFilter(typeof(RequireActiveAuthFilter))]
    public async Task<ActionResult<User>> Recover()
    {
        var user = await _userService.Recover();
        return Ok(user);
    }


    [HttpDelete("delete"), Authorize]
    [ServiceFilter(typeof(RequireVerifiedUserFilter))]
    [ServiceFilter(typeof(RequireActiveUserFilter))]
    [ServiceFilter(typeof(RequireActiveAuthFilter))]
    public async Task<ActionResult<User>> Delete()
    {
        var user = await _userService.Delete();
        return Ok(user);
    }

    [HttpDelete("suspend/{id}"), Authorize]
    [ServiceFilter(typeof(RequireVerifiedUserFilter))]
    [ServiceFilter(typeof(RequireActiveUserFilter))]
    [ServiceFilter(typeof(RequireActiveAuthFilter))]
    public async Task<ActionResult<User>> GetAll(Guid id)
    {
        var user = await _userService.Delete(id);
        return Ok(user);
    }
}
