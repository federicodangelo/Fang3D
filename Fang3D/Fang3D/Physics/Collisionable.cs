using System;
using System.Collections.Generic;
using System.Text;
using Fang3D.Attributes;

namespace Fang3D
{
    [DontShowInCreateMenu]
    public class Collisionable : Component
    {
        private bool renderInEditor = true;

        public bool RenderInEditor
        {
            get { return renderInEditor; }
            set { renderInEditor = value; }
        }

        public virtual bool CollidesWith(Collisionable col)
        {
            return false;
        }

        public virtual void DrawHelper(IBaseRender render)
        {

        }
    }
}
