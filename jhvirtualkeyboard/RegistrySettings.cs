// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhVirtualKeyboardDemoApp</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using JhLib;


namespace JhVirtualKeyboard
{
    public class RegistrySettings : RegistryBase
    {
        #region The
        /// <summary>
        /// The singleton-factory method
        /// </summary>
        public static RegistrySettings The
        {
            get
            {
                if (_theRegistry == null)
                {
                    _theRegistry = new RegistrySettings();
                }
                return _theRegistry;
            }
        }
        #endregion

        #region TheKeyboardLayout
        /// <summary>
        /// Get/set the current WhichKeyboardLayout to use.
        /// </summary>
        public WhichKeyboardLayout TheKeyboardLayout
        {
            get
            {
                if (!_keyboardLayout.HasValue)
                {
                    string sLayout = GetString("VKLayout", "English");
                    WhichKeyboardLayout layout = WhichKeyboardLayout.Unknown;
                    if (Enum.TryParse(sLayout, out layout))
                    {
                        if (layout != WhichKeyboardLayout.Unknown)
                        {
                            _keyboardLayout = layout;
                        }
                    }
                }
                return _keyboardLayout.Value;
            }
            set
            {
                if (! (_keyboardLayout.HasValue && value == _keyboardLayout.Value))
                {
                    _keyboardLayout = value;
                    // If we're saying that we don't know, then simply remove the Registry value.
                    if (value == WhichKeyboardLayout.Unknown)
                    {
                        DeleteValue("VKLayout");
                    }
                    else
                    {
                        // otherwise, save it under the value "VKLayout".
                        SetValue("VKLayout", value.ToString());
                    }
                }
            }
        }
        #endregion TheKeyboardLayout

        #region BaseKeyPath
        /// <summary>
        /// Base registry key that your subclass should override to provide the full path of keys/subkeys to access the key-values of your application.
        /// For example, "Software\GrassValley\KayenneMenu".
        /// </summary>
        public override string BaseKeyPath
        {
            get { return @"Software\DesignForge\VirtualKeyboard"; }
        }
        #endregion

        #region fields

        private static RegistrySettings _theRegistry;
        private WhichKeyboardLayout? _keyboardLayout;

        #endregion
    }
}
