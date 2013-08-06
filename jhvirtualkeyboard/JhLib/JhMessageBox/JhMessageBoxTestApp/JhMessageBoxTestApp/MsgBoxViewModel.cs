using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using JhLib;


namespace JhMessageBoxTestApp
{
    [Serializable]
    [XmlRoot("Settings")]
    public class MsgBoxViewModel : BaseViewModel
    {
        #region Constructor and Persistence

        #region Constructor

        private MsgBoxViewModel()
        {
            SetToDefaults();
        }
        #endregion

        #region The factory-method
        /// <summary>
        /// Get the one instance of this class.
        /// </summary>
        public static MsgBoxViewModel The
        {
            get
            {
                if (_settings == null)
                {
                    if (File.Exists(_settingsXMLFilename))
                    {
                        _settings = DeserializeFromXML();
                    }
                    else
                    {
                        Console.WriteLine("JhMessageBoxTestApp program settings file not found - using default values.");
                        _settings = new MsgBoxViewModel();
                    }
                    _settings.IsChanged = false;
                }
                return _settings;
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Save this object to storage.
        /// </summary>
        public void Save()
        {
            try
            {
                if (IsChanged)
                {
                    SerializeToXML();
                    IsChanged = false;
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.GetType().ToString() + " in MsgBoxViewModel.Save, " + x.Message);
            }
        }
        #endregion

        #region DeserializeFromXML

        private static MsgBoxViewModel DeserializeFromXML()
        {
            MsgBoxViewModel me = null;
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(MsgBoxViewModel));
                using (var textReader = new StreamReader(_settingsXMLFilename))
                {
                    Object obj = deserializer.Deserialize(textReader);
                    me = (MsgBoxViewModel)obj;
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.GetType().ToString() + " in MsgBoxViewModel.DeserializeFromXML, " + x.Message);
                Console.WriteLine("Because I can't read the Settings file " + _settingsXMLFilename + ", I'm deleting it to start anew.");
                if (File.Exists(_settingsXMLFilename))
                {
                    File.Delete(_settingsXMLFilename);
                }
                Console.WriteLine("Creating new Settings.");
                me = new MsgBoxViewModel();
            }
            return me;
        }
        #endregion

        #region SerializeToXML
        /// <summary>
        /// Save this object to a file in XML format.
        /// </summary>
        private void SerializeToXML()
        {
            var serializer = new XmlSerializer(typeof(MsgBoxViewModel));
            var textWriter = new StreamWriter(_settingsXMLFilename);
            serializer.Serialize(textWriter, this);
            textWriter.Close();
        }
        #endregion

        #endregion Constructor and Persistence

        public void SetToDefaults()
        {
            IsTopmostWindow = false;
            IsIncludingParentWindow = false;
            IsToCenterOverParent = true;
            IsUsingDefaultTimeoutValue = true;
            IsUsingNewerIcons = false;
            IsUsingAeroGlassEffect = false;
            IsSoundEnabled = false;
            IsUsingNewerSoundScheme = false;
            BackgroundTexture = JhMessageBoxBackgroundTexture.None;
            MessageBoxType = JhMessageBoxType.None;

            IsToPutOkButton = true;
            IsToPutCancelButton = false;
            IsToPutNoButton = false;
            IsToPutYesButton = false;
            IsToPutRetryButton = false;
            IsToPutCloseButton = false;
            IsToPutIgnoreButton = false;

            CompanyName = "MyBizName";
            ApplicationName = "MyApp";
            CaptionAfterPrefix = "MyCaption";
            TimeoutValue = 20;
            MessageBoxResultVisibility = Visibility.Hidden;
        }

        #region SummaryText
        /// <summary>
        /// Get/set the content to use for the Summary Text (which goes in the upper area of the message-box).
        /// </summary>
        public string SummaryText
        {
            get { return _summaryText; }
            set
            {
                if (value != _summaryText)
                {
                    _summaryText = value;
                    IsChanged = true;
                    Notify("SummaryText");
                }
            }
        }
        #endregion

        #region DetailText
        /// <summary>
        /// Get/set the content to use for the User Message
        /// </summary>
        public string DetailText
        {
            get { return _detailText; }
            set
            {
                if (value != _detailText)
                {
                    _detailText = value;
                    IsChanged = true;
                    Notify("DetailText");
                }
            }
        }
        #endregion

        #region CaptionAfterPrefix
        /// <summary>
        /// Get the text to display in the title-bar of the JhMessageBoxWindow Window.
        /// </summary>
        public string CaptionAfterPrefix
        {
            get { return _captionAfterPrefix; }
            set
            {
                if (value != _captionAfterPrefix)
                {
                    _captionAfterPrefix = value;
                    IsChanged = true;
                    Notify("CaptionAfterPrefix");
                }
            }
        }
        #endregion

        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                if (value != _companyName)
                {
                    _companyName = value;
                    IsChanged = true;
                    Notify("CompanyName");
                }
            }
        }

        public Visibility MessageBoxResultVisibility
        {
            get { return _messageBoxResultVisibility; }
            set
            {
                if (value != _messageBoxResultVisibility)
                {
                    _messageBoxResultVisibility = value;
                    Notify("MessageBoxResultVisibility");
                }
            }
        }

        private Visibility _messageBoxResultVisibility;

        public string ApplicationName
        {
            get { return _applicationName; }
            set
            {
                if (value != _applicationName)
                {
                    _applicationName = value;
                    IsChanged = true;
                    Notify("ApplicationName");
                }
            }
        }

        public JhMessageBoxType MessageBoxType
        {
            get { return _messageBoxType; }
            set
            {
                if (value != _messageBoxType)
                {
                    _messageBoxType = value;
                    IsChanged = true;
                    Notify("MessageBoxType");
                }
            }
        }

        public bool IsUsingNewerIcons
        {
            get { return _isUsingNewIcons; }
            set
            {
                if (value != _isUsingNewIcons)
                {
                    _isUsingNewIcons = value;
                    IsChanged = true;
                    Notify("IsUsingNewIcons");
                }
            }
        }

        public bool IsUsingAeroGlassEffect
        {
            get { return _isUsingAeroGlassEffect; }
            set
            {
                if (value != _isUsingAeroGlassEffect)
                {
                    _isUsingAeroGlassEffect = value;
                    IsChanged = true;
                    Notify("IsUsingAeroGlassEffect");
                }
            }
        }

        #region IsSoundEnabled
        /// <summary>
        /// Get/set whether sound is turned on
        /// </summary>
        public bool IsSoundEnabled
        {
            get { return _isSoundEnabled; }
            set
            {
                if (value != _isSoundEnabled)
                {
                    _isSoundEnabled = value;
                    IsChanged = true;
                    Notify("IsSoundEnabled");
                }
            }
        }
        #endregion

        public bool IsUsingNewerSoundScheme
        {
            get { return _isUsingNewSoundScheme; }
            set
            {
                if (value != _isUsingNewSoundScheme)
                {
                    _isUsingNewSoundScheme = value;
                    IsChanged = true;
                    Notify("IsUsingNewSoundScheme");
                }
            }
        }

        public bool IsTopmostWindow
        {
            get { return _isTopmostWindow; }
            set
            {
                if (value != _isTopmostWindow)
                {
                    _isTopmostWindow = value;
                    IsChanged = true;
                    Notify("IsTopmostWindow");
                }
            }
        }

        public bool IsIncludingParentWindow
        {
            get { return _isIncludingParentWindow; }
            set
            {
                if (value != _isIncludingParentWindow)
                {
                    _isIncludingParentWindow = value;
                    IsChanged = true;
                    Notify("IsIncludingParentWindow");
                }
            }
        }

        #region IsUsingDefaultTimeoutValue
        /// <summary>
        /// Get/set whether to leave out an explicit value for the timeout, as opposed to putting in the value from the "Timeout" field.
        /// </summary>
        public bool IsUsingDefaultTimeoutValue
        {
            get { return _isUsingDefaultTimeoutValue; }
            set
            {
                if (value != _isUsingDefaultTimeoutValue)
                {
                    _isUsingDefaultTimeoutValue = value;
                    IsChanged = true;
                    Notify("IsUsingDefaultTimeoutValue");
                }
            }
        }
        #endregion

        public bool IsToCenterOverParent
        {
            get { return _isToCenterOverParent; }
            set
            {
                if (value != _isToCenterOverParent)
                {
                    _isToCenterOverParent = value;
                    IsChanged = true;
                    Notify("IsToCenterOverParent");
                }
            }
        }

        public JhMessageBoxBackgroundTexture BackgroundTexture
        {
            get { return _backgroundTexture; }
            set
            {
                if (value != _backgroundTexture)
                {
                    _backgroundTexture = value;
                    IsChanged = true;
                    Notify("BackgroundTexture");
                }
            }
        }

        #region IsToPutCancelButton
        /// <summary>
        /// Get/set whether to put the Cancel button in the message-box.
        /// </summary>
        public bool IsToPutCancelButton
        {
            get { return _isToPutCancelButton; }
            set
            {
                if (value != _isToPutCancelButton)
                {
                    _isToPutCancelButton = value;
                    IsChanged = true;
                    Notify("IsToPutCancelButton");
                }
            }
        }
        #endregion

        #region IsToPutCloseButton
        /// <summary>
        /// Get/set whether to put the Close button in the message-box.
        /// </summary>
        public bool IsToPutCloseButton
        {
            get { return _isToPutCloseButton; }
            set
            {
                if (value != _isToPutCloseButton)
                {
                    _isToPutCloseButton = value;
                    IsChanged = true;
                    Notify("IsToPutCloseButton");
                }
            }
        }
        #endregion

        #region IsToPutIgnoreButton
        /// <summary>
        /// Get/set whether to put the Ignore button in the message-box.
        /// </summary>
        public bool IsToPutIgnoreButton
        {
            get { return _isToPutIgnoreButton; }
            set
            {
                if (value != _isToPutIgnoreButton)
                {
                    _isToPutIgnoreButton = value;
                    IsChanged = true;
                    Notify("IsToPutIgnoreButton");
                }
            }
        }
        #endregion

        #region IsToPutOkButton
        /// <summary>
        /// Get/set whether to put the OK button in the message-box.
        /// </summary>
        public bool IsToPutOkButton
        {
            get { return _isToPutOkButton; }
            set
            {
                if (value != _isToPutOkButton)
                {
                    _isToPutOkButton = value;
                    IsChanged = true;
                    Notify("IsToPutOkButton");
                }
            }
        }
        #endregion

        #region IsToPutYesButton
        /// <summary>
        /// Get/set whether to put the Yes button in the message-box.
        /// </summary>
        public bool IsToPutYesButton
        {
            get { return _isToPutYesButton; }
            set
            {
                if (value != _isToPutYesButton)
                {
                    _isToPutYesButton = value;
                    IsChanged = true;
                    Notify("IsToPutYesButton");
                }
            }
        }
        #endregion

        #region IsToPutNoButton
        /// <summary>
        /// Get/set whether to put the No button in the message-box.
        /// </summary>
        public bool IsToPutNoButton
        {
            get { return _isToPutNoButton; }
            set
            {
                if (value != _isToPutNoButton)
                {
                    _isToPutNoButton = value;
                    IsChanged = true;
                    Notify("IsToPutNoButton");
                }
            }
        }
        #endregion

        #region IsToPutRetryButton
        /// <summary>
        /// Get/set whether to put the Retry button in the message-box.
        /// </summary>
        public bool IsToPutRetryButton
        {
            get { return _isToPutRetryButton; }
            set
            {
                if (value != _isToPutRetryButton)
                {
                    _isToPutRetryButton = value;
                    IsChanged = true;
                    Notify("IsToPutRetryButton");
                }
            }
        }
        #endregion

        #region IsCustomButtonStyles
        /// <summary>
        /// Get/set whether to use our own custom WPF Styles for the message-box buttons.
        /// </summary>
        public bool IsCustomButtonStyles
        {
            get { return _isCustomButtonStyles; }
            set
            {
                if (value != _isCustomButtonStyles)
                {
                    _isCustomButtonStyles = value;
                    IsChanged = true;
                    Notify("IsCustomButtonStyles");
                }
            }
        }
        #endregion

        public int TimeoutValue
        {
            get { return _timeoutValue; }
            set
            {
                if (value != _timeoutValue)
                {
                    _timeoutValue = value;
                    IsChanged = true;
                    Notify("TimeoutValue");
                }
            }
        }

        #region IsChanged
        /// <summary>
        /// Get/set the flag that indicates whether any values of this object have changed since the last time this flag was cleared.
        /// </summary>
        public bool IsChanged
        {
            get { return _isChanged; }
            set { _isChanged = value; }
        }
        #endregion

        #region fields

        private static MsgBoxViewModel _settings;
        private static string _settingsXMLFilename = @"JhMessageBoxTestAppSettings.xml";
        private bool _isChanged;

        //TODO: I probably should explicitly define a developer-information field.
        private string _summaryText = "The summary to the User.";
        private string _detailText = "This is the detail text";
        private JhMessageBoxType _messageBoxType;
        private string _companyName;
        private string _applicationName;
        private string _captionAfterPrefix;
        private bool _isIncludingParentWindow;
        private bool _isUsingNewIcons;
        private bool _isUsingAeroGlassEffect;
        private bool _isSoundEnabled;
        private bool _isUsingNewSoundScheme;
        private bool _isTopmostWindow;
        private bool _isUsingDefaultTimeoutValue;
        /// <summary>
        /// This indicates whether we want to center the message-box window over that of the parent window.
        /// </summary>
        private bool _isToCenterOverParent;

        private bool _isToPutCancelButton;
        private bool _isToPutCloseButton;
        private bool _isToPutIgnoreButton;
        private bool _isToPutOkButton;
        private bool _isToPutNoButton;
        private bool _isToPutYesButton;
        private bool _isToPutRetryButton;
        private JhMessageBoxBackgroundTexture _backgroundTexture;
        /// <summary>
        /// This indicates whether to use our own custom WPF Styles for the message-box buttons.
        /// </summary>
        private bool _isCustomButtonStyles;
        /// <summary>
        /// The number which the user has entered into the field that represents the timeout value, in seconds.
        /// </summary>
        private int _timeoutValue;

        #endregion fields
    }
}
