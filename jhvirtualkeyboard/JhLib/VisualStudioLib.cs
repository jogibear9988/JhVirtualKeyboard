// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;


namespace JhLib
{
    public class VisualStudioLib
    {
        // http://stackoverflow.com/questions/2525457/automating-visual-studio-with-envdte
        // http://stackoverflow.com/questions/2651617/is-it-possible-to-programmatically-clear-the-ouput-window-in-visual-studio
        // http://stackoverflow.com/questions/2391473/can-the-visual-studio-debug-output-window-be-programatically-cleared

        public static void VstClearOutputWindow()
        {
            if (!Debugger.IsAttached)
            {
                return;
            }

            //Application.DoEvents();  // This is for Windows.Forms.
            // This delay to get it to work. Unsure why. See http://stackoverflow.com/questions/2391473/can-the-visual-studio-debug-output-window-be-programatically-cleared
            Thread.Sleep(1000);
            // In VS2008 use EnvDTE80.DTE2
            EnvDTE.DTE ide = (EnvDTE.DTE)Marshal.GetActiveObject("VisualStudio.DTE.10.0");
            ide.ExecuteCommand("Edit.ClearOutputWindow", "");
            Marshal.ReleaseComObject(ide);

//            EnvDTE80.DTE2 ide = (EnvDTE80.DTE2)Marshal.GetActiveObject("VisualStudio.DTE.10.0");
//            ide.ExecuteCommand("Edit.ClearOutputWindow", "");
//            Marshal.ReleaseComObject(ide);
        }
    }
}
