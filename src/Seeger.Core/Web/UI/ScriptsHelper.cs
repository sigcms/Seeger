using Newtonsoft.Json;
using Seeger.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Seeger.Web.UI
{
    public class ScriptsHelper
    {
        public HttpContextBase Context { get; private set; }

        public ScriptsHelper(HttpContext context)
            : this(new HttpContextWrapper(context))
        {
        }

        public ScriptsHelper(HttpContextBase context)
        {
            Context = context;
        }

        public string Render(string javascript)
        {
            return "<script type\"text/javascript\">" + Environment.NewLine + javascript + Environment.NewLine + "</script>";
        }

        public string GlobalResourcesInitialization()
        {
            return GlobalResourcesInitialization(CultureInfo.CurrentUICulture);
        }

        public string GlobalResourcesInitialization(CultureInfo culture)
        {
            return ResourcesInitialization(ResourcesFolder.Global.GetResourceDictionary(culture));
        }

        public string ResourcesInitialization(IDictionary<string, string> resourceKeyValueMap)
        {
            return Render("sig.GlobalResources.init(" + JsonConvert.SerializeObject(resourceKeyValueMap) + ");");
        }
    }
}
