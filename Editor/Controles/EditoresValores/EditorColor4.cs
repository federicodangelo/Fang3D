using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenTK.Math;
using OpenTK.Graphics;

namespace Fang3D.Editor.Controles.EditoresValores
{
    public partial class EditorColor4 : EditorBase
    {
        private bool actualizando;

        private bool autoIncrementar = false;
        private Point posAutoIncrementar;
        private float valorInicial;

        public EditorColor4(AccesorValor accesor)
            : base(accesor)
        {
            InitializeComponent();

            ActualizarValor();

            labelR.MouseDown += new MouseEventHandler(label_MouseDown);
            labelR.MouseUp += new MouseEventHandler(label_MouseUp);
            labelR.MouseMove += new MouseEventHandler(label_MouseMove);

            labelG.MouseDown += new MouseEventHandler(label_MouseDown);
            labelG.MouseUp += new MouseEventHandler(label_MouseUp);
            labelG.MouseMove += new MouseEventHandler(label_MouseMove);

            labelB.MouseDown += new MouseEventHandler(label_MouseDown);
            labelB.MouseUp += new MouseEventHandler(label_MouseUp);
            labelB.MouseMove += new MouseEventHandler(label_MouseMove);
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

                if (((Label) sender).Name == "labelR")
                    textBoxR.Text = Math.Round(val, 3).ToString(culture);
                else if (((Label) sender).Name == "labelG")
                    textBoxG.Text = Math.Round(val, 3).ToString(culture);
                else if (((Label) sender).Name == "labelB")
                    textBoxB.Text = Math.Round(val, 3).ToString(culture);
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
                    case "labelR":
                        ok = float.TryParse(textBoxR.Text, System.Globalization.NumberStyles.Float, culture, out valorInicial);
                        break;

                    case "labelG":
                        ok = float.TryParse(textBoxG.Text, System.Globalization.NumberStyles.Float, culture, out valorInicial);
                        break;

                    case "labelB":
                        ok = float.TryParse(textBoxB.Text, System.Globalization.NumberStyles.Float, culture, out valorInicial);
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
            if (textBoxR.Focused || textBoxG.Focused || textBoxB.Focused)
                return;

            actualizando = true;

            try
            {
                textBoxR.ReadOnly = textBoxG.ReadOnly = textBoxB.ReadOnly = accesor.ReadOnly;

                Color4 col = (Color4) accesor.GetValor();

                textBoxR.Text = Math.Round(col.R, 3).ToString(culture);
                textBoxG.Text = Math.Round(col.G, 3).ToString(culture);
                textBoxB.Text = Math.Round(col.B, 3).ToString(culture);
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
                float r, g, b;

                if (float.TryParse(textBoxR.Text, System.Globalization.NumberStyles.Float, culture, out r) &&
                    float.TryParse(textBoxG.Text, System.Globalization.NumberStyles.Float, culture, out g) &&
                    float.TryParse(textBoxB.Text, System.Globalization.NumberStyles.Float, culture, out b))
                {
                    accesor.SetValor(new Color4(r, g, b, 1.0f));
                }
            }
        }
    }
}
