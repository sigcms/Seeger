using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.QiniuCloud
{
    [Serializable]
    public class QiniuException : Exception
    {
        public QiniuException() { }
        public QiniuException(string message) : base(message) { }
        public QiniuException(string message, Exception inner) : base(message, inner) { }
        protected QiniuException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}