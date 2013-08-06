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
    /// This is the interface that your application Window needs to implement in order to make use of the Virtual Keyboard.
    /// </summary>
    public interface IVirtualKeyboardInjectable
    {
        /// <summary>
        /// This would be the TextBox or similar control that you want to use the Virtual Keyboard to type characters into.
        /// It is how you inform the Virtual Keyboard where to put the characters you type with it.
        /// </summary>
        System.Windows.Controls.Control ControlToInjectInto { get; }

        /// <summary>
        /// Get or set the application-window's reference to the virtual-keyboard.
        /// </summary>
        VirtualKeyboard TheVirtualKeyboard { get; set; }
    }
}
