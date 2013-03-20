using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Fang3D.Attributes;
using System.Xml.Serialization;
using System.IO;
using OpenTK.Math;

namespace Fang3D
{
    public class Scene
    {
        static private Scene currentScene;
        static private Stack<Scene> stackScenes = new Stack<Scene>();

        internal Component.ComponentInternals componentInternals;
        internal Entity.EntityInternals entityInternals;
        
        private bool editMode = false;
        private PhysicsEngine physicsEngine;

        static public Scene Current
        {
            get { return currentScene; }
        }

        static public void PushCurrentScene()
        {
            stackScenes.Push(currentScene);
        }

        static public void PopCurrentScene()
        {
            currentScene = stackScenes.Pop();
        }

        public Scene(bool editMode)
        {
            PushCurrentScene();

            this.editMode = editMode;

            try
            {
                MakeCurrent();

                componentInternals = new Component.ComponentInternals();
                entityInternals = new Entity.EntityInternals();

                componentInternals.Init();
                entityInternals.Init();
            }
            finally
            {
                PopCurrentScene();
            }
        }

        public bool EditMode
        {
            get { return editMode; }
        }

        public void MakeCurrent()
        {
            currentScene = this;
        }

        public void DestroyAllComponents()
        {
            PushCurrentScene();

            try
            {
                MakeCurrent();

                foreach (Component comp in new List<Component>(Component.AllComponents))
                    if (comp != Entity.Root && comp.Entity != Entity.Root)
                        comp.Destroy();

                physicsEngine = null;
            }
            finally
            {
                PopCurrentScene();
            }
        }

        public Scene Clone()
        {
            Scene cloned = new Scene(editMode);

            CopyTo(cloned);

            return cloned;
        }

        public void CopyTo(Scene destination)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Scenes.Serializer.SceneSerializer.Save(this, ms);

                ms.Seek(0, SeekOrigin.Begin);

                Scenes.Serializer.SceneSerializer.Load(destination, ms);
            }
        }

        public void AddEntityCreatedEvent(Entity.EntityCreatedDelegate del)
        {
            entityInternals.EntityCreatedEvent += del;
        }

        public void AddEntityDeletedEvent(Entity.EntityDeletedDelegate del)
        {
            entityInternals.EntityDeletedEvent += del;
        }

        public void AddEntityUpdatedEvent(Entity.EntityUpdatedDelegate del)
        {
            entityInternals.EntityUpdatedEvent += del;
        }

        public bool DispatchEnabled
        {
            get { return entityInternals.dispatchEnabled; }
            set { entityInternals.dispatchEnabled = value; }
        }

        public void Render(IBaseRender render)
        {
            List<LightSource> lights = new List<LightSource>();

            foreach (LightSource light in Component.AllComponentsOfType(typeof(LightSource)))
                if (light.Enabled)
                    lights.Add(light);

            if (lights.Count < 8)
            {
                render.ResetAllLights();

                foreach (LightSource light in lights)
                    render.AddLight(light);

                foreach (Render comp in Component.AllComponentsOfType(typeof(Render)))
                    if (comp.Enabled)
                        comp.Draw(render);
            }
            else
            {
                foreach (Render comp in Component.AllComponentsOfType(typeof(Render)))
                {
                    if (comp.Enabled)
                    {
                        render.ResetAllLights();

                        lucesOrdenar = new List<LuzOrdenable>(lights.Count);
                        Vector3 centroLuces = comp.Entity.Transformation.GlobalTranslation;

                        foreach (LightSource light in lights)
                            lucesOrdenar.Add(new LuzOrdenable(light, (light.Entity.Transformation.GlobalTranslation - centroLuces).LengthSquared));

                        lucesOrdenar.Sort(OrdenarLucesPorDistancia);

                        for (int k = 0; k < 8; k++)
                            render.AddLight(lucesOrdenar[k].light);
                        
                        comp.Draw(render);
                    }
                }
            }
        }

        private List<LuzOrdenable> lucesOrdenar;
        private class LuzOrdenable
        {
            public LightSource light;
            public float distancia;

            public LuzOrdenable(LightSource source, float distancia)
            {
                this.light = source;
                this.distancia = distancia;
            }
        }

        private static int OrdenarLucesPorDistancia(LuzOrdenable l1, LuzOrdenable l2)
        {
            if (l1.light.Priority > l2.light.Priority)
                return -1;
            else if (l1.light.Priority < l2.light.Priority)
                return 1;

            if (l1.distancia > l2.distancia)
                return 1;
            else if (l2.distancia > l1.distancia)
                return -1;

            return 0;
        }



        public void Update(float deltaTime)
        {
            Time.deltaTime = deltaTime;
            Scene.PushCurrentScene();

            if (physicsEngine == null)
                physicsEngine = new PhysicsEngine();

            try
            {
                MakeCurrent();
                
                PhysicsEngine.CollisionPair[] collisions = physicsEngine.Update(deltaTime);

                Component.EnterUpdate();

                try
                {
                    foreach (ScriptBase script in Component.AllComponentsOfType(typeof(ScriptBase)))
                        if (!script.Destroyed && script.Enabled && script.startCalled == false)
                        {
                            script.Start();
                            script.startCalled = true;
                        }

                    /*Component.LeaveUpdate();
                    Component.EnterUpdate();*/

                    if (collisions.Length > 0)
                    {
                        foreach (PhysicsEngine.CollisionPair cp in collisions)
                            if (!cp.col1.Destroyed)
                                foreach (ScriptBase script in cp.col1.Entity.FindChildComponents(typeof(ScriptBase)))
                                    if (!script.Destroyed && script.Enabled)
                                        script.Collision(cp.col2);

                        /*Component.LeaveUpdate();
                        Component.EnterUpdate();*/
                    }

                    foreach (ScriptBase script in Component.AllComponentsOfType(typeof(ScriptBase)))
                        if (!script.Destroyed && script.Enabled)
                            script.Update();
                }
                finally
                {
                    Component.LeaveUpdate();
                }
            }
            finally
            {
                Scene.PopCurrentScene();
            }
        }
    }
}
