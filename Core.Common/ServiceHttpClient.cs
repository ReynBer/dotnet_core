using Core.Common.Extension;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    public static class ServiceHttpClient
    {
        private static HttpClientHandler GetClientHandler()
            => new HttpClientHandler
            {
                PreAuthenticate = true,
                UseDefaultCredentials = true,
                AllowAutoRedirect = false
            };

        private static void SetHeaders(HttpClient client, Dictionary<string, string> headers)
        {
            foreach (var extraHeader in headers)
                client.DefaultRequestHeaders.Add(extraHeader.Key, extraHeader.Value);
        }

        public static AuthenticationHeaderValue GetBasicAuthenticationHeader(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return null;
            var byteArray = Encoding.ASCII.GetBytes($"{login}:{password}");
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public static HttpResponseMessage Get(string uri)
            => GetAsync(uri).Result;

        public static HttpResponseMessage Get(string uri, Dictionary<string, string> extraHeaders)
            => GetAsync(uri, extraHeaders).Result;

        public static async Task<HttpResponseMessage> GetAsync(string uri)
            => await GetAsync(uri, new Dictionary<string, string>());

        public static async Task<HttpResponseMessage> GetAsync(string uri, Dictionary<string, string> extraHeaders, AuthenticationHeaderValue authenticationHeader = null)
        {
            using (var httpClient = new HttpClient(GetClientHandler()) { Timeout = TimeSpan.FromMinutes(5) }
                        .Do(h => h.DefaultRequestHeaders.Authorization = authenticationHeader)
                        .Do(h => SetHeaders(h, extraHeaders)))
                return await httpClient.GetAsync(uri);
        }

        public static async Task<HttpResponseMessage> PostAsync(string uri, string data)
        {
            using (var httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(5) })
                return await httpClient.PostAsync(uri, new StringContent(data));
        }
    }
}
