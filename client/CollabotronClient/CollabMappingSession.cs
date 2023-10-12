using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;

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

        public delegate void CollabEventHandler(object sender, CollabEventArgs e);
        public event CollabEventHandler CollabEvent;

        public CollabMappingSession(string serverHost, string serverPort)
        {
            urlBase = $"http://{serverHost}:{serverPort}";
            netHandler = new HTTPHandler();
            beatmap = new BeatmapHandler();
            isMapHost = false;
            isSessionActive = false;

            try
            {
                var config = GetConfig();

                if (config.TryGetValue("songs_folder", out string songsFolder))
                {
                    beatmap.SetSongsFolder(songsFolder);
                }
            }
            catch (FileNotFoundException)
            {
            }
        }

        public async Task Authenticate(string accessCode)
        {
            beatmap.ReadEditor();
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
                    throw new Exception($"Could not authenticate successfully (server returned {result})");
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

        public async Task RefreshMapData()
        {
            string url = urlBase + refreshPath;

            string beatmapData = await netHandler.Get(url);
            beatmap.WriteToBeatmap(beatmapData);
        }

        public async Task BeginGetStateLoop()
        {
            while (true)
            {
                if (!isSessionActive)
                {
                    return;
                }

                string url = urlBase + statePath;

                var resp = JsonConvert.DeserializeObject<Dictionary<string, bool>>(await netHandler.Get(url));

                if (resp.TryGetValue("is_host", out bool isHost) && resp.TryGetValue("refresh", out bool isRefresh))
                {
                    if (isHost != isMapHost || isRefresh)
                    {
                        isMapHost = isHost;
                        OnCollabEvent(isHost, isRefresh);
                    }
                }
                else
                {
                    throw new Exception($"Bad state data: {resp}");
                }

                await Task.Delay(1000);
            }

            
            
        }

        public bool IsReady()
        {
            return beatmap.IsSongsFolderSet();
        }

        public void SetSongsFolder(string path)
        {
            beatmap.SetSongsFolder(path);

            Dictionary<string, string> config;
            try
            {
                config = GetConfig();
            }
            catch (FileNotFoundException)
            {
                config = new Dictionary<string, string>();
            }

            config.Add("songs_folder", path);
            SaveConfig(config);
        }

        private Dictionary<string, string> GetConfig()
        {
            var configContents = File.ReadAllText("config.json");
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(configContents);
        }

        private void SaveConfig(Dictionary<string, string> config)
        {
            var configContents = JsonConvert.SerializeObject(config);
            File.WriteAllText("config.json", configContents);
        }

        protected virtual void OnCollabEvent(bool isHost, bool isRefresh)
        {
            CollabEvent?.Invoke(this, new CollabEventArgs(isHost, isRefresh));
        }

        public void StopSession()
        {
            isSessionActive = false;
            isMapHost = false;
        }
    }
}
