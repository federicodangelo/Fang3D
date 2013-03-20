using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Fang3D.Editor.Controles
{
    public partial class VistaEscenaConArbol : UserControl
    {
        public Scene scene;
        private bool arbolVisible = true;
        private bool editorEntidadVisible = true;

        public VistaEscenaConArbol()
        {
            InitializeComponent();
        }

        public bool ArbolVisible
        {
            get { return arbolVisible; }
            set
            {
                arbolEntidades.Visible = value;
                arbolVisible = value;
                ActualizarPosiciones();
            }
        }

        public bool PropiedadesVisible
        {
            get { return editorEntidadVisible; }
            set
            {
                editorEntidad.Visible = value;
                editorEntidadVisible = value;
                ActualizarPosiciones();
            }
        }

        public void Init()
        {
            vistaEscena.scene = scene;
            vistaEscena.editorMultiplesReflectables = editorEntidad;
            vistaEscena.arbolEntidades = arbolEntidades;

            arbolEntidades.vistaEscena = vistaEscena;

            vistaEscena.Init();
            arbolEntidades.Init();

            vistaEscena.InicializarGL();
        }

        private void ActualizarPosiciones()
        {
            if (!editorEntidadVisible && !arbolVisible)
            {
                panelDerecho.Visible = false;
            }
            else
            {
                if (!editorEntidadVisible)
                    arbolEntidades.Dock = DockStyle.Fill;
                else
                    arbolEntidades.Dock = DockStyle.Top;

                panelDerecho.Visible = true;
            }
        }
    }
}
