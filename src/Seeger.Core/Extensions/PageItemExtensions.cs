using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Seeger
{
   public static class PageItemExtensions
    {
       public static SEOInfo GetSeoInfo(this PageItem page, bool inheritParentSeoSettings = false)
       {
           return GetSeoInfo(page, null, inheritParentSeoSettings);
       }

       public static SEOInfo GetSeoInfo(this PageItem page, CultureInfo culture, bool inheritParentSeoSettings = false)
       {
           var seo = new SEOInfo();

           if (culture != null)
           {
               seo.PageTitle = page.GetLocalized(x => x.PageTitle, culture);
               seo.MetaKeywords = page.GetLocalized(x => x.MetaKeywords, culture);
               seo.MetaDescription = page.GetLocalized(x => x.MetaDescription, culture);
           }
           else
           {
               seo.PageTitle = page.PageTitle;
               seo.MetaKeywords = page.MetaKeywords;
               seo.MetaDescription = page.MetaDescription;
           }

           if (inheritParentSeoSettings && page.Parent != null)
           {
               var parentSeo = page.Parent.GetSeoInfo(culture, true);
               seo.Merge(parentSeo);
           }

           return seo;
       }
    }
}
