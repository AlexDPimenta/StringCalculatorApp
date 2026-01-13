using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StringCalculatorApp.Interfaces;
using StringCalculatorApp.Models;
using StringCalculatorApp.Services;

namespace StringCalculatorApp.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddStringCalculator(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("CalculatorSettings");
        
        // Bind settings from appsettings.json
        services.Configure<CalculatorSettings>(section);

        // Register services
        services.AddSingleton<ICalculator, StringCalculator>();

        return services;
    }
}

