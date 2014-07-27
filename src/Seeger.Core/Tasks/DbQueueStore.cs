using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq;
using Seeger.Data;
using Seeger.Data.Mapping;
using Seeger.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Seeger.Tasks
{
    public class DbQueueStore : IQueueStore
    {
        public string QueueName { get; private set; }

        public DbQueueStore(string queueName)
        {
            Require.NotNullOrEmpty(queueName, "queueName");
            QueueName = queueName;
        }

        public TaskEntry Next()
        {
            using (var session = OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var task = Query(session).Where(t => t.Status == TaskStatus.Pending)
                                         .OrderBy(x => x.CreatedAtUtc)
                                         .FirstOrDefault();

                if (task != null)
                {
                    task.Status = TaskStatus.InProgress;

                    // 如果该任务已执行过，则本次执行为重试
                    if (task.LastStartedAtUtc != null)
                    {
                        task.TotalRetries++;
                    }

                    task.LastStartedAtUtc = DateTime.UtcNow;

                    tx.Commit();

                    return task.Clone();
                }

                return null;
            }
        }

        public void Starup()
        {
            using (var session = OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var tasks = Query(session).Where(t => t.Status == TaskStatus.InProgress).ToList();
                foreach (var task in tasks)
                {
                    task.Status = TaskStatus.Aborted;
                }

                tx.Commit();
            }
        }

        public void Append(Type taskType, string description, object state)
        {
            Require.NotNull(taskType, "taskType");

            var task = new TaskEntry(QueueName, taskType, description, state);

            using (var session = OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var newTask = task.Clone();
                newTask.QueueName = QueueName;
                session.Save(newTask);
                tx.Commit();
            }
        }

        public void MarkFailed(int taskId, Exception exception)
        {
            using (var session = OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var task = session.Get<TaskEntry>(taskId);
                if (task != null)
                {
                    task.Status = TaskStatus.Failed;
                    task.ErrorMessage = exception.Message;
                    task.ErrorDetail = ExceptionPrinter.Print(exception);
                    task.LastStoppedAtUtc = DateTime.UtcNow;

                    tx.Commit();
                }
            }
        }

        public void MarkCompleted(int taskId)
        {
            using (var session = OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var task = session.Get<TaskEntry>(taskId);
                if (task != null)
                {
                    task.Status = TaskStatus.Completed;
                    task.LastStoppedAtUtc = DateTime.UtcNow;
                    tx.Commit();
                }
            }
        }

        public void Reset(params int[] taskIds)
        {
            using (var session = OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var tasks = Query(session).Where(t => t.Status == TaskStatus.Failed)
                                          .Where(t => taskIds.Contains(t.Id))
                                          .ToList();

                foreach (var task in tasks)
                {
                    task.Status = TaskStatus.Pending;
                }

                tx.Commit();
            }
        }

        public QueueStatistics Stat()
        {
            var stat = new QueueStatistics();
            using (var session = OpenSession())
            {
                foreach (var value in Enum.GetValues(typeof(TaskStatus)))
                {
                    var status = (TaskStatus)value;
                    var count = Query(session).Where(t => t.Status == status).Count();
                    stat.StatusCounts.Add(status, count);
                }
            }

            return stat;
        }

        public IList<TaskEntry> Query(TaskStatus status, int pageIndex, int pageSize, out int total)
        {
            using (var session = OpenSession())
            {
                var query = Query(session).Where(t => t.Status == status).OrderBy(x => x.CreatedAtUtc);

                total = query.Count();

                return query.Skip(pageIndex * pageSize)
                            .Take(pageSize)
                            .ToList()
                            .Select(t => t.Clone())
                            .ToList();
            }
        }

        private ISession OpenSession()
        {
            return QueueDatabase.Instance.SessionFactory.OpenSession();
        }

        private IQueryable<TaskEntry> Query(ISession session)
        {
            return session.Query<TaskEntry>().Where(t => t.QueueName == QueueName);
        }

        class QueueDatabase
        {
            public ISessionFactory SessionFactory { get; private set; }

            public void Initialize()
            {
                var config = new Configuration();

                config.SetProperty(NHibernate.Cfg.Environment.Hbm2ddlKeyWords, "auto-quote");
                config.Configure(Database.ConfigurationFilePath);

                config.AddMapping(new ConventionMappingCompiler("cms").AddEntityTypes(typeof(TaskEntry)).CompileMapping());

                SessionFactory = config.BuildSessionFactory();
            }

            static readonly Lazy<QueueDatabase> _instance;

            public static QueueDatabase Instance
            {
                get
                {
                    return _instance.Value;
                }
            }

            static QueueDatabase()
            {
                _instance = new Lazy<QueueDatabase>(() =>
                {
                    var db = new QueueDatabase();
                    db.Initialize();
                    return db;
                }, true);
            }
        }
    }
}
