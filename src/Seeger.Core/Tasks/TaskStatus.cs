using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Tasks
{
    public enum TaskStatus
    {
        Pending = 0,
        InProgress = 1,
        Aborted = 4,
        Completed = 2,
        Failed = 3
    }
}
