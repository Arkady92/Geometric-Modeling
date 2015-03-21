namespace Geometric_Modeling
{
    partial class RenameForm
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
            this.RenameText = new System.Windows.Forms.Label();
            this.RenameTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // RenameText
            // 
            this.RenameText.AutoSize = true;
            this.RenameText.Location = new System.Drawing.Point(57, 12);
            this.RenameText.Name = "RenameText";
            this.RenameText.Size = new System.Drawing.Size(167, 13);
            this.RenameText.TabIndex = 0;
            this.RenameText.Text = "Enter the new name of the object:";
            // 
            // RenameTextBox
            // 
            this.RenameTextBox.Location = new System.Drawing.Point(60, 28);
            this.RenameTextBox.Name = "RenameTextBox";
            this.RenameTextBox.Size = new System.Drawing.Size(160, 20);
            this.RenameTextBox.TabIndex = 1;
            this.RenameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RenameTextBox_KeyDown);
            // 
            // RenameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 60);
            this.Controls.Add(this.RenameTextBox);
            this.Controls.Add(this.RenameText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RenameForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RenameForm";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RenameForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label RenameText;
        private System.Windows.Forms.TextBox RenameTextBox;
    }
}