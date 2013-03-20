using System.Collections.Generic;
using Fang3D.Attributes;

namespace Fang3D
{
    [DontShowInCreateMenu]
    public class Render : Component
    {
        public RenderMode renderMode = RenderMode.Normal;

        public virtual void Draw(IBaseRender render)
        {
        }
    }
}
