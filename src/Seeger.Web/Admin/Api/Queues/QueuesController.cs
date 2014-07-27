using Seeger.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seeger.Web.UI.Admin.Api.Queues
{
    public class QueuesController : AdminApiController
    {
        public IEnumerable<QueueModel> Get()
        {
            var models = new List<QueueModel>();
            foreach (var queue in TaskQueues.Queues.OrderBy(x => x.DisplayName))
            {
                var stat = queue.Stat();
                models.Add(new QueueModel
                {
                    Name = queue.Name,
                    DisplayName = queue.DisplayName,
                    Statistics = stat
                });
            }

            return models;
        }
    }
}