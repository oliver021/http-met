using System;

namespace HttpMet
{
    public class ParamAttribute : Attribute
    {
        public ParamAttribute(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }
    }

    public class QueryParamAttribute : ParamAttribute
    {
        public QueryParamAttribute(string name, string value) : base(name, value)
        {
        }
    }

    public class HeaderParamAttribute : ParamAttribute
    {
        public HeaderParamAttribute(string name, string value) : base(name, value)
        {
        }
    }
}