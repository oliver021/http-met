using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HttpMet
{
    /// <summary>
    /// Many helper to create rest request and another types request templates.
    /// </summary>
    public static class RestFactory
    {
        /// <summary>
        /// Internal service provider.
        /// </summary>
        internal static IServiceProvider Provider { get; private set; }

        /// <summary>
        /// Transient service getter.
        /// </summary>
        internal static HttpClient NewClient => Provider.GetService<HttpClient>();

        /// <summary>
        /// Serializer instance to serialize/deserialize tasks.
        /// </summary>
        public static IRestSerializer Serializer { get; private set; } = DefaultSerializer;

        /// <summary>
        /// Newtonsoft json serializer for default.
        /// </summary>
        public static IRestSerializer DefaultSerializer => new NewtonJsonSerializer();

        /// <summary>
        /// Init provider from internal <see cref="IServiceProvider"/>
        /// </summary>
        static RestFactory()
        {
            var collection = new ServiceCollection();

            // register request client
            collection.AddHttpClient();

            // register as transient service rest client
            collection.AddTransient<RestClient>();

            // build internal provider
            Provider = collection.BuildServiceProvider();
        }

        /// <summary>
        /// Set internal serializer for <see cref="RestFactory"/>
        /// </summary>
        /// <param name="serializer"></param>
        public static void SetRestFactorySerializer(IRestSerializer serializer)
        {
            Serializer = serializer;
        }

        /// <summary>
        /// Extension for quick set url.
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static HttpRequestMessage Url(this HttpRequestMessage httpRequest, string path)
        {
            httpRequest.RequestUri = new Uri(path);
            return httpRequest;
        }

        /// <summary>
        /// Set request method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpRequest"></param>
        /// <param name="verb"></param>
        /// <returns></returns>
        public static HttpRequestMessage Method(this HttpRequestMessage httpRequest, RestMethods verb)
        {
            switch (verb)
            {
                case RestMethods.Get:
                    httpRequest.Method = HttpMethod.Get;
                    break;

                case RestMethods.Post:
                    httpRequest.Method = HttpMethod.Post;
                    break;

                case RestMethods.Put:
                    httpRequest.Method = HttpMethod.Put;
                    break;

                case RestMethods.Patch:
                    httpRequest.Method = new HttpMethod("PATCH");
                    break;

                case RestMethods.Delete:
                    httpRequest.Method = HttpMethod.Delete;
                    break;

                case RestMethods.Head:
                    httpRequest.Method = HttpMethod.Head;
                    break;
            }

            return httpRequest;
        }

        /// <summary>
        /// Put the query in http message if not has been set
        /// </summary>
        /// <param name="message"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static HttpRequestMessage Query(this HttpRequestMessage message, Dictionary<string, object> keys)
        {
            if (message.RequestUri.Query != string.Empty)
            {
                throw new InvalidOperationException("The query url is setted");
            }

            message.RequestUri = new Uri(uriString: message.RequestUri.AbsolutePath + MakeQuery(keys));

            return message;
        }

        /// <summary>
        /// Set body from a value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpRequest"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static HttpRequestMessage Body<T>(this HttpRequestMessage httpRequest, T value)
        {
            Serializer.Serialize(value, httpRequest);
            return httpRequest;
        }

        /// <summary>
        /// Create request delegate from request template
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="build"></param>
        /// <returns></returns>
        public static TDelegate RequestFromDelegate<TDelegate, T, TResult>(Action<T, HttpRequestMessage> build)
            where TDelegate: Delegate
        {
            return (TDelegate) Request<T, TResult>(build).Method.CreateDelegate(typeof(TDelegate),null);
        }

        /// <summary>
        /// Create request delegate from request template
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="build"></param>
        /// <returns></returns>
        public static TDelegate RequestFromDelegate<TDelegate, T, T2, TResult>(Action<T, T2, HttpRequestMessage> build)
            where TDelegate : Delegate
        {
            return (TDelegate)Request<T, T2, TResult>(build).Method.CreateDelegate(typeof(TDelegate), null);
        }

        /// <summary>
        /// Create request delegate from request template
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="build"></param>
        /// <returns></returns>
        public static TDelegate RequestFromDelegate<TDelegate, T, T2, T3, TResult>(Action<T, T2, T3, HttpRequestMessage> build)
            where TDelegate : Delegate
        {
            return (TDelegate)Request<T, T2, T3, TResult>(build).Method.CreateDelegate(typeof(TDelegate), null);
        }

        /// <summary>
        /// Create request delegate from request template
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="build"></param>
        /// <returns></returns>
        public static TDelegate RequestFromDelegate<TDelegate, T, T2, T3, T4, TResult>(Action<T, T2, T3, T4, HttpRequestMessage> build)
            where TDelegate : Delegate
        {
            return (TDelegate)Request<T, T2, T3, T4, TResult>(build).Method.CreateDelegate(typeof(TDelegate), null);
        }

        /// <summary>
        /// Build a delegate that define http request template.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="build"></param>
        /// <returns></returns>
        public static Func<T, Task<TResult>> Request<T, TResult>(Action<T, HttpRequestMessage> build)
        {
            if (build is null)
            {
                throw new ArgumentNullException(nameof(build));
            }
            return async (arg) =>
            {
                // get http client from provider
                var http = Provider.GetService<HttpClient>();

                // init http request
                var msg = new HttpRequestMessage();

                // build message
                build(arg, msg);

                // make deserialization
                return await Serializer.Deserialize<TResult>(await http.SendAsync(msg));
            };
        }

        /// <summary>
        /// Build a delegate that define http request template
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="build"></param>
        /// <returns></returns>
        public static Func<T, T2, T3, Task<TResult>> Request<T, T2, T3, TResult>(Action<T, T2, T3, HttpRequestMessage> build)
        {
            if (build is null)
            {
                throw new ArgumentNullException(nameof(build));
            }
            // from delegate
            return async (arg, arg2, arg3) => 
            {
                // get http client from provider
                var http = Provider.GetService<HttpClient>();

                // init http request
                var msg = new HttpRequestMessage();

                // build message
                build(arg, arg2, arg3, msg);

                // make deserialization
                return await Serializer.Deserialize<TResult>(await http.SendAsync(msg));
            };
        }

        /// <summary>
        /// Build a delegate that define http request template.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="build"></param>
        /// <returns></returns>
        public static Func<T, T2, Task<TResult>> Request<T, T2, TResult>(Action<T, T2, HttpRequestMessage> build)
        {
            if (build is null)
            {
                throw new ArgumentNullException(nameof(build));
            }
            // from delegate
            return async (arg, arg2) =>
            {
                // get http client from provider
                var http = Provider.GetService<HttpClient>();

                // init http request
                var msg = new HttpRequestMessage();

                // build message
                build(arg, arg2, msg);

                // make deserialization
                return await Serializer.Deserialize<TResult>(await http.SendAsync(msg));
            };
        }

        /// <summary>
        /// Build a delegate that define http request template.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="build"></param>
        /// <returns></returns>
        public static Func<T, T2, T3, T4, Task<TResult>> Request<T, T2, T3, T4, TResult>(Action<T, T2, T3, T4, HttpRequestMessage> build)
        {
            if (build is null)
            {
                throw new ArgumentNullException(nameof(build));
            }
            // from delegate
            return async (arg, arg2, arg3, arg4) =>
            {
                // get http client from provider
                var http = Provider.GetService<HttpClient>();

                // init http request
                var msg = new HttpRequestMessage();

                // build message
                build(arg, arg2, arg3, arg4, msg);

                // make deserialization
                return await Serializer.Deserialize<TResult>(await http.SendAsync(msg));
            };
        }

        /// <summary>
        /// Simple helper to make a short code.
        /// </summary>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        internal static string MakeQuery(Dictionary<string, object> queryParams)
        {
            if (queryParams is null || queryParams.Any() is false)
            {
                return string.Empty;
            }

            return "?" + string.Join('&', queryParams.Select(q => $"{q.Key}={q.Value}"));
        }
    }
}
