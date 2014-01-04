using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping
{
    public class ConventionModelInspector : WrappedSimpleModelInspector
    {
        public ConventionModelInspector()
            : this(new SimpleModelInspector())
        {
        }

        public ConventionModelInspector(SimpleModelInspector inspector)
            : base(inspector)
        {
        }

        public override bool IsPersistentProperty(System.Reflection.MemberInfo member)
        {
            if (member.GetCustomAttributes(typeof(NotMappedAttribute), false).Any())
            {
                return false;
            }

            return base.IsPersistentProperty(member);
        }

        public override bool IsEntity(Type type)
        {
            if (IsComponent(type))
            {
                return false;
            }

            return base.IsEntity(type);
        }

        public override bool IsRootEntity(Type type)
        {
            if (IsComponent(type))
            {
                return false;
            }

            return base.IsRootEntity(type);
        }

        public override bool IsComponent(Type type)
        {
            if (type.GetCustomAttributes(typeof(ComponentAttribute), false).Any())
            {
                return true;
            }

            return base.IsComponent(type);
        }

        public override bool IsManyToOne(System.Reflection.MemberInfo member)
        {
            if (IsComponent(member.GetPropertyOrFieldType()))
            {
                return false;
            }

            return base.IsManyToOne(member);
        }

        public override bool IsOneToOne(System.Reflection.MemberInfo member)
        {
            if (IsComponent(member.GetPropertyOrFieldType()))
            {
                return false;
            }

            return base.IsOneToOne(member);
        }
    }
}
