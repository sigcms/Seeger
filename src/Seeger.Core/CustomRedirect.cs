using Seeger.Data.Mapping;
using Seeger.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Seeger
{
    [Entity, Cache]
    public class CustomRedirect
    {
        [EntityKey]
        public virtual int Id { get; protected set; }

        public virtual string From { get; set; }

        public virtual string To { get; set; }

        public virtual string Description { get; set; }

        public virtual int Priority { get; set; }

        public virtual bool MatchByRegex { get; set; }

        public virtual RedirectMode RedirectMode { get; set; }

        public virtual bool IsEnabled { get; set; }

        public virtual DateTime UtcCreatedTime { get; set; }

        public CustomRedirect()
        {
            IsEnabled = true;
            UtcCreatedTime = DateTime.UtcNow;
        }
    }

}
