using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HttpMet
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Register as service a request template function.
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="services"></param>
        /// <param name="build"></param>
        /// <returns></returns>
        public static IServiceCollection AddRequest<TDelegate, T, TResult>(this IServiceCollection services, Action<T, HttpRequestMessage> build)
        where TDelegate: Delegate
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // use provider from service collections.
            services.AddTransient(p => {
                return RestFactory.RequestFromDelegate<TDelegate, T, TResult>(build, p);
            });

            return services;
        } 
    }
}
