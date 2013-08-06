// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;


namespace JhLib
{
    #region class ColorComboBox
    /// <summary>
    /// This is a drop-in replacement for a normal WPF ComboBox
    /// that allows for selecting a color from a list of known colors.
    /// This is a migration/rename from the original WinForms-prototype's "ColorCombo"
    /// written by Ed Anderson.
    /// </summary>
    public class ColorComboBox : ComboBox, INotifyPropertyChanged
    {

        #region constructors

        public ColorComboBox()
            : base()
        {
            // A width of 120 seems to cover the longest string by itself (in English).
            this.Width = 148;
            this.Height = 26;

            // Create a DataTemplate for the items
            DataTemplate template = new DataTemplate(typeof(NamedColor));

            // Create a FrameworkElementFactory based on StackPanel
            FrameworkElementFactory factoryStack = new FrameworkElementFactory(typeof(StackPanel));
            factoryStack.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            // Make that the root of the DataTemplate visual tree.
            template.VisualTree = factoryStack;

            // Create a FrameworkElementFactory based on Rectangle.
            FrameworkElementFactory factoryRectangle = new FrameworkElementFactory(typeof(Rectangle));
            factoryRectangle.SetValue(Rectangle.WidthProperty, 16.0);
            factoryRectangle.SetValue(Rectangle.HeightProperty, 16.0);
            factoryRectangle.SetValue(Rectangle.MarginProperty, new Thickness(2));
            factoryRectangle.SetValue(Rectangle.StrokeProperty, SystemColors.WindowTextBrush);
            factoryRectangle.SetValue(Rectangle.FillProperty, new Binding("Brush"));

            // Add it to the StackPanel.
            factoryStack.AppendChild(factoryRectangle);

            // Create a FrameworkElementFactory based on TextBlock.
            FrameworkElementFactory factoryTextBlock = new FrameworkElementFactory(typeof(TextBlock));
            factoryTextBlock.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            factoryTextBlock.SetValue(TextBlock.TextProperty, new Binding("Name"));

            // Add it to the StackPanel.
            factoryStack.AppendChild(factoryTextBlock);

            // Set the ItemTemplate property to the template created above.
            this.ItemTemplate = template;

            // I'm setting this at the dropdown event, to save loading time.
            //this.ItemsSource = NamedColor.All;
            //this.DisplayMemberPath = "Name";
            this.SelectedValuePath = "Color";

            //DropDownOpened += new EventHandler(OnDropDownOpened);
            FillMyself();

            SelectionChanged += new SelectionChangedEventHandler(OnSelectionChanged);
        }

        static ColorComboBox()
        {
            ms_IsSortedByBrightnessProperty = DependencyProperty.Register("IsSortedByBrightness", typeof(bool), typeof(ColorComboBox));
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata();
            metadata.BindsTwoWayByDefault = true;
            metadata.PropertyChangedCallback = new PropertyChangedCallback(OnBrushChanged);
            metadata.DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            SelectedBrushProperty = DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(ColorComboBox), metadata);
        }
        #endregion

        private static void OnBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ColorComboBox thisComboBox = (ColorComboBox)d;
            SolidColorBrush newBrush = (SolidColorBrush)args.NewValue;
            Color thisColor = newBrush.Color;
            thisComboBox.SelectedColor = thisColor;
        }

        public Brush SelectedBrush
        {
            get
            {
                var theBrush = (SolidColorBrush)this.GetValue(SelectedBrushProperty);
                if (theBrush != null)
                {
                    return (SolidColorBrush)this.GetValue(SelectedBrushProperty);
                }
                else
                {
                    return _defaultBrush;
                }
            }
            set
            {
                this.SetValue(SelectedBrushProperty, value);
            }
        }


        #region SelectedNamedColor
        /// <summary>
        /// Gets or sets the selection as a NamedColor.
        /// If nothing is selected, then Get returns NamedColor.Empty.
        /// Use this or the other properties instead of SelectedItem.
        /// </summary>
        [Browsable(false)]
        public NamedColor SelectedNamedColor
        {
            get
            {
                if (this.SelectedItem != null)
                {
                    return (NamedColor)this.SelectedItem;
                }
                else if (this.SelectedValue != null)
                {
                    return NamedColor.From((Color)this.SelectedValue);
                }
                else
                {
                    return NamedColor.Empty;
                }
            }
            set
            {
                try
                {
                    // Transparent is used as a null value
                    if (value != NamedColor.Empty)
                    {
                        if (Items != null && Items.Count <= 0)
                        {
                            Items.Add(value);
                        }
                        //SelectedItem = value;
                        NamedColor namedColor = value;
                        //TODO: I'm getting a crash here!
                        if (namedColor != null)
                        {
                            SelectedValue = namedColor.Color;
                        }
                    }
                    else
                    {
                        // If the 'empty' color is specified, then set this to no selection.
                        SelectedIndex = -1;
                    }
                }
                catch (Exception x)
                {
                    Console.WriteLine("Exception in ColorComboBox.SelectedNamedColor.Set: " + x.Message);
                }
            }
        }
        #endregion

        #region SelectedColor
        /// <summary>
        /// Gets or sets the selected color. If nothing is selected, then Get returns transparent.
        /// Use this property and NOT SelectedItem.
        /// </summary>
        [Browsable(false)]
        public Color SelectedColor
        {
            get
            {
                if (this.SelectedValue != null)
                {
                    return (Color)this.SelectedValue;
                }
                else
                {
                    return Colors.Transparent;
                }
            }
            set
            {
                try
                {
                    // Transparent is used as a null value
                    if (value != Colors.Transparent)
                    {
                        NamedColor namedColor = NamedColor.From(value);
                        if (Items != null && Items.Count <= 0)
                        {
                            Items.Add(namedColor);
                        }
                        SelectedItem = namedColor;
                    }
                }
                catch (Exception x)
                {
                    Console.WriteLine("Exception in ColorComboBox.SelectedColor.Set: " + x.Message);
                }
            }
        }
        #endregion

        #region SelectedName
        /// <summary>
        /// Gets or sets the selected color-name.
        /// Use this property or SelectedColor in place of SelectedItem.
        /// </summary>
        [Browsable(false)]
        public string SelectedName
        {
            get
            {
                if (SelectedItem != null)
                {
                    NamedColor namedColor = (NamedColor)SelectedItem;
                    return namedColor.Name;
                }
                else
                {
                    //cbl Not. Transparent is used as a null value
                    return "";
                }
            }
            set
            {
                try
                {
                    // Transparent is used as a null value   cbl Not.
                    //if (value != "Transparent")
                    if (!String.IsNullOrEmpty(value))
                    {
                        NamedColor namedColor = NamedColor.From(value);
                        if (Items != null && Items.Count <= 0)
                        {
                            Items.Add(namedColor);
                        }
                        SelectedItem = namedColor;
                    }
                }
                catch (Exception x)
                {
                    Console.WriteLine("Exception in ColorComboBox.SelectedName.Set: " + x.Message);
                }
            }
        }
        #endregion

        #region IsSortedByBrightness

        public static readonly DependencyProperty ms_IsSortedByBrightnessProperty;

        /// <summary>
        /// Get or set whether the items are sorted in order of increasing brightness,
        /// as opposed to alphabetically.
        /// </summary>
        public bool IsSortedByBrightness
        {
            get { return (bool)GetValue(ms_IsSortedByBrightnessProperty); }
            set { SetValue(ms_IsSortedByBrightnessProperty, value); }
        }
        #endregion

        public void Notify(string sPropertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(sPropertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Internal Implementation

        /// <summary>
        /// Used so that the combobox is only filled once.
        /// </summary>
        private bool m_bFilledList;

        #region OnDropDownOpened
        /// <summary>
        /// Called when the list is dropped down. Fills the combo with colors if it's not already filled.
        /// </summary>
        void OnDropDownOpened(object sender, EventArgs e)
        {
            if (!m_bFilledList)
            {
                FillMyself();
            }
        }
        #endregion

        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (SelectedValue != null)
                {
                    Color color = (Color) SelectedValue;
                    if (color != null)
                    {
                        this.SelectedBrush = new SolidColorBrush(color);
                    }
                    Notify("SelectedBrush");
                }
            }
            catch (Exception x)
            {
                Console.WriteLine("Exception in ColorComboBox.OnSelectionChanged: " + x.Message);
            }
        }

        private void FillMyself()
        {
            try
            {
                //Console.WriteLine("ColorComboBox.FillMyself");
                Color selected = SelectedColor;
                Items.Clear();

                if (this.IsSortedByBrightness)
                {
                    List<NamedColor> colors = new List<NamedColor>();

                    // Loop through all the colors, putting them in a list to be sorted
                    foreach (NamedColor namedColor in NamedColor.AllWindowsMediaColors)
                    {
                        if (namedColor.Name != "Transparent")
                        {
                            colors.Add(namedColor);
                        }
                    }

                    // Sort the colors by brightness
                    colors.Sort(NamedColor.CompareLuminance);

                    // Put'm in da box..
                    foreach (NamedColor namedColor in colors)
                    {
                        this.Items.Add(namedColor);
                    }
                }
                else
                {
                    // This is the simple, un-sorted version.
                    this.ItemsSource = NamedColor.AllWindowsMediaColors;
                }
                SelectedColor = selected;
                m_bFilledList = true;
            }
            catch (Exception x)
            {
                Console.WriteLine("Exception in ColorComboBox.FillMyself: " + x.Message);
            }
        }

        #region fields

        public static readonly DependencyProperty SelectedBrushProperty;

        //TODO: The default color needs to be a settable value!
        private Brush _defaultBrush = new SolidColorBrush(Colors.Black);

        #endregion fields

        #endregion Internal Implementation
    }
    #endregion
}
