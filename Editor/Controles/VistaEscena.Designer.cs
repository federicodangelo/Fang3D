namespace Fang3D.Editor.Controles
{
    partial class VistaEscena
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // VistaEscena
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "VistaEscena";
            this.Size = new System.Drawing.Size(441, 346);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.VistaEscena_Paint);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.VistaEscena_DragDrop);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.VistaEscena_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VistaEscena_MouseDown);
            this.Resize += new System.EventHandler(this.VistaEscena_Resize);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.VistaEscena_MouseUp);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.VistaEscena_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VistaEscena_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
