namespace Fang3D.Editor.Controles
{
    partial class VistaEscenaConArbol
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.vistaEscena = new Fang3D.Editor.Controles.VistaEscena();
            this.panelDerecho = new System.Windows.Forms.Panel();
            this.editorEntidad = new Fang3D.Editor.Controles.EditorMultiplesReflectables();
            this.arbolEntidades = new Fang3D.Editor.Controles.ArbolEntidades();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelDerecho.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.vistaEscena, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelDerecho, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 499F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(717, 499);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // vistaEscena
            // 
            this.vistaEscena.AllowDrop = true;
            this.vistaEscena.BackColor = System.Drawing.Color.Black;
            this.vistaEscena.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vistaEscena.Location = new System.Drawing.Point(0, 0);
            this.vistaEscena.Margin = new System.Windows.Forms.Padding(0);
            this.vistaEscena.Name = "vistaEscena";
            this.vistaEscena.Pause = false;
            this.vistaEscena.SelectedEntity = null;
            this.vistaEscena.Size = new System.Drawing.Size(455, 499);
            this.vistaEscena.TabIndex = 0;
            this.vistaEscena.VSync = true;
            // 
            // panelDerecho
            // 
            this.panelDerecho.AutoSize = true;
            this.panelDerecho.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelDerecho.Controls.Add(this.editorEntidad);
            this.panelDerecho.Controls.Add(this.arbolEntidades);
            this.panelDerecho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDerecho.Location = new System.Drawing.Point(455, 0);
            this.panelDerecho.Margin = new System.Windows.Forms.Padding(0);
            this.panelDerecho.MinimumSize = new System.Drawing.Size(262, 0);
            this.panelDerecho.Name = "panelDerecho";
            this.panelDerecho.Size = new System.Drawing.Size(262, 499);
            this.panelDerecho.TabIndex = 0;
            // 
            // editorEntidad
            // 
            this.editorEntidad.AutoSize = true;
            this.editorEntidad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.editorEntidad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.editorEntidad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorEntidad.Element = null;
            this.editorEntidad.Location = new System.Drawing.Point(0, 200);
            this.editorEntidad.Margin = new System.Windows.Forms.Padding(0);
            this.editorEntidad.MinimumSize = new System.Drawing.Size(260, 100);
            this.editorEntidad.Name = "editorEntidad";
            this.editorEntidad.Size = new System.Drawing.Size(262, 299);
            this.editorEntidad.TabIndex = 1;
            // 
            // arbolEntidades
            // 
            this.arbolEntidades.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.arbolEntidades.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.arbolEntidades.Dock = System.Windows.Forms.DockStyle.Top;
            this.arbolEntidades.Location = new System.Drawing.Point(0, 0);
            this.arbolEntidades.Margin = new System.Windows.Forms.Padding(0);
            this.arbolEntidades.MinimumSize = new System.Drawing.Size(2, 200);
            this.arbolEntidades.Name = "arbolEntidades";
            this.arbolEntidades.SelectedEntity = null;
            this.arbolEntidades.Size = new System.Drawing.Size(262, 200);
            this.arbolEntidades.TabIndex = 2;
            this.arbolEntidades.UpdateEnabled = true;
            // 
            // VistaEscenaConArbol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "VistaEscenaConArbol";
            this.Size = new System.Drawing.Size(717, 499);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panelDerecho.ResumeLayout(false);
            this.panelDerecho.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelDerecho;
        
        public EditorMultiplesReflectables editorEntidad;
        public ArbolEntidades arbolEntidades;
        public VistaEscena vistaEscena;
    }
}
