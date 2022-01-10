using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpMet
{
    /// <summary>
    /// Http serializer contract for set body and fetch data from response
    /// </summary>
    public interface IRestSerializer
    {
         void Serialize<T>(T obj, HttpRequestMessage message);

         Task<T> Deserialize<T>(HttpResponseMessage message);
    }
}
