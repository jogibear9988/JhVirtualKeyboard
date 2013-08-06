// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Runtime.InteropServices;  // StructLayout, DllImport
using System.Windows;
using System.Windows.Interop;  // WindowInteropHelper
using System.Windows.Media;  // Brushes


namespace JhLib
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MARGINS
    {
        public MARGINS(Thickness t)
        {
            Left = (int) t.Left;
            Right = (int) t.Right;
            Top = (int) t.Top;
            Bottom = (int) t.Bottom;
        }

        public int Left;
        public int Right;
        public int Top;
        public int Bottom;
    }

    /// <summary>
    /// Utility used for extended the Aero glass-transparency effect into a Window's client area.
    /// This is taken from Nathan, WPF 4 Unleashed, p251
    /// </summary>
    public class GlassHelper
    {
        [DllImport("dwmapi.dll", PreserveSig = false)]
        private static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInsert);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        private static extern bool DwmIsCompositionEnabled();

        public static bool ExtendGlassFrame(Window window, Thickness margin)
        {
            if (!DwmIsCompositionEnabled())
            {
                return false;
            }

            IntPtr hwnd = new WindowInteropHelper(window).Handle;
            if (hwnd == IntPtr.Zero)
            {
                throw new InvalidOperationException("The Window must be shown before extending glass.");
            }

            // Set the background to transparent from both the WPF and Win32 perspectives.
            window.Background = Brushes.Transparent;
            HwndSource.FromHwnd(hwnd).CompositionTarget.BackgroundColor = Colors.Transparent;

            MARGINS margins = new MARGINS(margin);
            DwmExtendFrameIntoClientArea(hwnd, ref margins);
            return true;
        }
    }
}
