using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seeger.Data.Mapping
{
    public class WrappedSimpleModelInspector : IModelInspector, IModelExplicitDeclarationsHolder
    {
        protected IModelInspector InnerInspector { get; private set; }

        protected IModelExplicitDeclarationsHolder InnerExplicitDeclarationsHolder { get; private set; }

        public WrappedSimpleModelInspector(SimpleModelInspector inspector)
        {
            Require.NotNull(inspector, "inspector");
            InnerInspector = inspector;
            InnerExplicitDeclarationsHolder = inspector;
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

        public virtual void AddAsAny(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsAny(member);
        }

        public virtual void AddAsArray(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsArray(member);
        }

        public virtual void AddAsBag(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsBag(member);
        }

        public virtual void AddAsComponent(Type type)
        {
            InnerExplicitDeclarationsHolder.AddAsComponent(type);
        }

        public virtual void AddAsDynamicComponent(System.Reflection.MemberInfo member, Type componentTemplate)
        {
            InnerExplicitDeclarationsHolder.AddAsDynamicComponent(member, componentTemplate);
        }

        public virtual void AddAsIdBag(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsIdBag(member);
        }

        public virtual void AddAsList(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsList(member);
        }

        public virtual void AddAsManyToAnyRelation(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsManyToAnyRelation(member);
        }

        public virtual void AddAsManyToManyRelation(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsManyToManyRelation(member);
        }

        public virtual void AddAsManyToOneRelation(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsManyToOneRelation(member);
        }

        public virtual void AddAsMap(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsMap(member);
        }

        public virtual void AddAsNaturalId(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsNaturalId(member);
        }

        public virtual void AddAsOneToManyRelation(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsOneToManyRelation(member);
        }

        public virtual void AddAsOneToOneRelation(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsOneToOneRelation(member);
        }

        public virtual void AddAsPartOfComposedId(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsPartOfComposedId(member);
        }

        public virtual void AddAsPersistentMember(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsPersistentMember(member);
        }

        public virtual void AddAsPoid(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsPoid(member);
        }

        public virtual void AddAsProperty(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsProperty(member);
        }

        public virtual void AddAsPropertySplit(SplitDefinition definition)
        {
            InnerExplicitDeclarationsHolder.AddAsPropertySplit(definition);
        }

        public virtual void AddAsRootEntity(Type type)
        {
            InnerExplicitDeclarationsHolder.AddAsRootEntity(type);
        }

        public virtual void AddAsSet(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsSet(member);
        }

        public virtual void AddAsTablePerClassEntity(Type type)
        {
            InnerExplicitDeclarationsHolder.AddAsTablePerClassEntity(type);
        }

        public virtual void AddAsTablePerClassHierarchyEntity(Type type)
        {
            InnerExplicitDeclarationsHolder.AddAsTablePerClassHierarchyEntity(type);
        }

        public virtual void AddAsTablePerConcreteClassEntity(Type type)
        {
            InnerExplicitDeclarationsHolder.AddAsTablePerConcreteClassEntity(type);
        }

        public virtual void AddAsVersionProperty(System.Reflection.MemberInfo member)
        {
            InnerExplicitDeclarationsHolder.AddAsVersionProperty(member);
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> Any
        {
            get
            {
                return InnerExplicitDeclarationsHolder.Any;
            }
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> Arrays
        {
            get
            {
                return InnerExplicitDeclarationsHolder.Arrays;
            }
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> Bags
        {
            get
            {
                return InnerExplicitDeclarationsHolder.Bags;
            }
        }

        public virtual IEnumerable<Type> Components
        {
            get
            {
                return InnerExplicitDeclarationsHolder.Components;
            }
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> ComposedIds
        {
            get
            {
                return InnerExplicitDeclarationsHolder.ComposedIds;
            }
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> Dictionaries
        {
            get
            {
                return InnerExplicitDeclarationsHolder.Dictionaries;
            }
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> DynamicComponents
        {
            get
            {
                return InnerExplicitDeclarationsHolder.DynamicComponents;
            }
        }

        public virtual string GetSplitGroupFor(System.Reflection.MemberInfo member)
        {
            return InnerExplicitDeclarationsHolder.GetSplitGroupFor(member);
        }

        public virtual IEnumerable<string> GetSplitGroupsFor(Type type)
        {
            return InnerExplicitDeclarationsHolder.GetSplitGroupsFor(type);
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> IdBags
        {
            get
            {
                return InnerExplicitDeclarationsHolder.IdBags;
            }
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> Lists
        {
            get
            {
                return InnerExplicitDeclarationsHolder.Lists;
            }
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> ManyToAnyRelations
        {
            get
            {
                return InnerExplicitDeclarationsHolder.ManyToAnyRelations;
            }
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> ManyToManyRelations
        {
            get
            {
                return InnerExplicitDeclarationsHolder.ManyToManyRelations;
            }
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> ManyToOneRelations
        {
            get
            {
                return InnerExplicitDeclarationsHolder.ManyToOneRelations;
            }
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> NaturalIds
        {
            get
            {
                return InnerExplicitDeclarationsHolder.NaturalIds;
            }
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> OneToManyRelations
        {
            get
            {
                return InnerExplicitDeclarationsHolder.OneToManyRelations;
            }
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> OneToOneRelations
        {
            get
            {
                return InnerExplicitDeclarationsHolder.OneToOneRelations;
            }
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> PersistentMembers
        {
            get
            {
                return InnerExplicitDeclarationsHolder.PersistentMembers;
            }
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> Poids
        {
            get
            {
                return InnerExplicitDeclarationsHolder.Poids;
            }
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> Properties
        {
            get
            {
                return InnerExplicitDeclarationsHolder.Properties;
            }
        }

        public virtual IEnumerable<Type> RootEntities
        {
            get
            {
                return InnerExplicitDeclarationsHolder.RootEntities;
            }
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> Sets
        {
            get
            {
                return InnerExplicitDeclarationsHolder.Sets;
            }
        }

        public virtual IEnumerable<SplitDefinition> SplitDefinitions
        {
            get
            {
                return InnerExplicitDeclarationsHolder.SplitDefinitions;
            }
        }

        public virtual IEnumerable<Type> TablePerClassEntities
        {
            get
            {
                return InnerExplicitDeclarationsHolder.TablePerClassEntities;
            }
        }

        public virtual IEnumerable<Type> TablePerClassHierarchyEntities
        {
            get
            {
                return InnerExplicitDeclarationsHolder.TablePerClassHierarchyEntities;
            }
        }

        public virtual IEnumerable<Type> TablePerConcreteClassEntities
        {
            get
            {
                return InnerExplicitDeclarationsHolder.TablePerConcreteClassEntities;
            }
        }

        public virtual IEnumerable<System.Reflection.MemberInfo> VersionProperties
        {
            get
            {
                return InnerExplicitDeclarationsHolder.VersionProperties;
            }
        }
    }
}
