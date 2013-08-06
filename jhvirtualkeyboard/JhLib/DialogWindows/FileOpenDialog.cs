// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Diagnostics;
using System.Windows.Forms;


namespace JhLib.DialogWindows
{
   public class FileOpenDialog
   {
      #region default constructor
      /// <summary>
      /// Default constructor
      /// </summary>
      public FileOpenDialog()
      {
         _FileDialog = new System.Windows.Forms.OpenFileDialog();
         _FileDialog.CheckFileExists = true;
      }
      #endregion

      #region InitialDirectory
      /// <summary>
      /// Get or set the initial folder displayed by the dialog box.
      /// </summary>
      public string InitialDirectory
      {
         get { return _FileDialog.InitialDirectory; }
         set { _FileDialog.InitialDirectory = value; }
      }
      #endregion

      #region Title
      /// <summary>
      /// Get or set the open-file dialogbox title
      /// </summary>
      public string Title
      {
         get { return _FileDialog.Title; }
         set { _FileDialog.Title = value; }
      }
      #endregion

      #region SelectedFilename
      /// <summary>
      /// Gets or sets the file name selected by the user.
      /// </summary>
      public string SelectedFilename
      {
         get { return _FileDialog.FileName; }
         set { _FileDialog.FileName = value; }
      }
      #endregion

      #region ShowDialog
      /// <summary>
      /// Invokes a 'common dialog box' with a default owner-window.
      /// </summary>
      /// <returns>a TaskDialogResult that maps exactly what a Forms.FileDialog would return</returns>
      public JhDialogResult ShowDialog()
      {
         System.Windows.Forms.DialogResult dr;
         //cbl  I don't see that this distinction makes any difference at all.
         if (UseMainWindowAsOwner)
         {
            IWin32Window win32dow = GetMainWindowAsIWin32Window();
            dr = _FileDialog.ShowDialog(win32dow);
         }
         else
         {
            dr = _FileDialog.ShowDialog();
         }
         return JhMessageBox.ResultFrom(dr);
      }
      #endregion

      #region UseMainWindowAsOwner
      /// <summary>
      /// Get or set the flag that indicates whether we use the application's main window
      /// for the 'parent' window for this dialog.
      /// </summary>
      private bool UseMainWindowAsOwner { get; set; }

      #endregion

      #region Internal Implementation

      #region GetMainWindowAsIWin32Window
      /// <summary>
      /// Retrieves the main window for the current process and returns it as a IWin32Window
      /// </summary>
      /// <returns></returns>
      public IWin32Window GetMainWindowAsIWin32Window()
      {
         string sFriendlyName = AppDomain.CurrentDomain.FriendlyName;
         // Get process collection by the application name without extension (.exe)
         Process[] pro = Process.GetProcessesByName(sFriendlyName.Substring(0, sFriendlyName.LastIndexOf('.')));
         // Get main window handle pointer and wrap into IWin32Window
         return new WindowWrapper(pro[0].MainWindowHandle);
      }
      #endregion

      /// <summary>
      /// This embedded OpenFileDialog is used to perform all of the actual functionality.
      /// </summary>
      private System.Windows.Forms.OpenFileDialog _FileDialog;
      //private System.Windows.Forms.FileDialog _FileDialog;

      #endregion // Internal Implementation
   }

   public class WindowWrapper : System.Windows.Forms.IWin32Window
   {
       public WindowWrapper(IntPtr handle)
       {
           _hwnd = handle;
       }

       public IntPtr Handle
       {
           get { return _hwnd; }
       }

       #region GetMainWindowAsIWin32Window
       /// <summary>
       /// Retrieves the main window for the current process and returns it as a IWin32Window
       /// </summary>
       /// <returns></returns>
       public static WindowWrapper GetMainWindowAsIWin32Window()
       {
           //Get FriendlyName from Application Domain
           string sFriendlyName = AppDomain.CurrentDomain.FriendlyName;

           //Get process collection by the application name without extension (.exe)
           Process[] processes = Process.GetProcessesByName(sFriendlyName.Substring(0, sFriendlyName.LastIndexOf('.')));

           // Get main window handle pointer and wrap into IWin32Window
           return new WindowWrapper(processes[0].MainWindowHandle);
       }
       #endregion

       private IntPtr _hwnd;
   }
}
