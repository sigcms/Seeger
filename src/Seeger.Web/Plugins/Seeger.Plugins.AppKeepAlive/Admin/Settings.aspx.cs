using Seeger.Web;
using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Plugins.AppKeepAlive.Admin
{
    public partial class Settings : AdminPageBase
    {
        protected bool IsWorkerThreadRunning
        {
            get
            {
                return KeepAliveWorker.IsRunning;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var settings = GlobalSettingManager.Instance;
                Url.Text = settings.GetValue(SettingKeys.Url);
                Interval.Text = settings.TryGetValue(SettingKeys.IntervalInMinutes, Constants.DefaultIntervalInMinutes).ToString();
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            var request = HttpContext.Current.Request;
            var settings = GlobalSettingManager.Instance;

            var url = Url.Text.Trim();
            if (!String.IsNullOrEmpty(url) && !url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = UrlUtil.Combine(request.Url.Scheme + "://" + request.Url.Authority, url);
            }

            var interval = Convert.ToInt32(Interval.Text);

            settings.SetValue(SettingKeys.Url, url);
            settings.SetValue(SettingKeys.IntervalInMinutes, interval.ToString());

            settings.SubmitChanges();

            if (KeepAliveWorker.IsRunning)
            {
                KeepAliveWorker.Url = url;
                KeepAliveWorker.Interval = TimeSpan.FromMinutes(interval);
            }

            ((IMessageProvider)Master).ShowMessage(T("SaveSuccess"), MessageType.Success);
        }

        [WebMethod, ScriptMethod]
        public static void StartWorker()
        {
            if (KeepAliveWorker.IsRunning)
                throw new InvalidOperationException("App keep alive worker thread is already running.");

            var settings = GlobalSettingManager.Instance;
            var url = settings.GetValue(SettingKeys.Url);

            if (String.IsNullOrEmpty(url))
                throw new InvalidOperationException("Cannot start app keep alive worker thread. Url is not set.");

            var interval = settings.TryGetValue<int>(SettingKeys.IntervalInMinutes, Constants.DefaultIntervalInMinutes);

            KeepAliveWorker.Start(url, TimeSpan.FromMinutes(interval));
        }

        [WebMethod, ScriptMethod]
        public static void StopWorker()
        {
            KeepAliveWorker.Stop(true);
        }
    }
}