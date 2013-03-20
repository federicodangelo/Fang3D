using System;
using System.Collections.Generic;
using Fang3D.Attributes;

namespace Fang3D
{
    public class Entity : Component
    {
        public delegate void EntityCreatedDelegate(Entity ent);
        public delegate void EntityDeletedDelegate(Entity ent);
        public delegate void EntityUpdatedDelegate(Entity ent, String field);

        private List<Component> childComponents = new List<Component>();
        private Transformation transformation;
        private String name;
        private EntityEnumerable entityEnumerable;

        #region EntityInternals

        internal class EntityInternals
        {
            public event EntityCreatedDelegate EntityCreatedEvent;
            public event EntityDeletedDelegate EntityDeletedEvent;
            public event EntityUpdatedDelegate EntityUpdatedEvent;

            public Entity root;

            public bool dispatchEnabled = true;

            public void Init()
            {
                root = new Entity(-1);
            }

            public void DispatchCreated(Entity entity)
            {
                if (dispatchEnabled && EntityCreatedEvent != null)
                    EntityCreatedEvent(entity);
            }

            public void DispatchDeleted(Entity entity)
            {
                if (dispatchEnabled && EntityDeletedEvent != null)
                    EntityDeletedEvent(entity);
            }

            public void DispatchUpdated(Entity entity, String field)
            {
                if (dispatchEnabled && EntityUpdatedEvent != null)
                    EntityUpdatedEvent(entity, field);
            }
        }

        #endregion

        static private EntityInternals Internals
        {
            get { return Scene.Current.entityInternals; }
        }

        private Entity(int k)
        {
            name = "";
            new Transformation().Entity = this;

            Internals.DispatchCreated(this);
        }

        public Entity()
        {
            name = "";

            new Transformation().Entity = this;
            transformation.Parent = Root.Transformation;

            Internals.DispatchCreated(this);
        }

        public String Name
        {
            get
            {
                return name;
            }

            set
            {
                if (name != value)
                {
                    name = value;

                    ReportUpdated("Name");
                }
            }
        }

        static public Entity Root
        {
            get { return Internals.root; }
        }

        internal void AddComponent(Component comp)
        {
            if (Destroyed)
                return;

            if (comp is Transformation)
            {
                if (transformation != null) 
                {
                    Transformation oldTrans = transformation;

                    transformation = (Transformation) comp;

                    childComponents.Add(comp);

                    oldTrans.Destroy();

                    ReportUpdated("Components");

                    return;
                }

                transformation = (Transformation) comp;
            }

            childComponents.Add(comp);

            ReportUpdated("Components");
        }

        internal void RemoveComponent(Component comp)
        {
            childComponents.Remove(comp);

            if (comp == transformation)
                transformation = null;

            ReportUpdated("Components");
        }

        public void ReportUpdated(String name)
        {
            if (Destroyed)
                return;

            Internals.DispatchUpdated(this, name);
        }

        public IEnumerable<Component> ChildComponents
        {
            get
            {
                return childComponents;
            }
        }

        public Transformation Transformation
        {
            get
            {
                return transformation;
            }
        }

        public Entity FindChild(String name)
        {
            foreach (Entity child in Childs)
                if (child.name == name)
                    return child;

            return null;
        }

        public Entity FindChildFull(String name)
        {
            foreach (Entity child in Childs)
            {
                if (child.name == name)
                    return child;

                Entity child2 = child.FindChildFull(name);
                if (child2 != null)
                    return child2;
            }

            return null;
        }

        public Component FindChildComponent(Type type)
        {
            foreach (Component comp in ChildComponents)
                if (type.IsAssignableFrom(comp.GetType()))
                    return comp;

            return null;
        }

        public Component[] FindChildComponents(Type type)
        {
            List<Component> comps = new List<Component>();

            foreach (Component comp in ChildComponents)
                if (type.IsAssignableFrom(comp.GetType()))
                    comps.Add(comp);

            return comps.ToArray();
        }

        public IEnumerable<Entity> Childs
        {
            get
            {
                if (entityEnumerable == null)
                    entityEnumerable = new EntityEnumerable(this);

                return entityEnumerable;
            }
        }

        public override void Destroy()
        {
            if (!Destroyed)
            {
                if (this != Root)
                {
                    base.Destroy();

                    foreach (Entity ent in new List<Entity>(Childs))
                        ent.Destroy();

                    foreach (Component comp in new List<Component>(ChildComponents))
                        comp.Destroy();

                    Internals.DispatchDeleted(this);
                }
            }
        }

        public void CopyValuesTo(Entity ent)
        {
            CopyValuesTo((Component)ent);

            foreach (Component comp in new List<Component>(ChildComponents))
            {
                Component clonedComp = comp.Clone();
                clonedComp.Entity = ent;
            }

            foreach (Entity child in new List<Entity>(Childs))
            {
                Entity ent2 = (Entity) child.Clone();
                ent2.Transformation.Parent = ent.Transformation;
            }

            ent.Transformation.Parent = Transformation.Parent;
        }

        public override Component Clone()
        {
            Entity cloned = (Entity) GetType().GetConstructor(System.Type.EmptyTypes).Invoke(null);

            CopyValuesTo(cloned);

            return cloned;
        }

        public override string ToString()
        {
            return name;
        }

        #region Iterador de childs Entity

        private class EntityEnumerable : IEnumerable<Entity>
        {
            private Entity entity;

            public EntityEnumerable(Entity entity)
            {
                this.entity = entity;
            }

            public IEnumerator<Entity> GetEnumerator()
            {
                return new EntityEnumerator(entity);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return new EntityEnumerator(entity);
            }

            private class EntityEnumerator : IEnumerator<Entity>
            {
                private IEnumerator<Transformation> enumerator;

                #region EmptyEnumerator
                private class EmptyEnumerator : IEnumerator<Transformation>
                {
                    public Transformation Current
                    {
                        get { return null; }
                    }

                    public void Dispose()
                    {
                    }

                    object System.Collections.IEnumerator.Current
                    {
                        get { return null; }
                    }

                    public bool MoveNext()
                    {
                        return false;
                    }

                    public void Reset()
                    {
                        throw new NotImplementedException();
                    }
                }
                #endregion

                public EntityEnumerator(Entity entity)
                {
                    if (entity.transformation != null)
                        enumerator = entity.transformation.Childs.GetEnumerator();
                    else
                        enumerator = new EmptyEnumerator();
                }

                public Entity Current
                {
                    get
                    {
                        if (enumerator.Current != null)
                            return enumerator.Current.Entity;

                        return null;
                    }
                }

                public void Dispose()
                {
                    enumerator.Dispose();
                }

                object System.Collections.IEnumerator.Current
                {
                    get 
                    {
                        if (enumerator.Current != null)
                            return enumerator.Current.Entity;

                        return null;
                    }
                }

                public bool MoveNext()
                {
                    do
                    {
                        if (!enumerator.MoveNext())
                            return false;

                    } while (enumerator.Current.Entity == null);

                    return true;
                }

                public void Reset()
                {
                    throw new NotImplementedException();
                }
            }
        }

        #endregion
    }
}
