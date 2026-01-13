using Moq;
using Microsoft.Extensions.Options;
using StringCalculatorApp.Models;
using StringCalculatorApp.Services;
using Xunit;

namespace StringCalculatorApp.Tests.Services;

public class StringCalculatorTests
{
    private readonly StringCalculator _calculator;
    private readonly Mock<IOptionsMonitor<CalculatorSettings>> _mockSettingsMonitor;

    public StringCalculatorTests()
    {
        _mockSettingsMonitor = new Mock<IOptionsMonitor<CalculatorSettings>>();
        _mockSettingsMonitor.Setup(s => s.CurrentValue).Returns(new CalculatorSettings());
        _calculator = new StringCalculator(_mockSettingsMonitor.Object);
    }

    [Theory]
    [InlineData(null, 0)]
    [InlineData("", 0)]
    [InlineData("  ", 0)]
    public void Add_EmptyOrNullInput_ReturnsZero(string? input, int expected)
    {
        var result = _calculator.Add(input!);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Add_SingleNumber_ReturnsNumber()
    {
        var result = _calculator.Add("20");
        Assert.Equal(20, result);
    }

    [Fact]
    public void Add_TwoNumbers_ReturnsSum()
    {
        var result = _calculator.Add("1,500");
        Assert.Equal(501, result);
    }

    [Fact]
    public void Add_TwoNumbersWithNegative_ThrowsArgumentException()
    {
        Action action = () => _calculator.Add("4,-3");
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Contains("Negatives not allowed: -3", exception.Message);
    }

    [Fact]
    public void Add_MultipleNegativeNumbers_ThrowsArgumentExceptionWithAllNegatives()
    {
        Action action = () => _calculator.Add("1,-2,3,-4");
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Contains("-2, -4", exception.Message);
    }

    [Fact]
    public void Add_InvalidNumber_ConvertsToZero()
    {
        var result = _calculator.Add("5,tytyt");
        Assert.Equal(5, result);
    }

    [Fact]
    public void Add_MultipleNumbers_ReturnsSum()
    {
        var result = _calculator.Add("1,2,3,4,5,6,7,8,9,10,11,12");
        Assert.Equal(78, result);
    }

    [Fact]
    public void Add_NumbersGreaterThanMaxNumberValue_AreIgnored()
    {
        var result = _calculator.Add("2,1001,6");
        Assert.Equal(8, result);
    }

    [Fact]
    public void Add_NumbersEqualToMaxNumberValue_AreIncluded()
    {
        var result = _calculator.Add("2,1000,6");
        Assert.Equal(1008, result);
    }

    [Fact]
    public void Add_CustomSingleCharacterDelimiter_ReturnsSum()
    {
        var result = _calculator.Add("//#\n2#5");
        Assert.Equal(7, result);
    }

    [Fact]
    public void Add_CustomMultiCharacterDelimiter_ReturnsSum()
    {
        var result = _calculator.Add("//[***]\n11***22***33");
        Assert.Equal(66, result);
    }

    [Fact]
    public void Add_MultipleCustomDelimiters_ReturnsSum()
    {
        var result = _calculator.Add("//[*][!!][r9r]\n11r9r22*hh*33!!44");
        Assert.Equal(110, result);
    }

    [Fact]
    public void Add_CustomCommaDelimiterWithInvalidValues_ReturnsSumOfValidValues()
    {
        var result = _calculator.Add("//,\n2,ff,100");
        Assert.Equal(102, result);
    }

    [Fact]
    public void Add_SubtractionOperation_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new CalculatorSettings { Operation = '-', Delimiters = new[] { "," } };
        _mockSettingsMonitor.Setup(s => s.CurrentValue).Returns(settings);
        
        // Act
        var result = _calculator.Add("10,2,3");
        
        // Assert
        Assert.Equal(5, result); // 10 - 2 - 3 = 5
    }

    [Fact]
    public void Add_DivisionOperation_ReturnsCorrectResult()
    {
        // Arrange
        var settings = new CalculatorSettings { Operation = '/', Delimiters = new[] { "," } };
        _mockSettingsMonitor.Setup(s => s.CurrentValue).Returns(settings);
        
        // Act
        var result = _calculator.Add("20,2,2");
        
        // Assert
        Assert.Equal(5, result); // 20 / 2 / 2 = 5
    }

    [Fact]
    public void Add_DivisionByZero_ThrowsException()
    {
        // Arrange
        var settings = new CalculatorSettings { Operation = '/', Delimiters = new[] { "," } };
        _mockSettingsMonitor.Setup(s => s.CurrentValue).Returns(settings);
        
        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => _calculator.Add("10,0"));
    }
}
