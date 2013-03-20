using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Math;
using System.IO;
using System.Xml;

namespace Fang3D
{
    public class Mesh : Resource
    {
        public enum PrimitiveType
        {
            Triangle,
            Quad
        }

        public Vector3[] vertexList;
        public uint[] faceList;
        public uint[] colorList;
        public Vector2[] coordList;
        public Vector3[] normalList;
        public PrimitiveType primitiveType = PrimitiveType.Triangle;

        public Mesh()
        {
            Name = "Mesh";
        }

        public Mesh(double[] vertex, int[] faces) : this()
        {
            List<Vector3> ve = new List<Vector3>();

            for (int i = 0; i + 2 < vertex.Length; i += 3)
                ve.Add(new Vector3((float)vertex[i], (float)vertex[i + 1], (float)vertex[i + 2]));

            vertexList = ve.ToArray();

            List<uint> fa = new List<uint>();
            for (int i = 0; i + 3 < faces.Length; i += 4)
            {
                fa.Add((uint)faces[i]);
                fa.Add((uint)faces[i + 1]);
                fa.Add((uint)faces[i + 2]);
            }

            faceList = fa.ToArray();

            missing = false;
        }

        protected override void OnFileNameUpdated()
        {
            Name = Path.GetFileNameWithoutExtension(FileName);

            UnloadMesh();

            if (MeshImporter.LoadMesh(this, FileName))
                missing = false;

            base.OnFileNameUpdated();
        }

        public override void OnFileChanged()
        {
            UnloadMesh();

            if (MeshImporter.LoadMesh(this, FileName))
                missing = false;

            base.OnFileChanged();
        }

        public override void OnFileDeleted()
        {
            UnloadMesh();

            base.OnFileDeleted();
        }

        private void UnloadMesh()
        {
            vertexList = null;
            faceList = null;
            colorList = null;
            coordList = null;
            normalList = null;
            missing = true;
        }

        public override bool CanAssignTo(object obj)
        {
            if (obj is Scene)
                return true;

            return false;
        }

        public override object AssignTo(object obj)
        {
            if (obj is Scene)
            {
                Scene scene = (Scene)obj;
                
                Scene.PushCurrentScene();

                try
                {
                    scene.MakeCurrent();

                    Entity ent = new Entity();
                    MeshRender meshRender = new MeshRender();

                    ent.Name = Name;
                    meshRender.Entity = ent;
                    meshRender.Mesh = (Mesh) GetInstance();

                    return ent;
                }
                finally
                {
                    Scene.PopCurrentScene();
                }               
            }

            return null;
        }
    }
}
