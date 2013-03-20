using System;
using System.Collections.Generic;
using System.Text;
using Fang3D.Attributes;

namespace Fang3D
{
    [DontShowInCreateMenu]
    public class ScriptBase : Component
    {
        internal bool startCalled;

        public virtual void Start()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Collision(Collisionable col)
        {
        }

        public Transformation Transformation
        {
            get
            {
                if (Entity != null)
                    return Entity.Transformation;

                return null;
            }
        }
    }
}
