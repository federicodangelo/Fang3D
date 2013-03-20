using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics;

namespace Fang3D
{
    static public class ComponentFactory
    {
        static public Entity NewCube()
        {
            Entity ent = new Entity();
            MeshRender renderer = new MeshRender();
            new CollisionableOBB().Entity = ent;
            renderer.Entity = ent;
            renderer.Mesh = (Mesh)Resource.FindResource(typeof(Mesh), "Cube");

            ent.Name = "Cube";

            return ent;
        }

        static public Entity NewLight()
        {
            Entity ent = new Entity();
            LightSource light = new LightSource();
            light.Entity = ent;
            light.DiffuseColor = new Color4(1.0f, 1.0f, 1.0f, 1.0f);
            light.AmbientColor = new Color4(0.2f, 0.2f, 0.2f, 1.0f);

            ent.Name = "Light";

            return ent;
        }

        static public Entity NewParticleSystem()
        {
            Entity ent = new Entity();
            ParticleSystem partSystem = new ParticleSystem();
            ParticleRender partRender = new ParticleRender();

            partRender.Entity = ent;
            partSystem.Entity = ent;
            partRender.ParticleSystem = partSystem;

            ent.Name = "Particle System";

            return ent;
        }
    }
}
