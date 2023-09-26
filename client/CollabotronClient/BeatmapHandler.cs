using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Editor_Reader;

namespace CollabotronClient
{
    class BeatmapHandler
    {
        private string filePath;
        private EditorReader reader;

        public BeatmapHandler()
        {
            reader = new EditorReader();
        }

        public string GetFileName()
        {
            return filePath;
        }

        public void ReadEditor()
        {
            reader.FetchAll();
            filePath = Path.Combine(reader.ContainingFolder, reader.Filename);
        }

        public string GetBeatmapContents()
        {
            return File.ReadAllText(filePath);
        }

        public void WriteToBeatmap(string data)
        {
            File.WriteAllText(filePath, data);
        }
    }
}
