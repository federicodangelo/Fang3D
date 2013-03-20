using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenTK.Math;

namespace Fang3D.Editor.Controles
{
    public partial class ArbolEntidades : UserControl
    {
        public VistaEscena vistaEscena;

        private Entity selectedEntity;
        private bool updateEnabled = true;

        public bool UpdateEnabled
        {
            get { return updateEnabled; }
            set
            {
                if (value != updateEnabled)
                {
                    updateEnabled = value;

                    if (updateEnabled)
                    {
                        Scene.PushCurrentScene();

                        try
                        {
                            vistaEscena.scene.MakeCurrent();
                            ActualizarNodos();
                        }
                        finally
                        {
                            Scene.PopCurrentScene();
                        }
                    }
                }
            }
        }

        public class EntidadNode : TreeNode
        {
            public EntidadNode(Entity entity) : base(entity.Name)
            {
                Tag = entity;
                Checked = entity.Enabled;
            }

            public Entity Entity
            {
                get
                {
                    return (Entity)Tag;
                }
            }
        }

        public Entity SelectedEntity
        {
            set
            {
                if (selectedEntity != value)
                {
                    selectedEntity = value;

                    EntidadNode node = FindEntityNode(value);

                    if (node != null)
                        node.EnsureVisible();

                    treeView1.SelectedNode = node;

                    if (vistaEscena != null)
                        vistaEscena.SelectedEntity = selectedEntity;
                }
            }

            get
            {
                return selectedEntity;
            }
        }

        public ArbolEntidades()
        {
            InitializeComponent();

            treeView1.Sorted = true;
        }

        public void Init()
        {
            Scene.PushCurrentScene();

            try
            {
                vistaEscena.scene.MakeCurrent();
                ActualizarNodos();

                vistaEscena.scene.AddEntityCreatedEvent(new Entity.EntityCreatedDelegate(Entity_EntityCreatedEvent));
                vistaEscena.scene.AddEntityUpdatedEvent(new Entity.EntityUpdatedDelegate(Entity_EntityUpdatedEvent));
                vistaEscena.scene.AddEntityDeletedEvent(new Entity.EntityDeletedDelegate(Entity_EntityDeletedEvent));
            }
            finally
            {
                Scene.PopCurrentScene();
            }
        }

        void Entity_EntityDeletedEvent(Entity ent)
        {
            if (treeView1.IsDisposed)
                return;

            if (updateEnabled == false)
                return;

            EntidadNode node = FindEntityNode(ent);

            if (node != null)
                node.Remove();
        }

        void Entity_EntityUpdatedEvent(Entity ent, String field)
        {
            if (treeView1.IsDisposed)
                return;

            if (updateEnabled == false)
                return;

            EntidadNode node = FindEntityNode(ent);

            if (node != null)
            {
                treeView1.BeginUpdate();

                node.Text = ent.Name;

                EntidadNode root = null;

                if (ent.Transformation != null && ent.Transformation.Parent != null && ent.Transformation.Parent.Entity != null)
                    root = FindEntityNode(ent.Transformation.Parent.Entity);

                if (root != node.Parent)
                {
                    node.Remove();

                    if (root != null)
                    {
                        root.Nodes.Add(node);
                        root.ExpandAll();
                    }
                    else
                    {
                        treeView1.Nodes.Add(node);
                        treeView1.ExpandAll();
                    }
                }

                treeView1.EndUpdate();
            }
        }

        void Entity_EntityCreatedEvent(Entity ent)
        {
            if (treeView1.IsDisposed)
                return;

            if (updateEnabled == false)
                return;

            EntidadNode root = null;

            if (ent.Transformation.Parent != null && ent.Transformation.Parent.Entity != null)
                root = FindEntityNode(ent.Transformation.Parent.Entity);

            AgregarEntity(ent, root);
        }

        private EntidadNode FindEntityNode(Entity ent)
        {
            return FindEntityNode(ent, treeView1.Nodes);
        }

        private EntidadNode FindEntityNode(Entity ent, TreeNodeCollection col)
        {
            foreach (EntidadNode node in col)
            {
                if (node.Entity == ent)
                    return node;

                EntidadNode node2 = FindEntityNode(ent, node.Nodes);

                if (node2 != null)
                    return node2;
            }

            return null;
        }

        private void ActualizarNodos()
        {
            vistaEscena.scene.MakeCurrent();

            treeView1.BeginUpdate();

            treeView1.Nodes.Clear();

            foreach (Entity ent in Entity.Root.Childs)
                AgregarEntity(ent, null);

            treeView1.EndUpdate();
        }

        private void AgregarEntity(Entity ent, EntidadNode root)
        {
            EntidadNode nodo = new EntidadNode(ent);

            nodo.Expand();

            foreach (Entity ent2 in ent.Childs)
                AgregarEntity(ent2, nodo);

            if (root == null)
                treeView1.Nodes.Add(nodo);
            else
                root.Nodes.Add(nodo);
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(EntidadNode)))
                e.Effect = DragDropEffects.Move;
            else if (e.Data.GetDataPresent(typeof(ListaRecursos.ListViewItemResource)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(EntidadNode)))
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));

                EntidadNode destinationNode = (EntidadNode) ((TreeView)sender).GetNodeAt(pt);
                Entity destinationEntity;

                if (destinationNode != null)
                    destinationEntity = destinationNode.Entity;
                else
                    destinationEntity = Entity.Root;

                EntidadNode movingNode = (EntidadNode)e.Data.GetData(typeof(EntidadNode));
                Entity movingEntity = movingNode.Entity;

                if (movingEntity != destinationEntity)
                {
                    Entity temp = destinationEntity;
                    while (temp.Transformation.Parent != null)
                    {
                        if (temp.Transformation.Parent.Entity == movingEntity)
                            return;

                        temp = temp.Transformation.Parent.Entity;
                    }

                    movingEntity.Transformation.Parent = destinationEntity.Transformation;

                    Entity_EntityUpdatedEvent(movingEntity, "");

                    SelectedEntity = movingEntity;
                }
            }
            else if (e.Data.GetDataPresent(typeof(ListaRecursos.ListViewItemResource)))
            {
                Resource res = ((ListaRecursos.ListViewItemResource)e.Data.GetData(typeof(ListaRecursos.ListViewItemResource))).resource;

                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));

                EntidadNode destinationNode = (EntidadNode)((TreeView)sender).GetNodeAt(pt);
                Entity destinationEntity;

                if (destinationNode != null)
                {
                    destinationEntity = destinationNode.Entity;

                    if (res.CanAssignTo(destinationEntity))
                    {
                        res.AssignTo(destinationEntity);
                    }
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
                SelectedEntity = ((EntidadNode)treeView1.SelectedNode).Entity;
            else
                SelectedEntity = null;
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (SelectedEntity != null)
                {
                    SelectedEntity.Destroy();

                    SelectedEntity = null;
                }
            }
        }

        private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(e.Node.TreeView.BackColor), e.Bounds.X, e.Bounds.Y, e.Graphics.ClipBounds.Width - e.Bounds.X, e.Bounds.Height);

            // Retrieve the node font. If the node font has not been set,
            // use the TreeView font.
            Font nodeFont = e.Node.NodeFont;
            if (nodeFont == null) nodeFont = ((TreeView)sender).Font;

            String sufijo = "";

            if (e.Node.IsExpanded == false && e.Node.Nodes.Count > 0)
                sufijo = " -> ";

            // Draw the node text.
            if ((e.State & TreeNodeStates.Selected) != 0)
                e.Graphics.DrawString(e.Node.Text + sufijo, nodeFont, Brushes.Red, e.Bounds.X, e.Bounds.Y);
            else
                e.Graphics.DrawString(e.Node.Text + sufijo, nodeFont, Brushes.Black, e.Bounds.X, e.Bounds.Y);
        }
    }
}
