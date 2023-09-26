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
        }

        public string Get(Dictionary<string, string> getParameters = null)
        {// TODO
            return "";
        }
    }
}
