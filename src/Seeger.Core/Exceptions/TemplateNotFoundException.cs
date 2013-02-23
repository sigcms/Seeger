using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    [Serializable]
    public class TemplateNotFoundException : Exception
    {
        public string TemplateName { get; private set; }

        public TemplateNotFoundException() { }

        public TemplateNotFoundException(string templateName) 
            : base(String.Format("Template '{0}' was not found.", templateName)) 
        { 
        }

        public TemplateNotFoundException(string message, Exception inner) 
            : base(message, inner) 
        { 
        }

        protected TemplateNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
