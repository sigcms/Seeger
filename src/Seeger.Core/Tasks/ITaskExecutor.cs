using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Tasks
{
    public interface ITaskExecutor
    {
        void Execute(TaskItem task);
    }
}
