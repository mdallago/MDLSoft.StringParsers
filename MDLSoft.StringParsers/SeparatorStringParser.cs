using System;
using System.Linq.Expressions;

namespace MDLSoft.StringParsers
{
    public abstract class SeparatorStringParser<T> : AbstractStringParser<T> where T : class,new()
    {
        private class SeparatorParserDefinition : ParserDefinition
        {
            public int Index { get; set; }
        }

        private readonly char separator;
        private string[] partes;

        protected SeparatorStringParser(char separator)
        {
            this.separator = separator;
        }

        protected DefinitionBuilder Define<TProperty>(Expression<Func<T, TProperty>> property, int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative");
                
            return AddDefinition(property, new SeparatorParserDefinition { Index = index });
        }

        protected override string GetValue(ParserDefinition definition)
        {
            var sepDef = (SeparatorParserDefinition)definition;
            if (partes == null)
                throw new StringParserException("Input data has not been initialized");
            if (sepDef.Index < 0 || sepDef.Index >= partes.Length)
                throw new StringParserException(string.Format("Index {0} is out of range for field {1}", sepDef.Index, definition.Member.Name));
                
            return partes[sepDef.Index];
        }

        protected override string GetString(string value)
        {
            return value + separator;
        }

        protected override void Initialize(string text)
        {
            partes = text.Split(separator);
        }
    }
}