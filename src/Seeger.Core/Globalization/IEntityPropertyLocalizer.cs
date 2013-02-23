using Seeger.Data;
using Seeger.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Seeger.Globalization
{
    public interface IEntityPropertyLocalizer
    {
        string GetLocalizedValue(string entityType, string entityKey, string propertyPath, CultureInfo culture);

        NameValueCollection GetLocalizedValues(string entityType, string entityKey, CultureInfo culture);

        void SetLocalizedValue(string entityType, string entityKey, string propertyPath, object propertyValue, CultureInfo culture);
    }

    public static class EntityPropertyLocalizerExtensions
    {
        public static string GetLocalizedValue<TEntity, TProperty>(this IEntityPropertyLocalizer localizer,
            TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression, CultureInfo culture)
            where TEntity : ILocalizableEntity
        {
            var entityType = typeof(TEntity).FullName;
            var propertyPath = PropertyPathBuilder.BuildPropertyPath(propertyExpression);
            var entityKey = EntityKeyManager.GetEntityKey(entity);

            if (entityKey == null)
                throw new InvalidOperationException("Cannot get localized property value because entity key is null. Entity type: " + entityType);

            return localizer.GetLocalizedValue(entityType, entityKey.ToString(), propertyPath, culture);
        }

        public static void SetLocalizedValue<TEntity, TProperty>(this IEntityPropertyLocalizer localizer, 
            TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression, object propertyValue, CultureInfo culture)
            where TEntity : ILocalizableEntity
        {
            var entityType = typeof(TEntity).FullName;
            var propertyPath = PropertyPathBuilder.BuildPropertyPath(propertyExpression);
            var entityKey = EntityKeyManager.GetEntityKey(entity);

            if (entityKey == null)
                throw new InvalidOperationException("Cannot set localized property value because entity key is null. Entity type: " + entityType);

            localizer.SetLocalizedValue(entityType, entityKey.ToString(), propertyPath, propertyValue, culture);
        }
    }

    public static class EntityPropertyLocalizers
    {
        public static Func<IEntityPropertyLocalizer> Current = () => new DefaultEntityPropertyLocalizer(NhSessionManager.GetCurrentSession());
    }
}
