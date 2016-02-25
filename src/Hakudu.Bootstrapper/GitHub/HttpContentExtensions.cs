using System;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace Hakudu.Bootstrapper.GitHub
{
    /// <summary>
    /// Provides polyfills for standard extension methods available in Microsoft.AspNet.WebApi.Client package.
    /// </summary>
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            return (T) await content.ReadAsAsync(typeof(T));
        }

        public static async Task<object> ReadAsAsync(this HttpContent content, Type type)
        {
            var serializer = new DataContractJsonSerializer(type);

            using (var responseStream = await content.ReadAsStreamAsync())
            {
                return serializer.ReadObject(responseStream);
            }
        }
    }
}
