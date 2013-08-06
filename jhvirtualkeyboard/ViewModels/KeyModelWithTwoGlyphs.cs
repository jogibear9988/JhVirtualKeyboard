// ****************************************************************************
// <email>JamesH@DesignForge.com</email>
// <date>2012-2-26</date>
// <project>JhVirtualKeyboard</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Text;


namespace JhVirtualKeyboard.ViewModels
{
    #region class KeyModelWithTwoGlyphs
    /// <summary>
    /// This sublcass of KeyModel defines a key-button that displays two glyphs at one time, one above the other.
    /// </summary>
    public class KeyModelWithTwoGlyphs : KeyModel
    {
        #region Constructors

        public KeyModelWithTwoGlyphs()
            : base()
        {
        }

        /// <summary>
        /// This is used only for letters. It composes the tooltips automatically.
        /// </summary>
        /// <param name="iCodePoint">The Unicode codepoint for this particular key-button</param>
        /// <param name="iShiftedCodePoint">The Unicode codepoint to use when in the SHIFT state</param>
        //public KeyModelWithTwoGlyphs(string displayAtDesignTime, int iCodePoint, int iShiftedCodePoint)
        //{
        //    designTimeDisplayName = displayAtDesignTime;
        //    UnshiftedCodePoint = (char)iCodePoint;
        //    ShiftedCodePoint = (char)iShiftedCodePoint;
        //    string sTheLetter = ((char)iShiftedCodePoint).ToString();
        //    UnshiftedToolTip = "lowercase letter " + sTheLetter;
        //    ShiftedToolTip = "capital letter " + sTheLetter;
        //    IsLetter = true;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayAtDesignTime">The text to show on the key-face at design-time</param>
        /// <param name="iCodePoint">The Unicode codepoint for this particular key-button</param>
        /// <param name="iShiftedCodePoint">The Unicode codepoint to use when in the SHIFT state</param>
        /// <param name="toolTipText">The hint to display when the user hovers over this key</param>
        /// <param name="shiftedToolTipText">The hint to display when in the SHIFT state</param>
        /// <param name="isThisALetter">Indicates whether this is an alphabetic letter</param>
        public KeyModelWithTwoGlyphs(string displayAtDesignTime, int iCodePoint, int iShiftedCodePoint, string toolTip, string shiftedToolTip, bool isLetter)
        {
            designTimeDisplayName = displayAtDesignTime;
            UnshiftedCodePoint = (char)iCodePoint;
            ShiftedCodePoint = (char)iShiftedCodePoint;
            UnshiftedToolTip = toolTip;
            ShiftedToolTip = shiftedToolTip;
            IsLetter = isLetter;
        }

        #endregion Constructors

        #region IsInUpperGlyphMode
        /// <summary>
        /// Get/set whether the upper glyph of this key-button currently applies.
        /// </summary>
        public override bool IsInUpperGlyphMode
        {
            //TODO This doesn't actually need to be here. I'm just trying to get binding to work in SL. shit.
            get
            {
                if (theKeyboardViewModel != null)
                {
                    return theKeyboardViewModel.IsShiftLock || (this.IsLetter && theKeyboardViewModel.IsCapsLock);
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

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
                var sb = new StringBuilder("KeyModelWithTwoGlyphs(");
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
    }
    #endregion class KeyModelWithTwoGlyphs
}
