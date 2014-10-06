using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Comments
{
    public class Subject
    {
        public string Type { get; set; }

        public string Id { get; set; }

        public string Title { get; set; }

        public Subject()
        {
            Type = "Default";
        }
    }
}