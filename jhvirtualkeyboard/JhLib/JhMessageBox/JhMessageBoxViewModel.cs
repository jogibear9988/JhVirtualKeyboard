// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace JhLib
{
    #region class JhMessageBoxViewModel
    /// <summary>
    /// A minimalist view-model-viewmodel for the JhMessageBoxWindow view.
    /// </summary>
    public class JhMessageBoxViewModel : BaseViewModel
    {
        #region Constructor and singleton accessor

        public JhMessageBoxViewModel()
        {
            if (this.IsInDesignMode)
            {
                _options = new JhMessageBoxOptions(JhMessageBoxType.Error);
                Options.ButtonFlags = JhMessageBoxButtons.Yes | JhMessageBoxButtons.No | JhMessageBoxButtons.Ok | JhMessageBoxButtons.Cancel
                    | JhMessageBoxButtons.Close | JhMessageBoxButtons.Ignore | JhMessageBoxButtons.Retry;
                Title = "Vendor Product: Caption";
                string summaryTextPattern = "The Summary Text";
                SummaryText = summaryTextPattern.ExpandTo(20);
                string detailTextPattern = "The Details Texts. X";
                DetailText = detailTextPattern.ExpandTo(63);
                //DetailText = "The Details Texts. XThe Details Texts. XThe Details Texts._XThe Details Texts._XThe Details Texts._XThe";
                //DetailText = "The Details Text";
                Options.BackgroundTexture = JhMessageBoxBackgroundTexture.None;
                Options.IsUsingNewerIcons = false;
            }
        }

        public JhMessageBoxViewModel(JhMessageBox manager, JhMessageBoxOptions options)
        {
            _options = options;
            DetailText = options.DetailText;
            SummaryText = options.SummaryText;
            // If there is no caption, then just put the caption-prefix without the spacer.
            if (String.IsNullOrWhiteSpace(options.CaptionAfterPrefix))
            {
                if (String.IsNullOrWhiteSpace(options.CaptionPrefix))
                {
                    Title = manager.CaptionPrefix;
                }
                else
                {
                    Title = options.CaptionPrefix;
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(options.CaptionPrefix))
                {
                    Title = manager.CaptionPrefix + ": " + options.CaptionAfterPrefix;
                }
                else
                {
                    Title = options.CaptionPrefix + ": " + options.CaptionAfterPrefix;
                }
            }
        }

        public static JhMessageBoxViewModel GetInstance(JhMessageBox manager, JhMessageBoxOptions options)
        {
            _theInstance = new JhMessageBoxViewModel(manager, options);
            return _theInstance;
        }

        public static JhMessageBoxViewModel Instance
        {
            get
            {
                // This is intended only for design-mode.
                if (_theInstance == null)
                {
                    _theInstance = new JhMessageBoxViewModel();
                }
                return _theInstance;
            }
        }
        #endregion Constructor and singleton accessors

        public RelayCommand CopyCommand { get; set; }
        public RelayCommand StayCommand { get; set; }

        #region BackgroundBrush
        /// <summary>
        /// Get or set the ImageBrush that gives the message-box Window it's background texture, if any. Default is null, indicating no image.
        /// </summary>
        public Brush BackgroundBrush
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                return _backgroundBrush;
            }
        }
        #endregion

        #region ButtonMargin
        /// <summary>
        /// Get the value to use for the Margin property of each of the buttons.
        /// </summary>
        public Thickness ButtonMargin
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                // Remember - a Thickness is a struct that gets initialized by default to Thickness(0,0,0,0)
                //            so you can't test it for being null here to see whether it has been set already.
                if (_buttonMargin == default(Thickness))
                {
                    _buttonMargin = new Thickness(_buttonMarginSide, 2, _buttonMarginSide, 2);
                }
                return _buttonMargin;
            }
        }
        #endregion

        #region ButtonCount
        /// <summary>
        /// Get the number of buttons that are visible in the message-box
        /// </summary>
        public int ButtonCount
        {
            get { return _buttonCount; }
        }
        #endregion

        #region ButtonWidth
        /// <summary>
        /// Get the value to use for the Width property of each of the buttons.
        /// </summary>
        public double ButtonWidth
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                return _buttonWidth;
            }
        }
        #endregion

        #region ButtonPanelBackground
        /// <summary>
        /// Get the Brush to use for the background fill of the lower panel that contains the buttons. Default is null.
        /// </summary>
        public Brush ButtonPanelBackground
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                return _buttonPanelBackground;
            }
        }
        #endregion

        #region button text

        public string ButtonAbortText
        {
            get { return _options.ButtonAbortText; }
        }

        public string ButtonCancelText
        {
            get { return _options.ButtonCancelText; }
        }

        public string ButtonCloseText
        {
            get { return _options.ButtonCloseText; }
        }

        public string ButtonIgnoreText
        {
            get { return _options.ButtonIgnoreText; }
        }

        public string ButtonNoText
        {
            get { return _options.ButtonNoText; }
        }

        public string ButtonOkText
        {
            get { return _options.ButtonOkText; }
        }

        public string ButtonRetryText
        {
            get { return _options.ButtonRetryText; }
        }

        public string ButtonYesText
        {
            get { return _options.ButtonYesText; }
        }

        #endregion button text

        #region button tooltips

        public string ButtonAbortToolTip
        {
            get { return _options.ButtonAbortToolTip; }
        }

        public string ButtonCancelToolTip
        {
            get { return _options.ButtonCancelToolTip; }
        }

        public string ButtonCloseToolTip
        {
            get { return _options.ButtonCloseToolTip; }
        }

        public string ButtonIgnoreToolTip
        {
            get { return _options.ButtonIgnoreToolTip; }
        }

        public string ButtonNoToolTip
        {
            get { return _options.ButtonNoToolTip; }
        }

        public string ButtonOkToolTip
        {
            get { return _options.ButtonOkToolTip; }
        }

        public string ButtonRetryToolTip
        {
            get { return _options.ButtonRetryToolTip; }
        }

        public string ButtonYesToolTip
        {
            get { return _options.ButtonYesToolTip; }
        }

        #endregion button tooltips

        #region button visibility

        /// <summary>
        /// Get the Visibility property for the Cancel button.
        /// </summary>
        public Visibility ButtonCancelVisibility
        {
            get
            {
                return (_isCancelButtonVisible ? Visibility.Visible : Visibility.Collapsed);
            }
        }
        private bool _isCancelButtonVisible;

        /// <summary>
        /// Get the Visibility property for the Close button.
        /// </summary>
        public Visibility ButtonCloseVisibility
        {
            get
            {
                return (_isCloseButtonVisible ? Visibility.Visible : Visibility.Collapsed);
            }
        }
        private bool _isCloseButtonVisible;

        /// <summary>
        /// Get the Visibility property for the Ignore button.
        /// </summary>
        public Visibility ButtonIgnoreVisibility
        {
            get
            {
                return (_isIgnoreButtonVisible ? Visibility.Visible : Visibility.Collapsed);
            }
        }
        private bool _isIgnoreButtonVisible;

        /// <summary>
        /// Get the Visibility property for the Ok button.
        /// </summary>
        public Visibility ButtonOkVisibility
        {
            get
            {
                return (_isOkButtonVisible ? Visibility.Visible : Visibility.Collapsed);
            }
        }
        private bool _isOkButtonVisible;

        /// <summary>
        /// Get the Visibility property for the Retry button.
        /// </summary>
        public Visibility ButtonRetryVisibility
        {
            get
            {
                return (_isRetryButtonVisible ? Visibility.Visible : Visibility.Collapsed);
            }
        }
        private bool _isRetryButtonVisible;

        /// <summary>
        /// Get the Visibility property for the Yes button.
        /// </summary>
        public Visibility ButtonYesVisibility
        {
            get
            {
                return (_isYesButtonVisible ? Visibility.Visible : Visibility.Collapsed);
            }
        }
        private bool _isYesButtonVisible;

        /// <summary>
        /// Get the Visibility property for the No button.
        /// </summary>
        public Visibility ButtonNoVisibility
        {
            get
            {
                return (_isNoButtonVisible ? Visibility.Visible : Visibility.Collapsed);
            }
        }
        private bool _isNoButtonVisible;

        #endregion button visibility

        #region Height
        /// <summary>
        /// Get the height to use for the message-box Window.
        /// </summary>
        public double Height
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                return _windowHeight;
            }
        }
        #endregion

        #region SummaryText
        /// <summary>
        /// Get or set the text that's shown in the upper, summary-text area.
        /// </summary>
        public string SummaryText
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_summaryText))
                {
                    return String.Empty;
                }
                else
                {
                    return _summaryText;
                }
            }
            set
            {
                if (value != _summaryText)
                {
                    _summaryText = value;
                    Notify("SummaryText");
                }
            }
        }
        #endregion

        #region SummaryTextColor
        /// <summary>
        /// Gets or sets the Brush to use for the foreground color of the summary-text. Default is 003399, which is blue-ish.
        /// </summary>
        public Brush SummaryTextColor
        {
            get
            {
                if (_summaryTextForegroundColor == null)
                {
                    _summaryTextForegroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#003399"));
                }
                return _summaryTextForegroundColor;
            }
            set
            {
                if (value != _summaryTextForegroundColor)
                {
                    _summaryTextForegroundColor = value;
                    Notify("SummaryTextColor");
                }
            }
        }
        #endregion

        #region SummaryTextBackground
        /// <summary>
        /// Get the Brush to use for the background of the summary text. Default is Transparent.
        /// </summary>
        public Brush SummaryTextBackground
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                return _summaryTextBackground;
            }
        }
        #endregion

        #region SummaryTextHorizontalContentAlignment
        /// <summary>
        /// Get the value to use for the HorizontalContentAlignment property of the summary-text. Default is Center.
        /// </summary>
        public HorizontalAlignment SummaryTextHorizontalContentAlignment
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                return _summaryTextHorizontalAlignment;
            }
        }
        #endregion

        #region SummaryTextMargin
        /// <summary>
        /// Get the value to use for the message-box's "summary text" area Margin property.
        /// </summary>
        public Thickness SummaryTextMargin
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                return _summaryTextMargin;
            }
        }
        #endregion

        #region SummaryTextRowSpan
        /// <summary>
        /// Get the value to use for the message-box's "summary text" area Grid.RowSpan attached-property.
        /// </summary>
        public int SummaryTextRowSpan
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                return _summaryTextRowSpan;
            }
            set
            {
                if (value != _summaryTextRowSpan)
                {
                    _summaryTextRowSpan = value;
                    Notify("SummaryTextRowSpan");
                }
            }
        }
        #endregion

        #region DetailText
        /// <summary>
        /// Get or set the text to be shown to the user.
        /// </summary>
        public string DetailText
        {
            get
            {
                //Console.WriteLine("VM.get_DetailText, value is " + StringLib.AsNonNullString(_detailText));
                if (String.IsNullOrWhiteSpace(_detailText))
                {
                    return String.Empty;
                }
                else
                {
                    return _detailText;
                }
            }
            set
            {
                //Console.WriteLine("VM.set_DetailText, value is " + StringLib.AsNonNullString(_detailText));
                if (value != _detailText)
                {
                    _detailText = value;
                    Notify("DetailText");
                }
            }
        }
        #endregion

        #region DetailTextBackground
        /// <summary>
        /// Get the Brush to use for the background of the DetailText. Default is Transparent.
        /// </summary>
        public Brush DetailTextBackground
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                return _detailTextBackground;
            }
        }
        #endregion

        #region DetailTextHorizontalContentAlignment
        /// <summary>
        /// Get the value to use for the HorizontalContentAlignment property of the DetailText. Default is Center.
        /// </summary>
        public HorizontalAlignment DetailTextHorizontalContentAlignment
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                return _detailTextHorizontalAlignment;
            }
        }
        #endregion

        #region DetailTextVerticalAlignment
        /// <summary>
        /// Get the value to use for the VerticalAlignment property of the DetailText. Default is Center.
        /// </summary>
        public VerticalAlignment DetailTextVerticalAlignment
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                return _detailTextVerticalAlignment;
            }
        }
        #endregion

        #region DetailTextMargin
        /// <summary>
        /// Get the value to use for the message-box's "detail text" area Margin property.
        /// </summary>
        public Thickness DetailTextMargin
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                // Remember - a Thickness is a struct that gets initialized by default to Thickness(0,0,0,0)
                //            so you can't test it for being null here to see whether it has been set already.
                if (_detailTextMargin == default(Thickness))
                {
                    _detailTextMargin = new Thickness(_detailTextMarginLeft, 0, 0, 0);
                }
                return _detailTextMargin;
            }
        }
        #endregion

        #region GradiantLeftColor
        /// <summary>
        /// Get the color for the left edge of the color gradiant that underlies the upper portion of the message-box.
        /// </summary>
        public Color GradiantLeftColor
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                return _gradiantLeftColor;
            }
        }
        #endregion

        #region GradiantRightColor
        /// <summary>
        /// Get the color for the right edge of the color gradiant that underlies the upper portion of the message-box.
        /// </summary>
        public Color GradiantRightColor
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                return _gradiantRightColor;
            }
        }
        #endregion

        #region IconImage
        /// <summary>
        /// Get the BitmapImage to use for the message-box's icon.
        /// </summary>
        public BitmapImage IconImage
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                if (_iconBitmapImage == null)
                {
                    // If no image-name has been assigned, then produce no icon-image.
                    if (_iconImageName != null)
                    {
#if SILVERLIGHT
                        string stringForUri = @"images/" + _iconImageName;
                        _bitmapImageForIcon = new BitmapImage(new Uri(stringForUri, UriKind.Relative));
#else
                        string stringForUri = @"pack://application:,,,/JhLib;Component/images/" + _iconImageName;
                        _iconBitmapImage = new BitmapImage(new Uri(stringForUri));
#endif
                    }
                }
                return _iconBitmapImage;
            }
        }
        #endregion

        #region IconMargin
        /// <summary>
        /// Get the value to use for the message-box's icon's Margin property.
        /// </summary>
        public Thickness IconMargin
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                if (_iconMargin == default(Thickness))
                {
                    _iconMargin = new Thickness(_iconMarginLeft, _iconMarginTop, _iconMarginRight, _iconMarginBottom);
                }
                return _iconMargin;
            }
        }
        #endregion

        #region Title
        /// <summary>
        /// Get or set the text to be displayed in the message-box Window's title-bar.
        /// </summary>
        public string Title
        {
            get
            {
                if (IsInDesignMode)
                {
                    return _title + " (MessageType is " + this.Options.MessageType + ")";
                }
                else
                {
                    if (_title == null)
                    {
                        return "A null title!";  //TODO
                    }
                    return _title;
                }
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    Notify("Title");
                }
            }
        }
        #endregion

        #region UpperRectangleRowSpan
        /// <summary>
        /// Get the value for the Grid.RowSpan property of the Rectangle visual element.
        /// Default is 1, but it changes to 2 if there is no detail-text.
        /// </summary>
        public int UpperRectangleRowSpan
        {
            get
            {
                return _upperRectangleRowSpan;
            }
        }
        #endregion

        #region UpperRectangleVisibility
        /// <summary>
        /// Get the visibility-value of the Rectangle that underlies the upper half of the interior of the message-box.
        /// </summary>
        public Visibility UpperRectangleVisibility
        {
            get
            {
                if (!_isInitialized)
                {
                    InitializeVisualElements();
                }
                return _upperRectangleVisibility;
            }
        }
        #endregion

        #region Options
        /// <summary>
        /// Get or set the options that dictate the appearance and behavior of the message-box.
        /// </summary>
        public JhMessageBoxOptions Options
        {
            get
            {
                if (_options == null)
                {
                    _options = new JhMessageBoxOptions(JhMessageBoxType.None);
                }
                return _options;
            }
            set { _options = value; }
        }
        #endregion

        #region internal implementation

        #region InitializeVisualElements
        /// <summary>
        /// This does all of the initialization of the text colors and the background texture-image,
        /// setting the various visual properties based upon which texture was selected.
        /// </summary>
        /// <param name="texture"></param>
        private void InitializeVisualElements()
        {
            #region Based upon the BackgroundTexture, set the background image and background colors.

            JhMessageBoxBackgroundTexture texture = this.Options.BackgroundTexture;
            ImageBrush imageBrush = null;
            string imageFilename = null;
            string sTextBackground1 = null;
            string sTextBackground2 = null;
            string sButtonPanelBackground = null;
            Rect rectViewport = new Rect(0, 0, 40, 40);
            bool isDark = false;
            switch (texture)
            {
                case JhMessageBoxBackgroundTexture.BlueTexture:
                    imageFilename = "bgBlueTexture.png";
                    rectViewport.Width = 593;
                    rectViewport.Height = 463;
                    sTextBackground1 = "#99AECA";
                    sTextBackground2 = "#99AECA";
                    isDark = true;
                    break;
                case JhMessageBoxBackgroundTexture.BlueMarble:
                    imageFilename = "bgBlueMarble.jpg";
                    rectViewport.Width = rectViewport.Height = 192;
                    //    sTextBackground1 = "#D2DDE3";
                    //    sTextBackground2 = "#D2DDE3";
                    break;
                case JhMessageBoxBackgroundTexture.BrownMarble1:
                    imageFilename = "bgBrownMarble1.gif";
                    rectViewport.Width = 300;
                    rectViewport.Height = 397;
                    //sTextBackground1 = "#F7F7EF";
                    //sTextBackground2 = "#F7F7EF";
                    sButtonPanelBackground = "#FFF0F0F0";
                    break;
                case JhMessageBoxBackgroundTexture.BrownMarble2:
                    imageFilename = "bgBrownMarble2.jpg";
                    rectViewport.Width = rectViewport.Height = 256;
                    sButtonPanelBackground = "#FFF0F0F0";
                    break;
                case JhMessageBoxBackgroundTexture.BrownTexture1:
                    imageFilename = "bgBrownTexture1.jpg";
                    rectViewport.Width = rectViewport.Height = 89;
                    break;
                case JhMessageBoxBackgroundTexture.BrownTexture2:
                    imageFilename = "bgBrownTexture2.gif";
                    rectViewport.Width = 60;
                    rectViewport.Height = 90;
                    sButtonPanelBackground = "#FFF0F0F0";
                    isDark = true;
                    break;
                case JhMessageBoxBackgroundTexture.BrushedMetal:
                    imageFilename = "bgBrushedMetal.png";
                    rectViewport.Width = 980;
                    rectViewport.Height = 192;
                    //rectBackgroundForButtonPanel.Fill = null;
                    sButtonPanelBackground = "#A5A5A5";
                    isDark = true;
                    break;
                case JhMessageBoxBackgroundTexture.GrayMarble:
                    imageFilename = "bgGrayMarble.jpg";
                    rectViewport.Width = rectViewport.Height = 128;
                    //sTextBackground1 = "#C0BBB8";
                    //txtText.Background = (SolidColorBrush)bc.ConvertFromString("#C0BBB8");
                    sButtonPanelBackground = "#C0BBB8";
                    isDark = true;
                    break;
                case JhMessageBoxBackgroundTexture.GrayTexture1:
                    imageFilename = "TextureR40x40.png";
                    rectViewport.Width = rectViewport.Height = 40;
                    //sTextBackground1 = "#BBBBBB";
                    //sTextBackground2 = "#ABABAB";
                    sButtonPanelBackground = "#ABABAB";
                    isDark = true;
                    break;
                case JhMessageBoxBackgroundTexture.GrayTexture2:
                    imageFilename = "bgGrayTexture2.tif";
                    rectViewport.Width = 429;
                    rectViewport.Height = 440;
                    sTextBackground1 = "#BBBBBB";
                    sTextBackground2 = "#BBBBBB";
                    sButtonPanelBackground = "#D5D5D5";
                    isDark = true;
                    break;
                case JhMessageBoxBackgroundTexture.GreenTexture1:
                    imageFilename = "bgGreenTexture1.gif";
                    rectViewport.Width = 102;
                    rectViewport.Height = 101;
                    sButtonPanelBackground = "#B2B08B";
                    isDark = true;
                    break;
                case JhMessageBoxBackgroundTexture.GreenTexture2:
                    imageFilename = "bgGreenTexture2.gif";
                    rectViewport.Width = 106;
                    rectViewport.Height = 108;
                    sButtonPanelBackground = "#999966";
                    isDark = true;
                    break;
                case JhMessageBoxBackgroundTexture.GreenTexture3:
                    imageFilename = "bgGreenTexture3.jpg";
                    rectViewport.Width = rectViewport.Height = 200;
                    //sTextBackground1 = "#A4B487";
                    //sTextBackground2 = "#A4B487";
                    sButtonPanelBackground = "#8E9E71";
                    isDark = true;
                    break;
                default:
                    break;
            }
            if (imageFilename != null)
            {
                imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/JhLib;Component/Images/" + imageFilename));
                imageBrush.ViewportUnits = BrushMappingMode.Absolute;
                imageBrush.Stretch = Stretch.None;
                imageBrush.Viewport = rectViewport;
                imageBrush.TileMode = TileMode.Tile;
                _backgroundBrush = imageBrush;
            }
            else
            {
                _backgroundBrush = Brushes.White;
            }
#if !SILVERLIGHT
            var bc = new System.Windows.Media.BrushConverter();
#endif
            if (sTextBackground1 == null)
            {
                _summaryTextBackground = Brushes.Transparent;
            }
            else
            {
                _summaryTextBackground = (SolidColorBrush)bc.ConvertFromString(sTextBackground1);
            }
            if (sTextBackground2 == null)
            {
                _detailTextBackground = Brushes.Transparent;
            }
            else
            {
                _detailTextBackground = (SolidColorBrush)bc.ConvertFromString(sTextBackground2);
            }
            if (sButtonPanelBackground != null)
            {
                _buttonPanelBackground = (SolidColorBrush)bc.ConvertFromString(sButtonPanelBackground);
            }
            else
            {
                // or, the default value set in XAML had been #FFF0F0F0
                //_buttonPanelBackground = Brushes.Gray; //(SolidColorBrush)bc.ConvertFromString("Gray");
                _buttonPanelBackground = (SolidColorBrush)bc.ConvertFromString("#FFF0F0F0");
                //_buttonPanelBackground = null;
            }
            #endregion Based upon the BackgroundTexture, set the background image and background colors.

            #region Colors and Icon

            // Set the colors and icon..
            Color colorLeft = Colors.Green, colorRight = Colors.Yellow;
            string sLeftColor = null;
            string sRightColor = null;
            double minLeftMargin = 39;

            switch (_options.MessageType)
            {
                case JhMessageBoxType.SecurityIssue:
                    _iconImageName = "SecurityWarning.png";
                    _iconWidth = 26;
                    _iconHeight = 33;
                    // A yellow gradiant, #F2B100 at the left, to #FECD48 at the right.
                    sLeftColor = "#F2B100";
                    sRightColor = "#FECD48";
                    if (imageFilename == null)
                    {
                        _summaryTextForegroundColor = new SolidColorBrush(Colors.Black);
                    }
                    else if (texture == JhMessageBoxBackgroundTexture.BrushedMetal)
                    {
                        _summaryTextForegroundColor = new SolidColorBrush(Colors.White);
                    }
                    break;
                case JhMessageBoxType.SecuritySuccess:
                    _iconImageName = "SecuritySuccess.png";
                    _iconWidth = 26;
                    _iconHeight = 30;
                    // This is a gradient that goes from #157615 at the left, to #39963F at the right.
                    sLeftColor = "#157615";
                    sRightColor = "#39963F";
                    //_summaryTextForegroundColor = new SolidColorBrush(Colors.White);
                    if (imageFilename == null || texture == JhMessageBoxBackgroundTexture.GreenTexture3)
                    {
                        _summaryTextForegroundColor = new SolidColorBrush(Colors.White);
                    }
                    else if (texture == JhMessageBoxBackgroundTexture.BrushedMetal)
                    {
                        _summaryTextForegroundColor = new SolidColorBrush(Colors.LightGreen);
                    }
                    else
                    {
                        _summaryTextForegroundColor = new SolidColorBrush(Colors.Green);
                    }
                    break;
                //            else if (this.IconScheme == JhMessageBoxIcon.SecurityShieldBlue)
                //            {
                //                imgIcon.Source = new BitmapImage(new Uri(@"pack://application:,,,/JhLib;Component/images/SecurityWarning.png"));
                //                //imgIcon.Source = Microsoft.SDK.Samples.VistaBridge.Library.StockIcons.StockIcons.Shield; // The yellow/blue shield thingy
                //                // This is a gradient that goes from #045042 at the left, to #1C7885 at the right.
                //                sLeftColor = "#045042";
                //                sRightColor = "#1C7885";
                //                txtSummaryText.Foreground = (SolidColorBrush)bc.ConvertFromString("White");
                //            }
                //            else if (this.IconScheme == JhMessageBoxIcon.SecurityShieldGray)
                //            {
                //                imgIcon.Source = new BitmapImage(new Uri(@"pack://application:,,,/JhLib;Component/images/SecurityWarning.png"));
                //                //imgIcon.Source = Microsoft.SDK.Samples.VistaBridge.Library.StockIcons.StockIcons.Shield; // The yellow/blue shield thingy
                //                // This is a gradient that goes from #9D8F85 at the left, to #A49890 at the right.
                //                sLeftColor = "#9D8F85";
                //                sRightColor = "#A49890";
                //                txtSummaryText.Foreground = (SolidColorBrush)bc.ConvertFromString("White");
                //            }
                case JhMessageBoxType.Information:
                    if (_options.IsUsingNewerIcons)
                    {
                        _iconImageName = "InfoBlueDiamond_64x64.png";
                        _iconWidth = _iconHeight = 64;
                        _iconMarginLeft = _iconMarginRight = _iconMarginBottom = 0;
                        //                       minLeftMargin = 60;
                    }
                    else // use the old icon
                    {
                        _iconImageName = "iconInformationStd.png";
                        _iconWidth = _iconHeight = 32;
                        _iconMarginLeft = 4;
                        _iconMarginRight = 0;
                        _iconMarginBottom = 0;
                        //                        minLeftMargin = 37;
                    }
                    _upperRectangleVisibility = Visibility.Collapsed;
                    break;
                case JhMessageBoxType.Question:
                    if (_options.IsUsingNewerIcons)
                    {
                        _iconImageName = "QDiamond_64x64.png";
                        _iconWidth = _iconHeight = 82;
                        _iconMarginLeft = _iconMarginRight = _iconMarginBottom = 0;
                        //minLeftMargin = 77;
                    }
                    else // use the old icon
                    {
                        _iconImageName = "iconQuestionStd.png";
                        _iconWidth = _iconHeight = 32;
                        _iconMarginLeft = 4;
                        _iconMarginRight = _iconMarginBottom = 0;
                        //minLeftMargin = 37;
                    }
                    _upperRectangleVisibility = Visibility.Collapsed;
                    // Use black for question text.
                    _summaryTextForegroundColor = new SolidColorBrush(Colors.Black);
                    break;
                case JhMessageBoxType.UserMistake:
                    if (_options.IsUsingNewerIcons)
                    {
                        //TODO: This image needs to have it's background transparency adjusted.
                        _iconImageName = "Oops.png";
                        _iconWidth = _iconHeight = 65;
                        _iconMarginLeft = _iconMarginRight = _iconMarginBottom = 0;
                        //minLeftMargin = 60;
                    }
                    else // use the old icon
                    {
                        _iconImageName = "iconWarningStd.gif";
                        _iconWidth = 31;
                        _iconHeight = 28;
                        _iconMarginLeft = 4;
                        _iconMarginRight = 0;
                        _iconMarginBottom = 0;
                        //minLeftMargin = 34;
                    }
                    //F6C53A  CF6E17
                    if (texture != JhMessageBoxBackgroundTexture.BrownTexture2 && texture != JhMessageBoxBackgroundTexture.GrayMarble)
                    {
                        if (isDark)
                        {
                            _summaryTextForegroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F6C53A"));
                        }
                        else
                        {
                            _summaryTextForegroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CF6E17"));
                        }
                    }
                    _upperRectangleVisibility = Visibility.Collapsed;
                    break;
                case JhMessageBoxType.Warning:
                    if (_options.IsUsingNewerIcons)
                    {
                        _iconImageName = "warning3_64x64.png";
                        _iconWidth = _iconHeight = 64;
                        _iconMarginLeft = 3;
                        _iconMarginRight = 0;
                        _iconMarginBottom = 0;
                        //minLeftMargin = 66;
                    }
                    else // use the old icon
                    {
                        _iconImageName = "iconWarningStd.gif";
                        _iconWidth = 31;
                        _iconHeight = 28;
                        _iconMarginLeft = 4;
                        _iconMarginRight = 0;
                        _iconMarginBottom = 0;
                        //minLeftMargin = 34;
                    }
                    _upperRectangleVisibility = Visibility.Collapsed;
                    // Push it to pure blue for these two rather difficult backgrounds..
                    if (texture == JhMessageBoxBackgroundTexture.BrushedMetal || texture == JhMessageBoxBackgroundTexture.GreenTexture3)
                    {
                        //_summaryTextForegroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0000FF"));
                        _summaryTextForegroundColor = new SolidColorBrush(Colors.Blue);
                    }
                    break;
                case JhMessageBoxType.Error:
                    if (_options.IsUsingNewerIcons)
                    {
                        _iconImageName = "errorDiamond_48x48.png";
                        _iconWidth = _iconHeight = 48;
                        _iconMarginLeft = 1;
                        _iconMarginRight = 0;
                        _iconMarginBottom = 0;
                        //minLeftMargin = 48;
                    }
                    else // use the old icon
                    {
                        _iconImageName = "RedShield.png";
                        _iconWidth = 26;
                        _iconHeight = 32;
                        // This is a red gradient that goes from #AC0100 at the left, to #E30100 at the right.
                        sLeftColor = "#AC0100";
                        sRightColor = "#EB0100"; // "#E30100" is what I measured on the native messagebox color.
                        _iconMarginLeft = 3;
                        _iconMarginRight = 0;
                        _iconMarginBottom = 0;
                    }
                    // If there is no texture, then make the summary-text white to contrast against the red gradiant..
                    if (imageFilename == null)
                    {
                        _summaryTextForegroundColor = new SolidColorBrush(Colors.White);
                    }
                    else
                    {
                        if (texture == JhMessageBoxBackgroundTexture.BrushedMetal)
                        {
                            _summaryTextForegroundColor = new SolidColorBrush(Colors.White);
                        }
                        else
                        {
                            _summaryTextForegroundColor = new SolidColorBrush(Colors.Red);
                        }
                    }
                    break;
                case JhMessageBoxType.Stop:
                    if (_options.IsUsingNewerIcons)
                    {
                        _iconImageName = "hand64x64.png";
                        _iconWidth = _iconHeight = 64;
                        _iconMarginLeft = 0;
                        _iconMarginRight = 0;
                        _iconMarginBottom = 0;
                        //minLeftMargin = 60;
                    }
                    else // use the old icon
                    {
                        _iconImageName = "iconErrorStd.gif";
                        _iconWidth = _iconHeight = 32;
                        _iconMarginLeft = 5;
                        _iconMarginRight = 0;
                        _iconMarginBottom = 0;
                        //minLeftMargin = 40;
                    }
                    // If there is no texture, then make the summary-text white to contrast against the red gradiant..
                    if (imageFilename == null)
                    {
                        _summaryTextForegroundColor = new SolidColorBrush(Colors.White);
                    }
                    else
                    {
                        if (texture == JhMessageBoxBackgroundTexture.BrushedMetal)
                        {
                            _summaryTextForegroundColor = new SolidColorBrush(Colors.White);
                        }
                        else
                        {
                            _summaryTextForegroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3E29"));
                        }
                    }
                    break;
                default:  // no particular message-type
                    _iconWidth = 0;
                    _iconMarginLeft = _iconMarginTop = _iconMarginRight = _iconMarginBottom = 0;
                    _upperRectangleVisibility = Visibility.Collapsed;
                    break;
            } // end switch.

            //TODO: This is just tinkeriing..
            if (texture == JhMessageBoxBackgroundTexture.GreenTexture3)
            {
                imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/JhLib;Component/Images/" + imageFilename));
                imageBrush.ViewportUnits = BrushMappingMode.Absolute;
                imageBrush.Stretch = Stretch.None;
                imageBrush.Viewport = rectViewport;
                imageBrush.TileMode = TileMode.Tile;
                _buttonPanelBackground = new SolidColorBrush(Colors.Transparent);
                _summaryTextBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8E9E71"));
                _detailTextBackground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8E9E71"));
            }
            // Determine whether the icon extends into the bottom, detail-text region..
            bool isIconExtendingIntoDetailArea = false;
            if (_iconHeight > 38)
            {
                if (_iconHeight <= 48)
                {
                    _iconMarginTop = 5;
                }
                else
                {
                    _iconMarginTop = 0;
                }
                isIconExtendingIntoDetailArea = true;
            }
            else
            {
                _iconMarginLeft = 9;
                _iconMarginTop = 36 - _iconHeight;
            }
            if (_iconWidth > 32)
            {
                minLeftMargin = _iconWidth + 2;
            }
            else if (_iconWidth == 0)
            {
                minLeftMargin = 0;
            }
            else
            {
                minLeftMargin = _iconWidth - 4;
            }
            //            Console.WriteLine("minLeftMargin = " + minLeftMargin);

            #endregion Colors and Icon

            #region Set the color-gradiant rectangle

            // If there is no background-texture, then - if we have gradient colors defined (for that upper part)..
            if (imageFilename != null)
            {
                _upperRectangleVisibility = Visibility.Collapsed;
            }
            else if (sLeftColor != null && sRightColor != null)
            {
                // There is a left and right color defined, thus there is a gradiant to show.
#if SILVERLIGHT
                // Silverlight does not have it's own ColorConverter, so I had to provide one.
                var cc = new ColorConverter();
                colorLeft = (Color)cc.Convert(sLeftColor, typeof(Color), null, null);
                colorRight = (Color)cc.Convert(sRightColor, typeof(Color), null, null);
#else
                colorLeft = (Color)ColorConverter.ConvertFromString(sLeftColor);
                colorRight = (Color)ColorConverter.ConvertFromString(sRightColor);
#endif
                _gradiantLeftColor = colorLeft;
                _gradiantRightColor = colorRight;
                _upperRectangleVisibility = Visibility.Visible;
            }
            #endregion Set the color-gradiant rectangle

            #region Position the text

            // 77 is a reasonable number of characters if you only want this to shift to the left after two lines of text are filled.
            if (SummaryText.Length > 87)
            {
                _summaryTextHorizontalAlignment = HorizontalAlignment.Left;
                _summaryTextMargin = new Thickness(_iconMarginLeft + _iconWidth, 0, 0, 0);
            }
            else if (SummaryText.Length > 34)
            {
                // Shift it over partway, so it is still at least somewhat centered.
                //_summaryTextMargin = new Thickness(minLeftMargin + 20, 0, 0, 0);
                _summaryTextHorizontalAlignment = HorizontalAlignment.Center;
                _summaryTextMargin = new Thickness(_iconMarginLeft + _iconWidth, 0, 0, 0);
            }
            else
            {
                _summaryTextHorizontalAlignment = HorizontalAlignment.Center;
                if (DetailText.Length < 40)
                {
                    _detailTextMarginLeft = 0;
                    _summaryTextMargin = new Thickness(0, 0, 0, 0);
                }
                else
                {
                    _summaryTextMargin = new Thickness(_iconMarginLeft + _iconWidth, 0, 0, 0);
                }
            }

            _detailTextMarginLeft = minLeftMargin;
            // If there is no detail-text,
            if (String.IsNullOrWhiteSpace(this.DetailText))
            {
                // then let the summary-text span the entire height that is available for text.
                SummaryTextRowSpan = 2;
                _upperRectangleRowSpan = 2;
            }
            else // there is some detail-text
            {
                SummaryTextRowSpan = 1;
                _upperRectangleRowSpan = 1;
                // If the detail-text is too long to fit on one line, expand it to the left so that it fills the width of the entire message-box.
                if (isIconExtendingIntoDetailArea)
                {
                    if (SummaryText.Length < 34 && DetailText.Length < 40)
                    {
                        _detailTextMarginLeft = 0;
                    }
                    else
                    {
                        _detailTextMarginLeft = minLeftMargin;
                    }
                }
                else // the icon does not extend into the detail-text area
                {
                    if (DetailText.Length > 61)
                    {
                        _detailTextMarginLeft = 0;
                    }
                }
                // If the detail-text is long enough such that to center it no longer makes sense, set it to left-justify.
                if (DetailText.Length > 100)
                {
                    _detailTextHorizontalAlignment = HorizontalAlignment.Left;
                }

                if (DetailText.Length > 87 || SummaryText.Length > 60)
                {
                    _windowHeight = 192;
                }
                // If the (main) text is long enough to warrant more than one line,
                // make the text area left-justified instead of centered.
                if (DetailText.Length > 122)
                {
                    _detailTextVerticalAlignment = VerticalAlignment.Top;
                    _windowHeight = 220.0;
                }
            }
            //            Console.WriteLine("_detailTextHorizontalAlignment = " + _detailTextHorizontalAlignment);
            //            Console.WriteLine("_detailTextMarginLeft = " + _detailTextMarginLeft);
            #endregion Position the text

            #region Size and position the buttons

            _buttonCount = 0;
            if ((Options.ButtonFlags & JhMessageBoxButtons.Cancel) == JhMessageBoxButtons.Cancel)
            {
                _isCancelButtonVisible = true;
                _buttonCount++;
            }
            if ((Options.ButtonFlags & JhMessageBoxButtons.Close) == JhMessageBoxButtons.Close)
            {
                _isCloseButtonVisible = true;
                _buttonCount++;
            }
            if ((Options.ButtonFlags & JhMessageBoxButtons.Ignore) == JhMessageBoxButtons.Ignore)
            {
                _isIgnoreButtonVisible = true;
                _buttonCount++;
            }
            if ((Options.ButtonFlags & JhMessageBoxButtons.Ok) == JhMessageBoxButtons.Ok)
            {
                _isOkButtonVisible = true;
                _buttonCount++;
            }
            if ((Options.ButtonFlags & JhMessageBoxButtons.Retry) == JhMessageBoxButtons.Retry)
            {
                _isRetryButtonVisible = true;
                _buttonCount++;
            }
            if ((Options.ButtonFlags & JhMessageBoxButtons.Yes) == JhMessageBoxButtons.Yes)
            {
                _isYesButtonVisible = true;
                _buttonCount++;
            }
            if ((Options.ButtonFlags & JhMessageBoxButtons.No) == JhMessageBoxButtons.No)
            {
                _isNoButtonVisible = true;
                _buttonCount++;
            }

            // Button width by default had been 55, which works for 6 buttons.
            // If we only have a small subset of the available buttons showing, then give them a bit more breathing room..
            int n = _buttonCount;
            if (n < 5)
            {
                _buttonWidth = 80;
            }
            else if (n < 6)
            {
                _buttonWidth = 65;
            }
            else if (n < 7)
            {
                _buttonWidth = 57;
            }
            else
            {
                _buttonWidth = 49;
            }

            if (n < 4)
            {
                _buttonMargin = new Thickness(9.0, 2.0, 9.0, 2.0);
            }
            else if (n < 5)
            {
                _buttonMargin = new Thickness(4.0, 2.0, 5.0, 2.0);
            }
            else if (n < 6)
            {
                _buttonMargin = new Thickness(3.0, 2.0, 3.0, 2.0);
            }
            else if (n < 7)
            {
                _buttonMargin = new Thickness(2.0, 2.0, 2.0, 2.0);
            }
            else
            {
                _buttonMargin = new Thickness(1.5, 2.0, 1.5, 2.0);
            }
            #endregion Size and position the buttons

            _isInitialized = true;
        }
        #endregion

        #region fields

        private static JhMessageBoxViewModel _theInstance;
        /// <summary>
        /// This is the ImageBrush that gives the message-box Window it's background texture, if any. Default is null, indicating no image.
        /// </summary>
        private Brush _backgroundBrush;
        private Thickness _buttonMargin;
        private double _buttonMarginSide = 1.0;
        private int _buttonCount;
        private double _buttonWidth = 50.0;
        private Brush _buttonPanelBackground;
        private string _detailText;
        private Brush _detailTextBackground;
        private Thickness _detailTextMargin;
        private double _detailTextMarginLeft;
        private HorizontalAlignment _detailTextHorizontalAlignment = HorizontalAlignment.Center;
        private VerticalAlignment _detailTextVerticalAlignment = VerticalAlignment.Center;
        private Color _gradiantLeftColor = Colors.Yellow;
        private Color _gradiantRightColor = Colors.Green;
        /// <summary>
        /// the BitmapImage to use for the one optional icon-image that may be shown in the upper-left (not necessarily an "icon", technically).
        /// </summary>
        private BitmapImage _iconBitmapImage;
        /// <summary>
        /// The name of the bitmap file to use for the message-box's icon (just the filename itself - not the entire path).
        /// </summary>
        private string _iconImageName;
        private Thickness _iconMargin;
        private double _iconMarginLeft = 8;
        private double _iconMarginTop = 5;
        private double _iconMarginRight = 8;
        private double _iconMarginBottom = 0;
        private int _iconWidth;
        private int _iconHeight;
        /// <summary>
        /// This indicates whether the properties of this view-model have been set, as a function of the values within the Options.
        /// </summary>
        private bool _isInitialized;
        private JhMessageBoxOptions _options;
        private string _summaryText;
        private Brush _summaryTextBackground;
        private Brush _summaryTextForegroundColor;
        private HorizontalAlignment _summaryTextHorizontalAlignment = HorizontalAlignment.Center;
        private Thickness _summaryTextMargin = new Thickness(100, 0, 0, 0);
        /// <summary>
        /// This is the value for the Grid.RowSpan property on the summary-text TextBox. Usually it is 1, but we set it to 2 if there is no DetailText.
        /// </summary>
        private int _summaryTextRowSpan = 1;
        /// <summary>
        /// The value for the upper gradiant-color Rectangle's Grid.RowSpan property.
        /// This gets set to 2 if the detail-text region is not happenin.
        /// </summary>
        private int _upperRectangleRowSpan = 1;
        /// <summary>
        /// the text to be displayed in the message-box Window's title-bar.
        /// </summary>
        private string _title;
        private Visibility _upperRectangleVisibility;
        private double _windowHeight = 155;

        #endregion fields

        #endregion internal implementation
    }
    #endregion
}
