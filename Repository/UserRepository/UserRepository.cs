using bilhete24.Models;
using bilhete24.Data;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;


namespace bilhete24.Repository.UserRepository;

public class UserRepository : IUserRepository
{

    private readonly DataContext _context;
    private readonly IHttpContextAccessor _accessor;

    public UserRepository(DataContext context, IHttpContextAccessor accessor)
    {
        _accessor = accessor;
        _context = context;
    }

    public Guid GetId()
    {
        var id = _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Sid);
        return Guid.Parse(id);
    }

    public async Task<User> Create(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> Update(User user)
    {
        _context.Users.Attach(user);

        _context.Entry(user).State = EntityState.Modified;

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpgradeRole(User user)
    {
        _context.Users.Attach(user);

        user.Role = "admin";
        _context.Entry(user).Property(u => u.Role).IsModified = true;

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> Verify(User user)
    {
        _context.Users.Attach(user);

        user.IsVerified = true;
        _context.Entry(user).Property(u => u.IsVerified).IsModified = true;

        await _context.SaveChangesAsync();
        return user;

    }

    public async Task<bool> IsUserVerified()
    {
        var user = await Get();
        if(user.IsVerified == true)
        {
            return true;
            
        } else {

            return false;
        }
    }

    public async Task<bool> IsUserDeleted()
    {
        var user = await Get();
        if(user.IsDeleted == true)
        {
            return true;
            
        } else {

            return false;
        }
    }

    public async Task<User> Recover(User user)
    {
        _context.Users.Attach(user);

        user.DateDeleted = null;
        _context.Entry(user).Property(u => u.DateDeleted).IsModified = true;

        await _context.SaveChangesAsync();
        return user;

    }


    public async Task<User> Delete(User user)
    {
        _context.Users.Attach(user);

        user.DateDeleted = DateTime.Now;
        _context.Entry(user).Property(u => u.DateDeleted).IsModified = true;

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> Get(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task<User> Get()
    {
        var id = GetId();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task<List<User>> GetActives()
    {
        var users = await _context.Users.Where(a => a.DateDeleted == null).ToListAsync();
        return users;
    }


    public async Task<User> GetByPhone(string phone)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Phone == phone);
        return user;
    }

    public async Task<User> GetByEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task<List<User>> GetAll()
    {
        var users = await _context.Users.ToListAsync();
        return users;
    }

    public async Task<bool> ExistsAny(string phone)
    {
        var exists = await _context.Users.AnyAsync(u => u.Phone == phone);
        return exists;
    }

    public async Task<bool> ExistsAnyEmail(string email)
    {
        var exists = await _context.Users.AnyAsync(u => u.Email == email);
        return exists;
    }
}
