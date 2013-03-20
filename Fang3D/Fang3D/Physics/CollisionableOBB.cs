using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Math;

namespace Fang3D
{
    public class CollisionableOBB : Collisionable
    {
        private Vector3 size = new Vector3(1.0f, 1.0f, 1.0f);
        private float radio;

        public Vector3 Size
        {
            get { return size; }
            set 
            { 
                size = value;
                radio = Math.Max(Math.Max(Math.Abs(size.X), Math.Abs(size.Y)), Math.Abs(size.Z)) / 2.0f;
            }
        }

        public override bool CollidesWith(Collisionable col)
        {
            if (col is CollisionableOBB)
            {
                CollisionableOBB col2 = (CollisionableOBB)col;

                Matrix4 matThis = Entity.Transformation.GlobalMatrix;
                Matrix4 matOther = col2.Entity.Transformation.GlobalMatrix;

                Vector3 centerThis = Math.TranslationFromMatrix(matThis);
                Vector3 centerOther = Math.TranslationFromMatrix(matThis);

                if ((centerThis - centerOther).LengthSquared < (this.radio + col2.radio))
                {
                    Vector3 v1t = Vector3.Transform(-size / 2.0f, matThis).Xyz;
                    Vector3 v2t = Vector3.Transform(size / 2.0f, matThis).Xyz;

                    Vector3 v1o = Vector3.Transform(-col2.size / 2.0f, matOther).Xyz;
                    Vector3 v2o = Vector3.Transform(col2.size / 2.0f, matOther).Xyz;

                    Vector3 minThis = new Vector3(
                        System.Math.Min(v1t.X, v2t.X),
                        System.Math.Min(v1t.Y, v2t.Y),
                        System.Math.Min(v1t.Z, v2t.Z));

                    Vector3 maxThis = new Vector3(
                        System.Math.Max(v1t.X, v2t.X),
                        System.Math.Max(v1t.Y, v2t.Y),
                        System.Math.Max(v1t.Z, v2t.Z));

                    Vector3 minOther = new Vector3(
                        System.Math.Min(v1o.X, v2o.X),
                        System.Math.Min(v1o.Y, v2o.Y),
                        System.Math.Min(v1o.Z, v2o.Z));

                    Vector3 maxOther = new Vector3(
                        System.Math.Max(v1o.X, v2o.X),
                        System.Math.Max(v1o.Y, v2o.Y),
                        System.Math.Max(v1o.Z, v2o.Z));

                    if (minOther.X > maxThis.X ||
                        minOther.Y > maxThis.Y ||
                        minOther.Z > maxThis.Z ||
                        minThis.X > maxOther.X ||
                        minThis.Y > maxOther.Y ||
                        minThis.Z > maxOther.Z)
                    {
                        return false;
                    }

                    return true;
                }
            }

            return base.CollidesWith(col);
        }

        public override void DrawHelper(IBaseRender render)
        {
            if (RenderInEditor)
            {
                Mesh cube = (Mesh) Resource.FindResource(typeof(Mesh), "Cube");

                Matrix4 mat = Entity.Transformation.GlobalMatrix;

                mat = Matrix4.Scale(size) * mat;

                render.Draw(cube, mat, RenderMode.Lines, new OpenTK.Graphics.Color4(0.0f, 1.0f, 0.0f, 1.0f));

            }
        }
    }
}
