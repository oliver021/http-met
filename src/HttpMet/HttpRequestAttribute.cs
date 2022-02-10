using System;

namespace HttpMet
{
    public class HttpRequestAttribute : Attribute
    {
        public HttpRequestAttribute(string path, RestMethods methods)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or empty.", nameof(path));
            }
            Path = path;
            Methods = methods;
        }

        public string Path { get; }
        public RestMethods Methods { get; }
    }
}