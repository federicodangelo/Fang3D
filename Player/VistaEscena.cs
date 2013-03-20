using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using OpenTK.Math;
using OpenTK.Input;

namespace Fang3D.Player
{
    public class VistaEscena : OpenTK.GameWindow
    {
        public Scene scene;
        private IBaseRender render = new RenderOpenGL();
        private Dictionary<Key, KeyEnum> traduccionTeclas;
        
        public VistaEscena() : base(800, 600, GraphicsMode.Default, "Fang3D! Player")
        {
            Keyboard.KeyDown += new OpenTK.Input.KeyDownEvent(Keyboard_KeyDown);
            Keyboard.KeyUp += new OpenTK.Input.KeyUpEvent(Keyboard_KeyUp);

            #region Inicialización de traducción de teclas

            traduccionTeclas = new Dictionary<Key, KeyEnum>();

            traduccionTeclas.Add(Key.Left, KeyEnum.Left);
            traduccionTeclas.Add(Key.Right, KeyEnum.Right);
            traduccionTeclas.Add(Key.Up, KeyEnum.Up);
            traduccionTeclas.Add(Key.Down, KeyEnum.Down);
            traduccionTeclas.Add(Key.Space, KeyEnum.Space);
            traduccionTeclas.Add(Key.A, KeyEnum.A);
            traduccionTeclas.Add(Key.B, KeyEnum.B);
            traduccionTeclas.Add(Key.C, KeyEnum.C);
            traduccionTeclas.Add(Key.D, KeyEnum.D);
            traduccionTeclas.Add(Key.E, KeyEnum.E);
            traduccionTeclas.Add(Key.F, KeyEnum.F);
            traduccionTeclas.Add(Key.G, KeyEnum.G);
            traduccionTeclas.Add(Key.H, KeyEnum.H);
            traduccionTeclas.Add(Key.I, KeyEnum.I);
            traduccionTeclas.Add(Key.J, KeyEnum.J);
            traduccionTeclas.Add(Key.K, KeyEnum.K);
            traduccionTeclas.Add(Key.L, KeyEnum.L);
            traduccionTeclas.Add(Key.M, KeyEnum.M);
            traduccionTeclas.Add(Key.N, KeyEnum.N);
            traduccionTeclas.Add(Key.O, KeyEnum.O);
            traduccionTeclas.Add(Key.P, KeyEnum.P);
            traduccionTeclas.Add(Key.Q, KeyEnum.Q);
            traduccionTeclas.Add(Key.R, KeyEnum.R);
            traduccionTeclas.Add(Key.S, KeyEnum.S);
            traduccionTeclas.Add(Key.T, KeyEnum.T);
            traduccionTeclas.Add(Key.U, KeyEnum.U);
            traduccionTeclas.Add(Key.V, KeyEnum.V);
            traduccionTeclas.Add(Key.W, KeyEnum.W);
            traduccionTeclas.Add(Key.X, KeyEnum.X);
            traduccionTeclas.Add(Key.Y, KeyEnum.Y);
            traduccionTeclas.Add(Key.Z, KeyEnum.Z);
            traduccionTeclas.Add(Key.Number0, KeyEnum.Number0);
            traduccionTeclas.Add(Key.Number1, KeyEnum.Number1);
            traduccionTeclas.Add(Key.Number2, KeyEnum.Number2);
            traduccionTeclas.Add(Key.Number3, KeyEnum.Number3);
            traduccionTeclas.Add(Key.Number4, KeyEnum.Number4);
            traduccionTeclas.Add(Key.Number5, KeyEnum.Number5);
            traduccionTeclas.Add(Key.Number6, KeyEnum.Number6);
            traduccionTeclas.Add(Key.Number7, KeyEnum.Number7);
            traduccionTeclas.Add(Key.Number8, KeyEnum.Number8);
            traduccionTeclas.Add(Key.Number9, KeyEnum.Number9);

            #endregion
        }

        public override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InicializarGL();
        }

        private void SetearPerspectiva()
        {
            render.SetSreenSize(Width, Height);
            render.SetProjectionMatrix(Matrix4.Perspective(45.0f, (float)Width / (float)Height, render.GetMinZ(), render.GetMaxZ()));
        }

        public void InicializarGL()
        {
            render.Init();

            SetearPerspectiva();
        }

        public override void OnUpdateFrame(OpenTK.UpdateFrameEventArgs e)
        {
            if (e.Time != 0)
                scene.Update((float) e.Time);
        }

        public override void OnRenderFrame(OpenTK.RenderFrameEventArgs e)
        {
            foreach (Camara camara in Component.AllComponentsOfType(typeof(Camara)))
            {
                if (camara.Enabled)
                {
                    Matrix4 matCamara = Matrix4.Invert(camara.Transformation.GlobalMatrix);
                    render.SetCamara(matCamara);
                    break;
                }
            }

            render.BeginDraw();

            render.ResetAllLights();

            scene.Render(render);

            render.EndDraw();

            SwapBuffers();
        }

        protected override void OnResize(OpenTK.Platform.ResizeEventArgs e)
        {
            base.OnResize(e);

            SetearPerspectiva();
        }

        void Keyboard_KeyUp(OpenTK.Input.KeyboardDevice sender, OpenTK.Input.Key key)
        {
            ActualizarEstadoInput(key, false);
        }

        void Keyboard_KeyDown(OpenTK.Input.KeyboardDevice sender, OpenTK.Input.Key key)
        {
            ActualizarEstadoInput(key, true);
        }
       
        private void ActualizarEstadoInput(Key key, bool press)
        {
            if (traduccionTeclas.ContainsKey(key))
                Input.ProcessKeyPress(traduccionTeclas[key], press);
        }
    }
}
