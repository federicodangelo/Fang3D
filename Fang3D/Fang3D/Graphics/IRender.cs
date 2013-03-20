using System;
using OpenTK.Math;

namespace Fang3D
{
    public enum RenderMode
    {
        Normal,
        Lines
    }

    public interface IBaseRender
    {
        void BeginDraw();
        void Draw(Mesh mesh, Matrix4 matrix, RenderMode mode);
        void Draw(Mesh mesh, Matrix4 matrix, RenderMode mode, OpenTK.Graphics.Color4 color);
        void Draw(Mesh mesh, Matrix4 matrix, RenderMode mode, Texture2D texture);
        void EndDraw();

        Matrix4 GetCamara();
        void SetCamara(Matrix4 camara);

        void BeginSelectionMode(int x, int y, uint[] bufferSelection);
        int EndSelectionMode();

        void GetScreenPoint(int x, int y, out Vector3 p1, out Vector3 p2);
        void GetScreenPoint(int x, int y, Matrix4 mat, out Vector3 p1, out Vector3 p2);

        void Init();
        void SetSreenSize(int width, int height);
        void SetProjectionMatrix(Matrix4 matrix);
        Matrix4 GetProjectionMatrix();

        int GetWidth();
        int GetHeight();
        float GetMinZ();
        float GetMaxZ();

        void ClearZBuffer();

        void BindTexture(Texture2D texture);
        void UnloadTexture(Texture2D texture);
        void LoadTexture(Texture2D texture);

        void ResetAllLights();
        void AddLight(LightSource light);
        void TurnLightingOff();
        void TurnLightingOn();

        void DrawQuads(Matrix4 matrix, uint[] indices, int lenIndices, Vector3[] vertexs, uint[] colors, RenderMode mode);
        void DrawTriangles(Matrix4 matrix, uint[] indices, int lenIndices, Vector3[] vertexs, uint[] colors, RenderMode mode);
    }
}
