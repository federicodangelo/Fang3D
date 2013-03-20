using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Fang3D.Editor.Controles.EditoresValores
{
    public partial class EditorBimap : EditorBase
    {
        public EditorBimap(AccesorValor accesor)
            : base(accesor)
        {
            InitializeComponent();

            ActualizarValor();
        }

        public override void ActualizarValor()
        {
            Bitmap bmp = (Bitmap) accesor.GetValor();

            if (bmp != null)
                pictureBox1.Image = bmp;
            else
                pictureBox1.Image = null;
        }
    }
}
