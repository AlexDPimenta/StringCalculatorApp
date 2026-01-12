namespace StringCalculatorApp.Models;

public class CalculatorSettings
{
    public string DefaultDelimiter { get; set; } = "\n";
    public bool DenyNegatives { get; set; } = true;
    public int UpperBound { get; set; } = 1000;
    public char Operation { get; set; } = '+';
}

