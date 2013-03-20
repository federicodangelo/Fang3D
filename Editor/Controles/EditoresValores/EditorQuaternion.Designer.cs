namespace Fang3D.Editor.Controles.EditoresValores
{
    partial class EditorQuaternion
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelX = new System.Windows.Forms.Label();
            this.textBoxX = new System.Windows.Forms.TextBox();
            this.labelY = new System.Windows.Forms.Label();
            this.textBoxY = new System.Windows.Forms.TextBox();
            this.labelZ = new System.Windows.Forms.Label();
            this.textBoxZ = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.labelX);
            this.flowLayoutPanel1.Controls.Add(this.textBoxX);
            this.flowLayoutPanel1.Controls.Add(this.labelY);
            this.flowLayoutPanel1.Controls.Add(this.textBoxY);
            this.flowLayoutPanel1.Controls.Add(this.labelZ);
            this.flowLayoutPanel1.Controls.Add(this.textBoxZ);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(231, 20);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // labelX
            // 
            this.labelX.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelX.AutoSize = true;
            this.labelX.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.labelX.Location = new System.Drawing.Point(0, 3);
            this.labelX.Margin = new System.Windows.Forms.Padding(0);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(17, 13);
            this.labelX.TabIndex = 0;
            this.labelX.Text = "X:";
            this.labelX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxX
            // 
            this.textBoxX.Location = new System.Drawing.Point(17, 0);
            this.textBoxX.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.Size = new System.Drawing.Size(40, 20);
            this.textBoxX.TabIndex = 1;
            this.textBoxX.TextChanged += new System.EventHandler(this.textBoxX_TextChanged);
            // 
            // labelY
            // 
            this.labelY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelY.AutoSize = true;
            this.labelY.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.labelY.Location = new System.Drawing.Point(77, 3);
            this.labelY.Margin = new System.Windows.Forms.Padding(0);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(17, 13);
            this.labelY.TabIndex = 2;
            this.labelY.Text = "Y:";
            this.labelY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxY
            // 
            this.textBoxY.Location = new System.Drawing.Point(94, 0);
            this.textBoxY.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.Size = new System.Drawing.Size(40, 20);
            this.textBoxY.TabIndex = 3;
            this.textBoxY.TextChanged += new System.EventHandler(this.textBoxY_TextChanged);
            // 
            // labelZ
            // 
            this.labelZ.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelZ.AutoSize = true;
            this.labelZ.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.labelZ.Location = new System.Drawing.Point(154, 3);
            this.labelZ.Margin = new System.Windows.Forms.Padding(0);
            this.labelZ.Name = "labelZ";
            this.labelZ.Size = new System.Drawing.Size(17, 13);
            this.labelZ.TabIndex = 4;
            this.labelZ.Text = "Z:";
            this.labelZ.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxZ
            // 
            this.textBoxZ.Location = new System.Drawing.Point(171, 0);
            this.textBoxZ.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxZ.Name = "textBoxZ";
            this.textBoxZ.Size = new System.Drawing.Size(40, 20);
            this.textBoxZ.TabIndex = 5;
            this.textBoxZ.TextChanged += new System.EventHandler(this.textBoxZ_TextChanged);
            // 
            // EditorQuaternion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "EditorQuaternion";
            this.Size = new System.Drawing.Size(231, 20);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.TextBox textBoxX;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.TextBox textBoxY;
        private System.Windows.Forms.Label labelZ;
        private System.Windows.Forms.TextBox textBoxZ;
    }
}
