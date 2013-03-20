using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Fang3D.Attributes;

namespace Fang3D
{
    static internal class InternalFunctions
    {
        static public AccesorValor[] GetAccesoresValores(Object obj, bool includeReadOnly)
        {
            Type tipoComponente = obj.GetType();

            List<AccesorValor> accesoresValores = new List<AccesorValor>();

            PropertyInfo[] propiedades = tipoComponente.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            FieldInfo[] campos = tipoComponente.GetFields(BindingFlags.Instance | BindingFlags.Public);

            foreach (PropertyInfo prop in propiedades)
                if (prop.CanRead && (prop.CanWrite || includeReadOnly == true))
                    if (prop.GetCustomAttributes(typeof(DontStore), true).Length == 0)
                        accesoresValores.Add(new AccesorValor(prop, obj));

            foreach (FieldInfo field in campos)
                if (field.GetCustomAttributes(typeof(DontStore), true).Length == 0)
                    accesoresValores.Add(new AccesorValor(field, obj));

            return accesoresValores.ToArray();
        }

        static public FieldValue[] GetValores(Object obj)
        {
            List<FieldValue> values = new List<FieldValue>();

            List<AccesorValor> accesoresValores = new List<AccesorValor>();

            Type tipoComponente = obj.GetType();

            PropertyInfo[] propiedades = tipoComponente.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            FieldInfo[] campos = tipoComponente.GetFields(BindingFlags.Instance | BindingFlags.Public);

            foreach (PropertyInfo prop in propiedades)
                if (prop.CanWrite && prop.CanRead)
                    if (prop.GetCustomAttributes(typeof(DontStore), true).Length == 0)
                        values.Add(new FieldValue(prop.Name, prop.PropertyType, prop.GetValue(obj, null)));

            foreach (FieldInfo field in campos)
                if (field.GetCustomAttributes(typeof(DontStore), true).Length == 0)
                    values.Add(new FieldValue(field.Name, field.FieldType, field.GetValue(obj)));

            return values.ToArray();
        }

        static public void SetValores(Object obj, FieldValue[] values)
        {
            List<AccesorValor> accesoresValores = new List<AccesorValor>();

            Type tipoComponente = obj.GetType();

            PropertyInfo[] propiedades = tipoComponente.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            FieldInfo[] campos = tipoComponente.GetFields(BindingFlags.Instance | BindingFlags.Public);

            foreach (PropertyInfo prop in propiedades)
            {
                if (prop.CanWrite && prop.CanRead)
                    if (prop.GetCustomAttributes(typeof(DontStore), true).Length == 0)
                    {
                        foreach (FieldValue f in values)
                        {
                            if (f.name == prop.Name)
                            {
                                if (f.value != null && prop.PropertyType.IsAssignableFrom(f.value.GetType()))
                                    prop.SetValue(obj, f.value, null);
                                break;
                            }
                        }
                    }
            }

            foreach (FieldInfo field in campos)
            {
                if (field.GetCustomAttributes(typeof(DontStore), true).Length == 0)
                {
                    foreach (FieldValue f in values)
                    {
                        if (f.name == field.Name)
                        {
                            if (f.value != null && field.FieldType.IsAssignableFrom(f.value.GetType()))
                                field.SetValue(obj, f.value);
                            break;
                        }
                    }
                }
            }
        }
    }
}
