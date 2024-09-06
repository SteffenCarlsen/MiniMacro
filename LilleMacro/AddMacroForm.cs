using System;
using System.Windows.Forms;

namespace LilleMacro
{
    public partial class AddMacroForm : Form
    {

        public Keys Hotkey { get; private set; }
        public string MacroString { get; private set; } = string.Empty;

        public AddMacroForm()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (Enum.TryParse(this._hotkeyTextBox.Text, out Keys hotkey))
            {
                this.Hotkey = hotkey;
                this.MacroString = this._macroStringTextBox.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid hotkey. Please enter a valid key (e.g., F8).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}