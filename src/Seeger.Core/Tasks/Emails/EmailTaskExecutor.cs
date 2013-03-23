using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using Seeger.Config;

namespace Seeger.Tasks.Emails
{
    public class EmailTaskExecutor : ITaskExecutor
    {
        public static readonly string TaskType = "Email";

        public void Execute(TaskItem task)
        {
            var settings = GlobalSettingManager.Instance.Smtp;

            if (String.IsNullOrEmpty(settings.Server))
                throw new InvalidOperationException("SMTP server should be configured first.");

            var taskData = EmailTaskData.Deserialize(task.Data);

            var from = new MailAddress(settings.SenderEmail, settings.SenderName, Encoding.UTF8);
            var to = new MailAddress(taskData.ReceiverEmail, taskData.ReceiverName, Encoding.UTF8);

            var message = new MailMessage(from, to);
            message.Subject = taskData.Subject;
            message.SubjectEncoding = Encoding.UTF8;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            message.Body = taskData.Body;

            var client = new SmtpClient();
            client.Host = settings.Server;
            client.Port = settings.Port;
            client.EnableSsl = settings.EnableSsl;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            if (!String.IsNullOrEmpty(settings.AccountName))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(settings.AccountName, settings.Password);
            }

            client.Send(message);
       }
    }
}
