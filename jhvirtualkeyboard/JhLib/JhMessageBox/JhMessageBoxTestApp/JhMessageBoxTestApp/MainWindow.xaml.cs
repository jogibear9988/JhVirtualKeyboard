using System;
using System.Windows;
using JhLib;
using ParseLib;


namespace JhMessageBoxTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            _messageBoxManager = JhMessageBox.GetNewInstance("Invensys", "JhMessageBoxTestApp");
            rbUserMistake.IsChecked = true;
            _viewModel = MsgBoxViewModel.The;
            DataContext = _viewModel;
            Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }
        #endregion

        #region Event Handlers

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.Application.EnableVisualStyles();
            txtTime.Text = _viewModel.TimeoutValue.ToString();
        }

        private void btnStandard_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.MessageBoxType = this.JhMessageBoxTypeToUse;
            MessageBoxImage standardMessageBoxImageToUse;

            switch (_viewModel.MessageBoxType)
            {
                case JhMessageBoxType.Error:
                    standardMessageBoxImageToUse = MessageBoxImage.Error;
                    break;
                case JhMessageBoxType.Information:
                    standardMessageBoxImageToUse = MessageBoxImage.Information;
                    break;
                case JhMessageBoxType.Question:
                    standardMessageBoxImageToUse = MessageBoxImage.Question;
                    break;
                case JhMessageBoxType.Stop:
                    standardMessageBoxImageToUse = MessageBoxImage.Stop;
                    break;
                case JhMessageBoxType.UserMistake:
                    standardMessageBoxImageToUse = MessageBoxImage.Hand;
                    break;
                case JhMessageBoxType.Warning:
                    standardMessageBoxImageToUse = MessageBoxImage.Warning;
                    break;
                default:
                    standardMessageBoxImageToUse = MessageBoxImage.None;
                    break;
            }

            System.Windows.MessageBox.Show(this, _viewModel.DetailText, TextOfTitleBar, MessageBoxButton.OK, standardMessageBoxImageToUse);
        }

        private void btnNotifyUserOfMistake_Click(object sender, RoutedEventArgs e)
        {
            _messageBoxManager.NotifyUserOfMistake(_viewModel.SummaryText, _viewModel.DetailText);
        }

        private void btnTaskDialog_Click(object sender, RoutedEventArgs e)
        {
            uint iTimeout = (uint)this.TimeToDisplayIt;
            Window parentWindow = null;
            if (_viewModel.IsIncludingParentWindow)
            {
                parentWindow = this;
            }
            JhMessageBoxButtons buttons = GetButtonsToShow();
            //TaskDialogIcon iconToShow = TaskDialogIconToUse;
            string summary = _viewModel.SummaryText;
            string sText = _viewModel.DetailText;
            //string sCaption = CaptionAfterPrefix;

            //TaskDialogs.NotifyUser(sMessage: MessageToUser, sMessageAdditional: "additional stuff", sCaption: CaptionAfterPrefix, icon: iconToShow, timeout: iTimeout);
        }

        private void btnJhMessageBox_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.MessageBoxType = this.JhMessageBoxTypeToUse;
            JhMessageBoxOptions options = new JhMessageBoxOptions(_viewModel.MessageBoxType);

            if (_viewModel.IsUsingDefaultTimeoutValue)
            {
                options.TimeoutPeriodInSeconds = 0;
            }
            else
            {
                options.TimeoutPeriodInSeconds = this.TimeToDisplayIt;
                _viewModel.TimeoutValue = options.TimeoutPeriodInSeconds;
            }
            string timeoutValueText = txtTime.Text;
            //var result = ParseLib.ParseForTimeInterval(timeoutValueText);
            //if (result.IsOk)
            //{
            //    _viewModel.TimeoutValue = (int)result.ValueInSeconds.Value;
            //}
            //else
            //{
            //    Console.WriteLine("What you put for TimeoutValue doesn't look valid to me. What-up-widat?");
            //    return;
            //}
            //options.TimeoutPeriodInSeconds = _viewModel.TimeoutValue;


            if (_viewModel.IsIncludingParentWindow)
            {
                options.ParentElement = this;
            }

            options.BackgroundTexture = _viewModel.BackgroundTexture;
            options.ButtonFlags = GetButtonsToShow();
            options.CaptionAfterPrefix = _viewModel.CaptionAfterPrefix;
            if (!String.IsNullOrWhiteSpace(_viewModel.CompanyName) || !String.IsNullOrWhiteSpace(_viewModel.ApplicationName))
            {
                options.CaptionPrefix = _viewModel.CompanyName + " " + _viewModel.ApplicationName;
            }
            options.DetailText = _viewModel.DetailText;
            options.SummaryText = _viewModel.SummaryText;
            options.IsCustomButtonStyles = _viewModel.IsCustomButtonStyles;
            options.IsSoundEnabled = _viewModel.IsSoundEnabled;
            options.IsToCenterOverParent = _viewModel.IsToCenterOverParent;
            options.IsToBeTopmostWindow = _viewModel.IsTopmostWindow;
            options.IsUsingAeroGlassEffect = _viewModel.IsUsingAeroGlassEffect;
            options.IsUsingNewerSoundScheme = _viewModel.IsUsingNewerSoundScheme;
            options.IsUsingNewerIcons = _viewModel.IsUsingNewerIcons;


            JhDialogResult r = _messageBoxManager.NotifyUser(options);

            txtResult.Text = "JhDialogResult." + r.ToString();
            borderResult.Visibility = System.Windows.Visibility.Visible;
            lblResult.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #region Adding text

        private void btnAddOneChar_Click(object sender, RoutedEventArgs e)
        {
            string s = _viewModel.DetailText;
            int i = s.Length;
            int nMax = _textFull.Length;
            if (i < nMax - 1)
            {
                string newText = _textFull.Substring(0, i + 1);
                _viewModel.DetailText = newText;
                txtTextLength.Text = newText.Length.ToString();
            }
        }

        private void btnAddTenChars_Click(object sender, RoutedEventArgs e)
        {
            string s = _viewModel.DetailText;
            int i = s.Length;
            int nMax = _textFull.Length;
            if (i < nMax - 10)
            {
                string newText = _textFull.Substring(0, i + 10);
                _viewModel.DetailText = newText;
                txtTextLength.Text = newText.Length.ToString();
            }
        }

        private void btnAddOneCharToSummary_Click(object sender, RoutedEventArgs e)
        {
            string s = _viewModel.SummaryText;
            int i = s.Length;
            int nMax = _textFull.Length;
            if (i < nMax - 1)
            {
                string newText = s + _textFull.Substring(_iAddedSummary, 1);
                _iAddedSummary++;
                _viewModel.SummaryText = newText;
                txtSummaryTextLength.Text = newText.Length.ToString();
            }
        }

        private void btnAddTenCharsToSummary_Click(object sender, RoutedEventArgs e)
        {
            string s = _viewModel.SummaryText;
            int i = s.Length;
            int nMax = _textFull.Length;
            if (i < nMax - 10)
            {
                string newText = s + _textFull.Substring(_iAddedSummary, 10);
                _iAddedSummary += 10;
                _viewModel.SummaryText = newText;
                txtSummaryTextLength.Text = newText.Length.ToString();
            }
        }

        #endregion Adding text

        private void btnBeep_Click(object sender, RoutedEventArgs e)
        {
            JhMessageBoxType messageBoxType = this.JhMessageBoxTypeToUse;
            JhMessageBoxWindow.MessageBeep(messageBoxType);
        }

        private void btnSetToDefaults_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SetToDefaults();
        }

        #endregion Event Handlers

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _viewModel.Save();
            base.OnClosing(e);
        }

        public JhMessageBoxType JhMessageBoxTypeToUse
        {
            get
            {
                JhMessageBoxType t = JhMessageBoxType.None;
                if (rbError.IsChecked == true)
                {
                    t = JhMessageBoxType.Error;
                }
                else if (rbInformation.IsChecked == true)
                {
                    t = JhMessageBoxType.Information;
                }
                else if (rbQuestion.IsChecked == true)
                {
                    t = JhMessageBoxType.Question;
                }
                else if (rbUserMistake.IsChecked == true)
                {
                    t = JhMessageBoxType.UserMistake;
                }
                else if (rbSecurityIssue.IsChecked == true)
                {
                    t = JhMessageBoxType.SecurityIssue;
                }
                else if (rbSecuritySuccess.IsChecked == true)
                {
                    t = JhMessageBoxType.SecuritySuccess;
                }
                else if (rbStop.IsChecked == true)
                {
                    t = JhMessageBoxType.Stop;
                }
                else if (rbSecurityIssue.IsChecked == true)
                {
                    t = JhMessageBoxType.SecurityIssue;
                }
                else if (rbWarning.IsChecked == true)
                {
                    t = JhMessageBoxType.Warning;
                }
                return t;
            }
        }

        #region JhMessageBoxIconToUse
        /// <summary>
        /// Get which JhMessageBoxIcon the user has selected to show.
        /// </summary>
        //public JhMessageBoxIcon JhMessageBoxIconToUse
        //{
        //    get
        //    {
        //        JhMessageBoxIcon icon = JhMessageBoxIcon.None;
        //        if (rbIconInformation.IsChecked == true)
        //        {
        //            icon = JhMessageBoxIcon.Information;
        //        }
        //        else if (rbIconWarning.IsChecked == true)
        //        {
        //            icon = JhMessageBoxIcon.Warning;
        //        }
        //        else if (rbIconStop.IsChecked == true)
        //        {
        //            icon = JhMessageBoxIcon.Stop;
        //        }
        //        else if (rbIconSecurityWarning.IsChecked == true)
        //        {
        //            icon = JhMessageBoxIcon.SecurityWarning;
        //        }
        //        else if (rbIconError.IsChecked == true)
        //        {
        //            icon = JhMessageBoxIcon.Error;
        //        }
        //        else if (rbIconSecuritySuccess.IsChecked == true)
        //        {
        //            icon = JhMessageBoxIcon.SecuritySuccess;
        //        }
        //        else if (rbIconSecurityShield.IsChecked == true)
        //        {
        //            icon = JhMessageBoxIcon.SecurityShield;
        //        }
        //        else if (rbIconSecurityShieldBlue.IsChecked == true)
        //        {
        //            icon = JhMessageBoxIcon.SecurityShieldBlue;
        //        }
        //        else if (rbIconSecurityShieldGray.IsChecked == true)
        //        {
        //            icon = JhMessageBoxIcon.SecurityShieldGray;
        //        }
        //        else if (rbIconInfoBlue.IsChecked == true)
        //        {
        //            icon = JhMessageBoxIcon.InformationBlue;
        //        }
        //        else if (rbIconExclamationGreen.IsChecked == true)
        //        {
        //            icon = JhMessageBoxIcon.ExclamationGreen;
        //        }
        //        else if (rbStopDelete.IsChecked == true)
        //        {
        //            icon = JhMessageBoxIcon.StopDelete;
        //        }
        //        else if (rbIconNone.IsChecked == true)
        //        {
        //            icon = JhMessageBoxIcon.None;
        //        }
        //        return icon;
        //    }
        //}
        #endregion

        #region TaskDialogIconToUse
        /// <summary>
        /// Get which TaskDialogIcon the user has selected to show.
        /// </summary>
        //public TaskDialogIcon TaskDialogIconToUse
        //{
        //    get
        //    {
        //        TaskDialogIcon icon = TaskDialogIcon.Information;
        //        if (rbIconInformation.IsChecked == true)
        //        {
        //            icon = TaskDialogIcon.Information;
        //        }
        //        else if (rbIconWarning.IsChecked == true)
        //        {
        //            icon = TaskDialogIcon.Warning;
        //        }
        //        else if (rbIconStop.IsChecked == true)
        //        {
        //            icon = TaskDialogIcon.Stop;
        //        }
        //        else if (rbIconSecurityWarning.IsChecked == true)
        //        {
        //            icon = TaskDialogIcon.SecurityWarning;
        //        }
        //        else if (rbIconError.IsChecked == true)
        //        {
        //            icon = TaskDialogIcon.Error;
        //        }
        //        else if (rbIconSecuritySuccess.IsChecked == true)
        //        {
        //            icon = TaskDialogIcon.SecuritySuccess;
        //        }
        //        else if (rbIconSecurityShield.IsChecked == true)
        //        {
        //            icon = TaskDialogIcon.SecurityShield;
        //        }
        //        else if (rbIconSecurityShieldBlue.IsChecked == true)
        //        {
        //            icon = TaskDialogIcon.SecurityShieldBlue;
        //        }
        //        else if (rbIconSecurityShieldGray.IsChecked == true)
        //        {
        //            icon = TaskDialogIcon.SecurityShieldGray;
        //        }
        //        else if (rbIconNone.IsChecked == true)
        //        {
        //            icon = TaskDialogIcon.None;
        //        }
        //        return icon;
        //    }
        //}
        #endregion

        #region GetButtonsToShow
        /// <summary>
        /// Return the JhMessageBoxButtons object that dictates which buttons to show on the JhMessageBox.
        /// </summary>
        public JhMessageBoxButtons GetButtonsToShow()
        {
            var buttons = JhMessageBoxButtons.None;
            if (_viewModel.IsToPutOkButton)
            {
                buttons |= JhMessageBoxButtons.Ok;
            }
            if (_viewModel.IsToPutCancelButton)
            {
                buttons |= JhMessageBoxButtons.Cancel;
            }
            if (_viewModel.IsToPutNoButton)
            {
                buttons |= JhMessageBoxButtons.No;
            }
            if (_viewModel.IsToPutYesButton)
            {
                buttons |= JhMessageBoxButtons.Yes;
            }
            if (_viewModel.IsToPutRetryButton)
            {
                buttons |= JhMessageBoxButtons.Retry;
            }
            if (_viewModel.IsToPutCloseButton)
            {
                buttons |= JhMessageBoxButtons.Close;
            }
            if (_viewModel.IsToPutIgnoreButton)
            {
                buttons |= JhMessageBoxButtons.Ignore;
            }
            return buttons;
        }
        #endregion

        #region TextOfTitleBar
        /// <summary>
        /// Get the text to display in the title-bar of the JhMessageBox Window.
        /// </summary>
        public string TextOfTitleBar
        {
            get
            {
                return _viewModel.CompanyName + " " + _viewModel.ApplicationName + ":  " + _viewModel.CaptionAfterPrefix;
            }
        }
        #endregion

        #region TimeToDisplayIt
        /// <summary>
        /// Get how long to display the message-box (ie, it's timeout value) in milliseconds.
        /// </summary>
        public int TimeToDisplayIt
        {
            get
            {
                ResultOfParseForNumber r = Parser.ParseForInteger(txtTime, false);
                if (r.IsOk)
                {
                    return r.IntegerValue;
                }
                else
                {
                    System.Windows.MessageBox.Show("There's a problem with what you entered for the time: " + r.Reason);
                    txtTime.Focus();
                    return 0;
                }
            }
        }
        #endregion

        #region fields

        private MsgBoxViewModel _viewModel;

        private JhMessageBox _messageBoxManager;

        private int _iAddedSummary = 0;

        private string _textFull = "This is the user text. The Court of Appeals (the highest court in New York) reversed and dismissed Palsgraf's complaint, deciding that the relationship of the guard's action to Palsgraf's injury was too indirect to make him liable."
        + Environment.NewLine +
        "Cardozo, writing for three other judges, wrote that there was no way that the guard could have known that the package wrapped in newspaper was dangerous, and that pushing the passenger would thereby cause an explosion. The court wrote that there was nothing in the situation to suggest to the most cautious mind that the parcel wrapped in newspaper would spread wreckage through the station. If the guard had thrown it down knowingly and willfully, he would not have threatened the plaintiff's safety, so far as appearances could warn him. Without any perception that one's actions could harm someone, there could be no duty towards that person, and therefore no negligence for which to impose liability."
        + Environment.NewLine +
        "The court also stated that whether the guard had acted negligently to the passenger he pushed was irrelevant for her claim, because the only negligence that a person can sue for is a wrongful act that violates their own rights. Palsgraf could not sue the guard for pushing the other passenger because that act did not violate a duty to her, as is required for liability under a negligence theory. It is not enough for a plaintiff to merely claim an injury. If the harm was not willful, he must show that the act as to him had possibilities of danger so many and apparent as to entitle him to be protected against the doing of it though the harm was unintended."
        + Environment.NewLine +
        "This concept of foreseeability in tort law tends to limit liability to the consequences of an act that could reasonably be foreseen rather than every single consequence that follows. Otherwise, liability could be unlimited in scope, as causes never truly cease having effects far removed in time and space (ex. the Butterfly Effect).";

        #endregion fields
    }
}
