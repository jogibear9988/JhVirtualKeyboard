// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhVirtualKeyboard</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System.Windows;
using JhVirtualKeyboard.ViewModels;


namespace JhVirtualKeyboard.KeyAssignmentSets
{
    /// <summary>
    /// The ArabicKeyAssignmentSet subclass of KeyAssignmentSet overrides those KeyAssignments that need to differ
    /// from those of the English keyboard.
    /// </summary>
    public class ArabicKeyAssignmentSet : KeyAssignmentSet
    {
        public ArabicKeyAssignmentSet()
            : base()
        {
        }

        #region KeyboardLayout
        /// <summary>
        /// Get the WhichKeyboardLayout this KeyAssignmentSet is used for.
        /// </summary>
        public override WhichKeyboardLayout KeyboardLayout
        {
            get { return WhichKeyboardLayout.Arabic; }
        }
        #endregion

        #region IsAltGrKeyUsed
        /// <summary>
        /// Get whether the AltGr key (also known as the Alt Graph, Alt Grill, or Alt Char key) is used as a modifier key
        /// in this KeyAssignmentSet. This is false by default, but for this Arabic set we return true.
        /// </summary>
        public override bool IsAltGrKeyUsed
        {
            get { return true; }
        }
        #endregion

        #region FlowDirection
        /// <summary>
        /// Get the FlowDirection associated with this keyboard layout, which in this case is RightToLeft.
        /// </summary>
        public override FlowDirection FlowDirection
        {
            get
            {
                return System.Windows.FlowDirection.RightToLeft;
            }
        }
        #endregion

        #region The individual key assignments
        // This one calls for a total override from the English defaults.
        // See http://pratyeka.org/arabic/, and http://unicode.org/charts/PDF/U0600.pdf
        //
        // But you may want to bare in mind that this is a right-to-left writing system.
        // The string concatenation will not yield what you expect otherwise.

        #region 1:  KeyVK_1
        public override KeyModel KeyVK_1
        {
            get
            {
                if (vk_1 == null)
                {
                    vk_1 = new KeyModelWithThreeGlyphs("VK_1", 0x0031, 0x0021, 0x0661, "digit ONE", "Exclamation Mark", "U+0661 Arabic-Indic digit ONE");
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
                    vk_2 = new KeyModelWithThreeGlyphs("VK_2", 0x0032, 0x0040, 0x0662, "digit TWO", "At sign", "U+0662 Arabic-Indic digit TWO");
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
                    vk_3 = new KeyModelWithThreeGlyphs("VK_3", 0x0033, 0x0023, 0x0663, "digit THREE", "Number sign", "U+0663 Arabic-Indic digit THREE");
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
                    vk_4 = new KeyModelWithThreeGlyphs("VK_4", 0x0034, 0x0024, 0x0664, "digit FOUR", "Dollar sign", "U+0664 Arabic-Indic digit FOUR");
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
                    vk_5 = new KeyModelWithThreeGlyphs("VK_5", 0x0035, 0x0025, 0x0665, "digit FIVE", "Percent sign", "U+0665 Arabic-Indic digit FIVE");
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
                    vk_6 = new KeyModelWithThreeGlyphs("VK_6", 0x0036, 0x005E, 0x0666, "digit SIX", "Circumflex accent", "U+0666 Arabic-Indic digit SIX");
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
                    vk_7 = new KeyModelWithThreeGlyphs("VK_7", 0x0037, 0x0026, 0x0667, "digit SEVEN", "Ampersand", "U+0667 Arabic-Indic digit SEVEN");
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
                    vk_8 = new KeyModelWithThreeGlyphs("VK_8", 0x0038, 0x002A, 0x0668, "digit EIGHT", "Asterisk", "U+0668 Arabic-Indic digit EIGHT");
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
                    vk_9 = new KeyModelWithThreeGlyphs("VK_9", 0x0039, 0x0028, 0x0669, "digit NINE", "Left Parenthesis", "U+0669 Arabic-Indic digit NINE");
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
                    vk_0 = new KeyModelWithThreeGlyphs("VK_0", 0x0030, 0x0029, 0x0660, "digit ZERO", "Right Parenthesis", "U+0660 Arabic-Indic digit ZERO");
                }
                return vk_0;
            }
        }
        #endregion

        #region 13: KeyVK_Q
        /// <summary>
        /// 13  Arabic letter DAD
        /// </summary>
        public override KeyModel KeyVK_Q
        {
            get
            {
                if (vk_Q == null)
                {
                    vk_Q = new KeyModel("VK_Q", 0x0636, "U+0636 Arabic letter DAD", true);
                }
                return vk_Q;
            }
        }
        #endregion

        #region 14: KeyVK_W
        /// <summary>
        /// 14  Arabic letter SAD
        /// </summary>
        public override KeyModel KeyVK_W
        {
            get
            {
                if (vk_W == null)
                {
                    vk_W = new KeyModel("VK_W", 0x0635, "U+0635 Arabic letter SAD", true);
                }
                return vk_W;
            }
        }
        #endregion

        #region 15: KeyVK_E
        /// <summary>
        /// 15  Arabic letter THA, or THEH, DAMMA
        /// </summary>
        public override KeyModel KeyVK_E
        {
            get
            {
                if (vk_E == null)
                {
                    vk_E = new KeyModel("VK_E", 0x062B, "U+062B Arabic letter THEH", true);
                }
                return vk_E;
            }
        }
        #endregion

        #region 16: KeyVK_R
        /// <summary>
        /// 16  Arabic letter QAF
        /// </summary>
        public override KeyModel KeyVK_R
        {
            get
            {
                if (vk_R == null)
                {
                    vk_R = new KeyModel("VK_R", 0x0642, "U+0642 Arabic letter QAF", true);
                }
                return vk_R;
            }
        }
        #endregion

        #region 17: KeyVK_T
        /// <summary>
        /// 17  Arabic letter FA, or FEH
        /// </summary>
        public override KeyModel KeyVK_T
        {
            get
            {
                if (vk_T == null)
                {
                    vk_T = new KeyModel("VK_T", 0x0641, "U+0641 Arabic letter FEH", true);
                }
                return vk_T;
            }
        }
        #endregion

        #region 18: KeyVK_Y
        /// <summary>
        /// 18  Arabic letter GHAYN, or GHAIN
        /// </summary>
        public override KeyModel KeyVK_Y
        {
            get
            {
                if (vk_Y == null)
                {
                    vk_Y = new KeyModel("VK_Y", 0x063A, "U+063A Arabic letter GHAYN", true);
                }
                return vk_Y;
            }
        }
        #endregion

        #region 19: KeyVK_U
        /// <summary>
        /// 19  Arabic letter AYN, or AIN
        /// </summary>
        public override KeyModel KeyVK_U
        {
            get
            {
                if (vk_U == null)
                {
                    vk_U = new KeyModel("VK_U", 0x0639, "U+0639 Arabic letter AYN", true);
                }
                return vk_U;
            }
        }
        #endregion

        #region 20: KeyVK_I
        /// <summary>
        /// 20  Arabic letter HA
        /// </summary>
        public override KeyModel KeyVK_I
        {
            get
            {
                if (vk_I == null)
                {
                    vk_I = new KeyModel("VK_I", 0x0665, "U+0665 Arabic letter HA", true);
                }
                return vk_I;
            }
        }
        #endregion

        #region 21: KeyVK_O
        /// <summary>
        /// 21  Arabic letter KHA
        /// </summary>
        public override KeyModel KeyVK_O
        {
            get
            {
                if (vk_O == null)
                {
                    vk_O = new KeyModel("VK_O", 0x0681, "U+0681 Arabic letter KHA", true);
                }
                return vk_O;
            }
        }
        #endregion

        #region 22: KeyVK_P
        /// <summary>
        /// 22  Arabic letter KA
        /// </summary>
        public override KeyModel KeyVK_P
        {
            get
            {
                if (vk_P == null)
                {
                    vk_P = new KeyModel("VK_P", 0x062D, "U+062D Arabic letter KA", true);
                }
                return vk_P;
            }
        }
        #endregion

        #region 26: KeyVK_A
        /// <summary>
        /// 26  Arabic letter SHIN, or SHEEN
        /// </summary>
        public override KeyModel KeyVK_A
        {
            get
            {
                if (vk_A == null)
                {
                    vk_A = new KeyModel("VK_A", 0x0634, "U+0634 Arabic letter SHEEN", true);
                }
                return vk_A;
            }
        }
        #endregion

        #region 27: KeyVK_S
        /// <summary>
        /// 27  Arabic letter SIN, or SEEN
        /// </summary>
        public override KeyModel KeyVK_S
        {
            get
            {
                if (vk_S == null)
                {
                    vk_S = new KeyModel("VK_S", 0x0633, "U+0633 Arabic letter SEEN", true);
                }
                return vk_S;
            }
        }
        #endregion

        #region 28: KeyVK_D
        /// <summary>
        /// 28  Arabic letter YA, or YEH
        /// </summary>
        public override KeyModel KeyVK_D
        {
            get
            {
                if (vk_D == null)
                {
                    vk_D = new KeyModel("VK_D", 0x064A, "U+064A Arabic letter YEH", true);
                }
                return vk_D;
            }
        }
        #endregion

        #region 29: KeyVK_F
        /// <summary>
        /// 29  Arabic letter BA, or BEH
        /// </summary>
        public override KeyModel KeyVK_F
        {
            get
            {
                if (vk_F == null)
                {
                    vk_F = new KeyModel("VK_F", 0x0628, "U+0628 Arabic letter BEH", true);
                }
                return vk_F;
            }
        }
        #endregion

        #region 30: KeyVK_G
        /// <summary>
        /// 30  Arabic letter LAM
        /// </summary>
        public override KeyModel KeyVK_G
        {
            get
            {
                if (vk_G == null)
                {
                    vk_G = new KeyModel("VK_G", 0x0644, "U+0644 Arabic letter LAM", true);
                }
                return vk_G;
            }
        }
        #endregion

        #region 31: KeyVK_H
        /// <summary>
        /// 31  Arabic letter ALIF, or ALEF 
        /// </summary>
        public override KeyModel KeyVK_H
        {
            get
            {
                if (vk_H == null)
                {
                    vk_H = new KeyModel("VK_H", 0x0627, "U+0627 Arabic letter ALEF", true);
                }
                return vk_H;
            }
        }
        #endregion

        #region 32: KeyVK_J
        /// <summary>
        /// 32  Arabic letter TA, or TEH
        /// </summary>
        public override KeyModel KeyVK_J
        {
            get
            {
                if (vk_J == null)
                {
                    vk_J = new KeyModel("VK_J", 0x062A, "U+062A Arabic letter TEH", true);
                }
                return vk_J;
            }
        }
        #endregion

        #region 33: KeyVK_K
        /// <summary>
        /// 33  Arabic letter NUN, or NOON
        /// </summary>
        public override KeyModel KeyVK_K
        {
            get
            {
                if (vk_K == null)
                {
                    vk_K = new KeyModel("VK_K", 0x0646, "U+0646 Arabic letter NOON", true);
                }
                return vk_K;
            }
        }
        #endregion

        #region 34: KeyVK_L
        /// <summary>
        /// 34  Arabic letter MIM, or MEEM
        /// </summary>
        public override KeyModel KeyVK_L
        {
            get
            {
                if (vk_L == null)
                {
                    vk_L = new KeyModel("VK_L", 0x0645, "U+0645 Arabic letter MEEM", true);
                }
                return vk_L;
            }
        }
        #endregion

        #region 35: KeyVK_Oem1 (VK_OEM_1)
        /// <summary>
        /// 35  Arabic Letter KAF / ?
        /// </summary>
        public override KeyModel KeyVK_Oem1
        {
            get
            {
                if (vk_Oem1 == null)
                {
                    vk_Oem1 = new KeyModelWithTwoGlyphs("VK_Oem1", 0x0643, 0x003A, "U+0643 Arabic Letter KAF", "Colon", false);
                }
                return vk_Oem1;
            }
        }
        #endregion

        #region 36: KeyVK_Oem7 (VK_OEM_7)
        /// <summary>
        /// 36  Arabic Semicolon/Double-Quotation Mark
        /// </summary>
        public override KeyModel KeyVK_Oem7
        {
            get
            {
                if (vk_Oem7 == null)
                {
                    vk_Oem7 = new KeyModelWithTwoGlyphs("VK_Oem7", 0x061B, 0x0022, "Arabic Semicolon", "Double-Quotation mark", false);
                }
                return vk_Oem7;
            }
        }
        #endregion

        #region 37: KeyVK_Z
        /// <summary>
        /// 37  Arabic letter YEH with Hamza Above
        /// </summary>
        public override KeyModel KeyVK_Z
        {
            get
            {
                if (vk_Z == null)
                {
                    vk_Z = new KeyModel("VK_Z", 0x0626, "U+0626 Arabic letter YEH with Hamza above", true);
                }
                return vk_Z;
            }
        }
        #endregion

        #region 38: KeyVK_X
        /// <summary>
        /// 38  Arabic letter KAMZA 
        /// </summary>
        public override KeyModel KeyVK_X
        {
            get
            {
                if (vk_X == null)
                {
                    vk_X = new KeyModel("VK_X", 0x06F6, "U+06F6 Arabic letter KAMZA", true);
                }
                return vk_X;
            }
        }
        #endregion

        #region 39: KeyVK_C
        /// <summary>
        /// 39  Arabic letter WAW with Hamza above
        /// </summary>
        public override KeyModel KeyVK_C
        {
            get
            {
                if (vk_C == null)
                {
                    vk_C = new KeyModel("VK_C", 0x0624, "U+0624 Arabic letter WAW with Hamza above", true);
                }
                return vk_C;
            }
        }
        #endregion

        #region 40: KeyVK_V
        /// <summary>
        /// 40  Arabic letter RA, or REH
        /// </summary>
        public override KeyModel KeyVK_V
        {
            get
            {
                if (vk_V == null)
                {
                    vk_V = new KeyModel("VK_V", 0x0631, "U+0631 Arabic letter REH", true);
                }
                return vk_V;
            }
        }
        #endregion

        #region 41: KeyVK_B
        /// <summary>
        /// 41  Arabic letter ?
        /// </summary>
        public override KeyModel KeyVK_B
        {
            get
            {
                if (vk_B == null)
                {
                    vk_B = new KeyModel("VK_B", 0x0438, "U+0438 Arabic letter ?", true);
                }
                return vk_B;
            }
        }
        #endregion

        #region 42: KeyVK_N
        /// <summary>
        /// 42  Arabic letter Alef Maksura
        /// </summary>
        public override KeyModel KeyVK_N
        {
            get
            {
                if (vk_N == null)
                {
                    vk_N = new KeyModel("VK_N", 0x0649, "U+0649 Arabic letter ALEF MAKSURA", true);
                }
                return vk_N;
            }
        }
        #endregion

        #region 43: KeyVK_M
        /// <summary>
        /// 43  Arabic letter TEH
        /// </summary>
        public override KeyModel KeyVK_M
        {
            get
            {
                if (vk_M == null)
                {
                    vk_M = new KeyModel("VK_M", 0x0629, "U+0629 Arabic letter TEH", true);
                }
                return vk_M;
            }
        }
        #endregion

        #region 44: KeyVK_OemComma
        /// <summary>
        /// 44  Arabic Comma / Less-than sign   TODO: Not done
        /// </summary>
        public override KeyModel KeyVK_OemComma
        {
            get
            {
                if (vk_OemComma == null)
                {
                    vk_OemComma = new KeyModelWithTwoGlyphs("VK_OemComma", 0x060C, 0x003C, "U+060C Arabic Comma", "U+003C Less-than sign", false);
                }
                return vk_OemComma;
            }
        }
        #endregion

        #region 46: KeyVK_OemQuestion
        /// <summary>
        /// 46  Arabic Letter ZAH, Arabic Question Mark
        /// </summary>
        public override KeyModel KeyVK_OemQuestion
        {
            get
            {
                if (vk_OemQuestion == null)
                {
                    vk_OemQuestion = new KeyModelWithTwoGlyphs("VK_OemQuestion", 0x0638, 0x061F, "U+0638 Arabic Letter ZAH", "U+061F Arabic Question Mark", false);
                }
                return vk_OemQuestion;
            }
        }
        #endregion

        #endregion The individual key assignments
    }
}
