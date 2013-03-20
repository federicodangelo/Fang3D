using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Fang3D.Editor.Controles
{
    public partial class ListaRecursos : UserControl
    {
        public VistaEscena vistaEscena;

        public class ListViewItemResource : ListViewItem
        {
            public Resource resource;

            public ListViewItemResource(Resource res)
            {
                this.resource = res;

                Text = res.Name;
                SubItems.Add(res.GetType().Name);
                SubItems.Add(res.FileName);

                if (res.Missing)
                {
                    BackColor = Color.Red;
                    ForeColor = Color.Black;
                }
            }
        }

        public ListaRecursos()
        {
            InitializeComponent();

            Resource.ResourceCreatedEvent += new Resource.ResourceCreatedDelegate(Resource_ResourceCreatedEvent);
            Resource.ResourceDeletedEvent += new Resource.ResourceDeletedDelegate(Resource_ResourceDeletedEvent);
            Resource.ResourceUpdatedEvent += new Resource.ResourceUpdatedDelegate(Resource_ResourceUpdatedEvent);

            ActualizarListadoRecursos();
        }

        protected Resource SelectedResource
        {
            get
            {
                if (listView1.SelectedIndices.Count > 0)
                {
                    ListViewItemResource lvi = (ListViewItemResource)listView1.SelectedItems[0];

                    return lvi.resource;
                }

                return null;
            }
        }

        void Resource_ResourceCreatedEvent(Resource res)
        {
            ActualizarListadoRecursos();
        }

        void Resource_ResourceUpdatedEvent(Resource res)
        {
            ActualizarListadoRecursos();
        }

        void Resource_ResourceDeletedEvent(Resource res)
        {
            ActualizarListadoRecursos();
        }

        private void ActualizarListadoRecursos()
        {
            listView1.BeginUpdate();

            Resource oldSelectedResource = SelectedResource;

            listView1.Items.Clear();

            foreach (Resource res in Resource.AllResources)
            {
                if (res.Name != null && res.Name != "")
                {
                    ListViewItemResource lvi = new ListViewItemResource(res);
                    listView1.Items.Add(lvi);

                    if (res == oldSelectedResource)
                        lvi.Selected = true;
                }
            }

            listView1.EndUpdate();
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Copy);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItemResource lvi = (ListViewItemResource)listView1.GetItemAt(e.X, e.Y);

            if (lvi != null && lvi.resource != null)
                OpenEditor(lvi.resource);
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            Resource res = SelectedResource;

            if (res != null)
            {
                if (vistaEscena != null)
                    vistaEscena.editorMultiplesReflectables.Element = res;

                if (e.Button == MouseButtons.Right)
                    contextMenuStrip1.Show(listView1, e.X, e.Y);
            }
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedResource != null)
                OpenEditor(SelectedResource);
        }

        private void OpenEditor(Resource res)
        {
            if (res.FileName != null && File.Exists(res.FileName))
            {
                try
                {
                    ProcessStartInfo si = new ProcessStartInfo(res.FileName);

                    Process pr = Process.Start(si);
                    
                    if (pr != null)
                        pr.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteError(ex.ToString());
                }
            }
        }
    }
}
