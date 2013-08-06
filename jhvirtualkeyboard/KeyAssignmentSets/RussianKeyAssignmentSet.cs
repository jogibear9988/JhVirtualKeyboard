// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhVirtualKeyboard</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using JhVirtualKeyboard.ViewModels;


namespace JhVirtualKeyboard.KeyAssignmentSets
{
    /// <summary>
    /// The RussianKeyAssignmentSet subclass of KeyAssignmentSet overrides those KeyAssignments that need to differ
    /// from those of the English keyboard.
    /// </summary>
    public class RussianKeyAssignmentSet : KeyAssignmentSet
    {
        public RussianKeyAssignmentSet()
            : base()
        {
        }

        #region KeyboardLayout
        /// <summary>
        /// Get the WhichKeyboardLayout this KeyAssignmentSet is used for.
        /// </summary>
        public override WhichKeyboardLayout KeyboardLayout
        {
            get { return WhichKeyboardLayout.Russian; }
        }
        #endregion

        #region KeycapFontFamilyName
        /// <summary>
        /// Get the name of the FontFamily that we want to use to display the keycaps of the keyboard in, when using this Russian keyboard.
        /// </summary>
        public override string KeycapFontFamilyName
        {
            get
            {
                return "Sabon LT CYR";
            }
        }
        #endregion

        #region The individual key assignments

        // This one called for a total override from the English defaults.

        #region 0: KeyVK_Oem3
        /// <summary>
        /// 0  Cyrillic letter IO
        /// </summary>
        public override KeyModel KeyVK_Oem3
        {
            get
            {
                if (vk_Oem3 == null)
                {
                    vk_Oem3 = new KeyModel("VK_Oem3", 0x0451, 0x0401, "Cyrillic lowercase letter IO", "Cyrillic capital letter IO", true);
                }
                return vk_Oem3;
            }
        }
        #endregion

        #region 13:  KeyVK_Q
        /// <summary>
        /// 13  Cyrillic letter SHORT I
        /// </summary>
        public override KeyModel KeyVK_Q
        {
            get
            {
                if (vk_Q == null)
                {
                    vk_Q = new KeyModel("VK_Q", 0x0439, 0x0419, "Cyrillic small letter SHORT I", "Cyrillic capital letter SHORT I", true);
                }
                return vk_Q;
            }
        }
        #endregion

        #region 14: KeyVK_W
        /// <summary>
        /// 14  Cyrillic letter TSE
        /// </summary>
        public override KeyModel KeyVK_W
        {
            get
            {
                if (vk_W == null)
                {
                    vk_W = new KeyModel("VK_W", 0x0446, 0x0426, "Cyrillic small letter TSE", "Cyrillic capital letter TSE", true);
                }
                return vk_W;
            }
        }
        #endregion

        #region 15: KeyVK_E
        /// <summary>
        /// 15  Cyrillic letter U
        /// </summary>
        public override KeyModel KeyVK_E
        {
            get
            {
                if (vk_E == null)
                {
                    vk_E = new KeyModel("VK_E", 0x0443, 0x0423, "Cyrillic small letter U", "Cyrillic capital letter U", true);
                }
                return vk_E;
            }
        }
        #endregion

        #region 16: KeyVK_R
        /// <summary>
        /// 16  Cyrillic letter KA
        /// </summary>
        public override KeyModel KeyVK_R
        {
            get
            {
                if (vk_R == null)
                {
                    vk_R = new KeyModel("VK_R", 0x043A, 0x041A, "Cyrillic small letter KA", "Cyrillic capital letter KA", true);
                }
                return vk_R;
            }
        }
        #endregion

        #region 17: KeyVK_T
        /// <summary>
        /// 17  Cyrillic letter IE
        /// </summary>
        public override KeyModel KeyVK_T
        {
            get
            {
                if (vk_T == null)
                {
                    vk_T = new KeyModel("VK_T", 0x0435, 0x0415, "Cyrillic small letter IE", "Cyrillic capital letter IE", true);
                }
                return vk_T;
            }
        }
        #endregion

        #region 18: KeyVK_Y
        /// <summary>
        /// 18  Cyrillic letter EN
        /// </summary>
        public override KeyModel KeyVK_Y
        {
            get
            {
                if (vk_Y == null)
                {
                    vk_Y = new KeyModel("VK_Y", 0x043D, 0x041D, "Cyrillic small letter EN", "Cyrillic capital letter EN", true);
                }
                return vk_Y;
            }
        }
        #endregion

        #region 19: KeyVK_U
        /// <summary>
        /// 19  Cyrillic letter GHE
        /// </summary>
        public override KeyModel KeyVK_U
        {
            get
            {
                if (vk_U == null)
                {
                    vk_U = new KeyModel("VK_U", 0x0433, 0x0413, "Cyrillic small letter GHE", "Cyrillic capital letter GHE", true);
                }
                return vk_U;
            }
        }
        #endregion

        #region 20: KeyVK_I
        /// <summary>
        /// 20  Cyrillic letter SHA
        /// </summary>
        public override KeyModel KeyVK_I
        {
            get
            {
                if (vk_I == null)
                {
                    vk_I = new KeyModel("VK_I", 0x0448, 0x0428, "Cyrillic small letter SHA", "Cyrillic capital letter SHA", true);
                }
                return vk_I;
            }
        }
        #endregion

        #region 21: KeyVK_O
        /// <summary>
        /// 21  Cyrillic letter SHCHA
        /// </summary>
        public override KeyModel KeyVK_O
        {
            get
            {
                if (vk_O == null)
                {
                    vk_O = new KeyModel("VK_O", 0x0449, 0x0429, "Cyrillic small letter SHCHA", "Cyrillic capital letter SHCHA", true);
                }
                return vk_O;
            }
        }
        #endregion

        #region 22: KeyVK_P
        /// <summary>
        /// 22  Cyrillic letter ZE
        /// </summary>
        public override KeyModel KeyVK_P
        {
            get
            {
                if (vk_P == null)
                {
                    vk_P = new KeyModel("VK_P", 0x0437, 0x0417, "Cyrillic small letter ZE", "Cyrillic capital letter ZE", true);
                }
                return vk_P;
            }
        }
        #endregion

        #region 23: VK_OEM_4
        /// <summary>
        /// 23  Cyrillic letter HA
        /// </summary>
        public override KeyModel KeyVK_OemOpenBrackets
        {
            get
            {
                if (vk_OemOpenBrackets == null)
                {
                    vk_OemOpenBrackets = new KeyModel("VK_OemOpenBrackets", 0x0445, 0x0425, "Cyrillic small letter HA", "Cyrillic capital letter HA", true);
                }
                return vk_OemOpenBrackets;
            }
        }
        #endregion

        #region 24: KeyVK_Oem6
        /// <summary>
        /// 24  Cyrillic HARD SIGN
        /// </summary>
        public override KeyModel KeyVK_Oem6
        {
            get
            {
                if (vk_Oem6 == null)
                {
                    vk_Oem6 = new KeyModel("VK_Oem6", 0x044A, 0x042A, "Cyrillic small HARD SIGN", "Cyrillic uppercase HARD SIGN", true);
                }
                return vk_Oem6;
            }
        }
        #endregion

        #region 26: KeyVK_A
        /// <summary>
        /// 26  Cyrillic letter EF
        /// </summary>
        public override KeyModel KeyVK_A
        {
            get
            {
                if (vk_A == null)
                {
                    vk_A = new KeyModel("VK_A", 0x0444, 0x0424, "Cyrillic lowercase letter EF", "Cyrillic capital letter EF", true);
                }
                return vk_A;
            }
        }
        #endregion

        #region 27: KeyVK_S
        /// <summary>
        /// 27  Cyrillic letter YERU
        /// </summary>
        public override KeyModel KeyVK_S
        {
            get
            {
                if (vk_S == null)
                {
                    vk_S = new KeyModel("VK_S", 0x044B, 0x042B, "Cyrillic lowercase letter YERU", "Cyrillic capital letter YERU", true);
                }
                return vk_S;
            }
        }
        #endregion

        #region 28: KeyVK_D
        /// <summary>
        /// 28  Cyrillic letter VE
        /// </summary>
        public override KeyModel KeyVK_D
        {
            get
            {
                if (vk_D == null)
                {
                    vk_D = new KeyModel("VK_D", 0x0432, 0x0412, "Cyrillic lowercase letter VE", "Cyrillic capital letter VE", true);
                }
                return vk_D;
            }
        }
        #endregion

        #region 29: KeyVK_F
        /// <summary>
        /// 29  Cyrillic letter A
        /// </summary>
        public override KeyModel KeyVK_F
        {
            get
            {
                if (vk_F == null)
                {
                    vk_F = new KeyModel("VK_F", 0x0430, 0x0410, "Cyrillic lowercase letter A", "Cyrillic capital letter A", true);
                }
                return vk_F;
            }
        }
        #endregion

        #region 30: KeyVK_G
        /// <summary>
        /// 30  Cyrillic letter PE
        /// </summary>
        public override KeyModel KeyVK_G
        {
            get
            {
                if (vk_G == null)
                {
                    vk_G = new KeyModel("VK_G", 0x043F, 0x041F, "Cyrillic lowercase letter PE", "Cyrillic capital letter PE", true);
                }
                return vk_G;
            }
        }
        #endregion

        #region 31: KeyVK_H
        /// <summary>
        /// 31  Cyrillic letter ER
        /// </summary>
        public override KeyModel KeyVK_H
        {
            get
            {
                if (vk_H == null)
                {
                    vk_H = new KeyModel("VK_H", 0x0440, 0x0420, "Cyrillic lowercase letter ER", "Cyrillic capital letter ER", true);
                }
                return vk_H;
            }
        }
        #endregion

        #region 32: KeyVK_J
        /// <summary>
        /// 32  Cyrillic letter O
        /// </summary>
        public override KeyModel KeyVK_J
        {
            get
            {
                if (vk_J == null)
                {
                    vk_J = new KeyModel("VK_J", 0x043E, 0x041E, "Cyrillic lowercase letter O", "Cyrillic capital letter O", true);
                }
                return vk_J;
            }
        }
        #endregion

        #region 33: KeyVK_K
        /// <summary>
        /// 33  Cyrillic letter EL
        /// </summary>
        public override KeyModel KeyVK_K
        {
            get
            {
                if (vk_K == null)
                {
                    vk_K = new KeyModel("VK_K", 0x043B, 0x041B, "Cyrillic lowercase letter EL", "Cyrillic capital letter EL", true);
                }
                return vk_K;
            }
        }
        #endregion

        #region 34: KeyVK_L
        /// <summary>
        /// 34  Cyrillic letter DE
        /// </summary>
        public override KeyModel KeyVK_L
        {
            get
            {
                if (vk_L == null)
                {
                    vk_L = new KeyModel("VK_L", 0x0434, 0x0414, "Cyrillic lowercase letter DE", "Cyrillic capital letter DE", true);
                }
                return vk_L;
            }
        }
        #endregion

        #region 35: KeyVK_Oem1
        /// <summary>
        /// 35  Cyrillic letter ZHE
        /// </summary>
        public override KeyModel KeyVK_Oem1
        {
            get
            {
                if (vk_Oem1 == null)
                {
                    vk_Oem1 = new KeyModel("VK_Oem1", 0x0436, 0x0416, "Cyrillic lowercase letter ZHE", "Cyrillic capital letter ZHE", true);
                }
                return vk_Oem1;
            }
        }
        #endregion

        #region 36: VK_OEM_7
        /// <summary>
        /// 36  Cyrillic letter E
        /// </summary>
        public override KeyModel KeyVK_Oem7
        {
            get
            {
                if (vk_Oem7 == null)
                {
                    vk_Oem7 = new KeyModel("VK_Oem7", 0x044D, 0x042D, "Cyrillic lowercase letter E", "Cyrillic capital letter E", true);
                }
                return vk_Oem7;
            }
        }
        #endregion

        #region 37: KeyVK_Z
        /// <summary>
        /// 37  Cyrillic letter YA
        /// </summary>
        public override KeyModel KeyVK_Z
        {
            get
            {
                if (vk_Z == null)
                {
                    vk_Z = new KeyModel("VK_Z", 0x044F, 0x042F, "Cyrillic small letter YA", "Cyrillic capital letter YA", true);
                }
                return vk_Z;
            }
        }
        #endregion

        #region 38: KeyVK_X
        /// <summary>
        /// 38  Cyrillic letter CHE
        /// </summary>
        public override KeyModel KeyVK_X
        {
            get
            {
                if (vk_X == null)
                {
                    vk_X = new KeyModel("VK_X", 0x0447, 0x0427, "Cyrillic small letter CHE", "Cyrillic capital letter CHE", true);
                }
                return vk_X;
            }
        }
        #endregion

        #region 39: KeyVK_C
        /// <summary>
        /// 39  Cyrillic letter ES
        /// </summary>
        public override KeyModel KeyVK_C
        {
            get
            {
                if (vk_C == null)
                {
                    vk_C = new KeyModel("VK_C", 0x0441, 0x0421, "Cyrillic small letter ES", "Cyrillic capital letter ES", true);
                }
                return vk_C;
            }
        }
        #endregion

        #region 40: KeyVK_V
        /// <summary>
        /// 40  Cyrillic letter EM
        /// </summary>
        public override KeyModel KeyVK_V
        {
            get
            {
                if (vk_V == null)
                {
                    vk_V = new KeyModel("VK_V", 0x043C, 0x041C, "Cyrillic small letter EM", "Cyrillic capital letter EM", true);
                }
                return vk_V;
            }
        }
        #endregion

        #region 41: KeyVK_B
        /// <summary>
        /// 41  Cyrillic letter I
        /// </summary>
        public override KeyModel KeyVK_B
        {
            get
            {
                if (vk_B == null)
                {
                    vk_B = new KeyModel("VK_B", 0x0438, 0x0418, "Cyrillic small letter I", "Cyrillic capital letter I", true);
                }
                return vk_B;
            }
        }
        #endregion

        #region 42: KeyVK_N
        /// <summary>
        /// 42  Cyrillic letter TE
        /// </summary>
        public override KeyModel KeyVK_N
        {
            get
            {
                if (vk_N == null)
                {
                    vk_N = new KeyModel("VK_N", 0x0442, 0x0422, "Cyrillic small letter TE", "Cyrillic capital letter TE", true);
                }
                return vk_N;
            }
        }
        #endregion

        #region 43: KeyVK_M
        /// <summary>
        /// 43  Cyrillic SOFT SIGN
        /// </summary>
        public override KeyModel KeyVK_M
        {
            get
            {
                if (vk_M == null)
                {
                    vk_M = new KeyModel("VK_M", 0x044C, 0x042C, "Cyrillic lowercase SOFT SIGN", "Cyrillic uppercase SOFT SIGN", true);
                }
                return vk_M;
            }
        }
        #endregion

        #region 44: KeyVK_OemComma
        /// <summary>
        /// 44  Cyrillic BE
        /// </summary>
        public override KeyModel KeyVK_OemComma
        {
            get
            {
                if (vk_OemComma == null)
                {
                    vk_OemComma = new KeyModel("VK_OemComma", 0x0431, 0x0411, "Cyrillic lowercase BE", "Cyrillic uppercase BE", true);
                }
                return vk_OemComma;
            }
        }
        #endregion

        #region 45: KeyVK_OemPeriod
        /// <summary>
        /// 45  Cyrillic YU
        /// </summary>
        public override KeyModel KeyVK_OemPeriod
        {
            get
            {
                if (vk_OemPeriod == null)
                {
                    vk_OemPeriod = new KeyModel("VK_OemPeriod", 0x044E, 0x042E, "Cyrillic lowercase YU", "Cyrillic uppercase YU", true);
                }
                return vk_OemPeriod;
            }
        }
        #endregion

        #region 46: VK_OEM_2
        /// <summary>
        /// 46  Period
        /// </summary>
        public override KeyModel KeyVK_OemQuestion
        {
            get
            {
                if (vk_OemQuestion == null)
                {
                    vk_OemQuestion = new KeyModel("VK_OemQuestion", 0x002E, "Period", false);
                }
                return vk_OemQuestion;
            }
        }
        #endregion

        #endregion The individual key assignments
    }
}
