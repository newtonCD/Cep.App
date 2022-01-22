using Cep.App.Application.Common.Validators;
using Cep.App.Core.DTOs.Catalog;
using FluentValidation;

namespace Cep.App.Application.Catalog.Validators;

public class UpdateAddressRequestValidator : CustomValidator<UpdateAddressRequest>
{
    public UpdateAddressRequestValidator()
    {
        RuleFor(p => p.ZipCode).MaximumLength(8).MinimumLength(8).NotEmpty().NotNull();
        RuleFor(p => p.TypeId).GreaterThanOrEqualTo(0).NotEmpty().NotNull();
        RuleFor(p => p.Street).MaximumLength(100).NotEmpty().NotNull();
        RuleFor(p => p.Neighborhood).MaximumLength(100).NotEmpty().NotNull();
        RuleFor(p => p.City).MaximumLength(100).NotEmpty().NotNull();
        RuleFor(p => p.State).MaximumLength(2).MinimumLength(2).NotEmpty().NotNull();
        RuleFor(p => p.CountryIsoCode).MaximumLength(3).MinimumLength(3).NotEmpty().NotNull();
    }
}