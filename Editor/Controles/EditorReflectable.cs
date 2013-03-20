using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using OpenTK.Math;
using Fang3D.Editor.Controles.EditoresValores;

namespace Fang3D.Editor.Controles
{
    public partial class EditorReflectable : UserControl
    {
        private IReflectable reflectable;
        private Type loadedType;
        private Dictionary<String, EditorBase> controles = new Dictionary<string, EditorBase>();

        public IReflectable Reflectable
        {
            get
            {
                return reflectable;
            }
            set
            {
                if (reflectable != value)
                {
                    reflectable = value;
                    CargarAtributosComponente();

                    if (reflectable is Component)
                        contextMenuStrip1.Enabled = true;
                    else
                        contextMenuStrip1.Enabled = false;
                }
            }
        }

        private void CargarAtributosComponente()
        {
            if (loadedType != reflectable.GetType())
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowCount = 0;
                controles.Clear();

                labelNombreComponente.Text = Reflectable.GetType().Name;

                AccesorValor[] accesoresValores = Reflectable.GetAccesoresValores();

                foreach (AccesorValor accesor in accesoresValores)
                    AgregarEditor(accesor);

                tableLayoutPanel1.RowCount++;

                Controls.Remove(buttonActualizar);

                loadedType = Reflectable.GetType();
            }
            else
            {
                AccesorValor[] accesoresValores = Reflectable.GetAccesoresValores();

                foreach (AccesorValor accesor in accesoresValores)
                    AgregarEditor(accesor);
            }
        }

        private void AgregarEditor(AccesorValor accesor)
        {
            if (accesor.ShowInEditor)
            {
                EditorBase controlEditor = null;

                if (controles.TryGetValue(accesor.ValueName, out controlEditor) == false)
                {
                    Type tipo = accesor.ValueType;

                    if (tipo == typeof(Vector3))
                        controlEditor = new EditoresValores.EditorVector3(accesor);
                    if (tipo == typeof(OpenTK.Graphics.Color4))
                        controlEditor = new EditoresValores.EditorColor4(accesor);
                    else if (tipo == typeof(Quaternion))
                        controlEditor = new EditoresValores.EditorQuaternion(accesor);
                    else if (tipo == typeof(Boolean))
                        controlEditor = new EditoresValores.EditorBoolean(accesor);
                    else if (tipo == typeof(String))
                        controlEditor = new EditoresValores.EditorString(accesor);
                    else if (tipo == typeof(int) || tipo == typeof(float))
                        controlEditor = new EditoresValores.EditorNumber(accesor);
                    else if (tipo.IsEnum)
                        controlEditor = new EditoresValores.EditorEnum(accesor, new EnumConverter(tipo));
                    else if (tipo == typeof(Bitmap))
                        controlEditor = new EditoresValores.EditorBimap(accesor);
                    else if (typeof(Component).IsAssignableFrom(tipo) ||
                             typeof(Resource).IsAssignableFrom(tipo))
                    {
                        controlEditor = new EditoresValores.EditorType(accesor);
                    }

                    if (controlEditor != null)
                    {
                        controles.Add(accesor.ValueName, controlEditor);

                        Label labelNombre = new Label();
                        labelNombre.AutoSize = true;
                        labelNombre.Text = ProcesarNombreCampo(accesor.ValueName);

                        labelNombre.Anchor = AnchorStyles.Left;

                        controlEditor.Anchor = AnchorStyles.Left;

                        tableLayoutPanel1.RowCount++;
                        tableLayoutPanel1.Controls.Add(labelNombre, 0, tableLayoutPanel1.RowCount - 1);
                        tableLayoutPanel1.Controls.Add(controlEditor, 1, tableLayoutPanel1.RowCount - 1);
                    }
                }
                else
                {
                    controlEditor.accesor = accesor;

                    controlEditor.Update();
                }
            }
        }

        private String ProcesarNombreCampo(String nombre)
        {
            List<String> palabras = new List<string>();
            String palabraActual = "";

            foreach (char c in nombre)
            {
                if (Char.ToLower(c) != c)
                {
                    if (palabraActual != "")
                    {
                        palabras.Add(palabraActual);
                        palabraActual = "" + c;
                    }
                    else
                    {
                        palabraActual += c;
                    }
                }
                else
                {
                    palabraActual += c;
                }
            }

            if (palabraActual != "")
                palabras.Add(palabraActual);

            nombre = "";

            foreach (String palabra in palabras)
                nombre += Char.ToUpper(palabra[0]) + palabra.ToLower().Substring(1) + " ";

            nombre = nombre.Trim();

            return nombre;
        }

        public EditorReflectable()
        {
            InitializeComponent();
        }

        public void ActualizarValores()
        {
            foreach (EditorBase editor in controles.Values)
                editor.ActualizarValor();
        }

        private void buttonActualizar_Click(object sender, EventArgs e)
        {
            ActualizarValores();
        }

        private void eliminarComponenteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (reflectable is Component)
                ((Component) reflectable).Destroy();
        }
    }
}
