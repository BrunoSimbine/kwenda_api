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

    public async Task<Auth> Logout()
    {
        var token = _authRepository.GetCurrentToken();
        var auth = await _authRepository.GetByToken(token);
        return await _authRepository.Delete(auth);
    }
}