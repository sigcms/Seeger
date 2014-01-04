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
using Seeger.Data.Mapping;

namespace Seeger
{
    [Class]
    public class PageItem : ILocalizableEntity, ITreeNode<PageItem>
    {
        [EntityKey, HiloId]
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
        public virtual IList<LocatedWidget> LocatedWidgets { get; protected set; }
        public virtual EntityAttributeCollection Attributes { get; protected set; }
        public virtual IList<PageItem> Pages { get; protected set; }

        public PageItem()
            : this(null)
        {
        }

        public PageItem(PageItem parent)
        {
            IsDeletable = true;

            Parent = parent;

            MenuText = String.Empty;
            PageTitle = String.Empty;
            MetaKeywords = String.Empty;
            MetaDescription = String.Empty;

            LocatedWidgets = new List<LocatedWidget>();
            Pages = new List<PageItem>();

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

        [NotMapped]
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
                            RemoveLocatedWidgetsByZone(zone.Name);
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
        
        [NotMapped]
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

        public virtual IEnumerable<LocatedWidget> FindLocatedWidgetsByZone(string zoneName)
        {
            return LocatedWidgets.Where(it => it.ZoneName == zoneName).OrderBy(it => it.Order);
        }

        public virtual void RemoveLocatedWidgetsByZone(string zoneName)
        {
            Require.NotNullOrEmpty(zoneName, "zoneName");

            foreach (var w in FindLocatedWidgetsByZone(zoneName).ToList())
            {
                LocatedWidgets.Remove(w);
            }
        }

        public virtual LocatedWidget AddWidgetToZone(Zone zone, WidgetDefinition widget)
        {
            var order = 0;

            if (LocatedWidgets.Count > 0)
            {
                order = LocatedWidgets.Max(it => it.Order) + 5;
            }

            return AddWidgetToZone(zone, widget, order);
        }

        public virtual LocatedWidget AddWidgetToZone(Zone zone, WidgetDefinition widget, int displayOrder)
        {
            Require.NotNull(zone, "zone");
            Require.NotNull(widget, "widget");

            var locatedWidget = new LocatedWidget(this)
            {
                PluginName = widget.Plugin.Name,
                WidgetName = widget.Name,
                ZoneName = zone.Name,
                Order = displayOrder
            };

            LocatedWidgets.Add(locatedWidget);

            return locatedWidget;
        }

        public virtual LocatedWidget FindLocatedWidget(int locatedWidgetId)
        {
            if (locatedWidgetId < 0)
            {
                return null;
            }
            return LocatedWidgets.FirstOrDefault(it => it.Id == locatedWidgetId);
        }

        public virtual bool IsSiblingOf(PageItem other)
        {
            if (other == null) return false;

            if (Parent == null && other.Parent == null) return true;

            if (Parent == null || other.Parent == null) return false;

            return Parent.Id == other.Parent.Id;
        }

        public virtual string GetPagePath()
        {
            return GetPagePathFrom(null);
        }

        public virtual string GetPagePathFrom(PageItem root)
        {
            var current = this;
            var path = String.Empty;

            while (current != null)
            {
                if (root != null && root.Id == current.Id) break;

                path = current.UrlSegment + "/" + path;
                current = current.Parent;
            }

            path = "/" + path.TrimEnd('/');

            return path;
        }

        public override string ToString()
        {
            return DisplayName;
        }

        IEnumerable<PageItem> ITreeNode<PageItem>.Children
        {
            get { return Pages; }
        }
    }
}
