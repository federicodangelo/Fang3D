namespace Fang3D.Editor.Controles
{
    partial class EditorReflectable
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonActualizar = new System.Windows.Forms.Button();
            this.labelNombreComponente = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.eliminarComponenteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 16);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(260, 11);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonActualizar
            // 
            this.buttonActualizar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonActualizar.Location = new System.Drawing.Point(0, 27);
            this.buttonActualizar.Margin = new System.Windows.Forms.Padding(0);
            this.buttonActualizar.Name = "buttonActualizar";
            this.buttonActualizar.Size = new System.Drawing.Size(260, 23);
            this.buttonActualizar.TabIndex = 1;
            this.buttonActualizar.Text = "Actualizar";
            this.buttonActualizar.UseVisualStyleBackColor = true;
            this.buttonActualizar.Click += new System.EventHandler(this.buttonActualizar_Click);
            // 
            // labelNombreComponente
            // 
            this.labelNombreComponente.BackColor = System.Drawing.Color.SteelBlue;
            this.labelNombreComponente.ContextMenuStrip = this.contextMenuStrip1;
            this.labelNombreComponente.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelNombreComponente.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNombreComponente.ForeColor = System.Drawing.Color.White;
            this.labelNombreComponente.Location = new System.Drawing.Point(0, 0);
            this.labelNombreComponente.Margin = new System.Windows.Forms.Padding(0);
            this.labelNombreComponente.Name = "labelNombreComponente";
            this.labelNombreComponente.Size = new System.Drawing.Size(260, 16);
            this.labelNombreComponente.TabIndex = 2;
            this.labelNombreComponente.Text = "Nombre Componente";
            this.labelNombreComponente.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eliminarComponenteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(186, 26);
            // 
            // eliminarComponenteToolStripMenuItem
            // 
            this.eliminarComponenteToolStripMenuItem.Name = "eliminarComponenteToolStripMenuItem";
            this.eliminarComponenteToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.eliminarComponenteToolStripMenuItem.Text = "Eliminar Componente";
            this.eliminarComponenteToolStripMenuItem.Click += new System.EventHandler(this.eliminarComponenteToolStripMenuItem_Click);
            // 
            // EditorReflectable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.labelNombreComponente);
            this.Controls.Add(this.buttonActualizar);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(260, 50);
            this.Name = "EditorReflectable";
            this.Size = new System.Drawing.Size(260, 50);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonActualizar;
        private System.Windows.Forms.Label labelNombreComponente;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem eliminarComponenteToolStripMenuItem;

    }
}
