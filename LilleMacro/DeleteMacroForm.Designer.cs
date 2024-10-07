// DeleteMacroForm.Designer.cs
using System.ComponentModel;
using System.Windows.Forms;

namespace LilleMacro
{
    partial class DeleteMacroForm
    {
        private IContainer components = null;
        private ListView _macroListView;
        private Button _deleteButton;
        private Button _okButton;

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
            this._macroListView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable
            };
            this._macroListView.Columns.Add("Hotkey", 100);
            this._macroListView.Columns.Add("Macro", 200);

            this._deleteButton = new Button { Text = "Delete", AutoSize = true, Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right };
            this._okButton = new Button { Text = "OK", AutoSize = true, Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right };

            this._deleteButton.Click += DeleteButton_Click;
            this._okButton.Click += OkButton_Click;

            var layout = new TableLayoutPanel { Dock = DockStyle.Fill };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(this._macroListView, 0, 0);
            layout.Controls.Add(this._okButton, 0, 1);
            layout.Controls.Add(this._deleteButton, 0, 2);

            this.Controls.Add(layout);
            this.Text = "Delete Macro";
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.AutoScaleMode = AutoScaleMode.Font;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}