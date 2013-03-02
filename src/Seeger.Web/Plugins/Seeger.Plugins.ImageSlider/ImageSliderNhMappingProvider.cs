using NHibernate.Cfg.MappingSchema;
using Seeger.Data;
using Seeger.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Seeger.Plugins.ImageSlider
{
    public class ImageSliderNhMappingProvider : INhMappingProvider
    {
        public IEnumerable<HbmMapping> GetMappings()
        {
            yield return ByCodeMappingLoader.LoadMappingFrom(Assembly.GetExecutingAssembly());
        }
    }
}