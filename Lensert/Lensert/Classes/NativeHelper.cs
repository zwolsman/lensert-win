using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lensert
{
    static class NativeHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            private readonly int Left;
            private readonly int Top;
            private readonly int Right;
            private readonly int Bottom;

            public Rectangle ToRectangle() => new Rectangle(Left, Top, Right - Left, Bottom - Top);
        }

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("kernel32.dll")]
        private static extern int GetLastError();

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr windowHandle);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        private static extern bool ReleaseDC(IntPtr windowHandle, IntPtr dcHandle);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr windowHandle);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr dcHandle);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteDC(IntPtr dcHandle);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr objectHandle);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr dcHandle, IntPtr objectHandle);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr dcHandle, int width, int height);

        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr destinationDcHandle, int destinationX, int destinationY, int width, int height, IntPtr sourceDcHandle, int sourceX, int sourceY, CopyPixelOperation rasterOperation);
        
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr handle);

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr handle);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr handle);

        [DllImport("dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out RECT pvAttribute, int cbAttribute);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, string lp);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("uxtheme", CharSet = CharSet.Unicode)]
        public extern static int SetWindowTheme(IntPtr hWnd, string textSubAppName, string textSubIdList);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern IntPtr DefWindowProc(IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, WinProc dwNewLong);

        private delegate IntPtr WinProc(IntPtr hWnd, int Msg, int wParam, int lParam);

        const int GWL_WNDPROC = -4;

        private const int DWMWA_EXTENDED_FRAME_BOUNDS = 9;
        public const int LVM_FIRST = 0x1000;
        public const int LVM_SETEXTENDEDLISTVIEWSTYLE = LVM_FIRST + 54;
        public const int LVS_EX_DOUBLEBUFFER = 0x00010000;


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        
        const uint SW_HIDE = 0;
        const uint SW_SHOWNORMAL = 1;
        const uint SW_NORMAL = 1;
        const uint SW_SHOWMINIMIZED = 2;
        const uint SW_SHOWMAXIMIZED = 3;
        const uint SW_MAXIMIZE = 3;
        const uint SW_SHOWNOACTIVATE = 4;
        const uint SW_SHOW = 5;
        const uint SW_MINIMIZE = 6;
        const uint SW_SHOWMINNOACTIVE = 7;
        const uint SW_SHOWNA = 8;
        const uint SW_RESTORE = 9;
        
        private static int GetWindowState(IntPtr handle)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(handle, ref placement);
            return placement.showCmd;
        }
        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            var result = DefWindowProc(hwnd, msg, wParam, lParam).ToInt32();

            return new IntPtr(result);
        }

        public static void HookWindow(IntPtr hwnd)
        {
            SetWindowLong(hwnd, -4, WndProc);
        }

        private static IntPtr WndProc(IntPtr hWnd, int Msg, int wParam, int lParam)
        {
            throw new NotImplementedException();
        }

        public static Rectangle GetForegroundWindowAea()
        {
            var handle = GetForegroundWindow();
            if (handle == IntPtr.Zero)
                return Rectangle.Empty;

            return GetWindowRectangle(handle);
        }
        
        public static Bitmap TakeScreenshot(Rectangle area)
        {
            var handleDesktopWindow = GetDesktopWindow();
            var handleSource = GetWindowDC(handleDesktopWindow);
            var handleDestination = CreateCompatibleDC(handleSource);
            var handleBitmap = CreateCompatibleBitmap(handleSource, area.Width, area.Height);
            var handleOldBitmap = SelectObject(handleDestination, handleBitmap);

            BitBlt(handleDestination, 0, 0, area.Width, area.Height, handleSource, area.X, area.Y, CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt);

            var screenshot = Image.FromHbitmap(handleBitmap);
            SelectObject(handleDestination, handleOldBitmap);
            DeleteObject(handleBitmap);
            DeleteDC(handleDestination);
            ReleaseDC(handleDesktopWindow, handleSource);

            return screenshot;
        }

        public static IEnumerable<Rectangle> GetWindowDimensions()
        {
            var list = new List<Rectangle>();
            EnumWindows((handle, lparam) =>
            {
                if (GetClassName(handle) != "Shell_TrayWnd" && (!IsWindow(handle) || !IsWindowVisible(handle)))
                    return true;
                
                var rectangle = GetWindowRectangle(handle);

                if (GetWindowState(handle) == SW_MAXIMIZE)
                {
                    var differenceX = SystemInformation.WorkingArea.X - rectangle.X;
                    var differenceY = SystemInformation.WorkingArea.Y - rectangle.Y;
                     if (differenceX > 0)
                     {
                         rectangle.Width -= differenceX * 2;
                         rectangle.X += differenceX;
                     }
                     if (differenceY > 0)
                     {
                        rectangle.Height -= differenceY * 2;
                        rectangle.Y += differenceY;
                    }
                }
                list.Add(rectangle);

                return true;
            }, IntPtr.Zero);

            return list;
        }

        private static string GetClassName(IntPtr hWnd)
        {
            var sb = new StringBuilder(256);
            GetClassName(hWnd, sb, sb.Capacity);

            return sb.ToString();
        }

        private static Rectangle GetWindowRectangle(IntPtr handle)
        {   //Thank you: http://stackoverflow.com/questions/16484894/form-tells-wrong-size-on-windows-8-how-to-get-real-size
            RECT rect;
            if (Environment.OSVersion.Version.Major < 6)            //before Windows 8 GetWindowRect works
            {
                GetWindowRect(handle, out rect);
                return rect.ToRectangle();
            }

            var result = DwmGetWindowAttribute(handle, DWMWA_EXTENDED_FRAME_BOUNDS, out rect, Marshal.SizeOf(typeof(RECT)));
            if (result >= 0)
                return rect.ToRectangle();

            GetWindowRect(handle, out rect);
            return rect.ToRectangle();
        }
    }
}