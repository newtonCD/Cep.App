﻿namespace Cep.App.Core.DTOs.Catalog;

public class UpdateAddressRequest : IMustBeValid
{
    public string ZipCode { get; set; }
    public int TypeId { get; set; }
    public string Street { get; set; }
    public string Neighborhood { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string CountryIsoCode { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
}
