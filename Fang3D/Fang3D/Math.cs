using OpenTK.Math;

namespace Fang3D
{
    static public class Math
    {
        public const float PI = (float) System.Math.PI;
        public const float HALF_PI = (float)System.Math.PI / 2.0f;
        public const float PI2 = (float)System.Math.PI * 2.0f;

        static public float Sin(float d)
        {
            return (float)System.Math.Sin(d);
        }

        static public float Cos(float d)
        {
            return (float)System.Math.Cos(d);
        }

        static public float Tan(float d)
        {
            return (float)System.Math.Tan(d);
        }

        static public float Asin(float d)
        {
            return (float) System.Math.Asin(d);
        }

        static public float Acos(float d)
        {
            return (float)System.Math.Acos(d);
        }

        static public float Atan(float d)
        {
            return (float)System.Math.Atan(d);
        }

        static public float Atan2(float y, float x)
        {
            return (float)System.Math.Atan2(y, x);
        }

        static public float Abs(float d)
        {
            return System.Math.Abs(d);
        }

        static public float Round(float d)
        {
            return (float) System.Math.Round(d);
        }

        static public float Round(float d, int decimals)
        {
            return (float)System.Math.Round(d, decimals);
        }

        static public float Sign(float n)
        {
            return System.Math.Sign(n);
        }

        static public float Max(float v1, float v2)
        {
            return System.Math.Max(v1, v2);
        }

        static public float Min(float v1, float v2)
        {
            return System.Math.Min(v1, v2);
        }

        static public Vector3 TranslationFromMatrix(Matrix4 mat)
        {
            return mat.Row3.Xyz;
        }

        static public Vector3 ScaleFromMatrix(Matrix4 mat)
        {
            return new Vector3(mat.Row0.Length, mat.Row1.Length, mat.Row2.Length);
        }

        static public float RoundRotation(float rotationRadians)
        {
            while (rotationRadians > PI)
                rotationRadians -= Math.PI2;

            while (rotationRadians < -PI)
                rotationRadians += Math.PI2;

            if (Math.Abs(rotationRadians) <= 0.000001f)
                rotationRadians = 0;

            return rotationRadians;
        }

        static public Vector3 RoundRotation(Vector3 vecRotationRadians)
        {
            vecRotationRadians.X = RoundRotation(vecRotationRadians.X);
            vecRotationRadians.Y = RoundRotation(vecRotationRadians.Y);
            vecRotationRadians.Z = RoundRotation(vecRotationRadians.Z);

            return vecRotationRadians;
        }

        static public Vector3 RotationFromMatrix(Matrix4 mat)
        {
            Vector3 scale = ScaleFromMatrix(mat);
            scale.X = 1.0f / scale.X;
            scale.Y = 1.0f / scale.Y;
            scale.Z = 1.0f / scale.Z;

            Matrix4 matSinScale = Matrix4.Scale(scale) * mat;

            // We will compute the Euler angle values in radians and store them here:
            float h, p, b;

            float m11 = Math.RoundRotation(matSinScale.Row0.X), m12 = Math.RoundRotation(matSinScale.Row0.Y), m13 = Math.RoundRotation(matSinScale.Row0.Z);
            float m21 = Math.RoundRotation(matSinScale.Row1.X), m22 = Math.RoundRotation(matSinScale.Row1.Y), m23 = Math.RoundRotation(matSinScale.Row1.Z);
            float m31 = Math.RoundRotation(matSinScale.Row2.X), m32 = Math.RoundRotation(matSinScale.Row2.Y), m33 = Math.RoundRotation(matSinScale.Row2.Z);

            // Extract pitch from m23, being careful for domain errors with asin(). We could have
            // values slightly out of range due to floating point arithmetic.
            float sp = -m23;
            if (sp <= -1.0f)
            {
                p = -HALF_PI; // –pi/2
            }
            else if (sp >= 1.0f)
            {
                p = HALF_PI; // pi/2
            }
            else
            {
                p = (float)Math.Asin(sp);
            }
            // Check for the Gimbal lock case, giving a slight tolerance
            // for numerical imprecision
            if (sp > 0.9999f)
            {
                // We are looking straight up or down.
                // Slam bank to zero and just set heading
                b = 0.0f;
                h = (float)Math.Atan2(-m31, m11);
            }
            else
            {
                // Compute heading from m13 and m33
                h = (float)Math.Atan2(m13, m33);
                // Compute bank from m21 and m22
                b = (float)Math.Atan2(m21, m22);
            }

            /*p = (float) Math.Round(p, 4);
            b = (float) Math.Round(b, 4);
            h = (float) Math.Round(h, 4);*/

            return new Vector3(-p, -h, -b);
        }

        static public Matrix4 RotationMatrixFromEuler(Vector3 vecRotation)
        {
            Matrix4 matRotation = Matrix4.RotateX(vecRotation.X) * Matrix4.RotateZ(vecRotation.Z) * Matrix4.RotateY(vecRotation.Y);

            return matRotation;
        }
    }
}
