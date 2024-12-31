using bilhete24.Repository;
using bilhete24.Models;

namespace bilhete24.Repository.AuthRepository;

public interface IAuthRepository : IBaseRepository<Auth>
{
	Task<Auth> Get(Guid id);
	Task<List<Auth>> GetActives(Guid userId);
	Task<bool> IsDeleted();
	Task<List<Auth>> GetAll();
	Task<Auth> GetByToken(string token);
	string GetCurrentToken();
	string GetCurrentDevice();
	string GetIpAddress();
	void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
	bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
	string CreateToken(User user);
}
