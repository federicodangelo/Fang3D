using System.Collections.Generic;
using OpenTK.Math;
using Fang3D.Attributes;

namespace Fang3D
{
    public sealed class Transformation : Component
    {
        private Quaternion rotation = Quaternion.Identity;
        private Vector3 translation = Vector3.Zero;
        private Vector3 scale = new Vector3(1, 1, 1);

        private Matrix4 localMatrix = Matrix4.Identity;

        private Transformation parent;
        private List<Transformation> childs = new List<Transformation>(0);

        public Transformation()
        {
        }

        public Transformation(Transformation parent)
        {
            Parent = parent;
        }

        public IEnumerable<Transformation> Childs
        {
            get
            {
                return childs;
            }
        }

        public int ChildsCount
        {
            get
            {
                return childs.Count;
            }
        }

        [DontShowInEditor]
        public Transformation Parent
        {
            get
            {
                return parent;
            }
            set
            {
                if (parent != value)
                {
                    Matrix4 matGlobal = GlobalMatrix;
                    Vector3 rotGlobalPre = Math.RoundRotation(Math.RotationFromMatrix(matGlobal));

                    if (parent != null)
                        parent.childs.Remove(this);

                    parent = value;

                    if (parent != null)
                        parent.childs.Add(this);

                    if (!Destroyed && Entity != null)
                        Entity.ReportUpdated("Parent");

                    GlobalMatrix = matGlobal;

                    Matrix4 matGlobal2 = GlobalMatrix;
                    Vector3 rotGlobalPost = Math.RoundRotation(Math.RotationFromMatrix(matGlobal2));

                    if (rotGlobalPre == rotGlobalPost)
                    {
                    }
                }
            }
        }

        public override void Destroy()
        {
            if (!Destroyed)
            {
                base.Destroy();

                Parent = null;
            }
        }

        public Vector3 LocalTranslation
        {
            get
            {
                return translation;
            }
            set
            {
                translation = value;

                UpdateLocalMatrix();
            }
        }

        [DontStore]
        public Vector3 GlobalTranslation
        {
            get
            {
                return Math.TranslationFromMatrix(GlobalMatrix);
            }
        }

        public Quaternion LocalRotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;

                UpdateLocalMatrix();
            }
        }

        [DontStore]
        public Vector3 LocalRotationEuler
        {
            get
            {
                return Math.RotationFromMatrix(localMatrix);
            }
        }

        public Vector3 LocalScale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;

                UpdateLocalMatrix();
            }
        }

        [DontStore]
        public Vector3 GlobalScale
        {
            get
            {
                return Math.ScaleFromMatrix(GlobalMatrix);
            }
        }

        [DontStore]
        public Matrix4 LocalMatrix
        {
            get
            {
                if (Enabled)
                    return localMatrix;
                else
                    return Matrix4.Identity;
            }

            set
            {
                translation = Math.TranslationFromMatrix(value);
                scale = Math.ScaleFromMatrix(value);

                Vector3 eulerRot = Math.RotationFromMatrix(value);
                eulerRot = Math.RoundRotation(eulerRot);
                rotation =
                    Quaternion.FromAxisAngle(Vector3.UnitX, eulerRot.X) *
                    Quaternion.FromAxisAngle(Vector3.UnitZ, eulerRot.Z) *
                    Quaternion.FromAxisAngle(Vector3.UnitY, eulerRot.Y);

                UpdateLocalMatrix();
            }
        }

        public Vector3 DirectionX
        {
            get
            {
                return Vector3.Normalize(Vector3.Transform(Vector3.UnitX, GlobalMatrix).Xyz - GlobalTranslation);
            }
        }

        public Vector3 DirectionY
        {
            get
            {
                return Vector3.Normalize(Vector3.Transform(Vector3.UnitY, GlobalMatrix).Xyz - GlobalTranslation);
            }
        }

        public Vector3 DirectionZ
        {
            get
            {
                return Vector3.Normalize(Vector3.Transform(Vector3.UnitZ, GlobalMatrix).Xyz - GlobalTranslation);
            }
        }

        [DontStore]
        public Matrix4 GlobalMatrix
        {
            get
            {
                if (parent != null)
                    return LocalMatrix * parent.GlobalMatrix;

                return LocalMatrix;
            }

            set
            {
                if (parent != null)
                {
                    Matrix4 mat = parent.GlobalMatrix;
                    mat.Invert();
                    LocalMatrix = value * mat;
                }
                else
                {
                    LocalMatrix = value;
                }
            }
        }

        private void UpdateLocalMatrix()
        {
            Matrix4 matTranslation = Matrix4.Translation(translation);
            Matrix4 matRotation = Matrix4.Rotate(rotation);
            Matrix4 matScale = Matrix4.Scale(scale);

            localMatrix = matScale * matRotation * matTranslation;
        }
    }
}
