using Seeger.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Seeger.Tasks
{
    public class InMemoryQueueStore : IQueueStore
    {
        private Dictionary<TaskStatus, List<TaskEntry>> _tasksByStatus = new Dictionary<TaskStatus, List<TaskEntry>>();
        private Dictionary<int, TaskEntry> _tasksById = new Dictionary<int, TaskEntry>();

        public string QueueName { get; private set; }

        public InMemoryQueueStore(string queueName)
        {
            QueueName = queueName;

            _tasksByStatus.Add(TaskStatus.Pending, new List<TaskEntry>());
            _tasksByStatus.Add(TaskStatus.InProgress, new List<TaskEntry>());
            _tasksByStatus.Add(TaskStatus.Aborted, new List<TaskEntry>());
            _tasksByStatus.Add(TaskStatus.Failed, new List<TaskEntry>());
            _tasksByStatus.Add(TaskStatus.Completed, new List<TaskEntry>());
        }

        public TaskEntry Next()
        {
            var pendings = _tasksByStatus[TaskStatus.Pending];
            if (pendings.Count > 0)
            {
                var task = pendings[0];

                if (task.LastStartedAtUtc != null)
                {
                    task.TotalRetries++;
                }

                task.LastStartedAtUtc = DateTime.UtcNow;

                ChangeTaskStatus(task, TaskStatus.InProgress);

                return task.Clone();
            }

            return null;
        }

        public void Starup()
        {
        }

        public void Append(Type taskType, string description, object state)
        {
            var task = new TaskEntry(QueueName, taskType, description, state);

            task.Id = IdAllocator.Next();
            task.Status = TaskStatus.Pending;

            _tasksByStatus[task.Status].Add(task);
            _tasksById.Add(task.Id, task);
        }

        public void MarkFailed(int taskId, Exception exception)
        {
            var task = GetTaskById(taskId);
            if (task != null)
            {
                ChangeTaskStatus(task, TaskStatus.Failed);
                task.ErrorMessage = exception.Message;
                task.ErrorDetail = ExceptionPrinter.Print(exception);
            }
        }

        private TaskEntry GetTaskById(int id)
        {
            TaskEntry entry;
            if (_tasksById.TryGetValue(id, out entry))
            {
                return entry;
            }

            return null;
        }

        public void MarkCompleted(int taskId)
        {
            ChangeTaskStatus(GetTaskById(taskId), TaskStatus.Completed);
        }

        public void Delete(int taskId)
        {
            var task = GetTaskById(taskId);
            RemoveTask(task);
        }

        public void Reset(params int[] taskIds)
        {
            if (taskIds != null && taskIds.Length > 0)
            {
                foreach (var id in taskIds)
                {
                    var task = GetTaskById(id);
                    ChangeTaskStatus(task, TaskStatus.Pending);
                }
            }
        }

        private void RemoveTask(TaskEntry entry)
        {
            _tasksByStatus[entry.Status].Remove(entry);
            _tasksById.Remove(entry.Id);
        }

        private void ChangeTaskStatus(TaskEntry task, TaskStatus newStatus)
        {
            _tasksByStatus[task.Status].Remove(task);

            task.Status = newStatus;
            if (newStatus == TaskStatus.Completed || newStatus == TaskStatus.Failed || newStatus == TaskStatus.Aborted)
            {
                task.LastStoppedAtUtc = DateTime.UtcNow;
            }

            _tasksByStatus[newStatus].Add(task);
        }

        public QueueStatistics Stat()
        {
            var stat = new QueueStatistics();

            stat.StatusCounts.Add(TaskStatus.Pending, _tasksByStatus[TaskStatus.Pending].Count);
            stat.StatusCounts.Add(TaskStatus.InProgress, _tasksByStatus[TaskStatus.InProgress].Count);
            stat.StatusCounts.Add(TaskStatus.Failed, _tasksByStatus[TaskStatus.Failed].Count);
            stat.StatusCounts.Add(TaskStatus.Aborted, _tasksByStatus[TaskStatus.Aborted].Count);
            stat.StatusCounts.Add(TaskStatus.Completed, _tasksByStatus[TaskStatus.Completed].Count);

            return stat;
        }

        public IList<TaskEntry> Query(TaskStatus status, int pageIndex, int pageSize, out int total)
        {
            total = _tasksByStatus[status].Count;

            return _tasksByStatus[status].Skip(pageIndex * pageSize)
                                         .Take(pageSize)
                                         .Select(t => t.Clone())
                                         .ToList();
        }

        static class IdAllocator
        {
            static int _id;

            public static int Next()
            {
                return Interlocked.Increment(ref _id);
            }
        }
    }
}
