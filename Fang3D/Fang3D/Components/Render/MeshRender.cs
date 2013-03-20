using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Math;

namespace Fang3D
{
    public class MeshRender : Render
    {
        private Mesh mesh;
        private Texture2D texture;

        public Mesh Mesh
        {
            get { return mesh; }
            set { this.mesh = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { this.texture = value; }
        }

        public override void Draw(IBaseRender render)
        {
            if (Entity != null && Mesh != null)
            {
                if (texture != null)
                    render.Draw(mesh, Entity.Transformation.GlobalMatrix, renderMode, texture);
                else
                    render.Draw(mesh, Entity.Transformation.GlobalMatrix, renderMode);
            }
        }
    }
}
