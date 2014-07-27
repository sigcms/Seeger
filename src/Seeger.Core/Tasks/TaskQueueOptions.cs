using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Tasks
{
    public class TaskQueueOptions
    {
        public int WorkerNumber { get; set; }

        public bool DeleteCompletedTasks { get; set; }

        public TaskQueueOptions()
        {
            WorkerNumber = 1;
            DeleteCompletedTasks = true;
        }

        public TaskQueueOptions Clone()
        {
            return (TaskQueueOptions)MemberwiseClone();
        }
    }
}
