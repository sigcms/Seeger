using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Seeger.Plugins.Analytics
{
    public class AnalyticsPageLifecycleInterceptor : EmptyPageLifecycleInterceptor
    {
        public override void OnPreRender(Page page)
        {
            var layoutPage = page as LayoutPageBase;

            if (layoutPage == null || layoutPage.IsInDesignMode) return;

            var analyticsCode = GetAnalyticsCode();

            if (String.IsNullOrEmpty(analyticsCode)) return;

            if (page.Form != null)
            {
                // find possible holder
                var holder = page.Form.FindControl("AnalyticsCodeHolder");

                if (holder == null)
                {
                    holder = page.Form;
                }

                holder.Controls.Add(new LiteralControl
                {
                    Text = analyticsCode
                });
            }
        }

        private string GetAnalyticsCode()
        {
            var settings = GlobalSettingManager.Instance;
            var enableAnalyticsCode = settings.TryGetValue<bool>(SettingKeys.EnableAnalyticsCode, false);
            var analyticsCode = settings.GetValue(SettingKeys.AnalyticsCode);

            if (enableAnalyticsCode && !String.IsNullOrEmpty(analyticsCode))
            {
                return analyticsCode;
            }

            return null;
        }
    }
}