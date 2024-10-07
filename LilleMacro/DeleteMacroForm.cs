// DeleteMacroForm.cs

namespace LilleMacro
{
    public partial class DeleteMacroForm : Form
    {
        public List<SavedMacro> SavedMacros { get; set; }

        public DeleteMacroForm(List<SavedMacro> savedMacros)
        {
            InitializeComponent();
            SavedMacros = savedMacros;
            UpdateMacroList();
            Closing += (_, _) => { DialogResult = DialogResult.OK; };
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_macroListView.SelectedIndices.Count > 0)
            {
                SavedMacros.RemoveAt(_macroListView.SelectedIndices[0]);
                UpdateMacroList();
            }
            else
            {
                MessageBox.Show("Please select a macro to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateMacroList()
        {
            _macroListView.Items.Clear();
            foreach (var macro in SavedMacros)
            {
                var item = new ListViewItem(macro.Hotkey.ToString());
                item.SubItems.Add(macro.MacroString);
                _macroListView.Items.Add(item);
            }
        }
    }
}