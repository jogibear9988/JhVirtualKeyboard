// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.ComponentModel;
using System.Windows;


// Notes:
//   For simplicity I chose the name "JhMessageBox", to closely resemble the ubiquitous "MessageBox"
//   upon which many of us relied in the Windows Forms days,
//   and it does reflect our purpose better than "TaskDialog" which could connote any dialog for accomplishing a task
//   (perhaps a bit too generic).
//   msdn webpage describing task dialog design considerations:
//   http://msdn.microsoft.com/en-us/library/Aa511268#commitButtons


namespace JhLib
{
    #region types used with JhMessageBox
    /// <summary>
    /// This is the "result" that indicates what the user's choice was
    /// in response to a dialog-window.
    /// </summary>
    /// <remarks>
    /// These follow the TaskDialogResult values, except for the TimedOut value.
    /// </remarks>
    public enum JhDialogResult
    {
        Ok = 1,
        Cancel = 2,
        Retry = 4,
        Yes = 6,
        No = 7,
        Close = 8,
        Abort,
        Ignore,
        TimedOut
    }

    /// <summary>
    /// This is a more abstract alternative to using the JhMessageBoxIcon to set the default graphics and audio for the message-box.
    /// </summary>
    public enum JhMessageBoxType
    {
        None,
        Information,
        Question,
        Warning,
        UserMistake,
        Error,
        SecurityIssue,
        SecuritySuccess,
        Stop
    }

    /// <summary>
    /// This bit-field enumeration-type specifies the combination of buttons to be shown.
    /// </summary>
    [Flags]
    public enum JhMessageBoxButtons
    {
        None = 0,
        Ok = 0x0001,
        Yes = 0x0002,
        No = 0x0004,
        Cancel = 0x0008,
        Retry = 0x0010,
        Close = 0x0020,
        Ignore = 0x0040
    }

    /// <summary>
    /// This selects one of the predefined background bitmap images.
    /// </summary>
    public enum JhMessageBoxBackgroundTexture
    {
        None,
        BlueTexture,
        BlueMarble,
        BrownMarble1,
        BrownMarble2,
        BrownTexture1,
        BrownTexture2,
        BrushedMetal,
        GrayTexture1,
        GrayTexture2,
        GrayMarble,
        GreenTexture1,
        GreenTexture2,
        GreenTexture3,
        GreenGradiant   // This one uses XAML gradiants instead of a bitmap
    }

    #region class MessageBoxCompletedArgs
    /// <summary>
    /// This class provides the argument for the Completed event's handler.
    /// </summary>
    public class MessageBoxCompletedArgs : AsyncCompletedEventArgs
    {
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="result">the JhDialogResult that the message-box was ended with</param>
        /// <param name="error">an exception if it was thrown during the operation of the message-box, otherwise null</param>
        /// <param name="messageBoxWindow">the message-box that raised this event</param>
        public MessageBoxCompletedArgs(JhDialogResult result, Exception error, JhMessageBoxWindow messageBoxWindow)
            : base(error: error, cancelled: false, userState: messageBoxWindow)
        {
            this.Result = result;
            this.MessageBoxWindow = messageBoxWindow;
            this.Options = messageBoxWindow._options;
        }

        /// <summary>
        /// Get the JhMessageBoxResultOptions that the message-box was invoked with.
        /// </summary>
        public JhMessageBoxOptions Options { get; private set; }

        /// <summary>
        /// Get the JhDialogResult that the message-box was ended with.
        /// </summary>
        public JhDialogResult Result { get; private set; }

        /// <summary>
        /// Get the JhMessageBoxWindow instance that the message-box was displayed with.
        /// </summary>
        public JhMessageBoxWindow MessageBoxWindow { get; private set; }
    }
    #endregion

    #endregion types used with JhMessageBox


    #region class JhMessageBox
    /// <summary>
    /// This is the JhMessageBox-manager class, the single-point-of-access for our message-box facility
    /// which displays notifications to the user via the JhMessageBoxWindow as opposed to the older MessageBox.
    /// </summary>
    public class JhMessageBox : IInterlocution, IDisposable
    {
        #region Constructor and factory instance methods
        /// <summary>
        /// Get a new instance of a JhMessageBox (which is an IInterlocution),
        /// given the text to use as a prefix for all captions.
        /// </summary>
        /// <param name="vendorName">the name of the business-entity that owns this application, which is included within the caption for all message-boxes</param>
        /// <param name="productName">the name of this application, which is included within the caption for all message-boxes</param>
        /// <returns>a new instance of JhMessageBox for use in your application</returns>
        public static JhMessageBox GetNewInstance(string vendorName, string productName)
        {
            if (String.IsNullOrWhiteSpace(vendorName))
            {
                throw new ArgumentException("You must supply a value for vendorName - this is part of the mandatory title-bar prefix.");
            }
            if (String.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("productName.");
            }
            var newMessageBoxManager = new JhMessageBox();
            string titlebarPrefix = vendorName + " " + productName;
            newMessageBoxManager._captionPrefix = titlebarPrefix;
            return newMessageBoxManager;
        }

        public static JhMessageBox GetNewInstance(System.Windows.Application ownerApplication)
        {
            var newMessageBoxManager = new JhMessageBox();
            newMessageBoxManager._ownerApplication = ownerApplication;
            newMessageBoxManager._captionPrefix = newMessageBoxManager.GetAReasonableTitlebarPrefix();
            return newMessageBoxManager;
        }

        internal JhMessageBox()
        {
        }
        #endregion Constructor and factory instance methods

        #region AlertUser
        /// <summary>
        /// Static method to display a message-box to the user. Wait for his response or else close itself after the timeout has expired.
        /// This assumes only an Ok button, that it wants to be a top-most window and a timeout of five seconds.
        /// </summary>
        /// <param name="ofWhat">the text to show</param>
        /// <param name="messageType">which basic type of message this is (optional - defaults to Information)</param>
        /// <param name="captionAfterPrefix">what to show in the titlebar of this message-box, after the standard prefix (optional)</param>
        /// <param name="timeout">the maximum time to show it, in seconds (optional - the default is 5)</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        public static void AlertUser(string ofWhat,
                                     JhMessageBoxType messageType = JhMessageBoxType.Information,
                                     string captionAfterPrefix = null,
                                     int timeout = 5,  // Assume five seconds for the timeout value unless it is explicitly specified.
                                     FrameworkElement parent = null)
        {
            JhMessageBox.GetNewInstance(Application.Current).NotifyUser(summaryText: ofWhat,
                                                                        detailText: null,
                                                                        buttons: JhMessageBoxButtons.Ok,
                                                                        messageType: messageType,
                                                                        captionAfterPrefix: captionAfterPrefix,
                                                                        isTopmostWindow: true,
                                                                        timeout: timeout,
                                                                        parent: parent);
        }
        #endregion AlertUser

        #region the NotifyUser.. methods

        #region NotifyUser
        /// <summary>
        /// Display a message-box to the user, based upon the given options.
        /// This particular method is the one that all of the other Notify methods invoke to do the actual work.
        /// </summary>
        /// <param name="options">a JhMessageBoxOptions object that fully specifies everything about the message-box</param>
        /// <returns>a JhDialogResult indicating which action the user took, or TimedOut if the user took no action before the timeout expired</returns>
        public JhDialogResult NotifyUser(JhMessageBoxOptions options)
        {
            var messageBoxWindow = new JhMessageBoxWindow(this, options);

            // If we are doing testing, and have specified that a specific button is to be emulated as having been selected by the user,
            // then verify that that button is indeed present..
            if (IsTesting)
            {
                if (TestFacility.ButtonResultToSelect.HasValue)
                {
                    string complaint;
                    if (!GetWhetherButtonIsIncluded(options.ButtonFlags, out complaint))
                    {
                        // Announce the error  
                        throw new ArgumentOutOfRangeException(complaint);
                    }
                }
            }

            //TODO: The following should not be needed for Silverlight..
            // Try to give it a parent-window to position itself relative to.
#if !SILVERLIGHT
            FrameworkElement parentElement = options.ParentElement;
            Window parentWindow = null;
            if (parentElement == null)
            {
                // When running unit-tests, Application.Current might be null.
                if (Application.Current != null)
                {
                    parentWindow = Application.Current.MainWindow;
                    messageBoxWindow.Owner = parentWindow;
                }
            }
            else if (parentElement.IsLoaded)
            {
                parentWindow = parentElement as Window;
                if (parentWindow != null)
                {
                    messageBoxWindow.Owner = parentWindow;
                }
            }
#endif

            // Finally, show the message-box.
            //TODO: for Silverlight, need to prepare to receive the result asynchronously!!!
#if SILVERLIGHT
            messageBox.Show();
#else
            _messageBoxWindow = messageBoxWindow;
            if (options.IsAsynchronous)
            {
                messageBoxWindow.Show();
            }
            else
            {
                messageBoxWindow.ShowDialog();
            }
#endif
            return messageBoxWindow.Result;
        }

        /// <summary>
        /// Display a message-box to the user. Wait for his response or else close itself after the timeout has expired.
        /// The message-type is assumed to be JhMessageBoxType.Information
        /// </summary>
        /// <param name="ofWhat">The text message to display to the user</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        /// <returns>A JhDialogResult that indicates the user's response</returns>
        public JhDialogResult NotifyUser(string ofWhat, FrameworkElement parent = null)
        {
            // Use the default options for everything except that which is explicitly set for this instance.
            var options = new JhMessageBoxOptions(DefaultOptions);
            options.SummaryText = ofWhat;
            options.DetailText = null;
            options.CaptionAfterPrefix = null;
            options.ButtonFlags = JhMessageBoxButtons.Ok;
            options.MessageType = JhMessageBoxType.Information;
            options.IsToBeTopmostWindow = false;
#if !SILVERLIGHT
            options.ParentElement = parent;
#endif
            options.TimeoutPeriodInSeconds = 0;  // Assume zero for the timeout value, which invokes the default value.
            return NotifyUser(options);
        }

        /// <summary>
        /// Display a message-box to the user. Wait for his response or else close itself after the timeout has expired.
        /// The message-type is assumed to be JhMessageBoxType.Information
        /// </summary>
        /// <param name="summaryText">the summary text to show in the upper area</param>
        /// <param name="detailText">the detail text to show in the lower area</param>
        /// <param name="captionAfterPrefix">what to show in the titlebar of this message-box, after the standard prefix (optional)</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        /// <returns>a JhDialogResult that signals what the user clicked, or whether it timed-out</returns>
        public JhDialogResult NotifyUser(string summaryText, string detailText, string captionAfterPrefix = null, FrameworkElement parent = null)
        {
            // Use the default options for everything except that which is explicitly set for this instance.
            var options = new JhMessageBoxOptions(DefaultOptions);
            options.SummaryText = summaryText;
            options.DetailText = detailText;
            options.CaptionAfterPrefix = captionAfterPrefix;
            options.ButtonFlags = JhMessageBoxButtons.Ok;
            options.MessageType = JhMessageBoxType.Information;
            options.IsToBeTopmostWindow = false;
#if !SILVERLIGHT
            options.ParentElement = parent;
#endif
            options.TimeoutPeriodInSeconds = 0;  // Assume zero for the timeout value, which invokes the default value.
            return NotifyUser(options);
        }

        /// <summary>
        /// Display a message-box to the user. Wait for his response or else close itself after the timeout has expired.
        /// This is the overload that has all of the options, which the other methods call.
        /// </summary>
        /// <param name="summaryText">the summary text to show in the upper area</param>
        /// <param name="detailText">the detail text to show in the lower area</param>
        /// <param name="buttons">which buttons to show</param>
        /// <param name="messageType">which basic type of message this is (optional - defaults to Information)</param>
        /// <param name="captionAfterPrefix">what to show in the titlebar of this message-box, after the standard prefix (optional)</param>
        /// <param name="isTopmostWindow">whether to make the message-box the top-most window on the user's desktop (optional - defaults to false)</param>
        /// <param name="timeout">the maximum time to show it, in seconds (optional)</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        /// <returns>a JhDialogResult indicating which action the user took, or TimedOut if the user took no action before the timeout expired</returns>
        public JhDialogResult NotifyUser(string summaryText,
                                             string detailText,
                                             JhMessageBoxButtons buttons,
                                             JhMessageBoxType messageType = JhMessageBoxType.Information,
                                             string captionAfterPrefix = null,
                                             bool isTopmostWindow = false,
                                             int timeout = 0,  // Assume zero for the timeout value, which invokes the default value.
                                             FrameworkElement parent = null)
        {
            // Use the default options for everything except that which is explicitly set for this instance.
            var options = new JhMessageBoxOptions(DefaultOptions);
            options.SummaryText = summaryText;
            options.DetailText = detailText;
            options.CaptionAfterPrefix = captionAfterPrefix;
            options.ButtonFlags = buttons;
            options.MessageType = messageType;
            options.IsToBeTopmostWindow = isTopmostWindow;
#if !SILVERLIGHT
            options.ParentElement = parent;
#endif
            options.TimeoutPeriodInSeconds = timeout;
            return NotifyUser(options);
        }

        #region NotifyUserAsync
        /// <summary>
        /// Display a message-box to the user as a non-modal dialog window, and return immediately.
        /// The message-type is assumed to be JhMessageBoxType.Information
        /// </summary>
        /// <param name="summaryText">The text message to display to the user</param>
        /// <param name="parent">(optional) the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        public void NotifyUserAsync(string summaryText, FrameworkElement parent = null)
        {
            var options = new JhMessageBoxOptions(this.DefaultOptions)
                .SetMessageType(JhMessageBoxType.Information)
                .SetIsAsynchronous(true)
                .SetSummaryText(summaryText)
                .SetButtonFlags(JhMessageBoxButtons.Ok)
                .SetTimeoutPeriod(0)
                .SetParent(parent);
            NotifyUser(options);
        }

        /// <summary>
        /// Display a message-box to the user as a non-modal dialog window, and return immediately.
        /// The message-type is assumed to be JhMessageBoxType.Information
        /// </summary>
        /// <param name="summaryText">the summary text to show in the upper area</param>
        /// <param name="detailText">the detail text to show in the lower area</param>
        /// <param name="captionAfterPrefix">what to show in the titlebar of this message-box, after the standard prefix (optional)</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        public void NotifyUserAsync(string summaryText, string detailText, string captionAfterPrefix = null, FrameworkElement parent = null)
        {
            var options = new JhMessageBoxOptions(this.DefaultOptions)
                .SetButtonFlags(JhMessageBoxButtons.Ok)
                .SetCaptionAfterPrefix(captionAfterPrefix)
                .SetDetailText(detailText)
                .SetSummaryText(summaryText)
                .SetIsAsynchronous(true)
                .SetMessageType(JhMessageBoxType.Information)
                .SetParent(parent)
                .SetTimeoutPeriod(0);
            NotifyUser(options);
        }

        /// <summary>
        /// Display a message-box to the user. Wait for his response or else close itself after the timeout has expired.
        /// This is the overload that has all of the options, which the other methods call.
        /// </summary>
        /// <param name="summaryText">the summary text to show in the upper area</param>
        /// <param name="detailText">the detail text to show in the lower area</param>
        /// <param name="buttons">which buttons to show</param>
        /// <param name="messageType">the basic type of message-box to show (optional - default is Information)</param>
        /// <param name="captionAfterPrefix">what to show in the titlebar of this message-box, after the standard prefix (optional)</param>
        /// <param name="isTopmostWindow">whether to force this message-box to be over top of all other windows (optional - default is false)</param>
        /// <param name="timeoutInSeconds">the maximum time to show it, in seconds (optional)</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        /// <returns>a JhDialogResult indicating which action the user took, or TimedOut if the user took no action before the timeout expired</returns>
        public void NotifyUserAsync(string summaryText,
                                    string detailText,
                                    JhMessageBoxButtons buttons,
                                    JhMessageBoxType messageType = JhMessageBoxType.Information,
                                    string captionAfterPrefix = null,
                                    bool isTopmostWindow = false,
                                    int timeoutInSeconds = 0,  // Assume zero for the timeout value, which invokes the default value.
                                    FrameworkElement parent = null)
        {
            // Use the default options for everything except that which is explicitly set for this instance.
            var options = new JhMessageBoxOptions(DefaultOptions)
                .SetButtonFlags(buttons)
                .SetCaptionAfterPrefix(captionAfterPrefix)
                .SetDetailText(detailText)
                .SetSummaryText(summaryText)
                .SetIsAsynchronous(true)
                .SetToBeTopmostWindow(isTopmostWindow)
                .SetMessageType(messageType)
                .SetTimeoutPeriod(timeoutInSeconds)
                .SetParent(parent);
            NotifyUser(options);
        }
        #endregion NotifyUserAsync

        #endregion NotifyUser

        #region NotifyUserOfMistake
        /// <summary>
        /// Show a message-box to notify the user of a mistake that he has made in his operation of this program.
        /// </summary>
        /// <param name="summaryText">the text that goes in the upper, basic-message area</param>
        /// <param name="detailText">the more detailed text that goes in the lower area</param>
        /// <param name="captionAfterPrefix">what to put in the title-bar of the message-box - after the prefix that would contain the vendor and program-name</param>
        /// <param name="parent">the user-interface element to serve as the parent of the message-box, so that it can center itself over that</param>
        /// <returns>a JhDialogResult indicating what the user clicked on to close the message-box</returns>
        public JhDialogResult NotifyUserOfMistake(string summaryText, string detailText = null, string captionAfterPrefix = null, FrameworkElement parent = null)
        {
            //TODO: May want to provide for the possibility of having other buttons for this type of message-box. ?
            // Use the default options for everything except that which is explicitly set for this instance.
            var options = new JhMessageBoxOptions(DefaultOptions);
            // Derive a timeout value..
            int timeout = 0;
            if (_defaultOptions != null)
            {
                timeout = DefaultOptions.TimeoutPeriodForUserMistakes;
            }
            if (timeout == 0)
            {
                timeout = JhMessageBoxOptions.GetDefaultTimeoutValueFor(JhMessageBoxType.UserMistake);
            }
            // Derive a suitable text to use for the message-box caption if none was specified..
            string whatToUseForCaptionAfterPrefix = captionAfterPrefix;
            if (String.IsNullOrWhiteSpace(whatToUseForCaptionAfterPrefix))
            {
                if (DefaultOptions.CaptionForUserMistakes != null)
                {
                    whatToUseForCaptionAfterPrefix = DefaultOptions.CaptionForUserMistakes;
                }
                else
                {
                    //TODO: May want to be a little more creative here.
                    whatToUseForCaptionAfterPrefix = "Oops!";
                }
            }
#if SILVERLIGHT
            return NotifyUser(summaryText, detailText, whatToUseForCaptionAfterPrefix, JhMessageBoxButtons.Ok, JhMessageBoxType.UserMistake, timeout);
#else
            options.SummaryText = summaryText;
            options.DetailText = detailText;
            options.CaptionAfterPrefix = whatToUseForCaptionAfterPrefix;
            options.ButtonFlags = JhMessageBoxButtons.Ok;
            options.MessageType = JhMessageBoxType.UserMistake;
            options.ParentElement = parent;
            options.TimeoutPeriodInSeconds = timeout;
            return NotifyUser(options);
#endif
        }
        #endregion NotifyUserOfMistake

        #region NotifyUserOfMistakeAsync
        /// <summary>
        /// Asynchronously show a message-box to notify the user of a mistake that he has made in his operation of this program, and return immediately.
        /// </summary>
        /// <param name="summaryText">the text that goes in the upper, basic-message area</param>
        /// <param name="detailText">the more detailed text that goes in the lower area</param>
        /// <param name="captionAfterPrefix">what to put in the title-bar of the message-box - after the prefix that would contain the vendor and program-name</param>
        /// <param name="parent">the user-interface element to serve as the parent of the message-box, so that it can center itself over that</param>
        public void NotifyUserOfMistakeAsync(string summaryText, string detailText, string captionAfterPrefix, FrameworkElement parent = null)
        {
            //TODO:
            // Need to ensure it is asynchronous,
            // that we have a means of responding to the user-selection,
            // that the parent-window doesn't rise to occlude this one, since it will be shown non-modally
            // I'm not even sure this facility is even needed.
            NotifyUserOfMistake(summaryText, detailText, captionAfterPrefix, parent);
        }
        #endregion NotifyUserOfMistakeAsync

        #region NotifyUserOfError

        /// <summary>
        /// This is the main base-method that other overloads should call.
        /// This can be called from a background thread.
        /// </summary>
        /// <param name="toUser"></param>
        /// <param name="toDeveloper"></param>
        /// <param name="caption"></param>
        /// <param name="exception"></param>
        /// <param name="parent"></param>
        public void NotifyUserOfError(string toUser, string toDeveloper, string captionAfterPrefix, Exception exception, int timeout = 0, FrameworkElement parent = null)
        {
            // Ensure the code is called from the application's GUI thread.
            //TODO: Commented out for SL. In the case of SL, do I need to provide for calling this asynchronously?
#if !SILVERLIGHT
            Application.Current.InvokeIfRequired(() =>
            {
#endif
                if (String.IsNullOrWhiteSpace(toUser))
                {
                    toUser = "Uh oh!";
                }
                string detailText = toDeveloper;
                if (String.IsNullOrWhiteSpace(detailText))
                {
                    if (exception != null)
                    {
                        detailText = exception.Message;
                    }
                    else
                    {
                        detailText = "An embarrassing program-fault has ocurred.";
                    }
                }
                if (String.IsNullOrWhiteSpace(captionAfterPrefix))
                {
                    captionAfterPrefix = GetAnExpressionOfDismay();
                }
                //TODO: Should I do this, or use LogNut here?
                //      If LogNut is also doing this, we'd get duplicate text spilling out onto the Console.
                if (exception == null)
                {
                    Console.WriteLine("NotifyUserOfError(toUser: " + toUser + ", toDeveloper: " + toDeveloper);
                }
                else
                {
                    Console.Write("NotifyUserOfError(toUser: " + toUser + ", toDeveloper: " + toDeveloper + Environment.NewLine + ",  exception: ");
                    Console.WriteLine(exception.ToString());
                }
                if (timeout == 0)
                {
                    timeout = JhMessageBoxOptions.GetDefaultTimeoutValueFor(JhMessageBoxType.Error);
                }
#if SILVERLIGHT
                NotifyUser(toUser, detailText, captionAfterPrefix, JhMessageBoxButtons.Ok, JhMessageBoxType.Error, timeout);
#else
                NotifyUser(summaryText: toUser,
                           detailText: detailText,
                           captionAfterPrefix: null,
                           buttons: JhMessageBoxButtons.Ok,
                           messageType: JhMessageBoxType.Error,
                           timeout: timeout,
                           parent: parent);
#endif

#if !SILVERLIGHT
            });
#endif
        }

        public void NotifyUserOfError(string toUser, string toDeveloper = null, FrameworkElement parent = null)
        {
            NotifyUserOfError(toUser: toUser, toDeveloper: toDeveloper, captionAfterPrefix: null, exception: null, parent: parent);
        }

        public void NotifyUserOfError(string toUser, string toDeveloper, Exception exception, FrameworkElement parent = null)
        {
            NotifyUserOfError(toUser: toUser, toDeveloper: toDeveloper, captionAfterPrefix: null, exception: exception, parent: parent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="parent"></param>
        public void NotifyUserOfError(Exception exception, FrameworkElement parent = null)
        {
            NotifyUserOfError(toUser: null,
                              toDeveloper: null,
                              captionAfterPrefix: null,
                              exception: exception,
                              parent: parent);
        }
        #endregion NotifyUserOfError

        #region AskYesOrNo
        /// <summary>
        /// Display a message-box to the user asking a Yes-or-No question. Wait for his response or else close itself after the timeout has expired.
        /// </summary>
        /// <param name="question">the text of the question to pose to the user, which will go into the summary-text area of the message-box</param>
        /// <param name="defaultAnswer">this is the user-response to assume if the message-box is closed via its system-menu or times-out</param>
        /// <param name="detailText">additional text that can go into the detail-text area (optional)</param>
        /// <param name="caption">text to add to the normal default title-bar prefix that will appear as the 'caption' for this message-box (optional)</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        /// <returns>a JhDialogResult indicating which action the user took, or TimedOut if the user took no action before the timeout expired</returns>
        public JhDialogResult AskYesOrNo(string question,
                                             JhDialogResult defaultAnswer,
                                             string detailText = null,
                                             string caption = null,
                                             FrameworkElement parent = null)
        {
#if SILVERLIGHT
            return NotifyUser(question, null, sFullCaption, JhMessageBoxButtons.Yes | JhMessageBoxButtons.No, JhMessageBoxType.Question, timeout);
#else
            JhDialogResult r = NotifyUser(summaryText: question,
                                              detailText: null,
                                              captionAfterPrefix: caption,
                                              buttons: JhMessageBoxButtons.Yes | JhMessageBoxButtons.No,
                                              messageType: JhMessageBoxType.Question,
                                              timeout: 0,
                                              parent: parent);
            if (r == JhDialogResult.TimedOut)
            {
                return defaultAnswer;
            }
            else
            {
                return r;
            }
#endif
        }
        #endregion

        #region Show methods, for backward compatibility with MessageBox

        // These Show methods are simply provided for backward compatibility.
        //TODO: What should this really be? and it's duplicated in JhMessageBoxWindow.
        private const JhDialogResult _defaultResult = JhDialogResult.TimedOut;

        public JhDialogResult Show(string messageBoxText)
        {
            string caption = _captionPrefix;
            return Show(messageBoxText, caption, JhMessageBoxButtons.Ok, JhMessageBoxType.None, _defaultResult);
        }

        public JhDialogResult Show(string messageBoxText, string caption)
        {
            return Show(messageBoxText, caption, JhMessageBoxButtons.Ok, JhMessageBoxType.None, _defaultResult);
        }

        public JhDialogResult Show(string messageBoxText, string caption, JhMessageBoxButtons buttons)
        {
            return Show(messageBoxText, caption, buttons, JhMessageBoxType.None, _defaultResult);
        }

        public JhDialogResult Show(string messageBoxText, string caption, JhMessageBoxButtons buttons, JhMessageBoxType messageType)
        {
            return Show(messageBoxText, caption, buttons, messageType, _defaultResult);
        }

        public JhDialogResult Show(string messageBoxText, string caption, JhMessageBoxButtons buttons, JhMessageBoxType messageType, JhDialogResult defaultResult)
        {
#if !SILVERLIGHT
            FrameworkElement owner = Application.Current.MainWindow;
            return Show(messageBoxText, caption, buttons, messageType, _defaultResult, owner);
#else
            return Show(messageBoxText, caption, buttons, messageType, _defaultResult);
#endif
        }

        public JhDialogResult Show(string messageBoxText, FrameworkElement owner)
        {
            string caption = GetAReasonableTitlebarPrefix();
            return Show(messageBoxText, caption, JhMessageBoxButtons.Ok, JhMessageBoxType.None, _defaultResult, owner);
        }

        public JhDialogResult Show(string messageBoxText, string caption, FrameworkElement owner)
        {
            return Show(messageBoxText, caption, JhMessageBoxButtons.Ok, JhMessageBoxType.None, _defaultResult, owner);
        }

        public JhDialogResult Show(string messageBoxText, string caption, JhMessageBoxButtons buttons, FrameworkElement owner)
        {
            return Show(messageBoxText, caption, buttons, JhMessageBoxType.None, _defaultResult, owner);
        }

        public JhDialogResult Show(string messageBoxText, string caption, JhMessageBoxButtons buttons, JhMessageBoxType messageType, JhDialogResult defaultResult, FrameworkElement owner)
        {
            int defaultTimeout = JhMessageBoxOptions.GetDefaultTimeoutValueFor(messageType);
#if !SILVERLIGHT
            JhDialogResult r = NotifyUser(summaryText: messageBoxText,
                                              detailText: "",
                                              buttons: buttons,
                                              messageType: messageType,
                                              captionAfterPrefix: caption,
                                              timeout: defaultTimeout,
                                              parent: owner);
#else
            JhDialogResult r = NotifyUser(messageBoxText, "", caption, buttons, messageType, defaultTimeout);
#endif
            // In this library, we ordinarily consider the TimedOut result to be the default result,
            // but here the caller of this method has specified a specific defaultResult.
            if (r == JhDialogResult.TimedOut)
            {
                r = defaultResult;
            }
            return r;
        }

        #endregion Show methods

        #endregion the NotifyUser.. methods

        #region DefaultOptions
        /// <summary>
        /// Get or set the JhMessageBoxOptions to use by default for future invocations of a message-box.
        /// </summary>
        public JhMessageBoxOptions DefaultOptions
        {
            get
            {
                if (_defaultOptions == null)
                {
                    _defaultOptions = new JhMessageBoxOptions(JhMessageBoxType.None);
                }
                return _defaultOptions;
            }
            set { _defaultOptions = value; }
        }

        /// <summary>
        /// Set the options to use by default for subsequent invocations of a message-box.
        /// </summary>
        /// <param name="options">The options to set (the MessageType property is ignored)</param>
        public void SetDefaultOptions(JhMessageBoxOptions options)
        {
            DefaultOptions = options;
        }
        #endregion

        #region CaptionPrefix
        /// <summary>
        /// Get or set the text that's shown as the prefix of the title-bar of the Window.
        /// </summary>
        public string CaptionPrefix
        {
            get { return _captionPrefix; }
            set { _captionPrefix = value; }
        }

        /// <summary>
        /// Set the text that is to be shown as the prefix for the title-bar text of the message-box window.
        /// </summary>
        /// <param name="captionPrefix">the text to use for the title-bar prefix</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBox SetCaptionPrefix(string captionPrefix)
        {
            _captionPrefix = captionPrefix;
            return this;
        }
        #endregion

        #region unit-testing facilities

        #region BeginTest
        /// <summary>
        /// Turn on test-mode, and get a new instance of JhMessageBox.
        /// </summary>
        /// <returns></returns>
        public static JhMessageBox BeginTest()
        {
            var newInstance = GetNewInstance("TestVendor", "TestApp");
            // Having _testFacility being non-null puts this class into test-mode.
            newInstance._testFacility = new JhMessageBoxTestFacility(newInstance);
            return newInstance;
        }
        #endregion

        #region IsTesting
        /// <summary>
        /// Get whether message-boxes are being used in automated-test mode.
        /// This provides a way to check IsTesting without instanciating a JhMessageBoxTestFacility object.
        /// </summary>
        public bool IsTesting
        {
            get
            {
                if (_testFacility == null)
                {
                    return false;
                }
                else
                {
                    return TestFacility.IsTesting;
                }
            }
        }
        #endregion

        public JhMessageBoxWindow MessageBoxWindow
        {
            get { return _messageBoxWindow; }
        }

        #region TestFacility
        /// <summary>
        /// Get the message-box test-options that apply when running automated tests.
        /// </summary>
        public JhMessageBoxTestFacility TestFacility
        {
            get
            {
                return _testFacility;
            }
        }
        #endregion

        #region TextOf(the various buttons)

        public static string TextOfCancelButton
        {
            //TODO: This is hard-coded just for now -- it really needs to be converted to use a globalization facility.
            get { return "Cancel"; }
        }

        public static string TextOfCloseButton
        {
            //TODO: This is hard-coded just for now -- it really needs to be converted to use a globalization facility.
            get { return "Close"; }
        }

        public static string TextOfOkButton
        {
            //TODO: This is hard-coded just for now -- it really needs to be converted to use a globalization facility.
            get { return "OK"; }
        }

        public static string TextOfYesButton
        {
            //TODO: This is hard-coded just for now -- it really needs to be converted to use a globalization facility.
            get { return "Yes"; }
        }

        public static string TextOfNoButton
        {
            //TODO: This is hard-coded just for now -- it really needs to be converted to use a globalization facility.
            get { return "No"; }
        }

        public static string TextOfRetryButton
        {
            //TODO: This is hard-coded just for now -- it really needs to be converted to use a globalization facility.
            get { return "Retry"; }
        }

        public static string TextOfIgnoreButton
        {
            //TODO: This is hard-coded just for now -- it really needs to be converted to use a globalization facility.
            get { return "Ignore"; }
        }

        public static string TextOfAbortButton
        {
            //TODO: This is hard-coded just for now -- it really needs to be converted to use a globalization facility.
            get { return "Abort"; }
        }
        #endregion TextOf(the various buttons)

        #endregion unit-testing facilities

        #region ResultFrom(System.Windows.Forms.DialogResult)
#if !SILVERLIGHT
        /// <summary>
        /// Converts a System.Windows.Forms.DialogResult to our preferred JhDialogResult
        /// </summary>
        /// <param name="dialogResult">the System.Windows.Forms.DialogResult we want to convert</param>
        /// <returns>the coresponding JhDialogResult</returns>
        public static JhDialogResult ResultFrom(System.Windows.Forms.DialogResult dialogResult)
        {
            JhDialogResult r;
            switch (dialogResult)
            {
                case System.Windows.Forms.DialogResult.Abort:
                    r = JhDialogResult.Abort;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                    r = JhDialogResult.Cancel;
                    break;
                case System.Windows.Forms.DialogResult.Ignore:
                    r = JhDialogResult.Ignore;
                    break;
                case System.Windows.Forms.DialogResult.No:
                    r = JhDialogResult.No;
                    break;
                case System.Windows.Forms.DialogResult.None:
                    // Map None to TimedOut
                    r = JhDialogResult.TimedOut;
                    break;
                case System.Windows.Forms.DialogResult.OK:
                    r = JhDialogResult.Ok;
                    break;
                case System.Windows.Forms.DialogResult.Retry:
                    r = JhDialogResult.Retry;
                    break;
                case System.Windows.Forms.DialogResult.Yes:
                    r = JhDialogResult.Yes;
                    break;
                default:
                    r = JhDialogResult.Close;
                    break;
            }
            return r;
        }
#endif
        #endregion

        #region ResultFrom(System.Windows.MessageBoxResult)
        /// <summary>
        /// Converts a MessageBoxResult to our preferred JhDialogResult
        /// </summary>
        /// <param name="messageBoxResult">the MessageBoxResult we want to convert</param>
        /// <returns>the coresponding JhDialogResult</returns>
        public static JhDialogResult ResultFrom(MessageBoxResult messageBoxResult)
        {
            JhDialogResult r;
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    r = JhDialogResult.Yes;
                    break;
                case MessageBoxResult.No:
                    r = JhDialogResult.No;
                    break;
                case MessageBoxResult.Cancel:
                    r = JhDialogResult.Cancel;
                    break;
                case MessageBoxResult.OK:
                    r = JhDialogResult.Ok;
                    break;
                default:
                    r = JhDialogResult.Close;
                    break;
            }
            return r;
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Release any test-specific resources and turn off test-mode.
        /// </summary>
        public void Dispose()
        {
            if (_testFacility != null)
            {
                _testFacility.Dispose();
                // By setting JhMessageBox's _testFacility variable to null, we tell it that it is no longer in test-mode.
                _testFacility = null;
            }
            GC.SuppressFinalize(this);
        }
        #endregion

        #region internal implementation

        #region GetWhetherButtonIsIncluded
        /// <summary>
        /// See whether the TestFacility.ButtonResultToSelect is present amongst the given JhMessageBoxButtons
        /// and return true if it is.
        /// </summary>
        /// <param name="buttonFlags">the buttons to check against, to see whether the one that is required is present</param>
        /// <returns>true if the ButtonResultToSelect is included within the given buttonFlags, false otherwise</returns>
        internal bool GetWhetherButtonIsIncluded(JhMessageBoxButtons buttonFlags, out string complaint)
        {
            complaint = String.Empty;
            bool isOkay = true;
            if (_testFacility != null)
            {
                JhDialogResult? desiredResult = _testFacility.ButtonResultToSelect;
                if (desiredResult.HasValue)
                {
                    switch (desiredResult)
                    {
                        case JhDialogResult.Abort:
                            if ((buttonFlags & JhMessageBoxButtons.Cancel) == 0)
                            {
                                // If testing, and the test calls for this button to be selected - complain that it won't be possible.
                                complaint = "You want the Cancel button to be selected, but that button is not being shown!";
                                isOkay = false;
                            }
                            break;
                        case JhDialogResult.Cancel:
                            if ((buttonFlags & JhMessageBoxButtons.Cancel) == 0)
                            {
                                // If testing, and the test calls for this button to be selected - complain that it won't be possible.
                                complaint = "You want the Cancel button to be selected, but that button is not being shown!";
                                isOkay = false;
                            }
                            break;
                        case JhDialogResult.Close:
                            if ((buttonFlags & JhMessageBoxButtons.Close) == 0)
                            {
                                // If testing, and the test calls for this button to be selected - complain that it won't be possible.
                                complaint = "You want the Close button to be selected, but that button is not being shown!";
                                isOkay = false;
                            }
                            break;
                        case JhDialogResult.Ignore:
                            if ((buttonFlags & JhMessageBoxButtons.Ignore) == 0)
                            {
                                // If testing, and the test calls for this button to be selected - complain that it won't be possible.
                                complaint = "You want the Ignore button to be selected, but that button is not being shown!";
                                isOkay = false;
                            }
                            break;
                        case JhDialogResult.No:
                            if ((buttonFlags & JhMessageBoxButtons.No) == 0)
                            {
                                // If testing, and the test calls for this button to be selected - complain that it won't be possible.
                                complaint = "You want the No button to be selected, but that button is not being shown!";
                                isOkay = false;
                            }
                            break;
                        case JhDialogResult.Ok:
                            if ((buttonFlags & JhMessageBoxButtons.Ok) == 0)
                            {
                                // If testing, and the test calls for this button to be selected - complain that it won't be possible.
                                complaint = "You want the Ok button to be selected, but that button is not being shown!";
                                isOkay = false;
                            }
                            break;
                        case JhDialogResult.Retry:
                            if ((buttonFlags & JhMessageBoxButtons.Retry) == 0)
                            {
                                // If testing, and the test calls for this button to be selected - complain that it won't be possible.
                                complaint = "You want the Retry button to be selected, but that button is not being shown!";
                                isOkay = false;
                            }
                            break;
                        case JhDialogResult.Yes:
                            if ((buttonFlags & JhMessageBoxButtons.Yes) == 0)
                            {
                                // If testing, and the test calls for this button to be selected - complain that it won't be possible.
                                complaint = "You want the Yes button to be selected, but that button is not being shown!";
                                isOkay = false;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return isOkay;
        }
        #endregion GetWhetherButtonIsIncluded

        #region GetAnExpressionOfDismay
        /// <summary>
        /// Return a somewhat-varying English expression of dismay. This is intended specifically for heading an error-message.
        /// </summary>
        /// <returns>An English expression such as "oops" or "oh crap"</returns>
        public string GetAnExpressionOfDismay()
        {
            string somethingToSay = _someExpressionsOfDismay[_indexIntoExpressionsOfDismay];
            _indexIntoExpressionsOfDismay++;
            if (_indexIntoExpressionsOfDismay >= _someExpressionsOfDismay.Length)
            {
                _indexIntoExpressionsOfDismay = 0;
            }
            return somethingToSay;
        }
        #endregion

        #region GetAReasonableTitlebarPrefix
        /// <summary>
        /// Return a prefix to use in the title-bar of this message-box.
        /// </summary>
        /// <returns>A title-bar prefix that indicates the application, or else an empty-string if this is not an IApp.</returns>
        private string GetAReasonableTitlebarPrefix()
        {
            string titlebarPrefix;
            // If this application implements IApp, then use it's facility to get the title-bar prefix.
            var iapp = _ownerApplication as IApp;
            if (iapp != null)
            {
                titlebarPrefix = iapp.ProductIdentificationPrefix;
            }
            else
            {
                //TODO  How to do this for SL?
#if SILVERLIGHT
                titlebarPrefix = String.Empty;
#else
                titlebarPrefix = JhApp.GetProgramName();
                // Shorten it if it's too long..
                int n = titlebarPrefix.Length;
                const int nLimit = 64;
                if (n > nLimit)
                {
                    // Show just the last nLimit characters preceded by ".."
                    string sPartToShow = titlebarPrefix.Substring(n - nLimit - 2);
                    titlebarPrefix = sPartToShow + "..";
                }
#endif
            }
            return titlebarPrefix;
        }
        #endregion

        internal void SignalThatMessageBoxHasEnded(object sender, JhDialogResult result)
        {
            //TODO: May want to provide a value for error and isCancelled
            JhMessageBoxWindow messageBoxWindow = sender as JhMessageBoxWindow;
            JhMessageBoxOptions options = messageBoxWindow._options;
            options.SignalThatMessageBoxHasEnded(messageBoxWindow, result);
        }

        #region fields

        /// <summary>
        /// the JhMessageBoxOptions to use by default for invocations of this message-box
        /// </summary>
        private JhMessageBoxOptions _defaultOptions;
        /// <summary>
        /// This is the standard prefix-text that is shown on the caption.
        /// </summary>
        private string _captionPrefix;
        /// <summary>
        /// This stores a reference to the most recent message-box window that is currently being shown.
        /// </summary>
        private JhMessageBoxWindow _messageBoxWindow;
        private System.Windows.Application _ownerApplication;

        private int _indexIntoExpressionsOfDismay;
        private string[] _someExpressionsOfDismay = { "Oops!", "Oh man!", "Well isn't that special!", "Well, it's like this..", "Awh crap!" };

        /// <summary>
        /// This contains the set of test-options, and helper methods, that apply specifically for running automated tests.
        /// </summary>
        internal JhMessageBoxTestFacility _testFacility;

        #endregion fields

        #endregion internal implementation
    }
    #endregion class JhMessageBox
}
