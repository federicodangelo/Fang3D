using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Math;

namespace Fang3D
{
    public class ParticleRender : Render
    {
        private ParticleSystem particleSystem;

        private uint[] puntos;
        private Vector3[] vertices;
        private uint[] colores;
        private uint cantPuntos;

        public ParticleSystem ParticleSystem
        {
            get { return particleSystem; }
            set { particleSystem = value; puntos = null; vertices = null; colores = null; }
        }

        private void CrearVectores()
        {
            Array.Resize<uint>(ref puntos, particleSystem.particles.Length * 6);
            Array.Resize<uint>(ref colores, particleSystem.particles.Length * 4);
            Array.Resize<Vector3>(ref vertices, particleSystem.particles.Length * 4);

            uint offsetVertices = 0;

            for (int i = 0; i < particleSystem.particles.Length * 6; i += 6)
            {
                puntos[i+0] = offsetVertices + 0;
                puntos[i+1] = offsetVertices + 1;
                puntos[i+2] = offsetVertices + 2;

                puntos[i+3] = offsetVertices + 2;
                puntos[i+4] = offsetVertices + 3;
                puntos[i+5] = offsetVertices + 0;

                offsetVertices += 4;
            }
        }

        private void ActualizarVectores(IBaseRender render)
        {
            uint offsetPuntos = 0;

            Matrix4 camara = render.GetCamara();

            for (int i = 0; i < particleSystem.particles.Length; i++)
            {
                Particle part = particleSystem.particles[i];

                if (part.life > 0)
                {
                    float halfSize = part.size / 2.0f;

                    vertices[offsetPuntos] = new Vector3(-halfSize + part.position.X, -halfSize + part.position.Y, part.position.Z);
                    vertices[offsetPuntos + 1] = new Vector3(halfSize + part.position.X, -halfSize + part.position.Y, part.position.Z);
                    vertices[offsetPuntos + 2] = new Vector3(halfSize + part.position.X, halfSize + part.position.Y, part.position.Z);
                    vertices[offsetPuntos + 3] = new Vector3(-halfSize + part.position.X, halfSize + part.position.Y, part.position.Z);

                    colores[offsetPuntos] = colores[offsetPuntos + 1] = colores[offsetPuntos + 2] = colores[offsetPuntos + 3] = part.color;

                    offsetPuntos += 4;
                }
            }

            cantPuntos = offsetPuntos;
        }

        public override void Draw(IBaseRender render)
        {
            if (particleSystem != null && particleSystem.particles != null)
            {
                if (puntos == null || particleSystem.particles.Length != puntos.Length)
                    CrearVectores();

                ActualizarVectores(render);

                if (ParticleSystem.UseLighting == false)
                    render.TurnLightingOff();

                if (ParticleSystem.GlobalPosition)
                    render.DrawTriangles(Matrix4.Identity, puntos, (int)cantPuntos * 3 / 2, vertices, colores, renderMode);
                else
                    render.DrawTriangles(Entity.Transformation.GlobalMatrix, puntos, (int)cantPuntos * 3 / 2, vertices, colores, renderMode);

                if (ParticleSystem.UseLighting == false)
                    render.TurnLightingOn();
            }
        }

    }
}
