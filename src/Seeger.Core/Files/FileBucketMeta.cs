using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Seeger.Files
{
    public class FileBucketMeta
    {
        public string BucketId { get; set; }

        public string DisplayName { get; set; }

        public string FileSystemProviderName { get; set; }

        public IDictionary<string, string> Config { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsDefault { get; set; }

        public FileBucketMeta()
        {
            CreatedAt = DateTime.Now;
            Config = new Dictionary<string, string>();
        }

        public FileBucketMeta Clone()
        {
            var meta = (FileBucketMeta)this.MemberwiseClone();
            meta.Config = new Dictionary<string, string>(Config);
            return meta;
        }
    }
}
