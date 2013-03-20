using System.Windows.Forms;
using Fang3D.Editor.Controles;
using OpenTK.Math;
using System.Collections.Generic;
using Fang3D.Scenes;
using Fang3D.Attributes;
using System.IO;
using System.Drawing;

namespace Fang3D.Editor
{
    public partial class FormPrincipal : Form
    {
        public Scene sceneEdit;
        public VistaEscenaConArbol vistaConArbolEdit;

        public Scene scenePlay;
        public VistaEscenaConArbol vistaConArbolPlay;

        public VistaEscenaConArbol vistaConArbolActiva;
        public ListaRecursos listaRecursos;

        private FormConsole formConsole;
        public bool menuComponentesValido = false;

        static private FormPrincipal instance;

        static public FormPrincipal Instance
        {
            get { return instance; }
        }

        public FormPrincipal()
        {
            instance = this;

            InitializeComponent();

            formConsole = new FormConsole();

            toolStripStatusLabel1.Text = "";
            Console.ConsoleLineEvent += new Console.ConsoleLineDelegate(Console_ConsoleLineEvent);

            StaticResources.Init();

            MonitorRecursos.StartMonitorearRecursos();

            ActualizarMenuComponentesYEntidades();

            sceneEdit = new Scene(true);
            sceneEdit.MakeCurrent();

            vistaConArbolEdit = new VistaEscenaConArbol();
            vistaConArbolEdit.scene = sceneEdit;
            vistaConArbolEdit.vistaEscena.editMode = true;
            vistaConArbolEdit.Init();

            vistaConArbolEdit.Dock = DockStyle.Fill;
            tabPageEdit.Controls.Add(vistaConArbolEdit);


            listaRecursos = new ListaRecursos();
            listaRecursos.vistaEscena = vistaConArbolEdit.vistaEscena;
            listaRecursos.Dock = DockStyle.Left;
            tabPageEdit.Controls.Add(listaRecursos);

            scenePlay = new Scene(false);
            scenePlay.MakeCurrent();

            vistaConArbolPlay = new VistaEscenaConArbol();
            vistaConArbolPlay.scene = scenePlay;
            vistaConArbolPlay.Init();

            vistaConArbolPlay.Dock = DockStyle.Fill;
            tabPagePlay.Controls.Add(vistaConArbolPlay);

            tabControl.SelectTab(tabPageEdit);

            tabControl_Selected(null, null);

            /*Transformation trans = new Transformation();

            trans.LocalRotation = Quaternion.FromAxisAngle(Vector3.UnitX, 1);
            trans.LocalMatrix = trans.LocalMatrix;
            trans.LocalRotationEuler = trans.LocalRotationEuler;

            trans.LocalRotation = Quaternion.FromAxisAngle(Vector3.UnitY, 1);
            trans.LocalMatrix = trans.LocalMatrix;
            trans.LocalRotationEuler = trans.LocalRotationEuler;

            trans.LocalRotation = Quaternion.FromAxisAngle(Vector3.UnitZ, 1);
            trans.LocalMatrix = trans.LocalMatrix;
            trans.LocalRotationEuler = trans.LocalRotationEuler;*/
        }


        private void acercaDeFang3DToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            new About().ShowDialog();
        }

        private void salvarToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Scenes.Serializer.SceneSerializer.Save(sceneEdit, saveFileDialog1.FileName);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Saving Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cargarToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    vistaConArbolEdit.arbolEntidades.UpdateEnabled = false;
                    sceneEdit.DispatchEnabled = false;

                    Scenes.Serializer.SceneSerializer.Load(sceneEdit, openFileDialog1.FileName);

                    sceneEdit.DispatchEnabled = true;
                    vistaConArbolEdit.arbolEntidades.UpdateEnabled = true;

                    saveFileDialog1.FileName = openFileDialog1.FileName;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    sceneEdit.DestroyAllComponents();
                }
            }
        }

        private void nuevaToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show("Crear Nueva Escena? Se perderá los cambios no salvados de la escena actual", "Nueva Escena", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                toolStripButtonStop_Click(null, null);
                sceneEdit.DestroyAllComponents();
            }
        }

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl.SelectedTab == tabPageEdit)
                vistaConArbolActiva = vistaConArbolEdit;
            else
                vistaConArbolActiva = vistaConArbolPlay;

            ActualizarMenuVista();
        }

        private void ActualizarMenuVista()
        {
            dibujarGrillaToolStripMenuItem.Checked = vistaConArbolActiva.vistaEscena.drawGrid;
            arbolEntidadesToolStripMenuItem.Checked = vistaConArbolActiva.ArbolVisible;
            propiedadesToolStripMenuItem.Checked = vistaConArbolActiva.PropiedadesVisible;
        }

        void toolStripButtonStop_Click(object sender, System.EventArgs e)
        {
            if (toolStripButtonPlay.Checked == true)
            {
                toolStripButtonPlay.Checked = false;
                toolStripButtonPause.Checked = false;

                vistaConArbolPlay.arbolEntidades.UpdateEnabled = false;

                scenePlay.DestroyAllComponents();

                vistaConArbolPlay.arbolEntidades.UpdateEnabled = true;

                tabControl.SelectTab(tabPageEdit);

                Console.WriteLog("------------------------ STOP ------------------------");
            }
        }

        void toolStripButtonPause_Click(object sender, System.EventArgs e)
        {
            if (toolStripButtonPlay.Checked == true)
            {
                vistaConArbolPlay.vistaEscena.Pause = toolStripButtonPause.Checked;
            }
            else
            {
                toolStripButtonPause.Checked = false;
            }
        }

        void toolStripButtonPlay_Click(object sender, System.EventArgs e)
        {
            if (toolStripButtonPlay.Checked == false)
            {
                toolStripButtonPlay.Checked = true;

                vistaConArbolPlay.vistaEscena.Pause = true;

                vistaConArbolPlay.arbolEntidades.UpdateEnabled = false;

                sceneEdit.CopyTo(scenePlay);

                tabControl.SelectTab(tabPagePlay);

                vistaConArbolPlay.arbolEntidades.UpdateEnabled = true;

                vistaConArbolPlay.vistaEscena.Pause = false;

                //vistaConArbolPlay.arbolEntidades.UpdateEnabled = false;

                Console.WriteLog("------------------------ PLAY ------------------------");
            }
        }

        private void PosicionarEntityEnVista(Entity ent, VistaEscena vista )
        {
            ent.Transformation.LocalTranslation = vista.UnprojectPoint(vista.Width / 2, vista.Height / 2);
        }

        private void ActualizarMenuComponentesYEntidades()
        {
            if (menuComponentesValido == false)
            {
                System.Type[] types = System.Reflection.Assembly.GetAssembly(typeof(Component)).GetExportedTypes();

                entitysToolStripMenuItem.DropDownItems.Clear();
                componentsToolStripMenuItem.DropDownItems.Clear();
                scriptsToolStripMenuItem.DropDownItems.Clear();

                foreach (System.Type type in types)
                {
                    if (type.GetCustomAttributes(typeof(DontShowInCreateMenu), false).Length > 0)
                        continue;

                    if (typeof(Entity).IsAssignableFrom(type))
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem();

                        item.Text = type.Name;
                        item.Tag = type;
                        item.Click += new System.EventHandler(menuCrearEntity_Click);

                        entitysToolStripMenuItem.DropDownItems.Add(item);
                    }
                    else if (typeof(Component).IsAssignableFrom(type))
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem();

                        item.Text = type.Name;
                        item.Tag = type;
                        item.Click += new System.EventHandler(menuCrearComponent_Click);

                        componentsToolStripMenuItem.DropDownItems.Add(item);
                    }
                }

                foreach (ScriptResource script in Resource.AllResourcesOfType(typeof(ScriptResource)))
                {
                    ToolStripMenuItem item = new ToolStripMenuItem();

                    item.Text = script.Name;
                    item.Tag = script;
                    item.Click += new System.EventHandler(menuCrearScript_Click);

                    scriptsToolStripMenuItem.DropDownItems.Add(item);
                }

                menuComponentesValido = true;
            }
        }

        void menuCrearEntity_Click(object sender, System.EventArgs e)
        {
            if (vistaConArbolActiva != null)
            {
                ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;

                System.Type entityType = (System.Type) menuItem.Tag;

                try
                {
                    Entity ent = (Entity) entityType.GetConstructor(System.Type.EmptyTypes).Invoke(null);

                    ent.Name = menuItem.Text;

                    PosicionarEntityEnVista(ent, vistaConArbolActiva.vistaEscena);

                    vistaConArbolActiva.vistaEscena.SelectedEntity = ent;
                }
                catch (System.Exception)
                {
                }
            }
        }

        void menuCrearComponent_Click(object sender, System.EventArgs e)
        {
            if (vistaConArbolActiva != null && vistaConArbolActiva.vistaEscena.SelectedEntity != null)
            {
                ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;

                System.Type componentType = (System.Type)menuItem.Tag;

                try
                {
                    Component comp = (Component)componentType.GetConstructor(System.Type.EmptyTypes).Invoke(null);

                    comp.Entity = vistaConArbolActiva.vistaEscena.SelectedEntity;
                }
                catch (System.Exception)
                {
                }
            }
        }

        void menuCrearScript_Click(object sender, System.EventArgs e)
        {
            if (vistaConArbolActiva != null && vistaConArbolActiva.vistaEscena.SelectedEntity != null)
            {
                ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;

                ScriptResource script = (ScriptResource) menuItem.Tag;

                script.AssignTo(vistaConArbolActiva.vistaEscena.SelectedEntity);
            }
        }

        private void cuboToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (vistaConArbolActiva != null)
            {
                Entity cube = ComponentFactory.NewCube();

                PosicionarEntityEnVista(cube, vistaConArbolActiva.vistaEscena);

                vistaConArbolActiva.vistaEscena.SelectedEntity = cube;
            }
        }


        private void luzToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (vistaConArbolActiva != null)
            {
                Entity light = ComponentFactory.NewLight();

                PosicionarEntityEnVista(light, vistaConArbolActiva.vistaEscena);

                vistaConArbolActiva.vistaEscena.SelectedEntity = light;
            }
        }

        private void fuenteParticulasToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (vistaConArbolActiva != null)
            {
                Entity ps = ComponentFactory.NewParticleSystem();

                PosicionarEntityEnVista(ps, vistaConArbolActiva.vistaEscena);

                vistaConArbolActiva.vistaEscena.SelectedEntity = ps;
            }

        }
        
        private void Console_ConsoleLineEvent(string text, Console.TextType textType)
        {
            if (InvokeRequired)
            {
                Invoke(new Console.ConsoleLineDelegate(Console_ConsoleLineEvent), text, textType);
                return;
            }

            switch (textType)
            {
                case Console.TextType.LOG:
                    toolStripStatusLabel1.Text = "LOG: " + text;
                    toolStripStatusLabel1.ForeColor = System.Drawing.Color.Black;
                    break;
                
                case Console.TextType.WARNING:
                    toolStripStatusLabel1.Text = "WARNING: " + text;
                    toolStripStatusLabel1.ForeColor = System.Drawing.Color.Yellow;
                    break;

                case Console.TextType.ERROR:
                    toolStripStatusLabel1.Text = "ERROR: " + text;
                    toolStripStatusLabel1.ForeColor = System.Drawing.Color.Red;
                    break;
            }
        }

        private void toolStripStatusLabel1_DoubleClick(object sender, System.EventArgs e)
        {
            formConsole.Show();
            formConsole.BringToFront();
            toolStripStatusLabel1.Text = "";
        }

        private void FormPrincipal_Load(object sender, System.EventArgs e)
        {
            toolStrip1.Renderer = new ToolStripProfessionalRendererSinBordes();
            menuStrip1.Renderer = new ToolStripProfessionalRendererSinIconos();
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            MonitorRecursos.MonitorearRecursos();
        }

        public class ToolStripProfessionalRendererSinIconos : ToolStripProfessionalRenderer
        {
            protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
            {
                //base.OnRenderImageMargin(e);
            }
        }

        public class ToolStripProfessionalRendererSinBordes : ToolStripProfessionalRenderer
        {
            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                //base.OnRenderToolStripBorder(e);
            }
        }

        private void menuStrip1_MenuActivate(object sender, System.EventArgs e)
        {
            ActualizarMenuComponentesYEntidades();

            if (vistaConArbolActiva != null && vistaConArbolActiva.vistaEscena.SelectedEntity != null)
            {
                componentsToolStripMenuItem.Enabled = true;
                foreach (ToolStripItem item in componentsToolStripMenuItem.DropDownItems)
                    item.Enabled = true;

                scriptsToolStripMenuItem.Enabled = true;
                foreach (ToolStripItem item in scriptsToolStripMenuItem.DropDownItems)
                    item.Enabled = true;
            }
            else
            {
                componentsToolStripMenuItem.Enabled = false;
                foreach (ToolStripItem item in componentsToolStripMenuItem.DropDownItems)
                    item.Enabled = false;

                scriptsToolStripMenuItem.Enabled = false;
                foreach (ToolStripItem item in scriptsToolStripMenuItem.DropDownItems)
                    item.Enabled = false;
            }
        }

        private void dibujarGrillaToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (vistaConArbolActiva != null)
            {
                dibujarGrillaToolStripMenuItem.Checked = !dibujarGrillaToolStripMenuItem.Checked;

                vistaConArbolActiva.vistaEscena.drawGrid = dibujarGrillaToolStripMenuItem.Checked;
            }
        }

        private void arbolEntidadesToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (vistaConArbolActiva != null)
            {
                arbolEntidadesToolStripMenuItem.Checked = !arbolEntidadesToolStripMenuItem.Checked;

                vistaConArbolActiva.ArbolVisible = arbolEntidadesToolStripMenuItem.Checked;
            }

        }

        private void propiedadesToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (vistaConArbolActiva != null)
            {
                propiedadesToolStripMenuItem.Checked = !propiedadesToolStripMenuItem.Checked;

                vistaConArbolActiva.PropiedadesVisible = propiedadesToolStripMenuItem.Checked;
            }
        }
    }
}
