using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cep.App.Infrastructure.Common.Extensions;

public static class OptionsExtensions
{
    public static T GetOptions<T>(this IServiceCollection services, string sectionName)
    where T : new()
    {
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var section = configuration.GetSection(sectionName);
        var options = new T();
        section.Bind(options);

        return options;
    }
}
