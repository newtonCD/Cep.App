using Cep.App.Application.Catalog.Interfaces;
using Cep.App.Application.Common.Interfaces;
using Cep.App.Core.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Cep.App.Infrastructure.Persistence.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly ISqlDataAccess _db;
    private readonly IDistributedCache _cache;

    public AddressRepository(ISqlDataAccess db, IDistributedCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<Address> GetByZipCodeAsync(string zipcode)
    {
        if (string.IsNullOrWhiteSpace(zipcode))
        {
            throw new ArgumentException($"'{nameof(zipcode)}' cannot be null or whitespace.", nameof(zipcode));
        }

        Address address = null;

        byte[] objectFromCache = await _cache.GetAsync(zipcode);

        if (objectFromCache != null)
        {
            string jsonToDeserialize = System.Text.Encoding.UTF8.GetString(objectFromCache);
            address = JsonSerializer.Deserialize<Address>(jsonToDeserialize, new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        });

            if (address != null) return address;
        }

        IEnumerable<Address> addressResult = await _db.GetAsync<Address, dynamic>("dbo.Cep_GetAddress", new { Zipcode = zipcode });

        if (addressResult != null)
        {
            address = addressResult.FirstOrDefault();
            byte[] objectToCache = JsonSerializer.SerializeToUtf8Bytes(address, new JsonSerializerOptions
                                    {
                                        WriteIndented = true,
                                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                                    });

            await _cache.SetAsync(zipcode, objectToCache, new DistributedCacheEntryOptions()
                                                            .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                                                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(180)));
        }

        return address;
    }

    public Task<Address> AddAsync(Address entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Address entity)
    {
        throw new NotImplementedException();
    }

    public Task<Address> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Address entity)
    {
        throw new NotImplementedException();
    }
}