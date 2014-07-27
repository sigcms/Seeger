using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Tasks
{
    public class QueueStatistics
    {
        public IDictionary<TaskStatus, int> StatusCounts { get; set; }

        public QueueStatistics()
        {
            StatusCounts = new Dictionary<TaskStatus, int>();
        }
    }
}
