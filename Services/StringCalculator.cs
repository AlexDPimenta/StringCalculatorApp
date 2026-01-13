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
        if (string.IsNullOrWhiteSpace(numbers))
        {
            return 0;
        }

        var splitNumbers = numbers.Split(',');

        return splitNumbers
            .Select(s => int.TryParse(s.Trim(), out var n) ? n : 0)
            .Sum();
    }
}
