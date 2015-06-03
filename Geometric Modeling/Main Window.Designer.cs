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
            this.GregorySurfaceButton = new System.Windows.Forms.Button();
            this.IntersectionButton = new System.Windows.Forms.Button();
            this.BezierSurfaceC2Button = new System.Windows.Forms.Button();
            this.BezierSurfaceC0Button = new System.Windows.Forms.Button();
            this.InterpolationCurveButton = new System.Windows.Forms.Button();
            this.BezierCurveC2Button = new System.Windows.Forms.Button();
            this.BezierCurveButton = new System.Windows.Forms.Button();
            this.PointButton = new System.Windows.Forms.Button();
            this.EllipsoidButton = new System.Windows.Forms.Button();
            this.TorusButton = new System.Windows.Forms.Button();
            this.OptionsPanel = new System.Windows.Forms.Panel();
            this.SurfaceGridResolutionYBox = new System.Windows.Forms.TextBox();
            this.SurfaceGridResolutionXBox = new System.Windows.Forms.TextBox();
            this.ChordParametrizationRadioButton = new System.Windows.Forms.RadioButton();
            this.PixelMaxSizeLabel = new System.Windows.Forms.Label();
            this.NormalParametrizationRadioButton = new System.Windows.Forms.RadioButton();
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
            this.CollapseButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.LoadButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.SelectionButton = new System.Windows.Forms.Button();
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
            this.EffectsLabel = new System.Windows.Forms.Label();
            this.EffectsPanel = new System.Windows.Forms.Panel();
            this.PolygonalChainCheckBox = new System.Windows.Forms.CheckBox();
            this.DeBoorsPointsRadioButton = new System.Windows.Forms.RadioButton();
            this.ControlPointsRadioButton = new System.Windows.Forms.RadioButton();
            this.AdditiveColorBlendingCheckBox = new System.Windows.Forms.CheckBox();
            this.StereoscopyCheckBox = new System.Windows.Forms.CheckBox();
            this.CursorXLabel = new System.Windows.Forms.Label();
            this.CursorXBox = new System.Windows.Forms.TextBox();
            this.CursorYLabel = new System.Windows.Forms.Label();
            this.CursorYBox = new System.Windows.Forms.TextBox();
            this.CursorZLabel = new System.Windows.Forms.Label();
            this.CursorZBox = new System.Windows.Forms.TextBox();
            this.CursorScreenXLabel = new System.Windows.Forms.Label();
            this.CursorScreenXBox = new System.Windows.Forms.TextBox();
            this.CursorScreenYLabel = new System.Windows.Forms.Label();
            this.CursorScreenYBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ObjectsPanel.SuspendLayout();
            this.OptionsPanel.SuspendLayout();
            this.OperationsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorldPanel)).BeginInit();
            this.EffectsPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ObjectsList
            // 
            this.ObjectsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ObjectsList.FormattingEnabled = true;
            this.ObjectsList.Location = new System.Drawing.Point(702, 424);
            this.ObjectsList.Name = "ObjectsList";
            this.ObjectsList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ObjectsList.Size = new System.Drawing.Size(150, 186);
            this.ObjectsList.TabIndex = 0;
            this.ObjectsList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ObjectsList_MouseClick);
            this.ObjectsList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ObjectsList_KeyDown);
            this.ObjectsList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ObjectsList_MouseDoubleClick);
            // 
            // ObjectsPanel
            // 
            this.ObjectsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ObjectsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ObjectsPanel.Controls.Add(this.GregorySurfaceButton);
            this.ObjectsPanel.Controls.Add(this.IntersectionButton);
            this.ObjectsPanel.Controls.Add(this.BezierSurfaceC2Button);
            this.ObjectsPanel.Controls.Add(this.BezierSurfaceC0Button);
            this.ObjectsPanel.Controls.Add(this.InterpolationCurveButton);
            this.ObjectsPanel.Controls.Add(this.BezierCurveC2Button);
            this.ObjectsPanel.Controls.Add(this.BezierCurveButton);
            this.ObjectsPanel.Controls.Add(this.PointButton);
            this.ObjectsPanel.Controls.Add(this.EllipsoidButton);
            this.ObjectsPanel.Controls.Add(this.TorusButton);
            this.ObjectsPanel.Location = new System.Drawing.Point(12, 12);
            this.ObjectsPanel.Name = "ObjectsPanel";
            this.ObjectsPanel.Size = new System.Drawing.Size(840, 31);
            this.ObjectsPanel.TabIndex = 1;
            // 
            // GregorySurfaceButton
            // 
            this.GregorySurfaceButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.GregorySurfaceButton.Location = new System.Drawing.Point(741, 3);
            this.GregorySurfaceButton.Name = "GregorySurfaceButton";
            this.GregorySurfaceButton.Size = new System.Drawing.Size(94, 23);
            this.GregorySurfaceButton.TabIndex = 11;
            this.GregorySurfaceButton.Text = "Gregory Surface";
            this.GregorySurfaceButton.UseVisualStyleBackColor = true;
            this.GregorySurfaceButton.Click += new System.EventHandler(this.GregorySurfaceButton_Click);
            // 
            // IntersectionButton
            // 
            this.IntersectionButton.Location = new System.Drawing.Point(495, 3);
            this.IntersectionButton.Name = "IntersectionButton";
            this.IntersectionButton.Size = new System.Drawing.Size(76, 23);
            this.IntersectionButton.TabIndex = 8;
            this.IntersectionButton.Text = "Intersection";
            this.IntersectionButton.UseVisualStyleBackColor = true;
            this.IntersectionButton.Click += new System.EventHandler(this.IntersectionButton_Click);
            // 
            // BezierSurfaceC2Button
            // 
            this.BezierSurfaceC2Button.Location = new System.Drawing.Point(659, 3);
            this.BezierSurfaceC2Button.Name = "BezierSurfaceC2Button";
            this.BezierSurfaceC2Button.Size = new System.Drawing.Size(76, 23);
            this.BezierSurfaceC2Button.TabIndex = 10;
            this.BezierSurfaceC2Button.Text = "Surface C2";
            this.BezierSurfaceC2Button.UseVisualStyleBackColor = true;
            this.BezierSurfaceC2Button.Click += new System.EventHandler(this.BezierSurfaceC2Button_Click);
            // 
            // BezierSurfaceC0Button
            // 
            this.BezierSurfaceC0Button.Location = new System.Drawing.Point(577, 3);
            this.BezierSurfaceC0Button.Name = "BezierSurfaceC0Button";
            this.BezierSurfaceC0Button.Size = new System.Drawing.Size(76, 23);
            this.BezierSurfaceC0Button.TabIndex = 9;
            this.BezierSurfaceC0Button.Text = "Surface C0";
            this.BezierSurfaceC0Button.UseVisualStyleBackColor = true;
            this.BezierSurfaceC0Button.Click += new System.EventHandler(this.BezierSurfaceC0Button_Click);
            // 
            // InterpolationCurveButton
            // 
            this.InterpolationCurveButton.Location = new System.Drawing.Point(413, 3);
            this.InterpolationCurveButton.Name = "InterpolationCurveButton";
            this.InterpolationCurveButton.Size = new System.Drawing.Size(76, 23);
            this.InterpolationCurveButton.TabIndex = 7;
            this.InterpolationCurveButton.Text = "Interpolation";
            this.InterpolationCurveButton.UseVisualStyleBackColor = true;
            this.InterpolationCurveButton.Click += new System.EventHandler(this.InterpolationCurveButton_Click);
            // 
            // BezierCurveC2Button
            // 
            this.BezierCurveC2Button.Location = new System.Drawing.Point(331, 3);
            this.BezierCurveC2Button.Name = "BezierCurveC2Button";
            this.BezierCurveC2Button.Size = new System.Drawing.Size(76, 23);
            this.BezierCurveC2Button.TabIndex = 6;
            this.BezierCurveC2Button.Text = "Curve C2";
            this.BezierCurveC2Button.UseVisualStyleBackColor = true;
            this.BezierCurveC2Button.Click += new System.EventHandler(this.BezierCurveC2Button_Click);
            // 
            // BezierCurveButton
            // 
            this.BezierCurveButton.Location = new System.Drawing.Point(249, 3);
            this.BezierCurveButton.Name = "BezierCurveButton";
            this.BezierCurveButton.Size = new System.Drawing.Size(76, 23);
            this.BezierCurveButton.TabIndex = 5;
            this.BezierCurveButton.Text = "Curve C0";
            this.BezierCurveButton.UseVisualStyleBackColor = true;
            this.BezierCurveButton.Click += new System.EventHandler(this.BezierCurveButton_Click);
            // 
            // PointButton
            // 
            this.PointButton.Location = new System.Drawing.Point(167, 3);
            this.PointButton.Name = "PointButton";
            this.PointButton.Size = new System.Drawing.Size(76, 23);
            this.PointButton.TabIndex = 4;
            this.PointButton.Text = "Point";
            this.PointButton.UseVisualStyleBackColor = true;
            this.PointButton.Click += new System.EventHandler(this.PointButton_Click);
            // 
            // EllipsoidButton
            // 
            this.EllipsoidButton.Location = new System.Drawing.Point(85, 3);
            this.EllipsoidButton.Name = "EllipsoidButton";
            this.EllipsoidButton.Size = new System.Drawing.Size(76, 23);
            this.EllipsoidButton.TabIndex = 3;
            this.EllipsoidButton.Text = "Ellipsoid";
            this.EllipsoidButton.UseVisualStyleBackColor = true;
            this.EllipsoidButton.Click += new System.EventHandler(this.EllipsoidButton_Click);
            // 
            // TorusButton
            // 
            this.TorusButton.Location = new System.Drawing.Point(3, 3);
            this.TorusButton.Name = "TorusButton";
            this.TorusButton.Size = new System.Drawing.Size(76, 23);
            this.TorusButton.TabIndex = 2;
            this.TorusButton.Text = "Torus";
            this.TorusButton.UseVisualStyleBackColor = true;
            this.TorusButton.Click += new System.EventHandler(this.TorusButton_Click);
            // 
            // OptionsPanel
            // 
            this.OptionsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OptionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OptionsPanel.Controls.Add(this.SurfaceGridResolutionYBox);
            this.OptionsPanel.Controls.Add(this.SurfaceGridResolutionXBox);
            this.OptionsPanel.Controls.Add(this.ChordParametrizationRadioButton);
            this.OptionsPanel.Controls.Add(this.PixelMaxSizeLabel);
            this.OptionsPanel.Controls.Add(this.NormalParametrizationRadioButton);
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
            this.OptionsPanel.Location = new System.Drawing.Point(702, 103);
            this.OptionsPanel.Name = "OptionsPanel";
            this.OptionsPanel.Size = new System.Drawing.Size(150, 139);
            this.OptionsPanel.TabIndex = 2;
            // 
            // SurfaceGridResolutionYBox
            // 
            this.SurfaceGridResolutionYBox.Location = new System.Drawing.Point(97, 33);
            this.SurfaceGridResolutionYBox.Name = "SurfaceGridResolutionYBox";
            this.SurfaceGridResolutionYBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SurfaceGridResolutionYBox.Size = new System.Drawing.Size(46, 20);
            this.SurfaceGridResolutionYBox.TabIndex = 13;
            this.SurfaceGridResolutionYBox.Tag = "";
            this.SurfaceGridResolutionYBox.TextChanged += new System.EventHandler(this.SurfaceGridResolutionYBox_TextChanged);
            // 
            // SurfaceGridResolutionXBox
            // 
            this.SurfaceGridResolutionXBox.Location = new System.Drawing.Point(99, 6);
            this.SurfaceGridResolutionXBox.Name = "SurfaceGridResolutionXBox";
            this.SurfaceGridResolutionXBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SurfaceGridResolutionXBox.Size = new System.Drawing.Size(46, 20);
            this.SurfaceGridResolutionXBox.TabIndex = 12;
            this.SurfaceGridResolutionXBox.Tag = "";
            this.SurfaceGridResolutionXBox.TextChanged += new System.EventHandler(this.SurfaceGridResolutionXBox_TextChanged);
            // 
            // ChordParametrizationRadioButton
            // 
            this.ChordParametrizationRadioButton.AutoSize = true;
            this.ChordParametrizationRadioButton.Checked = true;
            this.ChordParametrizationRadioButton.Location = new System.Drawing.Point(10, 32);
            this.ChordParametrizationRadioButton.Name = "ChordParametrizationRadioButton";
            this.ChordParametrizationRadioButton.Size = new System.Drawing.Size(128, 17);
            this.ChordParametrizationRadioButton.TabIndex = 6;
            this.ChordParametrizationRadioButton.TabStop = true;
            this.ChordParametrizationRadioButton.Text = "Chord Parametrization";
            this.ChordParametrizationRadioButton.UseVisualStyleBackColor = true;
            this.ChordParametrizationRadioButton.Visible = false;
            this.ChordParametrizationRadioButton.CheckedChanged += new System.EventHandler(this.ChordParametrizationRadioButton_CheckedChanged);
            // 
            // PixelMaxSizeLabel
            // 
            this.PixelMaxSizeLabel.AutoSize = true;
            this.PixelMaxSizeLabel.Location = new System.Drawing.Point(7, 114);
            this.PixelMaxSizeLabel.Name = "PixelMaxSizeLabel";
            this.PixelMaxSizeLabel.Size = new System.Drawing.Size(75, 13);
            this.PixelMaxSizeLabel.TabIndex = 11;
            this.PixelMaxSizeLabel.Text = "Pixel Max Size";
            this.PixelMaxSizeLabel.Visible = false;
            // 
            // NormalParametrizationRadioButton
            // 
            this.NormalParametrizationRadioButton.AutoSize = true;
            this.NormalParametrizationRadioButton.Location = new System.Drawing.Point(10, 9);
            this.NormalParametrizationRadioButton.Name = "NormalParametrizationRadioButton";
            this.NormalParametrizationRadioButton.Size = new System.Drawing.Size(133, 17);
            this.NormalParametrizationRadioButton.TabIndex = 5;
            this.NormalParametrizationRadioButton.Text = "Normal Parametrization";
            this.NormalParametrizationRadioButton.UseVisualStyleBackColor = true;
            this.NormalParametrizationRadioButton.Visible = false;
            this.NormalParametrizationRadioButton.CheckedChanged += new System.EventHandler(this.NormalParametrizationRadioButton_CheckedChanged);
            // 
            // PixelMaxSizeBox
            // 
            this.PixelMaxSizeBox.Location = new System.Drawing.Point(99, 111);
            this.PixelMaxSizeBox.Name = "PixelMaxSizeBox";
            this.PixelMaxSizeBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PixelMaxSizeBox.Size = new System.Drawing.Size(46, 20);
            this.PixelMaxSizeBox.TabIndex = 10;
            this.PixelMaxSizeBox.Tag = "";
            this.PixelMaxSizeBox.Visible = false;
            this.PixelMaxSizeBox.TextChanged += new System.EventHandler(this.PixelMaxSizeBox_TextChanged);
            // 
            // ZAxisFactorLabel
            // 
            this.ZAxisFactorLabel.AutoSize = true;
            this.ZAxisFactorLabel.Location = new System.Drawing.Point(7, 88);
            this.ZAxisFactorLabel.Name = "ZAxisFactorLabel";
            this.ZAxisFactorLabel.Size = new System.Drawing.Size(69, 13);
            this.ZAxisFactorLabel.TabIndex = 11;
            this.ZAxisFactorLabel.Text = "Z-Axis Factor";
            this.ZAxisFactorLabel.Visible = false;
            // 
            // ZAxisFactorBox
            // 
            this.ZAxisFactorBox.Location = new System.Drawing.Point(99, 85);
            this.ZAxisFactorBox.Name = "ZAxisFactorBox";
            this.ZAxisFactorBox.Size = new System.Drawing.Size(46, 20);
            this.ZAxisFactorBox.TabIndex = 10;
            this.ZAxisFactorBox.Tag = "";
            this.ZAxisFactorBox.Visible = false;
            this.ZAxisFactorBox.TextChanged += new System.EventHandler(this.ZAxisFactorBox_TextChanged);
            // 
            // YAxisFactorLabel
            // 
            this.YAxisFactorLabel.AutoSize = true;
            this.YAxisFactorLabel.Location = new System.Drawing.Point(7, 62);
            this.YAxisFactorLabel.Name = "YAxisFactorLabel";
            this.YAxisFactorLabel.Size = new System.Drawing.Size(69, 13);
            this.YAxisFactorLabel.TabIndex = 9;
            this.YAxisFactorLabel.Text = "Y-Axis Factor";
            this.YAxisFactorLabel.Visible = false;
            // 
            // YAxisFactorBox
            // 
            this.YAxisFactorBox.Location = new System.Drawing.Point(99, 59);
            this.YAxisFactorBox.Name = "YAxisFactorBox";
            this.YAxisFactorBox.Size = new System.Drawing.Size(46, 20);
            this.YAxisFactorBox.TabIndex = 8;
            this.YAxisFactorBox.Tag = "";
            this.YAxisFactorBox.Visible = false;
            this.YAxisFactorBox.TextChanged += new System.EventHandler(this.YAxisFactorBox_TextChanged);
            // 
            // XAxisFactorLabel
            // 
            this.XAxisFactorLabel.AutoSize = true;
            this.XAxisFactorLabel.Location = new System.Drawing.Point(7, 35);
            this.XAxisFactorLabel.Name = "XAxisFactorLabel";
            this.XAxisFactorLabel.Size = new System.Drawing.Size(69, 13);
            this.XAxisFactorLabel.TabIndex = 7;
            this.XAxisFactorLabel.Text = "X-Axis Factor";
            this.XAxisFactorLabel.Visible = false;
            // 
            // XAxisFactorBox
            // 
            this.XAxisFactorBox.Location = new System.Drawing.Point(99, 32);
            this.XAxisFactorBox.Name = "XAxisFactorBox";
            this.XAxisFactorBox.Size = new System.Drawing.Size(46, 20);
            this.XAxisFactorBox.TabIndex = 6;
            this.XAxisFactorBox.Tag = "";
            this.XAxisFactorBox.Visible = false;
            this.XAxisFactorBox.TextChanged += new System.EventHandler(this.XAxisFactorBox_TextChanged);
            // 
            // IlluminanceLabel
            // 
            this.IlluminanceLabel.AutoSize = true;
            this.IlluminanceLabel.Location = new System.Drawing.Point(7, 9);
            this.IlluminanceLabel.Name = "IlluminanceLabel";
            this.IlluminanceLabel.Size = new System.Drawing.Size(60, 13);
            this.IlluminanceLabel.TabIndex = 5;
            this.IlluminanceLabel.Text = "Illuminance";
            this.IlluminanceLabel.Visible = false;
            // 
            // IlluminanceBox
            // 
            this.IlluminanceBox.Location = new System.Drawing.Point(99, 6);
            this.IlluminanceBox.Name = "IlluminanceBox";
            this.IlluminanceBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.IlluminanceBox.Size = new System.Drawing.Size(46, 20);
            this.IlluminanceBox.TabIndex = 4;
            this.IlluminanceBox.Tag = "";
            this.IlluminanceBox.Visible = false;
            this.IlluminanceBox.TextChanged += new System.EventHandler(this.IlluminanceBox_TextChanged);
            // 
            // GridResolutionYLabel
            // 
            this.GridResolutionYLabel.AutoSize = true;
            this.GridResolutionYLabel.Location = new System.Drawing.Point(7, 35);
            this.GridResolutionYLabel.Name = "GridResolutionYLabel";
            this.GridResolutionYLabel.Size = new System.Drawing.Size(89, 13);
            this.GridResolutionYLabel.TabIndex = 3;
            this.GridResolutionYLabel.Text = "Grid Resolution Y";
            this.GridResolutionYLabel.Visible = false;
            // 
            // GridResolutionYBox
            // 
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
            this.GridResolutionXLabel.Location = new System.Drawing.Point(7, 9);
            this.GridResolutionXLabel.Name = "GridResolutionXLabel";
            this.GridResolutionXLabel.Size = new System.Drawing.Size(89, 13);
            this.GridResolutionXLabel.TabIndex = 1;
            this.GridResolutionXLabel.Text = "Grid Resolution X";
            this.GridResolutionXLabel.Visible = false;
            // 
            // GridResolutionXBox
            // 
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
            this.TranslationXButton.Size = new System.Drawing.Size(50, 23);
            this.TranslationXButton.TabIndex = 2;
            this.TranslationXButton.Text = "T(X)";
            this.TranslationXButton.UseVisualStyleBackColor = true;
            this.TranslationXButton.Click += new System.EventHandler(this.TranslationXButton_Click);
            // 
            // OperationsPanel
            // 
            this.OperationsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OperationsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OperationsPanel.Controls.Add(this.CollapseButton);
            this.OperationsPanel.Controls.Add(this.SaveButton);
            this.OperationsPanel.Controls.Add(this.LoadButton);
            this.OperationsPanel.Controls.Add(this.ClearButton);
            this.OperationsPanel.Controls.Add(this.SelectionButton);
            this.OperationsPanel.Controls.Add(this.ScaleButton);
            this.OperationsPanel.Controls.Add(this.RotationZButton);
            this.OperationsPanel.Controls.Add(this.RotationYButton);
            this.OperationsPanel.Controls.Add(this.RotationXButton);
            this.OperationsPanel.Controls.Add(this.TranslationZButton);
            this.OperationsPanel.Controls.Add(this.TranslationYButton);
            this.OperationsPanel.Controls.Add(this.TranslationXButton);
            this.OperationsPanel.Location = new System.Drawing.Point(12, 57);
            this.OperationsPanel.Name = "OperationsPanel";
            this.OperationsPanel.Size = new System.Drawing.Size(840, 31);
            this.OperationsPanel.TabIndex = 4;
            // 
            // CollapseButton
            // 
            this.CollapseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CollapseButton.Location = new System.Drawing.Point(534, 3);
            this.CollapseButton.Name = "CollapseButton";
            this.CollapseButton.Size = new System.Drawing.Size(55, 23);
            this.CollapseButton.TabIndex = 9;
            this.CollapseButton.Text = "Collapse";
            this.CollapseButton.UseVisualStyleBackColor = true;
            this.CollapseButton.Click += new System.EventHandler(this.CollapseButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Location = new System.Drawing.Point(656, 3);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(55, 23);
            this.SaveButton.TabIndex = 11;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadButton.Location = new System.Drawing.Point(717, 3);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(55, 23);
            this.LoadButton.TabIndex = 12;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearButton.Location = new System.Drawing.Point(778, 3);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(55, 23);
            this.ClearButton.TabIndex = 13;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // SelectionButton
            // 
            this.SelectionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectionButton.Location = new System.Drawing.Point(595, 3);
            this.SelectionButton.Name = "SelectionButton";
            this.SelectionButton.Size = new System.Drawing.Size(55, 23);
            this.SelectionButton.TabIndex = 10;
            this.SelectionButton.Text = "Select";
            this.SelectionButton.UseVisualStyleBackColor = true;
            this.SelectionButton.Click += new System.EventHandler(this.SelectionButton_Click);
            // 
            // ScaleButton
            // 
            this.ScaleButton.Location = new System.Drawing.Point(339, 3);
            this.ScaleButton.Name = "ScaleButton";
            this.ScaleButton.Size = new System.Drawing.Size(50, 23);
            this.ScaleButton.TabIndex = 8;
            this.ScaleButton.Text = "Scale";
            this.ScaleButton.UseVisualStyleBackColor = true;
            this.ScaleButton.Click += new System.EventHandler(this.ScaleButton_Click);
            // 
            // RotationZButton
            // 
            this.RotationZButton.Location = new System.Drawing.Point(283, 3);
            this.RotationZButton.Name = "RotationZButton";
            this.RotationZButton.Size = new System.Drawing.Size(50, 23);
            this.RotationZButton.TabIndex = 7;
            this.RotationZButton.Text = "R(Z)";
            this.RotationZButton.UseVisualStyleBackColor = true;
            this.RotationZButton.Click += new System.EventHandler(this.RotationZButton_Click);
            // 
            // RotationYButton
            // 
            this.RotationYButton.Location = new System.Drawing.Point(227, 3);
            this.RotationYButton.Name = "RotationYButton";
            this.RotationYButton.Size = new System.Drawing.Size(50, 23);
            this.RotationYButton.TabIndex = 6;
            this.RotationYButton.Text = "R(Y)";
            this.RotationYButton.UseVisualStyleBackColor = true;
            this.RotationYButton.Click += new System.EventHandler(this.RotationYButton_Click);
            // 
            // RotationXButton
            // 
            this.RotationXButton.Location = new System.Drawing.Point(171, 3);
            this.RotationXButton.Name = "RotationXButton";
            this.RotationXButton.Size = new System.Drawing.Size(50, 23);
            this.RotationXButton.TabIndex = 5;
            this.RotationXButton.Text = "R(X)";
            this.RotationXButton.UseVisualStyleBackColor = true;
            this.RotationXButton.Click += new System.EventHandler(this.RotationXButton_Click);
            // 
            // TranslationZButton
            // 
            this.TranslationZButton.Location = new System.Drawing.Point(115, 3);
            this.TranslationZButton.Name = "TranslationZButton";
            this.TranslationZButton.Size = new System.Drawing.Size(50, 23);
            this.TranslationZButton.TabIndex = 4;
            this.TranslationZButton.Text = "T(Z)";
            this.TranslationZButton.UseVisualStyleBackColor = true;
            this.TranslationZButton.Click += new System.EventHandler(this.TranslationZButton_Click);
            // 
            // TranslationYButton
            // 
            this.TranslationYButton.Location = new System.Drawing.Point(59, 3);
            this.TranslationYButton.Name = "TranslationYButton";
            this.TranslationYButton.Size = new System.Drawing.Size(50, 23);
            this.TranslationYButton.TabIndex = 3;
            this.TranslationYButton.Text = "T(Y)";
            this.TranslationYButton.UseVisualStyleBackColor = true;
            this.TranslationYButton.Click += new System.EventHandler(this.TranslationYButton_Click);
            // 
            // ObjectsLabel
            // 
            this.ObjectsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.ObjectsLabel.AutoSize = true;
            this.ObjectsLabel.Location = new System.Drawing.Point(416, 0);
            this.ObjectsLabel.Name = "ObjectsLabel";
            this.ObjectsLabel.Size = new System.Drawing.Size(43, 13);
            this.ObjectsLabel.TabIndex = 5;
            this.ObjectsLabel.Text = "Objects";
            // 
            // OperationsLabel
            // 
            this.OperationsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.OperationsLabel.AutoSize = true;
            this.OperationsLabel.Location = new System.Drawing.Point(409, 45);
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
            this.SettingsLabel.Location = new System.Drawing.Point(754, 91);
            this.SettingsLabel.Name = "SettingsLabel";
            this.SettingsLabel.Size = new System.Drawing.Size(45, 13);
            this.SettingsLabel.TabIndex = 7;
            this.SettingsLabel.Text = "Settings";
            // 
            // ObjectsListLabel
            // 
            this.ObjectsListLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ObjectsListLabel.AutoSize = true;
            this.ObjectsListLabel.Location = new System.Drawing.Point(744, 412);
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
            this.WorldPanel.BackColor = System.Drawing.Color.Black;
            this.WorldPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.WorldPanel.Location = new System.Drawing.Point(12, 103);
            this.WorldPanel.Name = "WorldPanel";
            this.WorldPanel.Size = new System.Drawing.Size(674, 464);
            this.WorldPanel.TabIndex = 9;
            this.WorldPanel.TabStop = false;
            this.WorldPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WorldPanel_MouseDown);
            this.WorldPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WorldPanel_MouseMove);
            this.WorldPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.WorldPanel_MouseUp);
            // 
            // EffectsLabel
            // 
            this.EffectsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EffectsLabel.AutoSize = true;
            this.EffectsLabel.Location = new System.Drawing.Point(754, 249);
            this.EffectsLabel.Name = "EffectsLabel";
            this.EffectsLabel.Size = new System.Drawing.Size(40, 13);
            this.EffectsLabel.TabIndex = 11;
            this.EffectsLabel.Text = "Effects";
            // 
            // EffectsPanel
            // 
            this.EffectsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EffectsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EffectsPanel.Controls.Add(this.PolygonalChainCheckBox);
            this.EffectsPanel.Controls.Add(this.DeBoorsPointsRadioButton);
            this.EffectsPanel.Controls.Add(this.ControlPointsRadioButton);
            this.EffectsPanel.Controls.Add(this.AdditiveColorBlendingCheckBox);
            this.EffectsPanel.Controls.Add(this.StereoscopyCheckBox);
            this.EffectsPanel.Location = new System.Drawing.Point(702, 261);
            this.EffectsPanel.Name = "EffectsPanel";
            this.EffectsPanel.Size = new System.Drawing.Size(150, 144);
            this.EffectsPanel.TabIndex = 10;
            // 
            // PolygonalChainCheckBox
            // 
            this.PolygonalChainCheckBox.AutoSize = true;
            this.PolygonalChainCheckBox.Checked = true;
            this.PolygonalChainCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PolygonalChainCheckBox.Location = new System.Drawing.Point(10, 50);
            this.PolygonalChainCheckBox.Name = "PolygonalChainCheckBox";
            this.PolygonalChainCheckBox.Size = new System.Drawing.Size(102, 17);
            this.PolygonalChainCheckBox.TabIndex = 2;
            this.PolygonalChainCheckBox.Text = "Polygonal Chain";
            this.PolygonalChainCheckBox.UseVisualStyleBackColor = true;
            this.PolygonalChainCheckBox.Visible = false;
            this.PolygonalChainCheckBox.CheckedChanged += new System.EventHandler(this.PolygonalChainCheckBox_CheckedChanged);
            // 
            // DeBoorsPointsRadioButton
            // 
            this.DeBoorsPointsRadioButton.AutoSize = true;
            this.DeBoorsPointsRadioButton.Location = new System.Drawing.Point(10, 96);
            this.DeBoorsPointsRadioButton.Name = "DeBoorsPointsRadioButton";
            this.DeBoorsPointsRadioButton.Size = new System.Drawing.Size(103, 17);
            this.DeBoorsPointsRadioButton.TabIndex = 4;
            this.DeBoorsPointsRadioButton.Text = "De Boor\'s Points";
            this.DeBoorsPointsRadioButton.UseVisualStyleBackColor = true;
            this.DeBoorsPointsRadioButton.Visible = false;
            this.DeBoorsPointsRadioButton.CheckedChanged += new System.EventHandler(this.DeBoorsPointsRadioButton_CheckedChanged);
            // 
            // ControlPointsRadioButton
            // 
            this.ControlPointsRadioButton.AutoSize = true;
            this.ControlPointsRadioButton.Location = new System.Drawing.Point(10, 73);
            this.ControlPointsRadioButton.Name = "ControlPointsRadioButton";
            this.ControlPointsRadioButton.Size = new System.Drawing.Size(90, 17);
            this.ControlPointsRadioButton.TabIndex = 3;
            this.ControlPointsRadioButton.Text = "Control Points";
            this.ControlPointsRadioButton.UseVisualStyleBackColor = true;
            this.ControlPointsRadioButton.Visible = false;
            this.ControlPointsRadioButton.CheckedChanged += new System.EventHandler(this.ControlPointsRadioButton_CheckedChanged);
            // 
            // AdditiveColorBlendingCheckBox
            // 
            this.AdditiveColorBlendingCheckBox.AutoSize = true;
            this.AdditiveColorBlendingCheckBox.Location = new System.Drawing.Point(10, 27);
            this.AdditiveColorBlendingCheckBox.Name = "AdditiveColorBlendingCheckBox";
            this.AdditiveColorBlendingCheckBox.Size = new System.Drawing.Size(135, 17);
            this.AdditiveColorBlendingCheckBox.TabIndex = 1;
            this.AdditiveColorBlendingCheckBox.Text = "Additive Color Blending";
            this.AdditiveColorBlendingCheckBox.UseVisualStyleBackColor = true;
            this.AdditiveColorBlendingCheckBox.Visible = false;
            this.AdditiveColorBlendingCheckBox.CheckedChanged += new System.EventHandler(this.AdditiveColorBlendingCheckBox_CheckedChanged);
            // 
            // StereoscopyCheckBox
            // 
            this.StereoscopyCheckBox.AutoSize = true;
            this.StereoscopyCheckBox.Location = new System.Drawing.Point(10, 4);
            this.StereoscopyCheckBox.Name = "StereoscopyCheckBox";
            this.StereoscopyCheckBox.Size = new System.Drawing.Size(85, 17);
            this.StereoscopyCheckBox.TabIndex = 0;
            this.StereoscopyCheckBox.Text = "Stereoscopy";
            this.StereoscopyCheckBox.UseVisualStyleBackColor = true;
            this.StereoscopyCheckBox.Visible = false;
            this.StereoscopyCheckBox.CheckedChanged += new System.EventHandler(this.StereoscopyCheckBox_CheckedChanged);
            // 
            // CursorXLabel
            // 
            this.CursorXLabel.AutoSize = true;
            this.CursorXLabel.Location = new System.Drawing.Point(40, 6);
            this.CursorXLabel.Name = "CursorXLabel";
            this.CursorXLabel.Size = new System.Drawing.Size(47, 13);
            this.CursorXLabel.TabIndex = 13;
            this.CursorXLabel.Text = "Cursor X";
            // 
            // CursorXBox
            // 
            this.CursorXBox.Location = new System.Drawing.Point(93, 3);
            this.CursorXBox.Name = "CursorXBox";
            this.CursorXBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CursorXBox.Size = new System.Drawing.Size(46, 20);
            this.CursorXBox.TabIndex = 12;
            this.CursorXBox.Tag = "";
            this.CursorXBox.TextChanged += new System.EventHandler(this.CursorXBox_TextChanged);
            // 
            // CursorYLabel
            // 
            this.CursorYLabel.AutoSize = true;
            this.CursorYLabel.Location = new System.Drawing.Point(145, 6);
            this.CursorYLabel.Name = "CursorYLabel";
            this.CursorYLabel.Size = new System.Drawing.Size(47, 13);
            this.CursorYLabel.TabIndex = 15;
            this.CursorYLabel.Text = "Cursor Y";
            // 
            // CursorYBox
            // 
            this.CursorYBox.Location = new System.Drawing.Point(198, 3);
            this.CursorYBox.Name = "CursorYBox";
            this.CursorYBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CursorYBox.Size = new System.Drawing.Size(46, 20);
            this.CursorYBox.TabIndex = 14;
            this.CursorYBox.Tag = "";
            this.CursorYBox.TextChanged += new System.EventHandler(this.CursorYBox_TextChanged);
            // 
            // CursorZLabel
            // 
            this.CursorZLabel.AutoSize = true;
            this.CursorZLabel.Location = new System.Drawing.Point(250, 6);
            this.CursorZLabel.Name = "CursorZLabel";
            this.CursorZLabel.Size = new System.Drawing.Size(47, 13);
            this.CursorZLabel.TabIndex = 17;
            this.CursorZLabel.Text = "Cursor Z";
            // 
            // CursorZBox
            // 
            this.CursorZBox.Location = new System.Drawing.Point(303, 3);
            this.CursorZBox.Name = "CursorZBox";
            this.CursorZBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CursorZBox.Size = new System.Drawing.Size(46, 20);
            this.CursorZBox.TabIndex = 16;
            this.CursorZBox.Tag = "";
            this.CursorZBox.TextChanged += new System.EventHandler(this.CursorZBox_TextChanged);
            // 
            // CursorScreenXLabel
            // 
            this.CursorScreenXLabel.AutoSize = true;
            this.CursorScreenXLabel.Location = new System.Drawing.Point(355, 6);
            this.CursorScreenXLabel.Name = "CursorScreenXLabel";
            this.CursorScreenXLabel.Size = new System.Drawing.Size(84, 13);
            this.CursorScreenXLabel.TabIndex = 19;
            this.CursorScreenXLabel.Text = "Cursor Screen X";
            // 
            // CursorScreenXBox
            // 
            this.CursorScreenXBox.Enabled = false;
            this.CursorScreenXBox.Location = new System.Drawing.Point(445, 3);
            this.CursorScreenXBox.Name = "CursorScreenXBox";
            this.CursorScreenXBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CursorScreenXBox.Size = new System.Drawing.Size(46, 20);
            this.CursorScreenXBox.TabIndex = 18;
            this.CursorScreenXBox.Tag = "";
            this.CursorScreenXBox.TextChanged += new System.EventHandler(this.CursorScreenXBox_TextChanged);
            // 
            // CursorScreenYLabel
            // 
            this.CursorScreenYLabel.AutoSize = true;
            this.CursorScreenYLabel.Location = new System.Drawing.Point(497, 6);
            this.CursorScreenYLabel.Name = "CursorScreenYLabel";
            this.CursorScreenYLabel.Size = new System.Drawing.Size(84, 13);
            this.CursorScreenYLabel.TabIndex = 21;
            this.CursorScreenYLabel.Text = "Cursor Screen Y";
            // 
            // CursorScreenYBox
            // 
            this.CursorScreenYBox.Enabled = false;
            this.CursorScreenYBox.Location = new System.Drawing.Point(587, 3);
            this.CursorScreenYBox.Name = "CursorScreenYBox";
            this.CursorScreenYBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CursorScreenYBox.Size = new System.Drawing.Size(46, 20);
            this.CursorScreenYBox.TabIndex = 20;
            this.CursorScreenYBox.Tag = "";
            this.CursorScreenYBox.TextChanged += new System.EventHandler(this.CursorScreenYBox_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.CursorXLabel);
            this.panel1.Controls.Add(this.CursorScreenYLabel);
            this.panel1.Controls.Add(this.CursorXBox);
            this.panel1.Controls.Add(this.CursorScreenYBox);
            this.panel1.Controls.Add(this.CursorYBox);
            this.panel1.Controls.Add(this.CursorScreenXLabel);
            this.panel1.Controls.Add(this.CursorYLabel);
            this.panel1.Controls.Add(this.CursorScreenXBox);
            this.panel1.Controls.Add(this.CursorZBox);
            this.panel1.Controls.Add(this.CursorZLabel);
            this.panel1.Location = new System.Drawing.Point(12, 581);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(674, 29);
            this.panel1.TabIndex = 22;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 622);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.EffectsLabel);
            this.Controls.Add(this.EffectsPanel);
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
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(880, 660);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Geometric Modeling";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyUp);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.ObjectsPanel.ResumeLayout(false);
            this.OptionsPanel.ResumeLayout(false);
            this.OptionsPanel.PerformLayout();
            this.OperationsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.WorldPanel)).EndInit();
            this.EffectsPanel.ResumeLayout(false);
            this.EffectsPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.Label EffectsLabel;
        private System.Windows.Forms.Panel EffectsPanel;
        private System.Windows.Forms.CheckBox StereoscopyCheckBox;
        private System.Windows.Forms.CheckBox AdditiveColorBlendingCheckBox;
        private System.Windows.Forms.Button PointButton;
        private System.Windows.Forms.Button BezierCurveButton;
        private System.Windows.Forms.Label CursorXLabel;
        private System.Windows.Forms.TextBox CursorXBox;
        private System.Windows.Forms.Label CursorYLabel;
        private System.Windows.Forms.TextBox CursorYBox;
        private System.Windows.Forms.Label CursorZLabel;
        private System.Windows.Forms.TextBox CursorZBox;
        private System.Windows.Forms.Label CursorScreenXLabel;
        private System.Windows.Forms.TextBox CursorScreenXBox;
        private System.Windows.Forms.Label CursorScreenYLabel;
        private System.Windows.Forms.TextBox CursorScreenYBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button SelectionButton;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.RadioButton DeBoorsPointsRadioButton;
        private System.Windows.Forms.RadioButton ControlPointsRadioButton;
        private System.Windows.Forms.CheckBox PolygonalChainCheckBox;
        private System.Windows.Forms.Button BezierCurveC2Button;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button InterpolationCurveButton;
        private System.Windows.Forms.RadioButton ChordParametrizationRadioButton;
        private System.Windows.Forms.RadioButton NormalParametrizationRadioButton;
        private System.Windows.Forms.Button BezierSurfaceC2Button;
        private System.Windows.Forms.Button BezierSurfaceC0Button;
        private System.Windows.Forms.TextBox SurfaceGridResolutionYBox;
        private System.Windows.Forms.TextBox SurfaceGridResolutionXBox;
        private System.Windows.Forms.Button CollapseButton;
        private System.Windows.Forms.Button GregorySurfaceButton;
        private System.Windows.Forms.Button IntersectionButton;
    }
}

