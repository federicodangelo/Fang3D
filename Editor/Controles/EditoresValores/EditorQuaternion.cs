using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenTK.Math;

namespace Fang3D.Editor.Controles.EditoresValores
{
    public partial class EditorQuaternion : EditorBase
    {
        private bool actualizando;

        private bool autoIncrementar = false;
        private Point posAutoIncrementar;
        private float valorInicial;

        private Quaternion quatInicial;

        public EditorQuaternion(AccesorValor accesor)
            : base(accesor)
        {
            InitializeComponent();

            ActualizarValor();

            labelX.MouseDown += new MouseEventHandler(label_MouseDown);
            labelX.MouseUp += new MouseEventHandler(label_MouseUp);
            labelX.MouseMove += new MouseEventHandler(label_MouseMove);

            labelY.MouseDown += new MouseEventHandler(label_MouseDown);
            labelY.MouseUp += new MouseEventHandler(label_MouseUp);
            labelY.MouseMove += new MouseEventHandler(label_MouseMove);

            labelZ.MouseDown += new MouseEventHandler(label_MouseDown);
            labelZ.MouseUp += new MouseEventHandler(label_MouseUp);
            labelZ.MouseMove += new MouseEventHandler(label_MouseMove);
        }


        void label_MouseMove(object sender, MouseEventArgs e)
        {
            if (autoIncrementar)
            {
                int dy = -(e.Y - posAutoIncrementar.Y);

                float cambioRadians = Functions.DegreesToRadians(dy);

                Quaternion quat = new Quaternion();

                if (((Label)sender).Name == "labelX")
                    quat = quatInicial * Quaternion.FromAxisAngle(Vector3.UnitX, cambioRadians);
                else if (((Label)sender).Name == "labelY")
                    quat = quatInicial * Quaternion.FromAxisAngle(Vector3.UnitY, cambioRadians);
                else if (((Label)sender).Name == "labelZ")
                    quat = quatInicial * Quaternion.FromAxisAngle(Vector3.UnitZ, cambioRadians);

                accesor.SetValor(quat);

                ActualizarValor();
            }
        }

        void label_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                autoIncrementar = false;
                ActualizarValor();
            }
        }

        void label_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && accesor.ReadOnly == false)
            {
                bool ok = false;

                switch (((Label)sender).Name)
                {
                    case "labelX":
                        ok = float.TryParse(textBoxX.Text, System.Globalization.NumberStyles.Float, culture, out valorInicial);
                        break;

                    case "labelY":
                        ok = float.TryParse(textBoxY.Text, System.Globalization.NumberStyles.Float, culture, out valorInicial);
                        break;

                    case "labelZ":
                        ok = float.TryParse(textBoxZ.Text, System.Globalization.NumberStyles.Float, culture, out valorInicial);
                        break;
                }

                if (ok)
                {
                    autoIncrementar = true;
                    quatInicial = (Quaternion) accesor.GetValor();
                    posAutoIncrementar = e.Location;

                    //Seteo el foco en un label asi si lo tiene alguno de los textBoxs, lo pierde
                    labelX.Focus();
                }
            }
        }

        public override void ActualizarValor()
        {
            if (textBoxX.Focused || textBoxY.Focused || textBoxZ.Focused)
                return;

            actualizando = true;

            try
            {
                textBoxX.ReadOnly = textBoxY.ReadOnly = textBoxZ.ReadOnly = accesor.ReadOnly;

                Quaternion quat = (Quaternion)accesor.GetValor();

                float p, h, b;

                float sp = -2.0f * (quat.Y * quat.Z - quat.W * quat.X);

                // Check for Gimbal lock, giving slight tolerance for numerical imprecision
                if (Math.Abs(sp) > 0.9999f)
                {
                    // Looking straight up or down
                    p = Math.HALF_PI * sp; // pi/2
                    // Compute heading, slam bank to zero
                    h = (float)Math.Atan2(-quat.X * quat.Z + quat.W * quat.Y, 0.5f - quat.Y * quat.Y - quat.Z * quat.Z);
                    b = 0.0f;
                }
                else
                {
                    // Compute angles
                    p = (float)Math.Asin(sp);
                    h = (float)Math.Atan2(quat.X * quat.Z + quat.W * quat.Y, 0.5f - quat.X * quat.X - quat.Y * quat.Y);
                    b = (float)Math.Atan2(quat.X * quat.Y + quat.W * quat.Z, 0.5f - quat.X * quat.X - quat.Z * quat.Z);
                }

	            /*float test = q1.X*q1.Y + q1.Z*q1.W;
	            if (test > 0.499f) { // singularity at north pole
		            h = 2 * Math.Atan2(q1.X,q1.W);
		            p = Math.HALF_PI;
		            b = 0;

                }
                else if (test < -0.499f)
                { // singularity at south pole
                    h = -2 * Math.Atan2(q1.X, q1.W);
                    p = -Math.HALF_PI;
                    b = 0;
                }
                else
                {
                    float sqx = q1.X * q1.X;
                    float sqy = q1.Y * q1.Y;
                    float sqz = q1.Z * q1.Z;
                    h = Math.Atan2(2 * q1.Y * q1.W - 2 * q1.X * q1.Z, 1 - 2 * sqy - 2 * sqz);
                    p = Math.Asin(2 * test);
                    b = Math.Atan2(2 * q1.X * q1.W - 2 * q1.Y * q1.Z, 1 - 2 * sqx - 2 * sqz);
                }*/

                p = Functions.RadiansToDegrees(p);
                h = Functions.RadiansToDegrees(h);
                b = Functions.RadiansToDegrees(b);

                textBoxX.Text = Math.Round(p, 3).ToString(culture);
                textBoxY.Text = Math.Round(h, 3).ToString(culture);
                textBoxZ.Text = Math.Round(b, 3).ToString(culture);
            }
            finally
            {
                actualizando = false;
            }
        }

        private void textBoxX_TextChanged(object sender, EventArgs e)
        {
            SetearValor();
        }

        private void textBoxY_TextChanged(object sender, EventArgs e)
        {
            SetearValor();
        }

        private void textBoxZ_TextChanged(object sender, EventArgs e)
        {
            SetearValor();
        }

        private void SetearValor()
        {
            if (actualizando == false && accesor.ReadOnly == false)
            {
                float x, y, z;

                if (float.TryParse(textBoxX.Text, System.Globalization.NumberStyles.Float, culture, out x) &&
                    float.TryParse(textBoxY.Text, System.Globalization.NumberStyles.Float, culture, out y) &&
                    float.TryParse(textBoxZ.Text, System.Globalization.NumberStyles.Float, culture, out z))
                {
                    x = Functions.DegreesToRadians(x);
                    y = Functions.DegreesToRadians(y);
                    z = Functions.DegreesToRadians(z);

                    Quaternion qX = Quaternion.FromAxisAngle(Vector3.UnitX, x);
                    Quaternion qY = Quaternion.FromAxisAngle(Vector3.UnitY, y);
                    Quaternion qZ = Quaternion.FromAxisAngle(Vector3.UnitZ, z);

                    Quaternion quat = qY * qX * qZ;

                    accesor.SetValor(quat);
                }
            }
        }
    }
}
