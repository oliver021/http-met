using System;

namespace HttpMet
{
    public class BaseUriAttribute : Attribute
    {
        public BaseUriAttribute(string uri)
        {
            if (uri is null)
            {
                throw new ArgumentNullException(nameof(uri));
            }
            Uri = uri;
        }

        public string Uri { get; }
    }
}