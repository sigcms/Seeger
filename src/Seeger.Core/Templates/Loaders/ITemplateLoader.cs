using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Templates.Loaders
{
    public interface ITemplateLoader
    {
        Template Load(string templateName);
    }

    public static class TemplateLoaders
    {
        public static Func<ITemplateLoader> Current = () => new DefaultTemplateLoader();
    }
}
