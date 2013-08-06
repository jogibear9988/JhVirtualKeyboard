using System;
using System.Windows;


namespace JhLib
{
    #region class JhMessageBoxOptions
    /// <summary>
    /// class JhMessageBoxOptions encapsulates all of the parameter options that you may want to set on your JhMessageBox,
    /// in case you prefer to invoke it with one object containing all of the parameter values.
    /// </summary>
    public sealed class JhMessageBoxOptions
    {
        public event EventHandler<MessageBoxCompletedArgs> Completed = delegate { };

        #region constructors
        /// <summary>
        /// Default Constructor: Create a new JhMessageBoxOptions object with the a JhMessageBoxType of None.
        /// </summary>
        public JhMessageBoxOptions()
        {
            this._messageType = JhMessageBoxType.None;
        }

        /// <summary>
        /// Create a new JhMessageBoxOptions object with the given JhMessageBoxType and default values.
        /// </summary>
        public JhMessageBoxOptions(JhMessageBoxType whichType)
            : this()
        {
            this._messageType = whichType;
        }

        /// <summary>
        /// The Copy-Constructor. This does NOT copy the SummaryText or DetailText.
        /// </summary>
        /// <param name="source">the JhMessageBoxOptions object to copy the values from</param>
        public JhMessageBoxOptions(JhMessageBoxOptions source)
        {
            this._backgroundTexture = source.BackgroundTexture;
            this._buttonFlags = source._buttonFlags;
            //            this._captionPrefix = source.CaptionPrefix;
            this._captionAfterPrefix = source.CaptionAfterPrefix;
            this._captionForUserMistakes = source.CaptionForUserMistakes;
            this._isCustomButtonStyles = source.IsCustomButtonStyles;
            this._isSoundEnabled = source.IsSoundEnabled;
            this._isToCenterOverParent = source.IsToCenterOverParent;
            this._isToBeTopmostWindow = source.IsToBeTopmostWindow;
            this._isTouch = source.IsTouch;
            this._isUsingAeroGlassEffect = source.IsUsingAeroGlassEffect;
            this._isUsingNewerIcons = source.IsUsingNewerIcons;
            this._isUsingNewerSoundScheme = source.IsUsingNewerSoundScheme;
            this._messageType = source.MessageType;
            this._timeoutPeriodInSeconds = source.TimeoutPeriodInSeconds;
            this._timeoutPeriodForUserMistakes = source.TimeoutPeriodForUserMistakes;
        }
        #endregion

        #region BackgroundTexture
        /// <summary>
        /// Get or set one (or none) out of a set of predefined background images for the message-box window. Default is None.
        /// </summary>
        public JhMessageBoxBackgroundTexture BackgroundTexture
        {
            get { return _backgroundTexture; }
            set { _backgroundTexture = value; }
        }

        /// <summary>
        /// Set one (or none) out of a set of predefined background images for the message-box window. Default is None.
        /// </summary>
        /// <param name="imageToUse">Which of the predefined images to use for the background texture</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBoxOptions SetBackgroundTexture(JhMessageBoxBackgroundTexture imageToUse)
        {
            _backgroundTexture = imageToUse;
            return this;
        }
        #endregion

        #region buttons
        /// <summary>
        /// Get or set the flags that dictate which buttons to show. The default is to just show the "Ok" button.
        /// </summary>
        public JhMessageBoxButtons ButtonFlags
        {
            get { return _buttonFlags; }
            set { _buttonFlags = value; }
        }

        /// <summary>
        /// Set the flags that dictate which buttons to show. The default is to just show the "Ok" button.
        /// </summary>
        /// <param name="buttons">A bitwise-combination of the enum flags defining which buttons to show</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBoxOptions SetButtonFlags(JhMessageBoxButtons buttons)
        {
            _buttonFlags = buttons;
            return this;
        }

        public string GetButtonText(JhDialogResult forWhichResponse)
        {
            string buttonText = String.Empty;
            switch (forWhichResponse)
            {
                case JhDialogResult.Abort:
                    buttonText = _buttonAbortText;
                    break;
                case JhDialogResult.Cancel:
                    buttonText = _buttonCancelText;
                    break;
                case JhDialogResult.Close:
                    buttonText = _buttonCloseText;
                    break;
                case JhDialogResult.Ignore:
                    buttonText = _buttonIgnoreText;
                    break;
                case JhDialogResult.No:
                    buttonText = _buttonNoText;
                    break;
                case JhDialogResult.Ok:
                    buttonText = _buttonOkText;
                    break;
                case JhDialogResult.Retry:
                    buttonText = _buttonRetryText;
                    break;
                case JhDialogResult.Yes:
                    buttonText = _buttonYesText;
                    break;
                default:
                    break;
            }
            return buttonText;
        }

        public string ButtonAbortText
        {
            get { return _buttonAbortText; }
        }

        public string ButtonCancelText
        {
            get { return _buttonCancelText; }
        }

        public string ButtonCloseText
        {
            get { return _buttonCloseText; }
        }

        public string ButtonIgnoreText
        {
            get { return _buttonIgnoreText; }
        }

        public string ButtonNoText
        {
            get { return _buttonNoText; }
        }

        public string ButtonOkText
        {
            get { return _buttonOkText; }
        }

        public string ButtonRetryText
        {
            get { return _buttonRetryText; }
        }

        public string ButtonYesText
        {
            get { return _buttonYesText; }
        }

        public JhMessageBoxOptions SetButtonText(JhDialogResult forWhichResponse, string buttonText)
        {
            switch (forWhichResponse)
            {
                case JhDialogResult.Abort:
                    _buttonAbortText = buttonText;
                    break;
                case JhDialogResult.Cancel:
                    _buttonCancelText = buttonText;
                    break;
                case JhDialogResult.Close:
                    _buttonCloseText = buttonText;
                    break;
                case JhDialogResult.Ignore:
                    _buttonIgnoreText = buttonText;
                    break;
                case JhDialogResult.No:
                    _buttonNoText = buttonText;
                    break;
                case JhDialogResult.Ok:
                    _buttonOkText = buttonText;
                    break;
                case JhDialogResult.Retry:
                    _buttonRetryText = buttonText;
                    break;
                case JhDialogResult.Yes:
                    _buttonYesText = buttonText;
                    break;
                default:
                    break;
            }
            return this;
        }

        public string ButtonAbortToolTip
        {
            get { return _buttonAbortToolTipText; }
        }

        public string ButtonCancelToolTip
        {
            get { return _buttonCancelToolTipText; }
        }

        public string ButtonCloseToolTip
        {
            get { return _buttonCloseToolTipText; }
        }

        public string ButtonIgnoreToolTip
        {
            get { return _buttonIgnoreToolTipText; }
        }

        public string ButtonNoToolTip
        {
            get { return _buttonNoToolTipText; }
        }

        public string ButtonOkToolTip
        {
            get { return _buttonOkToolTipText; }
        }

        public string ButtonRetryToolTip
        {
            get { return _buttonRetryToolTipText; }
        }

        public string ButtonYesToolTip
        {
            get { return _buttonYesToolTipText; }
        }

        public JhMessageBoxOptions SetButtonToolTip(JhDialogResult forWhichResponse, string tooltipText)
        {
            switch (forWhichResponse)
            {
                case JhDialogResult.Abort:
                    _buttonAbortToolTipText = tooltipText;
                    break;
                case JhDialogResult.Cancel:
                    _buttonCancelToolTipText = tooltipText;
                    break;
                case JhDialogResult.Close:
                    _buttonCloseToolTipText = tooltipText;
                    break;
                case JhDialogResult.Ignore:
                    _buttonIgnoreToolTipText = tooltipText;
                    break;
                case JhDialogResult.No:
                    _buttonNoToolTipText = tooltipText;
                    break;
                case JhDialogResult.Ok:
                    _buttonOkToolTipText = tooltipText;
                    break;
                case JhDialogResult.Retry:
                    _buttonRetryToolTipText = tooltipText;
                    break;
                case JhDialogResult.Yes:
                    _buttonYesToolTipText = tooltipText;
                    break;
                default:
                    break;
            }
            return this;
        }
        #endregion buttons

        #region caption

        /// <summary>
        /// Get or set the initial text that's shown in the title-bar of the Window, which normally would be comprised of
        /// the vendor-name followed by the application-name. This is an override-value; it defaults to null.
        /// </summary>
        public string CaptionPrefix
        {
            get { return _captionPrefix; }
            set { _captionPrefix = value; }
        }

        /// <summary>
        /// Get or set the text that's shown in the title-bar of the Window.
        /// </summary>
        public string CaptionAfterPrefix
        {
            get { return _captionAfterPrefix; }
            set { _captionAfterPrefix = value; }
        }

        /// <summary>
        /// Set the text that is to be shown (after the prefix) in the title-bar of the message-box window.
        /// </summary>
        /// <param name="caption">the text to use for the new title-bar content</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBoxOptions SetCaptionAfterPrefix(string caption)
        {
            _captionAfterPrefix = caption;
            return this;
        }
        #endregion caption

        #region CaptionForUserMistakes
        /// <summary>
        /// Get or set an optional, fixed title-bar text (or caption) to use for message-boxes of type UserMistake.
        /// </summary>
        public string CaptionForUserMistakes
        {
            get
            {
                if (_captionForUserMistakes != null)
                {
                    return _captionForUserMistakes;
                }
                else
                {
                    return _captionAfterPrefix;
                }
            }
            set
            {
                _captionForUserMistakes = value;
            }
        }

        /// <summary>
        /// Set a fixed titlebar-text (aside from the standard prefix) to use for message-boxes of type UserMistake.
        /// </summary>
        /// <param name="caption">the text to use for the title-bar content of UserMistake message-boxes</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBoxOptions SetCaptionForUserMistakes(string caption)
        {
            _captionForUserMistakes = caption;
            return this;
        }
        #endregion

        #region SummaryText
        /// <summary>
        /// Get or set the text that's shown in the upper, summary-text area.
        /// </summary>
        public string SummaryText
        {
            get { return _summaryText; }
            set { _summaryText = value; }
        }

        /// <summary>
        /// Set the text that's shown in the upper, summary-text area.
        /// </summary>
        /// <param name="summaryText">the string to use as the content of the summary-text section of the message-box</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBoxOptions SetSummaryText(string summaryText)
        {
            _summaryText = summaryText;
            return this;
        }
        #endregion

        #region DetailText
        /// <summary>
        /// Get or set the text to be shown to the user.
        /// </summary>
        public string DetailText
        {
            get { return _detailText; }
            set { _detailText = value; }
        }

        /// <summary>
        /// Set the text to be shown to the user.
        /// </summary>
        /// <param name="userText">the string to use as the content of the user-text section of the message-box</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBoxOptions SetDetailText(string userText)
        {
            _detailText = userText;
            return this;
        }
        #endregion

        #region GetDefaultTimeoutValueFor
        /// <summary>
        /// Get the default value to use for the message-box timeout, in seconds, for the given type of message-box.
        /// </summary>
        public static int GetDefaultTimeoutValueFor(JhMessageBoxType forWhatTypeOfMessageBox)
        {
            int t;
            switch (forWhatTypeOfMessageBox)
            {
                case JhMessageBoxType.Warning:
                    t = 15;
                    break;
                case JhMessageBoxType.Error:
                case JhMessageBoxType.SecurityIssue:
                    t = 30;
                    break;
                case JhMessageBoxType.Question:
                    t = 60;
                    break;
                default:
                    t = 10;
                    break;
            }
            return t;
        }
        #endregion

        #region IsAsynchronous
        /// <summary>
        /// Get or set whether to show the message-box as a non-modal dialog window, and return immediately from the method-call that invoked it,
        /// as opposed to showing it as a modal window and waiting for it to close before returning. Default is false.
        /// </summary>
        public bool IsAsynchronous
        {
            get { return _isAsynchronous; }
            set { _isAsynchronous = value; }
        }

        /// <summary>
        /// Set whether to show the message-box as a non-modal dialog window, and return immediately from the method-call that invoked it,
        /// as opposed to showing it as a modal window and waiting for it to close before returning. Default is false.
        /// </summary>
        /// <param name="isToInvokeAsynchronously">true to display as a non-modal dialog (ie, asynchronously), false to operate synchronously</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBoxOptions SetIsAsynchronous(bool isToInvokeAsynchronously)
        {
            IsAsynchronous = isToInvokeAsynchronously;
            return this;
        }
        #endregion

        #region IsCustomButtonStyles
        /// <summary>
        /// Get or set whether to use our own custom WPF Styles for the message-box buttons. Default is false - to use the plain standard style.
        /// </summary>
        public bool IsCustomButtonStyles
        {
            get { return _isCustomButtonStyles; }
            set { _isCustomButtonStyles = value; }
        }

        /// <summary>
        /// Set whether to use our own custom WPF Styles for the message-box buttons. Default is false.
        /// </summary>
        /// <param name="isToUseCustomStyles">true to make use of available custom button styles, false to stick with the standard appearance</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBoxOptions SetToUseCustomButtonStyles(bool isToUseCustomStyles)
        {
            _isCustomButtonStyles = isToUseCustomStyles;
            return this;
        }
        #endregion

        #region IsSoundEnabled
        /// <summary>
        /// Get or set the flag that indicates whether this message-box will make a sound when it shows. Defaults to true.
        /// </summary>
        public bool IsSoundEnabled
        {
            get { return _isSoundEnabled; }
            set { _isSoundEnabled = value; }
        }

        /// <summary>
        /// Set whether to make a sound when this message-box is displayed. Default is true.
        /// </summary>
        /// <param name="isToPlaySoundEffects">true to use the standard icons, false to use the non-standard set</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBoxOptions SetIsSoundEnabled(bool isToPlaySoundEffects)
        {
            IsSoundEnabled = isToPlaySoundEffects;
            return this;
        }
        #endregion

        #region IsToCenterOverParent
        /// <summary>
        /// Get or set whether we want to center the message-box window over that of the parent window. Default is true.
        /// </summary>
        public bool IsToCenterOverParent
        {
            get { return _isToCenterOverParent; }
            set { _isToCenterOverParent = value; }
        }

        /// <summary>
        /// Set whether we want to center the message-box window over that of the parent window. Default is true.
        /// </summary>
        /// <param name="isCentered"></param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBoxOptions SetToCenterOverParent(bool isToCenterOverParentWindow)
        {
            _isToCenterOverParent = isToCenterOverParentWindow;
            return this;
        }
        #endregion

        #region IsToBeTopmostWindow
        /// <summary>
        /// Get or set whether we want the message-box to position itself over top of any other window
        /// that is on the Windows Desktop. The default is false.
        /// </summary>
        public bool IsToBeTopmostWindow
        {
            get { return _isToBeTopmostWindow; }
            set { _isToBeTopmostWindow = value; }
        }

        /// <summary>
        /// Set whether we want the message-box to position itself over top of any other window
        /// that is on the Windows Desktop. Default is false.
        /// </summary>
        /// <param name="isToBeOnTop">true if we want to position this over top of all other windows</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBoxOptions SetToBeTopmostWindow(bool isToBeOnTop)
        {
            _isToBeTopmostWindow = isToBeOnTop;
            return this;
        }
        #endregion

        #region IsTouch
        /// <summary>
        /// Gets or sets a value that indicates whether we want the message-box to behave with the assumption that
        /// it is being displayed upon a multi-touch monitor.  Defaults to false.
        /// </summary>
        public bool IsTouch
        {
            get { return _isTouch; }
            set { _isTouch = value; }
        }

        /// <summary>
        /// Sets whether we want the message-box to behave with the assumption that
        /// it is being displayed upon a multi-touch monitor.  Default is false.
        /// </summary>
        /// <param name="isTouchSensitive">True if we want to run on a multi-touch display</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBoxOptions SetIsTouch(bool isTouchSensitive)
        {
            _isTouch = isTouchSensitive;
            return this;
        }
        #endregion

        #region IsUsingAeroGlassEffect
        /// <summary>
        /// Get or set whether to use the Windows Vista/7 Aero-glass translucency effect for this Window. Default is no.
        /// </summary>
        public bool IsUsingAeroGlassEffect
        {
            get { return _isUsingAeroGlassEffect; }
            set { _isUsingAeroGlassEffect = value; }
        }
        #endregion

        #region IsUsingNewerIcons
        /// <summary>
        /// Get or set whether to use owr own set, as opposed to the old-style standard operating-system icons. Default is true.
        /// </summary>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public bool IsUsingNewerIcons
        {
            get { return _isUsingNewerIcons; }
            set { _isUsingNewerIcons = value; }
        }

        /// <summary>
        /// Set whether to use our own newer set of message-box icons, as opposed to the older standard set. Default is true.
        /// </summary>
        /// <param name="isToUseNewerIcons">true to use the new set that I provided, false to use the old standard set</param>
        public JhMessageBoxOptions SetToUseNewIcons(bool isToUseNewerIcons)
        {
            IsUsingNewerIcons = isToUseNewerIcons;
            return this;
        }
        #endregion

        #region IsUsingNewerSoundScheme
        /// <summary>
        /// Get or set whether to use the our own custom set of sound effects, as opposed to the old-style default Windows set of messagebox sounds
        /// Default is false.
        /// </summary>
        public bool IsUsingNewerSoundScheme
        {
            get { return _isUsingNewerSoundScheme; }
            set { _isUsingNewerSoundScheme = value; }
        }

        /// <summary>
        /// Set whether to use our own custom set of sound effects, as opposed to the default Windows set of messagebox sounds.
        /// Default is false.
        /// </summary>
        /// <param name="isToUseNewerSoundScheme">true to use the newer set, false to use the old standard set</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBoxOptions SetToUseNewSounds(bool isToUseNewerSoundScheme)
        {
            IsUsingNewerSoundScheme = isToUseNewerSoundScheme;
            return this;
        }
        #endregion

        #region MaximumTimeoutPeriodInSeconds
        /// <summary>
        /// Get or set the maximum value that you can set the TimeoutPeriodInSeconds property to,
        /// thus defining the uppermost-limit for how long this dialog-window will be shown
        /// before it takes a default response and closes. In units of seconds.
        /// This is currently fixed at five minutes.
        /// </summary>
        public static int MaximumTimeoutPeriodInSeconds
        {
            get { return _maximumTimeoutPeriodInSeconds; }
        }
        #endregion

        #region MessageType
        /// <summary>
        /// Get or set the basic type of this message-box (i.e., Information, Warning, Error, etc.).
        /// The default is None.
        /// </summary>
        public JhMessageBoxType MessageType
        {
            get { return _messageType; }
            set { _messageType = value; }
        }

        /// <summary>
        /// Set the basic type of this message-box (i.e., Information, Warning, Error, etc.). Default is None.
        /// </summary>
        /// <param name="messageBoxType">an enum-type that defines the basic purpose of this message-box</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBoxOptions SetMessageType(JhMessageBoxType messageBoxType)
        {
            _messageType = messageBoxType;
            return this;
        }
        #endregion

        #region ParentElement
        /// <summary>
        /// Get or set the visual element that is considered to be the "parent" or owner of the message-box, such that it can know what
        /// to align itself with. This is null by default, which means no parent has been designated.
        /// </summary>
        public FrameworkElement ParentElement
        {
            get { return _parentElement; }
            set { _parentElement = value; }
        }

        /// <summary>
        /// Set the visual element that is considered to be the "parent" or owner of the message-box, such that it can know what
        /// to align itself with. This is null by default, which means no parent has been designated.
        /// </summary>
        /// <param name="parent">The parent-Window to associate the message-box with</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBoxOptions SetParent(FrameworkElement parent)
        {
            this.ParentElement = parent;
            return this;
        }
        #endregion

        #region SignalThatMessageBoxHasEnded
        /// <summary>
        /// Raise the Completed event with parameters indicating the current message-box window and the user-response
        /// it was closed with.
        /// </summary>
        /// <remarks>
        /// This is attached to JhMessageBoxOptions because it's handler needs to exist at the instance-level,
        /// and the Options object is easily available to the developer.
        /// </remarks>
        /// <param name="messageBoxWindow">the message-box window that signaled this event</param>
        /// <param name="result">the user-response with which the message-box window was dismissed</param>
        public void SignalThatMessageBoxHasEnded(JhMessageBoxWindow messageBoxWindow, JhDialogResult result)
        {
            var args = new MessageBoxCompletedArgs(result: result, error: null, messageBoxWindow: messageBoxWindow);
            this.Completed(this, args);
        }
        #endregion

        #region TimeoutPeriodInSeconds
        /// <summary>
        /// Get or set the time-span for which the JhMessageBox will be shown before it takes a default response and goes away. In seconds.
        /// Default is 10 seconds.
        /// </summary>
        public int TimeoutPeriodInSeconds
        {
            get { return _timeoutPeriodInSeconds; }
            set
            {
                _timeoutPeriodInSeconds = Math.Min(value, _maximumTimeoutPeriodInSeconds);
            }
        }

        /// <summary>
        /// Set the time-span for which the JhMessageBox will be shown before it takes a default response and goes away. In seconds.
        /// Default is 10 seconds.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBoxOptions SetTimeoutPeriod(int seconds)
        {
            _timeoutPeriodInSeconds = Math.Min(seconds, _maximumTimeoutPeriodInSeconds);
            return this;
        }
        #endregion

        #region TimeoutPeriodForUserMistakes
        /// <summary>
        /// Get the value which can specify a distinct default timeout-period (in seconds) for message-boxes when the message-type is UserMistake.
        /// If this is zero then the value of property TimeoutPeriodInSeconds is used - which is the default.
        /// </summary>
        public int TimeoutPeriodForUserMistakes
        {
            get { return _timeoutPeriodForUserMistakes; }
            set
            {
                _timeoutPeriodForUserMistakes = Math.Min(value, _maximumTimeoutPeriodInSeconds);
            }
        }

        /// <summary>
        /// Get the value which can specify a distinct default timeout-period (in seconds) for message-boxes when the message-type is UserMistake.
        /// If this is zero (which is the default) then the value of property TimeoutPeriodInSeconds is used.
        /// </summary>
        /// <param name="seconds">The number of seconds to allow before timing-out, or 0 to just use the default</param>
        /// <returns>a reference to this same object such that other method-calls may be chained</returns>
        public JhMessageBoxOptions SetTimeoutPeriodForUserMistakes(int seconds)
        {
            _timeoutPeriodForUserMistakes = Math.Min(seconds, _maximumTimeoutPeriodInSeconds);
            return this;
        }
        #endregion

        #region fields

        /// <summary>
        /// This selects one (or none) out of a set of predefined background images for the message-box window.
        /// The default value is None.
        /// </summary>
        private JhMessageBoxBackgroundTexture _backgroundTexture = JhMessageBoxBackgroundTexture.None;
        /// <summary>
        /// This specifies which of the pre-supplied buttons are to be shown. The default is just the Ok button.
        /// </summary>
        private JhMessageBoxButtons _buttonFlags = JhMessageBoxButtons.Ok;

        private string _buttonAbortText = "Abort";
        private string _buttonCancelText = "Cancel";
        private string _buttonCloseText = "Close";
        private string _buttonIgnoreText = "Ignore";
        private string _buttonNoText = "No";
        private string _buttonOkText = "Ok";
        private string _buttonRetryText = "Retry";
        private string _buttonYesText = "Yes";

        private string _buttonAbortToolTipText;
        private string _buttonCancelToolTipText;
        private string _buttonCloseToolTipText;
        private string _buttonIgnoreToolTipText;
        private string _buttonNoToolTipText;
        private string _buttonOkToolTipText;
        private string _buttonRetryToolTipText;
        private string _buttonYesToolTipText;

        /// <summary>
        /// The text that is shown in the message-box title-bar, after the prefix + ": "..
        /// The initial value for this is null.
        /// </summary>
        private string _captionAfterPrefix;
        /// <summary>
        /// The caption-text to show by default for the message-box title-bar text (after the prefix) for user-mistake message types.
        /// </summary>
        private string _captionForUserMistakes;
        /// <summary>
        /// The initial text to show in the title-bar of the message-box, to which a colon-space and then the caption-text is shown.
        /// This would normally be left null, so that the application's inherited facilities will generate this.
        /// </summary>
        private string _captionPrefix;
        //TODO: Let's call this something else. There's too much inconsistency between
        //      SummaryText and UserMessage.
        //
        // How about:
        //    TextHeadline
        //    TextExplanation
        private string _summaryText;
        private string _detailText;
        /// <summary>
        /// This flag indicates whether this message-box was invoked asynchronously;  ie with, for example, NotifyUserAsync
        /// </summary>
        private bool _isAsynchronous;
        /// <summary>
        /// This indicates whether to use our own custom WPF Styles for the message-box buttons. Default is false.
        /// </summary>
        private bool _isCustomButtonStyles;
        /// <summary>
        /// This flag controls whether the audial effects are generated when a message-box is shown. Default is true.
        /// </summary>
        private bool _isSoundEnabled = true;
        /// <summary>
        /// This indicates whether we want to center the message-box window over that of the parent window. Default is true.
        /// </summary>
        private bool _isToCenterOverParent = true;
        /// <summary>
        /// This dictates whether we want the message-box to position itself over top of any other window
        /// that is on the Windows Desktop. Default is false.
        /// </summary>
        private bool _isToBeTopmostWindow;
        /// <summary>
        /// This flag indicates that we are to act as though executing on a multi-touch screen. Default is false.
        /// </summary>
        private bool _isTouch;
        /// <summary>
        /// If true then the Windows Vista/7 Aero-glass translucency effect is used for this Window. Default is false.
        /// </summary>
        private bool _isUsingAeroGlassEffect;
        /// <summary>
        /// This flag indicates whether to use the custom set of audio files I provided, as opposed to the original sounds. Default is false.
        /// </summary>
        private bool _isUsingNewerSoundScheme;
        /// <summary>
        /// This flag indicates whether to use the custom set of icons I designed, as opposed to the old set. Default is true.
        /// </summary>
        private bool _isUsingNewerIcons = true;
        /// <summary>
        /// This number represents the greatist-possible timeout value that is allowed for a message-box.
        /// </summary>
        private const int _maximumTimeoutPeriodInSeconds = 5 * 60;  // ie, five minutes
        /// <summary>
        /// This denotes the basic "type" of this message-box - that is, whether it is a Warning, Error, Question, whatever.
        /// The default set in the constructor is None.
        /// </summary>
        private JhMessageBoxType _messageType;
        /// <summary>
        /// The visual element that is considered to be the "parent" or owner of the message-box, such that it can know what
        /// to align itself with. This is null by default, which means no parent has been designated.
        /// </summary>
        private FrameworkElement _parentElement;
        /// <summary>
        /// The time interval to wait before it "times-out" and we close the message-box automatically. Default is ten seconds.
        /// </summary>
        private int _timeoutPeriodInSeconds = 10;
        /// <summary>
        /// If non-zero, this denotes a distinct default timeout-period (in seconds) for message-boxes when the message-type is UserMistake.
        /// </summary>
        private int _timeoutPeriodForUserMistakes;

        #endregion fields
    }
    #endregion class JhMessageBoxOptions
}
