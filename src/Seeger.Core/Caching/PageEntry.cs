using Seeger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using Seeger.Templates;
using System.Globalization;
using Seeger.Collections;

namespace Seeger.Caching
{
    public class PageTree
    {
        public static readonly PageTree Instance;

        static PageTree()
        {
            Instance = new PageTree();

            using (var session = Database.OpenSession())
            {
                var pages = session.Query<PageItem>().ToList();
                BuildTree(pages, pages.Where(p => p.Parent == null), Instance.RootPages);
            }
        }

        static void BuildTree(IEnumerable<PageItem> all, IEnumerable<PageItem> pages, PageEntryCollection entries)
        {
            foreach (var page in pages.OrderBy(x => x.Order))
            {
                var entry = new PageEntry
                {
                    Id = page.Id,
                    UniqueName = page.UniqueName,
                    BindedDomains = page.BindedDomains,
                    UrlSegment = page.UrlSegment,
                    Layout = page.Layout,
                    Published = page.Published,
                    PageTitle = page.PageTitle,
                    MetaKeywords = page.MetaKeywords,
                    MetaDescription = page.MetaDescription,
                    Skin = page.Skin,
                    LocatedWidgets = page.LocatedWidgets.OrderBy(x => x.Order).Select(x => new LocatedWidgetEntry
                    {
                        Id = x.Id,
                        PluginName = x.PluginName,
                        WidgetName = x.WidgetName,
                        ZoneName = x.ZoneName,
                        Order = x.Order,
                        Attributes = x.Attributes
                    })
                    .ToList()
                };
                entries.Add(entry);

                Instance._pagesById.Add(entry.Id, entry);

                BuildTree(all, all.Where(x => x.Parent != null && x.Parent.Id == page.Id), entry.Pages);
            }
        }

        private Dictionary<int, PageEntry> _pagesById = new Dictionary<int, PageEntry>();

        public PageEntryCollection RootPages { get; private set; }

        public PageEntry Homepage
        {
            get
            {
                return RootPages.FirstOrDefault(p => p.Published);
            }
        }

        public PageTree()
        {
            RootPages = new PageEntryCollection(null);
        }

        public PageEntry Find(int id)
        {
            PageEntry result;
            if (_pagesById.TryGetValue(id, out result))
            {
                return result;
            }

            return null;
        }

        public PageEntry FindPage(Func<PageEntry, bool> predicate)
        {
            Require.NotNull(predicate, "predicate");

            foreach (var page in RootPages)
            {
                if (predicate(page))
                {
                    return page;
                }

                var result = page.FindDescendant(predicate);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }

    public class PageEntry : ITreeNode<PageEntry>
    {
        public int Id { get; set; }

        public string UniqueName { get; set; }

        public string BindedDomains { get; set; }

        public PageEntry Parent { get; set; }

        public PageEntryCollection Pages { get; set; }

        public string UrlSegment { get; set; }

        public bool Published { get; set; }

        public Layout Layout { get; set; }

        public TemplateSkin Skin { get; set; }

        public string PageTitle { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public PageEntry()
        {
            Pages = new PageEntryCollection(this);
            LocatedWidgets = new List<LocatedWidgetEntry>();
        }

        public SEOInfo GetSeoInfo(bool inheritParentSeoSettings = false)
        {
            return GetSeoInfo(null, inheritParentSeoSettings);
        }

        public SEOInfo GetSeoInfo(CultureInfo culture, bool inheritParentSeoSettings = false)
        {
            var seo = new SEOInfo();

            seo.PageTitle = PageTitle;
            seo.MetaKeywords = MetaKeywords;
            seo.MetaDescription = MetaDescription;

            if (inheritParentSeoSettings && Parent != null)
            {
                var parentSeo = Parent.GetSeoInfo(culture, true);
                seo.Merge(parentSeo);
            }

            return seo;
        }

        public List<LocatedWidgetEntry> LocatedWidgets { get; set; }

        public IEnumerable<LocatedWidgetEntry> FindLocatedWidgetsByZone(string zoneName)
        {
            return LocatedWidgets.Where(it => it.ZoneName == zoneName).OrderBy(it => it.Order);
        }


        public virtual string GetPagePath()
        {
            return GetPagePathFrom(null);
        }

        public virtual string GetPagePathFrom(PageEntry root)
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

        IEnumerable<PageEntry> ITreeNode<PageEntry>.Children
        {
            get { return Pages; }
        }
    }

    public class LocatedWidgetEntry
    {
        public virtual int Id { get; set; }

        public virtual int Order { get; set; }

        public virtual string PluginName { get; set; }

        public virtual string WidgetName { get; set; }

        public virtual string ZoneName { get; set; }

        public virtual EntityAttributeCollection Attributes { get; set; }
    }

    public class PageEntryCollection : IEnumerable<PageEntry>
    {
        private PageEntry _parent;
        private List<PageEntry> _items = new List<PageEntry>();

        public PageEntryCollection(PageEntry parent)
        {
            _parent = parent;
        }

        public void Add(PageEntry entry)
        {
            entry.Parent = _parent;
            _items.Add(entry);
        }

        public IEnumerator<PageEntry> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
