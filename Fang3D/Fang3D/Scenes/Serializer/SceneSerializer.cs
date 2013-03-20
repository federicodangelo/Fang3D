using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Fang3D.Scenes.Serializer
{
    public class SceneSerializer
    {
        public const int SERIALIZER_VERSION = 1;

        static System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;

        static public void Save(Scene scene, Stream stream)
        {
            Scene.PushCurrentScene();

            try
            {
                scene.MakeCurrent();

                SerializedScene serializedScene = new SerializedScene();

                serializedScene.SaveComponents();

                SaveToStream(serializedScene, stream);
            }
            finally
            {
                Scene.PopCurrentScene();
            }
        }

        static public void Load(Scene scene, Stream stream)
        {
            Scene.PushCurrentScene();

            try
            {
                scene.MakeCurrent();

                scene.DestroyAllComponents();

                SerializedScene serializedScene = new SerializedScene();

                LoadFromStream(serializedScene, stream);

                serializedScene.LoadComponents();
            }
            finally
            {
                Scene.PopCurrentScene();
            }
        }

        static public void Load(Scene scene, String fileName)
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                try
                {
                    Load(scene, r.BaseStream);

                    Console.WriteLog(fileName + " : Cargado con exito");
                }
                finally
                {
                    r.Close();
                }
            }
        }


        static public void Save(Scene scene, String fileName)
        {
            using (StreamWriter w = new StreamWriter(fileName))
            {
                try
                {
                    Save(scene, w.BaseStream);

                    Console.WriteLog(fileName + " : Salvado con exito");
                }
                finally
                {
                    w.Close();
                }
            }
        }

        static public void SaveToStream(SerializedScene serializedScene, Stream stream)
        {
            XmlDocument xDoc = new XmlDocument();

            xDoc.AppendChild(xDoc.CreateNode(XmlNodeType.XmlDeclaration,"",""));

            XmlElement xScene = xDoc.CreateElement("", "Scene", "");
            XmlAttribute xVersion = xDoc.CreateAttribute("version");
            xVersion.Value = SERIALIZER_VERSION.ToString(culture);
            xScene.Attributes.Append(xVersion);
            XmlElement xComponents = xDoc.CreateElement("", "Components", "");

            xScene.AppendChild(xComponents);

            foreach (SerializedScene.ComponentXML compXML in serializedScene.xmlComponents)
            {
                XmlNode xComponent = xDoc.CreateNode(XmlNodeType.Element, "Component", "");

                XmlAttribute xType = xDoc.CreateAttribute("type");
                xType.Value = compXML.type;

                XmlAttribute xID = xDoc.CreateAttribute("id");
                xID.Value = compXML.id.ToString(culture);

                xComponent.Attributes.Append(xType);
                xComponent.Attributes.Append(xID);

                XmlNode xValues = xDoc.CreateNode(XmlNodeType.Element, "Values", "");
                xComponent.AppendChild(xValues);

                foreach (SerializedScene.FieldValueXML valXML in compXML.attributes)
                {
                    XmlNode xValue = xDoc.CreateNode(XmlNodeType.Element, valXML.name, "");

                    if (valXML.value != null)
                    {
                        XmlAttribute xVal = xDoc.CreateAttribute("val");
                        xVal.Value = valXML.value;
                        xValue.Attributes.Append(xVal);
                    }

                    xValues.AppendChild(xValue);
                }

                xComponents.AppendChild(xComponent);
            }

            xDoc.AppendChild(xScene);

            xDoc.Save(stream);
        }

        static private XmlNode FindNode(XmlNode node, String name)
        {
            foreach (XmlNode child in node.ChildNodes)
                if (child.Name == name)
                    return child;

            return null;
        }

        static private XmlAttribute FindAttribute(XmlNode node, String name)
        {
            foreach (XmlAttribute child in node.Attributes)
                if (child.Name == name)
                    return child;

            return null;
        }

        static public void LoadFromStream(SerializedScene serializedScene, Stream stream)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(stream);

            XmlNodeList xScenes = xDoc.GetElementsByTagName("Scene");
            XmlNode xScene = xScenes[0];
            String version = FindAttribute(xScene, "version").Value;

            if (int.Parse(version) != SERIALIZER_VERSION)
                throw new FormatException("Version de escena no soportada: " + version);

            XmlNodeList xComponentss = xDoc.GetElementsByTagName("Components");
            XmlNodeList xComponents = xComponentss[0].ChildNodes;

            foreach (XmlNode xComponent in xComponents)
            {
                string type = FindAttribute(xComponent, "type").Value;
                string id = FindAttribute(xComponent, "id").Value;

                SerializedScene.ComponentXML compXML = new SerializedScene.ComponentXML();
                compXML.id = uint.Parse(id);
                compXML.type = type;

                if (xComponent.ChildNodes.Count == 1 && xComponent.FirstChild.Name == "Values")
                {
                    foreach (XmlNode xValue in xComponent.FirstChild.ChildNodes)
                    {
                        String name = xValue.Name;
                        String val = null;

                        foreach (XmlAttribute xVal in xValue.Attributes)
                            if (xVal.Name == "val")
                                val = xVal.Value;

                        SerializedScene.FieldValueXML valXML = new SerializedScene.FieldValueXML();

                        valXML.name = name;
                        valXML.value = val;

                        compXML.attributes.Add(valXML);
                    }
                }

                serializedScene.xmlComponents.Add(compXML);
            }
        }
    }
}
