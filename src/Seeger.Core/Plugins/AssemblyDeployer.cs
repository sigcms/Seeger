using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Compilation;
using System.Web.Hosting;

namespace Seeger.Plugins
{
    static class AssemblyDeployer
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public static readonly string RootBinDirectoryPath = HostingEnvironment.MapPath("/bin");

        public static void ClearAssemblies(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                foreach (var dll in Directory.GetFiles(directoryPath))
                {
                    File.Delete(dll);
                }
            }
        }

        public static IEnumerable<Assembly> DeployAssemblies(string sourceDirectoryPath, string targetDirectoryPath)
        {
            _log.Debug("Deploying assemblies. From: " + sourceDirectoryPath + " to " + targetDirectoryPath);

            var sourceDirectory = new DirectoryInfo(sourceDirectoryPath);

            if (!sourceDirectory.Exists)
            {
                return Enumerable.Empty<Assembly>();
            }

            Directory.CreateDirectory(targetDirectoryPath);

            var assemblies = new List<Assembly>();

            foreach (var sourceDll in sourceDirectory.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
            {
                var targetDll = new FileInfo(Path.Combine(targetDirectoryPath, sourceDll.Name));

                if (File.Exists(Path.Combine(RootBinDirectoryPath, targetDll.Name)))
                {
                    continue;
                }

                if (!targetDll.Exists | targetDll.LastWriteTimeUtc != sourceDll.LastWriteTimeUtc)
                {
                    _log.Debug("Copying assembly file to probing folder. Assembly file name: " + sourceDll.Name);

                    try
                    {
                        File.Copy(sourceDll.FullName, targetDll.FullName, true);
                    }
                    catch (IOException ex)
                    {
                        _log.DebugException("Directly copy assembly file failed. Tring rename the existing assembly file to xxx.dll.old.", ex);

                        var oldFile = targetDll.FullName + ".old";
                        File.Move(targetDll.FullName, oldFile);
                        File.Copy(sourceDll.FullName, targetDll.FullName, true);
                    }
                }

                var asm = Assembly.Load(AssemblyName.GetAssemblyName(targetDll.FullName));
                BuildManager.AddReferencedAssembly(asm);

                assemblies.Add(asm);
            }

            return assemblies;
        }
    }
}
