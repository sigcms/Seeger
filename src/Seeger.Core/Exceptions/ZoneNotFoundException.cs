using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger
{
    [Serializable]
    public class ZoneNotFoundException : Exception
    {
        public string ZoneName { get; private set; }

        public ZoneNotFoundException() { }

        public ZoneNotFoundException(string blockName) 
            : base(String.Format("Zone '{0}' was not found.", blockName)) 
        {
        }

        public ZoneNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ZoneNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
