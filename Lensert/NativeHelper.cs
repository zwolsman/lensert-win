using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Lensert
{
    internal static class NativeHelper
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
        private static extern bool DeleteObject(IntPtr objectHandle);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr dcHandle, IntPtr objectHandle);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr dcHandle, int width, int height);

        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr destinationDcHandle, int destinationX, int destinationY, int width, int height, IntPtr sourceDcHandle, int sourceX, int sourceY, CopyPixelOperation rasterOperation);
        
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr handle);

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr handle);

        [DllImport("dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out RECT pvAttribute, int cbAttribute);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("kernel32")]
        static extern long WritePrivateProfileString(string section, string key, string value, string path);

        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string section, string key, string standardValue, StringBuilder value, int len, string path);

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117
        }

        static NativeHelper()
        {
            var factor = GetScalingFactor();
            var virtualScreen = SystemInformation.VirtualScreen;

            var width = (int)Math.Ceiling(factor * virtualScreen.Width);
            var height = (int)Math.Ceiling(factor * virtualScreen.Height);

            UnscaledBounds = new Rectangle(Point.Empty, new Size(width, height));
        }

        public static Rectangle UnscaledBounds { get; }

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
            public Point ptMinPosition;
            public Point ptMaxPosition;
            public Rectangle rcNormalPosition;
        }
        
        const uint SW_MAXIMIZE = 3;
        
        private static int GetWindowState(IntPtr handle)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(handle, ref placement);
            return placement.showCmd;
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
        
        public static void WriteValueToIni<T>(string path, string key, T value, string section, bool comment = false)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            var strValue = converter.ConvertToString(value);
                
            WritePrivateProfileString(section, comment ? $";{key}" : key, strValue, path);
        }

        public static T ParseValueFromIni<T>(string path, string key, string section)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                var builder = new StringBuilder(255);
                GetPrivateProfileString(section, key, "", builder, builder.Capacity, path);

                var value = builder.ToString();
                return string.IsNullOrEmpty(value)
                    ? default(T)
                    : (T)converter.ConvertFromString(value);
            }
            catch
            {
                return default(T);
            }
        }

        private static float GetScalingFactor()
        {
            var g = Graphics.FromHwnd(IntPtr.Zero);
            var desktop = g.GetHdc();
            var screenVertres = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            var desktopVertres = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            return desktopVertres / (float)screenVertres;
        }
    }
}