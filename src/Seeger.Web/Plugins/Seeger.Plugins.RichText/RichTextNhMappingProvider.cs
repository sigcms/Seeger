﻿using Seeger.Data.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Seeger.Plugins.RichText
{
    public class RichTextNhMappingProvider : Seeger.Data.INhMappingProvider
    {
        public IEnumerable<NHibernate.Cfg.MappingSchema.HbmMapping> GetMappings()
        {
            yield return ByCodeMappingLoader.LoadMappingFrom(Assembly.GetExecutingAssembly());
        }
    }
}