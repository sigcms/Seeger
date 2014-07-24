using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Seo
{
    public class SitemapProviderCollection : Collection<ISitemapProvider>
    {
        public bool Remove<T>()
            where T : ISitemapProvider
        {
            var item = Items.FirstOrDefault(it => it.GetType() == typeof(T));
            if (item != null)
            {
                Items.Remove(item);
                return true;
            }

            return false;
        }
    }
}