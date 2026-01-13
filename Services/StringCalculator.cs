using System.Text.RegularExpressions;
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

        var parsedNumbers = ParseNumbers(numbers);

        ValidateNoNegatives(parsedNumbers);

        return parsedNumbers
            .Where(n => n <= _settings.MaxNumberValue)
            .Sum();
    }

    private List<int> ParseNumbers(string input)
    {
        var (delimiters, numbersToProcess) = ExtractDelimitersAndPayload(input);

        return numbersToProcess
            .Split(delimiters, StringSplitOptions.None)
            .Select(s => int.TryParse(s.Trim(), out var n) ? n : 0)
            .ToList();
    }

    private (string[] delimiters, string payload) ExtractDelimitersAndPayload(string input)
    {
        var delimiters = new List<string>(_settings.Delimiters ?? new[] { ",", "\n" });
        var payload = input;

        if (input.StartsWith("//"))
        {
            var parts = input.Split('\n', 2);
            if (parts.Length == 2)
            {
                var delimiterSpec = parts[0].Substring(2);
                
                if (delimiterSpec.StartsWith("[") && delimiterSpec.EndsWith("]"))
                {
                    var matches = Regex.Matches(delimiterSpec, @"\[(.*?)\]");
                    foreach (Match match in matches)
                    {
                        var customDelimiter = match.Groups[1].Value;
                        if (!string.IsNullOrEmpty(customDelimiter))
                        {
                            delimiters.Add(customDelimiter);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(delimiterSpec))
                {
                    delimiters.Add(delimiterSpec);
                }
                
                payload = parts[1];
            }
        }

        return (delimiters.ToArray(), payload);
    }

    private void ValidateNoNegatives(List<int> numbers)
    {
        var negatives = numbers.Where(n => n < 0).ToList();
        if (negatives.Any())
        {
            throw new ArgumentException($"Negatives not allowed: {string.Join(", ", negatives)}");
        }
    }
}
