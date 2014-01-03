using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.ComponentModel
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class StringClobAttribute : Attribute
    {
    }
}
