// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Windows;


namespace JhLib
{
    /// <summary>
    /// This interface specifies the defeault user and developer conversation facility that any IApp
    /// is expected to provide, including MessageBox dialog windows and logging.
    /// </summary>
    public interface IInterlocution
    {
        #region NotifyUser methods

        // If these have only an OK button, does it make sense to return a result?

        /// <summary>
        /// Display a message-box to the user, based upon the given options.
        /// </summary>
        /// <param name="options">a JhMessageBoxOptions object that fully specifies everything about the message-box</param>
        /// <returns>a JhDialogResult indicating which action the user took, or TimedOut if the user took no action before the timeout expired</returns>
        JhDialogResult NotifyUser(JhMessageBoxOptions allOptions);

        /// <summary>
        /// Display a message-box to the user. Wait for his response or else close itself after the timeout has expired.
        /// The message-type is assumed to be JhMessageBoxType.Information
        /// </summary>
        /// <param name="ofWhat">The text message to display to the user</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        /// <returns>A JhDialogResult that indicates the user's response</returns>
        JhDialogResult NotifyUser(string summaryText, FrameworkElement parent = null);

        /// <summary>
        /// Display a message-box to the user. Wait for his response or else close itself after the timeout has expired.
        /// The message-type is assumed to be JhMessageBoxType.Information
        /// </summary>
        /// <param name="summaryText">the summary text to show in the upper area</param>
        /// <param name="detailText">the detail text to show in the lower area</param>
        /// <param name="captionAfterPrefix">what to show in the titlebar of this message-box, after the standard prefix (optional)</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        /// <returns>a JhDialogResult that signals what the user clicked, or whether it timed-out</returns>
        JhDialogResult NotifyUser(string summaryText, string detailText, string captionAfterPrefix = null, FrameworkElement parent = null);

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
        JhDialogResult NotifyUser(string summaryText,
                                      string detailText,
                                      JhMessageBoxButtons buttons,
                                      JhMessageBoxType messageType = JhMessageBoxType.Information,
                                      string captionAfterPrefix = null,
                                      bool isTopmostWindow = false,
                                      int timeout = 0,
                                      FrameworkElement parent = null);


        JhDialogResult NotifyUserOfMistake(string summaryText, string detailText = null, string captionAfterPrefix = null, FrameworkElement parent = null);

 //TODO: How to handle the toDeveloper part?

        void NotifyUserOfError(string toUser, string toDeveloper = null, FrameworkElement parent = null);
        void NotifyUserOfError(string toUser, string toDeveloper, Exception exception, FrameworkElement parent = null);
        void NotifyUserOfError(Exception exception, FrameworkElement parent = null);
        void NotifyUserOfError(string toUser, string toDeveloper, string captionAfterPrefix, Exception exception, int timeout = 0, FrameworkElement parent = null);

        /// <summary>
        /// Display a message-box to the user asking a Yes-or-No question. Wait for his response or else close itself after the timeout has expired.
        /// </summary>
        /// <param name="question">the text of the question to pose to the user, which will go into the summary-text area of the message-box</param>
        /// <param name="defaultAnswer">this is the user-response to assume if the message-box is closed via its system-menu or times-out</param>
        /// <param name="detailText">additional text that can go into the detail-text area (optional)</param>
        /// <param name="caption">text to add to the normal default title-bar prefix that will appear as the 'caption' for this message-box (optional)</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        /// <returns>a JhDialogResult indicating which action the user took, or TimedOut if the user took no action before the timeout expired</returns>
        JhDialogResult AskYesOrNo(string question,
                                      JhDialogResult defaultAnswer,
                                      string detailText = null,
                                      string captionSuffix = null,
                                      FrameworkElement parent = null);

        #region asynchronous versions

        /// <summary>
        /// Display a message-box to the user as a non-modal dialog window, and return immediately.
        /// The message-type is assumed to be JhMessageBoxType.Information
        /// </summary>
        /// <param name="summaryText">The text message to display to the user</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        void NotifyUserAsync(string summaryText, FrameworkElement parent = null);

        /// <summary>
        /// Display a message-box to the user as a non-modal dialog window, and return immediately.
        /// The message-type is assumed to be JhMessageBoxType.Information
        /// </summary>
        /// <param name="summaryText">the summary text to show in the upper area</param>
        /// <param name="detailText">the detail text to show in the lower area</param>
        /// <param name="captionAfterPrefix">what to show in the titlebar of this message-box, after the standard prefix (optional)</param>
        /// <param name="parent">the visual-element to consider as the parent, or owner, of this message-box (optional)</param>
        void NotifyUserAsync(string summaryText, string detailText, string captionAfterPrefix = null, FrameworkElement parent = null);

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
        void NotifyUserAsync(string summaryText,
                             string detailText,
                             JhMessageBoxButtons buttons = JhMessageBoxButtons.Ok,
                             JhMessageBoxType messageType = JhMessageBoxType.Information,
                             string captionAfterPrefix = null,
                             bool isTopmostWindow = false,
                             int timeoutInSeconds = 0,
                             FrameworkElement parent = null);

        #endregion asynchronous versions

        #endregion NotifyUser methods


        /// <summary>
        /// Set the options to use by default for subsequent invocations of a message-box.
        /// </summary>
        /// <param name="options">The options to set (the MessageType property is ignored)</param>
        void SetDefaultOptions(JhMessageBoxOptions options);
    }
}
