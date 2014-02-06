using NHibernate;
using Seeger.Caching;
using Seeger.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Services
{
    public class PageMovementService
    {
        private ISession _session;

        public PageMovementService(ISession session)
        {
            _session = session;
        }

        public void Move(PageItem from, PageItem to, DropPosition dropPosition)
        {
            var pageCache = PageCache.From(_session);

            if (dropPosition == DropPosition.Over)
            {
                to.Pages.Add(from);
                to.Pages.AdjustOrders(false);
                return;
            }

            if (dropPosition == DropPosition.Before)
            {
                var pages = new PageItemCollectionWrapper(to.Parent == null ? pageCache.RootPages : to.Parent.Pages);

                if (from.IsSiblingOf(to))
                {
                    pages.MoveBefore(to, from);
                }
                else
                {
                    pages.AddBefore(to, from);
                }

                pages.AdjustOrders(false);

                return;
            }

            if (dropPosition == DropPosition.After)
            {
                var pages = new PageItemCollectionWrapper(to.Parent == null ? pageCache.RootPages : to.Parent.Pages);

                if (from.IsSiblingOf(to))
                {
                    pages.MoveAfter(to, from);
                }
                else
                {
                    pages.AddAfter(to, from);
                }

                pages.AdjustOrders(false);
            }
        }
    }
}
