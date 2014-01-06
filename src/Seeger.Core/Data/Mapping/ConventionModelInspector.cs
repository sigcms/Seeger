using NHibernate.Mapping.ByCode;
using Seeger.ComponentModel;
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

        public override bool IsPersistentId(System.Reflection.MemberInfo member)
        {
            if (member.GetCustomAttributes(typeof(IdAttribute), false).Any())
            {
                return true;
            }

            return base.IsPersistentId(member);
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
            if (!type.IsClass)
            {
                return false;
            }

            if (type.GetCustomAttributes(typeof(EntityAttribute), false).Any())
            {
                return true;
            }

            if (IsComponent(type))
            {
                return false;
            }

            return base.IsEntity(type);
        }

        public override bool IsRootEntity(Type type)
        {
            return type.IsClass && typeof(object).Equals(type.BaseType) && IsEntity(type);
        }

        public override bool IsComponent(Type type)
        {
            if (type.GetCustomAttributes(typeof(ComponentAttribute), false).Any())
            {
                return true;
            }
            if (type.GetCustomAttributes(typeof(EntityAttribute), false).Any())
            {
                return false;
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
