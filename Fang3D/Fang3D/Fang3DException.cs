using System;
using System.Collections.Generic;
using System.Text;

namespace Fang3D
{
    public class Fang3DException : Exception
    {
        public Fang3DException(String description)
            : base(description)
        {
        }
    }
}
