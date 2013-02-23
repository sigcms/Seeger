using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Globalization;

using NHibernate;

using Seeger.Data;
using Seeger.Globalization;
using Seeger.Services;
using Seeger.Plugins.Widgets;
using Seeger.Templates;
using Seeger.Collections;

namespace Seeger
{
    public class PageItem : ILocalizableEntity, ITreeNode<PageItem>
    {
        [EntityKey]
        public virtual int Id { get; set; }
        public virtual string UniqueName { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string UrlSegment { get; set; }
        public virtual string BindedDomains { get; set; }
        public virtual int Order { get; set; }
        public virtual DateTime CreatedTime { get; set; }
        public virtual DateTime ModifiedTime { get; set; }
        public virtual bool VisibleInMenu { get; set; }
        public virtual bool Published { get; set; }
        public virtual bool IsDeletable { get; set; }

        public virtual string MenuText { get; set; }
        public virtual string PageTitle { get; set; }
        public virtual string MetaKeywords { get; set; }
        public virtual string MetaDescription { get; set; }

        public virtual PageItem Parent { get; set; }
        public virtual IList<WidgetInPage> WidgetInPages { get; protected set; }
        public virtual EntityAttributeCollection Attributes { get; protected set; }

        private IList<PageItem> _nhChildPages = new List<PageItem>();
        private PageItemCollection _pages;

        public virtual PageItemCollection Pages
        {
            get
            {
                if (_pages == null)
                {
                    _pages = new PageItemCollection(this, _nhChildPages);
                }
                return _pages;
            }
        }

        public PageItem()
        {
            IsDeletable = true;

            MenuText = String.Empty;
            PageTitle = String.Empty;
            MetaKeywords = String.Empty;
            MetaDescription = String.Empty;

            WidgetInPages = new List<WidgetInPage>();

            VisibleInMenu = true;
            CreatedTime = DateTime.Now;
            ModifiedTime = CreatedTime;
        }

        public virtual bool HasBindedDomain(string domain)
        {
            return !String.IsNullOrEmpty(BindedDomains) && BindedDomains.Contains(domain);
        }

        #region Layout

        private string _layoutFullName;
        private Layout _layout;

        public virtual Layout Layout
        {
            get
            {
                if (_layout == null)
                {
                    _layout = TemplateManager.FindLayout(_layoutFullName);
                }
                return _layout;
            }
        }

        public virtual void UpdateLayout(Layout layout)
        {
            Require.NotNull(layout, "layout");

            if (!String.IsNullOrEmpty(_layoutFullName) && !_layoutFullName.IgnoreCaseEquals(layout.FullName))
            {
                var oldLayout = TemplateManager.FindLayout(_layoutFullName);
                if (oldLayout != null)
                {
                    List<Zone> zonesToClear = new List<Zone>();

                    foreach (var zone in oldLayout.Zones)
                    {
                        if (!layout.ContainsZone(zone.Name))
                        {
                            zonesToClear.Add(zone);
                        }
                    }

                    if (zonesToClear.Count > 0)
                    {
                        foreach (var zone in zonesToClear)
                        {
                            RemoveWidgets(zone.Name);
                        }
                    }
                }
            }

            _layout = layout;
            _layoutFullName = layout.FullName;
        }

        #endregion

        #region Skin

        private string _skinFullName;
        private TemplateSkin _skin;

        public virtual TemplateSkin Skin
        {
            get
            {
                if (_skin == null && !String.IsNullOrEmpty(_skinFullName))
                {
                    _skin = TemplateManager.FindSkin(_skinFullName);
                }
                return _skin;
            }
            set
            {
                _skin = value;
                if (_skin != null)
                {
                    _skinFullName = _skin.FullName;
                }
                else
                {
                    _skinFullName = String.Empty;
                }
            }
        }

        #endregion

        #region Widget Settings

        public virtual void RemoveWidgets(string zoneName)
        {
            Require.NotNullOrEmpty(zoneName, "zoneName");

            foreach (var w in WidgetInPages.Where(w => w.ZoneName == zoneName).ToList())
            {
                WidgetInPages.Remove(w);
            }
        }

        public virtual WidgetInPage AddWidget(Zone zone, WidgetDefinition widget)
        {
            int order = 0;
            if (this.WidgetInPages.Count > 0)
            {
                order = this.WidgetInPages.Max(it => it.Order) + 5;
            }
            return AddWidget(zone, widget, order);
        }

        public virtual WidgetInPage AddWidget(Zone zone, WidgetDefinition widget, int displayOrder)
        {
            Require.NotNull(zone, "zone");
            Require.NotNull(widget, "widget");

            WidgetInPage setting = new WidgetInPage(this)
            {
                PluginName = widget.Plugin.Name,
                WidgetName = widget.Name,
                ZoneName = zone.Name,
                Order = displayOrder
            };
            this.WidgetInPages.Add(setting);

            return setting;
        }

        public virtual WidgetInPage GetWidget(int widgetInPageId)
        {
            if (widgetInPageId < 0)
            {
                return null;
            }
            return this.WidgetInPages.FirstOrDefault(it => it.Id == widgetInPageId);
        }

        public virtual IEnumerable<WidgetInPage> GetWidgets(string zoneName)
        {
            return WidgetInPages.Where(it => it.ZoneName.IgnoreCaseEquals(zoneName));
        }

        #endregion

        public virtual void MoveTo(PageItem destination, DropPosition position)
        {
            if (destination == null && position != DropPosition.Over)
            {
                throw new InvalidOperationException();
            }

            PageItemCollection rootPageItems = PageService.GetRootPages();

            if (destination == null)
            {
                rootPageItems.AddLast(this);
            }
            else
            {
                if (position == DropPosition.Over)
                {
                    destination.Pages.AddLast(this);
                }
                else if (position == DropPosition.Before)
                {
                    var collection = destination.Parent == null ? rootPageItems : destination.Parent.Pages;

                    if (this.IsSiblingOf(destination))
                    {
                        collection.MoveBefore(this, destination);
                    }
                    else
                    {
                        collection.AddBefore(this, destination);
                    }
                }
                else if (position == DropPosition.After)
                {
                    var collection = destination.Parent == null ? rootPageItems : destination.Parent.Pages;

                    if (this.IsSiblingOf(destination))
                    {
                        collection.MoveAfter(this, destination);
                    }
                    else
                    {
                        collection.AddAfter(this, destination);
                    }
                }
            }
        }

        public virtual bool IsSiblingOf(PageItem other)
        {
            if (other == null)
            {
                return false;
            }

            if (Parent == null && other.Parent == null)
            {
                return true;
            }

            if (Parent != null && other.Parent != null && Parent.Id == other.Parent.Id)
            {
                return true;
            }

            return false;
        }

        public virtual PageItem FindDecendant(Func<PageItem, bool> predicate)
        {
            return this.BreadthFirstSearch(false, predicate);
        }

        public virtual string GetFinalPagePath()
        {
            var frontendSettings = GlobalSettingManager.Instance.FrontendSettings;

            if (frontendSettings.Multilingual)
            {
                return GetPagePath(CultureInfo.CurrentCulture, frontendSettings.PageExtension);
            }

            return GetPagePath(String.Empty, frontendSettings.PageExtension);
        }

        public virtual string GetPagePath(CultureInfo culture, string extension)
        {
            return GetPagePath(culture == null ? null : culture.Name, extension);
        }

        public virtual string GetPagePath(string cultureName, string extension = null)
        {
            return CmsVirtualPath.GetFull(GetPagePathRelativeToCmsRoot(cultureName, extension));
        }

        public virtual string GetPagePathRelativeToCmsRoot(string cultureName, string extension = null)
        {
            var pagePath = ReCalculatePagePath();
            var path = pagePath + (extension ?? String.Empty);
            if (!String.IsNullOrEmpty(cultureName))
            {
                path = "/" + cultureName + path;
            }

            return path;
        }

        private string ReCalculatePagePath()
        {
            string path = this.UrlSegment;
            var current = this;
            while (current.Parent != null)
            {
                path = current.Parent.UrlSegment + "/" + path;
                current = current.Parent;
            }

            path = "/" + path;

            return path;
        }

        public override string ToString()
        {
            return String.Format("Id:{0}, Name:{1}", Id, DisplayName ?? String.Empty);
        }

        IEnumerable<PageItem> ITreeNode<PageItem>.Children
        {
            get { return Pages; }
        }
    }
}
