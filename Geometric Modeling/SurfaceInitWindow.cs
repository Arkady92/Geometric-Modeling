using System.Globalization;
using System.Windows.Forms;

namespace Geometric_Modeling
{
    public partial class SurfaceInitWindow : Form
    {
        public bool IsSurfaceCylindrical = false;
        public double SurfaceWidth = 0.5;
        public double SurfaceHeight = 0.5;
        public double DefaultWidth = 0.5;
        public double DefaultRadius = 0.25;
        public int SurfacePatchesLengthCount = 1;
        public int SurfacePatchesBreadthCount = 1;

        public SurfaceInitWindow()
        {
            InitializeComponent();
            WidthTextBox.Text = SurfaceWidth.ToString(CultureInfo.InvariantCulture);
            HeightTextBox.Text = SurfaceHeight.ToString(CultureInfo.InvariantCulture);
            LengthTextBox.Text = SurfacePatchesLengthCount.ToString(CultureInfo.InvariantCulture);
            BreadthTextBox.Text = SurfacePatchesBreadthCount.ToString(CultureInfo.InvariantCulture);
        }

        private void FlatPatchRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null) IsSurfaceCylindrical = !radioButton.Checked;
            WidthLabel.Text = @"Width";
            WidthTextBox.Text = DefaultWidth.ToString(CultureInfo.InvariantCulture);
        }

        private void CylindricalPatchRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null) IsSurfaceCylindrical = radioButton.Checked;
            WidthLabel.Text = @"Radius";
            WidthTextBox.Text = DefaultRadius.ToString(CultureInfo.InvariantCulture);
        }

        private void LengthTextBox_TextChanged(object sender, System.EventArgs e)
        {
            var textBox = sender as TextBox;
            int result;
            if (textBox == null) return;
            if (int.TryParse(textBox.Text, out result) && result > 0 && result < 1000)
                SurfacePatchesLengthCount = result;
            else if (textBox.Text != string.Empty)
                textBox.Text = SurfacePatchesLengthCount.ToString(CultureInfo.InvariantCulture);
        }

        private void BreadthTextBox_TextChanged(object sender, System.EventArgs e)
        {
            var textBox = sender as TextBox;
            int result;
            if (textBox == null) return;
            if (int.TryParse(textBox.Text, out result) && result > 0 && result < 1000)
                SurfacePatchesBreadthCount = result;
            else if (textBox.Text != string.Empty)
                textBox.Text = SurfacePatchesBreadthCount.ToString(CultureInfo.InvariantCulture);
        }

        private void WidthTextBox_TextChanged(object sender, System.EventArgs e)
        {
            var textBox = sender as TextBox;
            double result;
            if (textBox == null) return;
            if (double.TryParse(textBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out result)
                && result > 0 && result <= 10)
                SurfaceWidth = result;
            else if (textBox.Text != string.Empty && textBox.Text[0] != '.' && textBox.Text[textBox.TextLength - 1] != '.'
                && textBox.Text != @"-")
                textBox.Text = SurfaceWidth.ToString(CultureInfo.InvariantCulture);
        }

        private void HeightTextBox_TextChanged(object sender, System.EventArgs e)
        {
            var textBox = sender as TextBox;
            double result;
            if (textBox == null) return;
            if (double.TryParse(textBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out result)
                && result > 0 && result <= 10)
                SurfaceHeight = result;
            else if (textBox.Text != string.Empty && textBox.Text[0] != '.' && textBox.Text[textBox.TextLength - 1] != '.'
                && textBox.Text != @"-")
                textBox.Text = SurfaceHeight.ToString(CultureInfo.InvariantCulture);
        }

        private void GenerateButton_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
