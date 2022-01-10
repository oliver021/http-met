using System.Net.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FluentRest
{
    /// <summary>
    /// This class manage the response source to handle by user
    /// </summary>
    public class ResponseHandler : IDisposable
    {
        /// <summary>
        /// this is an object that store the htto response source
        /// </summary>
        private HttpResponseMessage httpResponseMessage;

        /// <summary>
        /// require underlaying http response object
        /// </summary>
        /// <param name="httpResponseMessage"></param>
        public ResponseHandler(HttpResponseMessage httpResponseMessage)
        {
            this.httpResponseMessage = httpResponseMessage;
        }

        /// <summary>
        /// Indicate if the success request is true
        /// </summary>
        public bool Success { get => httpResponseMessage.IsSuccessStatusCode; }

        /// <summary>
        /// Number code of http request
        /// </summary>
        public int Code { get => (int) httpResponseMessage.StatusCode; }

        /// <summary>
        /// The code reason for standard http protocol
        /// </summary>
        public string CodeTitle { get =>  Enum.GetName(typeof(HttpStatusCode), httpResponseMessage.StatusCode); }

        /// <summary>
        /// Get a response content as string source
        /// </summary>
        /// <returns></returns>
        public Task<string> ContentAsString() => httpResponseMessage.Content.ReadAsStringAsync();

        /// <summary>
        /// for free resopurce from <see cref="httpResponseMessage"/>
        /// </summary>
        public void Dispose()
        {
            httpResponseMessage.Dispose();
        }

        /// <summary>
        /// This method allow fetch result data from a deserialization from json source result
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<TResult> GetResult<TResult>()
        {
            return JsonConvert.DeserializeObject<TResult>(await ContentAsString());
        }

        /// <summary>
        /// Method to fetch a header response
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<string> GetHeader(string name)
        {
            return httpResponseMessage.Headers.GetValues(name);
        }
    }
}