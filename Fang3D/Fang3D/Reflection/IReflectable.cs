using System;
using System.Collections.Generic;
using System.Text;

namespace Fang3D
{
    public interface IReflectable
    {
        AccesorValor[] GetAccesoresValores();
        FieldValue[] GetValores();
        void SetValores(FieldValue[] values);
    }
}
