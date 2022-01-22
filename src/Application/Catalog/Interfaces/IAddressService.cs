using Cep.App.Application.Common.Interfaces;
using Cep.App.Core.Entities;

namespace Cep.App.Application.Catalog.Interfaces;

public interface IAddressService : IScopedService
{
    Task<Address> GetByZipCodeAsync(string zipcode);
}
