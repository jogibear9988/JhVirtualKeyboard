// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************


namespace JhLib
{
    #region interface IAppVersion
    /// <summary>
    /// An application that implements IAppVersion provides a method for getting it's version.
    /// This provides an aspect-oriented way for your applic to supply it's version,
    /// and for low-level library routines to get it without having to reference your assembly.
    /// That's it. All it does. Get the fracn version.
    /// </summary>
    public interface IAppVersion
    {
        /// <summary>
        /// Return this application's version, as a simple string.
        /// </summary>
        string Version { get; }
    }
    #endregion
}
