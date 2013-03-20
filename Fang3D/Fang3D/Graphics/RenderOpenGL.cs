using OpenTK.Graphics;
using OpenTK.Math;
using System.Collections.Generic;

namespace Fang3D
{
    public class RenderOpenGL : IBaseRender
    {
        private Dictionary<Texture2D, TextureInfo> loadedTextures = new Dictionary<Texture2D, TextureInfo>();
        private class TextureInfo
        {
            public int id;
        }

        private int width, height;
        private float maxZ, minZ;
        private Matrix4 camara;
        private Matrix4 projection;
        private bool selectionMode;

        static private Color4 InvalidColor4 = new Color4(-1, -1, -1, -1);

        public void Init()
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GL.ClearDepth(1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            //GL.Enable(GL.GL_TEXTURE_2D);

            GL.ShadeModel(ShadingModel.Smooth);
            GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.Enable(EnableCap.CullFace);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.RescaleNormal);

            GL.Enable(EnableCap.ColorMaterial);
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);

            minZ = 0.1f;
            maxZ = 10000.0f;
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public float GetMinZ()
        {
            return minZ;
        }

        public float GetMaxZ()
        {
            return maxZ;
        }

        public Matrix4 GetCamara()
        {
            return camara;
        }

        public void SetCamara(Matrix4 camara)
        {
            this.camara = camara;
        }

        public void SetSreenSize(int width, int height)
        {
            GL.Viewport(0, 0, width, height);

            this.width = width;
            this.height = height;
        }

        public void SetProjectionMatrix(Matrix4 matrix)
        {
            this.projection = matrix;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref matrix);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return projection;
        }

        public void BeginDraw()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void ClearZBuffer()
        {
            GL.Clear(ClearBufferMask.DepthBufferBit);
        }

        public void EndDraw()
        {
        }

        public void BeginSelectionMode(int x, int y, uint[] bufferSelection)
        {
            int[] viewport = new int[4];

            ResetAllLights();
            ClearZBuffer();

            GL.GetInteger(GetPName.Viewport, viewport);

            GL.SelectBuffer(1024, bufferSelection);
            GL.RenderMode(RenderingMode.Select);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            Glu.PickMatrix(x, viewport[3] - y,
                           1.0f, 1.0f, viewport);

            GL.MultMatrix(ref projection);

            GL.MatrixMode(MatrixMode.Modelview);

            GL.InitNames();

            selectionMode = true;
        }

        public int EndSelectionMode()
        {
            int hits = GL.RenderMode(RenderingMode.Render);

            SetProjectionMatrix(projection);

            selectionMode = false;

            return hits;
        }

        private void PreRenderMode(RenderMode renderMode)
        {
            if (selectionMode == false)
            {
                switch (renderMode)
                {
                    case RenderMode.Normal:
                        break;

                    case RenderMode.Lines:
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                        GL.Color3(1.0f, 1.0f, 1.0f);
                        break;
                }
            }
        }

        private void PostRenderMode(RenderMode renderMode)
        {
            if (selectionMode == false)
            {
                switch (renderMode)
                {
                    case RenderMode.Normal:
                        break;

                    case RenderMode.Lines:
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                        break;
                }
            }
        }

        public void Draw(Mesh mesh, Matrix4 matrix, RenderMode mode)
        {
            if (mesh.faceList == null)
                return;

            PreRenderMode(mode);

            DrawMeshInternal(mesh, matrix, null, InvalidColor4);

            PostRenderMode(mode);
        }

        public void Draw(Mesh mesh, Matrix4 matrix, RenderMode mode, OpenTK.Graphics.Color4 color)
        {
            if (mesh.faceList == null)
                return;

            PreRenderMode(mode);

            DrawMeshInternal(mesh, matrix, null, color);

            PostRenderMode(mode);
        }

        public void Draw(Mesh mesh, Matrix4 matrix, RenderMode mode, Texture2D texture)
        {
            if (mesh.faceList == null)
                return;

            LoadTexture(texture);

            GL.Enable(EnableCap.Texture2D);


            PreRenderMode(mode);

            DrawMeshInternal(mesh, matrix, texture, InvalidColor4);

            PostRenderMode(mode);
        }

        private void DrawMeshInternal(Mesh mesh, Matrix4 matrix, Texture2D texture, Color4 color)
        {
            LoadTransformation(matrix);

            GL.EnableClientState(EnableCap.VertexArray);
            GL.VertexPointer(3, VertexPointerType.Float, 0, mesh.vertexList);

            if (selectionMode == false)
            {
                if (texture != null)
                {
                    LoadTexture(texture);
                    GL.Enable(EnableCap.Texture2D);
                }

                if (color == InvalidColor4)
                {
                    if (mesh.colorList != null)
                    {
                        GL.EnableClientState(EnableCap.ColorArray);
                        GL.ColorPointer(4, ColorPointerType.UnsignedByte, 0, mesh.colorList);
                    }
                    else
                        GL.Color3(1.0f, 1.0f, 1.0f);
                }
                else
                    GL.Color4(color.R, color.G, color.B, color.A);

                if (mesh.normalList != null)
                {
                    GL.EnableClientState(EnableCap.NormalArray);
                    GL.NormalPointer(NormalPointerType.Float, 0, mesh.normalList);
                }

                if (texture != null && mesh.coordList != null)
                {
                    GL.EnableClientState(EnableCap.TextureCoordArray);
                    GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, mesh.coordList);
                }

                if (texture != null)
                    BindTexture(texture);
            }

            BeginMode beginMode;

            switch (mesh.primitiveType)
            {
                case Mesh.PrimitiveType.Triangle:
                    beginMode = BeginMode.Triangles;
                    break;
                case Mesh.PrimitiveType.Quad:
                    beginMode = BeginMode.Quads;
                    break;
                default:
                    beginMode = BeginMode.Triangles;
                    break;
            }

            GL.DrawElements(beginMode, mesh.faceList.Length,
                DrawElementsType.UnsignedInt, mesh.faceList);

            if (selectionMode == false)
            {
                if (mesh.normalList != null)
                    GL.DisableClientState(EnableCap.NormalArray);

                if (color.R != -1.0f || color.G != -1.0f || color.B != -1.0f || color.A != -1.0f)
                    if (mesh.colorList != null)
                        GL.DisableClientState(EnableCap.ColorArray);

                if (texture != null && mesh.coordList != null)
                    GL.DisableClientState(EnableCap.TextureCoordArray);

                if (texture != null)
                    GL.Disable(EnableCap.Texture2D);
            }

            GL.DisableClientState(EnableCap.VertexArray);
        }

        private void LoadTransformation(Matrix4 matrix)
        {
            Matrix4 mat2 = matrix * camara;

            GL.LoadMatrix(ref mat2);
        }

        public void GetScreenPoint(int x, int y, out Vector3 p1, out Vector3 p2)
        {
            LoadTransformation(Matrix4.Identity);

            double[] modelMatrix = new double[16];
            double[] projmatrix = new double[16];
            int[] viewport = new int[4];

            GL.GetInteger(GetPName.Viewport, viewport);
            GL.GetDouble(GetPName.ModelviewMatrix, modelMatrix);
            GL.GetDouble(GetPName.ProjectionMatrix, projmatrix);
            y = height - y; // OpenGL renders with (0,0) on bottom, mouse reports with (0,0) on top

            Glu.UnProject(new Vector3(x, y, 0.1f), modelMatrix, projmatrix, viewport, out p1);
            Glu.UnProject(new Vector3(x, y, 1.0f), modelMatrix, projmatrix, viewport, out p2);
        }

        public void GetScreenPoint(int x, int y, Matrix4 model, out Vector3 p1, out Vector3 p2)
        {
            LoadTransformation(model);

            double[] modelMatrix = new double[16];
            double[] projmatrix = new double[16];
            int[] viewport = new int[4];

            GL.GetInteger(GetPName.Viewport, viewport);
            GL.GetDouble(GetPName.ModelviewMatrix, modelMatrix);
            GL.GetDouble(GetPName.ProjectionMatrix, projmatrix);
            y = height - y; // OpenGL renders with (0,0) on bottom, mouse reports with (0,0) on top

            Glu.UnProject(new Vector3(x, y, 0.1f), modelMatrix, projmatrix, viewport, out p1);
            Glu.UnProject(new Vector3(x, y, 1.0f), modelMatrix, projmatrix, viewport, out p2);
        }


        public void BindTexture(Texture2D texture)
        {
            if (loadedTextures.ContainsKey(texture))
                GL.BindTexture(TextureTarget.Texture2D, loadedTextures[texture].id);
        }

        public void UnloadTexture(Texture2D texture)
        {
            if (loadedTextures.ContainsKey(texture))
            {
                GL.DeleteTexture(loadedTextures[texture].id);
                loadedTextures.Remove(texture);
            }
        }

        public void LoadTexture(Texture2D texture)
        {
            if (loadedTextures.ContainsKey(texture) == false)
            {
                GL.Enable(EnableCap.Texture2D);

                TextureInfo ti = new TextureInfo();

                ti.id = GL.GenTexture();

                GL.BindTexture(TextureTarget.Texture2D, ti.id);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Clamp);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Clamp);

                if (texture.Format == Texture2DFormat.RGB)
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb8, texture.Width, texture.Height, 0, PixelFormat.Bgr, PixelType.UnsignedByte, texture.Data);

                loadedTextures.Add(texture, ti);

                GL.Disable(EnableCap.Texture2D);
            }
        }

        public void ResetAllLights()
        {
            GL.Disable(EnableCap.Light0);
            GL.Disable(EnableCap.Light1);
            GL.Disable(EnableCap.Light2);
            GL.Disable(EnableCap.Light3);
            GL.Disable(EnableCap.Light4);
            GL.Disable(EnableCap.Light5);
            GL.Disable(EnableCap.Light6);
            GL.Disable(EnableCap.Light7);
            GL.Disable(EnableCap.Lighting);

            enabledLights = 0;
        }

        private int enabledLights = 0;

        public void AddLight(LightSource light)
        {
            if (enabledLights == 0)
            {
                GL.Enable(EnableCap.Lighting);

                float[] global_ambient = new float[] { 0.0f, 0.0f, 0.0f, 1.0f };
                GL.LightModelv(LightModelParameter.LightModelAmbient, global_ambient);
            }

            if (enabledLights < 8)
            {
                LoadTransformation(light.Entity.Transformation.GlobalMatrix);

                GL.Enable((EnableCap)(EnableCap.Light0 + enabledLights));

                LightName lightName = (LightName)(LightName.Light0 + enabledLights);

                GL.Lightv(lightName, LightParameter.Ambient, light.AmbientColor);
                GL.Lightv(lightName, LightParameter.Diffuse, light.DiffuseColor);

                if (light.LightType == LightType.SPOT)
                {
                    GL.Lightv(lightName, LightParameter.Position, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
                    GL.Lightv(lightName, LightParameter.SpotDirection, new Vector4(0.0f, 0.0f, -1.0f, 0.0f));
                    GL.Light(lightName, LightParameter.SpotCutoff, light.SpotCutoff);
                    GL.Light(lightName, LightParameter.SpotExponent, light.SpotExponent);
                }
                else
                {
                    GL.Lightv(lightName, LightParameter.Position, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
                    GL.Lightv(lightName, LightParameter.SpotDirection, new Vector4(0.0f, 0.0f, 0.0f, 0.0f));
                    GL.Light(lightName, LightParameter.SpotCutoff, 180.0f);
                    GL.Light(lightName, LightParameter.SpotExponent, 0);
                }

                if (light.Distance > 0.0f)
                {
                    float dist = 1.0f / light.Distance;

                    if (light.Quadratic)
                    {
                        GL.Light(lightName, LightParameter.ConstantAttenuation, 1.0f);
                        GL.Light(lightName, LightParameter.LinearAttenuation, 0.0f);
                        GL.Light(lightName, LightParameter.QuadraticAttenuation, dist);
                    }
                    else
                    {
                        GL.Light(lightName, LightParameter.ConstantAttenuation, 1.0f);
                        GL.Light(lightName, LightParameter.LinearAttenuation, dist);
                        GL.Light(lightName, LightParameter.QuadraticAttenuation, 0.0f);
                    }
                }
                else
                {
                    GL.Light(lightName, LightParameter.ConstantAttenuation, 1.0f);
                    GL.Light(lightName, LightParameter.LinearAttenuation, 0.0f);
                    GL.Light(lightName, LightParameter.QuadraticAttenuation, 0.0f);
                }

                enabledLights++;
            }
        }

        public void TurnLightingOff()
        {
            GL.Disable(EnableCap.Lighting);
        }

        public void TurnLightingOn()
        {
            GL.Enable(EnableCap.Lighting);
        }

        public void DrawQuads(Matrix4 matrix, uint[] indices, int lenIndices, Vector3[] vertexs, uint[] colors, RenderMode mode)
        {
            LoadTransformation(matrix);

            PreRenderMode(mode);

            GL.EnableClientState(EnableCap.VertexArray);
            GL.VertexPointer(3, VertexPointerType.Float, 0, vertexs);

            GL.EnableClientState(EnableCap.ColorArray);
            GL.ColorPointer(4, ColorPointerType.UnsignedByte, 0, colors);

            GL.DrawElements(BeginMode.Quads, lenIndices,
                DrawElementsType.UnsignedInt, indices);

            GL.DisableClientState(EnableCap.ColorArray);
            GL.DisableClientState(EnableCap.VertexArray);

            PostRenderMode(mode);
        }

        public void DrawTriangles(Matrix4 matrix, uint[] indices, int lenIndices, Vector3[] vertexs, uint[] colors, RenderMode mode)
        {
            LoadTransformation(matrix);

            PreRenderMode(mode);

            GL.EnableClientState(EnableCap.VertexArray);
            GL.VertexPointer(3, VertexPointerType.Float, 0, vertexs);

            GL.EnableClientState(EnableCap.ColorArray);
            GL.ColorPointer(4, ColorPointerType.UnsignedByte, 0, colors);

            GL.DrawElements(BeginMode.Triangles, lenIndices,
                DrawElementsType.UnsignedInt, indices);

            GL.DisableClientState(EnableCap.ColorArray);
            GL.DisableClientState(EnableCap.VertexArray);

            PostRenderMode(mode);
        }
    }
}
