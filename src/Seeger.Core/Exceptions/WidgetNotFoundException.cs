using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    [Serializable]
    public class WidgetNotFoundException : Exception
    {
        public string WidgetName { get; private set; }

        public WidgetNotFoundException() { }

        public WidgetNotFoundException(string widgetName) 
            : base(String.Format("Widget '{0}' was not found.", widgetName)) 
        {
        }

        public WidgetNotFoundException(string message, Exception inner) 
            : base(message, inner) 
        { 
        }

        protected WidgetNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) 
        { 
        }
    }
}
