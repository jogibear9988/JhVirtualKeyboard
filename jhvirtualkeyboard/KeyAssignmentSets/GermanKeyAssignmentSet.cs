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
    /// The GermanKeyAssignments subclass of DefaultKeyAssignments overrides those KeyAssignments that need to differ
    /// from those of the English keyboard.
    /// </summary>
    public class GermanKeyAssignmentSet : KeyAssignmentSet
    {
        public GermanKeyAssignmentSet()
            : base()
        {
        }

        #region KeyboardLayout
        /// <summary>
        /// Get the WhichKeyboardLayout this KeyAssignmentSet is used for.
        /// </summary>
        public override WhichKeyboardLayout KeyboardLayout
        {
            get { return WhichKeyboardLayout.GermanAustrian; }
        }
        #endregion

        // See http://en.wikipedia.org/wiki/File:KB_Germany.svg

        #region 11: KeyVK_OemMinus
        /// <summary>
        /// 11  Replace the minus/underscore with Eszett (also called scharfes S)
        ///     German keyboard has Question-mark in the upper location. I'm deciding not to.
        /// </summary>
        public override KeyModel KeyVK_OemMinus
        {
            // See http://en.wikipedia.org/wiki/B <- that last char is the eszett.
            get
            {
                if (vk_OemMinus == null)
                {
                    vk_OemMinus = new KeyModel("VK_OemMinus", 0x00DF, 0x1E9E, "Eszett", "capital Eszett", true);
                }
                return vk_OemMinus;
            }
        }
        #endregion

        #region 18: KeyVK_Y
        /// <summary>
        /// 18  Swap Z and Y
        /// </summary>
        public override KeyModel KeyVK_Y
        {
            get
            {
                if (vk_Y == null)
                {
                    vk_Y = new KeyModel("VK_Y", 0x007A, 0x005A);
                }
                return vk_Y;
            }
        }
        #endregion

        #region 23: VK_OEM_4
        /// <summary>
        /// 23  U with umlaut (also called diaeresis)
        /// </summary>
        public override KeyModel KeyVK_OemOpenBrackets
        {
            get
            {
                if (vk_OemOpenBrackets == null)
                {
                    vk_OemOpenBrackets = new KeyModel("VK_OemOpenBrackets", 0x00FC, 0x00DC, "letter u with umlaut", "Capital U with umlaut", true);
                }
                return vk_OemOpenBrackets;
            }
        }
        #endregion

        #region 35: VK_OEM_1
        /// <summary>
        /// 35  Letter O with umlaut
        /// </summary>
        public override KeyModel KeyVK_Oem1
        {
            get
            {
                if (vk_Oem1 == null)
                {
                    vk_Oem1 = new KeyModel("VK_Oem1", 0x00F6, 0x00D6, "lowercase letter O with umlaut", "capital letter O with umlaut", true);
                }
                return vk_Oem1;
            }
        }
        #endregion

        #region 36: VK_OEM_7
        /// <summary>
        /// 36  Letter A with umlaut
        /// </summary>
        public override KeyModel KeyVK_Oem7
        {
            get
            {
                if (vk_Oem7 == null)
                {
                    vk_Oem7 = new KeyModel("VK_Oem7", 0x00E4, 0x00C4, "letter a with umlaut", "Capital A with umlaut", true);
                }
                return vk_Oem7;
            }
        }
        #endregion

        #region 37: KeyVK_Z
        /// <summary>
        /// 37  Swap Z and Y.
        /// </summary>
        public override KeyModel KeyVK_Z
        {
            get
            {
                if (vk_Z == null)
                {
                    vk_Z = new KeyModel("VK_Z", 0x0079, 0x0059);
                }
                return vk_Z;
            }
        }
        #endregion

    }
}
