// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Collections.Generic;
using System.IO;  // for UnmanagedMemoryStream
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;


namespace JhLib
{
    #region class JhMessageBoxWindow
    /// <summary>
    /// Our replacement for MessageBox. This is used via the JhMessageBox class.
    /// </summary>
    public partial class JhMessageBoxWindow : Window
    {
        #region Constructor
        /// <summary>
        /// Create a new JhMessageBoxWindow, with the given options if non-null - otherwise use the DefaultOptions.
        /// </summary>
        /// <param name="options">The options to use for this particular message-box invocation. Leave this null to use the DefaultOptions.</param>
        public JhMessageBoxWindow(JhMessageBox mgr, JhMessageBoxOptions options = null)
        {
            //Console.WriteLine("JhMessageBoxWindow ctor, with options.BackgroundTexture = " + options.BackgroundTexture.ToString());
            _manager = mgr;
            // _options has to be set before InitializeComponent, since that causes properties to be bound, and hence the DataContext creates it's view-model.
            if (options == null)
            {
                _options = mgr.DefaultOptions;
            }
            else
            {
                _options = options;
            }
            _viewModel = JhMessageBoxViewModel.GetInstance(mgr, _options);

            InitializeComponent();

            // Ensure the timeout value isn't ridiculously high (as when the caller mistakenly thinks it's in milliseconds).
            // Use the constant upper-limit value to test it against.
            if (_options.TimeoutPeriodInSeconds > JhMessageBoxOptions.MaximumTimeoutPeriodInSeconds)
            {
                _options.TimeoutPeriodInSeconds = JhMessageBoxOptions.GetDefaultTimeoutValueFor(_options.MessageType);
            }

            if (_options.IsToCenterOverParent)
            {
                this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            }

            // If the option to use custom styles for the buttons is turned off, then set these button styles to null
            // so that they do not inherit the styles of the parent application.
            if (!_options.IsCustomButtonStyles)
            {
                btnCancel.Style = btnClose.Style = btnNo.Style = btnOk.Style = btnRetry.Style = btnYes.Style = btnIgnore.Style = null;
            }

#if SILVERLIGHT
            _viewModel.CopyCommand = new RelayCommand(
                () => OnCopyCommandExecuted()
            );
            _viewModel.StayCommand = new RelayCommand(
                () => OnStayCommandExecuted(),
                () => StayCommand_CanExecute()
            );
#else
            _viewModel.CopyCommand = new RelayCommand(
                (x) => OnCopyCommandExecuted()
            );
            _viewModel.StayCommand = new RelayCommand(
                (x) => OnStayCommandExecuted(),
                (x) => StayCommand_CanExecute()
            );
#endif

            Loaded += OnLoaded;
#if SILVERLIGHT
            Closing += new EventHandler<System.ComponentModel.CancelEventArgs>(OnClosing);
#else
            ContentRendered += OnContentRendered;
            Closing += new System.ComponentModel.CancelEventHandler(OnClosing);
#endif
        }
        #endregion Constructor

        #region Properties

        #region The Buttons
        /// <summary>
        /// The Button that by default says "Close"
        /// These are provided so that you can access them to set their properties according to your needs.
        /// </summary>
        public Button TheCloseButton
        {
            get { return btnClose; }
        }

        /// <summary>
        /// The Button that by default says "Cancel"
        /// These are provided so that you can access them to set their properties according to your needs.
        /// </summary>
        public Button TheCancelButton
        {
            get { return btnCancel; }
        }

        /// <summary>
        /// The Button that by default says "Retry"
        /// These are provided so that you can access them to set their properties according to your needs.
        /// </summary>
        public Button TheRetryButton
        {
            get { return btnRetry; }
        }

        /// <summary>
        /// The Button that by default says "No"
        /// These are provided so that you can access them to set their properties according to your needs.
        /// </summary>
        public Button TheNoButton
        {
            get { return btnNo; }
        }

        /// <summary>
        /// The Button that by default says "Yes"
        /// These are provided so that you can access them to set their properties according to your needs.
        /// </summary>
        public Button TheYesButton
        {
            get { return btnYes; }
        }

        /// <summary>
        /// The Button that by default says "Ignore"
        /// These are provided so that you can access them to set their properties according to your needs.
        /// </summary>
        public Button TheIgnoreButton
        {
            get { return btnIgnore; }
        }

        /// <summary>
        /// The Button that by default says "Ok"
        /// These are provided so that you can access them to set their properties according to your needs.
        /// </summary>
        public Button TheOkButton
        {
            get { return btnOk; }
        }

        #endregion

        #region SummaryText
        /// <summary>
        /// Get or set the text that's shown in the upper, summary-text area.
        /// </summary>
        public string SummaryText
        {
            get { return _viewModel.SummaryText; }
            set { _viewModel.SummaryText = value; }
        }
        #endregion

        #region DetailText
        /// <summary>
        /// Get or set the "Detail Text" to be shown to the user, which is distinct from the "Summary Text". See documentation for explanation.
        /// </summary>
        public string DetailText
        {
            get { return _viewModel.DetailText; }
            set { _viewModel.DetailText = value; }
        }
        #endregion

        #region Result
        /// <summary>
        /// Get or set the result that this message-box wants to return to the caller.
        /// </summary>
        public JhDialogResult Result
        {
            get { return _result; }
            set { _result = value; }
        }
        #endregion

        #region MessageBeep
        /// <summary>
        /// Produce an audial beep sound, which corresponds to the default system sound that (more or less)
        /// corresponds to that of the given message-box type.
        /// </summary>
        /// <param name="messageType">A JhMessageBoxType which influences the type of beep sound that is made</param>
        public static void MessageBeep(JhMessageBoxType messageType)
        {
            // Try to map the messageType to a value that corresponds to what the original Win32 icon would be..
            string soundEventName;
            switch (messageType)
            {
                case JhMessageBoxType.Information:
                    soundEventName = "SystemAsterisk";
                    break;
                case JhMessageBoxType.Question:
                    soundEventName = "SystemQuestion";
                    break;
                case JhMessageBoxType.Warning:
                    soundEventName = "SystemExclamation";
                    break;
                case JhMessageBoxType.Error:
                case JhMessageBoxType.UserMistake:
                    soundEventName = "AppGPFault";
                    break;
                case JhMessageBoxType.SecurityIssue:
                    soundEventName = "WindowsUAC";
                    break;
                case JhMessageBoxType.SecuritySuccess:
                    soundEventName = "SystemNotification";
                    break;
                case JhMessageBoxType.Stop:
                    soundEventName = "SystemHand"; // Critical Stop
                    break;
                default:
                    soundEventName = "Default Beep";
                    break;
            }
            //Console.WriteLine("soundEventName is " + soundEventName);
            AudioLib.PlaySoundEvent(soundEventName);
        }
        #endregion

        #endregion Properties

        #region unit-testing facilities

        #region IsTesting
        /// <summary>
        /// Get whether message-boxes are being used in automated-test mode.
        /// This provides a way to check IsTesting without instanciating a JhMessageBoxTestFacility object.
        /// </summary>
        public bool IsTesting
        {
            get
            {
                if (_manager != null)
                {
                    return _manager.IsTesting;
                }
                return false;
            }
        }
        #endregion

        #region TestFacility
        /// <summary>
        /// Get the message-box test-options that apply when running automated tests.
        /// </summary>
        public JhMessageBoxTestFacility TestFacility
        {
            get
            {
                return _manager.TestFacility;
            }
        }
        #endregion

        #region TheButtons
        /// <summary>
        /// Get the list of all the buttons within this message-box. Used only during testing.
        /// </summary>
        public IList<Button> TheButtons
        {
            get
            {
                if (_theButtons == null)
                {
                    _theButtons = new List<Button>(7);
                    // Add these in the order of the frequency with which they'll most likely occurr within the software.
                    _theButtons.Add(btnOk);
                    _theButtons.Add(btnClose);
                    _theButtons.Add(btnYes);
                    _theButtons.Add(btnNo);
                    _theButtons.Add(btnCancel);
                    _theButtons.Add(btnRetry);
                    _theButtons.Add(btnIgnore);
                }
                return _theButtons;
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

        #region internal implementation

        #region event-handlers

        #region OnLoaded
        /// <summary>
        /// Handle the 'Loaded' Window-event.
        /// </summary>
        void OnLoaded(object sender, RoutedEventArgs e)
        {
            // If we are in auto-test mode, then add this message-box to the list of currently-showing message-boxes.
            // This is done here, after the content is rendered, because it doesn't actually count unless
            // this message-box has succeeded to that point at which the user can actually see it.
            if (IsTesting)
            {
                _manager.TestFacility.InstancesCurrentlyDisplaying.Add(this);
                // If we are testing and running this asynchronously, then the NotifyUser..Async method-call
                // is waiting for this ManualResetEvent to be set before it returns to its caller.
                if (_options.IsAsynchronous)
                {
                    _manager.TestFacility.ResetEventForWindowShowing.Set();
                }
            }
#if SILVERLIGHT
            OnContentRendered(null, null);
#endif
        }
        #endregion OnLoaded

        #region OnContentRendered
        /// <summary>
        /// Handle the ContentRendered event, which occurrs after the UI elements have been rendered on-screen.
        /// </summary>
        void OnContentRendered(object sender, EventArgs e)
        {
            #region Make me the top-most window.

            if (!this.IsTesting)
            {
                if (_options.IsToBeTopmostWindow)
                {
#if! SILVERLIGHT
                    this.Topmost = true;
                    this.Focus();
#endif
                }
            }
            #endregion

            #region Audial effects

            if (!this.IsTesting && _options.IsSoundEnabled)
            {
                if (!_options.IsUsingNewerSoundScheme)
                {
                    JhMessageBoxWindow.MessageBeep(_options.MessageType);
                }
                else
                {
                    //TODO: How to implement this in SL?
#if !SILVERLIGHT
                    switch (_options.MessageType)
                    {
                        case JhMessageBoxType.Information:
                            AudioLib.PlaySoundResourceEvenIfNotPCM(_informationSoundResource);
                            break;
                        case JhMessageBoxType.Question:
                            AudioLib.PlaySoundResourceEvenIfNotPCM(_questionSoundResource);
                            break;
                        case JhMessageBoxType.UserMistake:
                            AudioLib.PlaySoundResource(_userMistakeSoundResource);
                            break;
                        case JhMessageBoxType.Warning:
                            AudioLib.PlaySoundResource(_warningSoundResource);
                            break;
                        case JhMessageBoxType.Error:
                            AudioLib.PlaySoundResource(_errorSoundResource);
                            break;
                        case JhMessageBoxType.SecurityIssue:
                            AudioLib.PlaySoundResource(_securityIssueSoundResource);
                            break;
                        case JhMessageBoxType.Stop:
                            AudioLib.PlaySoundResource(_stopSoundResource);
                            break;
                    }
#endif
                }
            }
            #endregion Audial effects

            #region Timeout

            if (!IsTesting || (IsTesting && !TestFacility.IsToWaitForExplicitClose))
            {
                // Set the timer that will close this message-box.
                int timeoutPeriodToSet = _options.TimeoutPeriodInSeconds;
                if (timeoutPeriodToSet == 0)
                {
                    // If the default value of zero was indicated, that means to use the default value.
                    timeoutPeriodToSet = JhMessageBoxOptions.GetDefaultTimeoutValueFor(_options.MessageType);
                    _options.TimeoutPeriodInSeconds = timeoutPeriodToSet;
                }
                // If we are running tests and this option is set -- limit the timeout to our special, brief time-period.
                if (_manager._testFacility != null && TestFacility.IsTimeoutsFast)
                {
                    if (timeoutPeriodToSet > 1)
                    {
                        timeoutPeriodToSet = 1;
                    }
                }
                StartTimer(timeoutPeriodToSet);
            }

            #endregion Timeout
        }
        #endregion

        #region btnCancel_Click
        /// <summary>
        /// Handle the Cancel button click.
        /// </summary>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Result = JhDialogResult.Cancel;
            Close();
        }
        #endregion

        #region btnClose_Click
        /// <summary>
        /// Handle the "Close" button click.
        /// </summary>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Result = JhDialogResult.Close;
            Close();
        }
        #endregion

        #region btnIgnore_Click
        /// <summary>
        /// Handle the Ignore button click.
        /// </summary>
        private void btnIgnore_Click(object sender, RoutedEventArgs e)
        {
            Result = JhDialogResult.Ignore;
            DialogResult = true;
        }
        #endregion

        #region btnOK_Click
        /// <summary>
        /// Handle the OK button click.
        /// </summary>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Result = JhDialogResult.Ok;
            // Cannot set DialogResult if this was invoked via Show, as opposed to ShowDialog.
            if (this._options.IsAsynchronous)
            {
                this.Close();
            }
            else
            {
                DialogResult = true;
            }
        }
        #endregion

        #region btnRetry_Click
        /// <summary>
        /// Handle the "Retry" button click.
        /// </summary>
        private void btnRetry_Click(object sender, RoutedEventArgs e)
        {
            Result = JhDialogResult.Retry;
            DialogResult = true;
            Close();
        }
        #endregion

        #region btnYes_Click
        /// <summary>
        /// Handle the "Yes" button click.
        /// </summary>
        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            Result = JhDialogResult.Yes;
            DialogResult = true;
            Close();
        }
        #endregion

        #region btnNo_Click
        /// <summary>
        /// Handle the "No" button click.
        /// </summary>
        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            Result = JhDialogResult.No;
            DialogResult = true;
            Close();
        }
        #endregion

        #region OnTimerTick
        /// <summary>
        /// This is called after the close-myowndamself timer expires.
        /// </summary>
        /// <param name="sender">The timer, which should be _closeMyselfTimer</param>
        private void OnTimerTick(object sender, EventArgs e)
        {
            // If the mouse-cursor has moved within this Window within the last 10 seconds, give it another 10-second bump in time before dismissing it.
            if (_userIsMessingWithThis && !IsTesting)
            {
                DateTime now = DateTime.Now;
                TimeSpan t = new TimeSpan(now.Ticks - _whenLastMoved.Ticks);
                if (t.Seconds < 10)
                {
                    StopTimer();
                    StartTimer(10);
                    return;
                }
            }

            // Get rid of the timer, if that's running.
            StopTimer();

            this.Result = JhDialogResult.TimedOut;

            // If this is in automated-test mode and an emulated button-press was specified,
            // then force that to be the result..
            if (IsTesting)
            {
                // but don't do it if a non-existent button was specified.
                if (TestFacility.ButtonResultToSelect.HasValue)
                {
                    Result = TestFacility.ButtonResultToSelect.Value;
                }
                else
                {
                    if (TestFacility.TextOfButtonToSelect != null)
                    {
                        string textPattern = TestFacility.TextOfButtonToSelect;
                        if (textPattern.Equals(TextOfCancelButton))
                        {
                            Result = JhDialogResult.Cancel;
                        }
                        else if (textPattern.Equals(TextOfCloseButton))
                        {
                            Result = JhDialogResult.Close;
                        }
                        else if (textPattern.Equals(TextOfOkButton))
                        {
                            Result = JhDialogResult.Ok;
                        }
                        else if (textPattern.Equals(TextOfYesButton))
                        {
                            Result = JhDialogResult.Yes;
                        }
                        else if (textPattern.Equals(TextOfNoButton))
                        {
                            Result = JhDialogResult.No;
                        }
                        else if (textPattern.Equals(TextOfRetryButton))
                        {
                            Result = JhDialogResult.Retry;
                        }
                        else if (textPattern.Equals(TextOfIgnoreButton))
                        {
                            Result = JhDialogResult.Ignore;
                        }
                        else if (textPattern.Equals(TextOfAbortButton))
                        {
                            Result = JhDialogResult.Abort;
                        }
                    }
                }
            }
            Close();
        }
        #endregion

        #region Mouse-movement events
        /// <summary>
        /// Use the MouseEnter and MouseLeave events to remember when the user has moved his mouse cursor within the messagebox window,
        /// thus indicating that he/she may be doing something and perhaps we ought not to close it on 'em.
        /// </summary>
        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            _userIsMessingWithThis = true;
            _whenLastMoved = DateTime.Now;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            _userIsMessingWithThis = false;
            _whenLastMoved = DateTime.Now;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            _userIsMessingWithThis = true;
            _whenLastMoved = DateTime.Now;
        }
        #endregion Mouse-movement events

        #region OnClosing
        /// <summary>
        /// Handle the Closing Window-event, by clearing the timer.
        /// </summary>
        void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopTimer();
            // This message-box is going away, so remove it from the list of message-boxes currently being shown (if in test-mode).
            if (IsTesting)
            {
                _manager.TestFacility.InstancesThatHaveDisplayed.Add(this);
                _manager.TestFacility.InstancesCurrentlyDisplaying.Remove(this);
            }
            // If running asynchronously, raise the Completed event.
            if (_options.IsAsynchronous)
            {
                _manager.SignalThatMessageBoxHasEnded(this, this.Result);
            }
        }
        #endregion

        #endregion event-handlers

        #region Commanding

        #region Copy Command
        /// <summary>
        /// Handle the Copy-To-Clipboard command.
        /// </summary>
        private void OnCopyCommandExecuted()
        {
            string caption = this.Title;
            string textOfSummary = this.SummaryText;
            string mainText = this.DetailText;
            string totalText = Environment.NewLine + Environment.NewLine + "MessageBox Title: "
                + caption + Environment.NewLine
                + textOfSummary + Environment.NewLine
                + mainText + Environment.NewLine + Environment.NewLine;
            try
            {
                Clipboard.SetText(totalText);
            }
            catch (SecurityException)
            {
                System.Diagnostics.Debug.WriteLine("SecurityException prevents copy to Clipboard");
            }
        }
        #endregion

        #region Stay Command

        private void OnStayCommandExecuted()
        {
            StopTimer();
#if SILVERLIGHT
            tooltipStay.Content = "This is disabled because the timer has already been stayed.";
#else
            miStay.ToolTip = "This is disabled because the timer has already been stayed.";
#endif
        }

        /// <summary>
        /// Indicate whether the Stay command can execute.
        /// </summary>
        private bool StayCommand_CanExecute()
        {
            return _closeMyselfTimer != null;
        }
        #endregion

        #endregion Commanding

        #region StartTimer, StopTimer
        /// <summary>
        /// Start the DispatchTimer that will, if the user fails to close this JhMessageBoxWindow within the timeout period,
        /// automatically close this and return the default result.
        /// </summary>
        private void StartTimer(int seconds)
        {
            var timeout = TimeSpan.FromSeconds(seconds);

#if SILVERLIGHT
            _closeMyselfTimer = new DispatcherTimer();
            _closeMyselfTimer.Interval = timeout;
            _closeMyselfTimer.Tick += new EventHandler(OnTimerTick);
            _closeMyselfTimer.Start();
#else
            _closeMyselfTimer = new DispatcherTimer(timeout,
                                                    DispatcherPriority.ApplicationIdle,
                                                    new EventHandler(OnTimerTick),
                                                    this.Dispatcher);
#endif
        }

        /// <summary>
        /// If the DispatchTimer is running, stop it, and dispose of it.
        /// </summary>
        private void StopTimer()
        {
            // Get rid of the timer, if that's running.
            if (_closeMyselfTimer != null)
            {
                if (_closeMyselfTimer.IsEnabled)
                {
                    _closeMyselfTimer.Stop();
                }
                _closeMyselfTimer.Tick -= OnTimerTick;
                _closeMyselfTimer = null;
            }
        }
        #endregion StartTimer, StopTimer

#if !SILVERLIGHT
        #region the Aero-glass effect.

        //TODO: The aero-glass effect, when shown without the error gradients, looks like shit.
        // I need to either fix it, or eliminate this feature.
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (_options.IsUsingAeroGlassEffect)
            {
                // This can't be done any earlier than the SourceInitialized event.
                GlassHelper.ExtendGlassFrame(this, new Thickness(-1));

                // Attach a window procedure in order to detect later enabling of desktop composition.
                IntPtr hwnd = new WindowInteropHelper(this).Handle;
                HwndSource.FromHwnd(hwnd).AddHook(new HwndSourceHook(WndProc));
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_DWMCOMPOSITIONCHANGED)
            {
                // Reenable glass.
                GlassHelper.ExtendGlassFrame(this, new Thickness(-1));
                handled = true;
            }
            return IntPtr.Zero;
        }

        private const int WM_DWMCOMPOSITIONCHANGED = 0x031E;

        #endregion
#endif

        #region fields

        /// <summary>
        /// the JhMessageBoxOptions to use for this specific invocation of this message-box.
        /// </summary>
        internal JhMessageBoxOptions _options;
        /// <summary>
        /// This is the DispatcherTimer which provides the close-upon-timeout functionality.
        /// </summary>
        private DispatcherTimer _closeMyselfTimer;
        private const JhDialogResult _defaultResult = JhDialogResult.TimedOut;
        private JhMessageBox _manager;
        private JhDialogResult _result = _defaultResult;
        // These following are sound files to play for these given types of message-box.
        //TODO: How to implement these in SL?
#if !SILVERLIGHT
        /// <summary>
        /// This is the audio file Sounds/information.wav
        /// </summary>
        private static readonly UnmanagedMemoryStream _informationSoundResource = global::JhLib.Properties.Resources.Information;
        /// <summary>
        /// This is the audio file Sounds/DrumHit1.wav
        /// </summary>
        private static readonly UnmanagedMemoryStream _warningSoundResource = global::JhLib.Properties.Resources.DrumHit1;
        /// <summary>
        /// This is the audio file Sounds/Incorrect.wav
        /// </summary>
        private static readonly UnmanagedMemoryStream _userMistakeSoundResource = global::JhLib.Properties.Resources.Incorrect;
        /// <summary>
        /// This is the audio file Sounds/DrumHit2.wav
        /// </summary>
        private static readonly UnmanagedMemoryStream _errorSoundResource = global::JhLib.Properties.Resources.DrumHit2;
        /// <summary>
        /// This is the audio file Sounds/Question.wav
        /// </summary>
        private static readonly UnmanagedMemoryStream _questionSoundResource = global::JhLib.Properties.Resources.Question;
        /// <summary>
        /// This is the audio file Sounds/SecurityIssue.wav
        /// </summary>
        private static readonly UnmanagedMemoryStream _securityIssueSoundResource = global::JhLib.Properties.Resources.SecurityIssue;
        /// <summary>
        /// This is the audio file Sounds/Stop.wav
        /// </summary>
        private static readonly UnmanagedMemoryStream _stopSoundResource = global::JhLib.Properties.Resources.Stop;
#endif
        /// <summary>
        /// This has the list of all the buttons being shown within this message-box. Used only during testing.
        /// </summary>
        private List<Button> _theButtons;
        /// <summary>
        /// This flag is used to indicate when the user is interacting with this messagebox, thus we should not allow it to timeout and close on him.
        /// </summary>
        private bool _userIsMessingWithThis;
        /// <summary>
        /// The view-model for the message-box view.
        /// </summary>
        private JhMessageBoxViewModel _viewModel;
        /// <summary>
        /// The moment at which the user last moved the mouse-cursor within the message-box window.
        /// This is used to prevent the message-box from timing-out while the user is trying to copy-paste text from it.
        /// </summary>
        private DateTime _whenLastMoved = DateTime.MinValue;

        #endregion fields

        #endregion internal implementation
    }
    #endregion class JhMessageBoxWindow
}

