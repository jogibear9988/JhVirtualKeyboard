using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace JhLib
{
    /// <summary>
    /// This class contains all JhMessageBox options that apply specifically to running automated tests.
    /// </summary>
    public class JhMessageBoxTestFacility : IDisposable
    {
        #region Constructor
        /// <summary>
        /// Make a new JhMessageBoxTestFacility object with the default values.
        /// </summary>
        public JhMessageBoxTestFacility(JhMessageBox manager)
        {
            _messageBoxManager = manager;
            _isTesting = true;
            _isTimeoutsFast = true;
        }
        #endregion

        // The followiing functions are intended for use within Assert-type checks. If you add more, you should consider
        // starting with the prefix "Test", or "SetTest", so that Intellisense will group them together.

        #region AssertAMessageBoxIsBeingShown
        /// <summary>
        /// Throw an exception if there is not at least one JhMessageBox currently being displayed to the user.
        /// </summary>
        /// <returns>a reference to this same JhMessageBoxTestFacility object, so that you can chain together method-calls if you wish</returns>
        public JhMessageBoxTestFacility AssertAMessageBoxIsBeingShown()
        {
            if (_messageBoxManager.MessageBoxWindow._options.IsAsynchronous)
            {
                bool didNotTimeOut = ResetEventForWindowShowing.Wait(10000);
                if (!didNotTimeOut)
                {
                    Console.WriteLine("AssertAMessageBoxIsBeingShown: timed out waiting for thread synchronization.");
                }
            }
            if (InstancesCurrentlyDisplaying.Count == 0)
            {
                throw new AssertFailedException("No JhMessageBox is being displayed at this moment.");
            }
            return this;
        }
        #endregion

        #region AssertAMessageBoxHasBeenShown
        /// <summary>
        /// Throw an exception if there is not at least one JhMessageBox currently being, or which has been, displayed to the user.
        /// </summary>
        /// <returns>a reference to this same JhMessageBoxTestFacility object, so that you can chain together method-calls if you wish</returns>
        public JhMessageBoxTestFacility AssertAMessageBoxHasBeenShown()
        {
            if (InstancesCurrentlyDisplaying.Count == 0 && InstancesThatHaveDisplayed.Count == 0)
            {
                throw new AssertFailedException("No JhMessageBox is or was displayed at this moment.");
            }
            return this;
        }
        #endregion

        #region AssertIsWithinTitle
        /// <summary>
        /// Throw an exception if the given textPattern string is not present within the title-bar text of a message-box
        /// that is currently being displayed.
        /// The test is case-sensitive.
        /// </summary>
        /// <param name="textPattern">the pattern to check for</param>
        /// <returns>a reference to this same JhMessageBoxTestFacility object, so that you can chain together method-calls if you wish</returns>
        public JhMessageBoxTestFacility AssertIsWithinTitle(string textPattern)
        {
            int nCount = 0;
            foreach (var messageBox in this.InstancesCurrentlyDisplaying)
            {
                nCount++;
                string titleText = messageBox.Title;
                if (titleText.Contains(textPattern))
                {
                    return this;
                }
            }
            if (nCount == 0)
            {
                throw new AssertFailedException("No JhMessageBox is being displayed at this moment.");
            }
            else if (nCount == 1)
            {
                throw new AssertFailedException(String.Format("The JhMessageBox that is being displayed does not contain \"{0}\" within its title-text.", textPattern));
            }
            else
            {
                throw new AssertFailedException(String.Format("None of the {0} JhMessageBoxes being displayed contain \"{1}\" within its title-text.", nCount, textPattern));
            }
        }
        #endregion

        #region AssertIsWithinSummaryText
        /// <summary>
        /// Throw an exception if the give textPattern string is not present within the summary-text of a message-box
        /// that is currently being displayed.
        /// The test is case-sensitive.
        /// </summary>
        /// <param name="textPattern">the pattern to check for</param>
        /// <returns>a reference to this same JhMessageBoxTestFacility object, so that you can chain together method-calls if you wish</returns>
        public JhMessageBoxTestFacility AssertIsWithinSummaryText(string textPattern)
        {
            int nCount = 0;
            foreach (var messageBox in this.InstancesCurrentlyDisplaying)
            {
                nCount++;
                string summaryText = messageBox.SummaryText;
                if (summaryText.Contains(textPattern))
                {
                    return this;
                }
            }
            if (nCount == 0)
            {
                throw new AssertFailedException("No JhMessageBox is being displayed at this moment.");
            }
            else if (nCount == 1)
            {
                throw new AssertFailedException(String.Format("The JhMessageBox does not contain \"{0}\" within its summary-text.", textPattern));
            }
            else
            {
                throw new AssertFailedException(String.Format("None of the {0} JhMessageBoxes contains \"{1}\" within its summary-text.", nCount, textPattern));
            }
        }
        #endregion

        #region AssertWasWithinSummaryText
        /// <summary>
        /// Throw an exception if the give text was not present within the summary-text of any message-box that has been displayed.
        /// The test is case-sensitive.
        /// </summary>
        /// <param name="textPattern">the pattern to check for</param>
        /// <returns>a reference to this same JhMessageBoxTestFacility object, so that you can chain together method-calls if you wish</returns>
        public JhMessageBoxTestFacility AssertWasWithinSummaryText(string textPattern)
        {
            int nCount = 0;
            foreach (var messageBox in this.InstancesThatHaveDisplayed)
            {
                nCount++;
                string summaryText = messageBox.SummaryText;
                if (summaryText.Contains(textPattern))
                {
                    return this;
                }
            }
            if (nCount == 0)
            {
                throw new AssertFailedException("No JhMessageBox has been displayed.");
            }
            else if (nCount == 1)
            {
                throw new AssertFailedException(String.Format("The JhMessageBox did not contain \"{0}\" within its summary-text.", textPattern));
            }
            else
            {
                throw new AssertFailedException(String.Format("None of the {0} JhMessageBoxes contained \"{1}\" within its summary-text.", nCount, textPattern));
            }
        }
        #endregion

        #region AssertIsWithinDetailText
        /// <summary>
        /// Throw an exception if the give textPattern string is not present within the detail-text of a message-box
        /// that is currently being displayed.
        /// The test is case-sensitive.
        /// </summary>
        /// <param name="textPattern">the pattern to check for</param>
        /// <returns>a reference to this same JhMessageBoxTestFacility object, so that you can chain together method-calls if you wish</returns>
        public JhMessageBoxTestFacility AssertIsWithinDetailText(string textPattern)
        {
            int nCount = 0;
            foreach (var messageBox in this.InstancesCurrentlyDisplaying)
            {
                nCount++;
                string detailText = messageBox.DetailText;
                if (detailText.Contains(textPattern))
                {
                    return this;
                }
            }
            if (nCount == 0)
            {
                throw new AssertFailedException("No JhMessageBox is being displayed at this moment.");
            }
            else if (nCount == 1)
            {
                throw new AssertFailedException(String.Format("The JhMessageBox does not contain \"{0}\" within its detail-text.", textPattern));
            }
            else
            {
                throw new AssertFailedException(String.Format("None of the {0} JhMessageBoxes contains \"{1}\" within its detail-text.", nCount, textPattern));
            }
        }
        #endregion

        #region AssertWasWithinDetailText
        /// <summary>
        /// Throw an exception if the give textPattern string was not present within the detail-text of a message-box
        /// that has been displayed.
        /// The test is case-sensitive.
        /// </summary>
        /// <param name="textPattern">the pattern to check for</param>
        /// <returns>a reference to this same JhMessageBoxTestFacility object, so that you can chain together method-calls if you wish</returns>
        public JhMessageBoxTestFacility AssertWasWithinDetailText(string textPattern)
        {
            int nCount = 0;
            foreach (var messageBox in this.InstancesThatHaveDisplayed)
            {
                nCount++;
                string detailText = messageBox.DetailText;
                if (detailText.Contains(textPattern))
                {
                    return this;
                }
            }
            if (nCount == 0)
            {
                throw new AssertFailedException("No JhMessageBox is has been displayed yet.");
            }
            else if (nCount == 1)
            {
                throw new AssertFailedException(String.Format("The JhMessageBox which was displayed did not contain \"{0}\" within its detail-text.", textPattern));
            }
            else
            {
                throw new AssertFailedException(String.Format("None of the {0} JhMessageBoxes that have been displayed contained \"{1}\" within the detail-text.", nCount, textPattern));
            }
        }
        #endregion

        // Following are the tests that detect the presence of any of the 6 distinct buttons that may be present.
        // These are suitable for verifying program-operation via automated unit-tests, as they use a different, direct
        // mechanism to detect whether the respective buttons are being displayed, as opposed to that mechanism by which
        // they are controlled at the API level.

        #region AssertButtonIsPresent
        /// <summary>
        /// Throw an exception if there is not a message-box being shown to the user that has a button that contains the given text.
        /// The test is case-insensitive.
        /// </summary>
        /// <param name="withWhatText">the text-pattern to look for</param>
        /// <returns>true if there is at least one message-box currently displaying that contains it</returns>
        public JhMessageBoxTestFacility AssertButtonIsPresent(string withWhatText)
        {
            string patternAsLowercase = withWhatText.ToLower();
            int nCount = 0;
            foreach (var messageBox in this.InstancesCurrentlyDisplaying)
            {
                nCount++;
                foreach (var button in messageBox.TheButtons)
                {
                    if (button.Visibility == Visibility.Visible)
                    {
                        //TODO: Alert - this will only work if all the buttons have Content that can be cast into a string.
                        // This is not an assumption that is always true.
                        string buttonText = button.Content as string;
                        if (buttonText != null)
                        {
                            string buttonTextAsLowercase = buttonText.ToLower();
                            if (buttonTextAsLowercase.Contains(patternAsLowercase))
                            {
                                return this;
                            }
                        }
                    }
                }
            }
            if (nCount == 0)
            {
                throw new AssertFailedException(String.Format("in AssertButtonIsPresent({0}) - No JhMessageBox is being displayed at this moment.", withWhatText));
            }
            else if (nCount == 1)
            {
                throw new AssertFailedException(String.Format("in AssertButtonIsPresent({0}) - The JhMessageBox does not contain that within its buttons.", withWhatText));
            }
            else
            {
                throw new AssertFailedException(String.Format("in AssertButtonIsPresent({0}) - None of the {1} JhMessageBoxes contains that within its buttons.", withWhatText, nCount));
            }
        }
        #endregion

        #region AssertOkButtonIsPresent
        /// <summary>
        /// Throw an exception if there is not a JhMessageBox being displayed that has the "OK" button showing.
        /// If the text of the "Ok" button has been changed, this will fail.
        /// </summary>
        public JhMessageBoxTestFacility AssertOkButtonIsPresent()
        {
            AssertButtonIsPresent(JhMessageBoxWindow.TextOfOkButton);
            return this;
        }
        #endregion

        #region AssertYesButtonIsPresent
        /// <summary>
        /// Throw an exception if there is not a JhMessageBox being displayed that has the "Yes" button showing.
        /// If the text of the "Yes" button has been changed, this will fail.
        /// </summary>
        public JhMessageBoxTestFacility AssertYesButtonIsPresent()
        {
            AssertButtonIsPresent(JhMessageBoxWindow.TextOfYesButton);
            return this;
        }
        #endregion

        #region AssertNoButtonIsPresent
        /// <summary>
        /// Throw an exception if there is not a JhMessageBox being displayed that has the "No" button showing.
        /// If the text of the "No" button has been changed, this will fail.
        /// </summary>
        public JhMessageBoxTestFacility AssertNoButtonIsPresent()
        {
            AssertButtonIsPresent(JhMessageBoxWindow.TextOfNoButton);
            return this;
        }
        #endregion

        #region AssertCancelButtonIsPresent
        /// <summary>
        /// Throw an exception if there is not a JhMessageBox being displayed that has the "Cancel" button showing.
        /// If the text of the "Cancel" button has been changed, this will fail.
        /// </summary>
        public JhMessageBoxTestFacility AssertCancelButtonIsPresent()
        {
            AssertButtonIsPresent(JhMessageBoxWindow.TextOfCancelButton);
            return this;
        }
        #endregion

        #region AssertRetryButtonIsPresent
        /// <summary>
        /// Throw an exception if there is not a JhMessageBox being displayed that has the "Retry" button showing.
        /// If the text of the "Retry" button has been changed, this will fail.
        /// </summary>
        public JhMessageBoxTestFacility AssertRetryButtonIsPresent()
        {
            AssertButtonIsPresent(JhMessageBoxWindow.TextOfRetryButton);
            return this;
        }
        #endregion

        #region AssertCloseButtonIsPresent
        /// <summary>
        /// Throw an exception if there is not a JhMessageBox being displayed that has the "Close" button showing.
        /// If the text of the "Close" button has been changed, this will fail.
        /// </summary>
        public JhMessageBoxTestFacility AssertCloseButtonIsPresent()
        {
            AssertButtonIsPresent(JhMessageBoxWindow.TextOfCloseButton);
            return this;
        }
        #endregion

        #region ButtonResultToSelect
        /// <summary>
        /// Get the JhDialogResult that we want to emulate the user having selected. If none was specified - return null.
        /// </summary>
        public JhDialogResult? ButtonResultToSelect
        {
            get { return _buttonResultToSelect; }
        }
        #endregion

        #region InstancesCurrentlyDisplaying
        /// <summary>
        /// This is a collection of all of the message-boxes that are currently showing. This is intended only for running unit-tests.
        /// </summary>
        public IList<JhMessageBoxWindow> InstancesCurrentlyDisplaying
        {
            get
            {
                if (_instancesCurrentlyBeingDisplayed == null)
                {
                    _instancesCurrentlyBeingDisplayed = new List<JhMessageBoxWindow>();
                }
                return _instancesCurrentlyBeingDisplayed;
            }
        }
        #endregion

        #region InstancesThatHaveDisplayed
        /// <summary>
        /// This is a collection of all of the message-boxes that have been shown since the start of this current test-run.
        /// This is intended only for running unit-tests.
        /// </summary>
        public IList<JhMessageBoxWindow> InstancesThatHaveDisplayed
        {
            get
            {
                if (_instancesThatHaveDisplayed == null)
                {
                    _instancesThatHaveDisplayed = new List<JhMessageBoxWindow>();
                }
                return _instancesThatHaveDisplayed;
            }
        }
        #endregion

        #region IsTesting
        /// <summary>
        /// Get whether message-boxes are being used in automated-test mode.
        /// </summary>
        public bool IsTesting
        {
            get { return _isTesting; }
        }
        #endregion

        #region IsTimeoutsFast
        /// <summary>
        /// Get whether message-boxes are being used in automated-test mode and the timeouts are being limited to a brief interval.
        /// </summary>
        public bool IsTimeoutsFast
        {
            get { return _isTesting && _isTimeoutsFast; }
        }
        #endregion

        #region IsToWaitForExplicitClose
        /// <summary>
        /// Get or set whether, when in test-mode, instead of timing out - the message-box is to remain open
        /// until the method SimulateClose is called. This applies only in the case of asynchronous NotifyUser.. methods.
        /// Default is false.
        /// </summary>
        public bool IsToWaitForExplicitClose
        {
            get { return _isToWaitForExplicitClose; }
            set { _isToWaitForExplicitClose = value; }
        }

        /// <summary>
        /// Set whether instead of timing out - the message-box is to remain open until the method SimulateClose is called.
        /// This applies only in the case of asynchronous NotifyUser.. methods.  Default is false. 
        /// </summary>
        /// <param name="isToWait">true to wait until an explicit command is issued to close the message-box</param>
        /// <returns>this instance of JhMessageBoxOptions, such that additional method-calls may be chained together</returns>
        public JhMessageBoxTestFacility SetToWaitForExplicitClose(bool isToWait = true)
        {
            IsToWaitForExplicitClose = isToWait;
            return this;
        }
        #endregion

        #region Reset
        /// <summary>
        /// Clear the options back to their default values, and turns IsTesting and IsTimeoutsFast on.
        /// This is useful when setting up for automated unit-tests.
        /// </summary>
        /// <returns></returns>
        public JhMessageBoxTestFacility Reset()
        {
            _buttonResultToSelect = null;
            _buttonTextToSelect = null;
            _isTesting = _isTimeoutsFast = true;
            _instancesCurrentlyBeingDisplayed = null;
            _instancesThatHaveDisplayed = null;
            return this;
        }
        #endregion

        public ManualResetEventSlim ResetEventForWindowShowing
        {
            get
            {
                if (_resetEventForWindowShowing == null)
                {
                    _resetEventForWindowShowing = new ManualResetEventSlim();
                }
                return _resetEventForWindowShowing;
            }
        }

        #region SetButtonToSelect
        /// <summary>
        /// Set the message-box such that, when run as part of an automated test, we "mock" the user-interaction such that
        /// it acts as though the user selects the button that yields the given JhDialogResult.
        /// </summary>
        /// <param name="buttonResult">This determines which button to emulate being pushed.</param>
        /// <returns>this instance of JhMessageBoxOptions, such that additional method-calls may be chained together</returns>
        public JhMessageBoxTestFacility SetButtonToSelect(JhDialogResult buttonResult)
        {
            _buttonResultToSelect = buttonResult;
            return this;
        }
        #endregion

        #region SimulateClosing
        /// <summary>
        /// Cause the message-box window to immediately close with the given Result. Used for testing.
        /// </summary>
        /// <param name="withWhatResult">the JhDialogResult to assign to the Result property upon closing</param>
        public void SimulateClosing(JhDialogResult withWhatResult)
        {
            this._messageBoxManager.MessageBoxWindow.Result = withWhatResult;
            this._messageBoxManager.MessageBoxWindow.Close();
        }
        #endregion

        #region TextOfButtonToSelect
        /// <summary>
        /// Get the text of the button that we want to emulate the user having selected. If none was specified - return null.
        /// </summary>
        public string TextOfButtonToSelect
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_buttonTextToSelect))
                {
                    return null;
                }
                else
                {
                    return _buttonTextToSelect;
                }
            }
        }
        #endregion

        #region SetButtonTextToSelect
        /// <summary>
        /// Set the message-box such that, when run as part of an automated test, we "mock" the user-interaction such that
        /// it acts as though the user selects the button with the given text on it. The text is case-insensitive.
        /// </summary>
        /// <param name="buttonText">this determines which button to emulate being pushed, by virtue of the text on it</param>
        /// <returns>this instance of JhMessageBoxOptions, such that additional method-calls may be chained together</returns>
        public JhMessageBoxTestFacility SetButtonTextToSelect(string buttonText)
        {
            if (buttonText == null)
            {
                throw new ArgumentNullException("buttonText");
            }
            _buttonTextToSelect = buttonText.ToLower();
            return this;
        }
        #endregion

        #region SetTestTimeoutsToBeFast
        /// <summary>
        /// For running automated unit-tests, this limits all timeouts to one second - so that you can see it, but it will still
        /// run reasonably fast. This should ONLY be used for running tests, not while your end-user is using your product. Default is true.
        /// If you set this to true, it also sets IsTesting to true.
        /// </summary>
        /// <param name="isToGoAtFullSpeed">true to enable special auto-test mode (with short timeouts), false for normal operation</param>
        public JhMessageBoxTestFacility SetTestTimeoutsToBeFast(bool isToGoAtFullSpeed = true)
        {
            _isTimeoutsFast = isToGoAtFullSpeed;
            if (isToGoAtFullSpeed)
            {
                _isTesting = true;
            }
            return this;
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Release any test-specific resources and turn off test-mode.
        /// </summary>
        public void Dispose()
        {
            if (_instancesCurrentlyBeingDisplayed != null)
            {
                _instancesCurrentlyBeingDisplayed.Clear();
                _instancesCurrentlyBeingDisplayed = null;
            }
            if (_instancesThatHaveDisplayed != null)
            {
                _instancesThatHaveDisplayed.Clear();
                _instancesThatHaveDisplayed = null;
            }
            GC.SuppressFinalize(this);
        }
        #endregion

        #region fields

        internal ManualResetEventSlim _resetEventForWindowShowing;
        /// <summary>
        /// This string represents the text of the button that we want to assume the user has clicked on when the message-box is shown,
        /// for use when running automated tests.
        /// </summary>
        private string _buttonTextToSelect;

        private JhDialogResult? _buttonResultToSelect;

        /// <summary>
        /// This maintains a collection of the current JhMessageBox instances, intended for running automated unit-tests
        /// - such that the test method can check for the presence and properties of "a" message-box.
        /// </summary>
        private static List<JhMessageBoxWindow> _instancesCurrentlyBeingDisplayed;

        /// <summary>
        /// This maintains a collection of the JhMessageBox instances that have been shown to the user,
        /// and then closed. This is intended for running automated unit-tests
        /// - such that the test method can check that message-box was shown and examine its properties.
        /// </summary>
        private static List<JhMessageBoxWindow> _instancesThatHaveDisplayed;

        /// <summary>
        /// When true, this flag indicates that the calling program is running automated unit-tests.
        /// </summary>
        private bool _isTesting;

        /// <summary>
        /// This flag indicates that, when in test-mode, instead of timing out - the message-box is to remain open until the method SimulateClose is called.
        /// Default is false.
        /// </summary>
        private bool _isToWaitForExplicitClose;

        /// <summary>
        /// This flag dictates whether, when in test mode, all timeouts are limited to one second. Default is true - be fast.
        /// </summary>
        private bool _isTimeoutsFast;

        /// <summary>
        /// A reference back to the JhMessageBox that is managing this.
        /// </summary>
        private JhMessageBox _messageBoxManager;

        #endregion fields
    }
}
