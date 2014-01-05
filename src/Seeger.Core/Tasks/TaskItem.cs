using Seeger.Data.Mapping;
using Seeger.Data.Mapping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Tasks
{
    [Entity]
    public class TaskItem
    {
        public virtual int Id { get; protected set; }

        public virtual string Name { get; set; }

        public virtual string Type { get; set; }

        [StringClob]
        public virtual string Data { get; set; }

        public virtual bool Completed { get; set; }

        public virtual DateTime? CompletedTime { get; set; }

        public virtual DateTime CreatedTime { get; protected set; }

        public TaskItem()
        {
            CreatedTime = DateTime.Now;
        }
    }
}
