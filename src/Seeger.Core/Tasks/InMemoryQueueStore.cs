using Seeger.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Tasks
{
    public class InMemoryQueueStore : IQueueStore
    {
        private Dictionary<TaskStatus, List<TaskEntry>> _tasksByStatus = new Dictionary<TaskStatus, List<TaskEntry>>();
        private Dictionary<int, TaskEntry> _tasksById = new Dictionary<int, TaskEntry>();
        private int _totalCompleted = 0;

        public string QueueName { get; private set; }

        public InMemoryQueueStore(string queueName)
        {
            QueueName = queueName;
            _tasksByStatus.Add(TaskStatus.Pending, new List<TaskEntry>());
            _tasksByStatus.Add(TaskStatus.InProgress, new List<TaskEntry>());
            _tasksByStatus.Add(TaskStatus.Aborted, new List<TaskEntry>());
            _tasksByStatus.Add(TaskStatus.Failed, new List<TaskEntry>());
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
            // We need to remove completed tasks to reduce memory usage
            _totalCompleted++;
            RemoveTask(GetTaskById(taskId));
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
            if (newStatus == TaskStatus.Completed || newStatus == TaskStatus.Failed)
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
            stat.StatusCounts.Add(TaskStatus.Aborted, 0);
            stat.StatusCounts.Add(TaskStatus.Completed, _totalCompleted);

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
    }
}
