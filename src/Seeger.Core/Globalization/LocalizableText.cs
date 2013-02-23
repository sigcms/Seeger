using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Seeger.Globalization
{
    public class LocalizableText
    {
        private ResourceDescriptor _resourceDescriptor;

        public string OriginalText { get; private set; }

        public ResourceDescriptor ResourceDescriptor
        {
            get
            {
                if (_resourceDescriptor == null)
                {
                    _resourceDescriptor = ResourceDescriptor.Parse(OriginalText);
                }

                return _resourceDescriptor;
            }
        }

        public bool IsEmpty
        {
            get { return String.IsNullOrEmpty(OriginalText); }
        }

        public LocalizableText(string originalText)
        {
            this.OriginalText = originalText ?? String.Empty;
        }

        public static LocalizableText Empty()
        {
            return new LocalizableText(String.Empty);
        }

        public string Localize()
        {
            return Localize(CultureInfo.CurrentUICulture);
        }

        public string Localize(CultureInfo culture)
        {
            if (IsEmpty)
            {
                return String.Empty;
            }

            if (ResourceDescriptor.IsValid)
            {
                return ResourceDescriptor.Localize(culture);
            }

            return OriginalText;
        }

        public override string ToString()
        {
            return Localize();
        }
    }
}
