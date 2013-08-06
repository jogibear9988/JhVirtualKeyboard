// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhVirtualKeyboard</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************


namespace JhVirtualKeyboard
{
    /// <summary>
    /// The Control that you want the virtual keyboard to inject characters into,
    /// needs to implement this interface if it's not a TextBox.
    /// </summary>
    interface IInjectableControl
    {
        /// <summary>
        /// Implement this method to insert text at the caret-positioin of your text-control,
        /// just as if you had typed the character at the keyboard.
        /// </summary>
        /// <param name="sTextToInsert">The text to insert into your text-control</param>
        void InsertText(string sTextToInsert);

        /// <summary>
        /// Implement this method to emulate what you'd expect to happen when the user
        /// presses the backspace key.
        /// </summary>
        void Backspace();
    }
}
