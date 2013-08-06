// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhVirtualKeyboard</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using JhLib;
using JhVirtualKeyboard.KeyAssignmentSets;


namespace JhVirtualKeyboard.ViewModels
{
    /// <summary>
    /// The KeyboardViewModel is (duh) the view-model for the Virtual Keyboard Window.
    /// It's DataContext gets set to this.
    /// </summary>
    public class KeyboardViewModel : BaseViewModel, IDisposable
    {
        #region constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public KeyboardViewModel()
        {
            Debug.WriteLine("****Creating new KeyboardViewModel (default ctor)****");
            _VK_SPACE.IsLetter = false;
            _VK_SPACE.UnshiftedCodePoint = ' ';
            _Tab.IsLetter = false;
            _Tab.UnshiftedCodePoint = '\t';

            //WhichKeyboardLayout lastKeyboardLayout = WhichKeyboardLayout.English;
#if !SILVERLIGHT
            SelectedKeyboardLayout = RegistrySettings.The.TheKeyboardLayout;
            //if (_view.AppSettings != null)
            //{
            //    string sLastLayout = _view.AppSettings["VKLayout"] as String;
            //    Debug.WriteLine("in KeyboardViewModel.ctor, sLastLayout is " + sLastLayout);
            //    Enum.TryParse(sLastLayout, out lastKeyboardLayout);
            //}
#endif
            KeyModel.theKeyboardViewModel = this;
        }
        #endregion constructor

        #region Commanding

        #region AltStateCommand

        public RelayCommand AltStateCommand
        {
            get
            {
                if (_altStateCommand == null)
                {
                    _altStateCommand = new RelayCommand(
                        (x) => ExecuteAltStateCommand()
                    );
                }
                return _altStateCommand;
            }
        }

        #region ExecuteAltStateCommand
        /// <summary>
        /// Execute the AltStateCommand, which corresponds to the user pressing the right-hand "ALT" key.
        /// </summary>
        public void ExecuteAltStateCommand()
        {
            if (IsAltGrKeyUsed)
            {
                ToggleAltGrState();
            }
        }
        #endregion

        private RelayCommand _altStateCommand;

        #endregion AltStateCommand

        #region BackspaceCommand

        public RelayCommand BackspaceCommand
        {
            get
            {
                if (_backspaceCommand == null)
                {
                    _backspaceCommand = new RelayCommand(
                        (x) => ExecuteBackspaceCommand()
                    );
                }
                return _backspaceCommand;
            }
        }

        public void ExecuteBackspaceCommand()
        {
            Debug.WriteLine("KeyboardViewModel.ExecuteBackspaceCommand");

            if (TargetWindow != null)
            {
#if !SILVERLIGHT
                ((Window)TargetWindow).Focus();
#endif
                TextBox txtTarget = TargetWindow.ControlToInjectInto as TextBox;
                if (txtTarget != null)
                {
                    // Use the built-in Backspace command. Create it only once, and remove it when this VirtualKeyboard window is closing.
                    //TODO
#if !SILVERLIGHT
                    if (_backspaceCommandBinding == null)
                    {
                        _backspaceCommandBinding = new CommandBinding(EditingCommands.Backspace);
                        txtTarget.CommandBindings.Add(_backspaceCommandBinding);
                    }
                    _backspaceCommandBinding.Command.Execute(null);
#endif
                    MakeBackspaceSound();

                    // Perhaps I should return to the following code, if it will work in SL. ?

                    // This was how I did the Backspace operation without using the command..
                    //int iCaretIndex = txtTarget.CaretIndex;
                    //if (iCaretIndex > 0)
                    //{
                    //    string sOriginalContent = txtTarget.Text;
                    //    int nLength = sOriginalContent.Length;
                    //    if (iCaretIndex >= nLength)
                    //    {
                    //        txtTarget.Text = sOriginalContent.Substring(0, nLength - 1);
                    //    }
                    //    else
                    //    {
                    //        string sPartBeforeCaret = sOriginalContent.Substring(0, iCaretIndex - 1);
                    //        string sPartAfterCaret = sOriginalContent.Substring(iCaretIndex, nLength - iCaretIndex);
                    //        txtTarget.Text = sPartBeforeCaret + sPartAfterCaret;
                    //    }
                    //    txtTarget.CaretIndex = iCaretIndex - 1;
                    //}
                }
                else // let's hope it's an IInjectableControl
                {
                    IInjectableControl targetControl = TargetWindow.ControlToInjectInto as IInjectableControl;
                    if (targetControl != null)
                    {
                        targetControl.Backspace();
                        MakeBackspaceSound();
                    }
                    //else
                    //{
                    //    FsWpfControls.FsRichTextBox.FsRichTextBox txtrTarget = TargetWindow.ControlToInjectInto as FsWpfControls.FsRichTextBox.FsRichTextBox;
                    //    if (txtrTarget != null)
                    //    {
                    //        txtrTarget.InsertThis(chWhat.ToString());
                    //    }
                }
            }
            //textbox.Text = sOriginalContent.Insert(iCaretIndex, sStuffToInsert);
        }

        private RelayCommand _backspaceCommand;

        #endregion BackspaceCommand

        #region CapsLockCommand

        public RelayCommand CapsLockCommand
        {
            get
            {
                if (_capsLockCommand == null)
                {
                    _capsLockCommand = new RelayCommand(
                        (x) => ExecuteCapsLockCommand()
                    );
                }
                return _capsLockCommand;
            }
        }

        #region ExecuteCapsLockCommand
        /// <summary>
        /// Execute the CapsLockCommand, which ocurrs when the user clicks on the CAPS button.
        /// </summary>
        public void ExecuteCapsLockCommand()
        {
            if (IsCapsLock)
            {
                IsCapsLock = false;
            }
            else
            {
                IsCapsLock = true;
            }
            SetFocusOnTarget();
        }
        #endregion

        private RelayCommand _capsLockCommand;

        #endregion CapsLockCommand

        #region ExecuteHelpCommand
        /// <summary>
        /// Execute the HelpCommand, which ocurrs when the user clicks on the Help button.
        /// </summary>
        public void ExecuteHelpCommand()
        {
            //Debug.WriteLine("KeyboardViewModel.ExecuteHelpCommand");
        }
        #endregion

        #region KeyPressedCommand

        public RelayCommand KeyPressedCommand
        {
            get
            {
                if (_keyPressedCommand == null)
                {
                    _keyPressedCommand = new RelayCommand(
                        (x) => ExecuteKeyPressedCommand(x)
                    );
                }
                return _keyPressedCommand;
            }
        }

        #region ExecuteKeyPressedCommand
        /// <summary>
        /// Execute the KeyPressedCommand.
        /// </summary>
        /// <param name="arg">The KeyModel of the key-button that was pressed</param>
        public void ExecuteKeyPressedCommand(object arg)
        {
            //Debug.WriteLine("KeyboardViewModel.ExecuteKeyPressedCommand, arg = " + arg);
            KeyModel keyModel = arg as KeyModel;
            // Do for every key except the AltGr key.
            if (keyModel != Alt_Right)
            {
                if (keyModel != null)
                {
                    //Console.WriteLine("ExecuteKeyPressedCommand, btn.Name = " + btn.Name);
                    Inject(keyModel);
                }
                else
                {
                    String sWhat = arg as String;
                    if (!String.IsNullOrEmpty(sWhat))
                    {
                        Inject(sWhat);
                    }
                }
            }
        }
        #endregion

        private RelayCommand _keyPressedCommand;

        #endregion KeyPressedCommand

        #region ShiftCommand

        public RelayCommand ShiftCommand
        {
            get
            {
                if (_shiftCommand == null)
                {
                    _shiftCommand = new RelayCommand(
                        (x) => ExecuteShiftCommand()
                    );
                }
                return _shiftCommand;
            }
        }

        #region ExecuteShiftCommand
        /// <summary>
        /// Execute the ShiftCommand, which ocurrs when the user clicks on the SHIFT key.
        /// </summary>
        public void ExecuteShiftCommand()
        {
            //Debug.WriteLine("KeyboardViewModel.ExecuteShiftCommand");
            ToggleShiftState();
        }
        #endregion

        private RelayCommand _shiftCommand;

        #endregion ShiftCommand

        #endregion Commanding

        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            // Don't dispose more than once.
            if (_isAlreadyDisposed)
            {
                return;
            }
            if (isDisposing)
            {
                // Free managed resources..
                ClearShiftKeyResetTimer();
                ClearStatusTimer();
                // Cleanup the CommandBinding if we attached one to the target control..
                if (_targetWindow != null)
                {
                    //TODO: This needs to be expanded to work with more than just the TextBox.
#if !SILVERLIGHT
                    TextBox txtTarget = _targetWindow.ControlToInjectInto as TextBox;
                    if (txtTarget != null)
                    {
                        if (txtTarget.CommandBindings.Count > 0)
                        {
                            if (_backspaceCommandBinding != null)
                            {
                                txtTarget.CommandBindings.Remove(_backspaceCommandBinding);
                                _backspaceCommandBinding = null;
                            }
                        }
                    }
#endif
                }
            }
            // Free unmanaged resources..

            _isAlreadyDisposed = true;
        }

        #endregion IDisposable implementation

        #region Methods

        #region ClearStatusTimer
        /// <summary>
        /// Dispose of the timer that's used to clear the status-text.
        /// </summary>
        public void ClearStatusTimer()
        {
            // Dispose of the status-text-timer, if that's still around.
            if (_timerStatusText != null)
            {
                if (_timerStatusText.IsEnabled)
                {
                    _timerStatusText.Stop();
                }
                _timerStatusText = null;
            }
        }
        #endregion

        #region ClearShiftKeyResetTimer
        /// <summary>
        /// Dispose of the timer that's used to reset the shift-lock state back to the unshifted state.
        /// </summary>
        public void ClearShiftKeyResetTimer()
        {
            // Dispose of the reset-timer, if that's still around.
            if (_timerResetShiftKey != null)
            {
                if (_timerResetShiftKey.IsEnabled)
                {
                    _timerResetShiftKey.Stop();
                }
                _timerResetShiftKey = null;
            }
        }
        #endregion

        #region Inject
        /// <summary>
        /// Type the given string into whatever the target TextBox (or similar control) is.
        /// </summary>
        /// <param name="sWhat"></param>
        public void Inject(string sWhat)
        {
            if (TargetWindow != null)
            {
                ((Window)TargetWindow).Focus();
                TextBox txtTarget = TargetWindow.ControlToInjectInto as TextBox;
                if (txtTarget != null)
                {
                    if (txtTarget.IsEnabled && !txtTarget.IsReadOnly)
                    {
                        if (IsFlowDirectionLeftToRight
                        || (!IsFlowDirectionLeftToRight && !IsForcingLeftToRight))
                        {
                            txtTarget.InsertText(sWhat);
                        }
                        else  // the FlowDirection is backwards, but the user wants to force it to insert LeftToRight.
                        {
                            txtTarget.InsertTextLeftToRight(sWhat);
                        }
                        MakeKeyPressSound();
                    }
                    else
                    {
                        if (!txtTarget.IsEnabled)
                        {
                            SetStatusTextTo("Cannot type into disabled field");
                        }
                        else
                        {
                            SetStatusTextTo("Cannot type into readonly field");
                        }
                    }
                }
                else
                {
                    RichTextBox richTextBox = TargetWindow.ControlToInjectInto as RichTextBox;
                    if (richTextBox != null)
                    {
                        if (richTextBox.IsEnabled && !richTextBox.IsReadOnly)
                        {
                            richTextBox.InsertText(sWhat);
                            MakeKeyPressSound();
                        }
                        else
                        {
                            if (!txtTarget.IsEnabled)
                            {
                                SetStatusTextTo("Cannot type into disabled field");
                            }
                            else
                            {
                                SetStatusTextTo("Cannot type into readonly field");
                            }
                        }
                    }
                    else // let's hope it's an IInjectableControl
                    {
                        IInjectableControl targetControl = TargetWindow.ControlToInjectInto
                            as IInjectableControl;
                        if (targetControl != null)
                        {
                            targetControl.InsertText(sWhat);
                            MakeKeyPressSound();
                        }
                    }
                    //else   // if you have other text-entry controls such as a rich-text box, include them here.
                    //{
                    //    FsWpfControls.FsRichTextBox.FsRichTextBox txtrTarget
                    //     = TargetWindow.ControlToInjectInto as FsWpfControls.FsRichTextBox.FsRichTextBox;
                    //    if (txtrTarget != null)
                    //    {
                    //        txtrTarget.InsertThis(sWhat);
                    //    }
                }
            }
        }

        /// <summary>
        /// Inject the character represented by the given "key" switch view-model, into whatever the target TextBox is.
        /// </summary>
        /// <param name="vm">The KeyAssignment that carries the information of what to inject</param>
        public void Inject(KeyModel vm, bool? isShifted = null)
        {
            // The Tag would've been another way to associate the view-model with the button.
            if (vm != null)
            {
                if (vm == EnterKey)
                {
                    Inject(Environment.NewLine);
                }
                else
                {
                    if (isShifted.HasValue)
                    {
                        if (isShifted.Value == true)
                        {
                            Inject(vm.ShiftedCodePoint.ToString());
                        }
                        else
                        {
                            Inject(vm.UnshiftedCodePoint.ToString());
                        }
                    }
                    else
                    {
                        Debug.WriteLine("invoking Inject, _isCapsLock is " + _isCapsLock);
                        Inject(vm.CodePointString);
                    }
                }
            }
            // If we were in Shift-mode, reset that now.
            IsShiftLock = false;
            ClearShiftKeyResetTimer();
        }
        #endregion

        #region MakeBackspaceSound
        /// <summary>
        /// Make a "tock" sound (rather like you might hear with a clock's tick-tock sound). This is done is several places,
        /// so this method is provided in order to keep the code related to the choice of sound, in one place.
        /// </summary>
        public void MakeBackspaceSound()
        {
            // WP7 makes a slightly different tone for this, which I like. TODO: Get an audio file to play for this.
#if !SILVERLIGHT
            AudioLib.PlaySoundResource(SoundResourceForKeyPressSound);
#endif
        }
        #endregion

        #region MakeKeyPressSound
        /// <summary>
        /// Make a "tock" sound (rather like you might hear with a clock's tick-tock sound). This is done is several places,
        /// so this method is provided in order to keep the code related to the choice of sound, in one place.
        /// </summary>
        public void MakeKeyPressSound()
        {
            //todo
#if !SILVERLIGHT
            AudioLib.PlaySoundResource(SoundResourceForKeyPressSound);
#endif
        }
        #endregion

        #region NotifyTheIndividualKeys
        /// <summary>
        /// Make the individual key-button view models notify their views that their properties have changed.
        /// </summary>
        public void NotifyTheIndividualKeys(string notificationText = null)
        {
            //TODO
            if (KB != null && KB.KeyAssignments != null)
            {
                if (notificationText == null)
                {
                    foreach (var keyModel in KB.KeyAssignments)
                    {
                        if (keyModel != null)
                        {
                            keyModel.Notify("Text");
                            keyModel.Notify("TextUnshiftedNL");
                            keyModel.Notify("TextShifted");
                            keyModel.Notify("TextAltGr");
                            keyModel.Notify("Tag");
                            keyModel.Notify("Content");
                            keyModel.Notify("Template");
                            keyModel.Notify("ToolTip");
                        }
                    }
                }
                else // a specific notificationText was specified.
                {
                    foreach (var keyModel in KB.KeyAssignments)
                    {
                        if (keyModel != null)
                        {
                            keyModel.Notify(notificationText);
                        }
                    }
                }
            }
        }
        #endregion

        #region OnResetTimerTick
        /// <summary>
        /// Handle the "Tick" event of either of the timers.
        /// </summary>
        private void OnResetTimerTick(object sender, EventArgs args)
        {
            if (sender == _timerResetShiftKey)
            {
                ClearShiftKeyResetTimer();
                // Clear the Shift-Lock state.
                if (IsShiftLock)
                {
                    PutIntoShiftState(false);
                }
                if (IsAltGrKeyUsed)
                {
                    IsInAltGrState = false;
                }
            }
            else if (sender == _timerStatusText)
            {
                ClearStatusTimer();
                StatusText = "";
            }
        }
        #endregion

        #region PutIntoShiftState
        /// <summary>
        /// Flip the state of the SHIFT key-button.
        /// </summary>
        public void PutIntoShiftState(bool isShiftLock)
        {
            //Debug.WriteLine("PutIntoShiftState(" + isShiftLock.ToString() + ")");
            ClearShiftKeyResetTimer();
            // Set the shift-lock state.
            IsShiftLock = isShiftLock;

            // If we're turning Shiftlock on, give that a 10-second timeout before it resets by itself.
            if (isShiftLock)
            {
                // If we're going into shift-lock state,
                // regard the AltGr state as mutually-exclusive and turn that off.
                IsInAltGrState = false;
                // Set the reset-timer.
                var timeout = TimeSpan.FromSeconds(10);
#if SILVERLIGHT
                _timerResetShiftKey = new DispatcherTimer();
#else
                _timerResetShiftKey = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
#endif
                _timerResetShiftKey.Interval = timeout;
                _timerResetShiftKey.Tick += new EventHandler(OnResetTimerTick);
                _timerResetShiftKey.Start();
            }
            SetFocusOnTarget();
        }
        #endregion

        #region SetFocusOnTarget
        /// <summary>
        /// Set focus to the TextBox (or whatever we're injecting keystrokes into) of the target window.
        /// </summary>
        public void SetFocusOnTarget()
        {
#if !SILVERLIGHT
            if (TargetWindow != null)
            {
                ((Window)TargetWindow).Focus();
            }
#endif
        }
        #endregion

        #region SetKeyAssignmentsTo
        /// <summary>
        /// Set the key assignments to those of the give language.
        /// </summary>
        public void SetKeyAssignmentsTo(WhichKeyboardLayout whichKeyboardLayout)
        {
            var keyAssignmentSet = KeyAssignmentSet.For(whichKeyboardLayout);
            KB = keyAssignmentSet;
            Notify("IsDefaultSpecialCharacters");
            Notify("IsSanskrit");
            Notify("IsTamil");
            IsFlowDirectionLeftToRight = keyAssignmentSet.FlowDirection == FlowDirection.LeftToRight;
        }
        #endregion SetKeyAssignmentsTo

        #region SetStatusTextTo
        /// <summary>
        /// Set the StatusText area to display the given string, and set a timer to clear it shortly thereafter.
        /// </summary>
        /// <param name="text">The text to display in the StatusText field</param>
        public void SetStatusTextTo(string text)
        {
            StatusText = text;
            // Set a timer to clear that status-text in 4 seconds.
            var timeout = TimeSpan.FromSeconds(4);
#if SILVERLIGHT
            _timerStatusText = new DispatcherTimer();
#else
            _timerStatusText = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
#endif
            _timerStatusText.Interval = timeout;
            _timerStatusText.Tick += new EventHandler(OnResetTimerTick);
            _timerStatusText.Start();
        }
        #endregion

        #region ToggleAltGrState
        /// <summary>
        /// Flip the state of the right-hand ALT (or ALT GR) key-button into the shifted state, if applicable.
        /// </summary>
        public void ToggleAltGrState()
        {
            ClearShiftKeyResetTimer();
            // Toggle the AltGr state.
            IsInAltGrState = !IsInAltGrState;
            // If we're turning AltGr on, give that a 10-second timeout before it resets by itself.
            if (IsInAltGrState)
            {
                // If we're going into AltGr state,
                // regard the shift-lock state as mutually-exclusive and turn that off.
                PutIntoShiftState(false);
                // Set the reset-timer.
                var timeout = TimeSpan.FromSeconds(10);
#if SILVERLIGHT
                _timerResetShiftKey = new DispatcherTimer();
#else
                _timerResetShiftKey = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
#endif
                _timerResetShiftKey.Interval = timeout;
                _timerResetShiftKey.Tick += new EventHandler(OnResetTimerTick);
                _timerResetShiftKey.Start();
            }
            else
            {
                NotifyTheIndividualKeys("IsInAltGrState");
                NotifyTheIndividualKeys("IsInNormalState");
                NotifyTheIndividualKeys("IsInUpperGlyphMode");
            }
            SetFocusOnTarget();
        }
        #endregion

        #region ToggleShiftState
        /// <summary>
        /// Turn the shift-lock mode off if it's on, and on if it's off.
        /// </summary>
        public void ToggleShiftState()
        {
            if (IsShiftLock)
            {
                PutIntoShiftState(false);
            }
            else
            {
                PutIntoShiftState(true);
            }
        }
        #endregion

        #endregion Methods

        #region Properties

        #region AvailableKeyboardLayouts
        /// <summary>
        /// Get the collection of available keyboard layouts that we can display in this keyboard.
        /// </summary>
        public IList<string> AvailableKeyboardLayouts
        {
            get
            {
                if (_availableKeyboardLayouts == null)
                {
                    _availableKeyboardLayouts = new List<string>();
                    _availableKeyboardLayouts.Add("English");
                    _availableKeyboardLayouts.Add("Arabic");
                    _availableKeyboardLayouts.Add("French");
                    _availableKeyboardLayouts.Add("GermanAustrian");
                    _availableKeyboardLayouts.Add("Russian");
                    _availableKeyboardLayouts.Add("Spanish");
                    _availableKeyboardLayouts.Add("Sanskrit");
                    _availableKeyboardLayouts.Add("Tamil");
                }
                return _availableKeyboardLayouts;
            }
        }
        #endregion

        #region ColorForAltGr
        /// <summary>
        /// Get the color to be used for the AltGr key-cap, and for the AltGr characters on whichever keys use those.
        /// </summary>
        public SolidColorBrush ColorForAltGr
        {
            get { return _colorForAltGr; }
        }
        #endregion

        #region DefaultFontFamily
        /// <summary>
        /// Get/set the FontFamily to use when the KeycapFontFamily property is null or doesn't apply.
        /// </summary>
        public FontFamily DefaultFontFamily
        {
            get
            {
                if (this.IsInDesignMode)
                {
                    // Return a more compact typeface when in design-time, so that the name can fit on the button.
                    return new FontFamily("Arial Narrow");
                }
                return _defaultFontFamily;
            }
            set
            {
                if (value != _defaultFontFamily)
                {
                    _defaultFontFamily = value;
                    Notify("DefaultFontFamily");
                }
            }
        }
        #endregion

        #region DefaultFontSize
        /// <summary>
        /// Get the value to use, unless overridden, for the FontSize property of the key-buttons.
        /// </summary>
        public double DefaultFontSize
        {
            get
            {
                if (this.IsInDesignMode)
                {
                    // Return a smaller value when in design-time, so that the name can fit on the button.
                    return 10.0;
                }
                else
                {
                    return 18.0;
                }
            }
        }
        #endregion

        #region IsAltGrKeyUsed
        /// <summary>
        /// Get/set whether the AltGr can be used within the current KeyAssignmentSet.
        /// </summary>
        public bool IsAltGrKeyUsed
        {
            get
            {
                // When viewing this project in a design-tool, always show the button.
                if (this.KB == null || this.IsInDesignMode)
                {
                    return true;
                }
                else
                {
                    return this.KB.IsAltGrKeyUsed;
                }
            }
        }
        #endregion

        #region IsCapsLock
        /// <summary>
        /// Get/set whether the VirtualKeyboard is currently is Caps-Lock mode.
        /// </summary>
        public bool IsCapsLock
        {
            get
            {
                return _isCapsLock;
            }
            set
            {
                if (value != _isCapsLock)
                {
                    _isCapsLock = value;
                    if (IsShiftLock)
                    {
                        PutIntoShiftState(false);
                    }
                    else if (IsInAltGrState)
                    {
                        IsInAltGrState = false;
                    }
                    else
                    {
                        NotifyTheIndividualKeys("IsInAltGrState");
                        NotifyTheIndividualKeys("IsInNormalState");
                        NotifyTheIndividualKeys("IsInUpperGlyphMode");
                        NotifyTheIndividualKeys("Text");
                    }
                    Notify("IsCapsLock");
                }
            }
        }
        #endregion

        #region IsDefaultSpecialCharacters
        /// <summary>
        /// Get whether to show the normal, default panel of special characters,
        /// as opposed to the special ones we would show for certain languages.
        /// </summary>
        public bool IsDefaultSpecialCharacters
        {
            get { return (SelectedKeyboardLayout != WhichKeyboardLayout.Sanskrit && SelectedKeyboardLayout != WhichKeyboardLayout.Tamil); }
        }
        #endregion

        #region IsFlowDirectionLeftToRight
        /// <summary>
        /// Get/set whether the FlowDirection that the current keyboard-layout would like to experience,
        /// is RightToLeft (ie different than what we'd ordinarily expect with English).
        /// </summary>
        public bool IsFlowDirectionLeftToRight
        {
            get { return _isLeftToRight; }
            set
            {
                if (value != _isLeftToRight)
                {
                    _isLeftToRight = value;
                    Notify("IsFlowDirectionLeftToRight");
                }
            }
        }
        #endregion

        #region IsForcingLeftToRight
        /// <summary>
        /// Get/set whether the characters that get inserted, are forced to be inserted in the LeftToRight FlowDirection
        /// even if the keyboard-layout wants to insist upon RightToLeft.
        /// </summary>
        public bool IsForcingLeftToRight
        {
            get { return _isLeftToRightOverride; }
            set
            {
                if (value != _isLeftToRightOverride)
                {
                    _isLeftToRightOverride = value;
                    Notify("IsForcingLeftToRight");
                }
            }
        }
        #endregion

        #region IsHelpButtonVisible
        /// <summary>
        /// Get/set whether the help-button, which has the little question-mark icon, is visible.
        /// </summary>
        public bool IsHelpButtonVisible
        {
            get
            {
                if (this.KB == null || this.IsInDesignMode)
                {
                    return true;
                }
                else
                {
                    return KB.IsHelpButtonVisible;
                }
            }
        }
        #endregion

        #region IsInAltGrState
        /// <summary>
        /// Get/set whether the VirtualKeyboard is currently in the AltGr mode.
        /// </summary>
        public bool IsInAltGrState
        {
            get
            {
                return _Alt_Right.IsInAltGrState;
            }
            set
            {
                if (value != _Alt_Right.IsInAltGrState)
                {
                    _Alt_Right.IsInAltGrState = value;
                    Notify("IsInAltGrState");
                    NotifyTheIndividualKeys("IsInNormalState");
                    Notify("KB");
                }
            }
        }
        #endregion

        #region IsInNormalState
        /// <summary>
        /// Get whether we're in a "normal" state - ie, in neither SHIFT nor ALT state.
        /// </summary>
        public bool IsInNormalState
        {
            get { return !IsInAltGrState && !IsShiftLock; }
        }
        #endregion

        #region IsSanskrit
        /// <summary>
        /// Get true if we're showing the Sanskrit keyboard-layout. Used for setting the visibility of the Sanskrit special-chars.
        /// </summary>
        public bool IsSanskrit
        {
            get { return SelectedKeyboardLayout == WhichKeyboardLayout.Sanskrit; }
        }
        #endregion

        #region IsShiftLock
        /// <summary>
        /// Get/set whether the VirtualKeyboard is currently is Shift-Lock mode.
        /// </summary>
        public bool IsShiftLock
        {
            get
            {
                return _ShiftKey.IsInShiftState;
            }
            set
            {
                if (value != _ShiftKey.IsInShiftState)
                {
                    _ShiftKey.IsInShiftState = value;
                    //Debug.WriteLine("Setting IsShiftLock to " + value);
                    //TODO: Do we really need all of these notifications? !!
                    NotifyTheIndividualKeys("IsInAltGrState");
                    NotifyTheIndividualKeys("IsInNormalState");
                    NotifyTheIndividualKeys("IsInUpperGlyphMode");
                    NotifyTheIndividualKeys("Text");
                    Notify("IsInNormalState");
                    Notify("IsShiftLock");
                    Notify("KB");
                }
            }
        }
        #endregion

        #region IsTamil
        /// <summary>
        /// Get true if we're showing the Tamil keyboard-layout. Used for setting the visibility of the Tamil special-chars.
        /// </summary>
        public bool IsTamil
        {
            get { return SelectedKeyboardLayout == WhichKeyboardLayout.Tamil; }
        }
        #endregion

        #region SelectedKeyboardLayout
        /// <summary>
        /// Get/set the language setting for this keyboard.
        /// </summary>
        public WhichKeyboardLayout SelectedKeyboardLayout
        {
            get
            {
                Debug.WriteLine("KeyboardViewModel.SelectedKeyboardLayout.get, returning " + _selectedKeyboardLayout);
                return _selectedKeyboardLayout;
            }
            set
            {
                Debug.WriteLine("KeyboardViewModel.SelectedKeyboardLayout.set, from " + _selectedKeyboardLayout.ToString() + " to " + value.ToString());
                if (value != _selectedKeyboardLayout)
                {
                    _selectedKeyboardLayout = value;

                    if (_selectedKeyboardLayout != WhichKeyboardLayout.Unknown)
                    {
                        SetKeyAssignmentsTo(_selectedKeyboardLayout);
                        RegistrySettings.The.TheKeyboardLayout = value;
                        Notify("SelectedKeyboardLayout");
                    }
                }
                SetFocusOnTarget();
            }
        }
        #endregion

        #region SelectedKeyboardLayoutString
        /// <summary>
        /// Get/set the SelectedKeyboardLayout by the string value.
        /// </summary>
        public string SelectedKeyboardLayoutString
        {
            get { return _selectedKeyboardLayout.ToString(); }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    WhichKeyboardLayout newLayout = WhichKeyboardLayout.Unknown;
                    if (WhichKeyboardLayout.TryParse(value, true, out newLayout))
                    {
                        if (newLayout != WhichKeyboardLayout.Unknown)
                        {
                            // Okay, so we have a validly-specified layout.
                            this.SelectedKeyboardLayout = newLayout;
                        }
                    }
                }
            }
        }
        #endregion

        #region KB
        /// <summary>
        /// Get/set the specific subclass of KeyAssignmentSet that is currently attached to this keyboard.
        /// </summary>
        public KeyAssignmentSet KB
        {
            get
            {
                if (_currentKeyboardAssignment == null)
                {
                    if (IsInDesignMode)
                    {
                        _currentKeyboardAssignment = KeyAssignmentSet.For(WhichKeyboardLayout.English);
                    }
                    else
                    {
                        _currentKeyboardAssignment = KeyAssignmentSet.For(SelectedKeyboardLayout);
                    }
                }
                //                Debug.WriteLine("in KB.get, returning " + _currentKeyboardAssignment);
                return _currentKeyboardAssignment;
            }
            set
            {
                Debug.WriteLine("KeyboardViewModel.KB.set, from " + StringLib.AsNonNullString(_currentKeyboardAssignment) + " to " + value.ToString());
                if (value != _currentKeyboardAssignment)
                {
                    _currentKeyboardAssignment = value;
                    Notify("KB");
                }
            }
        }
        #endregion

        #region StatusText
        /// <summary>
        /// Get/set the text to display in the txtStatusText field (at the center-bottom).
        /// </summary>
        public string StatusText
        {
            get { return _statusText; }
            set
            {
                if (value != _statusText)
                {
                    _statusText = value;
                    Notify("StatusText");
                }
            }
        }
        #endregion

        #region TargetWindow
        /// <summary>
        /// Get/set which Window we're going to inject characters into.
        /// </summary>
        public IVirtualKeyboardInjectable TargetWindow
        {
            get { return _targetWindow; }
            set { _targetWindow = value; }
        }
        #endregion

        #region The view-model properties for the individual key-buttons of the keyboard

        public KeyModel TabKey
        {
            get { return _Tab; }
            set { _Tab = value; }
        }

        public KeyModel EnterKey
        {
            get { return _Enter; }
            set { _Enter = value; }
        }

        public KeyModel VK_SPACE
        {
            get { return _VK_SPACE; }
            set { _VK_SPACE = value; }
        }

        public ShiftKeyViewModel ShiftKey
        {
            get { return _ShiftKey; }
        }

        public AltKeyViewModel Alt_Right
        {
            get { return _Alt_Right; }
        }
        #endregion The view-model properties for the individual key-buttons of the keyboard

        #region TheKeyModels array
        /// <summary>
        /// Get the array of KeyModels from the KB that comprise the view-models for the keyboard key-buttons
        /// </summary>
        public KeyModel[] TheKeyModels
        {
            get
            {
                if (KB != null)
                {
                    return KB.KeyAssignments;
                }
                return null;
            }
        }
        #endregion

        #endregion Properties

        #region fields

        IList<string> _availableKeyboardLayouts;
#if !SILVERLIGHT
        private CommandBinding _backspaceCommandBinding;
#endif
        private static readonly SolidColorBrush _colorForAltGr = new SolidColorBrush(Colors.Red);
        private FontFamily _defaultFontFamily = new FontFamily("Arial Unicode MS");
        /// <summary>
        /// This flag indicates whether this object has already been disposed.
        /// </summary>
        private bool _isAlreadyDisposed;
        private bool _isCapsLock;
        private bool _isLeftToRight = true;
        private bool _isLeftToRightOverride;
        /// <summary>
        /// The KeyAssignmentSet that is currently attached to this keyboard.
        /// </summary>
        private KeyAssignmentSet _currentKeyboardAssignment;
        /// <summary>
        /// The currently-selected WhichKeyboardLayout value. The initial default value is Unknown.
        /// </summary>
        private WhichKeyboardLayout _selectedKeyboardLayout = WhichKeyboardLayout.Unknown;
        /// <summary>
        /// The text shown in the status-area at bottom of the virtual-keyboard.
        /// </summary>
        private string _statusText;
        /// <summary>
        /// The container-control of the textbox (or other control) into which this virtual-keyboard is inserting characters.
        /// </summary>
        private IVirtualKeyboardInjectable _targetWindow;
        private DispatcherTimer _timerResetShiftKey;
        private DispatcherTimer _timerStatusText;
        /// <summary>
        /// This specifies the audio-file, set as a resource within the assembly, to play for each keypress action.
        /// </summary>
#if !SILVERLIGHT
        private static readonly UnmanagedMemoryStream SoundResourceForKeyPressSound = global::JhVirtualKeyboard.Properties.Resources.tock_2;
#endif
        // These are view-models for the individual key-buttons of the keyboard which are not provided by the KeyAssignmentSet:
        private KeyModel _Tab = new KeyModel();
        private KeyModel _Enter = new KeyModel();
        private KeyModel _VK_SPACE = new KeyModel();
        private ShiftKeyViewModel _ShiftKey = new ShiftKeyViewModel();
        private AltKeyViewModel _Alt_Right = new AltKeyViewModel();

        #endregion fields
    }
}
