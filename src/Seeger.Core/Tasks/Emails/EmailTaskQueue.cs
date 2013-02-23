using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace Seeger.Tasks.Emails
{
    public static class EmailTaskQueue
    {
        public static TaskItem CreateTake(EmailTaskData data)
        {
            var task = new TaskItem();
            task.Name = EmailTaskExecutor.TaskType;
            task.Type = EmailTaskExecutor.TaskType;
            task.Data = data.Serialize();

            return task;
        }

        public static void Enqueue(EmailTaskData data)
        {
            var task = CreateTake(data);
            TaskQueue.Enqueue(task);
        }
    }
}
