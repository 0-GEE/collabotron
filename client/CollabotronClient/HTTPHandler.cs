using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace CollabotronClient
{
    class HTTPHandler
    {
        private string url;
        private string cookies;

        public HTTPHandler(string url)
        {
            this.url = url;
            cookies = null;
        }

        public async Task<string> Get()
        {
            using(var client = new HttpClient())
            {
                if (cookies != null)
                {
                    client.DefaultRequestHeaders.Add("Cookie", cookies);
                }
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"HTTP Request Failure. Code {response.StatusCode}");
                }

                if (response.Headers.TryGetValues("Set-Cookie", out var newCookies))
                {
                    cookies = newCookies.FirstOrDefault();
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
        }

        public async Task<string> Post(Dictionary<string, string> body)
        {// TODO
            return "";
        }
    }
}
