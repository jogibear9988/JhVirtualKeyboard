// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhVirtualKeyboard</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using JhVirtualKeyboard.ViewModels;


namespace JhVirtualKeyboard.KeyAssignmentSets
{
    /// <summary>
    /// The SpanishKeyAssignments subclass of KeyAssignmentSet overrides those KeyAssignments that need to differ
    /// from those of the English keyboard.
    /// </summary>
    public class SpanishKeyAssignmentSet : KeyAssignmentSet
    {
        public SpanishKeyAssignmentSet()
            : base()
        {
        }

        #region KeyboardLayout
        /// <summary>
        /// Get the WhichKeyboardLayout this KeyAssignmentSet is used for.
        /// </summary>
        public override WhichKeyboardLayout KeyboardLayout
        {
            get { return WhichKeyboardLayout.Spanish; }
        }
        #endregion

        #region The individual key assignments

        #region 11: KeyVK_OemMinus
        /// <summary>
        /// 11  Apostrophe/Question mark
        /// </summary>
        public override KeyModel KeyVK_OemMinus
        {
            get
            {
                if (vk_OemMinus == null)
                {
                    vk_OemMinus = new KeyModel("VK_OemMinus", 0x0027, 0x003F, "Apostrophe", "Question mark", false);
                }
                return vk_OemMinus;
            }
        }
        #endregion

        #region 12: KeyVK_OemPlus
        /// <summary>
        /// 12
        /// </summary>
        public override KeyModel KeyVK_OemPlus
        {
            get
            {
                if (vk_OemPlus == null)
                {
                    vk_OemPlus = new KeyModelWithTwoGlyphs("VK_OemPlus", 0x00A1, 0x00BF, "A1 InvertedExclamation mark", "BF Inverted Question mark", false);
                }
                return vk_OemPlus;
            }
        }
        #endregion

        #region 35: KeyVK_Oem1
        /// <summary>
        /// 35  N Tilde
        /// </summary>
        public override KeyModel KeyVK_Oem1
        {
            get
            {
                if (vk_Oem1 == null)
                {
                    vk_Oem1 = new KeyModel("VK_Oem1", 0x00F1, 0x00D1, "n tilde", "N Tilde", true);
                }
                return vk_Oem1;
            }
        }
        #endregion

        #region 36: VK_OEM_7
        /// <summary>
        /// 36  c-cedilla, used in the Albanian, Azerbaijani, Kurdish (Kurmanji dialect), Ligurian, Tatar, Turkish, Turkmen, and Zazaki alphabets.
        /// This letter also appears in Catalan, French, Friulian, Occitan, and Portuguese as a variant of the letter c.
        /// In the IPA, /(c-cedilla)/ represents the voiceless palatal fricative.
        /// </summary>
        public override KeyModel KeyVK_Oem7
        {
            get
            {
                if (vk_Oem7 == null)
                {
                    vk_Oem7 = new KeyModel("VK_Oem7", 0x00E7, 0x00C7, "minuscule c-cedilla", "Majuscule c-cedilla", true);
                }
                return vk_Oem7;
            }
        }
        #endregion

        #region 44: KeyVK_OemComma
        /// <summary>
        /// 44  Commas/Semicolon
        /// </summary>
        public override KeyModel KeyVK_OemComma
        {
            get
            {
                if (vk_OemComma == null)
                {
                    vk_OemComma = new KeyModel("VK_OemComma", 0x002C, 0x003B, "Comma", "Semicolon", false);
                }
                return vk_OemComma;
            }
        }
        #endregion

        #region 45: KeyVK_OemPeriod
        /// <summary>
        /// 45  Period/Semicolon
        /// </summary>
        public override KeyModel KeyVK_OemPeriod
        {
            get
            {
                if (vk_OemPeriod == null)
                {
                    vk_OemPeriod = new KeyModelWithTwoGlyphs("VK_OemPeriod", 0x002E, 0x003B, "Period", "Semicolon", false);
                }
                return vk_OemPeriod;
            }
        }
        #endregion

        #region 46: VK_OEM_2
        /// <summary>
        /// 46  Hyphen/Underline
        /// </summary>
        public override KeyModel KeyVK_OemQuestion
        {
            get
            {
                if (vk_OemQuestion == null)
                {
                    vk_OemQuestion = new KeyModelWithTwoGlyphs("VK_OemQuestion", 0x002D, 0x005F, "Hyphen", "Underline", false);
                }
                return vk_OemQuestion;
            }
        }
        #endregion

        #endregion The individual key assignments
    }
}
