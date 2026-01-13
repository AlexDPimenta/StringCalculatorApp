# String Calculator App

A robust and configurable String Calculator built with .NET 9, following clean code principles and solid architecture patterns.

## 🚀 Features

- **Multiple Delimiters**: Supports commas (`,`), newlines (`\n`), and custom delimiters.
- **Custom Delimiters**:
  - Single character: `//{delimiter}\n{numbers}` (e.g., `//#\n2#5`)
  - Multi-character: `//[{delimiter}]\n{numbers}` (e.g., `//[***]\n11***22`)
  - Multiple delimiters: `//[{del1}][{del2}]\n{numbers}` (e.g., `//[*][!!]\n1*2!!3`)
- **Configurable Math Operations**: Supports Addition (`+`), Multiplication (`*`), Subtraction (`-`), and Division (`/`) via configuration.
- **Negative Number Prevention**: Optional validation to deny negative numbers, listing all found negatives in the error message.
- **Upper Bound Filtering**: Automatically ignores numbers greater than a configured maximum value (default: 1000).
- **Detailed Output**: Prints the step-by-step operation in the console (e.g., `Result: 2+4+6 = 12`).

## 🏗️ Architecture

The solution is organized using a modular approach to ensure testability and maintainability:

- **StringCalculatorApp (Main Project)**:
  - `Interfaces/`: Contains `ICalculator`, defining the service contract.
  - `Services/`: Core logic implementation in `StringCalculator.cs`.
  - `Models/`: Configuration models like `CalculatorSettings.cs`.
  - `Extensions/`: Dependency Injection setup.
  - `Program.cs`: Entry point with a console loop and DI container initialization.
- **StringCalculatorApp.Tests**:
  - Unit tests using **xUnit** and **Moq** to verify all requirements and edge cases.

## 🛠️ Technologies

- **Language**: C# 13
- **Framework**: .NET 9.0
- **Configuration**: Microsoft.Extensions.Options (with `IOptionsMonitor` for real-time updates)
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection
- **Testing**: xUnit & Moq

## ⚙️ Configuration

You can customize the calculator behavior in `appsettings.json`:

```json
{
  "CalculatorSettings": {
    "Delimiters": [ ",", "\n" ],
    "DenyNegatives": true,
    "MaxNumberValue": 1000,
    "Operation": "+"
  }
}
```

- `Delimiters`: Default separators used when no custom delimiter is provided.
- `DenyNegatives`: If `true`, throws an exception if negative numbers are present.
- `MaxNumberValue`: Numbers higher than this value will be ignored in calculations.
- `Operation`: The math operation to perform (`+`, `*`, `-`, `/`).

## 🏃 How to Run

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Running the Console Application
1. Open a terminal in the project root.
2. Run the command:
   ```bash
   dotnet run
   ```
3. Type your numbers. **Note**: When using custom delimiters in the console, type `\n` literally (e.g., `//#\n2#5`), and the app will handle the conversion.

### Running Tests
To execute the unit test suite:
```bash
dotnet test
```

## 📝 Usage Example (Console)
```text
--- String Calculator Console ---
Enter the string of numbers (or press Ctrl+C to exit):
> 1,2,3
Result: 1+2+3 = 6

> //[*][!!]\n10*20!!5
Result: 10+20+5 = 35
```

