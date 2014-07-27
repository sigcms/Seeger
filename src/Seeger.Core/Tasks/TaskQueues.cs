using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Tasks
{
    public static class TaskQueues
    {
        static ConcurrentDictionary<string, TaskQueue> _queues = new ConcurrentDictionary<string, TaskQueue>(StringComparer.OrdinalIgnoreCase);

        public static IEnumerable<TaskQueue> Queues
        {
            get
            {
                return _queues.Values.ToList();
            }
        }

        public static TaskQueue Default
        {
            get
            {
                return Get("Default");
            }
        }

        public static TaskQueue InMemory
        {
            get
            {
                return Get("InMemory");
            }
        }

        public static void StartDefaultQueues()
        {
            StartNew("Default", "Default Queue", new TaskQueueOptions { WorkerNumber = 1 }, new DbQueueStore("Default"));
            StartNew("InMemory", "InMemory Queue", new TaskQueueOptions { WorkerNumber = 1 }, new InMemoryQueueStore("InMemory"));
        }

        public static TaskQueue StartNew(string name, string displayName, TaskQueueOptions options, IQueueStore queueStore)
        {
            var queue = new TaskQueue(name, displayName, options, queueStore);

            if (!_queues.TryAdd(name, queue))
            {
                queue.Dispose();
                throw new InvalidOperationException("Not able to add the queue to the pool.");
            }
            else
            {
                queue.Start();
            }

            return queue;
        }

        public static TaskQueue Get(string name)
        {
            TaskQueue queue = null;
            _queues.TryGetValue(name, out queue);

            return queue;
        }

        public static void StopAll()
        {
            foreach (var queue in _queues.Values.ToList())
            {
                queue.Stop();
            }
        }
    }
}
