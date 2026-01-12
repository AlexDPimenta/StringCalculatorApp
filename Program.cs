using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StringCalculatorApp.Extensions;
using StringCalculatorApp.Interfaces;

var builder = Host.CreateApplicationBuilder(args);

// Add services to the container
builder.Services.AddStringCalculator(builder.Configuration);

using IHost host = builder.Build();

var calculator = host.Services.GetRequiredService<ICalculator>();

Console.WriteLine("--- String Calculator Console ---");
Console.WriteLine("Digite a string de números (ou pressione Ctrl+C para sair):");

while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();

    try
    {
        var result = calculator.Add(input ?? "");
        Console.WriteLine($"Resultado: {result}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Erro de argumento: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro inesperado: {ex.Message}");
    }
}


