using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Fang3D.Editor.Controles.EditoresValores
{
    public partial class EditorBoolean : EditorBase
    {
        bool actualizando;

        public EditorBoolean(AccesorValor accesor)
            : base(accesor)
        {
            InitializeComponent();

            ActualizarValor();
        }

        public override void ActualizarValor()
        {
            actualizando = true;

            try
            {
                checkBox.Enabled = (accesor.ReadOnly == false);

                bool b = (bool) accesor.GetValor();

                checkBox.Checked = b;
            }
            finally
            {
                actualizando = false;
            }
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            SetearValor();
        }

        private void SetearValor()
        {
            if (actualizando == false && accesor.ReadOnly == false)
            {
                accesor.SetValor(checkBox.Checked);
            }
        }
    }
}
