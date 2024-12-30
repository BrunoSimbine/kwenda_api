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

    public async Task<User> CreateByOAuth(UserOAuthDto userOAuthDto)
    {
        if(!await _userRepository.ExistsAnyEmail(userOAuthDto.Email))
        {
            _authRepository.CreatePasswordHash(userOAuthDto.OAuthToken, out byte[] passwordHash, out byte[] passwordSalt);

            var user = await _userRepository.Create(new User
            {
                FirstName = userOAuthDto.FirstName,
                LastName = userOAuthDto.LastName,
                Email = userOAuthDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt =  passwordSalt
            });

            return user;
        } else {
            throw new PhoneAlreadyExistsException("Email are already used!");
            return new User();
        }
    }

    public async Task<Auth> GetOauth(OAuthDto oauthDto)
    {
        if (await _userRepository.ExistsAnyEmail(oauthDto.Email))
        {
            var user = await _userRepository.GetByEmail(oauthDto.Email);
            var token = _authRepository.CreateToken(user);

            var auth = await _authRepository.Create(new Auth
            {
                User = user,
                Device = oauthDto.Device,
                Token = token
            });

            return auth;
        } else {
            return new Auth();
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

    public async Task<List<User>> GetAll()
    {
        return await _userRepository.GetAll();
    }

    public async Task<User> Get()
    {
        return await _userRepository.Get();
    }

    public async Task<User> UpgradeRole()
    {
        var user = await _userRepository.Get();
        return await _userRepository.UpgradeRole(user);
    }

    public async Task<User> Verify()
    {
        var user = await _userRepository.Get();
        return await _userRepository.Verify(user);
    }


    public async Task<User> Recover()
    {
        var user = await _userRepository.Get();
        return await _userRepository.Recover(user);
    }

    public async Task<List<User>> GetActives()
    {
        return await _userRepository.GetActives();
    }




    public async Task<User> Delete()
    {
        var user = await _userRepository.Get();
        return await _userRepository.Delete(user);
    }

    public async Task<User> Delete(Guid id)
    {
        var user = await _userRepository.Get(id);
        return await _userRepository.Delete(user);
    }
}