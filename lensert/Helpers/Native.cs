using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Lensert.Helpers
{
    internal static class Native
    {
        private const int DEVICE_CAP_VERTES = 10;
        private const int DEVICE_CAP_DESKTOPVERTES = 117;
        private const int DWMWA_EXTENDED_FRAME_BOUNDS = 9;
        private const uint SW_MAXIMIZE = 3;

        static Native()
        {
            var factor = GetScalingFactor();
            var virtualScreen = SystemInformation.VirtualScreen;

            var width = (int) Math.Ceiling(factor*virtualScreen.Width);
            var height = (int) Math.Ceiling(factor*virtualScreen.Height);

            UnscaledBounds = new Rectangle(virtualScreen.Location, new Size(width, height));
        }

        public static Rectangle UnscaledBounds { get; }

        public static Rectangle GetForegroundWindowAea()
        {
            var handle = GetForegroundWindow();
            return handle == IntPtr.Zero
                ? Rectangle.Empty
                : GetWindowRectangle(handle);
        }

        public static Bitmap TakeCurrentCursorsWindowScreenshot()
        {
            var handle = GetTopCursorsWindowHandle();

            NativeRect rect;
            GetWindowRect(handle, out rect);

            var rectangle = rect.ToRectangle();
            var bitmap = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format32bppArgb);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                var hdcBitmap = graphics.GetHdc();
                PrintWindow(handle, hdcBitmap, 0);

                graphics.ReleaseHdc(hdcBitmap);
            }

            return bitmap;
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
                if ((GetClassName(handle) != "Shell_TrayWnd") && (!IsWindow(handle) || !IsWindowVisible(handle)))
                    return true;

                var rectangle = GetWindowRectangle(handle);

                if (GetWindowState(handle) == SW_MAXIMIZE)
                {
                    var differenceX = SystemInformation.WorkingArea.X - rectangle.X;
                    var differenceY = SystemInformation.WorkingArea.Y - rectangle.Y;
                    if (differenceX > 0)
                    {
                        rectangle.Width -= differenceX*2;
                        rectangle.X += differenceX;
                    }
                    if (differenceY > 0)
                    {
                        rectangle.Height -= differenceY*2;
                        rectangle.Y += differenceY;
                    }
                }
                list.Add(rectangle);

                return true;
            }, IntPtr.Zero);

            return list;
        }

        public static void WriteValueToIni<T>(string path, string key, T value, string section)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            var strValue = converter.ConvertToString(value);

            WritePrivateProfileString(section, key, strValue, path);
        }

        public static string ReadValueFromIni(string path, string key, string section)
        {
            var builder = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", builder, builder.Capacity, path);

            return builder.ToString();
        }

        public static T ParseValueFromIni<T>(string path, string key, string section)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                var value = ReadValueFromIni(path, key, section);
                return string.IsNullOrEmpty(value)
                    ? default(T)
                    : (T) converter.ConvertFromString(value);
            }
            catch
            {
                return default(T);
            }
        }

        private static IntPtr GetTopCursorsWindowHandle()
        {
            NativePoint point;
            GetCursorPos(out point);

            var handle = WindowFromPoint(point);

            IntPtr parentHandle;
            while ((parentHandle = GetParent(handle)) != IntPtr.Zero)
                handle = parentHandle;

            return handle;
        }

        private static int GetWindowState(IntPtr handle)
        {
            var placement = new WindowPlacement();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(handle, ref placement);
            return placement.showCmd;
        }

        private static string GetClassName(IntPtr handle)
        {
            var sb = new StringBuilder(256);
            GetClassName(handle, sb, sb.Capacity);

            return sb.ToString();
        }

        private static Rectangle GetWindowRectangle(IntPtr handle)
        { //Thank you: http://stackoverflow.com/questions/16484894/form-tells-wrong-size-on-windows-8-how-to-get-real-size
            NativeRect rect;
            if (Environment.OSVersion.Version.Major < 6) //before Windows 8 GetWindowRect works
            {
                GetWindowRect(handle, out rect);
                return rect.ToRectangle();
            }

            var result = DwmGetWindowAttribute(handle, DWMWA_EXTENDED_FRAME_BOUNDS, out rect, Marshal.SizeOf(typeof(NativeRect)));
            if (result >= 0)
                return rect.ToRectangle();

            GetWindowRect(handle, out rect);
            return rect.ToRectangle();
        }

        private static float GetScalingFactor()
        {
            var g = Graphics.FromHwnd(IntPtr.Zero);
            var desktop = g.GetHdc();
            var screenVertres = GetDeviceCaps(desktop, DEVICE_CAP_VERTES);
            var desktopVertres = GetDeviceCaps(desktop, DEVICE_CAP_DESKTOPVERTES);

            return desktopVertres/(float) screenVertres;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr handle, out NativeRect lpRect);

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
        private static extern int DwmGetWindowAttribute(IntPtr handle, int dwAttribute, out NativeRect pvAttribute, int cbAttribute);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr handle, StringBuilder lpClassName, int nMaxCount);

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string value, string path);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string standardValue, StringBuilder value, int len, string path);

        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowPlacement(IntPtr handle, ref WindowPlacement lpwndpl);

        [DllImport("user32.dll")]
        private static extern bool PrintWindow(IntPtr handle, IntPtr hdcBlt, int nFlags);

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out NativePoint lpPoint);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern IntPtr GetParent(IntPtr handle);

        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(NativePoint point);

        [StructLayout(LayoutKind.Sequential)]
        private struct NativeRect
        {
            private readonly int Left;
            private readonly int Top;
            private readonly int Right;
            private readonly int Bottom;

            public Rectangle ToRectangle() => new Rectangle(Left, Top, Right - Left, Bottom - Top);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NativePoint
        {
            private readonly int X;
            private readonly int Y;

            public Point ToPoitn() => new Point(X, Y);
        }

        private delegate bool EnumWindowsProc(IntPtr handle, IntPtr lParam);

        private struct WindowPlacement
        {
            public int length;
            public int flags;
            public int showCmd;
            public Point ptMinPosition;
            public Point ptMaxPosition;
            public Rectangle rcNormalPosition;
        }
    }
}
