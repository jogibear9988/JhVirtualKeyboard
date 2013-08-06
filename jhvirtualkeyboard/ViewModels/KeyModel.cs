// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhVirtualKeyboard</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using JhLib;


namespace JhVirtualKeyboard.ViewModels
{
    #region class KeyModel
    /// <summary>
    /// A KeyModel defines what is assigned to one keyboard key (which we're calling key-buttons).
    /// </summary>
    public class KeyModel : BaseViewModel
    {
        #region Constructors

        public KeyModel()
        {
            this.UnshiftedCodePoint = '?';
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayAtDesignTime">The text to show on the key-face at design-time</param>
        public KeyModel(string displayAtDesignTime)
            : base()
        {
            this.designTimeDisplayName = displayAtDesignTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayAtDesignTime">The text to show on the key-face at design-time</param>
        /// <param name="iCodePoint">The Unicode codepoint for this particular key-button</param>
        /// <param name="toolTipText">The hint to display when the user hovers over this key</param>
        /// <param name="isThisALetter">Indicates whether this is an alphabetic letter</param>
        public KeyModel(string displayAtDesignTime, int iCodePoint, string toolTip, bool isLetter)
        {
            designTimeDisplayName = displayAtDesignTime;
            UnshiftedCodePoint = (char)iCodePoint;
            ShiftedCodePoint = (char)0;
            UnshiftedToolTip = toolTip;
            IsLetter = isLetter;
        }

        /// <summary>
        /// This is used only for letters. It composes the tooltips automatically.
        /// </summary>
        /// <param name="displayAtDesignTime">The text to show on the key-face at design-time</param>
        /// <param name="iCodePoint">The Unicode codepoint for this particular key-button</param>
        /// <param name="iShiftedCodePoint">The Unicode codepoint to use when in the SHIFT state</param>
        public KeyModel(string displayAtDesignTime, int iCodePoint, int iShiftedCodePoint)
        {
            designTimeDisplayName = displayAtDesignTime;
            UnshiftedCodePoint = (char)iCodePoint;
            ShiftedCodePoint = (char)iShiftedCodePoint;
            string sTheLetter = ((char)iShiftedCodePoint).ToString();
            UnshiftedToolTip = "lowercase letter " + sTheLetter;
            ShiftedToolTip = "capital letter " + sTheLetter;
            IsLetter = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayAtDesignTime">The text to show on the key-face at design-time</param>
        /// <param name="iCodePoint">The Unicode codepoint for this particular key-button</param>
        /// <param name="iShiftedCodePoint">The Unicode codepoint to use when in the SHIFT state</param>
        /// <param name="toolTipText">The hint to display when the user hovers over this key</param>
        /// <param name="shiftedToolTipText">The hint to display when in the SHIFT state</param>
        /// <param name="isThisALetter">Indicates whether this is an alphabetic letter</param>
        public KeyModel(string displayAtDesignTime, int iCodePoint, int iShiftedCodePoint, string toolTip, string shiftedToolTip, bool isLetter)
        {
            designTimeDisplayName = displayAtDesignTime;
            UnshiftedCodePoint = (char)iCodePoint;
            ShiftedCodePoint = (char)iShiftedCodePoint;
            UnshiftedToolTip = toolTip;
            ShiftedToolTip = shiftedToolTip;
            IsLetter = isLetter;
        }

        #endregion Constructors

        #region Properties

        #region Code-Points

        #region CodePointString
        /// <summary>
        /// Get the Unicode code-point (as a string) that's assigned to this key-button, as a function of it's shifted or alt state.
        /// </summary>
        public virtual string CodePointString
        {
            get
            {
                if (theKeyboardViewModel != null)
                {
                    if ((theKeyboardViewModel.IsShiftLock || (isThisALetter && theKeyboardViewModel.IsCapsLock)) && shiftedCodePoint != 0)
                    {
                        return ((char)ShiftedCodePoint).ToString();
                    }
                    else
                    {
                        return ((char)UnshiftedCodePoint).ToString();
                    }
                }
                else
                {
                    // Just a glyph to signify that the view-model was null at the time this property was fetched.
                    return "-";
                }
            }
        }
        #endregion CodePointString

        #region UnshiftedCodePoint
        /// <summary>
        /// Get the Unicode code-point that is assigned to this key-button,
        /// when it is in it's normal state -- that is, not shifted or alt.
        /// </summary>
        public char UnshiftedCodePoint
        {
            get { return unshiftedCodePoint; }
            set
            {
                if (value != unshiftedCodePoint)
                {
                    unshiftedCodePoint = value;
                    //TODO  Have yet to decide what to notify
                    Notify("Text");
                    Notify("TextUnshifted");
                }
            }
        }
        #endregion UnshiftedCodePoint

        #region ShiftedCodePoint
        /// <summary>
        /// Get the Unicode code-point that is assigned to this key-button,
        /// when it is in it's shifted state -- that is, as when the Shift key is pressed.
        /// </summary>
        public char ShiftedCodePoint
        {
            get { return shiftedCodePoint; }
            set
            {
                if (value != shiftedCodePoint)
                {
                    shiftedCodePoint = value;
                    //TODO  Have yet to decide what to notify
                    Notify("Text");
                    Notify("TextShifted");
                }
            }
        }
        #endregion ShiftedCodePoint

        #endregion Code-Points

        #region ColorForAltGr
        /// <summary>
        /// Get the color to be used for the AltGr key-cap, and for the AltGr characters on whichever keys use those.
        /// </summary>
        public SolidColorBrush ColorForAltGr
        {
            get { return theKeyboardViewModel.ColorForAltGr; }
        }
        #endregion

        public string FontFamilyName { get; set; }

        public string HtmlEntityName { get; set; }

        #region IsLetter
        /// <summary>
        /// Get/set whether this key-button contains a 'letter',
        /// which for our purposes means the CAPS-LOCK makes it into a capital letter.
        /// </summary>
        public bool IsLetter
        {
            get { return isThisALetter; }
            set { isThisALetter = value; }
        }
        #endregion

        #region IsInAltGrState
        /// <summary>
        /// Get/set whether the VirtualKeyboard is currently is AltGr mode.
        /// </summary>
        public bool IsInAltGrState
        {
            // Note: This property is provided on the KeyModel, simply because I couldn't see how to access
            // the same property on the KeyboardViewModel from within the Button style.  TODO
            get
            {
                if (theKeyboardViewModel != null)
                {
                    return theKeyboardViewModel.IsInAltGrState;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region IsInNormalState
        /// <summary>
        /// Get whether this key-button is in it's "normal" state, meaning not Alt and not shifted.
        /// </summary>
        public bool IsInNormalState
        {
            get { return !IsInAltGrState && !IsInUpperGlyphMode; }
        }
        #endregion

        #region IsShiftLock
        /// <summary>
        /// Get/set whether the VirtualKeyboard is currently is Shift-Lock mode.
        /// </summary>
        public bool IsShiftLock
        {
            // Note: This property is provided on the KeyModel, simply because I couldn't see how to access
            // the same property on the KeyboardViewModel from within the Button style.  TODO
            get
            {
                if (theKeyboardViewModel != null)
                {
                    return theKeyboardViewModel.IsShiftLock;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region IsInUpperGlyphMode
        /// <summary>
        /// Get/set whether the upper glyph of this key-button currently applies.
        /// </summary>
        public virtual bool IsInUpperGlyphMode
        {
            //TODO This doesn't actually need to be virtual. I'm just trying to get binding to work in SL. shit.
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
        public virtual bool IsToShowBothGlyphs
        {
            get { return false; }
        }
        #endregion

        #region KeyName
        /// <summary>
        /// Get the button name of the WPF Button that this is a view-model of.
        /// </summary>
        public string KeyName
        {
            get
            {
                if (String.IsNullOrEmpty(keyName))
                {
                    return "";
                }
                else
                {
                    return keyName;
                }
            }
            set { keyName = value; }
        }
        #endregion

        #region Name
        /// <summary>
        /// Get a name to identify this key by, whether the display-name or the KeyName.
        /// </summary>
        public string Name
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(this.designTimeDisplayName))
                {
                    return designTimeDisplayName;
                }
                else if (!String.IsNullOrWhiteSpace(this.KeyName))
                {
                    return this.KeyName;
                }
                else
                {
                    return "NoName!";
                }
            }
        }
        #endregion

        #region Text-and-variants

        #region Text
        /// <summary>
        /// Get the Text to be shown on the corresponding Button
        /// </summary>
        public virtual string Text
        {
            get
            {
                //if (this.IsInDesignMode)
                //{
                //    if (String.IsNullOrWhiteSpace(designTimeDisplayName))
                //    {
                //        return keyName;
                //    }
                //    else
                //    {
                //        return designTimeDisplayName;
                //    }
                //}
                //else // not in design-mode
                {
                    string sText;
                    if (!String.IsNullOrEmpty(text))
                    {
                        sText = text;
                    }
                    else
                    {
                        sText = this.CodePointString;
                    }
                    //Debug.WriteLine("KeyModel for " + this.Name + ", Text.get returning " + sText);
                    return sText;
                }
            }
        }
        #endregion

        #region TextAltGr
        /// <summary>
        /// Get the text for the codepoint that this should yield when in the AltGr state.
        /// </summary>
        public virtual string TextAltGr
        {
            // Derived classes will override this, to provide the Alt-text when it applies.
            get
            {
                return null;
            }
        }
        #endregion

        #region TextShifted
        /// <summary>
        /// Get the text for the codepoint that this should yield when in the shifted state.
        /// </summary>
        public string TextShifted
        {
            get
            {
                if (shiftedCodePoint != 0)
                {
                    return shiftedCodePoint.ToString();
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region TextUnshifted
        /// <summary>
        /// Get the text for the codepoint that this should yield when not in the shifted state.
        /// </summary>
        public string TextUnshifted
        {
            get
            {
                return unshiftedCodePoint.ToString();
            }
        }
        #endregion

        #region TextUnshiftedNL
        /// <summary>
        /// Get the text for the codepoint that this should yield when not in the shifted state,
        /// with a new-line prefixed to it (such that it appears underneath the shifted text on the key-button).
        /// </summary>
        public string TextUnshiftedNL
        {
            get
            {
                return Environment.NewLine + unshiftedCodePoint.ToString();
            }
        }
        #endregion

        #endregion Text-and-variants

        #region ToolTip
        /// <summary>
        /// Get the ToolTip, whether it corresponds to the unshifted, shifted, or Alt codepoints.
        /// </summary>
        public virtual string ToolTip
        {
            get
            {
                if (theKeyboardViewModel != null)
                {
                    if ((theKeyboardViewModel.IsShiftLock
                        || (isThisALetter && theKeyboardViewModel.IsCapsLock)) && !String.IsNullOrEmpty(shiftedToolTipText))
                    {
                        return shiftedToolTipText;
                    }
                    else
                    {
                        return toolTipText;
                    }
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion ToolTip

        #region UnshiftedToolTip
        /// <summary>
        /// Gets or sets the text to use for the normal ToolTip (that is, when the SHIFT-key is not being pressed down).
        /// </summary>
        public string UnshiftedToolTip
        {
            get { return toolTipText; }
            set
            {
                if (value != toolTipText)
                {
                    toolTipText = value;
                    Notify("ToolTip");
                }
            }
        }
        #endregion

        #region ShiftedToolTip
        /// <summary>
        /// Gets or sets the text to use for the ToolTip when the SHIFT-key is being pressed down.
        /// </summary>
        public string ShiftedToolTip
        {
            get { return shiftedToolTipText; }
            set { shiftedToolTipText = value; }
        }
        #endregion

        #endregion Properties

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
                var sb = new StringBuilder("KeyModel(");
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

        protected Button button;
        /// <summary>
        /// What to show on the key-cap when being viewed in a design-tool such as Blend or Cider.
        /// </summary>
        protected string designTimeDisplayName;
        /// <summary>
        /// This flag indicates whether this key-button contains a 'letter'.
        /// </summary>
        protected bool isThisALetter;
        protected string keyName;
        protected string text;
        public static KeyboardViewModel theKeyboardViewModel;
        protected char unshiftedCodePoint; // the character you normally get from this key-button.
        protected char shiftedCodePoint; // the character you get when the Shift key is active.
        protected string toolTipText;
        protected string shiftedToolTipText;

        #endregion fields
    }
    #endregion class KeyModel
}
