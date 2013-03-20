using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Fang3D.Editor
{
    public partial class FormConsole : Form
    {
        public FormConsole()
        {
            Console.ConsoleLineEvent += new Console.ConsoleLineDelegate(Console_ConsoleLineEvent);

            InitializeComponent();
        }

        void Console_ConsoleLineEvent(string text, Console.TextType textType)
        {
            if (InvokeRequired)
            {
                Invoke(new Console.ConsoleLineDelegate(Console_ConsoleLineEvent), text, textType);
                return;
            }

            switch (textType)
            {
                case Console.TextType.LOG:
                    richTextBox1.SelectionColor = Color.Black;
                    richTextBox1.AppendText(" - LOG: " + text + "\n");
                    break;

                case Console.TextType.WARNING:
                    richTextBox1.SelectionColor = Color.Yellow;
                    richTextBox1.AppendText(" - WARNING: " + text + "\n");
                    break;

                case Console.TextType.ERROR:
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.AppendText(" - ERROR: " + text + "\n");
                    break;
            }
        }

        private void FormConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void buttonLimpiar_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void checkBoxSiempreVisible_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = checkBoxSiempreVisible.Checked;
        }
    }
}
