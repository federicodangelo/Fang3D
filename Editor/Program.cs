using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using OpenTK.Math;

namespace Fang3D.Editor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormPrincipal());
        }
    }
}
