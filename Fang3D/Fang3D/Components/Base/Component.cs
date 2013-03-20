using System.Collections.Generic;
using System.Reflection;
using System;
using Fang3D.Attributes;

namespace Fang3D
{
    [DontShowInCreateMenu]
    public class Component : IReflectable
    {
        private Component nextComponent;
        private Component previousComponent;

        private bool destroyed;
        private Entity entity;
        private uint id;

        private bool enabled = true;

        #region ComponentInternals

        internal class ComponentInternals
        {
            internal bool insideUpdate;

            internal uint nextId;
            internal Dictionary<Type, Component> componentDictionary;

            internal Queue<Component> creationList;
            internal Queue<Component> destructionList;

            public void Init()
            {
                componentDictionary = new Dictionary<Type, Component>();
                creationList = new Queue<Component>();
                destructionList = new Queue<Component>();
            }
        }

        #endregion

        static private ComponentInternals Internals
        {
            get { return Scene.Current.componentInternals; }
        }

        [DontShowInEditor]
        public bool Destroyed
        {
            get { return destroyed; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { this.enabled = value; }
        }

        public Component()
        {
            id = ++Internals.nextId;

            if (Internals.insideUpdate == false)
                RegisterComponent(this);
            else
                Internals.creationList.Enqueue(this);
        }

        public Component(Entity entity) : this()
        {
            Entity = entity;
        }

        [DontShowInEditor]
        public Entity Entity
        {
            get
            {
                return entity;
            }
            set
            {
                if (entity != value)
                {
                    if (entity != null)
                        entity.RemoveComponent(this);

                    entity = value;

                    if (!destroyed && entity != null)
                        entity.AddComponent(this);
                }
            }
        }

        public uint Id
        {
            get
            {
                return id;
            }
        }

        internal void ForceId(uint id)
        {
            this.id = id;

            if (id >= Internals.nextId)
                Internals.nextId = id + 1;
        }

        ~Component()
        {
            Destroy();
        }

        public virtual void Destroy()
        {
            if (!destroyed)
            {
                destroyed = true;

                Entity = null;

                if (Internals.insideUpdate == false)
                    UnregisterComponent(this);
                else
                    Internals.destructionList.Enqueue(this);
            }
        }

        public virtual AccesorValor[] GetAccesoresValores()
        {
            return InternalFunctions.GetAccesoresValores(this, false);
        }

        public virtual FieldValue[] GetValores()
        {
            return InternalFunctions.GetValores(this);
        }

        public virtual void SetValores(FieldValue[] values)
        {
            InternalFunctions.SetValores(this, values);
        }

        static public void EnterUpdate()
        {
            Internals.insideUpdate = true;
        }

        static public void LeaveUpdate()
        {
            Internals.insideUpdate = false;

            while (Internals.creationList.Count > 0)
                RegisterComponent(Internals.creationList.Dequeue());

            while (Internals.destructionList.Count > 0)
                UnregisterComponent(Internals.destructionList.Dequeue());
        }


        static private void RegisterComponent(Component comp)
        {
            Component compFirst;

            if (Internals.componentDictionary.TryGetValue(comp.GetType(), out compFirst))
            {
                if (compFirst != null)
                {
                    comp.nextComponent = compFirst.nextComponent;
                    comp.previousComponent = compFirst;

                    if (compFirst.nextComponent != null)
                        compFirst.nextComponent.previousComponent = comp;

                    compFirst.nextComponent = comp;
                }
                else
                {
                    Internals.componentDictionary[comp.GetType()] = comp;
                }
            }
            else
            {
                Internals.componentDictionary.Add(comp.GetType(), comp);
            }
        }

        static private void UnregisterComponent(Component comp)
        {
            if (comp.nextComponent != null)
                comp.nextComponent.previousComponent = comp.previousComponent;

            if (comp.previousComponent != null)
                comp.previousComponent.nextComponent = comp.nextComponent;
            else
                Internals.componentDictionary[comp.GetType()] = comp.nextComponent;
        }



        static public IEnumerable<Component> AllComponents
        {
            get
            {
                return new ComponentEnumerable(null);
            }
        }

        static public IEnumerable<Component> AllComponentsOfType(Type type)
        {
            return new ComponentEnumerable(type);
        }

        public void CopyValuesTo(Component comp)
        {
            FieldValue[] valores = GetValores();
            List<FieldValue> valoresFiltrados = new List<FieldValue>();

            foreach (FieldValue fieldValue in valores)
                if (fieldValue.name != "Entity")
                    valoresFiltrados.Add(fieldValue);
            
            comp.SetValores(valoresFiltrados.ToArray());
        }

        public virtual Component Clone()
        {
            Component cloned = (Component) GetType().GetConstructor(System.Type.EmptyTypes).Invoke(null);

            CopyValuesTo(cloned);

            return cloned;
        }

        #region Enumerador de todos los componentes

        private class ComponentEnumerable : IEnumerable<Component>
        {
            Type typeFilter;

            public ComponentEnumerable(Type typeFilter)
            {
                this.typeFilter = typeFilter;
            }

            public IEnumerator<Component> GetEnumerator()
            {
                return new ComponentEnumerator(typeFilter);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return new ComponentEnumerator(typeFilter);
            }
        }

        private class ComponentEnumerator : IEnumerator<Component>
        {
            private Type typeFilter;
            private Component current;
            private bool first = true;
            private Dictionary<Type, Component>.Enumerator currentDictionary;

            public ComponentEnumerator(Type typeFilter)
            {
                this.typeFilter = typeFilter;
            }

            public Component Current
            {
                get
                {
                    return current;
                }
            }

            public void Dispose()
            {
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return current;
                }
            }

            public bool MoveNext()
            {
                if (first)
                {
                    currentDictionary = Internals.componentDictionary.GetEnumerator();

                    SearchNextDictionary();

                    first = false;
                }
                else
                {
                    if (current != null)
                    {
                        current = current.nextComponent;

                        if (current == null)
                            SearchNextDictionary();
                    }
                }

                return current != null;
            }

            private void SearchNextDictionary()
            {
                if (typeFilter != null)
                {
                    while (currentDictionary.MoveNext())
                    {
                        if (typeFilter.IsAssignableFrom(currentDictionary.Current.Key) && currentDictionary.Current.Value != null)
                        {
                            current = currentDictionary.Current.Value;
                            break;
                        }
                    }
                }
                else
                {
                    while (currentDictionary.MoveNext())
                    {
                        if (currentDictionary.Current.Value != null)
                        {
                            current = currentDictionary.Current.Value;
                            break;
                        }
                    }
                }
            }

            public void Reset()
            {
                throw new System.NotImplementedException();
            }
        }

        #endregion
    }
}
