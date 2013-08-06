// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Markup;

[assembly: XmlnsDefinition("http://schemas.designforge.com/windows", "JhLib")]

// Usage:
//
// To make your WPF desktop application remember it's position and size on the screen between invocations,
// include this XML namespace-declaration and attribute within your Window's XAML:
//
// <Window x:Class="MyKicknApplication.MainWindow"
//     xmlns:jh="clr-namespace:JhLib;assembly=JhLib"
//     jh:WindowSettings.Save="True"
//
// To only save the Window position and not the size, use:
//
//     jh:WindowSettings.SavePosition="True"
//
// To not save the Window position, but save the size, use:
//
//     jh:WindowSettings.SaveSize="True"
//
// James W. Hurst,
// JamesH@Designforge.com

namespace JhLib
{
    /// <summary>
    /// Persists a Window's Size, Location and WindowState to UserScopeSettings 
    /// </summary>
    public class WindowSettings
    {
        #region Constructors

        // Provided for VS2008 since that does not support default parameter values.
        public WindowSettings(Window window)
        {
            _window = window;
            _isSavingSize = false;
            _isSavingPosition = true;
            // Use the class-name of the given Window, minus the namespace prefix, as the instance-key.
            // This is so that we have a distinct setting for different windows.
            string sWindowType = window.GetType().ToString();
            int iPos = sWindowType.LastIndexOf('.');
            string sKey;
            if (iPos > 0)
            {
                sKey = sWindowType.Substring(iPos + 1);
            }
            else
            {
                sKey = sWindowType;
            }
            _sInstanceSettingsKey = sKey;
        }

        public WindowSettings(Window window, bool isSavingPosition, bool isSavingSize, string keyExtensionValue = null)
        {
            _window = window;
            _isSavingPosition = isSavingPosition;
            _isSavingSize = isSavingSize;
            // Use the class-name of the given Window, minus the namespace prefix, as the instance-key.
            // This is so that we have a distinct setting for different windows.
            string sWindowType = window.GetType().ToString();
            int iPos = sWindowType.LastIndexOf('.');
            string sKey;
            if (iPos > 0)
            {
                sKey = sWindowType.Substring(iPos + 1);
            }
            else
            {
                sKey = sWindowType;
            }
            if (keyExtensionValue == null)
            {
                _sInstanceSettingsKey = sKey;
            }
            else
            {
                _sInstanceSettingsKey = sKey + keyExtensionValue;
            }
        }

        #endregion

        #region WindowApplicationSettings helper class
        public class WindowApplicationSettings : ApplicationSettingsBase
        {
            public WindowApplicationSettings(WindowSettings windowSettings, string sInstanceKey)
                : base(sInstanceKey)
            {
            }

            [UserScopedSetting]
            public Rect Location
            {
                get
                {
                    try
                    {
                        if (this["Location"] != null)
                        {
                            return ((Rect)this["Location"]);
                        }
                    }
                    catch (System.Configuration.ConfigurationErrorsException x)
                    {
                        // I added this for diagnosing a failure. See http://forums.msdn.microsoft.com/en-US/vbgeneral/thread/41cfc8e2-c7f4-462d-9a43-e751500deb0a
                        Console.WriteLine("Oh geez - you got an ConfigurationErrorsException within JhLib.WindowSettings.Location.Get. Something may be wrong with your app config file!" + x.ToString());

                        System.Configuration.Configuration exeConfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                        Console.WriteLine("  exe config FilePath is " + exeConfig.FilePath);

                        System.Configuration.Configuration localConfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal);
                        Console.WriteLine("  local config FilePath is " + localConfig.FilePath);

                        System.Configuration.Configuration roamingConfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.PerUserRoaming);
                        Console.WriteLine("  roaming config FilePath is " + roamingConfig.FilePath);
                    }
                    return Rect.Empty;
                }
                set
                {
                    this["Location"] = value;
                }
            }

            [UserScopedSetting]
            public WindowState WindowState
            {
                get
                {
                    try
                    {
                        if (this["WindowState"] != null)
                        {
                            return (WindowState)this["WindowState"];
                        }
                    }
                    catch (System.Configuration.ConfigurationErrorsException x)
                    {
                        // I added this for diagnosing a failure. See http://forums.msdn.microsoft.com/en-US/vbgeneral/thread/41cfc8e2-c7f4-462d-9a43-e751500deb0a
                        Console.WriteLine("Oh geez - you got an ConfigurationErrorsException within JhLib.WindowSettings.WindowState.Get. Something may be wrong with your app config file! " + x.Message);

                        System.Configuration.Configuration exeConfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                        Console.WriteLine("  exe config FilePath is " + exeConfig.FilePath);

                        System.Configuration.Configuration localConfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal);
                        Console.WriteLine("  local config FilePath is " + localConfig.FilePath);

                        System.Configuration.Configuration roamingConfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.PerUserRoaming);
                        Console.WriteLine("  roaming config FilePath is " + roamingConfig.FilePath);
                    }
                    return WindowState.Normal;
                }
                set
                {
                    this["WindowState"] = value;
                }
            }
        }
        #endregion

        #region Attached "Save" Property Implementation
        /// <summary>
        /// Register the "Save" attached property and the "OnSaveInvalidated" callback 
        /// </summary>
        public static readonly DependencyProperty SaveProperty
           = DependencyProperty.RegisterAttached("Save", typeof(bool), typeof(WindowSettings),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSaveInvalidated)));

        public static void SetSave(DependencyObject dependencyObject, bool enabled)
        {
            dependencyObject.SetValue(SaveProperty, enabled);
        }

        /// <summary>
        /// Called when Save is changed on an object.
        /// </summary>
        private static void OnSaveInvalidated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Window window = dependencyObject as Window;
            if (window != null)
            {
                if ((bool)e.NewValue)
                {
                    WindowSettings settings = new WindowSettings(window, true, true);
                    settings.Attach();
                }
            }
        }
        #endregion

        #region Attached "SavePosition" Property Implementation
        /// <summary>
        /// Register the "SavePosition" attached property and the "OnSavePositionInvalidated" callback 
        /// </summary>
        public static readonly DependencyProperty SavePositionProperty
           = DependencyProperty.RegisterAttached("SavePosition", typeof(bool), typeof(WindowSettings),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSavePositionInvalidated)));

        public static void SetSavePosition(DependencyObject dependencyObject, bool enabled)
        {
            dependencyObject.SetValue(SavePositionProperty, enabled);
        }

        /// <summary>
        /// Called when SavePosition is changed on an object.
        /// </summary>
        private static void OnSavePositionInvalidated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Window window = dependencyObject as Window;
            if (window != null)
            {
                if ((bool)e.NewValue)
                {
                    WindowSettings settings = new WindowSettings(window, true, false);
                    settings.Attach();
                }
            }
        }
        #endregion

        #region Attached "SaveSize" Property Implementation
        /// <summary>
        /// Register the "SaveSize" attached property and the "OnSaveSizeInvalidated" callback 
        /// </summary>
        public static readonly DependencyProperty SaveSizeProperty
           = DependencyProperty.RegisterAttached("SaveSize", typeof(bool), typeof(WindowSettings),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSaveSizeInvalidated)));

        public static void SetSaveSize(DependencyObject dependencyObject, bool enabled)
        {
            dependencyObject.SetValue(SaveSizeProperty, enabled);
        }

        /// <summary>
        /// Called when SaveSize is changed on an object.
        /// </summary>
        private static void OnSaveSizeInvalidated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Window window = dependencyObject as Window;
            if (window != null)
            {
                if ((bool)e.NewValue)
                {
                    WindowSettings settings = new WindowSettings(window, false, true);
                    settings.Attach();
                }
            }
        }
        #endregion

        #region LoadWindowState
        /// <summary>
        /// Load the Window Size Location and State from the settings object,
        /// assigning that to the Window that's associated with this.
        /// </summary>
        protected virtual void LoadWindowState()
        {
            this.Settings.Reload();
            // Deal with multiple monitors.
            if (this.Settings.Location != Rect.Empty)
            {
                if (_isSavingSize)
                {
                    this._window.Width = this.Settings.Location.Width;
                    this._window.Height = this.Settings.Location.Height;
                    //Console.WriteLine("in LoadWindowState, setting " + _sInstanceSettingsKey + " Width=" + Settings.Location.Width
                    //    + ", Height=" + Settings.Location.Height + ", _isSavingSizeOnly is " + _isSavingSizeOnly);
                }
                if (_isSavingPosition)
                {
                    this._window.Left = this.Settings.Location.Left;
                    this._window.Top = this.Settings.Location.Top;

                    // Apply a correction if the previous settings had it located on a monitor that no longer is available.
                    //
                    double VirtualScreenTop = System.Windows.SystemParameters.VirtualScreenTop;
                    double VirtualScreenWidth = System.Windows.SystemParameters.VirtualScreenWidth;
                    double VirtualScreenHeight = System.Windows.SystemParameters.VirtualScreenHeight;
                    double VirtualScreenLeft = System.Windows.SystemParameters.VirtualScreenLeft;
                    double VirtualScreenRight = VirtualScreenLeft + VirtualScreenWidth;
                    double VirtualScreenBottom = VirtualScreenTop + VirtualScreenHeight;
                    double MyWidth = _window.Width;
                    double MyBottom = _window.Top + _window.Height;

                    // If the 2nd monitor was to the right, and is now not..
                    if (_window.Left > (VirtualScreenRight - MyWidth))
                    {
                        _window.Left = VirtualScreenRight - MyWidth;
                    }
                    // or if it was to the left..
                    else if (_window.Left < VirtualScreenLeft)
                    {
                        _window.Left = VirtualScreenLeft;
                    }
                    // or if there was a vertical change..
                    if (MyBottom > VirtualScreenBottom)
                    {
                        _window.Top = VirtualScreenBottom - _window.Height;
                    }
                    else if (_window.Top < VirtualScreenTop)
                    {
                        _window.Top = VirtualScreenTop;
                    }
                }
            }
            if (this.Settings.WindowState != WindowState.Maximized)
            {
                this._window.WindowState = this.Settings.WindowState;
            }
        }
        #endregion

        #region SaveWindowState
        /// <summary>
        /// Save the Window Size, Location and State to the settings object
        /// </summary>
        protected virtual void SaveWindowState()
        {
            Settings.WindowState = _window.WindowState;
            Settings.Location = _window.RestoreBounds;
            Settings.Save();
            //Console.WriteLine("in WindowSettings.SaveWindowState, saving " + _sInstanceSettingsKey + " Width=" + Settings.Location.Width
            //    + ", Height=" + Settings.Location.Height + ", _isSavingSizeOnly is " + _isSavingSizeOnly);
        }
        #endregion

        #region Attach
        private void Attach()
        {
            if (this._window != null)
            {
                this._window.Closing += new CancelEventHandler(window_Closing);
                this._window.Initialized += new EventHandler(window_Initialized);
                this._window.Loaded += new RoutedEventHandler(window_Loaded);
            }
        }
        #endregion

        #region window_Loaded
        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Settings.WindowState == WindowState.Maximized)
            {
                this._window.WindowState = this.Settings.WindowState;
            }
        }
        #endregion

        #region window_Initialized
        private void window_Initialized(object sender, EventArgs e)
        {
            LoadWindowState();
        }
        #endregion

        #region window_Closing
        private void window_Closing(object sender, CancelEventArgs e)
        {
            SaveWindowState();
        }
        #endregion

        #region Settings Property Implementation

        protected virtual WindowApplicationSettings CreateWindowApplicationSettingsInstance()
        {
            return new WindowApplicationSettings(this, _sInstanceSettingsKey);
        }

        [Browsable(false)]
        public WindowApplicationSettings Settings
        {
            get
            {
                if (_windowApplicationSettings == null)
                {
                    _windowApplicationSettings = CreateWindowApplicationSettingsInstance();
                }
                return _windowApplicationSettings;
            }
        }
        #endregion

        #region fields

        private readonly Window _window;
        /// <summary>
        /// This is used to dictate whether we're saving this Window's size, vs position+size, or just the position alone.
        /// </summary>
        private readonly bool _isSavingSize;
        /// <summary>
        /// This is used to dictate whether we're saving this Window's position, vs position+size or just the size alone.
        /// </summary>
        private readonly bool _isSavingPosition;
        private WindowApplicationSettings _windowApplicationSettings;
        private readonly string _sInstanceSettingsKey;

        #endregion
    }
}
