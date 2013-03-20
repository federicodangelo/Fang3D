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
    public partial class EditorVector3 : EditorBase
    {
        private bool actualizando;

        private bool autoIncrementar = false;
        private Point posAutoIncrementar;
        private float valorInicial;

        public EditorVector3(AccesorValor accesor)
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

                float cambio = Math.Abs(valorInicial * 0.05f);
                if (Math.Round(cambio, 3) == 0)
                    cambio = 0.05f;

                float val = valorInicial + cambio * dy;

                if (((Label) sender).Name == "labelX")
                    textBoxX.Text = Math.Round(val, 3).ToString(culture);
                else if (((Label) sender).Name == "labelY")
                    textBoxY.Text = Math.Round(val, 3).ToString(culture);
                else if (((Label) sender).Name == "labelZ")
                    textBoxZ.Text = Math.Round(val, 3).ToString(culture);
            }
        }

        void label_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                autoIncrementar = false;
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
                    posAutoIncrementar = e.Location;
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

                Vector3 vec = (Vector3)accesor.GetValor();

                textBoxX.Text = Math.Round(vec.X, 3).ToString(culture);
                textBoxY.Text = Math.Round(vec.Y, 3).ToString(culture);
                textBoxZ.Text = Math.Round(vec.Z, 3).ToString(culture);
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
                    accesor.SetValor(new Vector3(x, y, z));
                }
            }
        }
    }
}
