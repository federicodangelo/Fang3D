using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Fang3D.Editor.Controles.EditoresValores
{
    public class EditorBase : UserControl
    {
        protected System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;
        internal AccesorValor accesor;

        internal EditorBase()
        {
        }

        public EditorBase(AccesorValor accesor)
        {
            this.accesor = accesor;
        }

        public virtual void ActualizarValor()
        {
        }
    }
}
