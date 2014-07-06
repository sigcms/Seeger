using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Web.Http
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AreaAttribute : Attribute
    {
        public string Name { get; private set; }

        public AreaAttribute(string name)
        {
            Name = name;
        }
    }
}
