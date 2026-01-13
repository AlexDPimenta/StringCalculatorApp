using Moq;
using Microsoft.Extensions.Options;
using StringCalculatorApp.Models;
using StringCalculatorApp.Services;
using Xunit;

namespace StringCalculatorApp.Tests.Services;

public class StringCalculatorTests
{
    private readonly StringCalculator _calculator;
    private readonly Mock<IOptions<CalculatorSettings>> _mockSettings;

    public StringCalculatorTests()
    {
        _mockSettings = new Mock<IOptions<CalculatorSettings>>();
        _mockSettings.Setup(s => s.Value).Returns(new CalculatorSettings());
        _calculator = new StringCalculator(_mockSettings.Object);
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
        var result = _calculator.Add("1,5000");
        Assert.Equal(5001, result);
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
    public void Add_NewlineDelimiter_ReturnsSum()
    {
        var result = _calculator.Add("1\n2,3");
        Assert.Equal(6, result);
    }
}
