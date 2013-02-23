using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Seeger.Plugins;
using Seeger.Templates;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Seeger.Web.AppCode.PreApplicationStart), "Initialize")]

namespace Seeger.Web.AppCode
{
    public static class PreApplicationStart
    {
        public static void Initialize()
        {
            PluginManager.Initialize();
            TemplateManager.Initialize();
        }
    }
}