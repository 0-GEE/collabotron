using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Editor_Reader;
using Newtonsoft.Json;

namespace CollabotronClient
{
    class BeatmapHandler
    {
        private string songsFolder;
        private string filePath;
        private EditorReader reader;

        public BeatmapHandler(string songsPath = null)
        {
            reader = new EditorReader();
            songsFolder = songsPath;

        }

        public string GetFileName()
        {
            return filePath;
        }

        public void ReadEditor()
        {
            if (songsFolder == null)
            {
                return;
            }
            reader.FetchAll();
            filePath = Path.Combine(new string[] { songsFolder, reader.ContainingFolder, reader.Filename });
        }

        public string GetBeatmapContents()
        {
            return File.ReadAllText(filePath);
        }

        public void WriteToBeatmap(string data)
        {
            File.WriteAllText(filePath, data);
        }

        public bool IsSongsFolderSet()
        {
            return songsFolder != null;
        }

        public void SetSongsFolder(string path)
        {
            songsFolder = path;
        }
    }
}
