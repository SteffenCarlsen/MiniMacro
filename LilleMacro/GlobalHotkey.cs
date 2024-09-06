using System.Runtime.InteropServices;

namespace LilleMacro;

public class GlobalHotkey : IDisposable
{
    private readonly int _id;
    private readonly bool _registered;
    private readonly IntPtr _windowHandle;

    public GlobalHotkey(IntPtr windowHandle, Keys key)
    {
        _windowHandle = windowHandle;
        _id = key.GetHashCode();
        _registered = RegisterHotKey(_windowHandle, _id, 0, (uint)key);
    }

    public void Dispose()
    {
        if (_registered)
        {
            UnregisterHotKey(_windowHandle, _id);
        }
    }

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
}