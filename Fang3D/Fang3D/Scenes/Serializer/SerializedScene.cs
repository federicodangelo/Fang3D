using System;
using System.Collections.Generic;
using System.Text;

namespace Fang3D.Scenes.Serializer
{
    public class SerializedScene
    {
        static System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;

        public List<ComponentXML> xmlComponents;
        private Dictionary<uint, Component> handledComponents;

        public SerializedScene()
        {
            xmlComponents = new List<ComponentXML>();
            handledComponents = new Dictionary<uint, Component>();
            handledComponents.Add(Entity.Root.Id, Entity.Root);
        }

        public void LoadComponents()
        {
            foreach (ComponentXML compXml in xmlComponents)
                LoadComponent(compXml);
        }

        public void SaveComponents()
        {
            foreach (Component comp in Component.AllComponents)
                if (comp != Entity.Root)
                    SaveComponent(comp);
        }

        private void SaveComponent(Component comp)
        {
            if (handledComponents.ContainsKey(comp.Id) == false)
            {
                handledComponents.Add(comp.Id, comp);

                ComponentXML xmlComponent = new ComponentXML();

                if (comp is Script)
                {
                    xmlComponent.id = comp.Id;
                    xmlComponent.type = typeof(Script).FullName;
                    SaveValueComponent(xmlComponent, new FieldValue("__ScriptName__", typeof(String), comp.GetType().Name));
                }
                else
                {
                    xmlComponent.id = comp.Id;
                    xmlComponent.type = comp.GetType().FullName;
                }

                FieldValue[] valores = comp.GetValores();

                foreach (FieldValue valor in valores)
                    SaveValueComponent(xmlComponent, valor);

                xmlComponents.Add(xmlComponent);
            }
        }

        private void LoadComponent(ComponentXML xmlComponent)
        {
            Type tipoComponente;
            
            if (xmlComponent.type == typeof(Script).FullName)
            {
                FieldValueXML fvScriptName = xmlComponent.FindAttribute("__ScriptName__");

                if (fvScriptName != null && fvScriptName.value != null)
                {
                    ScriptResource scriptRes = (ScriptResource)Resource.FindResource(typeof(ScriptResource), (String)fvScriptName.ValueAs(typeof(String)));

                    if (scriptRes != null)
                        tipoComponente = scriptRes.ScriptType;
                    else
                        tipoComponente = null;
                }
                else
                {
                    tipoComponente = null;
                }
            }
            else
            {
                tipoComponente = Type.GetType(xmlComponent.type);
            }

            if (tipoComponente != null)
            {
                Component comp = (Component)tipoComponente.GetConstructor(Type.EmptyTypes).Invoke(null);

                comp.ForceId(xmlComponent.id);

                handledComponents.Add(comp.Id, comp);

                AccesorValor[] accesoresValores = comp.GetAccesoresValores();

                object val;

                foreach (AccesorValor accesor in accesoresValores)
                    if (LoadValueComponent(xmlComponent, accesor, out val))
                        accesor.Value = val;
            }
        }

        private bool LoadValueComponent(ComponentXML xmlComponent, AccesorValor accesor, out object val)
        {
            FieldValueXML fv = xmlComponent.FindAttribute(accesor.ValueName);

            Type tipo = accesor.ValueType;

            if (fv != null && fv.value != null)
            {
                if (typeof(Component).IsAssignableFrom(tipo))
                {
                    uint id = (uint)fv.ValueAs(typeof(uint));

                    Component otComp;

                    if (id != uint.MaxValue)
                    {
                        if (handledComponents.TryGetValue(id, out otComp) == false)
                        {
                            otComp = null;
                        }
                    }
                    else
                        otComp = Entity.Root;

                    if (otComp != null && tipo.IsAssignableFrom(otComp.GetType()))
                    {
                        val = otComp;
                        return true;
                    }
                }
                else if (typeof(Resource).IsAssignableFrom(tipo))
                {
                    String resName = (String)fv.ValueAs(typeof(string));

                    Resource res = Resource.FindResource(tipo, resName);

                    if (res != null)
                    {
                        val = res;
                        return true;
                    }
                    else
                    {
                        val = null;
                        return false;
                    }
                }
                else if (tipo.IsEnum)
                {
                    try
                    {
                        val = Enum.Parse(tipo, (String)fv.ValueAs(typeof(String)), true);
                        return true;
                    }
                    catch (ArgumentException)
                    {
                    }
                }
                else
                {
                    val = fv.ValueAs(tipo);
                    return true;
                }
            }

            val = null;
            return false;
        }

        private void SaveValueComponent(ComponentXML xmlComponent, FieldValue valor)
        {
            if (valor.value != null)
            {
                if (typeof(Component).IsAssignableFrom(valor.type))
                {
                    Component compVal = (Component)valor.value;

                    if (compVal != Entity.Root)
                        SaveComponent(compVal);

                    if (compVal != Entity.Root)
                        xmlComponent.attributes.Add(new FieldValueXML(valor.name, compVal.Id));
                    else
                        xmlComponent.attributes.Add(new FieldValueXML(valor.name, uint.MaxValue));
                }
                else if (typeof(Resource).IsAssignableFrom(valor.type))
                {
                    Resource resVal = (Resource)valor.value;

                    xmlComponent.attributes.Add(new FieldValueXML(valor.name, resVal.Name));
                }
                else if (valor.type.IsEnum)
                {
                    xmlComponent.attributes.Add(new FieldValueXML(valor.name, valor.value.ToString()));
                }
                else if (FieldValueXML.CanSerialize(valor.value))
                {
                    xmlComponent.attributes.Add(new FieldValueXML(valor.name, valor.value));
                }
                else
                {
                    throw new NotSupportedException("No se puede serializar el tipo de dato: " + valor.value.GetType());
                }
            }
            else
            {
                xmlComponent.attributes.Add(new FieldValueXML(valor.name, null));
            }
        }

        #region Clases auxiliares

        public class ComponentXML
        {
            public uint id;
            public String type = "";
            public List<FieldValueXML> attributes = new List<FieldValueXML>();

            public FieldValueXML FindAttribute(String name)
            {
                foreach (FieldValueXML fv in attributes)
                    if (fv.name == name)
                        return fv;

                return null;
            }
        }

        public class FieldValueXML
        {
            static System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;

            public String name;
            public String value;

            public FieldValueXML()
            {
            }

            public FieldValueXML(String name, object val)
            {
                this.name = name;
                this.value = ConvertTo(val);
            }

            public object ValueAs(Type destination)
            {
                return ConvertFrom(value, destination);
            }

            static public bool CanSerialize(object obj)
            {
                if (obj is float ||
                    obj is bool ||
                    obj is int ||
                    obj is uint ||
                    obj is String ||
                    obj is OpenTK.Math.Vector3 ||
                    obj is OpenTK.Math.Quaternion ||
                    obj is OpenTK.Graphics.Color4)
                {
                    return true;
                }

                return false;
            }

            static public String ConvertTo(object obj)
            {
                if (obj == null)
                    return null;
                else if (obj is String)
                    return (String)obj;
                else if (obj is float)
                    return ((float)obj).ToString(culture);
                else if (obj is bool)
                    return ((bool)obj).ToString(culture);
                else if (obj is int)
                    return ((int)obj).ToString(culture);
                else if (obj is uint)
                    return ((uint)obj).ToString(culture);
                else if (obj is OpenTK.Math.Vector3)
                    return String.Format("{0},{1},{2}", ((OpenTK.Math.Vector3)obj).X.ToString(culture), ((OpenTK.Math.Vector3)obj).Y.ToString(culture), ((OpenTK.Math.Vector3)obj).Z.ToString(culture));
                else if (obj is OpenTK.Math.Quaternion)
                    return String.Format("{0},{1},{2},{3}", ((OpenTK.Math.Quaternion)obj).X.ToString(culture), ((OpenTK.Math.Quaternion)obj).Y.ToString(culture), ((OpenTK.Math.Quaternion)obj).Z.ToString(culture), ((OpenTK.Math.Quaternion)obj).W.ToString(culture));
                else if (obj is OpenTK.Graphics.Color4)
                    return String.Format("{0},{1},{2},{3}", ((OpenTK.Graphics.Color4)obj).R.ToString(culture), ((OpenTK.Graphics.Color4)obj).G.ToString(culture), ((OpenTK.Graphics.Color4)obj).B.ToString(culture), ((OpenTK.Graphics.Color4)obj).A.ToString(culture));

                throw new FormatException("Tipo de dato no soportado: " + obj.GetType());
            }

            static public object ConvertFrom(String val, Type destination)
            {
                if (destination == typeof(String))
                    return (String) val;
                else if (destination == typeof(float))
                    return float.Parse(val, culture);
                else if (destination == typeof(bool))
                    return bool.Parse(val);
                else if (destination == typeof(int))
                    return int.Parse(val, culture);
                else if (destination == typeof(uint))
                    return uint.Parse(val, culture);
                else if (destination == typeof(OpenTK.Math.Vector3))
                {
                    float[] floats = ParseStringVector(val);
                    return new OpenTK.Math.Vector3(floats[0], floats[1], floats[2]);
                }
                else if (destination == typeof(OpenTK.Math.Quaternion))
                {
                    float[] floats = ParseStringVector(val);
                    return new OpenTK.Math.Quaternion(floats[0], floats[1], floats[2], floats[3]);
                }
                else if (destination == typeof(OpenTK.Graphics.Color4))
                {
                    float[] floats = ParseStringVector(val);
                    return new OpenTK.Graphics.Color4(floats[0], floats[1], floats[2], floats[3]);
                }

                throw new FormatException("Tipo de dato no soportado: " + destination);
            }

            static private float[] ParseStringVector(String val)
            {
                String[] vals = val.Split(',');
                float[] floats = new float[vals.Length];
                for (int i = 0; i < vals.Length; i++)
                    floats[i] = float.Parse(vals[i], culture);
                return floats;
            }

        }        
        #endregion
    }
}
