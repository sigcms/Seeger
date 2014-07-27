using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Tasks
{
    public class DefaultTaskRunner : ITaskRunner
    {
        public void Run(TaskEntry taskEntry)
        {
            var taskType = Type.GetType(taskEntry.TaskType, true);
            var task = Activator.CreateInstance(taskType) as ITask;
            var context = new TaskContext
            {
                State = taskEntry.LoadState()
            };

            task.Execute(context);
        }
    }
}
