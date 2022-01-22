using Cep.App.Application.Catalog.Interfaces;
using Cep.App.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Cep.App.Infrastructure.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class
{
    private readonly IDistributedCache _cache;
    private readonly ISqlDataAccess _db;

    public GenericRepository(ISqlDataAccess db, IDistributedCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public Task<T> AddAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task RefreshCache()
    {
        throw new NotImplementedException();
    }
}
