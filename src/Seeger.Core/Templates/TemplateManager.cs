using Seeger.Plugins;
using Seeger.Templates;
using Seeger.Templates.Loaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace Seeger.Templates
{
    public static class TemplateManager
    {
        private static Dictionary<string, Template> _templates;

        public static bool WasInitialized { get; private set; }

        public static IEnumerable<Template> Templates
        {
            get
            {
                return _templates.Values;
            }
        }

        public static Template FindTemplate(string templateName)
        {
            Template template;
            _templates.TryGetValue(templateName, out template);
            return template;
        }

        public static Layout FindLayout(string fullName)
        {
            var indexOfDot = fullName.LastIndexOf('.');
            var templateName = fullName.Substring(0, indexOfDot);
            var layoutName = fullName.Substring(indexOfDot + 1);

            var template = FindTemplate(templateName);
            return template.FindLayout(layoutName);
        }

        public static TemplateSkin FindSkin(string fullName)
        {
            var indexOfDot = fullName.LastIndexOf('.');
            var templateName = fullName.Substring(0, indexOfDot);
            var layoutName = fullName.Substring(indexOfDot + 1);

            var template = FindTemplate(templateName);
            return template.FindSkin(layoutName);
        }

        /// <summary>
        /// Initialize the plugin manager. 
        /// This method will be invoked at PreApplicationStart stage in the main web application. 
        /// It's not intended to be used from your code.
        /// </summary>
        public static void Initialize()
        {
            if (WasInitialized)
                throw new InvalidOperationException("TemplateManager already initialized.");

            var templates = new Dictionary<string, Template>();
            var loader = TemplateLoaders.Current();

            var rootDirectory = new DirectoryInfo(HostingEnvironment.MapPath("/Templates"));
            if (rootDirectory.Exists)
            {
                foreach (var templateDirectory in rootDirectory.GetDirectories())
                {
                    if (templateDirectory.IsHidden()) continue;

                    var templateName = templateDirectory.Name;

                    var binDirectoryPath = Path.Combine(templateDirectory.FullName, "bin");
                    if (Directory.Exists(binDirectoryPath))
                    {
                        DeployTemplateAssemblies(templateName, binDirectoryPath);
                    }

                    var template = loader.Load(templateName);
                    templates.Add(template.Name, template);
                }
            }

            _templates = templates;
            WasInitialized = true;
        }

        static void DeployTemplateAssemblies(string templateName, string templateSourceBinDirectory)
        {
            var targetDirectory = HostingEnvironment.MapPath("/App_Data/Assemblies/Templates/" + templateName);
            Directory.CreateDirectory(targetDirectory);

            AssemblyDeployer.ClearAssemblies(targetDirectory);
            AssemblyDeployer.DeployAssemblies(templateSourceBinDirectory, targetDirectory);
        }
    }
}
