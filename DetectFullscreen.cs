using System;
using System.Runtime.InteropServices;

namespace Hoot
{
    internal class DetectFullscreen
    {
        // Import necessary functions from user32.dll
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct WINDOWINFO
        {
            public uint cbSize;
            public RECT rcWindow;
            public RECT rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public ushort atomWindowType;
            public ushort wCreatorVersion;

            public WINDOWINFO(Boolean? filler) : this() // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)"
            {
                cbSize = (UInt32)(Marshal.SizeOf(typeof(WINDOWINFO)));
            }
        }

        public static bool IsExclusiveFullscreen()
        {
            IntPtr foregroundWindowHandle = GetForegroundWindow();
            WINDOWINFO windowInfo = new WINDOWINFO();
            windowInfo.cbSize = (uint)Marshal.SizeOf(windowInfo);
            GetWindowInfo(foregroundWindowHandle, ref windowInfo);

            // Check if the window is maximized and covers the whole screen
            return ((windowInfo.dwStyle & 0x01000000) != 0) && (windowInfo.rcClient.Left == 0) && (windowInfo.rcClient.Top == 0);
        }
    }
}
