using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;

namespace HttpMet
{
    /// <summary>
    /// The Rest Client base to create a rest http request
    /// This class contains many several helper to create a request
    /// </summary>
    public class RestClient : IDisposable
    {
        /// <summary>
        /// The uri base to build a complete request url
        /// </summary>
        private readonly Uri _uriBase;

        /// <summary>
        /// http client to send request
        /// </summary>
        private HttpClient _client = null;
        
        /// <summary>
        /// This dictionary contains basic header to send in request
        /// </summary>
        private Dictionary<string, string> _headers { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// avoid insert parameters this a constructor parameterless
        /// </summary>
        public RestClient()
        {
            _client = new HttpClient();
        }

        /// <summary>
        /// Base uri string
        /// </summary>
        /// <param name="uriBase"></param>
        public RestClient(string uriBase) : base()
        {
            _uriBase = new Uri(uriBase);
        }

        /// <summary>
        /// Base uri string
        /// </summary>
        /// <param name="uriBase"></param>
        public RestClient(Uri uriBase) : base()
        {
            _uriBase = uriBase;
        }

        /// <summary>
        /// with relative url included
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="relativeUri"></param>
        public RestClient(Uri baseUri, string relativeUri) : base()
        {
            _uriBase = new Uri(baseUri, relativeUri);
        }

        /// <summary>
        /// Base uri string with uri kind parameter
        /// </summary>
        /// <param name="uriString"></param>
        /// <param name="uriKind"></param>
        public RestClient(string uriString, UriKind uriKind) : base()
        {
            _uriBase = new Uri(uriString, uriKind);
        }

        /// <summary>
        /// Disponse depends objects
        /// </summary>
        public void Dispose()
        {
            _client.Dispose();
        }

        /// <summary>
        /// Set an header to send a request
        /// </summary>
        /// <param name="header"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public RestClient UseHeader(string header, string value)
        {
            _headers.Add(header, value);
            return this;
        }

        /// <summary>
        /// The Beaber Token request can be set through this method
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public RestClient UseBeaberToken(string accessToken)
        {
            return this;
        }

        /// <summary>
        /// Set basic authentication method to make a request
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public RestClient UseBasicAuthorization(string username, string password)
        {
            return this;
        }

        /// <summary>
        /// Set no cache header to make a request
        /// </summary>
        /// <returns></returns>
        public RestClient NoCache()
        {
            return this;
        }

        public RestClient SetEncodeHeader()
        {
            return this;
        }

        /// <summary>
        /// This method allow send a http request through a data from class
        /// </summary>
        /// <param name="request"></param>
        /// <param name="extraPath"></param>
        /// <returns></returns>
        public Task<ResponseHandler> ExecuteAsync(object request, string extraPath = "")
        {
            var type = request.GetType();

            // recovery basic rest attribute is required
            var restAttr = type.GetCustomAttribute<RestEntityAttribute>( false);

            // validate and throw exception if attr is null
            if (restAttr is null)
                throw new ArgumentException($"The object of type {type.Name} has no Rest Attribute");

            // execute finally the request
            return MakeRequest(request, restAttr.Method, restAttr.Uri);
        }

        /// <summary>
        /// The basic send request implementation
        /// </summary>
        /// <param name="request"></param>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        private Task<ResponseHandler> MakeRequest(object request, RestMethods method, string uri)
        {
            // base on http method
            switch (method)
            {
                case RestMethods.Get:
                    return GetAsync(uri, _ToDictionary(request));

                case RestMethods.Post:
                    return PostAsync(uri, request);

                case RestMethods.Put:
                    return PutAsync(uri, request);

                case RestMethods.Delete:
                    return DeleteAsync(uri, _ToDictionary(request));

                // for default other method is not supported in this operation
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Simple extra helper to run a process to fetch data from an object and convert to a dictionary
        /// with key-value structure, then you can use a model class instance as query  container
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private Dictionary<string, string> _ToDictionary(object request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method make a http request by GET method and recive a dictionary as query params
        /// </summary>
        /// <param name="path"></param>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public async Task<ResponseHandler> GetAsync(string path, Dictionary<string,string> queryParams)
        {
            var httpMsg = new HttpRequestMessage(HttpMethod.Get, _uriBase.OriginalString + path + _makeQuery(queryParams));

            // send request and create handler object
            return new ResponseHandler(await _client.SendAsync(httpMsg));
        }

        /// <summary>
        /// This method make a http request by POST method and recive a dictionary as query params
        /// </summary>
        /// <param name="path"></param>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public async Task<ResponseHandler> PostAsync(string path, object data, Dictionary<string, string> queryParams = null, BodyKind bodyKind = BodyKind.Json)
        {
            var httpMsg = new HttpRequestMessage(HttpMethod.Post, _uriBase.OriginalString + path + _makeQuery(queryParams));

            // set content in body
            SetContent(data, bodyKind, httpMsg);

            // send request and create handler object
            return new ResponseHandler(await _client.SendAsync(httpMsg));
        }

        /// <summary>
        /// This method make a http request by PUT method and recive a dictionary as query params
        /// </summary>
        /// <param name="path"></param>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public async Task<ResponseHandler> PutAsync(string path, object data, Dictionary<string, string> queryParams = null, BodyKind bodyKind = BodyKind.Json)
        {
            var httpMsg = new HttpRequestMessage(HttpMethod.Put, _uriBase.OriginalString + path + _makeQuery(queryParams));

            // set content in body
            SetContent(data, bodyKind, httpMsg);

            // send request and create handler object
            return new ResponseHandler(await _client.SendAsync(httpMsg));
        }

        /// <summary>
        /// This method make a http request by DELETE method and recive a dictionary as query params
        /// </summary>
        /// <param name="path"></param>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        public async Task<ResponseHandler> DeleteAsync(string path, Dictionary<string, string> queryParams)
        {
            var httpMsg = new HttpRequestMessage(HttpMethod.Put, _uriBase.OriginalString + path + _makeQuery(queryParams));

            // send request and create handler object
            return new ResponseHandler(await _client.SendAsync(httpMsg));
        }

        /// <summary>
        /// Helper to set body data and format
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bodyKind"></param>
        private void SetContent(object data, BodyKind bodyKind, HttpRequestMessage httpMsg)
        {
            switch (bodyKind)
            {
                // json format encode basic and std data
                case BodyKind.Json:
                    httpMsg.Content = null;
                    break;

                // for files and parameters
                case BodyKind.Multipart:
                    break;

                // classic parameters to set forms
                case BodyKind.Urlencode:
                    break;

                // plain body support
                case BodyKind.Plain:
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Simple helper to make a short code
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        private string _makeQuery(Dictionary<string, string> queryParams)
        {
            throw new NotImplementedException();
        }
    }
}
