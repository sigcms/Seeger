using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger.Data;

namespace Seeger.Tasks
{
    public static class TaskQueue
    {
        public static void Enqueue(TaskItem task)
        {
            using (var session = NhSessionManager.OpenSession())
            {
                session.Save(task);
                session.Commit();
            }
        }
    }
}
