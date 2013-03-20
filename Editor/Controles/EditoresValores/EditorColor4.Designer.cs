namespace Fang3D.Editor.Controles.EditoresValores
{
    partial class EditorColor4
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
            this.labelR = new System.Windows.Forms.Label();
            this.textBoxR = new System.Windows.Forms.TextBox();
            this.labelG = new System.Windows.Forms.Label();
            this.textBoxG = new System.Windows.Forms.TextBox();
            this.labelB = new System.Windows.Forms.Label();
            this.textBoxB = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.labelR);
            this.flowLayoutPanel1.Controls.Add(this.textBoxR);
            this.flowLayoutPanel1.Controls.Add(this.labelG);
            this.flowLayoutPanel1.Controls.Add(this.textBoxG);
            this.flowLayoutPanel1.Controls.Add(this.labelB);
            this.flowLayoutPanel1.Controls.Add(this.textBoxB);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(173, 20);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // labelR
            // 
            this.labelR.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelR.AutoSize = true;
            this.labelR.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.labelR.Location = new System.Drawing.Point(0, 3);
            this.labelR.Margin = new System.Windows.Forms.Padding(0);
            this.labelR.Name = "labelR";
            this.labelR.Size = new System.Drawing.Size(18, 13);
            this.labelR.TabIndex = 0;
            this.labelR.Text = "R:";
            this.labelR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxR
            // 
            this.textBoxR.Location = new System.Drawing.Point(18, 0);
            this.textBoxR.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxR.Name = "textBoxR";
            this.textBoxR.Size = new System.Drawing.Size(40, 20);
            this.textBoxR.TabIndex = 1;
            this.textBoxR.TextChanged += new System.EventHandler(this.textBoxX_TextChanged);
            // 
            // labelG
            // 
            this.labelG.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelG.AutoSize = true;
            this.labelG.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.labelG.Location = new System.Drawing.Point(58, 3);
            this.labelG.Margin = new System.Windows.Forms.Padding(0);
            this.labelG.Name = "labelG";
            this.labelG.Size = new System.Drawing.Size(18, 13);
            this.labelG.TabIndex = 2;
            this.labelG.Text = "G:";
            this.labelG.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxG
            // 
            this.textBoxG.Location = new System.Drawing.Point(76, 0);
            this.textBoxG.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxG.Name = "textBoxG";
            this.textBoxG.Size = new System.Drawing.Size(40, 20);
            this.textBoxG.TabIndex = 3;
            this.textBoxG.TextChanged += new System.EventHandler(this.textBoxY_TextChanged);
            // 
            // labelB
            // 
            this.labelB.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelB.AutoSize = true;
            this.labelB.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.labelB.Location = new System.Drawing.Point(116, 3);
            this.labelB.Margin = new System.Windows.Forms.Padding(0);
            this.labelB.Name = "labelB";
            this.labelB.Size = new System.Drawing.Size(17, 13);
            this.labelB.TabIndex = 4;
            this.labelB.Text = "B:";
            this.labelB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxB
            // 
            this.textBoxB.Location = new System.Drawing.Point(133, 0);
            this.textBoxB.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxB.Name = "textBoxB";
            this.textBoxB.Size = new System.Drawing.Size(40, 20);
            this.textBoxB.TabIndex = 5;
            this.textBoxB.TextChanged += new System.EventHandler(this.textBoxZ_TextChanged);
            // 
            // EditorVector3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "EditorVector3";
            this.Size = new System.Drawing.Size(173, 20);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label labelR;
        private System.Windows.Forms.TextBox textBoxR;
        private System.Windows.Forms.Label labelG;
        private System.Windows.Forms.TextBox textBoxG;
        private System.Windows.Forms.Label labelB;
        private System.Windows.Forms.TextBox textBoxB;
    }
}
