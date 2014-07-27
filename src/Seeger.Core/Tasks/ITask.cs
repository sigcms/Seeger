using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Tasks
{
    public interface ITask
    {
        void Execute(TaskContext context);
    }

    public class TaskContext
    {
        public object State { get; set; }
    }
}
