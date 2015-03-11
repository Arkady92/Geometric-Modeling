namespace Geometric_Modeling
{
    partial class MainWindow
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
            this.ObjectsList = new System.Windows.Forms.ListBox();
            this.ObjectsPanel = new System.Windows.Forms.Panel();
            this.EllipsoidButton = new System.Windows.Forms.Button();
            this.TorusButton = new System.Windows.Forms.Button();
            this.OptionsPanel = new System.Windows.Forms.Panel();
            this.PixelMaxSizeLabel = new System.Windows.Forms.Label();
            this.PixelMaxSizeBox = new System.Windows.Forms.TextBox();
            this.ZAxisFactorLabel = new System.Windows.Forms.Label();
            this.ZAxisFactorBox = new System.Windows.Forms.TextBox();
            this.YAxisFactorLabel = new System.Windows.Forms.Label();
            this.YAxisFactorBox = new System.Windows.Forms.TextBox();
            this.XAxisFactorLabel = new System.Windows.Forms.Label();
            this.XAxisFactorBox = new System.Windows.Forms.TextBox();
            this.IlluminanceLabel = new System.Windows.Forms.Label();
            this.IlluminanceBox = new System.Windows.Forms.TextBox();
            this.GridResolutionYLabel = new System.Windows.Forms.Label();
            this.GridResolutionYBox = new System.Windows.Forms.TextBox();
            this.GridResolutionXLabel = new System.Windows.Forms.Label();
            this.GridResolutionXBox = new System.Windows.Forms.TextBox();
            this.TranslationXButton = new System.Windows.Forms.Button();
            this.OperationsPanel = new System.Windows.Forms.Panel();
            this.ScaleButton = new System.Windows.Forms.Button();
            this.RotationZButton = new System.Windows.Forms.Button();
            this.RotationYButton = new System.Windows.Forms.Button();
            this.RotationXButton = new System.Windows.Forms.Button();
            this.TranslationZButton = new System.Windows.Forms.Button();
            this.TranslationYButton = new System.Windows.Forms.Button();
            this.ObjectsLabel = new System.Windows.Forms.Label();
            this.OperationsLabel = new System.Windows.Forms.Label();
            this.SettingsLabel = new System.Windows.Forms.Label();
            this.ObjectsListLabel = new System.Windows.Forms.Label();
            this.WorldPanel = new System.Windows.Forms.PictureBox();
            this.ObjectsPanel.SuspendLayout();
            this.OptionsPanel.SuspendLayout();
            this.OperationsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // ObjectsList
            // 
            this.ObjectsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ObjectsList.FormattingEnabled = true;
            this.ObjectsList.Location = new System.Drawing.Point(622, 364);
            this.ObjectsList.Name = "ObjectsList";
            this.ObjectsList.Size = new System.Drawing.Size(150, 186);
            this.ObjectsList.TabIndex = 0;
            this.ObjectsList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ObjectsList_MouseClick);
            this.ObjectsList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ObjectsList_MouseDoubleClick);
            // 
            // ObjectsPanel
            // 
            this.ObjectsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ObjectsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ObjectsPanel.Controls.Add(this.EllipsoidButton);
            this.ObjectsPanel.Controls.Add(this.TorusButton);
            this.ObjectsPanel.Location = new System.Drawing.Point(12, 12);
            this.ObjectsPanel.Name = "ObjectsPanel";
            this.ObjectsPanel.Size = new System.Drawing.Size(760, 31);
            this.ObjectsPanel.TabIndex = 1;
            // 
            // EllipsoidButton
            // 
            this.EllipsoidButton.Location = new System.Drawing.Point(84, 3);
            this.EllipsoidButton.Name = "EllipsoidButton";
            this.EllipsoidButton.Size = new System.Drawing.Size(75, 23);
            this.EllipsoidButton.TabIndex = 10;
            this.EllipsoidButton.Text = "Ellipsoid";
            this.EllipsoidButton.UseVisualStyleBackColor = true;
            this.EllipsoidButton.Click += new System.EventHandler(this.EllipsoidButton_Click);
            // 
            // TorusButton
            // 
            this.TorusButton.Location = new System.Drawing.Point(3, 3);
            this.TorusButton.Name = "TorusButton";
            this.TorusButton.Size = new System.Drawing.Size(75, 23);
            this.TorusButton.TabIndex = 2;
            this.TorusButton.Text = "Torus";
            this.TorusButton.UseVisualStyleBackColor = true;
            this.TorusButton.Click += new System.EventHandler(this.TorusButton_Click);
            // 
            // OptionsPanel
            // 
            this.OptionsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OptionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OptionsPanel.Controls.Add(this.PixelMaxSizeLabel);
            this.OptionsPanel.Controls.Add(this.PixelMaxSizeBox);
            this.OptionsPanel.Controls.Add(this.ZAxisFactorLabel);
            this.OptionsPanel.Controls.Add(this.ZAxisFactorBox);
            this.OptionsPanel.Controls.Add(this.YAxisFactorLabel);
            this.OptionsPanel.Controls.Add(this.YAxisFactorBox);
            this.OptionsPanel.Controls.Add(this.XAxisFactorLabel);
            this.OptionsPanel.Controls.Add(this.XAxisFactorBox);
            this.OptionsPanel.Controls.Add(this.IlluminanceLabel);
            this.OptionsPanel.Controls.Add(this.IlluminanceBox);
            this.OptionsPanel.Controls.Add(this.GridResolutionYLabel);
            this.OptionsPanel.Controls.Add(this.GridResolutionYBox);
            this.OptionsPanel.Controls.Add(this.GridResolutionXLabel);
            this.OptionsPanel.Controls.Add(this.GridResolutionXBox);
            this.OptionsPanel.Location = new System.Drawing.Point(622, 103);
            this.OptionsPanel.Name = "OptionsPanel";
            this.OptionsPanel.Size = new System.Drawing.Size(150, 246);
            this.OptionsPanel.TabIndex = 2;
            // 
            // PixelMaxSizeLabel
            // 
            this.PixelMaxSizeLabel.AutoSize = true;
            this.PixelMaxSizeLabel.Enabled = false;
            this.PixelMaxSizeLabel.Location = new System.Drawing.Point(7, 164);
            this.PixelMaxSizeLabel.Name = "PixelMaxSizeLabel";
            this.PixelMaxSizeLabel.Size = new System.Drawing.Size(75, 13);
            this.PixelMaxSizeLabel.TabIndex = 11;
            this.PixelMaxSizeLabel.Text = "Pixel Max Size";
            // 
            // PixelMaxSizeBox
            // 
            this.PixelMaxSizeBox.Enabled = false;
            this.PixelMaxSizeBox.Location = new System.Drawing.Point(99, 161);
            this.PixelMaxSizeBox.Name = "PixelMaxSizeBox";
            this.PixelMaxSizeBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PixelMaxSizeBox.Size = new System.Drawing.Size(46, 20);
            this.PixelMaxSizeBox.TabIndex = 10;
            this.PixelMaxSizeBox.Tag = "";
            this.PixelMaxSizeBox.TextChanged += new System.EventHandler(this.PixelMaxSizeBox_TextChanged);
            // 
            // ZAxisFactorLabel
            // 
            this.ZAxisFactorLabel.AutoSize = true;
            this.ZAxisFactorLabel.Enabled = false;
            this.ZAxisFactorLabel.Location = new System.Drawing.Point(7, 138);
            this.ZAxisFactorLabel.Name = "ZAxisFactorLabel";
            this.ZAxisFactorLabel.Size = new System.Drawing.Size(69, 13);
            this.ZAxisFactorLabel.TabIndex = 11;
            this.ZAxisFactorLabel.Text = "Z-Axis Factor";
            // 
            // ZAxisFactorBox
            // 
            this.ZAxisFactorBox.Enabled = false;
            this.ZAxisFactorBox.Location = new System.Drawing.Point(99, 135);
            this.ZAxisFactorBox.Name = "ZAxisFactorBox";
            this.ZAxisFactorBox.Size = new System.Drawing.Size(46, 20);
            this.ZAxisFactorBox.TabIndex = 10;
            this.ZAxisFactorBox.Tag = "";
            this.ZAxisFactorBox.TextChanged += new System.EventHandler(this.ZAxisFactorBox_TextChanged);
            // 
            // YAxisFactorLabel
            // 
            this.YAxisFactorLabel.AutoSize = true;
            this.YAxisFactorLabel.Enabled = false;
            this.YAxisFactorLabel.Location = new System.Drawing.Point(7, 112);
            this.YAxisFactorLabel.Name = "YAxisFactorLabel";
            this.YAxisFactorLabel.Size = new System.Drawing.Size(69, 13);
            this.YAxisFactorLabel.TabIndex = 9;
            this.YAxisFactorLabel.Text = "Y-Axis Factor";
            // 
            // YAxisFactorBox
            // 
            this.YAxisFactorBox.Enabled = false;
            this.YAxisFactorBox.Location = new System.Drawing.Point(99, 109);
            this.YAxisFactorBox.Name = "YAxisFactorBox";
            this.YAxisFactorBox.Size = new System.Drawing.Size(46, 20);
            this.YAxisFactorBox.TabIndex = 8;
            this.YAxisFactorBox.Tag = "";
            this.YAxisFactorBox.TextChanged += new System.EventHandler(this.YAxisFactorBox_TextChanged);
            // 
            // XAxisFactorLabel
            // 
            this.XAxisFactorLabel.AutoSize = true;
            this.XAxisFactorLabel.Enabled = false;
            this.XAxisFactorLabel.Location = new System.Drawing.Point(7, 86);
            this.XAxisFactorLabel.Name = "XAxisFactorLabel";
            this.XAxisFactorLabel.Size = new System.Drawing.Size(69, 13);
            this.XAxisFactorLabel.TabIndex = 7;
            this.XAxisFactorLabel.Text = "X-Axis Factor";
            // 
            // XAxisFactorBox
            // 
            this.XAxisFactorBox.Enabled = false;
            this.XAxisFactorBox.Location = new System.Drawing.Point(99, 83);
            this.XAxisFactorBox.Name = "XAxisFactorBox";
            this.XAxisFactorBox.Size = new System.Drawing.Size(46, 20);
            this.XAxisFactorBox.TabIndex = 6;
            this.XAxisFactorBox.Tag = "";
            this.XAxisFactorBox.TextChanged += new System.EventHandler(this.XAxisFactorBox_TextChanged);
            // 
            // IlluminanceLabel
            // 
            this.IlluminanceLabel.AutoSize = true;
            this.IlluminanceLabel.Enabled = false;
            this.IlluminanceLabel.Location = new System.Drawing.Point(7, 61);
            this.IlluminanceLabel.Name = "IlluminanceLabel";
            this.IlluminanceLabel.Size = new System.Drawing.Size(60, 13);
            this.IlluminanceLabel.TabIndex = 5;
            this.IlluminanceLabel.Text = "Illuminance";
            // 
            // IlluminanceBox
            // 
            this.IlluminanceBox.Enabled = false;
            this.IlluminanceBox.Location = new System.Drawing.Point(99, 58);
            this.IlluminanceBox.Name = "IlluminanceBox";
            this.IlluminanceBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.IlluminanceBox.Size = new System.Drawing.Size(46, 20);
            this.IlluminanceBox.TabIndex = 4;
            this.IlluminanceBox.Tag = "";
            this.IlluminanceBox.TextChanged += new System.EventHandler(this.IlluminanceBox_TextChanged);
            // 
            // GridResolutionYLabel
            // 
            this.GridResolutionYLabel.AutoSize = true;
            this.GridResolutionYLabel.Enabled = false;
            this.GridResolutionYLabel.Location = new System.Drawing.Point(7, 35);
            this.GridResolutionYLabel.Name = "GridResolutionYLabel";
            this.GridResolutionYLabel.Size = new System.Drawing.Size(89, 13);
            this.GridResolutionYLabel.TabIndex = 3;
            this.GridResolutionYLabel.Text = "Grid Resolution Y";
            // 
            // GridResolutionYBox
            // 
            this.GridResolutionYBox.Enabled = false;
            this.GridResolutionYBox.Location = new System.Drawing.Point(99, 32);
            this.GridResolutionYBox.Name = "GridResolutionYBox";
            this.GridResolutionYBox.Size = new System.Drawing.Size(46, 20);
            this.GridResolutionYBox.TabIndex = 2;
            this.GridResolutionYBox.Tag = "";
            this.GridResolutionYBox.TextChanged += new System.EventHandler(this.GridResolutionYBox_TextChanged);
            // 
            // GridResolutionXLabel
            // 
            this.GridResolutionXLabel.AutoSize = true;
            this.GridResolutionXLabel.Enabled = false;
            this.GridResolutionXLabel.Location = new System.Drawing.Point(7, 9);
            this.GridResolutionXLabel.Name = "GridResolutionXLabel";
            this.GridResolutionXLabel.Size = new System.Drawing.Size(89, 13);
            this.GridResolutionXLabel.TabIndex = 1;
            this.GridResolutionXLabel.Text = "Grid Resolution X";
            // 
            // GridResolutionXBox
            // 
            this.GridResolutionXBox.Enabled = false;
            this.GridResolutionXBox.Location = new System.Drawing.Point(99, 6);
            this.GridResolutionXBox.Name = "GridResolutionXBox";
            this.GridResolutionXBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.GridResolutionXBox.Size = new System.Drawing.Size(46, 20);
            this.GridResolutionXBox.TabIndex = 0;
            this.GridResolutionXBox.Tag = "";
            this.GridResolutionXBox.TextChanged += new System.EventHandler(this.GridResolutionXBox_TextChanged);
            // 
            // TranslationXButton
            // 
            this.TranslationXButton.Location = new System.Drawing.Point(3, 3);
            this.TranslationXButton.Name = "TranslationXButton";
            this.TranslationXButton.Size = new System.Drawing.Size(84, 23);
            this.TranslationXButton.TabIndex = 2;
            this.TranslationXButton.Text = "Translation X";
            this.TranslationXButton.UseVisualStyleBackColor = true;
            this.TranslationXButton.Click += new System.EventHandler(this.TranslationXButton_Click);
            // 
            // OperationsPanel
            // 
            this.OperationsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OperationsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OperationsPanel.Controls.Add(this.ScaleButton);
            this.OperationsPanel.Controls.Add(this.RotationZButton);
            this.OperationsPanel.Controls.Add(this.RotationYButton);
            this.OperationsPanel.Controls.Add(this.RotationXButton);
            this.OperationsPanel.Controls.Add(this.TranslationZButton);
            this.OperationsPanel.Controls.Add(this.TranslationYButton);
            this.OperationsPanel.Controls.Add(this.TranslationXButton);
            this.OperationsPanel.Location = new System.Drawing.Point(12, 57);
            this.OperationsPanel.Name = "OperationsPanel";
            this.OperationsPanel.Size = new System.Drawing.Size(760, 31);
            this.OperationsPanel.TabIndex = 4;
            // 
            // ScaleButton
            // 
            this.ScaleButton.Location = new System.Drawing.Point(543, 3);
            this.ScaleButton.Name = "ScaleButton";
            this.ScaleButton.Size = new System.Drawing.Size(75, 23);
            this.ScaleButton.TabIndex = 4;
            this.ScaleButton.Text = "Scale";
            this.ScaleButton.UseVisualStyleBackColor = true;
            this.ScaleButton.Click += new System.EventHandler(this.ScaleButton_Click);
            // 
            // RotationZButton
            // 
            this.RotationZButton.Location = new System.Drawing.Point(453, 3);
            this.RotationZButton.Name = "RotationZButton";
            this.RotationZButton.Size = new System.Drawing.Size(84, 23);
            this.RotationZButton.TabIndex = 6;
            this.RotationZButton.Text = "Rotation Z";
            this.RotationZButton.UseVisualStyleBackColor = true;
            this.RotationZButton.Click += new System.EventHandler(this.RotationZButton_Click);
            // 
            // RotationYButton
            // 
            this.RotationYButton.Location = new System.Drawing.Point(363, 3);
            this.RotationYButton.Name = "RotationYButton";
            this.RotationYButton.Size = new System.Drawing.Size(84, 23);
            this.RotationYButton.TabIndex = 5;
            this.RotationYButton.Text = "Rotation Y";
            this.RotationYButton.UseVisualStyleBackColor = true;
            this.RotationYButton.Click += new System.EventHandler(this.RotationYButton_Click);
            // 
            // RotationXButton
            // 
            this.RotationXButton.Location = new System.Drawing.Point(273, 3);
            this.RotationXButton.Name = "RotationXButton";
            this.RotationXButton.Size = new System.Drawing.Size(84, 23);
            this.RotationXButton.TabIndex = 3;
            this.RotationXButton.Text = "Rotation X";
            this.RotationXButton.UseVisualStyleBackColor = true;
            this.RotationXButton.Click += new System.EventHandler(this.RotationXButton_Click);
            // 
            // TranslationZButton
            // 
            this.TranslationZButton.Location = new System.Drawing.Point(183, 3);
            this.TranslationZButton.Name = "TranslationZButton";
            this.TranslationZButton.Size = new System.Drawing.Size(84, 23);
            this.TranslationZButton.TabIndex = 4;
            this.TranslationZButton.Text = "Translation Z";
            this.TranslationZButton.UseVisualStyleBackColor = true;
            this.TranslationZButton.Click += new System.EventHandler(this.TranslationZButton_Click);
            // 
            // TranslationYButton
            // 
            this.TranslationYButton.Location = new System.Drawing.Point(93, 3);
            this.TranslationYButton.Name = "TranslationYButton";
            this.TranslationYButton.Size = new System.Drawing.Size(84, 23);
            this.TranslationYButton.TabIndex = 3;
            this.TranslationYButton.Text = "Translation Y";
            this.TranslationYButton.UseVisualStyleBackColor = true;
            this.TranslationYButton.Click += new System.EventHandler(this.TranslationYButton_Click);
            // 
            // ObjectsLabel
            // 
            this.ObjectsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.ObjectsLabel.AutoSize = true;
            this.ObjectsLabel.Location = new System.Drawing.Point(376, 0);
            this.ObjectsLabel.Name = "ObjectsLabel";
            this.ObjectsLabel.Size = new System.Drawing.Size(43, 13);
            this.ObjectsLabel.TabIndex = 5;
            this.ObjectsLabel.Text = "Objects";
            // 
            // OperationsLabel
            // 
            this.OperationsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.OperationsLabel.AutoSize = true;
            this.OperationsLabel.Location = new System.Drawing.Point(369, 45);
            this.OperationsLabel.Name = "OperationsLabel";
            this.OperationsLabel.Size = new System.Drawing.Size(58, 13);
            this.OperationsLabel.TabIndex = 6;
            this.OperationsLabel.Text = "Operations";
            // 
            // SettingsLabel
            // 
            this.SettingsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsLabel.AutoSize = true;
            this.SettingsLabel.Location = new System.Drawing.Point(674, 91);
            this.SettingsLabel.Name = "SettingsLabel";
            this.SettingsLabel.Size = new System.Drawing.Size(45, 13);
            this.SettingsLabel.TabIndex = 7;
            this.SettingsLabel.Text = "Settings";
            // 
            // ObjectsListLabel
            // 
            this.ObjectsListLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ObjectsListLabel.AutoSize = true;
            this.ObjectsListLabel.Location = new System.Drawing.Point(664, 352);
            this.ObjectsListLabel.Name = "ObjectsListLabel";
            this.ObjectsListLabel.Size = new System.Drawing.Size(62, 13);
            this.ObjectsListLabel.TabIndex = 8;
            this.ObjectsListLabel.Text = "Objects List";
            this.ObjectsListLabel.Click += new System.EventHandler(this.ObjectsListLabel_Click);
            // 
            // WorldPanel
            // 
            this.WorldPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WorldPanel.BackColor = System.Drawing.Color.White;
            this.WorldPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.WorldPanel.Location = new System.Drawing.Point(12, 103);
            this.WorldPanel.Name = "WorldPanel";
            this.WorldPanel.Size = new System.Drawing.Size(594, 447);
            this.WorldPanel.TabIndex = 9;
            this.WorldPanel.TabStop = false;
            this.WorldPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WorldPanel_MouseDown);
            this.WorldPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WorldPanel_MouseMove);
            this.WorldPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.WorldPanel_MouseUp);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.WorldPanel);
            this.Controls.Add(this.ObjectsListLabel);
            this.Controls.Add(this.SettingsLabel);
            this.Controls.Add(this.OperationsLabel);
            this.Controls.Add(this.ObjectsLabel);
            this.Controls.Add(this.OperationsPanel);
            this.Controls.Add(this.OptionsPanel);
            this.Controls.Add(this.ObjectsPanel);
            this.Controls.Add(this.ObjectsList);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Geometric Modeling";
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.ObjectsPanel.ResumeLayout(false);
            this.OptionsPanel.ResumeLayout(false);
            this.OptionsPanel.PerformLayout();
            this.OperationsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.WorldPanel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ObjectsList;
        private System.Windows.Forms.Panel ObjectsPanel;
        private System.Windows.Forms.Button TorusButton;
        private System.Windows.Forms.Panel OptionsPanel;
        private System.Windows.Forms.Label GridResolutionXLabel;
        private System.Windows.Forms.TextBox GridResolutionXBox;
        private System.Windows.Forms.Button TranslationXButton;
        private System.Windows.Forms.Panel OperationsPanel;
        private System.Windows.Forms.Button ScaleButton;
        private System.Windows.Forms.Button RotationXButton;
        private System.Windows.Forms.Label ObjectsLabel;
        private System.Windows.Forms.Label OperationsLabel;
        private System.Windows.Forms.Label SettingsLabel;
        private System.Windows.Forms.Label ObjectsListLabel;
        private System.Windows.Forms.Label GridResolutionYLabel;
        private System.Windows.Forms.TextBox GridResolutionYBox;
        private System.Windows.Forms.Button TranslationYButton;
        private System.Windows.Forms.Button TranslationZButton;
        private System.Windows.Forms.Button RotationZButton;
        private System.Windows.Forms.Button RotationYButton;
        private System.Windows.Forms.PictureBox WorldPanel;
        private System.Windows.Forms.Button EllipsoidButton;
        private System.Windows.Forms.Label IlluminanceLabel;
        private System.Windows.Forms.TextBox IlluminanceBox;
        private System.Windows.Forms.Label ZAxisFactorLabel;
        private System.Windows.Forms.TextBox ZAxisFactorBox;
        private System.Windows.Forms.Label YAxisFactorLabel;
        private System.Windows.Forms.TextBox YAxisFactorBox;
        private System.Windows.Forms.Label XAxisFactorLabel;
        private System.Windows.Forms.TextBox XAxisFactorBox;
        private System.Windows.Forms.Label PixelMaxSizeLabel;
        private System.Windows.Forms.TextBox PixelMaxSizeBox;
    }
}

