using bilhete24.Repository;
using bilhete24.Models;

namespace bilhete24.Repository.UserRepository;

public interface IUserRepository : IBaseRepository<User>
{
	Task<bool> ExistsAny(string phone);
	Task<User> GetByPhone(string phone);
	Guid GetId();
}
