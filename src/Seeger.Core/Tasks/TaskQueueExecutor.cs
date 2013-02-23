using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

using NLog;
using NHibernate.Linq;
using Seeger.Data;

namespace Seeger.Tasks
{
    public class TaskQueueExecutor
    {
        private static int _batchSize = 10;

        public static int BatchSize
        {
            get
            {
                return _batchSize;
            }
            set
            {
                Require.That(value > 0, "Batch size should be greater than 0.");
                _batchSize = value;
            }
        }

        private static readonly Thread _thread;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static TaskQueueExecutor()
        {
            _thread = new Thread(new ThreadStart(ExecuteTasks));
        }

        private static void ExecuteTasks()
        {
            while (true)
            {
                if (GlobalSettingManager.Instance.TaskQueue.Enabled && TaskExecutorFactory.HasExecutors)
                {
                    try
                    {
                        using (var session = NhSessionManager.OpenSession())
                        {
                            var tasks = session.Query<TaskItem>()
                                          .Where(it => !it.Completed)
                                          .OrderBy(it => it.Id)
                                          .Take(BatchSize)
                                          .ToList();

                            if (tasks.Count > 0)
                            {
                                _logger.Debug("Start executing task batch (" + tasks.Count + " tasks)");

                                foreach (var task in tasks)
                                {
                                    var executor = TaskExecutorFactory.CreateExecutor(task.Type);
                                    executor.Execute(task);

                                    task.Completed = true;
                                    task.CompletedTime = DateTime.Now;

                                    session.Commit();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.ErrorException(ex.Message, ex);
                    }
                }

                Thread.Sleep(new TimeSpan(0, GlobalSettingManager.Instance.TaskQueue.IntervalInMinutes, 0));
            }
        }

        public static void Start()
        {
            _thread.Start();
        }
    }
}
