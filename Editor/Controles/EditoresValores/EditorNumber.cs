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
    public partial class EditorNumber : EditorBase
    {
        private bool actualizando;

        private bool autoIncrementar = false;
        private Point posAutoIncrementar;
        private decimal valorInicial;

        private enum InternalType
        {
            FLOAT,
            INT
        }

        private InternalType internalType;


        public EditorNumber(AccesorValor accesor)
            : base(accesor)
        {
            InitializeComponent();

            if (accesor.ValueType == typeof(float))
                internalType = InternalType.FLOAT;
            else if (accesor.ValueType == typeof(int))
                internalType = InternalType.INT;
            else
                throw new NotSupportedException("Tipo de dato no soportado por el editor numerico: " + accesor.ValueType.Name);

            ActualizarValor();

            labelText.MouseDown += new MouseEventHandler(label_MouseDown);
            labelText.MouseUp += new MouseEventHandler(label_MouseUp);
            labelText.MouseMove += new MouseEventHandler(label_MouseMove);
        }

        void label_MouseMove(object sender, MouseEventArgs e)
        {
            if (autoIncrementar)
            {
                int dy = -(e.Y - posAutoIncrementar.Y);

                decimal cambio = System.Math.Abs(valorInicial * new Decimal(0.05));
                if (System.Math.Round(cambio, 3) == 0)
                    cambio = new Decimal(0.05);

                decimal val = valorInicial + cambio * dy;

                switch (internalType)
                {
                    case InternalType.FLOAT:
                        textBox.Text = Math.Round((float)val, 3).ToString(culture);
                        break;

                    case InternalType.INT:
                        textBox.Text = ((int) val).ToString(culture);
                        break;
                }
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

                ok = Decimal.TryParse(textBox.Text, System.Globalization.NumberStyles.Float, culture, out valorInicial);

                if (ok)
                {
                    autoIncrementar = true;
                    posAutoIncrementar = e.Location;
                }
            }
        }

        public override void ActualizarValor()
        {
            if (textBox.Focused)
                return;

            actualizando = true;

            try
            {
                textBox.ReadOnly = accesor.ReadOnly;

                object val = accesor.GetValor();

                switch (internalType)
                {
                    case InternalType.FLOAT:
                        textBox.Text = Math.Round((float)val, 3).ToString(culture);
                        break;

                    case InternalType.INT:
                        textBox.Text = ((int)val).ToString(culture);
                        break;
                }
            }
            finally
            {
                actualizando = false;
            }
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            SetearValor();
        }

        private void SetearValor()
        {
            if (actualizando == false && accesor.ReadOnly == false)
            {
                float f;
                int i;

                switch (internalType)
                {
                    case InternalType.FLOAT:
                        if (float.TryParse(textBox.Text, System.Globalization.NumberStyles.Float, culture, out f))
                            accesor.SetValor(f);
                        break;

                    case InternalType.INT:
                        if (int.TryParse(textBox.Text, System.Globalization.NumberStyles.Integer, culture, out i))
                            accesor.SetValor(i);
                        break;
                }
            }
        }
    }
}
