using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpMet
{
    /// <summary>
    /// Serializer for json format base on <see cref="Newtonsoft.Json"/>
    /// </summary>
    public class NewtonJsonSerializer : IRestSerializer
    {
        /// <summary>
        /// Take from response content and serialize using <see cref="Newtonsoft.Json.JsonConvert"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<T> Deserialize<T>(HttpResponseMessage message)
        {
            // from string content
            return JsonConvert.DeserializeObject<T>(await message.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Put json in body request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public void Serialize<T>(T obj, HttpRequestMessage message)
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(obj));
        }
    }
}