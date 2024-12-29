using bilhete24.Models;

namespace bilhete24.Repository;

public interface IBaseRepository<T> where T : BaseEntity
{
	Task<T> Create(T entity);
	Task<T> Update(T entity);
	Task<T> Delete(T entity);

	Task<T> Get(Guid id);
	Task<List<T>> GetAll();
}