using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Seeger.Plugins.RichText.Domain
{
    public class TextContentLocalization
    {
        public virtual int Id { get; set; }

        protected virtual TextContent Owner { get; set; }

        public virtual string Culture { get; set; }
        public virtual string Content { get; set; }

        protected TextContentLocalization() { }

        public TextContentLocalization(TextContent content, CultureInfo culture) 
        {
            Require.NotNull(content, "content");
            Require.NotNull(culture, "culture");

            Content = String.Empty;

            Owner = content;
            Culture = culture.Name;
        }
    }

}
