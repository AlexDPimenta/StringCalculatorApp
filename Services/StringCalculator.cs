using Microsoft.Extensions.Options;
using StringCalculatorApp.Interfaces;
using StringCalculatorApp.Models;

namespace StringCalculatorApp.Services;

public class StringCalculator : ICalculator
{
    private readonly CalculatorSettings _settings;

    public StringCalculator(IOptions<CalculatorSettings> settings)
    {
        _settings = settings.Value;
    }

    public int Add(string numbers)
    {
        throw new NotImplementedException();
    }
}
