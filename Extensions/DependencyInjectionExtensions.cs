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
        // Bind settings from appsettings.json
        services.Configure<CalculatorSettings>(configuration.GetSection("CalculatorSettings"));

        // Register services
        services.AddSingleton<ICalculator, StringCalculator>();

        return services;
    }
}

