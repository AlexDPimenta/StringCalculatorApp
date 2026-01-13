namespace StringCalculatorApp.Models;

public class CalculatorSettings
{
    public string[] Delimiters { get; set; } = new[] { ",", "\n" };
    public bool DenyNegatives { get; set; } = true;
    public int MaxNumberValue { get; set; } = 1000;
    public char Operation { get; set; } = '+';
}
