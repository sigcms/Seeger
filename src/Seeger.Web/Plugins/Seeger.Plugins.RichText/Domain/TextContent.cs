using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Seeger.Globalization;

namespace Seeger.Plugins.RichText.Domain
{
    public class TextContent : ILocalizableEntity
    {
        [EntityKey]
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Shared { get; set; }
        public virtual string Content { get; set; }
        public virtual DateTime CreatedTime { get; set; }

        public TextContent()
        {
            Name = String.Empty;
            Content = String.Empty;
            CreatedTime = DateTime.Now;
        }
    }
}
