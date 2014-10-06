using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Seeger.Plugins.Comments
{
    static class DateTimeHumanizer
    {
        public static string Humanize(this DateTime datetime)
        {
            var now = DateTime.Now;
            var offset = now - datetime;
            var culture = CultureInfo.CurrentUICulture;
            var plugin = PluginManager.FindEnabledPlugin(Strings.PluginName);

            if (offset.TotalSeconds < 10)
            {
                return plugin.Localize("Just now", culture);
            }

            if (offset.TotalSeconds < 60)
            {
                return String.Format(plugin.Localize("{0} seconds ago", culture), (int)offset.TotalSeconds);
            }

            if (offset.TotalMinutes < 60)
            {
                return String.Format(plugin.Localize("{0} minutes ago", culture), (int)offset.TotalMinutes);
            }

            if (offset.TotalHours < 24)
            {
                return String.Format(plugin.Localize("{0} hours ago", culture), (int)offset.TotalHours);
            }

            if (offset.TotalDays < 10)
            {
                return String.Format(plugin.Localize("{0} days ago", culture), (int)offset.TotalDays);
            }

            return datetime.ToString("yyyy-MM-dd");
        }
    }
}