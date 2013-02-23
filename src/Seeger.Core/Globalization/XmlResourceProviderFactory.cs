using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Compilation;

namespace Seeger.Globalization
{
    public class XmlResourceProviderFactory : ResourceProviderFactory
    {
        public override IResourceProvider CreateGlobalResourceProvider(string classKey)
        {
            return new XmlResourceProvider();
        }

        public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
        {
            return new XmlResourceProvider();
        }
    }
}
