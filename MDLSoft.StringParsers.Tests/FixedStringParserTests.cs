using System;
using Xunit;
using MDLSoft.StringParsers;

namespace MDLSoft.StringParsers.Tests;

public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

public class PersonFixedParser : FixedStringParser<Person>
{
    public PersonFixedParser()
    {
        Define(p => p.Name, 0, 10).WithWriteSpaceRightPadder();
        Define(p => p.Age, 10, 3).WithReadConverter<int>(s => int.Parse(s)).WithWriteZeroLeftPadder();
        ValidateDefinitions();
    }
}

public class FixedStringParserTests
{
    [Fact]
    public void Parse_Should_Parse_FixedString()
    {
        var parser = new PersonFixedParser();

        var text = "John      025"; // Name: 'John' padded to 10, Age: '025'

        var person = parser.Parse(text);

        Assert.Equal("John", person.Name.TrimEnd());
        Assert.Equal(25, person.Age);
    }

    [Fact]
    public void Write_Should_Write_FixedString()
    {
        var parser = new PersonFixedParser();

        var person = new Person { Name = "Ann", Age = 7 };
        var text = parser.Write(person);

        Assert.Equal("Ann       007", text);
    }

    [Fact]
    public void Parse_Should_Throw_ArgumentNullException_When_Text_Is_Null()
    {
        var parser = new PersonFixedParser();

        Assert.Throws<ArgumentNullException>(() => parser.Parse(null!));
    }

    [Fact]
    public void Write_Should_Throw_ArgumentNullException_When_Data_Is_Null()
    {
        var parser = new PersonFixedParser();

        Assert.Throws<ArgumentNullException>(() => parser.Write(null!));
    }
}

public class PersonCsvParser : SeparatorStringParser<Person>
{
    public PersonCsvParser() : base(',')
    {
        Define(p => p.Name, 0);
        Define(p => p.Age, 1).WithReadConverter<int>(s => int.Parse(s));
    }
}

public class SeparatorStringParserTests
{
    [Fact]
    public void Parse_Should_Parse_CsvString()
    {
        var parser = new PersonCsvParser();

        var text = "John,25";

        var person = parser.Parse(text);

        Assert.Equal("John", person.Name);
        Assert.Equal(25, person.Age);
    }

    [Fact]
    public void Write_Should_Write_CsvString()
    {
        var parser = new PersonCsvParser();

        var person = new Person { Name = "Ann", Age = 7 };
        var text = parser.Write(person);

        Assert.Equal("Ann,7,", text);
    }

    [Fact]
    public void Parse_Should_Throw_ArgumentNullException_When_Text_Is_Null()
    {
        var parser = new PersonCsvParser();

        Assert.Throws<ArgumentNullException>(() => parser.Parse(null!));
    }
}
