using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    [Serializable]
    public class PluginNotFoundException : Exception
    {
        public string PluginName { get; private set; }

        public PluginNotFoundException() { }

        public PluginNotFoundException(string pluginName) 
            : base(String.Format("Module '{0}' was not found.", pluginName)) 
        { 
        }

        public PluginNotFoundException(string message, Exception inner) : base(message, inner) { }
        
        protected PluginNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
