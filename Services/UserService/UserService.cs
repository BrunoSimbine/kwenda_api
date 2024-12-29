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


namespace bilhete24.Services.UserService;

public class UserService : IUserService
{

    private readonly IUserRepository _userRepository;
    private readonly IAuthRepository _authRepository;

    public UserService(IUserRepository userRepository, IAuthRepository authRepository)
    {
        _authRepository = authRepository;
        _userRepository = userRepository;
    }

    public async Task<User> Create(UserDto userDto)
    {
        if(!await _userRepository.ExistsAny(userDto.Phone))
        {
            _authRepository.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = await _userRepository.Create(new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Phone = userDto.Phone,
                PasswordHash = passwordHash,
                PasswordSalt =  passwordSalt
            });

            return user;
        } else {
            throw new PhoneAlreadyExistsException("Contact are already used!");
            return new User();
        }
    }

    public async Task<Auth> GetLogin(AuthDto authDto)
    {
        if (await _userRepository.ExistsAny(authDto.Phone))
        {
            var user = await _userRepository.GetByPhone(authDto.Phone);
            var token = _authRepository.CreateToken(user);

            var auth = await _authRepository.Create(new Auth
            {
                User = user,
                Device = authDto.Device,
                Token = token
            });

            return auth;
        } else {
            return new Auth();
        }
    }
}