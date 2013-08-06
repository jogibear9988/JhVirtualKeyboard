// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;


namespace JhLib
{
    /// <summary>
    /// This indicates WHERE to position a Window, relative to another Window (presumably it's parent or owner Window).
    /// The default value is ToRightOfParent.
    /// </summary>
    public enum AlignmentType { ToRightOfParent, ToLeftOfParent, UnderParent, AboveParent }

    #region class DesignerProperties
    /// <summary>
    /// Provides a custom implementation of DesignerProperties.GetIsInDesignMode
    /// to work around an issue.
    /// </summary>
    public static class DesignerProperties
    {
        // The method here is inspired by David Ansons blog article at http://blogs.msdn.com/b/delay/archive/2009/02/26/designerproperties-getisindesignmode-forrealz-how-to-reliably-detect-silverlight-design-mode-in-blend-and-visual-studio.aspx
        // dated 2009/2/26.
        // The issue was that DesignerProperties.GetIsInDesignMode doesn't always return the correct value under Visual Studio.

        /// <summary>
        /// Returns whether the control is in design mode (running under Blend
        /// or Visual Studio).
        /// </summary>
        /// <param name="element">The element from which the property value is
        /// read.</param>
        /// <returns>True if in design mode.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "element", Justification =
            "Matching declaration of System.ComponentModel.DesignerProperties.GetIsInDesignMode (which has a bug and is not reliable).")]
        public static bool GetIsInDesignMode(DependencyObject dependencyObject = null)
        {
#if SILVERLIGHT
            return BaseViewModel.IsInDesignModeStatic;
#else
            if (!_isInDesignMode.HasValue)
            {
                if (dependencyObject == null)
                {
                    // We can assume we're in 'design-mode', if Application.Current returns null,
                    // or if the type of the current application is simply Application, as opposed to our subclass of Application.
                    if (Application.Current == null || Application.Current.GetType() == typeof(Application))
                    {
                        //if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                        //{
                        //    Console.WriteLine("Yeah, but System.ComponentModel.DesignerProperties thinks we are in design-mode.");
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Yeah, but System.ComponentModel.DesignerProperties thinks we are not in design-mode.");
                        //}
                        _isInDesignMode = System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject());
                        //_isInDesignMode = true;
                    }
                    else
                    {
                        // At this point, Application.Current is not null, so let's check it's root visual element.
                        Window theMainWindow = Application.Current.MainWindow;
                        if (theMainWindow == null)
                        {
                            _isInDesignMode = true;
                        }
                        else
                        {
                            _isInDesignMode = System.ComponentModel.DesignerProperties.GetIsInDesignMode(theMainWindow);
                        }
                    }
                }
                else
                {
                    _isInDesignMode = System.ComponentModel.DesignerProperties.GetIsInDesignMode(dependencyObject);
                }
            }
            return _isInDesignMode.Value;
#endif
        }

        /// <summary>
        /// Get whether we are in design mode (ie, running under Blend or Visual Studio designer).
        /// Does the same thing as GetIsInDesignMode(null) -- just a shortcut method.
        /// </summary>
        public static bool IsInDesignMode
        {
            get { return GetIsInDesignMode(); }
        }

        /// <summary>
        /// Stores the computed InDesignMode value. A nullable is used such that the null state indicates that the value has not been set yet.
        /// </summary>
#if !SILVERLIGHT
        private static bool? _isInDesignMode;
#endif
    }
    #endregion class DesignerProperties

    /// <summary>
    /// This class is simply for hanging some WPF-specific extension methods upon.
    /// </summary>
    public static class WPFExtensions
    {
        #region IsInDesignMode
        /// <summary>
        /// Return a flag indicating whether we are currently running within the Visual Studio or Blend designer.
        /// </summary>
        /// <param name="anyDependencyObject">The Window or other DependencyObject to make this test on</param>
        /// <returns>true if we're now running within a designer, false if executing normally</returns>
        public static bool IsInDesignMode(this DependencyObject anyDependencyObject)
        {
            // Thanks to Alan Le for this tip.
#if Silverlight
            return !HtmlPage.IsEnabled;
#else
            return System.ComponentModel.DesignerProperties.GetIsInDesignMode(anyDependencyObject);
#endif
        }
        #endregion

        #region AlignToParent
        /// <summary>
        /// Causes the given Window to align itself alongside the parent (ie, Owner) window.
        /// You should put the call to this within the LayoutUpdated or Loaded event handler of the Window you want to align itself
        /// to it's parent. The Owner property has to have been set, otherwise there's nothing for it to align against.
        /// </summary>
        /// <param name="dialog">The given Window that is to be aligned</param>
        /// <param name="inWhichDirection">Specifieds a preference toward which side or edge to align it to, if there's room on the display for that.</param>
        public static void AlignToParent(this Window dialog, AlignmentType inWhichDirection)
        {
            //TODO ?
#if !SILVERLIGHT
            Window parent = dialog.Owner as Window;
            if (parent != null)
            {
                if (inWhichDirection == AlignmentType.ToRightOfParent)
                {
                    // Try the right side, then the left.
                    if (AlignToRight(dialog))
                    {
                        return;
                    }
                    else if (AlignToLeft(dialog))
                    {
                        return;
                    }
                }
                else if (inWhichDirection == AlignmentType.ToLeftOfParent)
                {
                    // Try the left side, then the right.
                    if (AlignToLeft(dialog))
                    {
                        return;
                    }
                    else if (AlignToRight(dialog))
                    {
                        return;
                    }
                }
                else if (inWhichDirection == AlignmentType.AboveParent)
                {
                    double parentTop = parent.Top;
                    double myHeight = dialog.Height;
                    double separation = 2;
                    if (parentTop >= myHeight)
                    {
                        if (parentTop > (myHeight + separation))
                        {
                            dialog.Top = parentTop - separation - myHeight;
                        }
                        else
                        {
                            dialog.Top = parentTop - myHeight;
                        }
                        dialog.Left = parent.Left;
                        return;
                    }
                }
                // failing that, I'll try underneath
                AlignToBottom(dialog);
                // otherwise.. at this point I think it's time to throw in the towel and forget about it.
            }
#endif
        }

        #region internal helper methods for AlignToParent

        /// <summary>
        /// This is a helper method for AlignToParent; it aligns the given Window to the right of the parent-Window, if possible.
        /// </summary>
        /// <param name="dialog">the Window to align</param>
        /// <returns>true if successful, false if there wasn't sufficient space on the display</returns>
        private static bool AlignToRight(Window dialog)
        {
            //TODO  ?
#if SILVERLIGHT
            return true;
#else
            Window parent = dialog.Owner as Window;
            double ParentRight = parent.Left + parent.Width;
            double Separation = 2;
            double ScreenRight = SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth;
            double VirtualScreenBottom = SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight;
            bool isMultipleScreensHorz = (SystemParameters.VirtualScreenWidth > SystemParameters.PrimaryScreenWidth);

            // This following, which brings in the need for Windows.Forms,
            // is for the case where a 2nd monitor is not as wide as the pri monitor.
            if (!isMultipleScreensHorz)
            {
                System.Windows.Forms.Screen[] allScreens = System.Windows.Forms.Screen.AllScreens;
                if (allScreens.Count() > 1)
                {
                    System.Windows.Forms.Screen s2 = allScreens[1];
                    double width2 = s2.Bounds.Width;
                    double top2 = s2.Bounds.Top;
                    double bottom2 = s2.Bounds.Bottom;

                    if (parent.Top.IsInRange(top2, bottom2))
                    {
                        // Evidently, this Window is on Screen 2, so use it's area.
                        ScreenRight = width2;
                    }
                }
            }

            // I'll position myself along the right side if there's sufficient space.
            if (ParentRight + dialog.Width < ScreenRight)
            {
                // Try to avoid falling over top of the break between two screens.
                if (isMultipleScreensHorz)
                {
                    // See if the parent window is entirely within the leftmost screen.
                    if (ParentRight < SystemParameters.PrimaryScreenWidth)
                    {
                        if (ParentRight + dialog.Width > SystemParameters.PrimaryScreenWidth)
                        {
                            dialog.Left = SystemParameters.PrimaryScreenWidth;
                            dialog.Top = Math.Min(parent.Top, VirtualScreenBottom - dialog.Height);
                            return true;
                        }
                    }
                }
                dialog.Left = ParentRight + Separation;
                // Adjust my vertical position so that my feet don't jut off the bottom edge.
                dialog.Top = Math.Min(parent.Top, VirtualScreenBottom - dialog.Height);
                return true;
            }
            else
            {
                return false;
            }
#endif
        }

        /// <summary>
        /// Just a helper method for AlignToParent
        /// </summary>
        /// <param name="dialog">the Window to align</param>
        /// <returns>true if successful, false if there wasn't sufficient space on the display</returns>
        private static bool AlignToLeft(Window dialog)
        {
            //TODO  ?
#if SILVERLIGHT
            return true;
#else
            Window parent = dialog.Owner as Window;
            double Separation = 2;
            double VirtualScreenBottom = SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight;
            double ScreenLeft = SystemParameters.VirtualScreenLeft;
            bool isMultipleScreensHorz = (SystemParameters.VirtualScreenWidth > SystemParameters.PrimaryScreenWidth);

            if (isMultipleScreensHorz)
            {
                System.Windows.Forms.Screen[] allScreens = System.Windows.Forms.Screen.AllScreens;
                if (allScreens.Count() > 1)
                {
                    System.Windows.Forms.Screen s2 = allScreens[1];
                    double left2 = s2.Bounds.Left;
                    double right2 = s2.Bounds.Right;

                    if (parent.Left.IsInRange(left2, right2))
                    {
                        // Evidently, this Window is on Screen 2, so use it's area.
                        double xToLeftOfParent = parent.Left - left2;
                        if (xToLeftOfParent > dialog.Width)
                        {
                            dialog.Left = parent.Left - dialog.Width - Separation;
                            dialog.Top = Math.Min(parent.Top, VirtualScreenBottom - dialog.Height);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            // Try the left side.
            //TODO: Check this for various positions of secondary display.

            if ((parent.Left - ScreenLeft) > dialog.Width)
            {
                dialog.Left = parent.Left - dialog.Width - Separation;
                dialog.Top = Math.Min(parent.Top, VirtualScreenBottom - dialog.Height);
                return true;
            }
            else
            {
                return false;
            }
#endif
        }

        private static bool AlignToBottom(Window dialog)
        {
#if SILVERLIGHT
            return true;
#else
            bool ok = true;
            Window parent = dialog.Owner as Window;
            // CBL  Is VirtualScreenTop always zero? !!!
            double VirtualScreenBottom = SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight;
            double ParentBottom = parent.Top + parent.Height;
            double Separation = 2;

            if (ParentBottom < (VirtualScreenBottom - dialog.Height))
            {
                dialog.Top = ParentBottom + Separation;
                dialog.Left = Math.Max(parent.Left, SystemParameters.VirtualScreenLeft);
            }
            // failing that, I'll try over-top
            else if (parent.Top > dialog.Height)
            {
                dialog.Top = parent.Top - dialog.Height - Separation;
                dialog.Left = Math.Max(parent.Top, SystemParameters.VirtualScreenLeft);
            }
            else
            {
                ok = false;
            }
            return ok;
#endif
        }
        #endregion

        #endregion

        #region TextBox extension method InsertText
        /// <summary>
        /// Insert the given text into this TextBox at the current CaretIndex, and replacing any already-selected text.
        /// </summary>
        /// <param name="textbox">The TextBox to insert the new text into</param>
        /// <param name="sTextToInsert">The text to insert into this TextBox</param>
        public static void InsertText(this System.Windows.Controls.TextBox textbox, string sTextToInsert)
        {
#if !SILVERLIGHT
            int iCaretIndex = textbox.CaretIndex;
            int iOriginalSelectionLength = textbox.SelectionLength;
            textbox.SelectedText = sTextToInsert;
            if (iOriginalSelectionLength > 0)
            {
                textbox.SelectionLength = 0;
            }
            textbox.CaretIndex = iCaretIndex + 1;
#else
            //TODO ?

            // Newer method, if needed (TODO: Test to see if this is needed.

            //int iOriginalTextLength = sOriginalContent.Length;
            //if (iOriginalSelectionLength == 0)
            //{
            //    if (iCaretIndex == iOriginalTextLength)
            //    {
            //        textbox.Text = sOriginalContent + sTextToInsert;
            //    }
            //    else if (iCaretIndex == 0)
            //    {
            //        textbox.Text = sTextToInsert + sOriginalContent;
            //    }
            //    else
            //    {
            //        string sPartBefore = sOriginalContent.Substring(0, iCaretIndex);
            //        string sRemainder = sOriginalContent.Substring(iCaretIndex);
            //        textbox.Text = sPartBefore + sTextToInsert + sRemainder;
            //    }
            //}
            //else  //TODO: Perhaps only this last part is sufficient?
            //{
            //    textbox.SelectedText = sTextToInsert;
            //    if (iOriginalSelectionLength > 0)
            //    {
            //        textbox.SelectionLength = 0;
            //    }
            //}
            //textbox.CaretIndex = iCaretIndex + sTextToInsert.Length;
#endif
        }
        #endregion

        #region TextBox extension method InsertTextLeftToRight
        /// <summary>
        /// Insert the given text into this TextBox at the current CaretIndex, and replacing any already-selected text.
        /// </summary>
        /// <param name="textbox">The TextBox to insert the new text into</param>
        /// <param name="sTextToInsert">The text to insert into this TextBox</param>
        public static void InsertTextLeftToRight(this System.Windows.Controls.TextBox textbox, string sTextToInsert)
        {
            // This replaces the algorithm that the preceding method uses, just to ensure the concatenation is LeftToRight.
            //TODO: This needs to be tested.
            //TODO  ?
#if !SILVERLIGHT
            int iCaretIndex = textbox.CaretIndex;
            int iOriginalSelectionLength = textbox.SelectionLength;
            string sOriginalContent = textbox.Text;
            int iOriginalTextLength = sOriginalContent.Length;
            if (iOriginalSelectionLength == 0)
            {
                if (iCaretIndex == iOriginalTextLength)
                {
                    textbox.Text = sOriginalContent.ConcatLeftToRight(sTextToInsert);
                }
                else if (iCaretIndex == 0)
                {
                    textbox.Text = sTextToInsert.ConcatLeftToRight(sOriginalContent);
                }
                else
                {
                    string sPartBefore = sOriginalContent.Substring(0, iCaretIndex);
                    string sRemainder = sOriginalContent.Substring(iCaretIndex);
                    // This next line does this, but using the ConcatLeftToRight method.
                    // textbox.Text = sPartBefore + sTextToInsert + sRemainder;
                    textbox.Text = sPartBefore.ConcatLeftToRight(sTextToInsert).ConcatLeftToRight(sRemainder);
                }
            }
            else  //TODO: Perhaps only this last part is sufficient?
            {
                textbox.SelectedText = sTextToInsert;
                if (iOriginalSelectionLength > 0)
                {
                    textbox.SelectionLength = 0;
                }
            }
            textbox.CaretIndex = iCaretIndex + sTextToInsert.Length;
#endif
        }
        #endregion

        #region RichTextBox extension method InsertText
        /// <summary>
        /// Insert the given text into this RichTextBox at the current CaretPosition, replacing any already-selected text.
        /// </summary>
        /// <param name="richTextBox">The RichTextBox to insert the new text into</param>
        /// <param name="sTextToInsert">The text to insert into this RichTextBox</param>
        public static void InsertText(this System.Windows.Controls.RichTextBox richTextBox, string sTextToInsert)
        {
            if (!String.IsNullOrEmpty(sTextToInsert))
            {
                //TODO
#if !SILVERLIGHT
                richTextBox.BeginChange();
                if (richTextBox.Selection.Text != string.Empty)
                {
                    richTextBox.Selection.Text = string.Empty;
                }
                TextPointer tp = richTextBox.CaretPosition.GetPositionAtOffset(0, LogicalDirection.Forward);
                richTextBox.CaretPosition.InsertTextInRun(sTextToInsert);
                richTextBox.CaretPosition = tp;
                richTextBox.EndChange();
                Keyboard.Focus(richTextBox);
#endif
            }
        }
        #endregion

        #region TextBox extension method TextTrimmed
        /// <summary>
        /// This is just a convenience extension-method to simplify the getting of strings
        /// from a WPF TextBox.
        /// It was a pain in da butt, having to remember to test for nulls, whitespace, etc.
        /// Now, all you have to do is check the .Length
        /// </summary>
        /// <param name="textbox">The WPF TextBox to get the Text from</param>
        /// <returns>If the TextBox was empty, then "" (empty string) otherwise the Text with leading and trailing whitespace trimmed</returns>
        public static string TextTrimmed(this System.Windows.Controls.TextBox textbox)
        {
            string sText = textbox.Text;
            if (String.IsNullOrEmpty(sText))
            {
                return "";
            }
            else
            {
                return sText.Trim();
            }
        }
        #endregion

        #region Window extension method MoveAsAGroup

        public static void MoveAsAGroup(this Window me,
                                        double desiredXDisplacement, double desiredYDisplacement,
                                        ref bool isIgnoringLocationChangedEvent)
        {
            //TODO ?
#if !SILVERLIGHT
            // Ensure we don't recurse when we reposition.
            isIgnoringLocationChangedEvent = true;

            Window windowToMoveWith = me.Owner;

            // Try to prevent me from sliding off the screen horizontally.
            double bitToShow = 32;
            double leftLimit = SystemParameters.VirtualScreenLeft - me.Width + bitToShow;
            double rightLimit = SystemParameters.VirtualScreenWidth - bitToShow;
            bool notTooMuchXDisplacement = Math.Abs(me.Left - windowToMoveWith.Left) < Math.Abs(desiredXDisplacement);
            if (me.Left >= rightLimit && notTooMuchXDisplacement)
            {
                // bumping against the right.
                me.Left = rightLimit;
            }
            else if (me.Left <= leftLimit && notTooMuchXDisplacement)
            {
                // bumping against the left.
                me.Left = leftLimit;
            }
            else // it's cool - just slide along with the other window.
            {
                me.Left = windowToMoveWith.Left + desiredXDisplacement;
            }

            // Try to prevent me from sliding off the screen vertically.
            double topLimit = SystemParameters.VirtualScreenTop - me.Height + bitToShow;
            double bottomLimit = SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight - bitToShow;
            bool notTooMuchYDisplacement = Math.Abs(me.Top - windowToMoveWith.Top) < Math.Abs(desiredYDisplacement);
            if (me.Top <= topLimit && notTooMuchYDisplacement)
            {
                // bumping up against the top.
                //Console.WriteLine("setting to topLimit of " + topLimit);
                me.Top = topLimit;
            }
            else if (me.Top >= bottomLimit && notTooMuchYDisplacement)
            {
                // bumping against the bottom.
                me.Top = bottomLimit;
            }
            else // it's cool - just slide along with the other window.
            {
                me.Top = windowToMoveWith.Top + desiredYDisplacement;
            }

            // Reset the handler for the LocationChanged event.
            isIgnoringLocationChangedEvent = false;
#endif
        }
        #endregion

        #region InvokeIfRequired
#if !SILVERLIGHT
        public static void InvokeIfRequired(this DispatcherObject control, Action methodCall)
        {
            //Provided for VS2008
            InvokeIfRequired(control, methodCall, DispatcherPriority.Background);
        }

        public static void InvokeIfRequired(this DispatcherObject control, Action methodCall, DispatcherPriority priorityForCall)
        {
            // This comes directly from Sacha Barber's article at http://www.codeproject.com/Articles/37314/Useful-WPF-Threading-Extension-Method.aspx

            // See whether we need to invoke the call from the Dispatcher thread.
            if (control.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                control.Dispatcher.Invoke(priorityForCall, methodCall);
            }
            else
            {
                methodCall();
            }
        }
#endif
        #endregion

        #region SetText_WithThreadSafety
        /// <summary>
        /// Assign the given string to the Text property of the WPF TextBox, but in a thread-safe manner.
        /// This may be called from a thread or Task other than that of the GUI.
        /// </summary>
        /// <param name="textBox">The TextBox to put the text into</param>
        /// <param name="textToPutIntoTheTextBox">The text to put into the TextBox</param>
        public static void SetText_WithThreadSafety(this TextBox textBox, string textToPutIntoTheTextBox)
        {
            //TODO  How to do in SL?
#if SILVERLIGHT
            textBox.Text = textToPutIntoTheTextBox;
#else
            // Fror the following, thanks to Simon Knox.  See article http://www.codeproject.com/KB/WPF/ThreadSafeWPF.aspx
            InvokeIfRequired(textBox, () => { textBox.Text = textToPutIntoTheTextBox; }, DispatcherPriority.Background);
#endif
        }

        /// <summary>
        /// Assign the given string to the Text property of the WPF TextBlock, but in a thread-safe manner.
        /// This may be called from a thread or Task other than that of the GUI.
        /// </summary>
        /// <param name="textBlock">The TextBlock to put the text into</param>
        /// <param name="textToPutIntoTheTextBlock">The text to put into the TextBlock</param>
        public static void SetText_WithThreadSafety(this TextBlock textBlock, string textToPutIntoTheTextBlock)
        {
            //TODO  How to do in SL?
#if SILVERLIGHT
            textBlock.Text = textToPutIntoTheTextBlock;
#else
            InvokeIfRequired(textBlock, () => { textBlock.Text = textToPutIntoTheTextBlock; }, DispatcherPriority.Background);
#endif
        }
        #endregion

        #region GetText_WithThreadSafety
        /// <summary>
        /// Return the the contents of the Text property of the WPF TextBox, but in a thread-safe manner.
        /// This may be called from a thread or Task other than that of the GUI.
        /// In addition, the string that is returned is String.Empty if the text had only whitespace.
        /// </summary>
        /// <param name="textBox">The TextBox to get the text from</param>
        public static string GetText_WithThreadSafety(this TextBox textBox)
        {
            string text = String.Empty;
            //TODO  How to do in SL?
#if SILVERLIGHT
            text = textBox.Text;
#else
            InvokeIfRequired(textBox, () => { text = textBox.Text; }, DispatcherPriority.Background);
#endif
            if (String.IsNullOrWhiteSpace(text))
            {
                return String.Empty;
            }
            else
            {
                return text;
            }
        }
        #endregion

        #region Sort(ListView,..)
#if !SILVERLIGHT
        public static void Sort(this System.Windows.Controls.ListView listview, string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(listview.ItemsSource);
            if (dataView != null)
            {
                dataView.SortDescriptions.Clear();
                SortDescription sd = new SortDescription(sortBy, direction);
                dataView.SortDescriptions.Add(sd);
                dataView.Refresh();
            }
            else
            {
                Console.WriteLine("Why the heck is GetDefaultView returning null? !!!");
            }
        }
#endif
        #endregion

        #region Reactivate

        #region class WindowEventArgs
        /// <summary>
        /// This subclass of EventArgs is intended for alerting the rest of the application of changes to the options, in realtime.
        /// </summary>
        public class WindowEventArgs : EventArgs
        {
            public WindowEventArgs(Window value)
            {
                this.Window = value;
            }

            public Window Window { get; set; }
        }
        #endregion class WindowEventArgs

        /// <summary>
        /// Make this Window the active one, by calling Activate and Focus
        /// but after a half-second delay in order to allow the user's clicking to not interfere.
        /// </summary>
        public static void Reactivate(this Window window)
        {
            //TODO: I ought to be able to pass the window within the args to the timer-handler!
            //var windowEventArgs = new WindowEventArgs(window);
            //EventArgs eventArgs = new EventArgs();
            _windowToBeReactivated = window;

            _reactivationTimer = new DispatcherTimer(TimeSpan.FromSeconds(0.5),
                                                     DispatcherPriority.Input,
                                                     new EventHandler(OnReactivationTimer2),
                //new EventHandler<WindowEventArgs>(OnReactivationTimer, windowEventArgs),
                                                     window.Dispatcher);
        }

        private static DispatcherTimer _reactivationTimer;
        private static Window _windowToBeReactivated;

        private static void OnReactivationTimer2(object sender, EventArgs args)
        {
            if (_reactivationTimer != null)
            {
                _reactivationTimer.Stop();
                _reactivationTimer.Tick -= OnReactivationTimer2;
                _reactivationTimer = null;
            }
            if (_windowToBeReactivated != null)
            {
                // Both are needed.
                _windowToBeReactivated.Activate();
                _windowToBeReactivated.Focus();
                // Clear this reference so that it doesn't prevent the Window from being garbage-collected.
                _windowToBeReactivated = null;
            }
        }

        private static void OnReactivationTimer(object sender, WindowEventArgs args)
        {
            _reactivationTimer.Stop();
            //            _reactivationTimer.Tick -= OnReactivationTimer;
            _reactivationTimer = null;
            // Both are needed.
            //            Activate();
            //            Focus();
        }
        #endregion Reactivate
    }
}
