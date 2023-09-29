using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace CollabotronClient
{
    class HTTPHandler
    {
        private string cookies;

        public HTTPHandler()
        {
            cookies = null;
        }

        public async Task<string> Get(string url)
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

        public async Task<string> Post(string url, List<KeyValuePair<string, string>> body = null, string uploadFilePath = null)
        {
            if ((body == null && uploadFilePath == null) || (body != null && uploadFilePath != null))
            {
                throw new ArgumentException("Exactly one of body, uploadFilePath, must be not null.");
            }

            using (var client = new HttpClient())
            {
                
                if (cookies != null)
                {
                    client.DefaultRequestHeaders.Add("Cookie", cookies);
                }

                HttpResponseMessage response = null;

                if (body != null)
                {
                    var content = new FormUrlEncodedContent(body);

                    response = await client.PostAsync(url, content);

                }
                else if (uploadFilePath != null)
                {
                    using(var content = new MultipartFormDataContent())
                    {
                        using (var fileStream = new FileStream(uploadFilePath, FileMode.Open, FileAccess.Read))
                        using (var fileContent = new StreamContent(fileStream))
                        {
                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                            {
                                Name = "\"file\"",
                                FileName = "\"map.osu\""
                            };

                            content.Add(fileContent);

                            response = await client.PostAsync(url, content);
                        }
                    }
                }

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
    }
}
