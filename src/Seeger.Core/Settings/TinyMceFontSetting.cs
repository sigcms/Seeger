using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Seeger
{
    public class TinyMceFontSetting
    {
        public string Culture { get; private set; }
        public string Fonts { get; private set; }

        public TinyMceFontSetting(string culture, string fonts)
        {
            Require.NotNullOrEmpty(culture, "culture");
            Require.NotNullOrEmpty(fonts, "fonts");

            this.Culture = culture;
            this.Fonts = fonts;
        }

        public static TinyMceFontSetting From(XElement xml)
        {
            return new TinyMceFontSetting(
                xml.AttributeValue("culture"),
                xml.AttributeValue("fonts")
            );
        }

        public override string ToString()
        {
            return Culture + ": " + Fonts;
        }
    }
}
