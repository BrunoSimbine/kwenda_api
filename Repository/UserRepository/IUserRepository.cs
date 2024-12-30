using bilhete24.Repository;
using bilhete24.Models;

namespace bilhete24.Repository.UserRepository;

public interface IUserRepository : IBaseRepository<User>
{
	Task<bool> ExistsAny(string phone);
	Task<User> GetByPhone(string phone);
	Task<User> UpgradeRole(User user);
	Task<User> Verify(User user);
	Task<User> Recover(User user);
	Task<bool> IsUserVerified();
	Task<bool> IsUserDeleted();
	Task<User> GetByEmail(string email);
	Task<bool> ExistsAnyEmail(string email);
	
	Guid GetId();
	Task<List<User>> GetActives();
	Task<User> Get();
}
