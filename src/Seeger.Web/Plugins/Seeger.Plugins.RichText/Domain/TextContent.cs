using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Seeger.Globalization;
using Seeger.Data.Mapping;

namespace Seeger.Plugins.RichText.Domain
{
    [Class]
    public class TextContent : ILocalizableEntity
    {
        [EntityKey, HiloId]
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        [StringClob]
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
