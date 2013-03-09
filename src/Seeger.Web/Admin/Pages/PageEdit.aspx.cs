using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Seeger;
using Seeger.Data;
using Seeger.Web;
using Seeger.Web.UI;
using Seeger.Globalization;
using Seeger.Caching;
using Seeger.Security;
using Seeger.Templates;

namespace Seeger.Web.UI.Admin.Pages
{
    public partial class PageEdit : AdminPageBase
    {
        public override bool VerifyAccess(User user)
        {
            string operation = IsEditing ? "Edit" : "Add";
            return user.HasPermission(null, "PageMgnt", operation);
        }

        protected int PageId
        {
            get { return Convert.ToInt32(Request.QueryString["pageid"]); }
        }

        protected int ParentPageId
        {
            get
            {
                int id = 0;
                int.TryParse(Request.QueryString["parentpageid"], out id);
                return id;
            }
        }

        protected bool IsEditing
        {
            get { return PageId > 0; }
        }

        private PageItem _currentPage;
        private PageItem CurrentPage
        {
            get
            {
                if (_currentPage == null && IsEditing)
                {
                    _currentPage = NhSession.Get<PageItem>(PageId);
                }
                return _currentPage;
            }
        }

        private PageItem _parentPage;
        private PageItem ParentPage
        {
            get
            {
                if (_parentPage == null && ParentPageId > 0)
                {
                    _parentPage = NhSession.Get<PageItem>(ParentPageId);
                }
                return _parentPage;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindForm();
            }
        }

        private void BindForm()
        {
            BindBaseUrl();
            BindTemplates();
            BindLayouts();
            BindSkins();

            if (ParentPage != null)
            {
                ParentPageHolder.Visible = true;
                ParentPageName.Text = ParentPage.DisplayName;

                VisibleInMenuHolder.Visible = false;
            }

            if (CurrentPage != null)
            {
                Name.Text = CurrentPage.DisplayName;
                UrlSegment.Text = CurrentPage.UrlSegment;
                BindedDomains.Text = CurrentPage.BindedDomains;
                Published.Checked = CurrentPage.Published;
                VisibleInMenu.Checked = CurrentPage.VisibleInMenu;

                PageUniqueName.Text = CurrentPage.UniqueName;
                Deletable.Checked = CurrentPage.IsDeletable;
            }
        }

        private void BindBaseUrl()
        {
            BaseUrl.Text = "http://" + Request.Url.Host;

            if (Request.Url.Port != 80)
            {
                BaseUrl.Text += ":" + Request.Url.Port;
            }

            if (ParentPage != null)
            {
                BaseUrl.Text += ParentPage.GetPagePath(String.Empty, null);
            }

            BaseUrl.Text += "/";
        }

        private void BindTemplates()
        {
            foreach (var template in TemplateManager.Templates)
            {
                TemplateList.Items.Add(new ListItem(template.DisplayName.Localize(), template.Name));
            }

            if (CurrentPage != null)
            {
                TemplateList.SelectedValue = CurrentPage.Layout.Template.Name;
            }
        }

        private void BindLayouts()
        {
            TemplateLayoutList.DataSource = TemplateManager.Templates;
            TemplateLayoutList.DataBind();

            if (CurrentPage != null)
            {
                CurrentLayout.Text = CurrentPage.Layout.FullName;
            }
        }

        protected void TemplateLayoutList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsDataItem())
            {
                Template template = (Template)e.Item.DataItem;

                Repeater layoutList = (Repeater)e.Item.FindControl("LayoutList");
                layoutList.DataSource = template.Layouts;
                layoutList.DataBind();
            }
        }

        private void BindSkins()
        {
            TemplateSkinList.DataSource = from t in TemplateManager.Templates
                                          where t.Skins.Count > 0
                                          select t;
            TemplateSkinList.DataBind();

            if (CurrentPage != null && CurrentPage.Skin != null)
            {
                CurrentSkin.Text = CurrentPage.Skin.FullName;
            }
        }

        protected void TemplateSkinList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsDataItem())
            {
                Template template = (Template)e.Item.DataItem;

                Repeater themeList = (Repeater)e.Item.FindControl("SkinList");
                themeList.DataSource = template.Skins;
                themeList.DataBind();
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (!IsValid)
            {
                return;
            }

            PageItem page = CurrentPage;
            if (page == null)
            {
                page = new PageItem();
            }

            page.DisplayName = Name.Text.Trim();
            page.UrlSegment = UrlSegment.Text.Trim();
            page.BindedDomains = BindedDomains.Text.Trim();
            page.VisibleInMenu = VisibleInMenu.Checked;
            page.Published = Published.Checked;
            page.UpdateLayout(TemplateManager.FindLayout(CurrentLayout.Text));

            page.UniqueName = PageUniqueName.Text.Trim();
            page.IsDeletable = Deletable.Checked;

            if (CurrentSkin.Text.Length > 0)
            {
                page.Skin = TemplateManager.FindSkin(CurrentSkin.Text);
            }
            else
            {
                page.Skin = null;
            }

            if (!IsEditing)
            {
                if (ParentPage != null)
                {
                    //page.ParentPageId = ParentPage.Id;
                    page.Parent = NhSession.Get<PageItem>(ParentPage.Id);
                    if (ParentPage.Pages.Count > 0)
                    {
                        page.Order = ParentPage.Pages.Max(it => it.Order) + 1;
                    }
                }
                else
                {
                    var pageCache = PageCache.From(NhSession);

                    if (pageCache.RootPages.Any())
                    {
                        page.Order = pageCache.RootPages.Max(it => it.Order) + 1;
                    }
                }
            }

            if (!IsEditing)
            {
                NhSession.Save(page);
            }

            page.ModifiedTime = DateTime.Now;

            NhSession.Commit();

            ClientScript.RegisterStartupScript(this.GetType(), "Success",
                String.Format("onSaved('{0}', {1}, {2}, {3}, '{4}', \"{5}\");",
                    page.DisplayName,
                    page.Id,
                    ParentPageId,
                    CurrentPage == null ? "false" : "true",
                    TreeNode.GetNodeImageUrl(page),
                    new PageItemView(page).ToJson().Replace("\"", "&quot;")
                    ),
                true);
        }

        #region Validation

        protected void UrlSegmentDuplicateValidator_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            var pageCache = PageCache.From(NhSession);

            if (IsEditing)
            {
                e.IsValid = pageCache.FindSiblings(PageId).All(it => !it.UrlSegment.IgnoreCaseEquals(e.Value));
            }
            else
            {
                if (ParentPage != null)
                {
                    e.IsValid = ParentPage.Pages.All(it => !it.UrlSegment.IgnoreCaseEquals(e.Value));
                }
                else
                {
                    e.IsValid = PageCache.From(NhSession).RootPages.All(it => !it.UrlSegment.IgnoreCaseEquals(e.Value));
                }
            }
        }

        #endregion
    }
}