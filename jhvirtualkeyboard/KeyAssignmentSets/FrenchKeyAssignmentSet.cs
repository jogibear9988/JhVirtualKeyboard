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
    /// The FrenchKeyAssignments subclass of KeyAssignmentSet overrides those KeyAssignments that need to differ
    /// from those of the English keyboard.
    /// </summary>
    public class FrenchKeyAssignmentSet : KeyAssignmentSet
    {
        public FrenchKeyAssignmentSet()
            : base()
        {
        }

        #region KeyboardLayout
        /// <summary>
        /// Get the WhichKeyboardLayout this KeyAssignmentSet is used for.
        /// </summary>
        public override WhichKeyboardLayout KeyboardLayout
        {
            get { return WhichKeyboardLayout.French; }
        }
        #endregion

        //TODO: This is unfinished. See image at
        // en.wikipedia.org/wiki/File:KB_France.svg

        #region 13:  KeyVK_Q
        /// <summary>
        /// 13  Replace Q with A
        /// </summary>
        public override KeyModel KeyVK_Q
        {
            get
            {
                if (vk_Q == null)
                {
                    vk_Q = new KeyModel("VK_Q", 0x0061, 0x0041);
                }
                return vk_Q;
            }
        }
        #endregion

        #region 14: KeyVK_W
        /// <summary>
        /// 14  Swap the Z and W
        /// </summary>
        public override KeyModel KeyVK_W
        {
            get
            {
                if (vk_W == null)
                {
                    vk_W = new KeyModel("VK_W", 0x007A, 0x005A);
                }
                return vk_W;
            }
        }
        #endregion

        #region 24: KeyVK_Oem6
        /// <summary>
        /// 24  Replace the Right brackets with dollar sign/franc sign
        /// TODO: This should actually get a 3rd, blue symbol - the currency sign.
        /// </summary>
        public override KeyModel KeyVK_Oem6
        {
            get
            {
                if (vk_Oem6 == null)
                {
                    vk_Oem6 = new KeyModelWithTwoGlyphs("VK_Oem6", 0x0024, 0x00A3, "Dollar sign", "Pound sign", false);
                }
                return vk_Oem6;
            }
        }
        #endregion

        #region 26: KeyVK_A
        /// <summary>
        /// 26  Replace A with Q
        /// </summary>
        public override KeyModel KeyVK_A
        {
            get
            {
                if (vk_A == null)
                {
                    vk_A = new KeyModel("VK_A", 0x0071, 0x0051);
                }
                return vk_A;
            }
        }
        #endregion

        #region 35: KeyVK_Oem1
        /// <summary>
        /// 35  Replace the semicolon/colon with M
        /// </summary>
        public override KeyModel KeyVK_Oem1
        {
            get
            {
                if (vk_Oem1 == null)
                {
                    vk_Oem1 = new KeyModel("VK_Oem1", 0x006D, 0x004D);
                }
                return vk_Oem1;
            }
        }
        #endregion

        #region 36: VK_OEM_7
        /// <summary>
        /// 36  Replace Apostrophe/Quotation with Letter U with grave/Micro
        /// </summary>
        public override KeyModel KeyVK_Oem7
        {
            get
            {
                if (vk_Oem7 == null)
                {
                    //TODO: Actually, we may need to allow the U-with-grave to have a CAPS state.
                    vk_Oem7 = new KeyModelWithTwoGlyphs("VK_Oem7", 0x00F9, 0x00B5, "letter U with grave", "Micro sign", false);
                }
                return vk_Oem7;
            }
        }
        #endregion

        #region 37: KeyVK_Z
        /// <summary>
        /// 37  Swap the Z and W
        /// </summary>
        public override KeyModel KeyVK_Z
        {
            get
            {
                if (vk_Z == null)
                {
                    vk_Z = new KeyModel("VK_Z", 0x0077, 0x0057);
                }
                return vk_Z;
            }
        }
        #endregion

    }
}
