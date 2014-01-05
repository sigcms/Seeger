using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Impl;
using Seeger.Data.Mapping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Seeger.Data.Mapping
{
    public class ConventionModelMapper : ModelMapper
    {
        public ICustomizersHolder CustomizersHolder { get; private set; }

        public string TablePrefix { get; private set; }

        public ConventionModelMapper(string tablePrefix)
            : this(tablePrefix, new ConventionModelInspector())
        {
        }

        public ConventionModelMapper(string tablePrefix, IModelInspector modelInspector)
            : this(tablePrefix, modelInspector, new CustomizersHolder())
        {
        }

        private ConventionModelMapper(string tablePrefix, IModelInspector modelInspector, ICustomizersHolder customizerHolder)
            : base(modelInspector, modelInspector as IModelExplicitDeclarationsHolder, customizerHolder, new DefaultCandidatePersistentMembersProvider())
        {
            TablePrefix = tablePrefix;
            CustomizersHolder = customizerHolder;
            AppendDefaultEvents();
        }

        protected virtual void AppendDefaultEvents()
        {
            BeforeMapClass += MapTableName;
            BeforeMapClass += ApplyClassLevelAttributeMappings;
            BeforeMapClass += NoPoidGuid;
            BeforeMapClass += NoSetterPoidToField;
            BeforeMapClass += ApplyIdMappings;

            BeforeMapProperty += MemberToFieldAccessor;
            BeforeMapProperty += MemberNoSetterToField;
            BeforeMapProperty += MemberReadOnlyAccessor;
            BeforeMapProperty += ApplyPropertyAttributeMappings;
            BeforeMapProperty += TryMapComponent;

            BeforeMapComponent += MemberToFieldAccessor;
            BeforeMapComponent += MemberNoSetterToField;
            BeforeMapComponent += MemberReadOnlyAccessor;
            BeforeMapComponent += ComponentParentToFieldAccessor;
            BeforeMapComponent += ComponentParentNoSetterToField;

            BeforeMapBag += MemberToFieldAccessor;
            BeforeMapIdBag += MemberToFieldAccessor;
            BeforeMapSet += MemberToFieldAccessor;
            BeforeMapMap += MemberToFieldAccessor;
            BeforeMapList += MemberToFieldAccessor;

            BeforeMapBag += MemberNoSetterToField;
            BeforeMapIdBag += MemberNoSetterToField;
            BeforeMapSet += MemberNoSetterToField;
            BeforeMapMap += MemberNoSetterToField;
            BeforeMapList += MemberNoSetterToField;

            BeforeMapBag += MemberReadOnlyAccessor;
            BeforeMapIdBag += MemberReadOnlyAccessor;
            BeforeMapSet += MemberReadOnlyAccessor;
            BeforeMapMap += MemberReadOnlyAccessor;
            BeforeMapList += MemberReadOnlyAccessor;

            BeforeMapManyToOne += MemberToFieldAccessor;
            BeforeMapOneToOne += MemberToFieldAccessor;
            BeforeMapAny += MemberToFieldAccessor;
            BeforeMapManyToOne += MemberNoSetterToField;
            BeforeMapOneToOne += MemberNoSetterToField;
            BeforeMapAny += MemberNoSetterToField;
            BeforeMapManyToOne += MemberReadOnlyAccessor;
            BeforeMapOneToOne += MemberReadOnlyAccessor;
            BeforeMapAny += MemberReadOnlyAccessor;
        }

        protected virtual void ComponentParentToFieldAccessor(IModelInspector modelInspector, PropertyPath member, IComponentAttributesMapper componentMapper)
        {
            System.Type componentType = member.LocalMember.GetPropertyOrFieldType();
            IEnumerable<MemberInfo> persistentProperties =
                MembersProvider.GetComponentMembers(componentType).Where(p => ModelInspector.IsPersistentProperty(p));

            MemberInfo parentReferenceProperty = GetComponentParentReferenceProperty(persistentProperties, member.LocalMember.ReflectedType);
            if (parentReferenceProperty != null && MatchPropertyToField(parentReferenceProperty))
            {
                componentMapper.Parent(parentReferenceProperty, cp => cp.Access(Accessor.Field));
            }
        }

        protected virtual void ComponentParentNoSetterToField(IModelInspector modelInspector, PropertyPath member, IComponentAttributesMapper componentMapper)
        {
            System.Type componentType = member.LocalMember.GetPropertyOrFieldType();
            IEnumerable<MemberInfo> persistentProperties =
                MembersProvider.GetComponentMembers(componentType).Where(p => ModelInspector.IsPersistentProperty(p));

            MemberInfo parentReferenceProperty = GetComponentParentReferenceProperty(persistentProperties, member.LocalMember.ReflectedType);
            if (parentReferenceProperty != null && MatchNoSetterProperty(parentReferenceProperty))
            {
                componentMapper.Parent(parentReferenceProperty, cp => cp.Access(Accessor.NoSetter));
            }
        }

        protected virtual void MemberReadOnlyAccessor(IModelInspector modelInspector, PropertyPath member, IAccessorPropertyMapper propertyCustomizer)
        {
            if (MatchReadOnlyProperty(member.LocalMember))
            {
                propertyCustomizer.Access(Accessor.ReadOnly);
            }
        }

        protected bool MatchReadOnlyProperty(MemberInfo subject)
        {
            var property = subject as PropertyInfo;
            if (property == null)
            {
                return false;
            }
            if (CanReadCantWriteInsideType(property) || CanReadCantWriteInBaseType(property))
            {
                return PropertyToField.GetBackFieldInfo(property) == null;
            }
            return false;
        }

        private bool CanReadCantWriteInsideType(PropertyInfo property)
        {
            return !property.CanWrite && property.CanRead && property.DeclaringType == property.ReflectedType;
        }

        private bool CanReadCantWriteInBaseType(PropertyInfo property)
        {
            if (property.DeclaringType == property.ReflectedType)
            {
                return false;
            }
            var rfprop = property.DeclaringType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
                                                                                     | BindingFlags.DeclaredOnly).SingleOrDefault(pi => pi.Name == property.Name);
            return rfprop != null && !rfprop.CanWrite && rfprop.CanRead;
        }

        protected virtual void MemberNoSetterToField(IModelInspector modelInspector, PropertyPath member, IAccessorPropertyMapper propertyCustomizer)
        {
            if (MatchNoSetterProperty(member.LocalMember))
            {
                propertyCustomizer.Access(Accessor.NoSetter);
            }
        }

        protected virtual void MemberToFieldAccessor(IModelInspector modelInspector, PropertyPath member, IAccessorPropertyMapper propertyCustomizer)
        {
            if (MatchPropertyToField(member.LocalMember))
            {
                propertyCustomizer.Access(Accessor.Field);
            }
        }

        protected bool MatchPropertyToField(MemberInfo subject)
        {
            var property = subject as PropertyInfo;
            if (property == null)
            {
                return false;
            }
            var fieldInfo = PropertyToField.GetBackFieldInfo(property);
            if (fieldInfo != null)
            {
                return fieldInfo.FieldType != property.PropertyType;
            }

            return false;
        }

        protected virtual void NoSetterPoidToField(IModelInspector modelInspector, System.Type type, IClassAttributesMapper classCustomizer)
        {
            MemberInfo poidPropertyOrField = MembersProvider.GetEntityMembersForPoid(type).FirstOrDefault(modelInspector.IsPersistentId);
            if (MatchNoSetterProperty(poidPropertyOrField))
            {
                classCustomizer.Id(idm => idm.Access(Accessor.NoSetter));
            }
        }

        protected bool MatchNoSetterProperty(MemberInfo subject)
        {
            var property = subject as PropertyInfo;
            if (property == null || property.CanWrite || !property.CanRead)
            {
                return false;
            }
            var fieldInfo = PropertyToField.GetBackFieldInfo(property);
            if (fieldInfo != null)
            {
                return fieldInfo.FieldType == property.PropertyType;
            }

            return false;
        }

        protected virtual void NoPoidGuid(IModelInspector modelInspector, System.Type type, IClassAttributesMapper classCustomizer)
        {
            MemberInfo poidPropertyOrField = MembersProvider.GetEntityMembersForPoid(type).FirstOrDefault(mi => modelInspector.IsPersistentId(mi));
            if (!ReferenceEquals(null, poidPropertyOrField))
            {
                return;
            }
            classCustomizer.Id(null, idm => idm.Generator(Generators.Guid));
        }

        private void MapTableName(IModelInspector modelInspector, System.Type type, IClassAttributesMapper classCustomizer)
        {
            classCustomizer.Table(GetTableName(type));
        }

        private void ApplyIdMappings(IModelInspector modelInspector, System.Type type, IClassAttributesMapper classCustomizer)
        {
            var member = MembersProvider.GetEntityMembersForPoid(type).FirstOrDefault(m => modelInspector.IsPersistentId(m));

            if (member != null)
            {
                var idAttr = member.GetCustomAttributes(typeof(IdAttribute), false).OfType<IdAttribute>().FirstOrDefault();

                // If IdAttribute is not specified for persistent id property, then use the default id mapping strategy
                if (idAttr == null)
                {
                    idAttr = new IdAttribute();
                }

                idAttr.ApplyMapping(modelInspector, type, GetTableName(type), member, classCustomizer);
            }
        }

        private void ApplyClassLevelAttributeMappings(IModelInspector modelInspector, System.Type type, IClassAttributesMapper classCustomizer)
        {
            foreach (ClassLevelAttribute attr in type.GetCustomAttributes(false).OfType<ClassLevelAttribute>())
            {
                attr.ApplyMapping(modelInspector, type, classCustomizer);
            }
        }

        private void TryMapComponent(IModelInspector modelInspector, PropertyPath member, IPropertyMapper propertyCustomizer)
        {
            if (modelInspector.IsComponent(member.LocalMember.DeclaringType))
            {
                propertyCustomizer.Column(member.PreviousPath.LocalMember.Name + "_" + member.LocalMember.Name);
            }
        }

        private void ApplyPropertyAttributeMappings(IModelInspector modelInspector, PropertyPath member, IPropertyMapper propertyCustomizer)
        {
            foreach (PropertyAttribute attr in member.LocalMember.GetCustomAttributes(false).OfType<PropertyAttribute>())
            {
                attr.ApplyMapping(modelInspector, member, propertyCustomizer);
            }
        }

        private string GetTableName(Type type)
        {
            if (String.IsNullOrEmpty(TablePrefix))
            {
                return type.Name;
            }

            var prefix = TablePrefix;
            if (!prefix.EndsWith("_"))
            {
                prefix = prefix + "_";
            }

            return prefix + type.Name;
        }
    }
}
