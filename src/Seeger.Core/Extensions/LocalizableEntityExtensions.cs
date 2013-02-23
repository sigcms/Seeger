using Seeger.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Seeger
{
    public static class LocalizableEntityExtensions
    {
        public static string GetLocalized<TEntity, TProperty>(this TEntity entity, Expression<Func<TEntity, TProperty>> property)
            where TEntity : ILocalizableEntity
        {
            return GetLocalized<TEntity, TProperty>(entity, property, CultureInfo.CurrentCulture);
        }

        public static string GetLocalized<TEntity, TProperty>(this TEntity entity, Expression<Func<TEntity, TProperty>> property, CultureInfo culture)
            where TEntity : ILocalizableEntity
        {
            var localizer = EntityPropertyLocalizers.Current();
            return localizer.GetLocalizedValue<TEntity, TProperty>(entity, property, culture);
        }

        public static void SetLocalized<TEntity, TProperty>(this TEntity entity, Expression<Func<TEntity, TProperty>> property, object propertyValue)
            where TEntity : ILocalizableEntity
        {
            SetLocalized<TEntity, TProperty>(entity, property, propertyValue, CultureInfo.CurrentCulture);
        }

        public static void SetLocalized<TEntity, TProperty>(this TEntity entity, Expression<Func<TEntity, TProperty>> property, object propertyValue, CultureInfo culture)
            where TEntity : ILocalizableEntity
        {
            var localizer = EntityPropertyLocalizers.Current();
            localizer.SetLocalizedValue<TEntity, TProperty>(entity, property, propertyValue, culture);
        }
    }
}
