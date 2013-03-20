using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OpenTK.Math;

namespace Fang3D
{
    static public class StaticResources
    {
        static public void Init()
        {
            #region CubeMesh

            /*Mesh cubeMesh = new Mesh();
            cubeMesh.Name = "Cube";

            cubeMesh.vertexList = new Vector3[]
                {
                    new Vector3(-0.5f, -0.5f,  0.5f),
                    new Vector3( 0.5f, -0.5f,  0.5f),
                    new Vector3( 0.5f,  0.5f,  0.5f),
                    new Vector3(-0.5f,  0.5f,  0.5f),
                    new Vector3(-0.5f, -0.5f, -0.5f),
                    new Vector3( 0.5f, -0.5f, -0.5f), 
                    new Vector3( 0.5f,  0.5f, -0.5f),
                    new Vector3(-0.5f,  0.5f, -0.5f)
                };

            cubeMesh.faceList = new uint[]
                {
                    // front face
                    0, 1, 2, 2, 3, 0,
                    // top face
                    3, 2, 6, 6, 7, 3,
                    // back face
                    7, 6, 5, 5, 4, 7,
                    // left face
                    4, 0, 3, 3, 7, 4,
                    // bottom face
                    0, 5, 1, 5, 0, 4,
                    // right face
                    1, 5, 6, 6, 2, 1,
                };

            cubeMesh.colorList = new uint[]
                {
                    0xFFFF0000,
                    0xFFFF0000,
                    0xFF00FF00,
                    0xFF00FF00,
                    0xFF0000FF,
                    0xFF0000FF,
                    0xFFFF00FF,
                    0xFFFF00FF,
                };

            cubeMesh.coordList = new Vector2[]
                {
                    new Vector2(0.0f, 0.0f),
                    new Vector2(1.0f, 0.0f),
                    new Vector2(1.0f, 1.0f),
                    new Vector2(0.0f, 1.0f),

                    new Vector2(0.0f, 0.0f),
                    new Vector2(0.0f, 1.0f),
                    new Vector2(1.0f, 0.0f),
                    new Vector2(1.0f, 1.0f),
                };*/
            #endregion
        }
    }
}
