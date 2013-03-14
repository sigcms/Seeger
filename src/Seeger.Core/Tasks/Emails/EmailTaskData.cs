using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger.Utils;

namespace Seeger.Tasks.Emails
{
    public class EmailTaskData
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public string ReceiverName { get; set; }

        public string ReceiverEmail { get; set; }

        public EmailTaskData()
        {
        }

        public string Serialize()
        {
            return XmlSerializerUtil.Serialize(this);
        }

        public static EmailTaskData Deserialize(string data)
        {
            return XmlSerializerUtil.Deserialize<EmailTaskData>(data);
        }
    }
}
