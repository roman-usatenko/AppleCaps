using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AppleCaps
{
    static class Program
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const int KEYEVENTF_KEYUP = 0x0002;
        private const byte VK_CONTROL = 0x11;
        private const byte VK_SHIFT = 0x10;
        private const byte VK_CAPSLOCK = 0x14;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private static LowLevelKeyboardProc Callback = HookCallback;
        private static IntPtr HookID = IntPtr.Zero;

        private static long DownEventTime;
        private static bool LetThru;

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_KEYUP))
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode == (int)Keys.CapsLock && !LetThru)
                {
                    if (wParam == (IntPtr)WM_KEYDOWN)
                    {
                        if (DownEventTime == 0)
                        {
                            DownEventTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        }
                    }
                    else
                    {
                        long upEventTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        if (upEventTime - DownEventTime < 500)
                        {
                            PressCtrlShift();
                        }
                        else
                        {
                            PressCapsLock();
                        }
                        DownEventTime = 0;
                    }
                    return (IntPtr)1; // Suppress the further processing of the CapsLock press
                }
            }
            return CallNextHookEx(HookID, nCode, wParam, lParam);
        }

        private static void PressCtrlShift()
        {
            keybd_event(VK_SHIFT, 0, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_CONTROL, 0, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_CONTROL, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
            keybd_event(VK_SHIFT, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        private static void PressCapsLock()
        {
            LetThru = true;
            keybd_event(VK_CAPSLOCK, 0, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_CAPSLOCK, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
            LetThru = false;
        }

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            HookID = SetHook(Callback);
            Application.Run(new AppContext());
            UnhookWindowsHookEx(HookID);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
    }
}