using Seeger.Web.UI.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate.Linq;
using Seeger.Logging;
using Seeger.Text.Markup;

namespace Seeger.Web.UI.Admin._System
{
    public partial class Logs_Grid : AjaxGridUserControlBase
    {
        protected IList<LogEntry> Items { get; private set; }

        public override void Bind(AjaxGridContext context)
        {
            var logs = NhSession.Query<LogEntry>()
                                .OrderByDescending(x => x.UtcTimestamp)
                                .Paging(Pager.PageSize);

            Items = logs.Page(context.PageIndex).ToList();

            Pager.RecordCount = logs.Count;
            Pager.PageIndex = context.PageIndex;
        }

        protected string GetOperatorHtml(LogEntry entry)
        {
            if (entry.Operator == null) return null;

            if (String.IsNullOrEmpty(entry.Operator.Nick) || entry.Operator.Nick.Equals(entry.Operator.UserName, StringComparison.OrdinalIgnoreCase))
            {
                return entry.Operator.UserName;
            }

            return entry.Operator.Nick + " (" + entry.Operator.UserName + ")";
        }

        protected string TransformMessage(string message)
        {
            return MarkupLanguage.Transform(message);
        }
    }
}