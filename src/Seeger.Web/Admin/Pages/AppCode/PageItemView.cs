using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

using Newtonsoft.Json;

using Seeger.Caching;

namespace Seeger.Web.UI.Admin
{
    public class PageItemView
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string UrlSegment { get; set; }
        public string Layout { get; set; }
        public string LayoutPreviewImage { get; set; }
        public string Skin { get; set; }
        public string SkinPreviewImage { get; set; }
        public string DesignerPath { get; set; }
        public string PagePath { get; set; }
        public bool IsDeletable { get; set; }

        public string CreatedTime { get; set; }
        public string LastModifiedTime { get; set; }

        public PageItemView()
        {
        }

        public PageItemView(PageItem page)
        {
            Require.NotNull(page, "page");

            Id = page.Id;
            DisplayName = page.DisplayName;
            UrlSegment = page.UrlSegment;
            Layout = page.Layout.DisplayName.Localize();
            LayoutPreviewImage = page.Layout.PreviewImageVirtualPath;
            DesignerPath = page.Layout.DesignerPath;
            PagePath = page.GetPagePath();
            IsDeletable = page.IsDeletable;
            Skin = String.Empty;
            SkinPreviewImage = String.Empty;

            if (page.Skin != null)
            {
                Skin = page.Skin.DisplayName.Localize();
                SkinPreviewImage = page.Skin.PreviewImageVirtualPath;
            }

            CreatedTime = page.CreatedTime.ToString(CultureInfo.CurrentCulture);
            LastModifiedTime = page.ModifiedTime.ToString(CultureInfo.CurrentCulture);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}