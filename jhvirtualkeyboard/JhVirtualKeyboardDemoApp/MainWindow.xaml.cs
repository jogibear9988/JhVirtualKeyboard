// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhVirtualKeyboardDemoApp</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using JhVirtualKeyboard;


namespace JhVirtualKeyboardDemoApp
{
    /// <summary>
    /// The code-behind logic for our MainWindow.
    /// </summary>
    public partial class MainWindow : Window, IVirtualKeyboardInjectable
    {
        // The constructor
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new ViewModel();
            // Remember which text field has the current focus.
            _txtLastToHaveFocus = txtBox;
            // Set up our event handlers.
            this.ContentRendered += OnContentRendered;
            this.LocationChanged += OnLocationChanged;
            this.Closing += new System.ComponentModel.CancelEventHandler(OnClosing);
            // Set the DataContext to our view-model.
            DataContext = _viewModel;
        }

        #region Event Handlers

        #region ContentRendered event handler
        /// <summary>
        /// Handle the ContentRendered event.
        /// </summary>
        void OnContentRendered(object sender, EventArgs e)
        {
            // If the Virtual Keyboard was up (being shown) when this application was last closed,
            // show it now.
            if (Properties.Settings.Default.IsVKUp)
            {
                ShowTheVirtualKeyboard();
            }
            // Put the initial focus on our first text field.
            txtBox.Focus();
        }
        #endregion

        #region LocationChanged event handler
        /// <summary>
        /// Handle the LocationChanged event, which is raised whenever the application window is moved.
        /// </summary>
        void OnLocationChanged(object sender, EventArgs e)
        {
            // Do this if, when your user moves the application window around the screen,
            // you want the Virtual Keyboard to move along with it.
            if (_virtualKeyboard != null)
            {
                _virtualKeyboard.MoveAlongWith();
            }
        }
        #endregion

        #region GotFocus event handlers
        private void txtBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Remember that the plain TextBox was the last to receive focus.
            _txtLastToHaveFocus = txtBox;
        }

        private void txtrichBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Remember that the RichTextBox was the last to receive focus.
            _txtLastToHaveFocus = txtrichBox;
        }
        #endregion

        #region btnVK_Click
        /// <summary>
        /// Handle the event that happens when the user clicks on the Show Virtual Keyboard button.
        /// </summary>
        private void btnVK_Click(object sender, RoutedEventArgs e)
        {
            ShowTheVirtualKeyboard();
        }
        #endregion

        #region btnClose_Click
        /// <summary>
        /// Handle the event that happens when the user clicks on the Close button.
        /// </summary>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Close this application.
            Close();
        }
        #endregion

        #region Closing event handler
        /// <summary>
        /// Handle the Closing event.
        /// </summary>
        void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool isVirtualKeyboardUp = _virtualKeyboard != null && VirtualKeyboard.IsUp;
            Properties.Settings.Default.IsVKUp = isVirtualKeyboardUp;
            Properties.Settings.Default.Save();
        }
        #endregion

        #endregion Event Handlers

        #region ShowTheVirtualKeyboard
        /// <summary>
        /// Put up the JhVirtualKeyboard
        /// </summary>
        public void ShowTheVirtualKeyboard()
        {
            // (Optional) Enable it to remember which language it was set to last time, so that it can preset itself to that this time also.
//            VirtualKeyboard.SaveStateToMySettings(Properties.Settings.Default);
            // Show the keyboard.
            VirtualKeyboard.ShowOrAttachTo(this);

            // (Optional) Call this for each text field for which you want the virtual keyboard to also remap your physical keyboard.
            if (_virtualKeyboard != null)
            {
                _virtualKeyboard.HandleKeysForControl(txtBox);
                _virtualKeyboard.HandleKeysForControl(txtrichBox);
            }
            else
            {
                Console.WriteLine("Uh-oh: in JhVirtualKeyboardDemoApp.MainWindow.ShowTheVirtualKeyboard, _virtualKeyboard is null!");
            }
        }
        #endregion

        #region Implementation for interface IVirtualKeyboardInjectable

        public System.Windows.Controls.Control ControlToInjectInto
        {
            get { return _txtLastToHaveFocus; }
        }

        /// <summary>
        /// Get or set the application-window's reference to the virtual-keyboard.
        /// </summary>
        public VirtualKeyboard TheVirtualKeyboard
        {
            get { return _virtualKeyboard; }
            set { _virtualKeyboard = value; }
        }

        #endregion Implementation for interface IVirtualKeyboardInjectable

        #region fields

        private VirtualKeyboard _virtualKeyboard;
        private Control _txtLastToHaveFocus;
        private ViewModel _viewModel;

        #endregion fields
    }
}
