// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhVirtualKeyboard</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System.Windows;  // FontWeight


namespace JhVirtualKeyboard.ViewModels
{
    /// <summary>
    /// This class specializes the KeyViewModel to represent just the right-hand ALT key-button, when used as an AltGr key.
    /// </summary>
    public class AltKeyViewModel : KeyModel
    {
        public AltKeyViewModel()
        {
            _toolTipNormal = "Alt Char - use this to insert alternate characters shown in red.";
            _toolTipInAltGrState = "Alt Char - is now active. You may now click the blue characters to insert them.";
        }

        #region FontWeight
        /// <summary>
        /// Get the FontWeight to be used for the text on this key-button, which varies as a function of it's AltGr state.
        /// </summary>
        public FontWeight FontWeight
        {
            get
            {
                if (_isInAltGrState)
                {
                    return FontWeights.Bold;
                }
                else
                {
                    return FontWeights.Thin;
                }
            }
        }
        #endregion

        #region IsInAltGrState
        /// <summary>
        /// Get/set whether we are in the AltGr state, which means the blue characters get entered when a key is pressed
        /// (assuming blue is used to indicate the AltGr characters).
        /// </summary>
        public new bool IsInAltGrState
        {
            get
            {
                return _isInAltGrState;
            }
            set
            {
                if (value != _isInAltGrState)
                {
                    _isInAltGrState = value;
                    Notify("IsInAltGrState");
                    // These are all the properties that may depend upon this flag.
                    Notify("Text");
                    Notify("FontWeight");
                    Notify("ToolTip");
                }
            }
        }
        #endregion

        #region Text
        /// <summary>
        /// Get the Text to be shown on the this Button
        /// </summary>
        public override string Text
        {
            get
            {
                return "AltGr";
            }
        }
        #endregion

        #region ToolTip
        /// <summary>
        /// Get the ToolTip, whether it corresponds to the normal or AltGr state.
        /// </summary>
        public override string ToolTip
        {
            get
            {
                if (_isInAltGrState)
                {
                    return _toolTipInAltGrState;
                }
                else
                {
                    return _toolTipNormal;
                }
            }
        }
        #endregion

        #region ToolTipInNormalState
        /// <summary>
        /// Get/set the ToolTip to use when in the normal, as opposed to the AltGr, state.
        /// </summary>
        public string ToolTipInNormalState
        {
            get { return _toolTipNormal; }
            set
            {
                if (value != _toolTipNormal)
                {
                    _toolTipNormal = value;
                    Notify("ToolTip");
                }
            }
        }
        #endregion

        #region ToolTipInAltGrState
        /// <summary>
        /// Get/set the ToolTip to use when in the AltGr state.
        /// </summary>
        public string ToolTipInAltGrState
        {
            get { return _toolTipInAltGrState; }
            set
            {
                if (value != _toolTipInAltGrState)
                {
                    _toolTipInAltGrState = value;
                    Notify("ToolTip");
                }
            }
        }
        #endregion

        #region fields

        private bool _isInAltGrState;
        private string _toolTipNormal;
        private string _toolTipInAltGrState;

        #endregion fields
    }
}
