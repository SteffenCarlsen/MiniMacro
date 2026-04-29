using System.ComponentModel;
using System.Windows.Forms;

namespace LilleMacro
{
    partial class AddMacroForm
    {
        private IContainer components = null;
        private TextBox _hotkeyTextBox;
        private TextBox _macroStringTextBox;
        private CheckBox _repeatEveryMinuteCheckBox;
        private Button _okButton;
        private Button _cancelButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this._hotkeyTextBox = new TextBox { PlaceholderText = "Enter hotkey (e.g., F8)", Width = 200, AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            this._macroStringTextBox = new TextBox { PlaceholderText = "Enter macro string (e.g., .com)", Width = 200, AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            this._repeatEveryMinuteCheckBox = new CheckBox { Text = "Repeat every 1 minute (toggle with hotkey)", AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Left };
            this._okButton = new Button { Text = "OK", AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            this._cancelButton = new Button { Text = "Cancel", AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
            layout.Controls.Add(new Label { Text = "Hotkey:", AutoSize = true }, 0, 0);
            layout.Controls.Add(this._hotkeyTextBox, 1, 0);
            layout.Controls.Add(new Label { Text = "Macro String:", AutoSize = true }, 0, 1);
            layout.Controls.Add(this._macroStringTextBox, 1, 1);
            layout.Controls.Add(this._repeatEveryMinuteCheckBox, 1, 2);
            layout.Controls.Add(this._okButton, 0, 3);
            layout.Controls.Add(this._cancelButton, 1, 3);

            this._okButton.Click += OkButton_Click;
            this._cancelButton.Click += CancelButton_Click;

            this.Controls.Add(layout);
            this.Text = "Add New Macro";
            this.ClientSize = new System.Drawing.Size(340, 180);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
        }
    }
}