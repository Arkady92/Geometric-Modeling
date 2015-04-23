using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Mathematics;
using Models;

namespace Geometric_Modeling
{
    public partial class MainWindow
    {
        #region Members
        private List<GeometricModel> _models;
        private List<GeometricModel> _hiddenModels;
        private Dictionary<Operation, Button> _operationsButtons;
        private Dictionary<ModelType, List<Control>> _modelsParameters;
        private Operation _currentOperation;
        private bool _operationInProgress;
        private int _mousePositionX;
        private int _mousePositionY;
        private Matrix _currentProjectionMatrix;
        private Matrix _currentStereoscopyLeftMatrix;
        private Matrix _currentStereoscopyRightMatrix;
        private Bitmap _backBuffer;
        private bool _forceStaticGraphics;
        private bool _enableAnimations;
        private bool _enableStereoscopy;
        private const double ProjectionR = 11;
        private const double ProjectionE = 1;
        private bool _enableAdditiveColorBlending;
        private bool _enableWorldDrawing;

        #endregion

        #region Initialization
        public MainWindow()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            SetDoubleBuffered(WorldPanel);
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            UpdateTextBoxes();
            Parameters.WorldPanelWidth = WorldPanel.Width;
            Parameters.WorldPanelHeight = WorldPanel.Height;
            _enableWorldDrawing = false;
            _forceStaticGraphics = false;
            _enableAnimations = false;
            _backBuffer = new Bitmap(WorldPanel.Width, WorldPanel.Height);
            _currentOperation = Operation.None;
            _currentProjectionMatrix = OperationsMatrices.Projection(200);
            _currentStereoscopyLeftMatrix = OperationsMatrices.StereoscopyLeft(ProjectionR, ProjectionE);
            _currentStereoscopyRightMatrix = OperationsMatrices.StereoscopyRight(ProjectionR, ProjectionE);
            _models = new List<GeometricModel>();
            _hiddenModels = new List<GeometricModel>();
            _operationsButtons = new Dictionary<Operation, Button>
            {
                {Operation.TranslationX, TranslationXButton},
                {Operation.TranslationY, TranslationYButton},
                {Operation.TranslationZ, TranslationZButton},
                {Operation.RotationX, RotationXButton},
                {Operation.RotationY, RotationYButton},
                {Operation.RotationZ, RotationZButton},
                {Operation.Scale, ScaleButton},
                {Operation.Selection, SelectionButton}
            };
            _modelsParameters = new Dictionary<ModelType, List<Control>>
            {
                {ModelType.Torus, new List<Control> {GridResolutionXBox, GridResolutionYBox, GridResolutionXLabel, 
                    GridResolutionYLabel, StereoscopyCheckBox, AdditiveColorBlendingCheckBox}},
                {ModelType.Ellipsoid, new List<Control> {IlluminanceBox, XAxisFactorBox, YAxisFactorBox, ZAxisFactorBox, PixelMaxSizeBox, 
                    IlluminanceLabel, XAxisFactorLabel, YAxisFactorLabel, ZAxisFactorLabel, PixelMaxSizeLabel}},
                    {ModelType.Point, new List<Control>{StereoscopyCheckBox, AdditiveColorBlendingCheckBox}},
                    {ModelType.BezierCurve, new List<Control>{StereoscopyCheckBox, AdditiveColorBlendingCheckBox, PolygonalChainCheckBox,}},
                    {ModelType.BezierCurveC2, new List<Control>{StereoscopyCheckBox, AdditiveColorBlendingCheckBox, PolygonalChainCheckBox,
                        ControlPointsRadioButton, DeBoorsPointsRadioButton}},
                    {ModelType.Cursor, new List<Control>{StereoscopyCheckBox, AdditiveColorBlendingCheckBox}},
                    {ModelType.InterpolationCurve, new List<Control>{StereoscopyCheckBox, AdditiveColorBlendingCheckBox, 
                        NormalParametrizationRadioButton, ChordParametrizationRadioButton}}
            };
            DisableAllSettings();

            _models.Add(Models.Cursor.Instance);
            
            Parameters.WorldPanelSizeFactor = (Parameters.WorldPanelWidth < Parameters.WorldPanelHeight) ?
                Parameters.WorldPanelWidth * 0.25f : Parameters.WorldPanelHeight * 0.25f;

            _enableWorldDrawing = true;
            DrawWorld();
        }

        private void UpdateTextBoxes()
        {
            _enableWorldDrawing = false;
            GridResolutionXBox.Text = Parameters.GridResolutionX.ToString(CultureInfo.InvariantCulture);
            GridResolutionYBox.Text = Parameters.GridResolutionY.ToString(CultureInfo.InvariantCulture);
            IlluminanceBox.Text = Parameters.Illuminance.ToString(CultureInfo.InvariantCulture);
            XAxisFactorBox.Text = Parameters.XAxisFactor.ToString(CultureInfo.InvariantCulture);
            YAxisFactorBox.Text = Parameters.YAxisFactor.ToString(CultureInfo.InvariantCulture);
            ZAxisFactorBox.Text = Parameters.ZAxisFactor.ToString(CultureInfo.InvariantCulture);
            PixelMaxSizeBox.Text = Parameters.PixelMaxSize.ToString(CultureInfo.InvariantCulture);
            var position = Models.Cursor.GetCurrentPosition();
            CursorXBox.Text = Math.Round(position.X,2).ToString(CultureInfo.InvariantCulture);
            CursorYBox.Text = Math.Round(position.Y,2).ToString(CultureInfo.InvariantCulture);
            CursorZBox.Text = Math.Round(position.Z,2).ToString(CultureInfo.InvariantCulture);
            CursorScreenXBox.Text = Models.Cursor.ScreenXPosition.ToString(CultureInfo.InvariantCulture);
            CursorScreenYBox.Text = Models.Cursor.ScreenYPosition.ToString(CultureInfo.InvariantCulture);
            _enableWorldDrawing = true;
        }

        public static void SetDoubleBuffered(Control c)
        {
            System.Reflection.PropertyInfo aProp = typeof(Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }
        #endregion
    }
}
