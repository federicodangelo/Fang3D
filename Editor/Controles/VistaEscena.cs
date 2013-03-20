using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK.Graphics;
using OpenTK.Math;

namespace Fang3D.Editor.Controles
{
    public partial class VistaEscena : OpenTK.GLControl
    {
        public Scene scene;
        public EditorMultiplesReflectables editorMultiplesReflectables;
        public ArbolEntidades arbolEntidades;
        public bool editMode;

        private Mesh meshArrow;
        private Mesh meshGrid;

        private bool pausa;
        public bool drawGrid = true;

        public bool Pause
        {
            get { return pausa; }
            set { if (pausa != value) { pausa = value; if (pausa == false) { ticksUltimoPaint = DateTime.Now.Ticks; } }  }
        }

        public IBaseRender render = new RenderOpenGL();
        private float deltaTime;

        private Vector3 posCamara;
        private Vector3 rotCamara;

        private Keys modificadoresTeclas = Keys.None;
        private MouseButtons botonesMouse = MouseButtons.None;

        private Entity selectedEntity;

        private int targetFrameRate = 45;

        private bool moviendoCamara = false;
        private System.Drawing.Point posCursorMoviendoCamara;

        private bool moviendoObjeto = false;
        private System.Drawing.Point posCursorMoviendoObjeto;
        private Ejes ejeMoviendoObjeto;

        static private Dictionary<Keys, KeyEnum> traduccionTeclas;

        private enum Ejes
        {
            EJE_X,
            EJE_Y,
            EJE_Z
        }

        private long ticksUltimoPaint;
        private long ticksLastWait;
        
        public Entity SelectedEntity
        {
            get { return selectedEntity; }
            set
            {
                if (selectedEntity != value)
                {
                    selectedEntity = value;

                    if (arbolEntidades != null)
                        arbolEntidades.SelectedEntity = selectedEntity;

                    moviendoObjeto = false;
                }

                if (editorMultiplesReflectables != null && editorMultiplesReflectables.Element != selectedEntity)
                    editorMultiplesReflectables.Element = selectedEntity;
            }
        }

        private Vector3 CameraRotation
        {
            get
            {
                return rotCamara;
            }
            set
            {
                rotCamara = value;
                ActualizarMatrizCamara();
            }
        }

        private Quaternion CameraRotationQuaternion
        {
            get
            {
                return Quaternion.FromAxisAngle(Vector3.UnitY, rotCamara.Y) *
                        Quaternion.FromAxisAngle(Vector3.UnitX, rotCamara.X) *
                        Quaternion.FromAxisAngle(Vector3.UnitZ, rotCamara.Z);
            }
        }

        private Vector3 CameraPosition
        {
            get
            {
                return posCamara;
            }
            set
            {
                posCamara = value;
                ActualizarMatrizCamara();
            }
        }

        private void ActualizarMatrizCamara()
        {
            Matrix4 matCamara = Matrix4.Rotate(CameraRotationQuaternion) * Matrix4.Translation(posCamara);

            matCamara.Invert();

            render.SetCamara(matCamara);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (DesignMode)
                base.OnPaintBackground(e);
        }

        public VistaEscena()
        {
            InitializeComponent();

            if (DesignMode)
                return;

            VSync = false;

            CreateAuxMesh();

            posCamara = new Vector3(0, 5, 10);
            rotCamara = new Vector3(0, 0, 0);
            ActualizarMatrizCamara();

            #region Inicialización de traducción de teclas

            traduccionTeclas = new Dictionary<Keys, KeyEnum>();

            traduccionTeclas.Add(Keys.Left, KeyEnum.Left);
            traduccionTeclas.Add(Keys.Right, KeyEnum.Right);
            traduccionTeclas.Add(Keys.Up, KeyEnum.Up);
            traduccionTeclas.Add(Keys.Down, KeyEnum.Down);
            traduccionTeclas.Add(Keys.Space, KeyEnum.Space);
            traduccionTeclas.Add(Keys.A, KeyEnum.A);
            traduccionTeclas.Add(Keys.B, KeyEnum.B);
            traduccionTeclas.Add(Keys.C, KeyEnum.C);
            traduccionTeclas.Add(Keys.D, KeyEnum.D);
            traduccionTeclas.Add(Keys.E, KeyEnum.E);
            traduccionTeclas.Add(Keys.F, KeyEnum.F);
            traduccionTeclas.Add(Keys.G, KeyEnum.G);
            traduccionTeclas.Add(Keys.H, KeyEnum.H);
            traduccionTeclas.Add(Keys.I, KeyEnum.I);
            traduccionTeclas.Add(Keys.J, KeyEnum.J);
            traduccionTeclas.Add(Keys.K, KeyEnum.K);
            traduccionTeclas.Add(Keys.L, KeyEnum.L);
            traduccionTeclas.Add(Keys.M, KeyEnum.M);
            traduccionTeclas.Add(Keys.N, KeyEnum.N);
            traduccionTeclas.Add(Keys.O, KeyEnum.O);
            traduccionTeclas.Add(Keys.P, KeyEnum.P);
            traduccionTeclas.Add(Keys.Q, KeyEnum.Q);
            traduccionTeclas.Add(Keys.R, KeyEnum.R);
            traduccionTeclas.Add(Keys.S, KeyEnum.S);
            traduccionTeclas.Add(Keys.T, KeyEnum.T);
            traduccionTeclas.Add(Keys.U, KeyEnum.U);
            traduccionTeclas.Add(Keys.V, KeyEnum.V);
            traduccionTeclas.Add(Keys.W, KeyEnum.W);
            traduccionTeclas.Add(Keys.X, KeyEnum.X);
            traduccionTeclas.Add(Keys.Y, KeyEnum.Y);
            traduccionTeclas.Add(Keys.Z, KeyEnum.Z);
            traduccionTeclas.Add(Keys.D0, KeyEnum.Number0);
            traduccionTeclas.Add(Keys.D1, KeyEnum.Number1);
            traduccionTeclas.Add(Keys.D2, KeyEnum.Number2);
            traduccionTeclas.Add(Keys.D3, KeyEnum.Number3);
            traduccionTeclas.Add(Keys.D4, KeyEnum.Number4);
            traduccionTeclas.Add(Keys.D5, KeyEnum.Number5);
            traduccionTeclas.Add(Keys.D6, KeyEnum.Number6);
            traduccionTeclas.Add(Keys.D7, KeyEnum.Number7);
            traduccionTeclas.Add(Keys.D8, KeyEnum.Number8);
            traduccionTeclas.Add(Keys.D9, KeyEnum.Number9);

            #endregion
        }

        private const int GRID_SIZE = 50;

        private void CreateAuxMesh()
        {
            meshArrow = new Mesh(
                new double[] {
                    0,0.5,0,-0.25,0,0.25,0.25,0,0.25,0.25,0,-0.25,-0.25,0,-0.25,
                    0.1,0,0,0.05,0,-0.05,0,0,-0.1,-0.05,0,-0.05,-0.1,0,0,-0.05,0,0.05,
                    0,0,0.1,0.05,0,0.05,0.1,-1,0,0,-1,-0.1,-0.1,-1,0,0,-1,0.1
                },

                new int[] {
                    0, 1, 2, -1, 0, 2, 3, -1, 0, 3, 4, -1, 0, 4, 1, -1, 1, 10, 11, -1, 
                    11, 12, 2, -1, 1, 11, 2, -1, 3, 6, 7, -1, 7, 8, 4, -1, 3, 7, 4, -1, 
                    4, 8, 9, -1, 9, 10, 1, -1, 4, 9, 1, -1, 5, 6, 3, -1, 5, 3, 2, -1, 
                    5, 2, 12, -1, 16, 11, 10, -1, 15, 16, 10, -1, 15, 10, 9, -1, 
                    15, 14, 13, -1, 13, 16, 15, -1, 12, 11, 16, -1, 12, 16, 13, -1, 
                    5, 12, 13, -1, 6, 5, 13, -1, 6, 13, 14, -1, 7, 6, 14, -1, 8, 7, 14, -1, 
                    8, 14, 15, -1, 9, 8, 15, -1
                }
            );
            meshArrow.Name = "";

            meshGrid = new Mesh();
            meshGrid.primitiveType = Mesh.PrimitiveType.Quad;
            meshGrid.vertexList = new Vector3[(GRID_SIZE+1) * (GRID_SIZE+1)];
            for (int x = -GRID_SIZE / 2; x < GRID_SIZE/2+1; x++)
                for (int y = -GRID_SIZE / 2; y < GRID_SIZE/2+1; y++)
                    meshGrid.vertexList[(x + GRID_SIZE / 2) * (GRID_SIZE+1) + (y + GRID_SIZE/2)] = new Vector3(x, 0, y);

            meshGrid.faceList = new uint[GRID_SIZE * GRID_SIZE * 4];
            uint offsetFaceList = 0;

            for (int x = 0; x < GRID_SIZE; x++)
            {
                for (int y = 0; y < GRID_SIZE; y++)
                {
                    meshGrid.faceList[offsetFaceList + 0] = (uint)(x * (GRID_SIZE+1) + y);
                    meshGrid.faceList[offsetFaceList + 1] = (uint)(x * (GRID_SIZE + 1) + (y + 1));
                    meshGrid.faceList[offsetFaceList + 2] = (uint)((x + 1) * (GRID_SIZE + 1) + (y + 1));
                    meshGrid.faceList[offsetFaceList + 3] = (uint)((x + 1) * (GRID_SIZE + 1) + y);

                    offsetFaceList += 4;
                }
            }

            meshGrid.Name = "";
        }

        public void Init()
        {
            Scene.PushCurrentScene();

            try
            {
                scene.MakeCurrent();

                scene.AddEntityDeletedEvent(new Entity.EntityDeletedDelegate(entityInternals_EntityDeletedEvent));
                scene.AddEntityUpdatedEvent(new Entity.EntityUpdatedDelegate(entityInternals_EntityUpdatedEvent));
            }
            finally
            {
                Scene.PopCurrentScene();
            }
        }

        void entityInternals_EntityDeletedEvent(Entity ent)
        {
            if (IsDisposed)
                return;

            if (ent == selectedEntity)
                SelectedEntity = null;
        }

        void entityInternals_EntityUpdatedEvent(Entity ent, String field)
        {
            if (IsDisposed)
                return;

            if (ent == selectedEntity && field != "Name")
            {
                SelectedEntity = null;
                SelectedEntity = ent;
            }
        }

        private void SetearPerspectiva()
        {
            MakeCurrent();

            render.SetSreenSize(Width, Height);
            render.SetProjectionMatrix(Matrix4.Perspective(45.0f, (float)Width / (float)Height, render.GetMinZ(), render.GetMaxZ()));
        }

        public void InicializarGL()
        {
            MakeCurrent();

            render.Init();

            SetearPerspectiva();
        }

        private void RenderComponentes()
        {
            scene.Render(render);

            if (selectedEntity != null)
            {
                Render compRender = (Render) selectedEntity.FindChildComponent(typeof(Render));

                if (compRender != null)
                {
                    RenderMode old = compRender.renderMode;
                    compRender.renderMode = RenderMode.Lines;
                    compRender.Draw(render);
                    compRender.renderMode = old;
                }
            }
        }

        private void EsperaFrame()
        {
            long ahora = DateTime.Now.Ticks;

            if (ticksLastWait != 0)
            {
                int deltaMS = (int)((ahora - ticksLastWait) / 10000);
                int targetMS = 1000 / targetFrameRate;

                if (deltaMS < targetMS)
                {
                    int sleepTime = targetMS - deltaMS;

                    if (sleepTime > 0)
                        System.Threading.Thread.Sleep(sleepTime);
                }

                ticksLastWait += targetMS * 10000;
                
                if ((ahora - ticksLastWait) / 10000 > targetMS)
                    ticksLastWait = ahora;
            }
            else
            {
                ticksLastWait = ahora;
            }

            if (!IsIdle)
                Application.DoEvents();

            Invalidate();
        }

        private int insidePaint;

        private void VistaEscena_Paint(object sender, PaintEventArgs e)
        {
            if (DesignMode)
                return;

            if (IsDisposed)
                return;

            insidePaint++;

            try
            {
                MakeCurrent();

                scene.MakeCurrent();

                long ahora = DateTime.Now.Ticks;

                if (selectedEntity != null && selectedEntity.Destroyed)
                    SelectedEntity = null;

                if (ticksUltimoPaint != 0)
                {
                    deltaTime = (float)((double)(ahora - ticksUltimoPaint) / (double)10000000);

                    if (!editMode)
                    {
                        foreach (Camara camara in Component.AllComponentsOfType(typeof(Camara)))
                        {
                            if (camara.Enabled)
                            {
                                render.SetCamara(Matrix4.Invert(camara.Transformation.GlobalMatrix));
                                break;
                            }
                        }
                    }

                    render.BeginDraw();

                    try
                    {
                        try
                        {
                            RenderComponentes();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteError("Render Error: " + ex.ToString());
                        }

                        RenderHelpers(false);
                    }
                    finally
                    {
                        render.EndDraw();

                        SwapBuffers();
                    }

                    if (insidePaint == 1 && editMode == false && pausa == false)
                    {
                        try
                        {
                            scene.Update(deltaTime);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteError("Update Error: " + ex.ToString());
                        }
                    }
                }

                ticksUltimoPaint = ahora;

                MoverCamara();

                MoverObjeto();

                if (insidePaint == 1)
                    EsperaFrame();
            }
            finally
            {
                insidePaint--;
            }
        }

        private void RenderHelpers(bool selection)
        {
            if (selection == false)
                render.ResetAllLights();

            //Dibujo la grilla
            if (selection == false && drawGrid)
                render.Draw(meshGrid, Matrix4.Identity, RenderMode.Lines);

            //Dibujo los objetos de edición
            if (scene.EditMode)
            {
                foreach (LightSource light in Component.AllComponentsOfType(typeof(LightSource)))
                {
                    if (selection)
                        GL.LoadName(light.Id);
                    light.DrawHelper(render);
                }
            }

            if (selectedEntity != null)
            {
                //Dibujo las flechas
                Matrix4 mat = selectedEntity.Transformation.GlobalMatrix;
                Vector3 scaleObject = selectedEntity.Transformation.GlobalScale;

                float helperSize = 1.0f;

                mat = Matrix4.Scale(helperSize / scaleObject.X, helperSize / scaleObject.Y, helperSize / scaleObject.Z) * mat;

                render.ClearZBuffer();

                mat = Matrix4.Translation(0, 1.0f, 0) * mat;

                if (selection)
                    GL.LoadName(-2); //Y
                render.Draw(meshArrow, mat, RenderMode.Normal, new Color4(0, 1, 0, 0));
                render.Draw(meshArrow, mat, RenderMode.Lines, new Color4(0, 0, 0, 0));

                mat = Matrix4.Translation(0, -1.0f, -1.0f) * mat;
                mat = Matrix4.Rotate(Quaternion.FromAxisAngle(Vector3.UnitX, Functions.DegreesToRadians(-90)))* mat;

                if (selection)
                    GL.LoadName(-3); //Z
                render.Draw(meshArrow, mat, RenderMode.Normal, new Color4(0, 0, 1, 0));
                render.Draw(meshArrow, mat, RenderMode.Lines, new Color4(0, 0, 0, 0));

                mat = Matrix4.Translation(1.0f, -1.0f, 0.0f) * mat;
                mat = Matrix4.Rotate(Quaternion.FromAxisAngle(Vector3.UnitZ, Functions.DegreesToRadians(-90))) * mat;

                if (selection)
                    GL.LoadName(-1); //X
                render.Draw(meshArrow, mat, RenderMode.Normal, new Color4(1, 0, 0, 0));
                render.Draw(meshArrow, mat, RenderMode.Lines, new Color4(0, 0, 0, 0));

                if (selection == false)
                {
                    //Dibujo los elementos colisionables que contenga el objeto seleccionado
                    foreach (Collisionable col in selectedEntity.FindChildComponents(typeof(Collisionable)))
                        col.DrawHelper(render);
                }
            }
        }

        private void VistaEscena_Resize(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            SetearPerspectiva();
        }

        private void VistaEscena_MouseDown(object sender, MouseEventArgs e)
        {
            if (DesignMode)
                return;

            botonesMouse = e.Button;

            MakeCurrent();

            if (modificadoresTeclas == Keys.None && e.Button == MouseButtons.Left)
                SelectedEntity = GetEntidadEnPosicion(e.X, e.Y);

            if (modificadoresTeclas == Keys.Control)
            {
                posCursorMoviendoCamara = MousePosition;
                moviendoCamara = true;
            }
        }

        private Entity GetEntidadEnPosicion(int x, int y)
        {
            uint[] bufferSeleccion = new uint[1024];

            render.BeginSelectionMode(x, y, bufferSeleccion);

            GL.PushName(0);

            foreach (Render comp in Component.AllComponentsOfType(typeof(Render)))
            {
                GL.LoadName(comp.Id);
                comp.Draw(render);
            }

            RenderHelpers(true);

            GL.PopName();

            int hits = render.EndSelectionMode();

            return ProcesarSeleccion(bufferSeleccion, hits);
        }

        public Vector3 UnprojectPoint(int x, int y)
        {
            Vector3 p1, p2;

            render.GetScreenPoint(x, y, out p1, out p2);

            Vector3 p = p1 + (p2 - p1) * 1.0f / (render.GetMaxZ() * render.GetMinZ());

            return p;
        }

        public Vector3 UnprojectPoint(int x, int y, Matrix4 mat)
        {
            Vector3 p1, p2;

            render.GetScreenPoint(x, y, mat, out p1, out p2);

            Vector3 p = p1 + (p2 - p1) * 1.0f / (render.GetMaxZ() * render.GetMinZ());

            return p;
        }

        private Entity ProcesarSeleccion(uint[] bufferSeleccion, int hits)
        {
            int offsetBuffer = 0;

            List<Component> componentesSeleccionados = new List<Component>();

            Component minComponent = null;
            uint minZ = uint.MaxValue;

            for (int i = 0; i < hits; i++)
            {
                uint names = bufferSeleccion[offsetBuffer++];

                uint zMin = bufferSeleccion[offsetBuffer++];
                uint zMax = bufferSeleccion[offsetBuffer++];

                for (uint n = 0; n < names; n++)
                {
                    int name = (int) bufferSeleccion[offsetBuffer++];

                    if (name == -1 || name == -2 || name == -3)
                    {
                        //Es uno de los helpers

                        switch(name)
                        {
                            case -1: //X
                                ejeMoviendoObjeto = Ejes.EJE_X;
                                break;

                            case -2: //Y
                                ejeMoviendoObjeto = Ejes.EJE_Y;
                                break;
                            
                            case -3: //Z
                                ejeMoviendoObjeto = Ejes.EJE_Z;
                                break;
                        }

                        moviendoObjeto = true;
                        posCursorMoviendoObjeto = MousePosition;

                        return selectedEntity;
                    }
                    else
                    {
                        foreach (Component comp in Component.AllComponents)
                        {
                            if (comp.Id == name)
                            {
                                componentesSeleccionados.Add(comp);

                                if (minComponent == null || zMin < minZ)
                                {
                                    minZ = zMin;
                                    minComponent = comp;
                                }

                                break;
                            }
                        }
                    }
                }
            }

            if (componentesSeleccionados.Count > 0)
                return minComponent.Entity;
            else
                return null;
        }

        private void MoverObjeto()
        {
            if (moviendoObjeto)
            {
                Vector3 filtroEje;

                if (ejeMoviendoObjeto == Ejes.EJE_X)
                    filtroEje = Vector3.UnitX;
                else if (ejeMoviendoObjeto == Ejes.EJE_Y)
                    filtroEje = Vector3.UnitY;
                else if (ejeMoviendoObjeto == Ejes.EJE_Z)
                    filtroEje = -Vector3.UnitZ;
                else
                    filtroEje = Vector3.Zero;

                Vector3 right = selectedEntity.Transformation.DirectionX;

                System.Drawing.Point posMouse = MousePosition;

                int dx = posMouse.X - posCursorMoviendoObjeto.X;
                int dy = posMouse.Y - posCursorMoviendoObjeto.Y;

                filtroEje = Vector3.Transform(filtroEje, Matrix4.Rotate(selectedEntity.Transformation.LocalRotation)).Xyz;

                selectedEntity.Transformation.LocalTranslation += filtroEje * (dx + -dy) * deltaTime;

                posCursorMoviendoObjeto = posMouse;
            }
        }

        private void MoverCamara()
        {
            if (moviendoCamara)
            {
                System.Drawing.Point posMouse = MousePosition;

                int dx = posMouse.X - posCursorMoviendoCamara.X;
                int dy = posMouse.Y - posCursorMoviendoCamara.Y;

                if (botonesMouse == MouseButtons.Left)
                {
                    rotCamara.Y += Functions.DegreesToRadians(3.0f * dx * deltaTime);
                    rotCamara.X += Functions.DegreesToRadians(3.0f * dy * deltaTime);
                }
                else if (botonesMouse == MouseButtons.Right)
                {
                    float distancia = deltaTime * -2.0f * dy;

                    Matrix4 mat = Matrix4.Translation((float)Math.Sin(rotCamara.Y) * distancia, (float) -Math.Sin(rotCamara.X) * distancia, (float)Math.Cos(rotCamara.Y) * distancia);

                    posCamara = Vector3.Transform(posCamara, mat).Xyz;
                }
                else if (botonesMouse == MouseButtons.Middle)
                {
                    float distanciaY = deltaTime * 2.0f * dy;

                    posCamara.Y += distanciaY;
                }

                ActualizarMatrizCamara();

                posCursorMoviendoCamara = posMouse;
            }
        }

        private void VistaEscena_MouseUp(object sender, MouseEventArgs e)
        {
            botonesMouse = e.Button;

            moviendoCamara = false;
            moviendoObjeto = false;
        }

        private void VistaEscena_KeyDown(object sender, KeyEventArgs e)
        {
            if (editMode)
            {
                modificadoresTeclas = e.Modifiers;

                if ((modificadoresTeclas & Keys.Control) == 0)
                    moviendoCamara = false;

                if (e.KeyCode == Keys.Delete && e.Modifiers == Keys.None)
                {
                    if (SelectedEntity != null)
                    {
                        SelectedEntity.Destroy();
                        SelectedEntity = null;
                    }
                }

                if (e.KeyCode == Keys.F && e.Modifiers == Keys.None)
                {
                    if (SelectedEntity != null)
                    {
                        CameraPosition = SelectedEntity.Transformation.GlobalTranslation + new Vector3(0, 0, 10);
                        CameraRotation = Vector3.Zero;
                    }
                }

                if (e.KeyCode == Keys.D && e.Modifiers == Keys.Control)
                {
                    if (SelectedEntity != null)
                    {
                        Entity ent = (Entity) SelectedEntity.Clone();

                        if (ent != null)
                            SelectedEntity = ent;
                    }
                }

                if (e.KeyCode == Keys.G && e.Modifiers == Keys.None)
                    drawGrid = !drawGrid;
            }

            ActualizarEstadoInput(e.KeyCode, true);            
        }

        private void VistaEscena_KeyUp(object sender, KeyEventArgs e)
        {
            if (editMode)
            {
                modificadoresTeclas = e.Modifiers;

                if ((modificadoresTeclas & Keys.Control) == 0)
                    moviendoCamara = false;
            }

            ActualizarEstadoInput(e.KeyCode, false);
        }

        private void VistaEscena_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListaRecursos.ListViewItemResource)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void VistaEscena_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListaRecursos.ListViewItemResource)))
            {
                System.Drawing.Point posMouse = this.PointToClient(MousePosition);

                Resource res = ((ListaRecursos.ListViewItemResource)e.Data.GetData(typeof(ListaRecursos.ListViewItemResource))).resource;

                Object assignTo = GetEntidadEnPosicion(posMouse.X, posMouse.Y);

                if (assignTo == null)
                    assignTo = scene;

                if (res.CanAssignTo(assignTo))
                {
                    Object created = res.AssignTo(assignTo);

                    if (created is Entity)
                    {
                        ((Entity)created).Transformation.LocalTranslation = UnprojectPoint(posMouse.X, posMouse.Y);
                        SelectedEntity = ((Entity)created);
                    }
                }
            }
        }

        private void ActualizarEstadoInput(Keys key, bool press)
        {
            if (traduccionTeclas.ContainsKey(key))
                Input.ProcessKeyPress(traduccionTeclas[key], press);
        }
    }
}
