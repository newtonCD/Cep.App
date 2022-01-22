using Cep.App.Core.Entities;

namespace Cep.App.Application.Catalog.Interfaces;

public interface IAddressRepository : IGenericRepository<Address>
{
    Task<Address> GetByZipCodeAsync(string zipcode);
}