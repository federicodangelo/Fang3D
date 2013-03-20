using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using OpenTK.Math;

namespace Fang3D
{
    public class MeshImporter
    {
        static System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;

        static public bool LoadMesh(Mesh mesh, String fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(fileName);

                XmlNodeList library_materials = xDoc.GetElementsByTagName("library_materials");
                XmlNodeList library_effects = xDoc.GetElementsByTagName("library_effects");
                XmlNodeList library_geometries = xDoc.GetElementsByTagName("library_geometries");

                if (library_geometries != null && library_geometries.Count > 0)
                    LoadGeometries(mesh, library_geometries[0]);

                Console.WriteLog(fileName + " : Cargado con exito");

                return true;
            }

            return false;
        }

        static private void LoadGeometries(Mesh mesh, XmlNode library_geometries)
        {
            foreach (XmlNode node in library_geometries.ChildNodes)
            {
                if (node.Name == "geometry")
                {
                    foreach (XmlNode node2 in node)
                    {
                        if (node2.Name == "mesh")
                        {
                            List<FloatArray> floatArrays = new List<FloatArray>();
                            String vertexPositionName = null;

                            foreach (XmlNode node3 in node2)
                            {
                                if (node3.Name == "source")
                                {
                                    String id = node3.Attributes.GetNamedItem("id").InnerText;

                                    foreach (XmlNode node4 in node3)
                                    {
                                        if (node4.Name == "float_array")
                                        {
                                            int count = Int32.Parse(node4.Attributes.GetNamedItem("count").InnerText, culture);

                                            FloatArray floatArray = new FloatArray();
                                            floatArray.name = id;
                                            floatArray.floats = new float[count];

                                            LoadFloats(node4.InnerText, floatArray.floats);

                                            floatArrays.Add(floatArray);
                                        }
                                    }
                                }
                                else if (node3.Name == "vertices")
                                {
                                    String id = node3.Attributes.GetNamedItem("id").InnerText;

                                    foreach (XmlNode node4 in node3)
                                    {
                                        if (node4.Name == "input")
                                        {
                                            String semantic = node4.Attributes.GetNamedItem("semantic").InnerText;
                                            String source = node4.Attributes.GetNamedItem("source").InnerText;

                                            if (semantic == "POSITION")
                                            {
                                                vertexPositionName = source;
                                            }
                                        }
                                    }
                                }
                                else if (node3.Name == "polygons")
                                {
                                    int countPolygons = Int32.Parse(node3.Attributes.GetNamedItem("count").InnerText, culture);

                                    List<uint>[] polygonData = new List<uint>[countPolygons];

                                    String nombreVertex = null;
                                    int offsetVertex = -1;
                                    String nombreNormal = null;
                                    int offsetNormal = -1;
                                    String nombreTexCoord = null;
                                    int offsetTexCoord = -1;

                                    int offsetPolygon = 0;

                                    foreach (XmlNode node4 in node3)
                                    {
                                        if (node4.Name == "input")
                                        {
                                            String semantic = node4.Attributes.GetNamedItem("semantic").InnerText;
                                            int offset = Int32.Parse(node4.Attributes.GetNamedItem("offset").InnerText, culture);
                                            String source = node4.Attributes.GetNamedItem("source").InnerText;

                                            if (semantic == "VERTEX")
                                            {
                                                nombreVertex = source;
                                                offsetVertex = offset;
                                            }
                                            else if (semantic == "NORMAL")
                                            {
                                                nombreNormal = source;
                                                offsetNormal = offset;
                                            }
                                            else if (semantic == "TEXCOORD")
                                            {
                                                nombreTexCoord = source;
                                                offsetTexCoord = offset;
                                            }
                                        }
                                        else if (node4.Name == "p")
                                        {
                                            List<uint> ints = new List<uint>();

                                            String[] texts = node4.InnerText.Split(' ');

                                            foreach (String textInt in texts)
                                            {
                                                String t = textInt.Trim().Replace(',', '.');

                                                if (t.Length > 0)
                                                    ints.Add(uint.Parse(t, culture));
                                            }

                                            if (ints.Count > 0)
                                                polygonData[offsetPolygon++] = ints;
                                        }
                                    }

                                    //Vertices
                                    if (nombreVertex != null && vertexPositionName != null)
                                    {
                                        foreach (FloatArray fa in floatArrays)
                                        {
                                            if (fa.name == vertexPositionName.Remove(0, 1))
                                            {
                                                mesh.vertexList = new Vector3[fa.floats.Length / 3];

                                                for (int k = 0; k < fa.floats.Length; k += 3)
                                                {
                                                    mesh.vertexList[k / 3] = new Vector3(
                                                        fa.floats[k],
                                                        fa.floats[k + 1],
                                                        fa.floats[k + 2]);
                                                }
                                                break;
                                            }
                                        }
                                    }

                                    //Normales
                                    FloatArray faNormales = null;
                                    if (nombreNormal != null)
                                    {
                                        foreach (FloatArray fa in floatArrays)
                                        {
                                            if (fa.name == nombreNormal.Remove(0, 1))
                                            {
                                                faNormales = fa;
                                                mesh.normalList = new Vector3[polygonData.Length * 3 /*fa.floats.Length / 3*/];
                                                break;
                                            }
                                        }
                                    }

                                    //Coordenadas texturas
                                    FloatArray faTexCoord = null;
                                    if (nombreTexCoord != null)
                                    {
                                        foreach (FloatArray fa in floatArrays)
                                        {
                                            if (fa.name == nombreTexCoord.Remove(0, 1))
                                            {
                                                faTexCoord = fa;
                                                mesh.coordList = new Vector2[polygonData.Length * 3 /*fa.floats.Length / 2*/];
                                                break;
                                            }
                                        }
                                    }

                                    //Indices vertices
                                    mesh.faceList = new uint[polygonData.Length * 3];
                                    int offsetFaces = 0;

                                    foreach (List<uint> ints in polygonData)
                                    {
                                        uint offsetVertex1 = ints[offsetVertex];
                                        uint offsetVertex2 = ints[offsetVertex + 3];
                                        uint offsetVertex3 = ints[offsetVertex + 6];

                                        int offsetInicial = offsetFaces;

                                        mesh.faceList[offsetFaces++] = offsetVertex1;
                                        mesh.faceList[offsetFaces++] = offsetVertex2;
                                        mesh.faceList[offsetFaces++] = offsetVertex3;

                                        if (offsetNormal != -1)
                                        {
                                            uint offsetNormal1 = ints[offsetNormal];
                                            uint offsetNormal2 = ints[offsetNormal + 3];
                                            uint offsetNormal3 = ints[offsetNormal + 6];

                                            mesh.normalList[offsetVertex1] = new Vector3(
                                                faNormales.floats[offsetNormal1 * 3],
                                                faNormales.floats[offsetNormal1 * 3 + 1],
                                                faNormales.floats[offsetNormal1 * 3 + 2]);

                                            mesh.normalList[offsetVertex2] = new Vector3(
                                                faNormales.floats[offsetNormal2 * 3],
                                                faNormales.floats[offsetNormal2 * 3 + 1],
                                                faNormales.floats[offsetNormal2 * 3 + 2]);

                                            mesh.normalList[offsetVertex3] = new Vector3(
                                                faNormales.floats[offsetNormal3 * 3],
                                                faNormales.floats[offsetNormal3 * 3 + 1],
                                                faNormales.floats[offsetNormal3 * 3 + 2]);
                                        }

                                        if (offsetTexCoord != -1)
                                        {
                                            uint offsetTexCoord1 = ints[offsetTexCoord];
                                            uint offsetTexCoord2 = ints[offsetTexCoord + 3];
                                            uint offsetTexCoord3 = ints[offsetTexCoord + 6];

                                            mesh.coordList[offsetVertex1] = new Vector2(
                                                faTexCoord.floats[offsetTexCoord1 * 2],
                                                faTexCoord.floats[offsetTexCoord1 * 2 + 1]);

                                            mesh.coordList[offsetVertex2] = new Vector2(
                                                faTexCoord.floats[offsetTexCoord2 * 2],
                                                faTexCoord.floats[offsetTexCoord2 * 2 + 1]);

                                            mesh.coordList[offsetVertex3] = new Vector2(
                                                faTexCoord.floats[offsetTexCoord3 * 2],
                                                faTexCoord.floats[offsetTexCoord3 * 2 + 1]);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    break;
                }
            }
        }

        static private void LoadFloats(String text, float[] floats)
        {
            String[] texts = text.Split(new char[] { ' ', '\r', '\n' });

            int offset = 0;

            foreach (String textFloat in texts)
            {
                String t = textFloat.Trim().Replace(',', '.');

                if (t.Length > 0)
                    floats[offset++] = float.Parse(t, culture);
            }
        }

        private class FloatArray
        {
            public string name;
            public float[] floats;
        }
    }
}
