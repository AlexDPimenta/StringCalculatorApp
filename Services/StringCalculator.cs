using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using StringCalculatorApp.Interfaces;
using StringCalculatorApp.Models;

namespace StringCalculatorApp.Services;

public class StringCalculator : ICalculator
{
    private readonly IOptionsMonitor<CalculatorSettings> _settingsMonitor;

    public StringCalculator(IOptionsMonitor<CalculatorSettings> settingsMonitor)
    {
        _settingsMonitor = settingsMonitor;
    }

    private CalculatorSettings Settings => _settingsMonitor.CurrentValue;

    public int Add(string numbers)
    {
        if (string.IsNullOrWhiteSpace(numbers))
        {
            Console.WriteLine("Result: 0 = 0");
            return 0;
        }

        var parsedNumbers = ParseNumbers(numbers);

        ValidateNoNegatives(parsedNumbers);

        var filteredNumbers = parsedNumbers
            .Where(n => n <= Settings.MaxNumberValue)
            .ToList();

        var result = Settings.Operation switch
        {
            '*' => filteredNumbers.Any() ? filteredNumbers.Aggregate(1, (acc, n) => acc * n) : 0,
            '-' => filteredNumbers.Any() ? filteredNumbers.Skip(1).Aggregate(filteredNumbers.First(), (acc, n) => acc - n) : 0,
            '/' => filteredNumbers.Any() ? filteredNumbers.Skip(1).Aggregate(filteredNumbers.First(), (acc, n) => n != 0 ? acc / n : throw new DivideByZeroException("Division by zero is not allowed.")) : 0,
            _ => filteredNumbers.Sum()
        };

        var detailedSum = filteredNumbers.Any() 
            ? string.Join(Settings.Operation.ToString(), filteredNumbers) 
            : "0";

        Console.WriteLine($"Result: {detailedSum} = {result}");

        return result;
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
        var delimiters = new List<string>(Settings.Delimiters ?? new[] { ",", "\n" });
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
