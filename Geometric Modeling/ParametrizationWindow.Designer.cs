namespace Geometric_Modeling
{
    partial class ParametrizationWindow
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.STParametrizationPictureBox = new System.Windows.Forms.PictureBox();
            this.UVParametrizationPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.STParametrizationPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UVParametrizationPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // STParametrizationPictureBox
            // 
            this.STParametrizationPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.STParametrizationPictureBox.BackColor = System.Drawing.Color.Black;
            this.STParametrizationPictureBox.Location = new System.Drawing.Point(12, 280);
            this.STParametrizationPictureBox.Name = "STParametrizationPictureBox";
            this.STParametrizationPictureBox.Size = new System.Drawing.Size(260, 260);
            this.STParametrizationPictureBox.TabIndex = 1;
            this.STParametrizationPictureBox.TabStop = false;
            // 
            // UVParametrizationPictureBox
            // 
            this.UVParametrizationPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UVParametrizationPictureBox.BackColor = System.Drawing.Color.Black;
            this.UVParametrizationPictureBox.Location = new System.Drawing.Point(12, 12);
            this.UVParametrizationPictureBox.Name = "UVParametrizationPictureBox";
            this.UVParametrizationPictureBox.Size = new System.Drawing.Size(260, 260);
            this.UVParametrizationPictureBox.TabIndex = 0;
            this.UVParametrizationPictureBox.TabStop = false;
            // 
            // ParametrizationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 552);
            this.Controls.Add(this.STParametrizationPictureBox);
            this.Controls.Add(this.UVParametrizationPictureBox);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 590);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 590);
            this.Name = "ParametrizationWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ParametrizationWindow";
            ((System.ComponentModel.ISupportInitialize)(this.STParametrizationPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UVParametrizationPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox STParametrizationPictureBox;
        private System.Windows.Forms.PictureBox UVParametrizationPictureBox;
    }
}