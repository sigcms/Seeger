using Seeger.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Web.UI.Admin.Api.Queues
{
    public class QueueModel
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public QueueStatistics Statistics { get; set; }
    }
}