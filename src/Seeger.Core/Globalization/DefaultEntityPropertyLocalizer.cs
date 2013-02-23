using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Seeger.Globalization
{
    public class DefaultEntityPropertyLocalizer : IEntityPropertyLocalizer
    {
        private ISession _session;

        public DefaultEntityPropertyLocalizer(ISession session)
        {
            Require.NotNull(session, "session");
            _session = session;
        }

        public string GetLocalizedValue(string entityType, string entityKey, string propertyPath, CultureInfo culture)
        {
            var localization = GetPropertyLocalization(entityType, entityKey, propertyPath, culture);
            return localization == null ? null : localization.PropertyValue;
        }

        public NameValueCollection GetLocalizedValues(string entityType, string entityKey, CultureInfo culture)
        {
            var localizations = _session.Query<EntityPropertyLocalization>()
                                        .Where(x => x.EntityType == entityType && x.EntityKey == entityKey && x.Culture == culture.Name)
                                        .ToList();

            var nameValues = new NameValueCollection();

            foreach (var localization in localizations)
            {
                nameValues[localization.PropertyValue] = localization.PropertyValue;
            }

            return nameValues;
        }

        public void SetLocalizedValue(string entityType, string entityKey, string propertyPath, object propertyValue, CultureInfo culture)
        {
            var localization = GetPropertyLocalization(entityType, entityKey, propertyPath, culture);
            if (localization == null)
            {
                localization = new EntityPropertyLocalization
                {
                    EntityType = entityType,
                    EntityKey = entityKey,
                    PropertyPath = propertyPath,
                    Culture = culture.Name,
                    PropertyValue = propertyValue.AsString()
                };
                _session.Save(localization);
            }

            localization.PropertyValue = propertyValue.AsString();
        }

        private EntityPropertyLocalization GetPropertyLocalization(string entityType, string entityKey, string propertyPath, CultureInfo culture)
        {
            return _session.Query<EntityPropertyLocalization>()
                           .FirstOrDefault(x => x.EntityType == entityType && x.EntityKey == entityKey && x.PropertyPath == propertyPath && x.Culture == culture.Name);
        }
    }
}
