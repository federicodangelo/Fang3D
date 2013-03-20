using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Fang3D.Editor.Controles.EditoresValores
{
    public partial class EditorEnum : EditorBase
    {
        bool actualizando;

        public EditorEnum(AccesorValor accesor, EnumConverter enumConverter) : base(accesor)
        {
            InitializeComponent();

            foreach (Object item in enumConverter.GetStandardValues())
                comboBox1.Items.Add(item);

            ActualizarValor();
        }

        public override void ActualizarValor()
        {
            if (comboBox1.Focused)
                return;

            actualizando = true;

            try
            {
                comboBox1.Enabled = (accesor.ReadOnly == false);

                object o = accesor.GetValor();

                comboBox1.SelectedItem = o;
            }
            finally
            {
                actualizando = false;
            }
        }

        private void SetearValor()
        {
            if (actualizando == false)
            {
                accesor.SetValor(comboBox1.SelectedItem);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetearValor();
        }
    }
}
