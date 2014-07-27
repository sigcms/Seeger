using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Tasks
{
    public interface IQueueStore
    {
        TaskEntry Next();

        void Starup();

        void Append(Type taskType, string description, object state);
        
        void MarkFailed(int taskId, Exception exception);

        void MarkCompleted(int taskId);

        void Reset(params int[] taskIds);

        QueueStatistics Stat();

        IList<TaskEntry> Query(TaskStatus status, int pageIndex, int pageSize, out int total);
    }
}
