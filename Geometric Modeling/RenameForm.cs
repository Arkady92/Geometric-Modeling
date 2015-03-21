using System.Windows.Forms;

namespace Geometric_Modeling
{
    public partial class RenameForm : Form
    {
        public string EnteredText;
 
        public RenameForm()
        {
            InitializeComponent();
        }

        private void RenameForm_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyboard(sender, e);
        }

        private void HandleKeyboard(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    EnteredText = RenameTextBox.Text;
                    DialogResult = DialogResult.OK;
                    break;
                case Keys.Escape:
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void RenameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyboard(sender, e);
        }
    }
}
