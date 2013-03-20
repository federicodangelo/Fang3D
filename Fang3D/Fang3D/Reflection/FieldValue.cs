using System;
using System.Collections.Generic;
using System.Text;

namespace Fang3D
{
    public class FieldValue
    {
        public String name;
        public Type type;
        public Object value;

        public FieldValue(String name, Type type, Object value)
        {
            this.name = name;
            this.type = type;
            this.value = value;
        }
    }
}
