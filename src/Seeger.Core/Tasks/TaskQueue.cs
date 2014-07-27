using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Seeger.Tasks
{
    public class TaskQueue : IDisposable
    {
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        private readonly IQueueStore _queueStore;
        private readonly Mutex _mutex = new Mutex();
        private readonly ManualResetEventSlim _queueNotEmptyEvent = new ManualResetEventSlim(false);
        private Thread[] _workers;
        private readonly object _startLock = new object();

        private bool _needToStop = false;
        private TaskQueueOptions _options;

        private ITaskRunner _taskRunner = new DefaultTaskRunner();

        public string Name { get; private set; }

        public string DisplayName { get; private set; }

        public bool IsRunning { get; private set; }

        public TaskQueue(string name, string displayName, TaskQueueOptions options, IQueueStore queueStore)
        {
            Require.NotNullOrEmpty(name, "name");
            Require.NotNull(options, "options");
            Require.NotNull(queueStore, "queueStore");

            Name = name;
            DisplayName = String.IsNullOrEmpty(displayName) ? name : displayName;
            _options = options.Clone();
            _queueStore = queueStore;
        }

        public void Enqueue<TTask>(string description, object state)
            where TTask : ITask
        {
            _mutex.WaitOne();

            try
            {
                var stored = false;

                try
                {
                    _queueStore.Append(typeof(TTask), description, state);
                    stored = true;
                }
                catch (Exception ex)
                {
                    _log.ErrorException("无法存储任务: " + ex.Message, ex);
                }

                if (stored)
                {
                    _queueNotEmptyEvent.Set();
                }
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public void Start()
        {
            if (IsRunning)
            {
                return;
            }

            lock (_startLock)
            {
                if (!IsRunning)
                {
                    _workers = new Thread[_options.WorkerNumber];

                    for (var i = 0; i < _workers.Length; i++)
                    {
                        _workers[i] = new Thread(DoWork);
                        _workers[i].Start();
                    }

                    _log.Info("Task queue '" + Name + "' started with " + _workers.Length + " workers.");

                    _queueStore.Starup();

                    // Check if there's tasks pending at startup
                    _queueNotEmptyEvent.Set();
                }
            }
        }

        private void DoWork()
        {
            while (!_needToStop)
            {
                _queueNotEmptyEvent.Wait();

                if (_needToStop)
                {
                    break;
                }

                _mutex.WaitOne();

                if (_needToStop)
                {
                    _mutex.ReleaseMutex();
                    break;
                }

                var noErrorOccurs = true;
                TaskEntry taskEntry = null;

                try
                {
                    taskEntry = _queueStore.Next();
                }
                catch (Exception ex)
                {
                    noErrorOccurs = false;
                    _log.ErrorException("无法提取任务: " + ex.Message, ex);
                }

                if (noErrorOccurs)
                {
                    if (taskEntry != null)
                    {
                        _mutex.ReleaseMutex();
                        SafeExecuteTask(taskEntry);
                    }
                    else
                    {
                        _queueNotEmptyEvent.Reset();
                        _mutex.ReleaseMutex();
                    }
                }
                else
                {
                    _mutex.ReleaseMutex();
                }
            }
        }

        private void SafeExecuteTask(TaskEntry taskEntry)
        {
            try
            {
                _taskRunner.Run(taskEntry);

                if (_options.DeleteCompletedTasks)
                {
                    _queueStore.Delete(taskEntry.Id);
                }
                else
                {
                    _queueStore.MarkCompleted(taskEntry.Id);
                }
            }
            catch (Exception ex)
            {
                _queueStore.MarkFailed(taskEntry.Id, ex);
            }
        }

        public QueueStatistics Stat()
        {
            return _queueStore.Stat();
        }

        public IList<TaskEntry> Query(TaskStatus status, int pageIndex, int pageSize, out int total)
        {
            return _queueStore.Query(status, pageIndex, pageSize, out total);
        }

        public void Reset(params int[] taskIds)
        {
            _mutex.WaitOne();

            try
            {
                _queueStore.Reset(taskIds);
                _queueNotEmptyEvent.Set();
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        public void Stop()
        {
            _needToStop = true;
            _queueNotEmptyEvent.Set();

            _log.Info("Queue '" + Name + "' received stop command, preparing to stop...");

            foreach (var worker in _workers)
            {
                worker.Join();
            }

            _log.Info("Queue '" + Name + "' stopped.");
        }

        public void Dispose()
        {
            _mutex.Dispose();
            _queueNotEmptyEvent.Dispose();
        }
    }
}
