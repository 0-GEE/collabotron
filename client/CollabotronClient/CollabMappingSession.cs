using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace CollabotronClient
{
    class CollabMappingSession
    {
        private HTTPHandler netHandler;
        private BeatmapHandler beatmap;
        private string urlBase;
        private bool isMapHost;
        private readonly string authPath = "/register";
        private readonly string refreshPath = "/refresh";
        private readonly string statePath = "/";
        private readonly string uploadPath = "/upload";

        private bool isSessionActive;

        public CollabMappingSession(string serverHost, string serverPort, string accessCode)
        {
            urlBase = $"http://{serverHost}:{serverPort}";
            netHandler = new HTTPHandler();
            beatmap = new BeatmapHandler();
            beatmap.ReadEditor();
            isMapHost = false;
            isSessionActive = false;

            Authenticate(accessCode);
        }

        public async void Authenticate(string accessCode)
        {
            var requestBody = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("password", accessCode)
            };

            var url = urlBase + authPath;

            var resp = JsonConvert.DeserializeObject<Dictionary<string, bool>>(await netHandler.Post(url, requestBody));

            if (resp.TryGetValue("success", out bool result))
            {
                if (!result)
                {
                    throw new Exception("Could not authenticate successfully (server returned 'false')");
                }

                isSessionActive = true;
            } else
            {
                throw new KeyNotFoundException("Could not find authentication result bool in server response.");
            }
        }

        public async Task<bool> UploadBeatmap()
        {
            Dictionary<string, bool> resp;
            
            try
            {
                string url = urlBase + uploadPath;
                resp = JsonConvert.DeserializeObject<Dictionary<string, bool>>(await netHandler.Post(url, null, beatmap.GetFileName()));
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return false;
                }

                throw;
            }

            if (resp.TryGetValue("success", out bool result))
            {
                return result;
            }

            throw new Exception("Could not find upload result bool in server response.");
        }

        public async void RefreshMapData()
        {
            // TODO
        }

        private async void GetStateAsync()
        {
            // TODO
        }
    }
}
