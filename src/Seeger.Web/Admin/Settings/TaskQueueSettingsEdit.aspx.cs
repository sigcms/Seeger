using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Web.UI.Admin.Settings
{
    public partial class TaskQueueSettingsEdit : BackendPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var setting = GlobalSettingManager.Instance.TaskQueue;

                Interval.Text = setting.IntervalInMinutes.ToString();
                Enable.Checked = setting.Enabled;
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var manager = GlobalSettingManager.Instance;

            manager.TaskQueue.IntervalInMinutes = Convert.ToInt32(Interval.Text);
            manager.TaskQueue.Enabled = Enable.Checked;

            manager.SubmitChanges();

            ((IMessageProvider)Master).ShowMessage(Localize("Message.SaveSuccess"), MessageType.Success);
        }
    }
}