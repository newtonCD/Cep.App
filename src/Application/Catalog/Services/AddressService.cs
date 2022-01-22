using Cep.App.Application.Catalog.Interfaces;
using Cep.App.Core.Entities;

namespace Cep.App.Application.Catalog.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;

    public AddressService(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<Address> GetByZipCodeAsync(string zipcode)
    {
        return await _addressRepository.GetByZipCodeAsync(zipcode);
    }
}