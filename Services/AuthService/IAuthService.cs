using bilhete24.Models;
using bilhete24.Dtos;

namespace bilhete24.Services.AuthService;

public interface IAuthService
{
    Task<List<Auth>> GetByUser();
    Task<Auth> GetLogin(AuthDto authDto);
    Task<Auth> GetOAuth(OAuthDto authDto);
    Task<Auth> Logout();
}