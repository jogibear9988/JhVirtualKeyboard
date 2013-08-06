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
    /// This class specializes the KeyViewModel to represent just the shift key-buttons.
    /// </summary>
    public class ShiftKeyViewModel : KeyModel
    {
        public ShiftKeyViewModel()
        {
            _toolTipNormal = "Enter the SHIFT mode - to insert the shifted characters.";
            _toolTipInShiftedState = "You are in SHIFT mode; click here again to go back to normal mode.";
        }

        #region IsInShiftState
        /// <summary>
        /// Get/set whether we are in the Shift state.
        /// </summary>
        public bool IsInShiftState
        {
            get { return _isInShiftState; }
            set
            {
                if (value != _isInShiftState)
                {
                    _isInShiftState = value;
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
                if (_isInShiftState)
                {
                    return "SHIFT ON";
                }
                else
                {
                    return "SHIFT";
                }
            }
        }
        #endregion

        #region ToolTip
        /// <summary>
        /// Get the ToolTip, whether it corresponds to the normal or shifted state.
        /// </summary>
        public override string ToolTip
        {
            get
            {
                if (_isInShiftState)
                {
                    return _toolTipInShiftedState;
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
        /// Get/set the ToolTip to use when in the normal, as opposed to the shifted, state.
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

        #region ToolTipInShiftedState
        /// <summary>
        /// Get/set the ToolTip to use when in the shifted state.
        /// </summary>
        public string ToolTipInShiftedState
        {
            get { return _toolTipInShiftedState; }
            set
            {
                if (value != _toolTipInShiftedState)
                {
                    _toolTipInShiftedState = value;
                }
            }
        }
        #endregion

        #region FontWeight
        /// <summary>
        /// Get the FontWeight to be used for the text on this key-button, which varies as a function of it's unshifted/shifted state.
        /// </summary>
        public FontWeight FontWeight
        {
            get
            {
                if (_isInShiftState)
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

        #region fields

        private bool _isInShiftState;
        private string _toolTipNormal;
        private string _toolTipInShiftedState;

        #endregion fields
    }
}
