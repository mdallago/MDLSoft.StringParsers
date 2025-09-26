using System;
using System.Linq;
using System.Linq.Expressions;

namespace MDLSoft.StringParsers
{
    public abstract class FixedStringParser<T> : AbstractStringParser<T> where T : class, new()
    {
        protected class FixedStringDefinitionBuilder
        {
            private readonly DefinitionBuilder builder;

            public FixedStringDefinitionBuilder(DefinitionBuilder builder)
            {
                this.builder = builder;
            }

            public FixedStringDefinitionBuilder WithValidator(Func<string, bool> validator)
            {
                builder.WithValidator(validator);
                return this;
            }

            public FixedStringDefinitionBuilder WithReadConverter<TProperty>(Func<string, TProperty> converter)
            {
                builder.WithReadConverter(converter);
                return this;
            }

            public FixedStringDefinitionBuilder WithWriteConverter<TProperty>(Func<TProperty, string> converter)
            {
                builder.WithWriteConverter(converter);
                return this;
            }

            public FixedStringDefinitionBuilder WithWriteLeftPadder(char @char)
            {
                builder.WithWriteConverter(StringOperations.LeftPadder<object>(((FixedParserDefinition)builder.Definition).Length, @char));
                return this;
            }

            public FixedStringDefinitionBuilder WithWriteRightPadder(char @char)
            {
                builder.WithWriteConverter(StringOperations.RightPadder<object>(((FixedParserDefinition)builder.Definition).Length, @char));
                return this;
            }

            public FixedStringDefinitionBuilder WithWriteZeroLeftPadder()
            {
                return WithWriteLeftPadder('0');
            }

            public FixedStringDefinitionBuilder WithWriteZeroRightPadder()
            {
                return WithWriteRightPadder('0');
            }

            public FixedStringDefinitionBuilder WithWriteSpaceLeftPadder()
            {
                return WithWriteLeftPadder(' ');
            }

            public FixedStringDefinitionBuilder WithWriteSpaceRightPadder()
            {
                return WithWriteRightPadder(' ');
            }
        }

        private class FixedParserDefinition : ParserDefinition
        {
            public int Start { get; set; }
            public int Length { get; set; }
        }

        public static class StringOperations
        {
            public static Func<TProperty, string> LeftPadder<TProperty>(int length, char @char)
            {
                return x =>
                {
                    if (x == null) return new string(@char, length);
                    var temp = x.ToString().PadLeft(length, @char);
                    return (temp.Length > length) ? temp.Substring(0, length) : temp;
                };
            }

            public static Func<TProperty, string> RightPadder<TProperty>(int length, char @char)
            {
                return x =>
                {
                    if (x == null) return new string(@char, length);
                    var temp = x.ToString().PadRight(length, @char);
                    return (temp.Length > length) ? temp.Substring(0, length) : temp;
                };
            }
        }

        private string data;

        protected FixedStringDefinitionBuilder Define<TProperty>(Expression<Func<T, TProperty>> property, int start, int length)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start), "Start position cannot be negative");
            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be positive");
                
            return new FixedStringDefinitionBuilder(AddDefinition(property, new FixedParserDefinition { Start = start, Length = length }));
        }

        protected override string GetValue(ParserDefinition definition)
        {
            var fixedDef = (FixedParserDefinition)definition;
            if (data == null)
                throw new StringParserException("Input data is null");
            if (fixedDef.Start < 0 || fixedDef.Start >= data.Length)
                throw new StringParserException(string.Format("Start position {0} is out of range for field {1}", fixedDef.Start, definition.Member.Name));
            if (fixedDef.Start + fixedDef.Length > data.Length)
                throw new StringParserException(string.Format("Length {0} extends beyond input data for field {1}", fixedDef.Length, definition.Member.Name));
            
            return data.Substring(fixedDef.Start, fixedDef.Length);
        }

        protected override string GetString(string value)
        {
            return value;
        }

        protected override void Initialize(string text)
        {
            data = text;
        }

        protected void ValidateDefinitions()
        {
            var def = Definitions.Take(1).Cast<FixedParserDefinition>().Single();
            int inicio = def.Start;
            int largo = def.Length;

            foreach (var definition in Definitions.Skip(1).Cast<FixedParserDefinition>())
            {
                if (definition.Start != inicio + largo)
                {
                    throw new StringParserException(string.Format("Error in field definition. Field: {0}", definition.Member));
                }
                inicio = definition.Start;
                largo = definition.Length;
            }
        }
    }
}