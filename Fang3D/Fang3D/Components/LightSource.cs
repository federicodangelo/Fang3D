using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Math;
using OpenTK.Graphics;

namespace Fang3D
{
    public enum LightType
    {
        OMNI,
        SPOT
    }

    public class LightSource : Component
    {
        private Color4 ambientColor;
        private Color4 diffuseColor;
        private Mesh mesh;
        private float distance;
        private bool quadratic;
        private LightType lightType = LightType.OMNI;
        private float spotCutoff = 45.0f; //Max: 90.0f
        private float spotExponent = 0.0f; //Max: 128.0f
        private int priority;

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public float SpotCutoff
        {
            get { return spotCutoff; }
            set { spotCutoff = value; }
        }

        public float SpotExponent
        {
            get { return spotExponent; }
            set { spotExponent = value; }
        }

        public float Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public bool Quadratic
        {
            get { return quadratic; }
            set { quadratic = value; }
        }

        public Color4 AmbientColor
        {
            get { return ambientColor; }
            set { this.ambientColor = value; }
        }

        public Color4 DiffuseColor
        {
            get { return diffuseColor; }
            set { this.diffuseColor = value; }
        }

        public LightType LightType
        {
            get { return lightType; }
            set { this.lightType = value; }
        }

        public void DrawHelper(IBaseRender render)
        {
            if (mesh == null)
                mesh = (Mesh)Resource.FindResource(typeof(Mesh), "Light");

            if (Entity != null && mesh != null)
            {
                Matrix4 mat = Entity.Transformation.GlobalMatrix;
                Vector3 scale = Entity.Transformation.GlobalScale;
                mat = Matrix4.Scale(1.0f / scale.X, 1.0f / scale.Y, 1.0f / scale.Z) * mat;
                render.Draw(mesh, mat, RenderMode.Normal , new Color4(1.0f, 1.0f, 0.0f, 0.0f));
            }
        }
    }
}
