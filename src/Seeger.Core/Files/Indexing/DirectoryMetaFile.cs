using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Seeger.Files.Indexing
{
    public class DirectoryMetaFile
    {
        private readonly object _lock = new object();

        public string Path { get; private set; }

        public DirectoryMetaFile(string path)
        {
            Require.NotNullOrEmpty(path, "path");
            Path = path;
        }

        public IList<FileIndexEntry> ReadEntries()
        {
            if (!File.Exists(Path))
            {
                return new List<FileIndexEntry>();
            }

            return JsonConvert.DeserializeObject<List<FileIndexEntry>>(File.ReadAllText(Path, Encoding.UTF8));
        }

        public void AddEntry(FileIndexEntry entry)
        {
            Require.NotNull(entry, "entry");

            lock (_lock)
            {
                var entries = ReadEntries();
                entries.Add(entry);
                File.WriteAllText(Path, JsonConvert.SerializeObject(entries));
            }
        }

        public void UpdateEntry(FileIndexEntry entry)
        {
            Require.NotNull(entry, "entry");

            lock (_lock)
            {
                var entries = ReadEntries();

                for (var i = 0; i < entries.Count; i++)
                {
                    if (entries[i].Name.Equals(entry.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        entries[i] = entry;
                        break;
                    }
                }

                File.WriteAllText(Path, JsonConvert.SerializeObject(entries));
            }
        }

        public void RemoveEntry(string fileName)
        {
            Require.NotNullOrEmpty(fileName, "fileName");

            lock (_lock)
            {
                var entries = ReadEntries();
                var entry = entries.FirstOrDefault(x => x.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase));
                if (entry != null)
                {
                    entries.Remove(entry);
                    File.WriteAllText(Path, JsonConvert.SerializeObject(entries));
                }
            }
        }
    }
}
