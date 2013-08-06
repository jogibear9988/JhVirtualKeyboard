// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhVirtualKeyboard</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;
using JhVirtualKeyboard.ViewModels;


namespace JhVirtualKeyboard.KeyAssignmentSets
{
    #region class KeyAssignmentSet
    /// <summary>
    /// This and it's subclasses represents the complete set of KeyAssignments that are currently assigned to the keyboard at a given time
    /// </summary>
    public class KeyAssignmentSet
    {
        #region For
        /// <summary>
        /// Return (or create anew if it hasn't been yet) the KeyAssignmentSet class for the given language.
        /// </summary>
        public static KeyAssignmentSet For(WhichKeyboardLayout whichKeyboardLayout)
        {
            // Create the new set, if it hasn't been already.
            if (s_theKeyAssignmentSet == null || s_theKeyAssignmentSet.KeyboardLayout != whichKeyboardLayout)
            {
                switch (whichKeyboardLayout)
                {
                    case WhichKeyboardLayout.Arabic:
                        s_theKeyAssignmentSet = new ArabicKeyAssignmentSet();
                        break;
                    case WhichKeyboardLayout.French:
                        s_theKeyAssignmentSet = new FrenchKeyAssignmentSet();
                        break;
                    case WhichKeyboardLayout.GermanAustrian:
                        s_theKeyAssignmentSet = new GermanKeyAssignmentSet();
                        break;
                    case WhichKeyboardLayout.Italian:
                        break;
                    case WhichKeyboardLayout.Mandarin:
                        break;
                    case WhichKeyboardLayout.Russian:
                        s_theKeyAssignmentSet = new RussianKeyAssignmentSet();
                        break;
                    case WhichKeyboardLayout.Spanish:
                        s_theKeyAssignmentSet = new SpanishKeyAssignmentSet();
                        break;
                    case WhichKeyboardLayout.Sanskrit:
                        s_theKeyAssignmentSet = new SanskritKeyAssignmentSet();
                        break;
                    case WhichKeyboardLayout.Tamil:
                        s_theKeyAssignmentSet = new TamilKeyAssignmentSet();
                        break;
                    default:  // This defaults to English.
                        s_theKeyAssignmentSet = new KeyAssignmentSet();
                        break;
                }
            }
            return s_theKeyAssignmentSet;
        }

        private static KeyAssignmentSet s_theKeyAssignmentSet;

        #endregion For

        #region KeyboardLayout
        /// <summary>
        /// Get the WhichKeyboardLayout this KeyAssignmentSet is used for.
        /// </summary>
        public virtual WhichKeyboardLayout KeyboardLayout
        {
            get { return WhichKeyboardLayout.English; }
        }
        #endregion

        #region IsAltGrKeyUsed
        /// <summary>
        /// Get whether the AltGr key (also known as the Alt Graph, Alt Grill, or Alt Char key) is used as a modifier key
        /// in this KeyAssignmentSet. This is false unless overridden.
        /// </summary>
        public virtual bool IsAltGrKeyUsed
        {
            get { return false; }
        }
        #endregion

        #region KeycapFontFamily
        /// <summary>
        /// Get/set the FontFamily to use for the character-entry keys (as opposed to the labels on the other keys, tooltips, etc).
        /// </summary>
        public FontFamily KeycapFontFamily
        {
            get
            {
                if (_keycapFontFamily == null)
                {
                    _keycapFontFamily = new FontFamily(this.KeycapFontFamilyName);
                }
                return _keycapFontFamily;
            }
        }

        private FontFamily _keycapFontFamily;

        #endregion

        #region KeycapFontFamilyName
        /// <summary>
        /// Get the name of the FontFamily that we want to use to display the keycaps of the keyboard in, when using this KeyAssignmentSet.
        /// Subclasses of KeyAssignmentSet must override this if they want to use different fonts than this.
        /// </summary>
        public virtual string KeycapFontFamilyName
        {
            get
            {
                return "Segoe UI";
            }
        }
        #endregion

        #region FontSizeForStackedGlyphs
        /// <summary>
        /// Get the FontSize to use for keycaps that have glyphs arranged in two rows, one over the other, or else side-by-side,
        /// such that a smaller FontSize is needed as opposed to when just a single glyph is to be shown.
        /// </summary>
        public virtual double FontSizeForStackedGlyphs
        {
            get { return 14.0; }
        }
        #endregion

        #region FontSizeForSingleGlyphs
        /// <summary>
        /// Get the FontSize to use for keycaps that have a single glyph, such that it can be rendered larger.
        /// </summary>
        public virtual double FontSizeForSingleGlyphs
        {
            get { return 24.0; }
        }
        #endregion

        #region FlowDirection
        /// <summary>
        /// Get the FlowDirection that this keyboard-layout would like to experience.
        /// You'll want to overload this in your subclasses for languages like Arabic.
        /// </summary>
        public virtual FlowDirection FlowDirection
        {
            get
            {
                return System.Windows.FlowDirection.LeftToRight;
            }
        }
        #endregion

        #region IsHelpButtonVisible
        /// <summary>
        /// Get/set whether the help-button, which has the little question-mark icon, is visible.
        /// You'll want to overload this in your subclasses for languages like Tamil and Sanskrit.
        /// </summary>
        public virtual bool IsHelpButtonVisible
        {
            get { return false; }
        }
        #endregion

        #region KeyAssignments property

        private KeyModel[] _theKeyAssignmentArray;

        /// <summary>
        /// Get the array of all the KeyAssignment objects within this set, in the same order and number as the key-button array within the virtual keyboard.
        /// </summary>
        [XmlArrayItem("ListItem", typeof(KeyModel))]
        [XmlArray("KeyAssignments")]
        public KeyModel[] KeyAssignments
        {
            get
            {
                if (_theKeyAssignmentArray == null)
                {
                    _theKeyAssignmentArray = new KeyModel[] {
                        KeyVK_Oem3, KeyVK_1, KeyVK_2, KeyVK_3, KeyVK_4, KeyVK_5, KeyVK_6, KeyVK_7, KeyVK_8, KeyVK_9, KeyVK_0, KeyVK_OemMinus, KeyVK_OemPlus,
                        KeyVK_Q, KeyVK_W, KeyVK_E, KeyVK_R, KeyVK_T, KeyVK_Y, KeyVK_U, KeyVK_I, KeyVK_O, KeyVK_P, KeyVK_OemOpenBrackets, KeyVK_Oem6, KeyVK_Oem5,
                        KeyVK_A, KeyVK_S, KeyVK_D, KeyVK_F, KeyVK_G, KeyVK_H, KeyVK_J, KeyVK_K, KeyVK_L, KeyVK_Oem1, KeyVK_Oem7,
                        KeyVK_Z, KeyVK_X, KeyVK_C, KeyVK_V, KeyVK_B, KeyVK_N, KeyVK_M, KeyVK_OemComma, KeyVK_OemPeriod, KeyVK_OemQuestion
                    };
                }
                return _theKeyAssignmentArray;
            }
        }
        #endregion KeyAssignments property

        #region The individual key assignments

        // This is a handy reference for Unicode values: en.wikipedia.org/wiki/List_of_Unicode_characters

        #region 0:  KeyVK_Oem3 (VK_OEM_3)
        /// <summary>
        /// Get the KeyAssignment for VK_OEM_3 (0: Grace Accent).
        /// </summary>
        public virtual KeyModel KeyVK_Oem3
        {
            get
            {
                if (vk_Oem3 == null)
                {
                    vk_Oem3 = new KeyModelWithTwoGlyphs("VK_Oem3", 0x0060, 0x007E, "Grace Accent", "Tilde", false);
                }
                return vk_Oem3;
            }
        }
        protected KeyModel vk_Oem3;
        #endregion

        #region 1:  KeyVK_1 + !
        public virtual KeyModel KeyVK_1
        {
            get
            {
                if (vk_1 == null)
                {
                    vk_1 = new KeyModelWithTwoGlyphs("VK_1", 0x0031, 0x0021, "digit ONE", "Exclamation Mark", false);
                }
                return vk_1;
            }
        }
        protected KeyModel vk_1;
        #endregion

        #region 2:  KeyVK_2 + Amphora
        public virtual KeyModel KeyVK_2
        {
            get
            {
                if (vk_2 == null)
                {
                    vk_2 = new KeyModelWithTwoGlyphs("VK_2", 0x0032, 0x0040, "digit TWO", "Amphora, also called the At sign", false);
                }
                return vk_2;
            }
        }
        protected KeyModel vk_2;
        #endregion

        #region 3:  KeyVK_3 + #
        public virtual KeyModel KeyVK_3
        {
            get
            {
                if (vk_3 == null)
                {
                    vk_3 = new KeyModelWithTwoGlyphs("VK_3", 0x0033, 0x0023, "digit THREE", "Number sign", false);
                }
                return vk_3;
            }
        }
        protected KeyModel vk_3;
        #endregion

        #region 4:  KeyVK_4 + $
        public virtual KeyModel KeyVK_4
        {
            get
            {
                if (vk_4 == null)
                {
                    vk_4 = new KeyModelWithTwoGlyphs("VK_4", 0x0034, 0x0024, "digit FOUR", "Dollar sign", false);
                }
                return vk_4;
            }
        }
        protected KeyModel vk_4;
        #endregion

        #region 5:  KeyVK_5 + %
        public virtual KeyModel KeyVK_5
        {
            get
            {
                if (vk_5 == null)
                {
                    vk_5 = new KeyModelWithTwoGlyphs("VK_5", 0x0035, 0x0025, "digit FIVE", "Percent sign", false);
                }
                return vk_5;
            }
        }
        protected KeyModel vk_5;
        #endregion

        #region 6:  KeyVK_6
        public virtual KeyModel KeyVK_6
        {
            get
            {
                if (vk_6 == null)
                {
                    vk_6 = new KeyModelWithTwoGlyphs("VK_6", 0x0036, 0x005E, "digit SIX", "Circumflex accent", false);
                }
                return vk_6;
            }
        }
        protected KeyModel vk_6;
        #endregion

        #region 7:  KeyVK_7 + Ampersand
        public virtual KeyModel KeyVK_7
        {
            get
            {
                if (vk_7 == null)
                {
                    vk_7 = new KeyModelWithTwoGlyphs("VK_7", 0x0037, 0x0026, "digit SEVEN", "Ampersand", false);
                }
                return vk_7;
            }
        }
        protected KeyModel vk_7;
        #endregion

        #region 8:  KeyVK_8 + *
        public virtual KeyModel KeyVK_8
        {
            get
            {
                if (vk_8 == null)
                {
                    vk_8 = new KeyModelWithTwoGlyphs("VK_8", 0x0038, 0x002A, "digit EIGHT", "Asterisk", false);
                }
                return vk_8;
            }
        }
        protected KeyModel vk_8;
        #endregion

        #region 9:  KeyVK_9 + (
        public virtual KeyModel KeyVK_9
        {
            get
            {
                if (vk_9 == null)
                {
                    vk_9 = new KeyModelWithTwoGlyphs("VK_9", 0x0039, 0x0028, "digit NINE", "Left Parenthesis", false);
                }
                return vk_9;
            }
        }
        protected KeyModel vk_9;
        #endregion

        #region 10: KeyVK_0 + )
        public virtual KeyModel KeyVK_0
        {
            get
            {
                if (vk_0 == null)
                {
                    vk_0 = new KeyModelWithTwoGlyphs("VK_0", 0x0030, 0x0029, "digit ZERO", "Right Parenthesis", false);
                }
                return vk_0;
            }
        }
        protected KeyModel vk_0;
        #endregion

        #region 11: KeyVK_OemMinus (VK_OEM_MINUS) + underscore
        /// <summary>
        /// 11  Hyphen
        /// </summary>
        public virtual KeyModel KeyVK_OemMinus
        {
            get
            {
                if (vk_OemMinus == null)
                {
                    vk_OemMinus = new KeyModelWithTwoGlyphs("VK_OemMinus", 0x002D, 0x005F, "Hyphen,Minus", "Underscore", false);
                }
                return vk_OemMinus;
            }
        }
        protected KeyModel vk_OemMinus;
        #endregion

        #region 12: KeyVK_OemPlus (VK_OEM_PLUS, = + Plus-sign)
        public virtual KeyModel KeyVK_OemPlus
        {
            get
            {
                if (vk_OemPlus == null)
                {
                    vk_OemPlus = new KeyModelWithTwoGlyphs("VK_OemPlus", 0x03D, 0x002B, "Equals sign", "Plus sign", false);
                }
                return vk_OemPlus;
            }
        }
        protected KeyModel vk_OemPlus;
        #endregion

        #region 13: KeyVK_Q
        public virtual KeyModel KeyVK_Q
        {
            get
            {
                if (vk_Q == null)
                {
                    vk_Q = new KeyModel("VK_Q", 0x0071, 0x0051);
                }
                return vk_Q;
            }
        }
        protected KeyModel vk_Q;
        #endregion

        #region 14: KeyVK_W
        public virtual KeyModel KeyVK_W
        {
            get
            {
                if (vk_W == null)
                {
                    vk_W = new KeyModel("VK_W", 0x0077, 0x0057);
                }
                return vk_W;
            }
        }
        protected KeyModel vk_W;
        #endregion

        #region 15: KeyVK_E
        /// <summary>
        /// 15
        /// </summary>
        public virtual KeyModel KeyVK_E
        {
            get
            {
                if (vk_E == null)
                {
                    vk_E = new KeyModel("VK_E", 0x0065, 0x0045);
                }
                return vk_E;
            }
        }
        protected KeyModel vk_E;
        #endregion

        #region 16: KeyVK_R
        /// <summary>
        /// 16
        /// </summary>
        public virtual KeyModel KeyVK_R
        {
            get
            {
                if (vk_R == null)
                {
                    vk_R = new KeyModel("VK_R", 0x0072, 0x0052);
                }
                return vk_R;
            }
        }
        protected KeyModel vk_R;
        #endregion

        #region 17: KeyVK_T
        /// <summary>
        /// 17
        /// </summary>
        public virtual KeyModel KeyVK_T
        {
            get
            {
                if (vk_T == null)
                {
                    vk_T = new KeyModel("VK_T", 0x0074, 0x0054);
                }
                return vk_T;
            }
        }
        protected KeyModel vk_T;
        #endregion

        #region 18: KeyVK_Y
        /// <summary>
        /// 18
        /// </summary>
        public virtual KeyModel KeyVK_Y
        {
            get
            {
                if (vk_Y == null)
                {
                    vk_Y = new KeyModel("VK_Y", 0x0079, 0x0059);
                }
                return vk_Y;
            }
        }
        protected KeyModel vk_Y;
        #endregion

        #region 19: KeyVK_U
        /// <summary>
        /// 19
        /// </summary>
        public virtual KeyModel KeyVK_U
        {
            get
            {
                if (vk_U == null)
                {
                    vk_U = new KeyModel("VK_U", 0x0075, 0x0055);
                }
                return vk_U;
            }
        }
        protected KeyModel vk_U;
        #endregion

        #region 20: KeyVK_I
        public virtual KeyModel KeyVK_I
        {
            get
            {
                if (vk_I == null)
                {
                    vk_I = new KeyModel("VK_I", 0x0069, 0x0049);
                }
                return vk_I;
            }
        }
        protected KeyModel vk_I;
        #endregion

        #region 21: KeyVK_O
        /// <summary>
        /// 21
        /// </summary>
        public virtual KeyModel KeyVK_O
        {
            get
            {
                if (vk_O == null)
                {
                    vk_O = new KeyModel("VK_O", 0x006F, 0x004F);
                }
                return vk_O;
            }
        }
        protected KeyModel vk_O;
        #endregion

        #region 22: KeyVK_P
        /// <summary>
        /// 22
        /// </summary>
        public virtual KeyModel KeyVK_P
        {
            get
            {
                if (vk_P == null)
                {
                    vk_P = new KeyModel("VK_P", 0x0070, 0x0050);
                }
                return vk_P;
            }
        }
        protected KeyModel vk_P;
        #endregion

        #region 23: VK_OemOpenBrackes (VK_OEM_4), [ + {
        /// <summary>
        /// 23  Left brackets
        /// </summary>
        public virtual KeyModel KeyVK_OemOpenBrackets
        {
            get
            {
                if (vk_OemOpenBrackets == null)
                {
                    vk_OemOpenBrackets = new KeyModelWithTwoGlyphs("VK_OemOpenBracket", 0x005B, 0x007B, "Left Square Bracket", "Left Curly Bracket", false);
                }
                return vk_OemOpenBrackets;
            }
        }
        protected KeyModel vk_OemOpenBrackets;
        #endregion

        #region 24: VK_OEM_6, ] + }
        /// <summary>
        /// 24  Right brackets
        /// </summary>
        public virtual KeyModel KeyVK_Oem6
        {
            get
            {
                if (vk_Oem6 == null)
                {
                    vk_Oem6 = new KeyModelWithTwoGlyphs("VK_Oem6", 0x005D, 0x007D, "Right Square Bracket", "Right Curly Bracket", false);
                }
                return vk_Oem6;
            }
        }
        protected KeyModel vk_Oem6;
        #endregion

        #region 25: KeyVK_Oem5
        /// <summary>
        /// 25
        /// </summary>
        public virtual KeyModel KeyVK_Oem5
        {
            get
            {
                if (vk_Oem5 == null)
                {
                    vk_Oem5 = new KeyModelWithTwoGlyphs("VK_Oem5", 0x005C, 0x007C, "Reverse Solidus", "Vertical Line", false);
                }
                return vk_Oem5;
            }
        }
        protected KeyModel vk_Oem5;
        #endregion

        #region 26: KeyVK_A
        /// <summary>
        /// 26
        /// </summary>
        public virtual KeyModel KeyVK_A
        {
            get
            {
                if (vk_A == null)
                {
                    vk_A = new KeyModel("VK_A", 0x0061, 0x0041);
                }
                return vk_A;
            }
        }
        protected KeyModel vk_A;
        #endregion

        #region 27: KeyVK_S
        /// <summary>
        /// 27
        /// </summary>
        public virtual KeyModel KeyVK_S
        {
            get
            {
                if (vk_S == null)
                {
                    vk_S = new KeyModel("VK_S", 0x0073, 0x0053);
                }
                return vk_S;
            }
        }
        protected KeyModel vk_S;
        #endregion

        #region 28: KeyVK_D
        /// <summary>
        /// 28
        /// </summary>
        public virtual KeyModel KeyVK_D
        {
            get
            {
                if (vk_D == null)
                {
                    vk_D = new KeyModel("VK_D", 0x0064, 0x0044);
                }
                return vk_D;
            }
        }
        protected KeyModel vk_D;
        #endregion

        #region 29: KeyVK_F
        /// <summary>
        /// 29
        /// </summary>
        public virtual KeyModel KeyVK_F
        {
            get
            {
                if (vk_F == null)
                {
                    vk_F = new KeyModel("VK_F", 0x0066, 0x0046);
                }
                return vk_F;
            }
        }
        protected KeyModel vk_F;
        #endregion

        #region 30: KeyVK_G
        /// <summary>
        /// 30
        /// </summary>
        public virtual KeyModel KeyVK_G
        {
            get
            {
                if (vk_G == null)
                {
                    vk_G = new KeyModel("VK_G", 0x0067, 0x0047);
                }
                return vk_G;
            }
        }
        protected KeyModel vk_G;
        #endregion

        #region 31: KeyVK_H
        /// <summary>
        /// 31
        /// </summary>
        public virtual KeyModel KeyVK_H
        {
            get
            {
                if (vk_H == null)
                {
                    vk_H = new KeyModel("VK_H", 0x0068, 0x0048);
                }
                return vk_H;
            }
        }
        protected KeyModel vk_H;
        #endregion

        #region 32: KeyVK_J
        /// <summary>
        /// 32
        /// </summary>
        public virtual KeyModel KeyVK_J
        {
            get
            {
                if (vk_J == null)
                {
                    vk_J = new KeyModel("VK_J", 0x006A, 0x004A);
                }
                return vk_J;
            }
        }
        protected KeyModel vk_J;
        #endregion

        #region 33: KeyVK_K
        /// <summary>
        /// 33
        /// </summary>
        public virtual KeyModel KeyVK_K
        {
            get
            {
                if (vk_K == null)
                {
                    vk_K = new KeyModel("VK_K", 0x006B, 0x004B);
                }
                return vk_K;
            }
        }
        protected KeyModel vk_K;
        #endregion

        #region 34: KeyVK_L
        /// <summary>
        /// 34
        /// </summary>
        public virtual KeyModel KeyVK_L
        {
            get
            {
                if (vk_L == null)
                {
                    vk_L = new KeyModel("VK_L", 0x006C, 0x004C);
                }
                return vk_L;
            }
        }
        protected KeyModel vk_L;
        #endregion

        #region 35: KeyVK_Oem1 (VK_OEM_1)
        /// <summary>
        /// 35
        /// </summary>
        public virtual KeyModel KeyVK_Oem1
        {
            get
            {
                if (vk_Oem1 == null)
                {
                    vk_Oem1 = new KeyModelWithTwoGlyphs("VK_Oem1", 0x003B, 0x003A, "Semicolon", "Colon", false);
                }
                return vk_Oem1;
            }
        }
        protected KeyModel vk_Oem1;
        #endregion

        #region 36: KeyVK_Oem7 (VK_OEM_7)
        /// <summary>
        /// 36  Apostrophe/Double-Quotation mark
        /// </summary>
        public virtual KeyModel KeyVK_Oem7
        {
            get
            {
                if (vk_Oem7 == null)
                {
                    vk_Oem7 = new KeyModelWithTwoGlyphs("VK_Oem7", 0x0027, 0x0022, "Apostrophe", "Double-Quotation mark", false);
                }
                return vk_Oem7;
            }
        }
        protected KeyModel vk_Oem7;
        #endregion

        #region 37: KeyVK_Z
        /// <summary>
        /// 37
        /// </summary>
        public virtual KeyModel KeyVK_Z
        {
            get
            {
                if (vk_Z == null)
                {
                    vk_Z = new KeyModel("VK_Z", 0x007A, 0x005A);
                }
                return vk_Z;
            }
        }
        protected KeyModel vk_Z;
        #endregion

        #region 38: KeyVK_X
        /// <summary>
        /// 38
        /// </summary>
        public virtual KeyModel KeyVK_X
        {
            get
            {
                if (vk_X == null)
                {
                    vk_X = new KeyModel("VK_X", 0x0078, 0x0058);
                }
                return vk_X;
            }
        }
        protected KeyModel vk_X;
        #endregion

        #region 39: KeyVK_C
        /// <summary>
        /// 39
        /// </summary>
        public virtual KeyModel KeyVK_C
        {
            get
            {
                if (vk_C == null)
                {
                    vk_C = new KeyModel("VK_C", 0x0063, 0x0043);
                }
                return vk_C;
            }
        }
        protected KeyModel vk_C;
        #endregion

        #region 40: KeyVK_V
        /// <summary>
        /// 40
        /// </summary>
        public virtual KeyModel KeyVK_V
        {
            get
            {
                if (vk_V == null)
                {
                    vk_V = new KeyModel("VK_V", 0x0076, 0x0056);
                }
                return vk_V;
            }
        }
        protected KeyModel vk_V;
        #endregion

        #region 41: KeyVK_B
        /// <summary>
        /// 41
        /// </summary>
        public virtual KeyModel KeyVK_B
        {
            get
            {
                if (vk_B == null)
                {
                    vk_B = new KeyModel("VK_B", 0x0062, 0x0042);
                }
                return vk_B;
            }
        }
        protected KeyModel vk_B;
        #endregion

        #region 42: KeyVK_N
        /// <summary>
        /// 42
        /// </summary>
        public virtual KeyModel KeyVK_N
        {
            get
            {
                if (vk_N == null)
                {
                    vk_N = new KeyModel("VK_N", 0x006E, 0x004E);
                }
                return vk_N;
            }
        }
        protected KeyModel vk_N;
        #endregion

        #region 43: KeyVK_M
        /// <summary>
        /// 43
        /// </summary>
        public virtual KeyModel KeyVK_M
        {
            get
            {
                if (vk_M == null)
                {
                    vk_M = new KeyModel("VK_M", 0x006D, 0x004D);
                }
                return vk_M;
            }
        }
        protected KeyModel vk_M;
        #endregion

        #region 44: KeyVK_OemComma
        /// <summary>
        /// 44  Commas / Less-than sign
        /// </summary>
        public virtual KeyModel KeyVK_OemComma
        {
            get
            {
                if (vk_OemComma == null)
                {
                    vk_OemComma = new KeyModelWithTwoGlyphs("VK_OemComma", 0x002C, 0x003C, "Comma", "Less-than sign", false);
                }
                return vk_OemComma;
            }
        }
        protected KeyModel vk_OemComma;
        #endregion

        #region 45: KeyVK_OemPeriod (VK_OEM_PERIOD)
        /// <summary>
        /// 45  Period / Greater-than sign
        /// </summary>
        public virtual KeyModel KeyVK_OemPeriod
        {
            get
            {
                if (vk_OemPeriod == null)
                {
                    vk_OemPeriod = new KeyModelWithTwoGlyphs("VK_OemPeriod", 0x002E, 0x003E, "Period", "Greater-than sign", false);
                }
                return vk_OemPeriod;
            }
        }
        protected KeyModel vk_OemPeriod;
        #endregion

        #region 46: KeyVK_OemQuestion (VK_OEM_2)
        /// <summary>
        /// 46  Solidus
        /// </summary>
        public virtual KeyModel KeyVK_OemQuestion
        {
            get
            {
                if (vk_OemQuestion == null)
                {
                    vk_OemQuestion = new KeyModel("VK_OemQuestion", 0x002F, "Solidus", false);
                }
                return vk_OemQuestion;
            }
        }
        protected KeyModel vk_OemQuestion;
        #endregion

        #endregion The individual key assignments

        #region ToString
        /// <summary>
        /// Override the ToString method to give a better indication of what this object represents.
        /// </summary>
        /// <returns>A description of the state of this object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("KeyAssignmentSet(");
            sb.Append(KeyboardLayout);
            if (IsAltGrKeyUsed)
            {
                sb.Append(", IsAltGrKeyUsed=true,");
            }
            else
            {
                sb.Append(", ");
            }
            sb.Append("KeycapFontFamily=");
            sb.Append("\"");
            sb.Append(KeycapFontFamilyName);
            sb.Append("\"");
            sb.Append(")");
            return sb.ToString();
        }
        #endregion
    }
    #endregion
}
