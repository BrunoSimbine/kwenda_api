using bilhete24.Models;
using bilhete24.Exceptions;
using bilhete24.Data;
using bilhete24.Dtos;
using bilhete24.Repository.UserRepository;
using bilhete24.Repository.AuthRepository;


using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;


namespace bilhete24.Services.AuthService;

public class AuthService : IAuthService
{

    private readonly IUserRepository _userRepository;
    private readonly IAuthRepository _authRepository;

    public AuthService(IUserRepository userRepository, IAuthRepository authRepository)
    {
        _authRepository = authRepository;
        _userRepository = userRepository;
    }

    public async Task<List<Auth>> GetByUser()
    {
        var userId = _userRepository.GetId();
        var auth = await _authRepository.GetActives(userId);
        return auth;
    }

    public async Task<Auth> GetLogin(AuthDto authDto)
    {
        bool userExists = await _userRepository.ExistsAny(authDto.Phone);
        if (userExists) {
            var user = await _userRepository.GetByPhone(authDto.Phone);
            var device = _authRepository.GetCurrentDevice();
            var auth = new Auth();
            if (_authRepository.VerifyPasswordHash(authDto.Password, user.PasswordHash, user.PasswordSalt)){
                var token = _authRepository.CreateToken(user);
                auth.IpAddress = _authRepository.GetIpAddress();
                auth.Device = device;
                auth.Token = token;
                auth.UserId = user.Id;
                await _authRepository.Create(auth);

                return auth;
            } else {
                throw new UserOrPassInvalidException("User or password invalid");
                return auth;
            }
        } else {
            throw new UserOrPassInvalidException("User or password invalid");
            return new Auth();
        }
    }

    public async Task<Auth> GetOAuth(OAuthDto authDto)
    {
        bool userExists = await _userRepository.ExistsAnyEmail(authDto.Email);
        if (userExists) {
            var user = await _userRepository.GetByEmail(authDto.Email);
            var device = _authRepository.GetCurrentDevice();
            var auth = new Auth();
            if (_authRepository.VerifyPasswordHash(authDto.OAuthToken, user.PasswordHash, user.PasswordSalt))
            {
                var token = _authRepository.CreateToken(user);
                auth.Device = device;
                auth.IpAddress = _authRepository.GetIpAddress();
                auth.Token = token;
                auth.UserId = user.Id;
                await _authRepository.Create(auth);

                return auth;
            } else {
                throw new UserOrPassInvalidException("User or password invalid");
                return auth;
            }
        } else {
            throw new UserOrPassInvalidException("User or password invalid");
            return new Auth();
        }
    }


    public async Task<Auth> Logout()
    {
        var token = _authRepository.GetCurrentToken();
        var auth = await _authRepository.GetByToken(token);
        return await _authRepository.Delete(auth);
    }
}