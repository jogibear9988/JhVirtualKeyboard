// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.IO;
using System.Reflection;
using System.Windows;


namespace JhLib
{
    #region class JhApp
    /// <summary>
    /// class JhApp implements IApp and provides a few convenient facilities that you can acquire if you subclass your application from it.  jh
    /// </summary>
    public class JhApp : Application, IApp
    {
        #region The
        /// <summary>
        /// Get a reference to the JhApp object - the parent class of our application. Same as the 'Current' method.
        /// </summary>
        public static JhApp The
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return Application.Current as JhApp; }
        }
        #endregion

        #region Current
        /// <summary>
        /// Get a reference to the JhApp object - the parent class of our application. Same as the 'The' method.
        /// </summary>
        public static new JhApp Current
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return (JhApp)Application.Current; }
        }
        #endregion

        #region ExecutablePath
        /// <summary>
        /// Get the pathname of this application's executable.
        /// </summary>
        public string ExecutablePath
        {
            get
            {
                if (String.IsNullOrEmpty(_sExecutablePath))
                {
                    Assembly assembly = Application.Current.GetType().Assembly;
                    _sExecutablePath = assembly.Location;
                }
                return _sExecutablePath;
            }
        }

        private string _sExecutablePath;

        #endregion

        #region GetProgramName
        /// <summary>
        /// Return the name of the currently-executing program. If this is an Application that implements IApp,
        /// then use it's ProductNameShort function, otherwise get it from the assembly. This applies to .Net or Silverlight.
        /// </summary>
        /// <returns>The program (assembly)-name as a string, or String.Empty if unable to determine it</returns>
        public static string GetProgramName()
        {
            // We test for null, not for an empty string - so that this test is only done once.
            if (_programName == null)
            {
                // If the applic implements IApp, then use that to get a (short) name for the application.
                IApp iApp = Application.Current as IApp;
                if (iApp != null)
                {
                    _programName = iApp.ProductNameShort;
                }
                else
                {
                    // This is the next-best method that I know of, to get the name of the running application.
#if SILVERLIGHT
                    Application thisApp = Application.Current;
                    string className = thisApp.ToString();
                    _programName = WithoutAtEnd(className, ".App");
#else
                    Assembly thisAssembly = Assembly.GetEntryAssembly();
                    if (thisAssembly != null)
                    {
                        AssemblyName thisAssemblyName = thisAssembly.GetName();
                        _programName = thisAssemblyName.Name;
                    }
                    else  // perhaps this is a unit-test that is running?
                    {
                        string friendlyName = AppDomain.CurrentDomain.FriendlyName;
                        int iColon = friendlyName.IndexOf(':');
                        if (iColon >= 0)
                        {
                            string result = friendlyName.Substring(iColon + 1);
                            if (!String.IsNullOrWhiteSpace(result))
                            {
                                _programName = result.Trim();
                            }
                        }
                    }
#endif
                }
            }
            if (_programName == null)
            {
                _programName = String.Empty;
            }
            return _programName;
        }

        /// <summary>
        /// The name (reasonably shortened) of the currently executing program.
        /// </summary>
        private static string _programName;

        #endregion GetProgramName

        #region Interlocution
        /// <summary>
        /// Get the object that will be used as the application-wide basis for user-notification.
        /// </summary>
        public virtual IInterlocution Interlocution
        {
            get
            {
                // Lock the object so we won't be competing with other threads at this moment..
                lock (_syncRoot)
                {
                    if (_interlocution == null)
                    {
                        _interlocution = JhMessageBox.GetNewInstance(this);
                    }
                    return _interlocution;
                }
            }
        }

        private IInterlocution _interlocution;
        /// <summary>
        /// syncronization root object, for locking
        /// </summary>
        private static readonly object _syncRoot = new object();

        #endregion

        #region ProductName
        /// <summary>
        /// Provide the name of this application as it would be displayed to the user in window titlebars, etc.
        /// Unless overridden, this simply yields the assembly name.
        /// Note: You should override this if you don't want reflection to be used to get this value.
        /// </summary>
        public virtual string ProductName
        {
            get
            {
                // Since the name hasn't been explicitly set anywhere (or this method overridden), get it from the assembly.
                if (_applicationName == null)
                {
#if SILVERLIGHT
                    Application thisApp = Application.Current;
                    string className = thisApp.ToString();
                    _applicationName = StringLib.WithoutAtEnd(className, ".App");
#else
                    // This is the implementation I found in a demo-app for TX Text Control.
                    _applicationName = ((AssemblyProductAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyProductAttribute))).Product;
                    // This was my own, earlier implementation.
                    //Assembly assembly = Application.Current.GetType().Assembly;
                    //_applicationName = assembly.GetName().Name;
#endif
                }
                return _applicationName;
            }
        }
        private string _applicationName;

        #endregion

        #region ProductNameShort
        /// <summary>
        /// Provide the abbreviated name of this application for display in titlebars, etc.
        /// Unless overridden, this yields the same as the "ProductName" property.
        /// </summary>
        public virtual string ProductNameShort
        {
            get
            {
                return this.ProductName;
            }
        }
        #endregion

        #region ProductIdentificationPrefix
        /// <summary>
        /// Get the string to use to identify this software product to the user,
        /// for use - for example - as a prefix in the titlebar for all windows and dialogs of this application,
        /// or for Windows Event Log entries.
        /// This implements the mandated standard which is the vendor followed by the application's name.
        /// To set the titlebar you would append a separator (such as a colon and a space) before your specific title information.
        /// </summary>
        public virtual string ProductIdentificationPrefix
        {
            get
            {
                return this.VendorName + " " + this.ProductNameShort;
            }
        }
        #endregion

        #region ThisApplicationDataFolder
        /// <summary>
        /// Get the filesystem-path for storing this application's data, such as would be common across local users of this desktop computer.
        /// This value is already specific to this vendor and this product.
        /// </summary>
        public virtual string ThisApplicationDataFolder
        {
            get
            {
                if (String.IsNullOrEmpty(_sThisApplicationDataFolder))
                {
                    string partUnderSpecialFolder = Path.Combine(this.VendorName, this.ProductName);
                    _sThisApplicationDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), partUnderSpecialFolder);
                }
                return _sThisApplicationDataFolder;
            }
        }

        private string _sThisApplicationDataFolder;

        #endregion

        #region Username
        /// <summary>
        /// Get the name of the user who is currently running this program.
        /// For Silverlight, you need to override this to provide an actual name - otherwise it simply returns a question-mark.
        /// </summary>
        public virtual string Username
        {
            get
            {
#if SILVERLIGHT
                return "?";
#else
                return Environment.UserName;
#endif
            }
        }
        #endregion

        #region VendorName
        /// <summary>
        /// Get the (short, one-word version of) name of the maker or owner of this software,
        /// as would be used for the first part of the CommonDataPath, and the window title-bar prefix.
        /// This is "DesignForge" as returned by this method on the JhApp class.
        /// </summary>
        public virtual string VendorName
        {
            get { return "DesignForge"; }
        }
        #endregion

        #region Version
        /// <summary>
        /// Return the version of this executing program, by getting it from the executing assembly.
        /// This applies to both .Net and Silverlight.
        /// </summary>
        /// <returns>The version (of the program or assembly) as a string</returns>
        public virtual string Version
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_programVersion))
                {
                    var assembly = Assembly.GetExecutingAssembly();
#if SILVERLIGHT
                    object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                    if (attributes != null && attributes.Length > 0)
                    {
                        var fileVersionAttribute = (AssemblyFileVersionAttribute)attributes[0];
                        _programVersion = fileVersionAttribute.Version;
                    }
#else
                    _programVersion = assembly.GetName().Version.ToString();
#endif
                }
                return _programVersion;
            }
        }

        /// <summary>
        /// This stores the version-number (as a string) of the subject-program.
        /// </summary>
        private string _programVersion;

        #endregion GetVersion

        #region The display-update event

        //public event DisplayCultureUpdateEventHandler DisplayCultureChanged;

        //public void RaiseDisplayCultureUpdateEvent(CultureInfo newCulture)
        //{
        //    DisplayCultureUpdateEventArgs args = new DisplayCultureUpdateEventArgs(newCulture);
        //    DisplayCultureUpdateEvent(this, args);
        //}

        #endregion The display-update event
    }
    #endregion class JhApp
}
