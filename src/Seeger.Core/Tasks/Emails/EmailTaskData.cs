using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return XmlSerializerHelper.Serialize(this);
        }

        public static EmailTaskData Deserialize(string data)
        {
            return XmlSerializerHelper.Deserialize<EmailTaskData>(data);
        }
    }
}
