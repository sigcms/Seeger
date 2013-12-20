using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.QiniuCloud
{
    public class QiniuBucketSettings
    {
        public string Bucket { get; set; }

        public string Domain { get; set; }

        public string AccessKey { get; set; }

        public string SecurityKey { get; set; }
    }
}