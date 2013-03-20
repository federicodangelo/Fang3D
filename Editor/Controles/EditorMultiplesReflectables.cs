using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Fang3D.Attributes;

namespace Fang3D.Editor.Controles
{
    public partial class EditorMultiplesReflectables : UserControl
    {
        private Object element;
        private Timer timer;
        private List<EditorReflectable> controlesEditorReflectables = new List<EditorReflectable>();

        public Object Element
        {
            get
            {
                return element;
            }
            set
            {
                if (element != value)
                {
                    element = value;

                    CargarDatosElemento();

                    if (element != null)
                        timer.Enabled = true;
                    else
                        timer.Enabled = false;
                }
            }

        }

        public EditorMultiplesReflectables()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 500;

            CargarDatosElemento();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            ActualizarValores();
        }

        public void ActualizarValores()
        {
            foreach (EditorReflectable atr in controlesEditorReflectables)
                atr.ActualizarValores();
        }

        private void CargarDatosElemento()
        {
            this.SuspendLayout();

            if (element != null)
            {
                if (element is Entity)
                {
                    Entity entity = (Entity)element;

                    labelNombreEntidad.Text = String.Format("Entity {0} : {1}", entity.Id, entity.Name);
                    textBoxNombreEntidad.Text = entity.Name;

                    textBoxNombreEntidad.Visible = true;
                    labelNombreEntidad.Visible = true;

                    int i = 0;

                    foreach (Component comp in entity.ChildComponents)
                    {
                        if (comp.GetType().GetCustomAttributes(typeof(DontShowInEditor), false).Length > 0)
                            continue;

                        EditorReflectable atr;

                        if (i < controlesEditorReflectables.Count && controlesEditorReflectables[i].Reflectable.GetType() == comp.GetType())
                        {
                            controlesEditorReflectables[i].Reflectable = comp;
                            controlesEditorReflectables[i].ActualizarValores();
                        }
                        else
                        {
                            if (i < controlesEditorReflectables.Count)
                            {
                                controlesEditorReflectables[i].Parent = null;

                                controlesEditorReflectables[i] = null;
                            }
                            else
                            {
                                controlesEditorReflectables.Add(null);
                            }

                            atr = new EditorReflectable();

                            atr.Reflectable = comp;
                            atr.Dock = DockStyle.Fill;

                            tableLayoutPanel1.RowCount++;

                            tableLayoutPanel1.Controls.Add(atr, 0, tableLayoutPanel1.RowCount - 1);

                            controlesEditorReflectables[i] = atr;
                        }

                        i++;
                    }

                    int newSize = i;

                    while (i < controlesEditorReflectables.Count)
                    {
                        controlesEditorReflectables[i].Parent = null;
                        i++;
                    }

                    controlesEditorReflectables.RemoveRange(newSize, controlesEditorReflectables.Count - newSize);
                }
                else if (element is IReflectable)
                {
                    IReflectable reflectable = (IReflectable)element;

                    textBoxNombreEntidad.Visible = false;
                    labelNombreEntidad.Visible = false;

                    if (controlesEditorReflectables.Count == 1 && controlesEditorReflectables[0].Reflectable.GetType() == element.GetType())
                    {
                        controlesEditorReflectables[0].Reflectable = reflectable;
                        controlesEditorReflectables[0].ActualizarValores();
                    }
                    else
                    {
                        for (int i = 0; i < controlesEditorReflectables.Count; i++)
                            controlesEditorReflectables[i].Parent = null;
                        controlesEditorReflectables.Clear();

                        EditorReflectable atr = new EditorReflectable();

                        atr.Reflectable = reflectable;
                        atr.Dock = DockStyle.Fill;

                        tableLayoutPanel1.RowCount++;

                        tableLayoutPanel1.Controls.Add(atr, 0, tableLayoutPanel1.RowCount - 1);

                        controlesEditorReflectables.Add(atr);
                    }
                }

                tableLayoutPanel1.RowCount++;
            }
            else
            {
                labelNombreEntidad.Text = "";
                textBoxNombreEntidad.Text = "";

                textBoxNombreEntidad.Visible = false;

                tableLayoutPanel1.Controls.Clear();

                tableLayoutPanel1.RowCount = 0;
                controlesEditorReflectables.Clear();
            }

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void textBoxNombreEntidad_TextChanged(object sender, EventArgs e)
        {
            if (element is Entity)
            {
                Entity entity = (Entity) element;

                entity.Name = textBoxNombreEntidad.Text;
                labelNombreEntidad.Text = String.Format("Entity {0} : {1}", entity.Id, entity.Name);
            }
        }
    }
}
