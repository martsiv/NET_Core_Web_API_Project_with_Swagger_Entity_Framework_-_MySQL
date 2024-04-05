using Ardalis.Specification;

namespace ApplicationCore.Interfaces
{
	public interface IRepository<TEntity> where TEntity : class
	{
		IEnumerable<TEntity> GetAll();
		TEntity? GetByID(object id);
		void Insert(TEntity entity);
		void Delete(object id);
		void Delete(TEntity entityToDelete);
		void Update(TEntity entityToUpdate);
		TEntity? GetItemBySpec(ISpecification<TEntity> specification);
		IEnumerable<TEntity> GetListBySpec(ISpecification<TEntity> specification);
		void Save();
		Task<IEnumerable<TEntity>> GetAllAsync();
		Task<TEntity?> GetByIDAsync(object id);
		Task InsertAsync(TEntity entity);
		Task DeleteAsync(object id);
		Task<TEntity?> GetItemBySpecAsync(ISpecification<TEntity> specification);
		Task<IEnumerable<TEntity>> GetListBySpecAsync(ISpecification<TEntity> specification);
		Task SaveAsync();
	}
}
