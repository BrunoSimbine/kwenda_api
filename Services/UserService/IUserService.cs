using bilhete24.Models;
using bilhete24.Dtos;

namespace bilhete24.Services.UserService;

public interface IUserService
{
    Task<User> Create(UserDto userDto);
    Task<Auth> GetLogin(AuthDto authDto);
    
}