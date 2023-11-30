using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CommonHttpClient
    {
        private readonly HttpClient httpClient;

        public CommonHttpClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CallApi(string url, HttpMethod httpMethod, string data,
            string accessToken = null, Dictionary<string, string> headers = null)
        {
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                AddAuthenticationToken(accessToken);
            }

            using var req = new HttpRequestMessage(httpMethod, url) { Method = httpMethod };
            AddHeaders(req, headers);

            if (!string.IsNullOrWhiteSpace(data))
            {
                req.Content = new StringContent(data, Encoding.UTF8, "application/json");
                req.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            }
            return await httpClient.SendAsync(req);
        }

        protected virtual void AddHeaders(HttpRequestMessage req, Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    req.Headers.Add(header.Key, header.Value);
                }
            }

        }

        protected virtual void AddAuthenticationToken(string accessToken)
        {

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
