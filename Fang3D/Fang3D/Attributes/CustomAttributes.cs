using System;
using System.Collections.Generic;
using System.Text;

namespace Fang3D.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class DontShowInEditor : System.Attribute
    {
    }

    [AttributeUsage(AttributeTargets.All)]
    public class DontStore : System.Attribute
    {
    }

    [AttributeUsage(AttributeTargets.All)]
    public class DontShowInCreateMenu : System.Attribute
    {
    }

    [AttributeUsage(AttributeTargets.All)]
    public class ReadOnlyInEditor : System.Attribute
    {
    }
}
