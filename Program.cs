using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StringCalculatorApp.Extensions;
using StringCalculatorApp.Interfaces;
using StringCalculatorApp.Models;

var builder = Host.CreateApplicationBuilder(args);

// Add services to the container
builder.Services.AddStringCalculator(builder.Configuration);

using IHost host = builder.Build();

var calculator = host.Services.GetRequiredService<ICalculator>();

Console.WriteLine("--- String Calculator Console ---");
Console.WriteLine("Enter the string of numbers (or press Ctrl+C to exit):");

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();

    if (input != null)
    {
        input = input.Replace("\\n", "\n");
    }

    try
    {
        calculator.Add(input ?? "");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Argument error: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
    }
}


