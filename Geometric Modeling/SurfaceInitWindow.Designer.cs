namespace Geometric_Modeling
{
    partial class SurfaceInitWindow
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
            this.SurfaceTypeLabel = new System.Windows.Forms.Label();
            this.FlatPatchRadioButton = new System.Windows.Forms.RadioButton();
            this.CylindricalPatchRadioButton = new System.Windows.Forms.RadioButton();
            this.SurfaceDimentionsLabel = new System.Windows.Forms.Label();
            this.WidthLabel = new System.Windows.Forms.Label();
            this.WidthTextBox = new System.Windows.Forms.TextBox();
            this.HeightTextBox = new System.Windows.Forms.TextBox();
            this.HeightLabel = new System.Windows.Forms.Label();
            this.GenerateButton = new System.Windows.Forms.Button();
            this.BreadthTextBox = new System.Windows.Forms.TextBox();
            this.BreadthLabel = new System.Windows.Forms.Label();
            this.LengthTextBox = new System.Windows.Forms.TextBox();
            this.LengthLabel = new System.Windows.Forms.Label();
            this.SurfacePatchCountLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SurfaceTypeLabel
            // 
            this.SurfaceTypeLabel.AutoSize = true;
            this.SurfaceTypeLabel.Location = new System.Drawing.Point(12, 9);
            this.SurfaceTypeLabel.Name = "SurfaceTypeLabel";
            this.SurfaceTypeLabel.Size = new System.Drawing.Size(106, 13);
            this.SurfaceTypeLabel.TabIndex = 999;
            this.SurfaceTypeLabel.Text = "Specify surface type:";
            // 
            // FlatPatchRadioButton
            // 
            this.FlatPatchRadioButton.AutoSize = true;
            this.FlatPatchRadioButton.Checked = true;
            this.FlatPatchRadioButton.Location = new System.Drawing.Point(15, 26);
            this.FlatPatchRadioButton.Name = "FlatPatchRadioButton";
            this.FlatPatchRadioButton.Size = new System.Drawing.Size(42, 17);
            this.FlatPatchRadioButton.TabIndex = 1;
            this.FlatPatchRadioButton.TabStop = true;
            this.FlatPatchRadioButton.Text = "Flat";
            this.FlatPatchRadioButton.UseVisualStyleBackColor = true;
            this.FlatPatchRadioButton.CheckedChanged += new System.EventHandler(this.FlatPatchRadioButton_CheckedChanged);
            // 
            // CylindricalPatchRadioButton
            // 
            this.CylindricalPatchRadioButton.AutoSize = true;
            this.CylindricalPatchRadioButton.Location = new System.Drawing.Point(15, 49);
            this.CylindricalPatchRadioButton.Name = "CylindricalPatchRadioButton";
            this.CylindricalPatchRadioButton.Size = new System.Drawing.Size(72, 17);
            this.CylindricalPatchRadioButton.TabIndex = 2;
            this.CylindricalPatchRadioButton.Text = "Cylindrical";
            this.CylindricalPatchRadioButton.UseVisualStyleBackColor = true;
            this.CylindricalPatchRadioButton.CheckedChanged += new System.EventHandler(this.CylindricalPatchRadioButton_CheckedChanged);
            // 
            // SurfaceDimentionsLabel
            // 
            this.SurfaceDimentionsLabel.AutoSize = true;
            this.SurfaceDimentionsLabel.Location = new System.Drawing.Point(12, 150);
            this.SurfaceDimentionsLabel.Name = "SurfaceDimentionsLabel";
            this.SurfaceDimentionsLabel.Size = new System.Drawing.Size(136, 13);
            this.SurfaceDimentionsLabel.TabIndex = 999;
            this.SurfaceDimentionsLabel.Text = "Specify surface dimentions:";
            // 
            // WidthLabel
            // 
            this.WidthLabel.AutoSize = true;
            this.WidthLabel.Location = new System.Drawing.Point(12, 169);
            this.WidthLabel.Name = "WidthLabel";
            this.WidthLabel.Size = new System.Drawing.Size(35, 13);
            this.WidthLabel.TabIndex = 4;
            this.WidthLabel.Text = "Width";
            // 
            // WidthTextBox
            // 
            this.WidthTextBox.Location = new System.Drawing.Point(56, 166);
            this.WidthTextBox.Name = "WidthTextBox";
            this.WidthTextBox.Size = new System.Drawing.Size(50, 20);
            this.WidthTextBox.TabIndex = 5;
            this.WidthTextBox.TextChanged += new System.EventHandler(this.WidthTextBox_TextChanged);
            // 
            // HeightTextBox
            // 
            this.HeightTextBox.Location = new System.Drawing.Point(56, 190);
            this.HeightTextBox.Name = "HeightTextBox";
            this.HeightTextBox.Size = new System.Drawing.Size(50, 20);
            this.HeightTextBox.TabIndex = 6;
            this.HeightTextBox.TextChanged += new System.EventHandler(this.HeightTextBox_TextChanged);
            // 
            // HeightLabel
            // 
            this.HeightLabel.AutoSize = true;
            this.HeightLabel.Location = new System.Drawing.Point(12, 193);
            this.HeightLabel.Name = "HeightLabel";
            this.HeightLabel.Size = new System.Drawing.Size(38, 13);
            this.HeightLabel.TabIndex = 6;
            this.HeightLabel.Text = "Height";
            // 
            // GenerateButton
            // 
            this.GenerateButton.Location = new System.Drawing.Point(26, 224);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(102, 23);
            this.GenerateButton.TabIndex = 7;
            this.GenerateButton.Text = "Generate surface";
            this.GenerateButton.UseVisualStyleBackColor = true;
            this.GenerateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // BreadthTextBox
            // 
            this.BreadthTextBox.Location = new System.Drawing.Point(62, 120);
            this.BreadthTextBox.Name = "BreadthTextBox";
            this.BreadthTextBox.Size = new System.Drawing.Size(50, 20);
            this.BreadthTextBox.TabIndex = 4;
            this.BreadthTextBox.TextChanged += new System.EventHandler(this.BreadthTextBox_TextChanged);
            // 
            // BreadthLabel
            // 
            this.BreadthLabel.AutoSize = true;
            this.BreadthLabel.Location = new System.Drawing.Point(12, 121);
            this.BreadthLabel.Name = "BreadthLabel";
            this.BreadthLabel.Size = new System.Drawing.Size(44, 13);
            this.BreadthLabel.TabIndex = 12;
            this.BreadthLabel.Text = "Breadth";
            // 
            // LengthTextBox
            // 
            this.LengthTextBox.Location = new System.Drawing.Point(62, 94);
            this.LengthTextBox.Name = "LengthTextBox";
            this.LengthTextBox.Size = new System.Drawing.Size(50, 20);
            this.LengthTextBox.TabIndex = 3;
            this.LengthTextBox.TextChanged += new System.EventHandler(this.LengthTextBox_TextChanged);
            // 
            // LengthLabel
            // 
            this.LengthLabel.AutoSize = true;
            this.LengthLabel.Location = new System.Drawing.Point(12, 97);
            this.LengthLabel.Name = "LengthLabel";
            this.LengthLabel.Size = new System.Drawing.Size(40, 13);
            this.LengthLabel.TabIndex = 10;
            this.LengthLabel.Text = "Length";
            // 
            // SurfacePatchCountLabel
            // 
            this.SurfacePatchCountLabel.AutoSize = true;
            this.SurfacePatchCountLabel.Location = new System.Drawing.Point(12, 78);
            this.SurfacePatchCountLabel.Name = "SurfacePatchCountLabel";
            this.SurfacePatchCountLabel.Size = new System.Drawing.Size(116, 13);
            this.SurfacePatchCountLabel.TabIndex = 999;
            this.SurfacePatchCountLabel.Text = "Specify patches count:";
            // 
            // SurfaceInitWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(158, 259);
            this.Controls.Add(this.BreadthTextBox);
            this.Controls.Add(this.BreadthLabel);
            this.Controls.Add(this.LengthTextBox);
            this.Controls.Add(this.LengthLabel);
            this.Controls.Add(this.SurfacePatchCountLabel);
            this.Controls.Add(this.GenerateButton);
            this.Controls.Add(this.HeightTextBox);
            this.Controls.Add(this.HeightLabel);
            this.Controls.Add(this.WidthTextBox);
            this.Controls.Add(this.WidthLabel);
            this.Controls.Add(this.SurfaceDimentionsLabel);
            this.Controls.Add(this.CylindricalPatchRadioButton);
            this.Controls.Add(this.FlatPatchRadioButton);
            this.Controls.Add(this.SurfaceTypeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SurfaceInitWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SurfaceInitWindow";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SurfaceTypeLabel;
        private System.Windows.Forms.RadioButton FlatPatchRadioButton;
        private System.Windows.Forms.RadioButton CylindricalPatchRadioButton;
        private System.Windows.Forms.Label SurfaceDimentionsLabel;
        private System.Windows.Forms.Label WidthLabel;
        private System.Windows.Forms.TextBox WidthTextBox;
        private System.Windows.Forms.TextBox HeightTextBox;
        private System.Windows.Forms.Label HeightLabel;
        private System.Windows.Forms.Button GenerateButton;
        private System.Windows.Forms.TextBox BreadthTextBox;
        private System.Windows.Forms.Label BreadthLabel;
        private System.Windows.Forms.TextBox LengthTextBox;
        private System.Windows.Forms.Label LengthLabel;
        private System.Windows.Forms.Label SurfacePatchCountLabel;
    }
}