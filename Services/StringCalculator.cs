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

        var delimiters = new List<string>(_settings.Delimiters ?? new[] { ",", "\n" });
        var numbersToProcess = numbers;

        if (numbers.StartsWith("//"))
        {
            var parts = numbers.Split('\n', 2);
            if (parts.Length == 2)
            {
                var delimiterSpec = parts[0].Substring(2);
                
                if (delimiterSpec.StartsWith("[") && delimiterSpec.EndsWith("]"))
                {
                    // Case for multiple delimiters or single multi-char delimiter: //[{del1}][{del2}]\n
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
                else
                {
                    // Case for single character delimiters: //{delimiter}\n
                    if (!string.IsNullOrEmpty(delimiterSpec))
                    {
                        delimiters.Add(delimiterSpec);
                    }
                }
                
                numbersToProcess = parts[1];
            }
        }

        var splitNumbers = numbersToProcess.Split(delimiters.ToArray(), StringSplitOptions.None);

        var parsedNumbers = splitNumbers
            .Select(s => int.TryParse(s.Trim(), out var n) ? n : 0)
            .ToList();

        var negatives = parsedNumbers.Where(n => n < 0).ToList();
        if (negatives.Any())
        {
            throw new ArgumentException($"Negatives not allowed: {string.Join(", ", negatives)}");
        }

        return parsedNumbers
            .Where(n => n <= _settings.MaxNumberValue)
            .Sum();
    }
}
