using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DNResource.IO
{

    public static class HTTPResource
    {
        public static IResource<HttpClient> HttpClient()
        {
            return Resource.Create(() => new HttpClient(), hc => hc.Dispose());
        }

        public static IResource<HttpResponseMessage> Get(string url)
        {
            return HTTPResource.HttpClient().SelectAsync(hc => hc.GetAsync(url));
        }

        public static IResource<String> GetString(string url)
        {
            return HTTPResource.HttpClient().SelectAsync(hc => hc.GetStringAsync(url));
        }
    }
}