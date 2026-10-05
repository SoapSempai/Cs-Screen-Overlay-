using System.Runtime.InteropServices;

namespace TEST_1;

internal sealed class SafeInputController
{
    public event Action? HotkeyTriggered;
    public event Action? EmergencyStopTriggered;

    private bool _enabled;
    private const int WM_HOTKEY = 0x0312;
    private const int HOTKEY_MOVE = 0x1001;
    private const int HOTKEY_STOP = 0x1002;
    private const uint MOD_NOREPEAT = 0x4000;

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    [StructLayout(LayoutKind.Sequential)]
    private struct INPUT { public uint type; public InputUnion U; }

    [StructLayout(LayoutKind.Explicit)]
    private struct InputUnion { [FieldOffset(0)] public MOUSEINPUT mi; }

    [StructLayout(LayoutKind.Sequential)]
    private struct MOUSEINPUT
    {
        public int dx, dy;
        public uint mouseData, dwFlags, time;
        public IntPtr dwExtraInfo;
    }

    public void Enable() => _enabled = true;
    public void Disable() => _enabled = false;

    public void MoveTo(Point point)
    {
        if (!_enabled) return;
        var bounds = Screen.PrimaryScreen?.Bounds ?? new Rectangle(0, 0, 1920, 1080);
        var x = Math.Clamp((point.X - bounds.Left) * 65535 /
                           Math.Max(1, bounds.Width - 1), 0, 65535);
        var y = Math.Clamp((point.Y - bounds.Top) * 65535 /
                           Math.Max(1, bounds.Height - 1), 0, 65535);

        SendInput(1, new[] {
            new INPUT { type = 0, U = new InputUnion {
                mi = new MOUSEINPUT { dx = x, dy = y, dwFlags = 0x8001 }
            }}
        }, Marshal.SizeOf<INPUT>());
    }

    public void ClickAt(Point point)
    {
        if (!_enabled) return;
        MoveTo(point);
        SendInput(1, new[] {
            new INPUT { type = 0, U = new InputUnion {
                mi = new MOUSEINPUT { dwFlags = 0x0002 }
            }}
        }, Marshal.SizeOf<INPUT>());
        SendInput(1, new[] {
            new INPUT { type = 0, U = new InputUnion {
                mi = new MOUSEINPUT { dwFlags = 0x0004 }
            }}
        }, Marshal.SizeOf<INPUT>());
    }

    public bool HandleWindowsMessage(ref Message m)
    {
        if (m.Msg != WM_HOTKEY) return false;
        if (m.WParam.ToInt32() == HOTKEY_MOVE) HotkeyTriggered?.Invoke();
        if (m.WParam.ToInt32() == HOTKEY_STOP) EmergencyStopTriggered?.Invoke();
        return true;
    }

    public void RegisterHotkeys(IntPtr handle)
    {
        RegisterHotKey(handle, HOTKEY_MOVE, MOD_NOREPEAT, (uint)Keys.F8);
        RegisterHotKey(handle, HOTKEY_STOP, MOD_NOREPEAT, (uint)Keys.F9);
    }

    public void UnregisterHotkeys() { }
    public void Dispose() { }
}
