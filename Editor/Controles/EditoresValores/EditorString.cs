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
    public partial class EditorString : EditorBase
    {
        private bool actualizando;

        public EditorString(AccesorValor accesor)
            : base(accesor)
        {
            InitializeComponent();

            ActualizarValor();
        }

        public override void ActualizarValor()
        {
            if (textBox.Focused)
                return;

            actualizando = true;

            try
            {
                textBox.ReadOnly = accesor.ReadOnly;

                String val = (String)accesor.GetValor();

                textBox.Text = val;
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
                accesor.SetValor(textBox.Text);
            }
        }
    }
}
