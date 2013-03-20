using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Fang3D.Editor.Controles.EditoresValores
{
    public partial class EditorType : EditorBase
    {
        private class NullItem
        {
            public override string ToString()
            {
                return "<null>";
            }
        }

        private bool actualizando;
        private NullItem nullItem = new NullItem();

        public EditorType(AccesorValor accesor)
            : base(accesor)
        {
            InitializeComponent();

            actualizando = true;
            try
            {
                comboBox1.Enabled = (accesor.ReadOnly == false);

                object val = accesor.GetValor();

                if (val != null)
                {
                    comboBox1.Items.Add(val);
                    comboBox1.SelectedItem = val;
                }
                else
                {
                    comboBox1.Items.Add(nullItem);
                    comboBox1.SelectedItem = nullItem;
                }
            }
            finally
            {
                actualizando = false;
            }
        }

        private void LoadCombo()
        {
            actualizando = true;

            try
            {
                comboBox1.Items.Clear();

                if (typeof(Resource).IsAssignableFrom(accesor.ValueType))
                {
                    foreach (Resource item in Resource.AllResourcesOfType(accesor.ValueType))
                        if (item.Name != "")
                            comboBox1.Items.Add(item);
                }
                else if (typeof(Component).IsAssignableFrom(accesor.ValueType))
                {
                    foreach (Component item in Component.AllComponentsOfType(accesor.ValueType))
                        comboBox1.Items.Add(item);
                }

                comboBox1.Items.Add(nullItem);

                object o = accesor.GetValor();

                if (o == null)
                    comboBox1.SelectedItem = nullItem;
                else
                    comboBox1.SelectedItem = o;
            }
            finally
            {
                actualizando = false;
            }
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

                if (o == null)
                    comboBox1.SelectedItem = nullItem;
                else
                    comboBox1.SelectedItem = o;
            }
            finally
            {
                actualizando = false;
            }
        }

        private void SetearValor()
        {
            if (actualizando == false && accesor.ReadOnly == false)
            {
                if (comboBox1.SelectedItem == nullItem)
                    accesor.SetValor(null);
                else
                    accesor.SetValor(comboBox1.SelectedItem);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetearValor();
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            LoadCombo();
        }
    }
}
