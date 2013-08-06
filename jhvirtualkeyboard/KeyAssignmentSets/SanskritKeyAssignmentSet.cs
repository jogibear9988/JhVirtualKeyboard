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
    /// The SanskritKeyAssignments subclass of DefaultKeyAssignments overrides those KeyAssignments that need to differ
    /// from those of the English keyboard.
    /// </summary>
    public class SanskritKeyAssignmentSet : KeyAssignmentSet
    {
        public SanskritKeyAssignmentSet()
            : base()
        {
        }

        #region KeyboardLayout
        /// <summary>
        /// Get the WhichKeyboardLayout this KeyAssignmentSet is used for.
        /// </summary>
        public override WhichKeyboardLayout KeyboardLayout
        {
            get { return WhichKeyboardLayout.Sanskrit; }
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
                return "Arial Unicode MS";
            }
        }
        #endregion

        #region FontSizeForStackedGlyphs
        /// <summary>
        /// Get the FontSize to use for keycaps that have glyphs arranged in two rows, one over the other,
        /// such that a smaller FontSize is needed as opposed to when just a single glyph is to be shown.
        /// </summary>
        public override double FontSizeForStackedGlyphs
        {
            get { return 16.0; }
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
                    vk_Oem3 = new KeyModelWithTwoGlyphs("VK_Oem3", 0x0060, 0x007E, "Grace Accent", "Tilde", false);
                    
                }
                return vk_Oem3;
            }
        }
       
        #endregion

        #region 1:  KeyVK_1
        public override  KeyModel KeyVK_1
        {
            get
            {
                if (vk_1 == null)
                {
                    vk_1 = new KeyModelWithTwoGlyphs("VK_1", 0x0967, 0x0021, "digit ONE", "Exclamation Mark", false);
                }
                return vk_1;
            }
        }
    
        #endregion

        #region 2:  KeyVK_2
        public override  KeyModel KeyVK_2
        {
            get
            {
                if (vk_2 == null)
                {
                    vk_2 = new KeyModelWithTwoGlyphs("VK_2", 0x0968, 0x0040, "digit TWO", "At sign", false);
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
                    vk_3 = new KeyModelWithTwoGlyphs("VK_3", 0x0969, 0x0023, "digit THREE", "Number sign", false);
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
                    vk_4 = new KeyModelWithTwoGlyphs("VK_4", 0x096A, 0x0024, "digit FOUR", "Dollar sign", false);
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
                    vk_5 = new KeyModelWithTwoGlyphs("VK_5", 0x096B, 0x0025, "digit FIVE", "Percent sign", false);
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
                    vk_6 = new KeyModelWithTwoGlyphs("VK_6", 0x096C, 0x005E, "digit SIX", "Circumflex accent", false);
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
                    vk_7 = new KeyModelWithTwoGlyphs("VK_7", 0x096D, 0x0026, "digit SEVEN", "Ampersand", false);
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
                    vk_8 = new KeyModelWithTwoGlyphs("VK_8", 0x096E, 0x002A, "digit EIGHT", "Asterisk", false);
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
                    vk_9 = new KeyModelWithTwoGlyphs("VK_9", 0x096F, 0x0028, "digit NINE", "Left Parenthesis", false);
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
                    vk_0 = new KeyModelWithTwoGlyphs("VK_0", 0x0966, 0x0029, "digit ZERO", "Right Parenthesis", false);
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
                    vk_OemMinus = new KeyModelWithTwoGlyphs("VK_OemMinus", 0x002D, 0x005F, "Hyphen,Minus", "Underscore", false);
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
                    vk_OemPlus = new KeyModelWithTwoGlyphs("VK_OemPlus", 0x03D, 0x002B, "Equals sign", "Plus sign", false);
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
                    //TODO: What da?!
                    vk_Q = new KeyModelWithTwoGlyphs("VK_Q", 0x0905, 0x094D, "Letter a", "Basic letter", false);  //letter a and no vowel
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
                    vk_W = new KeyModelWithTwoGlyphs("VK_W", 0x0906, 0x093E, "Letter A", "Vowel A", false); //letter A and first vowel
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
                    vk_E = new KeyModelWithTwoGlyphs("VK_E", 0x0907, 0x093F, "Letter e", "Vowel e", false);  // letter e
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
                    vk_R = new KeyModelWithTwoGlyphs("VK_R", 0x0908, 0x0940, "Letter E", "Vowel E", false);  //letter E
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
                    vk_T = new KeyModelWithTwoGlyphs("VK_T", 0x0909, 0x0941, "Letter u", "Vowel u", false);  //letter u
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
                    vk_Y = new KeyModelWithTwoGlyphs("VK_Y", 0x090A, 0x0942, "Letter U", "Vowel U", false);  //letter U
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
                    vk_U = new KeyModelWithTwoGlyphs("VK_U", 0x090B, 0x0943, "Letter RRi", "Vowel RRi", false);  //letter RRi
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
                    vk_I = new KeyModelWithTwoGlyphs("VK_I", 0x0960, 0x0944, "Letter RRI", "Vowel RRI", false); //letter RRI
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
                    vk_O = new KeyModelWithTwoGlyphs("VK_O", 0x090F, 0x0947, "Letter e", "Vowel e", false);  //letter e
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
                    vk_P = new KeyModelWithTwoGlyphs("VK_P", 0x0910, 0x0948, "Letter ai", "Vowel ai", false);  //letter ai, addition vowel ai
                }
                return vk_P;
            }
        }
       
        #endregion

        #region 23: KeyVK_OemOpenBrackets
        /// <summary>
        /// 23  Left brackets
        /// </summary>
        public override KeyModel KeyVK_OemOpenBrackets
        {
            get
            {
                if (vk_OemOpenBrackets == null)
                {
                    vk_OemOpenBrackets = new KeyModelWithTwoGlyphs("VK_OemOpenBrackets", 0x0913, 0x094B, "Vowel O", "Addition Vowel O", false); //letter 0
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
                    vk_Oem6 = new KeyModelWithTwoGlyphs("VK_Oem6", 0x0914, 0x094C, "Vowel au", "Addition vowel au", false);
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
                    vk_Oem5 = new KeyModelWithTwoGlyphs("VK_Oem5", 0x0964, 0x0965, "Single Danda", "Double Danda", true);
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
                    vk_A = new KeyModelWithTwoGlyphs("VK_A", 0x0915, 0x0916, "Letter ka", "Letter kha", false);  //letter ka, kha
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
                    vk_S = new KeyModelWithTwoGlyphs("VK_S", 0x0917, 0x0918, "Letter ga", "Letter gha", false); //letter ga, gha
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
                    vk_D = new KeyModelWithTwoGlyphs("VK_D", 0x0919, 0x093D, "Letter gya", "Letter conjunction a", false); //GYa, .a
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
                    vk_F = new KeyModelWithTwoGlyphs("VK_F", 0x091A, 0x091B, "Letter ca", "Letter cha", false); //ca, cha
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
                    vk_G = new KeyModelWithTwoGlyphs("VK_G", 0x091C, 0x091D, "Letter ja", "Letter jha", false); //ja jha
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
                    vk_H = new KeyModelWithTwoGlyphs("VK_H", 0x091E, 0x0902, "Letter nya", "Letter M", false);  //nya, M
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
                    vk_J = new KeyModelWithTwoGlyphs("VK_J", 0x091F, 0x0920, "Letter Ta", "Letter Tha", false);  //Ta, Tha
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
                    vk_K = new KeyModelWithTwoGlyphs("VK_K", 0x0921, 0x0922, "Letter Da", "Letter Dha", false);  //Da, Dha
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
                    vk_L = new KeyModelWithTwoGlyphs("VK_L", 0x0923, 0x0901, "Letter Na", "Candrabindhu", false);  //letter Na, candrabindhu
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
                    vk_Oem1 = new KeyModelWithTwoGlyphs("VK_Oem1", 0x0903, 0x200C, "Visarga", "Zero Width Non-joiner", false);
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
                    vk_Oem7 = new KeyModelWithTwoGlyphs("VK_Oem7", 0x0027, 0x0022, "Apostrophe", "Quotation mark", false);
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
                    vk_Z = new KeyModelWithTwoGlyphs("VK_Z", 0x0924, 0x0925, "Letter ta", "Letter tha", false);  //letter ta, tha
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
                    vk_X = new KeyModelWithTwoGlyphs("VK_X", 0x0926, 0x0927, "Letter da", "Letter dha", false);  //letter da, dha
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
                    vk_C = new KeyModelWithTwoGlyphs("VK_C", 0x0928, 0x200C, "Letter na", "Zero Width Non-joiner", false);  //na
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
                    vk_V = new KeyModelWithTwoGlyphs("VK_V", 0x092A, 0x092B, "Letter pa", "Letter pha", false);  //letter pa, pha
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
                    vk_B = new KeyModelWithTwoGlyphs("VK_B", 0x092C, 0x092D, "Letter ba", "Letter bha", false);  //letter ba, bha
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
                    vk_N = new KeyModelWithTwoGlyphs("VK_N", 0x092E, 0x092F, "Letter ma", "Letter ya", false);  //ma, ya
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
                    vk_M = new KeyModelWithTwoGlyphs("VK_M", 0x0930, 0x0932, "Letter ra", "Letter la", false);  //ra, la
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
                    vk_OemComma = new KeyModelWithTwoGlyphs("VK_OemComma", 0x0935, 0x0936, "va", "sha", false);
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
                    vk_OemPeriod = new KeyModelWithTwoGlyphs("VK_OemPeriod", 0x0937, 0x0938, "Sha", "sa", false);
                }
                return vk_OemPeriod;
            }
        }
        
        #endregion

        #region 46: KeyVK_OemQuestion
        /// <summary>
        /// 46  Solidus
        /// </summary>
        public override KeyModel KeyVK_OemQuestion
        {
            get
            {
                if (vk_OemQuestion == null)
                {
                    vk_OemQuestion = new KeyModel("VK_OemQuestion", 0x0939, "ha", false);
                }
                return vk_OemQuestion;
            }
        }
        
        #endregion
    }
}
