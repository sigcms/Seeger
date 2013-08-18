using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Compilation;
using System.Web.Hosting;
using Seeger.Logging;

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
            _log.Debug(UserReference.System(), "Deploying assemblies. From: " + sourceDirectoryPath + " to " + targetDirectoryPath);

            var sourceDirectory = new DirectoryInfo(sourceDirectoryPath);

            if (!sourceDirectory.Exists)
            {
                return Enumerable.Empty<Assembly>();
            }

            Directory.CreateDirectory(targetDirectoryPath);

            var assemblies = new List<Assembly>();

            foreach (var sourceFile in sourceDirectory.GetFiles("*.*", SearchOption.TopDirectoryOnly))
            {
                if (sourceFile.Extension != ".dll" && sourceFile.Extension != ".pdb") continue;

                var targetFile = new FileInfo(Path.Combine(targetDirectoryPath, sourceFile.Name));

                if (File.Exists(Path.Combine(RootBinDirectoryPath, targetFile.Name))) continue;

                if (!targetFile.Exists | targetFile.LastWriteTimeUtc != sourceFile.LastWriteTimeUtc)
                {
                    _log.Debug(UserReference.System(), "Copying file to probing folder. Assembly file name: " + sourceFile.Name);

                    try
                    {
                        File.Copy(sourceFile.FullName, targetFile.FullName, true);
                    }
                    catch (IOException ex)
                    {
                        _log.DebugException(UserReference.System(), ex, "Directly copy file failed. Tring rename the existing file to " + sourceFile.Name + ".old.");

                        var oldFile = targetFile.FullName + ".old";
                        File.Move(targetFile.FullName, oldFile);
                        File.Copy(sourceFile.FullName, targetFile.FullName, true);
                    }
                }

                if (sourceFile.Extension == ".dll")
                {
                    var asm = Assembly.Load(AssemblyName.GetAssemblyName(targetFile.FullName));
                    BuildManager.AddReferencedAssembly(asm);

                    assemblies.Add(asm);
                }
            }

            return assemblies;
        }
    }
}
