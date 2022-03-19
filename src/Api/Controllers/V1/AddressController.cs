using Cep.App.Application.Catalog.Interfaces;
using Cep.App.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Cep.App.Host.Controllers.V1;

public class AddressController : BaseController
{
    private readonly ILogger<AddressController> _logger;
    private readonly IAddressService _addressService;

    public AddressController(ILogger<AddressController> logger, IAddressService addressService)
    {
        _logger = logger;
        _addressService = addressService;
    }

    [HttpGet("{zipcode}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Address))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByZipCodeAsync([Required(AllowEmptyStrings = false),
        StringLength(8, ErrorMessage = "{0} deve conter exatos {1} caracteres", MinimumLength = 8),
        RegularExpression("^[0-9]*$", ErrorMessage = "{0} deve conter apenas caracteres numéricos")]
        string zipcode)
    {
        try
        {
            var results = await _addressService.GetByZipCodeAsync(zipcode);
            if (results == null) return NotFound();
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro interno");
            return Problem(ex.Message);
        }
    }
}
