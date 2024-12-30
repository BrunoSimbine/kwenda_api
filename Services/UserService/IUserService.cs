using bilhete24.Models;
using bilhete24.Dtos;

namespace bilhete24.Services.UserService;

public interface IUserService
{
    Task<User> Create(UserDto userDto);
    Task<User> CreateByOAuth(UserOAuthDto userOAuthDto);
    Task<Auth> GetLogin(AuthDto authDto);
    Task<Auth> GetOauth(OAuthDto oauthDto);
    Task<List<User>> GetAll();
    Task<List<User>> GetActives();
    Task<User> Get();
    Task<User> UpgradeRole();
    Task<User> Recover();
    Task<User> Verify();
    Task<User> Delete();
    Task<User> Delete(Guid id);
}