using System.Diagnostics;
using System.Text.Json;

namespace LilleMacro;

public class LilleMakro : ApplicationContext
{
    private const int FONTSIZE_BIG = 21;
    private const string SAVED_MACROS_FILE = "SavedMacros.json";
    private ToolStripMenuItem _exitApplicationItem;
    private List<GlobalHotkey> _globalHotkeys = new List<GlobalHotkey>();
    private List<MessageWindow> _messageWindows = new List<MessageWindow>();
    private List<SavedMacro> _savedMacros = new List<SavedMacro>();
    private ToolStripMenuItem _showSettingsItem;
    private ToolStripMenuItem _addTostartupItem;
    private ToolStripMenuItem _toolTipTitleItem;
    private ToolStripMenuItem _addnewMacroItem;
    private ToolStripMenuItem _deleteMacroItem;
    private NotifyIcon? _trayIcon;
    private RegistryController _registryController;
    private const string APP_NAME = "LilleMakro";
    
    public LilleMakro()
    {
        InitializeSavedMacros();
        Initialize();
        RegisterHotkey();
        Application.ApplicationExit += ApplicationOnApplicationExit;
    }

    private void ApplicationOnApplicationExit(object? sender, EventArgs e)
    {
        foreach (var globalHotkey in _globalHotkeys)
        {
            globalHotkey.Dispose();
        }

        foreach (var messageWindow in _messageWindows)
        {
            messageWindow.Dispose();
        }
        _trayIcon?.Dispose();
        this.Dispose(true);
        Environment.Exit(0);
    }

    private void InitializeSavedMacros()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var filePath = Path.Combine(Environment.CurrentDirectory, SAVED_MACROS_FILE);

        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            _savedMacros = JsonSerializer.Deserialize<List<SavedMacro>>(json) ?? new List<SavedMacro>();
        }
        else
        {
            _savedMacros.Add(new SavedMacro { Hotkey = Keys.F8, MacroString = ".com" });
            var json = JsonSerializer.Serialize(_savedMacros, options);
            File.WriteAllText(filePath, json);
        }
    }

    private void RegisterHotkey()
    {
        foreach (var macro in _savedMacros)
        {
            var msgWindow = new MessageWindow();
            _globalHotkeys.Add(new GlobalHotkey(msgWindow.Handle, macro.Hotkey));
            msgWindow.HotkeyPressed += () => SendKeys.Send(macro.MacroString);
            _messageWindows.Add(msgWindow);
        }
    }

    private void UnregisterHotkeys()
    {
        foreach (var hotkey in _globalHotkeys)
        {
            hotkey.Dispose();
        }

        foreach (var messageWindow in _messageWindows)
        {
            messageWindow.Dispose();
        }
        
        _messageWindows.Clear();
        _globalHotkeys.Clear();
    }

    private void Initialize()
    {
        _registryController = new RegistryController();
        _toolTipTitleItem = new ToolStripMenuItem("Small macro")
        {
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            Enabled = false
        };
        _exitApplicationItem = new ToolStripMenuItem("Exit", null, OnApplicationExit);
        _showSettingsItem = new ToolStripMenuItem("Settings", null, OpenSettingsFile);
        _addnewMacroItem = new ToolStripMenuItem("Add new macro", null, AddNewMacro);
        _addTostartupItem = new ToolStripMenuItem("Add to startup", null, AddToStartup) { Checked = _registryController.IsProgramRegistered(APP_NAME)};
        _deleteMacroItem = new ToolStripMenuItem("Delete macro", null, OnClickDeleteItem);
        _trayIcon = new NotifyIcon
        {
            Visible = true,
            Icon = GenerateIcon(),
            BalloonTipText = "Lille makro",
            Text = "Lille makro",
            ContextMenuStrip = new ContextMenuStrip
            {
                Items =
                {
                    _toolTipTitleItem,
                    _addnewMacroItem,
                    _deleteMacroItem,
                    _showSettingsItem,
                    _addTostartupItem,
                    _exitApplicationItem
                }
            }
        };
    }

    private void OnClickDeleteItem(object sender, EventArgs e)
    {
        var deleteMacroForm = new DeleteMacroForm(_savedMacros);
        deleteMacroForm.ShowDialog();
        if (deleteMacroForm.DialogResult == DialogResult.OK)
        {
            _savedMacros = deleteMacroForm.SavedMacros;
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_savedMacros, options);
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, SAVED_MACROS_FILE), json);
            UnregisterHotkeys();
            RegisterHotkey();
            _trayIcon?.ShowBalloonTip(250, "Success", "Macros updated successfully.", ToolTipIcon.Info);
        }
        else
        {
            _trayIcon?.ShowBalloonTip(250, "Error", "Failed to update macros", ToolTipIcon.Error);
        }
    }

    private void AddToStartup(object? sender, EventArgs e)
    {
        _addTostartupItem.Checked = !_addTostartupItem.Checked;
        if (_addTostartupItem is not null && _addTostartupItem.Checked)
        {
            string pathToExe = "\"" + Application.ExecutablePath;
            if (!_registryController.IsProgramRegistered(APP_NAME))
            {
                _trayIcon.BalloonTipText = _registryController.RegisterProgram(APP_NAME, pathToExe) 
                    ? "Registered to Autostart!" 
                    : "Registering to Autostart failed!";
            }
            else if (!_registryController.IsStartupPathUnchanged(APP_NAME, pathToExe))
            {
                _trayIcon.BalloonTipText = _registryController.RegisterProgram(APP_NAME, pathToExe)
                    ? "Updated Autostart Path!"
                    : "Updating Autostart Path failed!";
            }
            else
            {
                return;
            }
        }
        else
        {
            _trayIcon.BalloonTipText = _registryController.UnregisterProgram(APP_NAME) 
                ? "Unregistered from Autostart!" 
                : "Unregistering from Autostart failed!";
        }

        _trayIcon.ShowBalloonTip(250);
    }

    private void AddNewMacro(object? sender, EventArgs e)
    {
        var addMacroForm = new AddMacroForm();
        addMacroForm.ShowDialog();
        if (addMacroForm.DialogResult == DialogResult.OK)
        {
            _savedMacros.Add(new SavedMacro { Hotkey = addMacroForm.Hotkey, MacroString = addMacroForm.MacroString });
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_savedMacros, options);
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, SAVED_MACROS_FILE), json);
            UnregisterHotkeys();
            RegisterHotkey();
            _trayIcon?.ShowBalloonTip(250, "Success", "Macro Added successfully.", ToolTipIcon.Info);
        }
        else
        {
            _trayIcon?.ShowBalloonTip(250, "Error", "Failed to add macro.", ToolTipIcon.Error);
        }
    }
    private void OpenSettingsFile(object? sender, EventArgs e)
    {
        var filePath = Path.Combine(Environment.CurrentDirectory, SAVED_MACROS_FILE);
        if (File.Exists(filePath))
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }
        else
        {
            MessageBox.Show("Settings file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void OnApplicationExit(object? sender, EventArgs e)
    {
        ApplicationOnApplicationExit(sender, e);
        
        Application.Exit();
    }

    /// <summary>
    ///     Dynamically generate a weeknumber icon for the application
    /// </summary>
    /// <param name="weekNum">Weeknumber to generate icon for</param>
    /// <returns>A windows application icon</returns>
    private Icon GenerateIcon()
    {
        var bitmap = new Bitmap(32, 32);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, 32, 32));
            g.DrawString("M", new Font("Verdana", FONTSIZE_BIG, FontStyle.Bold), new SolidBrush(Color.White), new PointF(0, 0));
            return Icon.FromHandle(bitmap.GetHicon());
        }
    }

    private class MessageWindow : NativeWindow, IDisposable
    {
        private const int WM_HOTKEY = 0x0312;

        public MessageWindow()
        {
            CreateHandle(new CreateParams());
        }

        public void Dispose()
        {
            DestroyHandle();
        }

        public event Action? HotkeyPressed;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                HotkeyPressed?.Invoke();
            }

            base.WndProc(ref m);
        }
    }
}