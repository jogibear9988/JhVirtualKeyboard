// ****************************************************************************
// <author>James Witt Hurst, with contributions by V. Sesharaman who authored OnPreviewKeyDown and also the Tamil and Sanskrit KeyAssignmentSets</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhVirtualKeyboard</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;  // Debug
using System.Windows;
using System.Windows.Controls;  // Button, TextBox, SelectionChangedEventArgs
using System.Windows.Data;
using System.Windows.Input;  // KeyEventArgs, ExecutedRoutedEventArgs, RoutedUICommand
using JhLib;
using JhVirtualKeyboard.HelpFiles;
using JhVirtualKeyboard.ViewModels;


namespace JhVirtualKeyboard
{
    public enum WhichKeyboardLayout { Unknown, English, Arabic, French, GermanAustrian, Italian, Mandarin, Russian, Spanish, Tamil, Sanskrit };

    #region class VirtualKeyboard
    /// <summary>
    /// Interaction logic for the Virtual Keyboard.
    /// </summary>
    public partial class VirtualKeyboard : Window
    {
        #region constructor

        private VirtualKeyboard()
        {
            InitializeComponent();

            // The view-model was created during InitializeComponent,
            // so here we only need to get a reference to it..
            var objectDataProvider = (ObjectDataProvider)DataContext;
            _viewModel = (KeyboardViewModel)objectDataProvider.Data;

            if (_desiredTargetWindow != null)
            {
                _viewModel.TargetWindow = _desiredTargetWindow;
            }

            LayoutUpdated += OnLayoutUpdated;
            Loaded += OnLoaded;
#if !SILVERLIGHT
            LocationChanged += OnLocationChanged;
            PreviewKeyDown += new KeyEventHandler(OnPreviewKeyDown);
#endif
        }
        #endregion constructor

        #region event handlers

        #region LayoutUpdated event handler
        /// <summary>
        /// We use this flag to remember whether we've already done the alignment, as we only want to do it once.
        /// </summary>
        private bool _hasBeenSelfPositioned;

        /// <summary>
        /// Handle the LayoutUpdated event.
        /// We choose this event to do the alignment of this Window, because by doing so we avoid the brief flash
        /// of this Window being rendered and then moved.
        /// </summary>
        void OnLayoutUpdated(object sender, EventArgs e)
        {
            if (!_hasBeenSelfPositioned)
            {
                // Try to align it along the right side of the main window.
                this.AlignToParent(AlignmentType.ToRightOfParent);
                _hasBeenSelfPositioned = true;
            }
        }
        #endregion

        #region Loaded event handler
        /// <summary>
        /// Handle the Loaded event by completing the initialization of this Window.
        /// </summary>
        void OnLoaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("enter OnLoaded");
            // If TargetWindow hasn't been set yet, then default it to the owning Window.
#if !SILVERLIGHT
            if (_viewModel.TargetWindow == null)
            {
                _viewModel.TargetWindow = Owner as IVirtualKeyboardInjectable;
            }
#endif

            // Initialize our arrays of Key Buttons..
            _keyboardButtons = new Button[] {
                  btnVK_Oem3, btnVK_1, btnVK_2, btnVK_3, btnVK_4, btnVK_5, btnVK_6, btnVK_7, btnVK_8, btnVK_9, btnVK_0, btnVK_OemMinus, btnVK_OemPlus,
                  btnVK_Q, btnVK_W, btnVK_E, btnVK_R, btnVK_T, btnVK_Y, btnVK_U, btnVK_I, btnVK_O, btnVK_P, btnVK_OemOpenBrackets, btnVK_Oem6, btnVK_Oem5,
                  btnVK_A, btnVK_S, btnVK_D, btnVK_F, btnVK_G, btnVK_H, btnVK_J, btnVK_K, btnVK_L, btnVK_Oem1, btnVK_Oem7,
                  btnVK_Z, btnVK_X, btnVK_C, btnVK_V, btnVK_B, btnVK_N, btnVK_M, btnVK_OemComma, btnVK_OemPeriod, btnVK_OemQuestion };

            // Could also do this in Xaml.
            int n = _keyboardButtons.Length;
            for (int i = 0; i < n; i++)
            {
                Button button = _keyboardButtons[i];
                //TODO  This shouldn't be necessary.
                KeyModel keyModel = _viewModel.TheKeyModels[i];
                keyModel.KeyName = button.Name;
            }
            //TODO  This shouldn't be necessary.
            // Pre-select the language.
            //Console.WriteLine("in VirtualKeyboard.OnLoaded, _lastKeyboardLayoutWasSetTo is " + _lastKeyboardLayoutWasSetTo);
            //WhichKeyboardLayout lastKeyboardLayout = WhichKeyboardLayout.English;
#if !SILVERLIGHT
            //if (_appSettings != null)
            //{
            //    string sLastLayout = _appSettings["VKLayout"] as String;
            //    Debug.WriteLine("in OnLoaded, sLastLayout is " + sLastLayout);
            //    Enum.TryParse(sLastLayout, out lastKeyboardLayout);
            //}
#endif
            //_viewModel.SelectedKeyboardLayout = lastKeyboardLayout;
            //SetKeyAssignmentsTo(_viewModel.SelectedKeyboardLayout);

            Debug.WriteLine("exit OnLoaded");
        }
        #endregion OnLoaded

        #region LocationChanged event handler
        /// <summary>
        /// Use the LocationChanged event to remember where I am relative to the Owner window.
        /// </summary>
        void OnLocationChanged(object sender, EventArgs e)
        {
            if (Owner != null && !_isIgnoringLocationChangedEvent)
            {
                _relativeXDisplacement = this.Left - Owner.Left;
                _relativeYDisplacement = this.Top - Owner.Top;
            }
        }
        #endregion

        #region btnDismiss_Click
        /// <summary>
        /// Handler for when the user clicks on the Dismiss This button.
        /// </summary>
        private void btnDismiss_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region btnHelp_Click
        /// <summary>
        /// Handle the event that corresponds to when the user clicks on the Help button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            if (_helpWindow != null)
            {
                _helpWindow.Content = null;
                _helpWindow.Close();
            }
            if (_viewModel.SelectedKeyboardLayout != WhichKeyboardLayout.Sanskrit && _viewModel.SelectedKeyboardLayout != WhichKeyboardLayout.Tamil)
            {
                return;
            }
            _helpWindow = new Window();
            _hasHelpWindowBeenSelfPositioned = false;
            _helpWindow.LayoutUpdated += OnHelpWindowLayoutUpdated;

            _helpWindow.Width = 260; _helpWindow.Height = 600;
            if (_viewModel.SelectedKeyboardLayout == WhichKeyboardLayout.Sanskrit)
            {
                var sanskritContent = new SanskritVKHelp();
                _helpWindow.Content = sanskritContent;
            }
            else if (_viewModel.SelectedKeyboardLayout == WhichKeyboardLayout.Tamil)
            {
                var tamilContent = new TamilVKHelp();
                _helpWindow.Content = tamilContent;

            }
            _helpWindow.Owner = this;
            _helpWindow.Show();
        }
        #endregion

        #region OnHelpWindowLayoutUpdated

        /// <summary>
        /// We use this flag to remember whether we've already done the alignment, as we only want to do it once.
        /// </summary>
        private bool _hasHelpWindowBeenSelfPositioned;

        /// <summary>
        /// Handle the LayoutUpdated event.
        /// We choose this event to do the alignment of this Window, because by doing so we avoid the brief flash
        /// of this Window being rendered and then moved.
        /// </summary>
        private void OnHelpWindowLayoutUpdated(object sender, EventArgs e)
        {
            if (!_hasHelpWindowBeenSelfPositioned)
            {
                // Try to align it along the right side of the parent dialog.
                _helpWindow.AlignToParent(AlignmentType.ToRightOfParent);
                _hasHelpWindowBeenSelfPositioned = true;
            }
        }
        #endregion

        #region OnPreviewKeyDown
        /// <summary>
        /// Handle the PreviewKeyDown event for this virtual keyboard, or for any text fields of the target window
        /// that it is explicitly attached to (via calls to HandleKeysForControl).
        /// </summary>
        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // This code is courtesy of V. Sesharaman,
            // who contributed the beautiful Tamil and Sanskrit layouts, as well as the help-information Windows for those.
            // He provided this event-handler to accomplish the ability to type into the physical keyboard
            // and have it respond as the virtual-keyboard dictates.  2011/9/12

            // Capslock toggled
            if (e.Key == Key.CapsLock)
            {
                _viewModel.ExecuteCapsLockCommand();
                return;
            }
            // Backspace
            if (e.Key == Key.Back)
            {
                _viewModel.ExecuteBackspaceCommand();
                e.Handled = true;
                return;
            }
            // Enter key
            if (e.Key == Key.Enter)
            {
                KeyModel km = btnEnter.CommandParameter as KeyModel;
                _viewModel.Inject(km);
                e.Handled = true;
                return;
            }

            string keyString = string.Empty;

            // Check for numeric keys..
            if ((e.Key <= Key.NumPad9 & e.Key >= Key.NumPad0))
            {
                keyString = Convert.ToString((int)e.Key - (int)Key.NumPad0);
            }
            else if ((e.Key <= Key.D9 & e.Key >= Key.D0))
            {
                keyString = Convert.ToString((int)e.Key - (int)Key.D0);
            }
            else
            {
                keyString = (new KeyConverter()).ConvertToString(e.Key);
            }

            bool isShifted = false;
            ModifierKeys modifierKeys = Keyboard.Modifiers;
            if (modifierKeys.HasFlag(ModifierKeys.Shift) || _viewModel.IsCapsLock)
            {
                isShifted = true;
            }
            if (keyString == "Space")
            {
                keyString = keyString.ToUpper();
            }

            Button btnUser = (Button)FindName("btnVK_" + keyString);
            if (btnUser != null)
            {
                KeyModel keyViewModel = btnUser.CommandParameter as KeyModel;
                if (keyViewModel != null)
                {
                    _viewModel.Inject(keyViewModel, isShifted);

                    e.Handled = true;
                }
            }
        }
        #endregion OnPreviewKeyDown

        // This is for SilverLight, since the PreviewKeyDown event seems to be missing from that.
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            //TODO
        }

        #region OnClosing method override
        /// <summary>
        /// Override the OnClosing method to cleanup and save any settings.
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.Dispose();
            }
#if !SILVERLIGHT
            //TODO ???
            //if (TargetWindow != null)
            //{
            //    // Remove any PreviewKeyDown event handlers..
            //    if (_allControlsHandlingPreviewKeyDownEventsFor != null)
            //    {
            //        foreach (var control in _allControlsHandlingPreviewKeyDownEventsFor)
            //        {
            //            if (control != null)
            //            {
            //                control.PreviewKeyDown -= new KeyEventHandler(OnPreviewKeyDown);
            //            }
            //        }
            //    }
            //}
#endif
            // Remembered which language it was set to last time, so that we can pre-set it to that next time around.
            //TODO
#if !SILVERLIGHT
            //if (_appSettings != null)
            //{
            //    if (_isKeyboardLayoutChanged)
            //    {
            //        //Console.WriteLine("  Writing that to setting VKLayout.");
            //        _appSettings["VKLayout"] = VirtualKeyboard.LastKeyboardLayoutWasSetTo.ToString();
            //        _appSettings.Save();
            //    }
            //}
#endif
            // Set the static object to null so that we know that this has closed
            // and would need to be recreated the next time.
            VirtualKeyboard._theVirtualKeyboard = null;
        }
        #endregion OnClosing method override

        #region OnTheKeyboardClosed (static method)
        /// <summary>
        /// Handle the static Closed event of the virtual keyboard - for when I close it in order to re-open it for a different dialog-window
        /// (necessary for modal dialogs).
        /// </summary>
        private static void OnTheKeyboardClosed(object sender, EventArgs e)
        {
            _theVirtualKeyboard = ShowIt(_desiredTargetWindow);
        }
        #endregion

        #endregion event handlers

        #region Public Methods

        #region The
        /// <summary>
        /// Get the (only) VirtualKeyboard instance.
        /// </summary>
        public static VirtualKeyboard The
        {
            get { return _theVirtualKeyboard; }
        }
        #endregion

        #region ShowOrAttachTo - this is what you call from your own application.
        /// <summary>
        /// Either create and show, or else re-attach to (if it's already up) the virtual keyboard.
        /// To be called from the desired target-Window that wants to use it.
        /// </summary>
        /// <param name="targetWindow">The Window that wants to be the owner of the virtual-keyboard Window and the target of it's output</param>
        public static void ShowOrAttachTo(IVirtualKeyboardInjectable targetWindow)
        {
            try
            {
                _desiredTargetWindow = targetWindow;
                // Evidently, for modal Windows I can't share user-focus with another Window unless I first close and then recreate it.
                // A shame. Seems like a waste of time. But I don't know of a work-around to it (yet).
                if (IsUp)
                {
                    Console.WriteLine("VirtualKeyboard: re-attaching to a different Window.");
                    VirtualKeyboard.The.Closed += new EventHandler(OnTheKeyboardClosed);
                    VirtualKeyboard.The.Close();
                    targetWindow.TheVirtualKeyboard = null;
                }
                else
                {
                    VirtualKeyboard virtualKeyboard = ShowIt(targetWindow);
                    targetWindow.TheVirtualKeyboard = virtualKeyboard;
                }
            }
            catch (Exception x)
            {
                Console.WriteLine("in VirtualKeyboard.ShowOrAttachTo: " + x.Message);
                IInterlocution inter = Application.Current as IInterlocution;
                if (inter != null)
                {
                    inter.NotifyUserOfError("Well, now this is embarrassing.", "in VirtualKeyboard.ShowOrAttachTo.", x);
                }
            }
        }
        #endregion

        #region ShowIt  - this is the factory method for this class
        /// <summary>
        /// Create (and return) the VirtualKeyboard, and Show it.
        /// This is the factory-method for instantiating and showing the virtual keyboard.
        /// </summary>
        /// <param name="parentWindow">The window that will accept input from the keyboard</param>
        /// <returns>a reference to the VirtualKeyboard</returns>
        private static VirtualKeyboard ShowIt(IVirtualKeyboardInjectable targetWindow)
        {
            if (_theVirtualKeyboard == null)
            {
                _theVirtualKeyboard = new VirtualKeyboard();
            }
            _theVirtualKeyboard.Owner = (Window)targetWindow;
            _theVirtualKeyboard.ShowActivated = false;
            if (!_theVirtualKeyboard.IsLoaded)
            {
                _theVirtualKeyboard.Show();
            }
            _theVirtualKeyboard.TargetWindow = targetWindow;
            if (targetWindow.ControlToInjectInto != null)
            {
                targetWindow.ControlToInjectInto.Focus();
            }
            return _theVirtualKeyboard;
        }
        #endregion

        #region HandleKeysForControl
        /// <summary>
        /// If you want the virtual keyboard to map it's layout to your physically-attached keyboard also,
        /// so that you can just type on it to get the characters -- call this method for each of your text-input fields.
        /// </summary>
        /// <param name="control">The Control for which you want to handle input key-presses</param>
        public void HandleKeysForControl(System.Windows.Controls.Control control)
        {
            //TODO ?
#if SILVERLIGHT
            control.KeyDown += new KeyEventHandler(OnKeyDown);
#else
            control.PreviewKeyDown += new KeyEventHandler(OnPreviewKeyDown);
#endif
            // Remember these controls that we bind handlers to, so we can remove them later..
            if (_allControlsHandlingPreviewKeyDownEventsFor == null)
            {
                _allControlsHandlingPreviewKeyDownEventsFor = new List<Control>();
            }
            _allControlsHandlingPreviewKeyDownEventsFor.Add(control);
        }
        #endregion

        #region Movement as a group

        public void MoveAlongWith()
        {
            this.MoveAsAGroup(_relativeXDisplacement, _relativeYDisplacement, ref _isIgnoringLocationChangedEvent);
        }
        #endregion

        #region SetCulture
        /// <summary>
        /// Set the language of the GUI itself (as distinct from the setting of the keyboard layout).
        /// </summary>
        /// <param name="culture"></param>
        public void SetCulture(System.Globalization.CultureInfo culture)
        {
            //TODO  This is just tinkering at this point -- it all needs to be implemented.
            string sCode = culture.TwoLetterISOLanguageName;
            string sName = culture.Name;
            //int iKeyboardLayoutId = culture.KeyboardLayoutId;
            //if (sCode == "de")  // German
            //{
            //    btnEnter.Content = "?";
            //    btn
            //}
            if (sCode == "es")  // Spanish
            {
                btnEnter.Content = "Intro";
                btnVK_SPACE.Content = "Spacio";
            }
            else if (sCode == "fr")  // French
            {
                btnEnter.Content = "Entree";
                btnVK_SPACE.Content = "Space";
                btnCapsLock.Content = "Verr Maj";
            }
            else if (sCode == "ru")  // Russian
            {
                btnEnter.Content = "Enter";
                btnVK_SPACE.Content = "Mecto";
            }
            else  // default to English
            {
                btnEnter.Content = "Enter";
                btnVK_SPACE.Content = "Space";
            }
        }
        #endregion

        #region SaveStateToMySettings
        /// <summary>
        /// Your Window should call this before showing the virtual keyboard, if you want it to remember which language it was set to.
        /// </summary>
        /// <param name="appSettings">Your application's Settings object</param>
        public static void SaveStateToMySettings(System.Configuration.ApplicationSettingsBase appSettings)
        {
            _appSettings = appSettings;
        }
        #endregion

        #endregion Public Methods

        #region Properties

        #region IsSoundEnabled
        /// <summary>
        /// Get/set whether to make the tock-sound whenever a virtual keyboard key is pressed.
        /// </summary>
        public bool IsSoundEnabled
        {
            get { return _isSoundEnabled; }
            set { _isSoundEnabled = value; }
        }
        #endregion

        #region IsUp
        /// <summary>
        /// Get whether this virtual keyboard is currently showing,
        /// as indicated by whether the class-variable has been set to null.
        /// </summary>
        public static bool IsUp
        {
            get { return _theVirtualKeyboard != null; }
        }
        #endregion

        #region KeyboardLayout
        /// <summary>
        /// Get/set the language-setting for this keyboard.
        /// </summary>
        public WhichKeyboardLayout KeyboardLayout
        {
            get { return _viewModel.SelectedKeyboardLayout; }
            set { _viewModel.SelectedKeyboardLayout = value; }
        }
        #endregion

        #region TargetWindow
        /// <summary>
        /// Get/set which Window we're going to inject characters into.
        /// </summary>
        public IVirtualKeyboardInjectable TargetWindow
        {
            set
            {
                _desiredTargetWindow = value;
                if (_viewModel != null)
                {
                    _viewModel.TargetWindow = value;
                }
            }
        }
        #endregion

        #region VM property
        /// <summary>
        /// Get the KeyboardViewModel
        /// </summary>
        public KeyboardViewModel VM
        {
            get { return _viewModel; }
        }
        #endregion

        #endregion Properties

        #region Internal Implementation

        #region AssignKeyNames
        /// <summary>
        /// Set the text on the face of the key-button, to the name of that button (minus the "btn" prefix)
        /// for aid in development.  You can delete this code if you don't need it for development.
        /// </summary>
        //public void AssignKeyNames()
        //{
        //    //btnVK_OEM_3.Content = "VK_OEM_3";
        //    string sFontFamily = "Arial Narrow";
        //    var font = new FontFamily(sFontFamily);

        //    // For every keyboard-button in our array of buttons..
        //    for (int i = 0; i < _keyboardButtons.Length; i++)
        //    {
        //        Button b = _keyboardButtons[i];
        //        string sButtonName = b.Name;
        //        // Remove the "btn" prefix.
        //        string sKeyName = sButtonName.Substring(3);
        //        // Replace underscores with double-underscores, since they're interpreted as shortcut-keys when alone.
        //        string sKeyNameWUnderscore = sKeyName.Replace("_", @"__");
        //        // Make some of them appear on two lines, so they'll fit.
        //        int nUnderscores = sKeyName.Count(s => s == '_');
        //        if (nUnderscores > 1)
        //        {
        //            int i1 = sKeyName.IndexOf('_');
        //            int i2 = sKeyNameWUnderscore.IndexOf('_', i1 + 2);
        //            string s1 = sKeyNameWUnderscore.Substring(0, i2);
        //            string s2 = sKeyNameWUnderscore.Substring(i2);
        //            b.Content = s1 + Environment.NewLine + s2;
        //        }
        //        else
        //        {
        //            b.Content = sKeyNameWUnderscore;
        //        }
        //        b.FontFamily = font;
        //        b.FontSize = 12.0;
        //    }
        //}
        #endregion

        #region Fields

        private IList<System.Windows.Controls.Control> _allControlsHandlingPreviewKeyDownEventsFor;
        //TODO
#if !SILVERLIGHT
        private static System.Configuration.ApplicationSettingsBase _appSettings;
#endif
        private static IVirtualKeyboardInjectable _desiredTargetWindow;
        private Window _helpWindow;
        private bool _isIgnoringLocationChangedEvent;
        private bool _isSoundEnabled = true;
        private Button[] _keyboardButtons;
        private double _relativeXDisplacement;
        private double _relativeYDisplacement;
        private static VirtualKeyboard _theVirtualKeyboard;
        private KeyboardViewModel _viewModel;

        #endregion Fields

        #endregion Internal Implementation
    }
    #endregion class VirtualKeyboard
}
