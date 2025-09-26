# MDLSoft.StringParsers

A powerful .NET library for parsing and writing structured text data using fixed-width and separator-based formats.

## Features

- **Fixed-width string parsing**: Parse and write data with fixed column positions and lengths
- **Separator-based parsing**: Parse and write data using custom separators (CSV, TSV, etc.)
- **Type conversion**: Automatic conversion between string and target property types
- **Custom validators**: Add validation logic for parsed values  
- **Custom converters**: Transform data during parsing and writing
- **Built-in formatters**: Common padding and formatting operations
- **Strong typing**: Fluent API with compile-time type checking

## Quick Start

### Fixed-Width Parsing

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

public class PersonFixedParser : FixedStringParser<Person>
{
    public PersonFixedParser()
    {
        Define(p => p.Name, 0, 10).WithWriteSpaceRightPadder();
        Define(p => p.Age, 10, 3).WithReadConverter<int>(s => int.Parse(s))
                                  .WithWriteZeroLeftPadder();
        ValidateDefinitions();
    }
}

// Usage
var parser = new PersonFixedParser();
var person = parser.Parse("John      025"); // Name: "John", Age: 25
var text = parser.Write(new Person { Name = "Ann", Age = 7 }); // "Ann       007"
```

### Separator-Based Parsing

```csharp
public class CsvPersonParser : SeparatorStringParser<Person>
{
    public CsvPersonParser() : base(',')
    {
        Define(p => p.Name, 0);
        Define(p => p.Age, 1).WithReadConverter<int>(s => int.Parse(s));
    }
}

// Usage
var parser = new CsvPersonParser();
var person = parser.Parse("John,25"); // Name: "John", Age: 25
```

## Requirements

- .NET Standard 2.0+ 
- .NET Framework 4.0+

## License

Licensed under the MIT License. See LICENSE file for details.