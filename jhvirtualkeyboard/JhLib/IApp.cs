// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************


namespace JhLib
{
    #region Doc-stuff
    /// <summary>
    /// This namespace encompasses the base-level library of general-purpose C# stuff upon which the other libaries and applications depend.
    /// James W. Hurst
    /// </summary>
    public static class NamespaceDoc
    {
        // This class exists only to provide a hook for adding summary documentation
        // for the JhLib namespace.
    }
    #endregion

    #region interface IApp
    /// <summary>
    /// Providing this interface helps contribute a sort of 'aspect-oriented programming' structure
    /// by interjecting an Interlocution and other subsystems across the board.
    /// </summary>
    public interface IApp : IAppVersion
    {
        /// <summary>
        /// Get the pathname of this application's executable.
        /// </summary>
        string ExecutablePath { get; }

        /// <summary>
        /// Provide the name of this application as it would be displayed to the user (in other than abbreviated form).
        /// </summary>
        string ProductName { get; }

        /// <summary>
        /// Provide an abbreviated name of this application for display in dialog-window titlebars, etc.
        /// </summary>
        string ProductNameShort { get; }

        /// <summary>
        /// Get the string to use to identify this software product to the user,
        /// for use - for example - as a prefix in the titlebar for all windows and dialogs of this application,
        /// or for Windows Event Log entries.
        /// This implements the mandated standard which is the vendor followed by the application's name.
        /// To set the titlebar you would append a separator (such as a colon and a space) before your specific title information.
        /// </summary>
        string ProductIdentificationPrefix { get; }

        /// <summary>
        /// Provide access to the IInterlocution object that provides us with the user-notification services.
        /// </summary>
        IInterlocution Interlocution { get; }

        /// <summary>
        /// Get the filesystem-path for storing this application's data, such as would be common across local users of this desktop computer.
        /// </summary>
        string ThisApplicationDataFolder { get; }

        /// <summary>
        /// Get the username of the user who is currently running this program.
        /// </summary>
        string Username { get; }

        /// <summary>
        /// Get the (short, one-word version of) name of the maker or owner of this software,
        /// as would be used for the first part of the CommonDataPath, and the window title-bar prefix.
        /// </summary>
        string VendorName { get; }

        /// <summary>
        /// Get or set the default logger to be used in this application.
        /// </summary>
        //TODO: I'm still deciding how to handle this. I want JhLib to be a bottom-level library - not dependent on anything,
        //      but I also sort of want the logger to also be at the bottom-level. jh
        //ILog Logger { get; set; }
    }
    #endregion interface IApp
}
