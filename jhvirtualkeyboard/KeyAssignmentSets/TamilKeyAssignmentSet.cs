// ****************************************************************************
// <author>V. Sesharaman, with some slight editing by James W Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-9-12</date>
// <project>JhVirtualKeyboard</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using JhVirtualKeyboard.ViewModels;


namespace JhVirtualKeyboard.KeyAssignmentSets
{
    /// <summary>
    /// The TamilKeyAssignments subclass of DefaultKeyAssignments overrides those KeyAssignments that need to differ
    /// from those of the English keyboard.
    /// </summary>
    public class TamilKeyAssignmentSet : KeyAssignmentSet
    {
        public TamilKeyAssignmentSet()
            : base()
        {
        }

        #region KeyboardLayout
        /// <summary>
        /// Get the WhichKeyboardLayout this KeyAssignmentSet is used for.
        /// </summary>
        public override WhichKeyboardLayout KeyboardLayout
        {
            get { return WhichKeyboardLayout.Tamil; }
        }
        #endregion

        #region KeycapFontFamilyName
        /// <summary>
        /// Get the name of the FontFamily that we want to use to display the keycaps of the keyboard in, when using this keyboard.
        /// </summary>
        public override string KeycapFontFamilyName
        {
            get
            {
                return "Latha";
            }
        }
        #endregion

        //TODO: What's really needed for this keyboard-layout is not to reduce the font-size (for the stacked-glyph keys)
        // to 12 from 14, but rather to reduce the vertical spacing so that they'll fit on the key-cap.

        #region FontSizeForStackedGlyphs
        /// <summary>
        /// Get the FontSize to use for keycaps that have glyphs arranged in two rows, one over the other, or else side-by-side,
        /// such that a smaller FontSize is needed as opposed to when just a single glyph is to be shown.
        /// </summary>
        public override double FontSizeForStackedGlyphs
        {
            get { return 13.0; }
        }
        #endregion

        #region IsHelpButtonVisible
        /// <summary>
        /// Get whether the help-button, which has the little question-mark icon, is visible.
        /// </summary>
        public override bool IsHelpButtonVisible
        {
            get { return true; }
        }
        #endregion

        #region 0:  KeyVK_Oem3
        /// <summary>
        /// Get the KeyAssignment for KeyVK_Oem3 (0: Grace Accent).
        /// </summary>
        public override  KeyModel KeyVK_Oem3
        {
            get
            {
                if (vk_Oem3 == null)
                {
                    vk_Oem3 = new KeyModelWithTwoGlyphs("VK_Oem3", 0x0021, 0x002A, "Exclamation Mark", "Asterisk", false);
                }
                return vk_Oem3;
            }
        }

        #endregion

        #region 1:  KeyVK_1
        public override KeyModel KeyVK_1
        {
            get
            {
                if (vk_1 == null)
                {
                    vk_1 = new KeyModelWithTwoGlyphs("VK_1", 0x0BE7, 0x0031, "Tamil ONE", "digit ONE", false);
                }
                return vk_1;
            }
        }

        #endregion

        #region 2:  KeyVK_2
        public override KeyModel KeyVK_2
        {
            get
            {
                if (vk_2 == null)
                {
                    vk_2 = new KeyModelWithTwoGlyphs("VK_2", 0x0BE8, 0x0032, "Tamil TWO", "digit TWO", false);
                }
                return vk_2;
            }
        }

        #endregion

        #region 3:  KeyVK_3
        public override KeyModel KeyVK_3
        {
            get
            {
                if (vk_3 == null)
                {
                    vk_3 = new KeyModelWithTwoGlyphs("VK_3", 0x0BE9, 0x0033, "Tamil THREE", "digit THREE", false);
                }
                return vk_3;
            }
        }

        #endregion

        #region 4:  KeyVK_4
        public override KeyModel KeyVK_4
        {
            get
            {
                if (vk_4 == null)
                {
                    vk_4 = new KeyModelWithTwoGlyphs("VK_4", 0x0BEA, 0x0034, "Tamil FOUR", "digit FOUR", false);
                }
                return vk_4;
            }
        }

        #endregion

        #region 5:  KeyVK_5
        public override KeyModel KeyVK_5
        {
            get
            {
                if (vk_5 == null)
                {
                    vk_5 = new KeyModelWithTwoGlyphs("VK_5", 0x0BEB, 0x0035, "Tamil FIVE", "digit FIVE", false);
                }
                return vk_5;
            }
        }

        #endregion

        #region 6:  KeyVK_6
        public override KeyModel KeyVK_6
        {
            get
            {
                if (vk_6 == null)
                {
                    vk_6 = new KeyModelWithTwoGlyphs("VK_6", 0x0BEC, 0x0036, "Tamil SIX", "digit SIX", false);
                }
                return vk_6;
            }
        }

        #endregion

        #region 7:  KeyVK_7
        public override KeyModel KeyVK_7
        {
            get
            {
                if (vk_7 == null)
                {
                    vk_7 = new KeyModelWithTwoGlyphs("VK_7", 0x0BED, 0x0037, "Tamil SEVEN", "digit SEVEN", false);
                }
                return vk_7;
            }
        }

        #endregion

        #region 8:  KeyVK_8
        public override KeyModel KeyVK_8
        {
            get
            {
                if (vk_8 == null)
                {
                    vk_8 = new KeyModelWithTwoGlyphs("VK_8", 0x0BEE, 0x0038, "Tamil EIGHT", "digit EIGHT", false);
                }
                return vk_8;
            }
        }

        #endregion

        #region 9:  KeyVK_9
        public override KeyModel KeyVK_9
        {
            get
            {
                if (vk_9 == null)
                {
                    vk_9 = new KeyModelWithTwoGlyphs("VK_9", 0x0BEF, 0x0039, "Tamil NINE", "digit NINE", false);
                }
                return vk_9;
            }
        }

        #endregion

        #region 10: KeyVK_0
        public override KeyModel KeyVK_0
        {
            get
            {
                if (vk_0 == null)
                {
                    vk_0 = new KeyModel("VK_0", 0x0030, "digit ZERO", false);
                }
                return vk_0;
            }
        }

        #endregion

        #region 11: KeyVK_OemMinus
        /// <summary>
        /// 11  Hyphen
        /// </summary>
        public override KeyModel KeyVK_OemMinus
        {
            get
            {
                if (vk_OemMinus == null)
                {
                    vk_OemMinus = new KeyModelWithTwoGlyphs("VK_OemMinus", 0x002D, 0x002E, "Hyphen,Minus", "Period", false);
                }
                return vk_OemMinus;
            }
        }

        #endregion

        #region 12: KeyVK_OemPlus
        public override KeyModel KeyVK_OemPlus
        {
            get
            {
                if (vk_OemPlus == null)
                {
                    vk_OemPlus = new KeyModelWithTwoGlyphs("VK_OemPlus", 0x028, 0x0029, "Left Parenthesis", "Right Parenthesis", false);
                }
                return vk_OemPlus;
            }
        }

        #endregion

        #region 13:  KeyVK_Q
        public override KeyModel KeyVK_Q
        {
            get
            {
                if (vk_Q == null)
                {
                    vk_Q = new KeyModelWithTwoGlyphs("VK_Q", 0x0B85, 0x0BCD, "Letter a", "letter with upper dot", false);  //letter a and upper dot vowel
                }
                return vk_Q;
            }
        }

        #endregion

        #region 14: KeyVK_W
        public override KeyModel KeyVK_W
        {
            get
            {
                if (vk_W == null)
                {
                    vk_W = new KeyModelWithTwoGlyphs("VK_W", 0x0B86, 0x0BBE, "Letter A", "letter with leg", false); //letter nga
                }
                return vk_W;
            }
        }

        #endregion

        #region 15: KeyVK_E
        /// <summary>
        /// 15
        /// </summary>
        public override KeyModel KeyVK_E
        {
            get
            {
                if (vk_E == null)
                {
                    vk_E = new KeyModelWithTwoGlyphs("VK_E", 0x0B87, 0x0BBF, "Letter i", "addition vowel", false);  // letter e
                }
                return vk_E;
            }
        }

        #endregion

        #region 16: KeyVK_R
        /// <summary>
        /// 16
        /// </summary>
        public override KeyModel KeyVK_R
        {
            get
            {
                if (vk_R == null)
                {
                    vk_R = new KeyModelWithTwoGlyphs("VK_R", 0x0B88, 0x0BC0, "Letter E", "Addition Vowel E", false);  //letter E
                }
                return vk_R;
            }
        }

        #endregion

        #region 17: KeyVK_T
        /// <summary>
        /// 17
        /// </summary>
        public override KeyModel KeyVK_T
        {
            get
            {
                if (vk_T == null)
                {
                    vk_T = new KeyModelWithTwoGlyphs("VK_T", 0x0B89, 0x0BC1, "Letter u", "Addition Vowel", false);  //letter u
                }
                return vk_T;
            }
        }

        #endregion

        #region 18: KeyVK_Y
        /// <summary>
        /// 18
        /// </summary>
        public override KeyModel KeyVK_Y
        {
            get
            {
                if (vk_Y == null)
                {
                    vk_Y = new KeyModelWithTwoGlyphs("VK_Y", 0x0B8A, 0x0BC2, "Letter U", "Addition Vowel", false);  //letter U
                }
                return vk_Y;
            }
        }

        #endregion

        #region 19:  KeyVK_U
        /// <summary>
        /// 19
        /// </summary>
        public override KeyModel KeyVK_U
        {
            get
            {
                if (vk_U == null)
                {
                    vk_U = new KeyModelWithTwoGlyphs("VK_U", 0x0B8E, 0x0BC6, "Letter e", "Addition Vowel", false);  //letter RRi
                }
                return vk_U;
            }
        }

        #endregion

        #region 20: KeyVK_I
        public override KeyModel KeyVK_I
        {
            get
            {
                if (vk_I == null)
                {
                    vk_I = new KeyModelWithTwoGlyphs("VK_I", 0x0B8F, 0x0BC7, "Letter E", "Addition Vowel", false); //letter RRI
                }
                return vk_I;
            }
        }

        #endregion

        #region 21: KeyVK_O
        /// <summary>
        /// 21
        /// </summary>
        public override KeyModel KeyVK_O
        {
            get
            {
                if (vk_O == null)
                {
                    vk_O = new KeyModelWithTwoGlyphs("VK_O", 0x0B90, 0x0BC8, "Letter ai", "Addition Vowel", false);  //letter e
                }
                return vk_O;
            }
        }

        #endregion

        #region 22: KeyVK_P
        /// <summary>
        /// 22
        /// </summary>
        public override KeyModel KeyVK_P
        {
            get
            {
                if (vk_P == null)
                {
                    vk_P = new KeyModelWithTwoGlyphs("VK_P", 0x0B92, 0x0BCA, "Letter o", "Addition Vowel", false);  //letter ai
                }
                return vk_P;
            }
        }

        #endregion

        #region 23: VK_OEM_4, [ + {
        /// <summary>
        /// 23  Left brackets
        /// </summary>
        public override KeyModel KeyVK_OemOpenBrackets
        {
            get
            {
                if (vk_OemOpenBrackets == null)
                {
                    vk_OemOpenBrackets = new KeyModelWithTwoGlyphs("VK_OemOpenBrackets", 0x0B93, 0x0BCB, "Letter O", "Addition Vowel", false); //letter 0
                }
                return vk_OemOpenBrackets;
            }
        }

        #endregion

        #region 24: KeyVK_Oem6
        /// <summary>
        /// 24  Right brackets
        /// </summary>
        public override KeyModel KeyVK_Oem6
        {
            get
            {
                if (vk_Oem6 == null)
                {
                    vk_Oem6 = new KeyModelWithTwoGlyphs("VK_Oem6", 0x0B94, 0x0BCC, "Vowel au", "Aytham", false);
                }
                return vk_Oem6;
            }
        }

        #endregion

        #region 25: KeyVK_Oem5
        /// <summary>
        /// 25
        /// </summary>
        public override KeyModel KeyVK_Oem5
        {
            get
            {
                if (vk_Oem5 == null)
                {
                    vk_Oem5 = new KeyModel("VK_Oem5", 0x0B83, "Letter aytham", false);
                }
                return vk_Oem5;
            }
        }

        #endregion

        #region 26: KeyVK_A
        /// <summary>
        /// 26
        /// </summary>
        public override KeyModel KeyVK_A
        {
            get
            {
                if (vk_A == null)
                {
                    vk_A = new KeyModel("VK_A", 0x0B95, "LETTER ka", false);
                }
                return vk_A;
            }
        }

        #endregion

        #region 27: KeyVK_S
        /// <summary>
        /// 27
        /// </summary>
        public override KeyModel KeyVK_S
        {
            get
            {
                if (vk_S == null)
                {
                    vk_S = new KeyModel("VK_S", 0x0B99, "LETTER nga", false);
                }
                return vk_S;
            }
        }

        #endregion

        #region 28: KeyVK_D
        /// <summary>
        /// 28
        /// </summary>
        public override KeyModel KeyVK_D
        {
            get
            {
                if (vk_D == null)
                {
                    vk_D = new KeyModel("VK_D", 0x0B9A, "LETTER ca", false);
                }
                return vk_D;
            }
        }

        #endregion

        #region 29: KeyVK_F
        /// <summary>
        /// 29
        /// </summary>
        public override KeyModel KeyVK_F
        {
            get
            {
                if (vk_F == null)
                {
                    vk_F = new KeyModel("VK_F", 0x0B9E, "LETTER GYa", false);
                }
                return vk_F;
            }
        }

        #endregion

        #region 30: KeyVK_G
        /// <summary>
        /// 30
        /// </summary>
        public override KeyModel KeyVK_G
        {
            get
            {
                if (vk_G == null)
                {
                    vk_G = new KeyModel("VK_G", 0x0B9F, "LETTER Ta", false);
                }
                return vk_G;
            }
        }

        #endregion

        #region 31: KeyVK_H
        /// <summary>
        /// 31
        /// </summary>
        public override KeyModel KeyVK_H
        {
            get
            {
                if (vk_H == null)
                {
                    vk_H = new KeyModel("VK_H", 0x0BA3, "LETTER Na", false);
                }
                return vk_H;
            }
        }

        #endregion

        #region 32: KeyVK_J
        /// <summary>
        /// 32
        /// </summary>
        public override KeyModel KeyVK_J
        {
            get
            {
                if (vk_J == null)
                {
                    vk_J = new KeyModel("VK_J", 0x0BA4, "LETTER tha", false);
                }
                return vk_J;
            }
        }

        #endregion

        #region 33: KeyVK_K
        /// <summary>
        /// 33
        /// </summary>
        public override KeyModel KeyVK_K
        {
            get
            {
                if (vk_K == null)
                {
                    vk_K = new KeyModel("VK_K", 0x0BA8, "LETTER na", false);
                }
                return vk_K;
            }
        }

        #endregion

        #region 34: KeyVK_L
        /// <summary>
        /// 34
        /// </summary>
        public override KeyModel KeyVK_L
        {
            get
            {
                if (vk_L == null)
                {
                    vk_L = new KeyModel("VK_L", 0x0BA9, "LETTER nha", false);
                }
                return vk_L;
            }
        }

        #endregion

        #region 35: KeyVK_Oem1
        /// <summary>
        /// 35
        /// </summary>
        public override KeyModel KeyVK_Oem1
        {
            get
            {
                if (vk_Oem1 == null)
                {
                    vk_Oem1 = new KeyModel("VK_Oem1", 0x0BAA, "LETTER pa", false);
                }
                return vk_Oem1;
            }
        }

        #endregion

        #region 36: KeyVK_Oem7
        /// <summary>
        /// 36  Apostrophe/Quotation mark
        /// </summary>
        public override KeyModel KeyVK_Oem7
        {
            get
            {
                if (vk_Oem7 == null)
                {
                    vk_Oem7 = new KeyModel("VK_Oem7", 0x0BAE, "LETTER pa", false);
                }
                return vk_Oem7;
            }
        }

        #endregion

        #region 37: KeyVK_Z
        /// <summary>
        /// 37
        /// </summary>
        public override KeyModel KeyVK_Z
        {
            get
            {
                if (vk_Z == null)
                {
                    vk_Z = new KeyModel("VK_Z", 0x0BAF, "LETTER ya", false);
                }
                return vk_Z;
            }
        }

        #endregion

        #region 38: KeyVK_X
        /// <summary>
        /// 38
        /// </summary>
        public override KeyModel KeyVK_X
        {
            get
            {
                if (vk_X == null)
                {
                    vk_X = new KeyModel("VK_X", 0x0BB0, "LETTER ra", false);
                }
                return vk_X;
            }
        }

        #endregion

        #region 39: KeyVK_C
        /// <summary>
        /// 39
        /// </summary>
        public override KeyModel KeyVK_C
        {
            get
            {
                if (vk_C == null)
                {
                    vk_C = new KeyModel("VK_C", 0x0BB1, "LETTER Ra", false);
                }
                return vk_C;
            }
        }

        #endregion

        #region 40: KeyVK_V
        /// <summary>
        /// 40
        /// </summary>
        public override KeyModel KeyVK_V
        {
            get
            {
                if (vk_V == null)
                {
                    vk_V = new KeyModel("VK_V", 0x0BB2, "LETTER la", false);
                }
                return vk_V;
            }
        }

        #endregion

        #region 41: KeyVK_B
        /// <summary>
        /// 41
        /// </summary>
        public override KeyModel KeyVK_B
        {
            get
            {
                if (vk_B == null)
                {
                    vk_B = new KeyModel("VK_B", 0x0BB3, "LETTER La", false);
                }
                return vk_B;
            }
        }

        #endregion

        #region 42: KeyVK_N
        /// <summary>
        /// 42
        /// </summary>
        public override KeyModel KeyVK_N
        {
            get
            {
                if (vk_N == null)
                {
                    vk_N = new KeyModel("VK_N", 0x0BB4, "LETTER za", false);
                }
                return vk_N;
            }
        }

        #endregion

        #region 43: KeyVK_M
        /// <summary>
        /// 43
        /// </summary>
        public override KeyModel KeyVK_M
        {
            get
            {
                if (vk_M == null)
                {
                    vk_M = new KeyModel("VK_M", 0x0BB5, "LETTER va", false);
                }
                return vk_M;
            }
        }

        #endregion

        #region 44: KeyVK_OemComma
        /// <summary>
        /// 44  Commas / Less-than sign
        /// </summary>
        public override KeyModel KeyVK_OemComma
        {
            get
            {
                if (vk_OemComma == null)
                {
                    vk_OemComma = new KeyModel("VK_OemComma", 0x0B9C, "Letter ja", false);
                }
                return vk_OemComma;
            }
        }

        #endregion

        #region 45: KeyVK_OemPeriod
        /// <summary>
        /// 45  Period / Greater-than sign
        /// </summary>
        public override KeyModel KeyVK_OemPeriod
        {
            get
            {
                if (vk_OemPeriod == null)
                {
                    vk_OemPeriod = new KeyModel("VK_OemPeriod", 0x002E, "Period", false);
                }
                return vk_OemPeriod;
            }
        }

        #endregion
    }
}
