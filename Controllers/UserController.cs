using Microsoft.AspNetCore.Mvc;
using bilhete24.Models;
using bilhete24.Dtos;
using bilhete24.Exceptions;
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

}
