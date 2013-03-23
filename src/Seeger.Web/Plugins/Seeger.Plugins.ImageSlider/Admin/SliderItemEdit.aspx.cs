using Seeger.Plugins.ImageSlider.Domain;
using Seeger.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Seeger.Plugins.ImageSlider.Admin
{
    public partial class SliderItemEdit : AdminPageBase
    {
        protected int SliderId
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["sliderId"]);
            }
        }

        protected Slider Slider { get; private set; }

        protected int ItemId
        {
            get
            {
                return Request.QueryString.TryGetValue<int>("itemId", 0);
            }
        }

        public override bool VerifyAccess(Security.User user)
        {
            if (ItemId > 0)
            {
                return user.HasPermission(Strings.PluginName, "ImageSlider", "EditSliderItem");
            }

            return user.HasPermission(Strings.PluginName, "ImageSlider", "AddSliderItem");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Slider = NhSession.Get<Slider>(SliderId);

            if (ItemId > 0 && !IsPostBack)
            {
                var item = Slider.Items.FirstOrDefault(x => x.Id == ItemId);
                Bind(item);
            }
        }

        private void Bind(SliderItem item)
        {
            Caption.Text = item.Caption;
            ImageUrl.Text = item.ImageUrl;
            DisplayOrder.Text = item.DisplayOrder.ToString();
            NavigateUrl.Text = item.NavigateUrl;
        }

        private void Update(SliderItem item)
        {
            item.Caption = Caption.Text.Trim();
            item.ImageUrl = ImageUrl.Text.Trim();
            item.DisplayOrder = Convert.ToInt32(DisplayOrder.Text.Trim());
            item.NavigateUrl = NavigateUrl.Text.Trim();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            if (ItemId > 0)
            {
                var item = Slider.Items.FirstOrDefault(x => x.Id == ItemId);
                Update(item);
            }
            else
            {
                var item = new SliderItem();
                Update(item);
                Slider.Items.Add(item);
            }

            NhSession.Commit();

            Response.Redirect("SliderItems.aspx?sliderId=" + SliderId);
        }
    }
}