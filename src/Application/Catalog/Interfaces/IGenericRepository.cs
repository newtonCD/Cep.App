namespace Cep.App.Application.Catalog.Interfaces;

public interface IGenericRepository<T>
    where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}