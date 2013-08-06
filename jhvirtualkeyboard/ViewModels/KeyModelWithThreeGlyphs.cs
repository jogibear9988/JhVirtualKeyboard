// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2012-2-26</date>
// <project>JhVirtualKeyboard</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Text;


namespace JhVirtualKeyboard.ViewModels
{
    #region class KeyModelWithThreeGlyphs
    /// <summary>
    /// This sublcass of KeyModelWithTwoGlyphs defines a key-button that displays three glyphs at one time.
    /// </summary>
    public class KeyModelWithThreeGlyphs : KeyModelWithTwoGlyphs
    {
        #region Constructor
        /// <summary>
        /// This constructor provides for an AltGr codepoint.
        /// It assumes that there is no distinct need to consider it as a letter, and that you always want to display all 3 possible codepoints.
        /// </summary>
        /// <param name="iCodePoint">The Unicode codepoint for this particular key-button</param>
        /// <param name="iShiftedCodePoint">The Unicode codepoint to use when in the SHIFT state</param>
        /// <param name="iAltGrCodePoint">The Unicode codepoint to use when in the ALT state</param>
        /// <param name="toolTipText">The hint to display when the user hovers over this key</param>
        /// <param name="shiftedToolTipText">The hint to display when in the SHIFT state</param>
        /// <param name="altGrToolTip">The hint to display when in the ALT state</param>
        public KeyModelWithThreeGlyphs(string displayAtDesignTime, int iCodePoint, int iShiftedCodePoint, int iAltGrCodePoint, string toolTip, string shiftedToolTip, string altGrToolTip)
        {
            designTimeDisplayName = displayAtDesignTime;
            UnshiftedCodePoint = (char)iCodePoint;
            ShiftedCodePoint = (char)iShiftedCodePoint;
            AltGrCodePoint = (char)iAltGrCodePoint;
            UnshiftedToolTip = toolTip;
            ShiftedToolTip = shiftedToolTip;
            AltGrToolTip = altGrToolTip;
            IsLetter = false;
        }
        #endregion Constructor

        #region CodePointString
        /// <summary>
        /// Get the Unicode code-point (as a string) that's assigned to this key-button, as a function of it's shifted or alt state.
        /// </summary>
        public override string CodePointString
        {
            get
            {
                if ((theKeyboardViewModel.IsShiftLock || (isThisALetter && theKeyboardViewModel.IsCapsLock)) && shiftedCodePoint != 0)
                {
                    return ((char)ShiftedCodePoint).ToString();
                }
                else if (theKeyboardViewModel.IsInAltGrState && altCodePoint != 0)
                {
                    return ((char)AltGrCodePoint).ToString();
                }
                else
                {
                    return ((char)UnshiftedCodePoint).ToString();
                }
            }
        }
        #endregion CodePointString

        #region AltGrCodePoint
        /// <summary>
        /// Get the Unicode code-point that is assigned to this key-button,
        /// when it is in it's "ALT" state -- that is, as when the Alt-Gr key is pressed.
        /// </summary>
        public char AltGrCodePoint
        {
            get { return altCodePoint; }
            set
            {
                if (value != altCodePoint)
                {
                    altCodePoint = value;
                    //TODO  Have yet to decide what to notify
                    Notify("Text");
                    Notify("TextAltGr");
                }
            }
        }
        #endregion AltGrCodePoint

        #region IsToShowBothGlyphs
        /// <summary>
        /// Get/set whether both the CodePoint, and the ShiftedCodePoint, glyphs
        /// are to be shown together, one below the other, on the key-cap.
        /// </summary>
        public override bool IsToShowBothGlyphs
        {
            get { return true; }
        }
        #endregion

        #region TextAltGr
        /// <summary>
        /// Get the text for the codepoint that this should yield when in the AltGr state.
        /// </summary>
        public override string TextAltGr
        {
            get
            {
                if (altCodePoint != 0)
                {
                    return altCodePoint.ToString();
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region ToolTip
        /// <summary>
        /// Get the ToolTip, whether it corresponds to the unshifted, shifted, or Alt codepoints.
        /// </summary>
        public override string ToolTip
        {
            get
            {
                if ((theKeyboardViewModel.IsShiftLock
                    || (isThisALetter && theKeyboardViewModel.IsCapsLock)) && !String.IsNullOrEmpty(shiftedToolTipText))
                {
                    return shiftedToolTipText;
                }
                else if (theKeyboardViewModel.IsInAltGrState && !String.IsNullOrEmpty(altToolTip))
                {
                    return altToolTip;
                }
                else
                {
                    return toolTipText;
                }
            }
        }
        #endregion ToolTip

        #region AltGrToolTip
        /// <summary>
        /// Get the ToolTip that corresponds to the Alt codepoints.
        /// </summary>
        public string AltGrToolTip
        {
            get { return altToolTip; }
            set { altToolTip = value; }
        }
        #endregion

        #region ToString
        /// <summary>
        /// Override the ToString method to give a better indication of what this object represents.
        /// </summary>
        /// <returns>A description of the state of this object, or else if in design-time just the key-name</returns>
        public override string ToString()
        {
            if (IsInDesignMode)
            {
                return this.Name;
            }
            else
            {
                var sb = new StringBuilder("KeyModelWithThreeGlyphs(");
                sb.Append(Name);
                if (IsLetter)
                {
                    sb.Append(",letter,");
                }
                else
                {
                    sb.Append(",notletter,");
                }
                if (!String.IsNullOrEmpty(UnshiftedToolTip))
                {
                    sb.Append("\"");
                    sb.Append(UnshiftedToolTip);
                    sb.Append("\"");
                    if (!String.IsNullOrEmpty(ShiftedToolTip))
                    {
                        sb.Append(",\"");
                        sb.Append(ShiftedToolTip);
                        sb.Append("\"");
                        if (!String.IsNullOrEmpty(AltGrToolTip))
                        {
                            sb.Append(",\"");
                            sb.Append(AltGrToolTip);
                            sb.Append("\"");
                        }
                    }
                }
                else
                {
                    sb.Append("no tooltip");
                }
                sb.Append(")");
                return sb.ToString();
            }
        }
        #endregion

        #region fields

        protected char altCodePoint; // the character you get when the AltGr key is active.
        protected string altToolTip;

        #endregion fields
    }
    #endregion class KeyModelWithThreeGlyphs
}
