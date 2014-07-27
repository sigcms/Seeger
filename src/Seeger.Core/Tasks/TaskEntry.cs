using Newtonsoft.Json;
using Seeger.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Tasks
{
    public class TaskEntry
    {
        [HiloId]
        public virtual int Id { get; set; }

        public virtual string Description { get; set; }

        public virtual string QueueName { get; set; }

        [StringClob]
        public virtual string State { get; set; }

        public virtual string TaskType { get; set; }

        public virtual TaskStatus Status { get; set; }

        public virtual int TotalRetries { get; set; }

        public virtual string ErrorMessage { get; set; }

        [StringClob]
        public virtual string ErrorDetail { get; set; }

        public virtual DateTime CreatedAtUtc { get; set; }

        public virtual DateTime? LastStartedAtUtc { get; set; }

        public virtual DateTime? LastStoppedAtUtc { get; set; }

        protected TaskEntry()
        {
        }

        public TaskEntry(string queueName, Type taskType, string description, object state)
        {
            CreatedAtUtc = DateTime.UtcNow;
            QueueName = queueName;
            TaskType = taskType.AssemblyQualifiedNameWithoutVersion();
            Description = description;
            if (state != null)
            {
                State = JsonConvert.SerializeObject(state, GetJsonSerializationSettings());
            }
        }

        public virtual object LoadState()
        {
            if (String.IsNullOrEmpty(State))
            {
                return null;
            }

            return JsonConvert.DeserializeObject(State, GetJsonSerializationSettings());
        }

        private JsonSerializerSettings GetJsonSerializationSettings()
        {
            return new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
            };
        }

        public virtual TaskEntry Clone()
        {
            return (TaskEntry)MemberwiseClone();
        }

        public override string ToString()
        {
            return String.Format("[{0}] {1}", QueueName, Description);
        }
    }
}
