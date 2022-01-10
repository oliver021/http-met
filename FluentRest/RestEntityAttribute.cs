using System;
using System.Collections.Generic;
using System.Text;

namespace FluentRest
{
    /// <summary>
    /// Rest Entity Attribute help to set an entity as request to send via json or other serializer
    /// an endpoint rest api
    /// </summary>
    public class RestEntityAttribute : Attribute
    {
        /// <summary>
        /// Construct to set basic parameters
        /// </summary>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        public RestEntityAttribute(RestMethods method, string uri)
        {
            if (uri is null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            Method = method;
            Uri = uri;
        }

        public RestMethods Method { get; }
        public string Uri { get; }
    }
}
