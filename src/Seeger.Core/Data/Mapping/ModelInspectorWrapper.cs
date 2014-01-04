using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping
{
    public class ModelInspectorWrapper : IModelInspector
    {
        protected IModelInspector InnerInspector { get; private set; }

        public ModelInspectorWrapper(IModelInspector inspector)
        {
            InnerInspector = inspector;
        }

        public virtual Type GetDynamicComponentTemplate(System.Reflection.MemberInfo member)
        {
            return InnerInspector.GetDynamicComponentTemplate(member);
        }

        public virtual IEnumerable<string> GetPropertiesSplits(Type type)
        {
            return InnerInspector.GetPropertiesSplits(type);
        }

        public virtual bool IsAny(System.Reflection.MemberInfo member)
        {
            return InnerInspector.IsAny(member);
        }

        public virtual bool IsArray(System.Reflection.MemberInfo role)
        {
            return InnerInspector.IsArray(role);
        }

        public virtual bool IsBag(System.Reflection.MemberInfo role)
        {
            return InnerInspector.IsBag(role);
        }

        public virtual bool IsComponent(Type type)
        {
            return InnerInspector.IsComponent(type);
        }

        public virtual bool IsDictionary(System.Reflection.MemberInfo role)
        {
            return InnerInspector.IsDictionary(role);
        }

        public virtual bool IsDynamicComponent(System.Reflection.MemberInfo member)
        {
            return InnerInspector.IsDynamicComponent(member);
        }

        public virtual bool IsEntity(Type type)
        {
            return InnerInspector.IsEntity(type);
        }

        public virtual bool IsIdBag(System.Reflection.MemberInfo role)
        {
            return InnerInspector.IsIdBag(role);
        }

        public virtual bool IsList(System.Reflection.MemberInfo role)
        {
            return InnerInspector.IsList(role);
        }

        public virtual bool IsManyToAny(System.Reflection.MemberInfo member)
        {
            return InnerInspector.IsManyToAny(member);
        }

        public virtual bool IsManyToMany(System.Reflection.MemberInfo member)
        {
            return InnerInspector.IsManyToMany(member);
        }

        public virtual bool IsManyToOne(System.Reflection.MemberInfo member)
        {
            return InnerInspector.IsManyToOne(member);
        }

        public virtual bool IsMemberOfComposedId(System.Reflection.MemberInfo member)
        {
            return InnerInspector.IsMemberOfComposedId(member);
        }

        public virtual bool IsMemberOfNaturalId(System.Reflection.MemberInfo member)
        {
            return InnerInspector.IsMemberOfNaturalId(member);
        }

        public virtual bool IsOneToMany(System.Reflection.MemberInfo member)
        {
            return InnerInspector.IsOneToMany(member);
        }

        public virtual bool IsOneToOne(System.Reflection.MemberInfo member)
        {
            return InnerInspector.IsOneToOne(member);
        }

        public virtual bool IsPersistentId(System.Reflection.MemberInfo member)
        {
            return InnerInspector.IsPersistentId(member);
        }

        public virtual bool IsPersistentProperty(System.Reflection.MemberInfo member)
        {
            return InnerInspector.IsPersistentProperty(member);
        }

        public virtual bool IsProperty(System.Reflection.MemberInfo member)
        {
            return InnerInspector.IsProperty(member);
        }

        public virtual bool IsRootEntity(Type type)
        {
            return InnerInspector.IsRootEntity(type);
        }

        public virtual bool IsSet(System.Reflection.MemberInfo role)
        {
            return InnerInspector.IsSet(role);
        }

        public virtual bool IsTablePerClass(Type type)
        {
            return InnerInspector.IsTablePerClass(type);
        }

        public virtual bool IsTablePerClassHierarchy(Type type)
        {
            return InnerInspector.IsTablePerClassHierarchy(type);
        }

        public virtual bool IsTablePerClassSplit(Type type, object splitGroupId, System.Reflection.MemberInfo member)
        {
            return InnerInspector.IsTablePerClassSplit(type, splitGroupId, member);
        }

        public virtual bool IsTablePerConcreteClass(Type type)
        {
            return InnerInspector.IsTablePerConcreteClass(type);
        }

        public virtual bool IsVersion(System.Reflection.MemberInfo member)
        {
            return InnerInspector.IsVersion(member);
        }
    }
}
