using System;
using System.Collections.Generic;
using System.Text;
using Fang3D.Attributes;

namespace Fang3D
{
    public class Resource : IReflectable
    {
        static private Dictionary<Type, Resource> resourceDictionary = new Dictionary<Type, Resource>();
        static private uint nextId;

        private bool destroyed;
        private uint id;
        private Resource nextResource;
        private Resource previousResource;
        private String name;
        private String fileName;
        protected bool missing = true;

        public delegate void ResourceCreatedDelegate(Resource res);
        public delegate void ResourceUpdatedDelegate(Resource res);
        public delegate void ResourceDeletedDelegate(Resource res);

        static public event ResourceCreatedDelegate ResourceCreatedEvent;
        static public event ResourceUpdatedDelegate ResourceUpdatedEvent;
        static public event ResourceDeletedDelegate ResourceDeletedEvent;
        

        [ReadOnlyInEditor]
        public String FileName
        {
            get { return fileName; }
            set
            {
                if (fileName != value)
                {
                    fileName = value;

                    OnFileNameUpdated();
                }
            }
        }

        [DontShowInEditor]
        public bool Missing
        {
            get { return missing; }
        }

        public Resource()
        {
            id = ++nextId;
            name = "";

            Resource resFirst;

            if (resourceDictionary.TryGetValue(GetType(), out resFirst))
            {
                if (resFirst != null)
                {
                    nextResource = resFirst.nextResource;
                    previousResource = resFirst;

                    if (resFirst.nextResource != null)
                        resFirst.nextResource.previousResource = this;

                    resFirst.nextResource = this;
                }
                else
                {
                    resourceDictionary[GetType()] = this;
                }
            }
            else
            {
                resourceDictionary.Add(GetType(), this);
            }

            if (ResourceCreatedEvent != null)
                ResourceCreatedEvent(this);
        }

        public uint Id
        {
            get { return id; }
        }

        [ReadOnlyInEditor]
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
                    ReportUpdated();
                }
            }
        }

        [DontShowInEditor]
        public bool Destroyed
        {
            get { return destroyed; }
        }

        ~Resource()
        {
            Destroy();
        }

        public virtual void Destroy()
        {
            if (!destroyed)
            {
                destroyed = true;

                if (nextResource != null)
                    nextResource.previousResource = previousResource;

                if (previousResource != null)
                    previousResource.nextResource = nextResource;
                else
                    resourceDictionary[GetType()] = nextResource;

                if (ResourceDeletedEvent != null)
                    ResourceDeletedEvent(this);
            }
        }

        public virtual bool CanAssignTo(Object obj)
        {
            return false;
        }

        public virtual Object AssignTo(Object obj)
        {
            return null;
        }

        protected virtual void OnFileNameUpdated()
        {
            if (!Destroyed)
                ReportUpdated();
        }

        public virtual void OnFileChanged()
        {
            if (!Destroyed)
                ReportUpdated();
        }

        public virtual void OnFileDeleted()
        {
            if (!Destroyed)
                ReportUpdated();
        }

        public virtual object GetInstance()
        {
            return this;
        }

        static public Resource FindResource(Type type, String name)
        {
            foreach (Resource res in AllResourcesOfType(type))
                if (res.name == name)
                    return res;

            return null;
        }

        public override string ToString()
        {
            return name;
        }

        protected void ReportUpdated()
        {
            if (ResourceUpdatedEvent != null)
                ResourceUpdatedEvent(this);
        }


        public virtual AccesorValor[] GetAccesoresValores()
        {
            return InternalFunctions.GetAccesoresValores(this, true);
        }

        public virtual FieldValue[] GetValores()
        {
            return InternalFunctions.GetValores(this);
        }

        public virtual void SetValores(FieldValue[] values)
        {
            InternalFunctions.SetValores(this, values);
        }

        static public IEnumerable<Resource> AllResources
        {
            get
            {
                return new ResourceEnumerable(null);
            }
        }

        static public IEnumerable<Resource> AllResourcesOfType(Type type)
        {
            return new ResourceEnumerable(type);
        }

        #region Enumerador de todos los componentes

        private class ResourceEnumerable : IEnumerable<Resource>
        {
            Type typeFilter;

            public ResourceEnumerable(Type typeFilter)
            {
                this.typeFilter = typeFilter;
            }

            public IEnumerator<Resource> GetEnumerator()
            {
                return new ResourceEnumerator(typeFilter);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return new ResourceEnumerator(typeFilter);
            }
        }

        private class ResourceEnumerator : IEnumerator<Resource>
        {
            private Type typeFilter;
            private Resource current;
            private bool first = true;
            private Dictionary<Type, Resource>.Enumerator currentDictionary;

            public ResourceEnumerator(Type typeFilter)
            {
                this.typeFilter = typeFilter;
            }

            public Resource Current
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
                    currentDictionary = resourceDictionary.GetEnumerator();

                    SearchNextDictionary();

                    first = false;
                }
                else
                {
                    if (current != null)
                    {
                        current = current.nextResource;

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
