using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeger.Tasks;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;

namespace Seeger.Data.Mapping
{
    class TaskItemMap : ClassMapping<TaskItem>
    {
        public string TableName
        {
            get
            {
                return "cms_" + typeof(TaskItem).Name;
            }
        }

        public TaskItemMap()
        {
            Table(TableName);

            this.HighLowId(c => c.Id, TableName);

            Property(c => c.Name);
            Property(c => c.Type, m => m.Column("`Type`"));
            Property(c => c.Data, m => m.Type(NHibernateUtil.StringClob));
            Property(c => c.Completed);
            Property(c => c.CompletedTime);
            Property(c => c.CreatedTime);
        }
    }
}
