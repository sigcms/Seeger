using System;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;

namespace Seeger.Web.Http
{
    public abstract class HttpAreaRegistration
    {
        public abstract string AreaName { get; }

        public abstract void RegisterArea(HttpAreaRegistrationContext context);

        public static void RegisterAllAreas()
        {
            foreach (var asm in BuildManager.GetReferencedAssemblies().OfType<Assembly>())
            {
                foreach (var type in asm.GetExportedTypes())
                {
                    if (!type.IsAbstract && typeof(HttpAreaRegistration).IsAssignableFrom(type))
                    {
                        var registration = (HttpAreaRegistration)Activator.CreateInstance(type) as HttpAreaRegistration;
                        registration.RegisterArea(new HttpAreaRegistrationContext(registration.AreaName, GlobalConfiguration.Configuration.Routes));
                    }
                }
            }
        }
    }
}
