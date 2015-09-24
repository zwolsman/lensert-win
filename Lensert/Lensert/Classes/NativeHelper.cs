using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lensert
{
    static class NativeHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public readonly int Left;
            public readonly int Top;
            public readonly int Right;
            public readonly int Bottom;

            public Rectangle ToRectangle()
            {
                return new Rectangle(Left, Top, Right - Left, Bottom - Top);
            }
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
        private static extern IntPtr CreateCompatibleDC(IntPtr dcHandle);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteDC(IntPtr dcHandle);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr objectHandle);

        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr dcHandle, IntPtr objectHandle);

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

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr handle);

        public static Rectangle GetForegroundWindowAea()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd == IntPtr.Zero)
                return Rectangle.Empty;

            RECT rect;
            if (!GetWindowRect(hwnd, out rect))
                throw new Win32Exception(GetLastError());

            return rect.ToRectangle();
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
                if (!IsWindow(handle) || !IsWindowVisible(handle) || GetWindowTextLength(handle) < 1)
                    return true;

                RECT rect;
                if (!GetWindowRect(handle, out rect))
                    throw new Win32Exception(GetLastError()); //wondering why this failes lol

                list.Add(rect.ToRectangle());

                return true;
            }, IntPtr.Zero);

            return list;
        }
    }
}