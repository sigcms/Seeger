using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Seeger
{
    public class ObjectHelper
    {
        public static bool ExistProperty(object obj, string propertyName)
        {
            Require.NotNull(obj, "obj");
            Require.NotNullOrEmpty(propertyName, "propertyName");

            return GetPropertyDescriptor(obj, propertyName) != null;
        }

        public static bool TryGetProperty(object obj, string propertyName, out object value)
        {
            Require.NotNull(obj, "obj");
            Require.NotNullOrEmpty(propertyName, "propertyName");

            PropertyDescriptor propDescriptor = GetPropertyDescriptor(obj, propertyName);
            if (propDescriptor != null)
            {
                value = propDescriptor.GetValue(obj);
                return true;
            }
            value = null;
            return false;
        }

        public static object GetProperty(object obj, string propertyName)
        {
            Require.NotNull(obj, "obj");
            Require.NotNullOrEmpty(propertyName, "propertyName");

            PropertyDescriptor propDescriptor = GetPropertyDescriptor(obj, propertyName);
            if (propDescriptor == null)
            {
                throw new InvalidOperationException(String.Format("Property '{0}' was not found at type '{1}'.", propertyName, obj.GetType().FullName));
            }

            return propDescriptor.GetValue(obj);
        }

        public static bool TrySetProperty(object obj, string propertyName, object value)
        {
            Require.NotNull(obj, "obj");
            Require.NotNullOrEmpty(propertyName, "propertyName");

            PropertyDescriptor propDescriptor = GetPropertyDescriptor(obj, propertyName);
            if (propDescriptor != null)
            {
                propDescriptor.SetValue(obj, value);
                return true;
            }
            return false;
        }

        public static void SetProperty(object obj, string propertyName, object value)
        {
            Require.NotNull(obj, "obj");
            Require.NotNullOrEmpty(propertyName, "propertyName");

            PropertyDescriptor propDescriptor = GetPropertyDescriptor(obj, propertyName);

            if (propDescriptor == null)
            {
                throw new InvalidOperationException(String.Format("Property '{0}' was not found at type '{1}'.", propertyName, obj.GetType().FullName));
            }

            propDescriptor.SetValue(obj, value);
        }

        private static PropertyDescriptor GetPropertyDescriptor(object obj, string propertyName)
        {
            return TypeDescriptor.GetProperties(obj).Find(propertyName, false);
        }

    }
}
