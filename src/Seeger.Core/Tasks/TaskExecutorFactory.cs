using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Seeger.Tasks.Emails;

namespace Seeger.Tasks
{
    public class TaskExecutorFactory
    {
        private static readonly object _lock = new object();
        private static readonly Dictionary<string, Type> _executors;

        public static bool HasExecutors
        {
            get
            {
                return _executors.Count > 0;
            }
        }

        static TaskExecutorFactory()
        {
            _executors = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            _executors.Add(EmailTaskExecutor.TaskType, typeof(EmailTaskExecutor));
        }

        public static void Register<T>(string taskType)
            where T : ITaskExecutor
        {
            Register(taskType, typeof(T));
        }

        public static void Register(string taskType, Type executorType)
        {
            Require.NotNullOrEmpty(taskType, "taskType");
            Require.NotNull(executorType, "executorType");
            Require.That(typeof(ITaskExecutor).IsAssignableFrom(executorType), "'executorType' should implement " + typeof(ITaskExecutor) + ".");

            ThrowOnDuplicateRegister(taskType);

            lock (_lock)
            {
                ThrowOnDuplicateRegister(taskType);

                _executors.Add(taskType, executorType);
            }
        }

        public static ITaskExecutor CreateExecutor(string taskType)
        {
            Require.NotNullOrEmpty(taskType, "taskType");

            if (!_executors.ContainsKey(taskType))
                throw new InvalidOperationException("Cannot find any executor for task type '" + taskType + "'.");

            return (ITaskExecutor)Activator.CreateInstance(_executors[taskType]);
        }

        private static void ThrowOnDuplicateRegister(string taskType)
        {
            if (_executors.ContainsKey(taskType))
                throw new InvalidOperationException("An executor for task type '" + taskType + "' already exists.");
        }
    }
}
