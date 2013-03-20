using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Fang3D.Attributes;

namespace Fang3D
{
    public class AccesorValor
    {
        private FieldInfo field;
        private PropertyInfo property;
        private object obj;

        public bool ShowInEditor
        {
            get
            {
                return (field != null && field.GetCustomAttributes(typeof(DontShowInEditor), true).Length == 0 ||
                        property != null && property.GetCustomAttributes(typeof(DontShowInEditor), true).Length == 0);
            }
        }

        public bool ReadOnly
        {
            get
            {
                if (property != null && property.CanWrite == false)
                    return true;

                if (property != null && property.GetCustomAttributes(typeof(ReadOnlyInEditor), false).Length > 0)
                    return true;

                if (field != null && field.GetCustomAttributes(typeof(ReadOnlyInEditor), false).Length > 0)
                    return true;

                return false;
            }
        }

        public Type ValueType
        {
            get
            {
                if (field != null)
                    return field.FieldType;
                else
                    return property.PropertyType;
            }
        }

        public String ValueName
        {
            get
            {
                if (field != null)
                    return field.Name;
                else
                    return property.Name;
            }
        }

        public object Value
        {
            get { return GetValor(); }
            set { SetValor(value); }
        }


        public AccesorValor(FieldInfo field, Object obj)
        {
            this.field = field;
            this.obj = obj;
        }

        public AccesorValor(PropertyInfo property, Object obj)
        {
            this.property = property;
            this.obj = obj;
        }

        public object GetValor()
        {
            if (field != null)
                return field.GetValue(obj);
            else
                return property.GetValue(obj, null);
        }

        public void SetValor(object val)
        {
            if (field != null)
                field.SetValue(obj, val);
            else
                property.SetValue(obj, val, null);
        }
    }
}
