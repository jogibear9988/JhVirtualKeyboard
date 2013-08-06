using System;
using System.Windows;
using System.Windows.Controls;


namespace JhLib
{
    // This module is courtesy of the excellent article by Gary R. Wheeler at
    // http://www.codeproject.com/Articles/61725/WPF-radio-buttons-and-enumeration-values?msg=4333474#xx4333474xx
    // jh

    public class EnumRadioButton : RadioButton
    {
        public EnumRadioButton()
        {
            Loaded += OnLoaded;
            Checked += OnChecked;
        }

        private void OnLoaded(object sender, RoutedEventArgs event_arguments)
        {
            SetChecked();
        }

        private void OnChecked(object sender, RoutedEventArgs event_arguments)
        {
            if (IsChecked == true)
            {
                object binding = EnumBinding;

                if ((binding is Enum) && (EnumValue != null))
                {
                    try
                    {
                        EnumBinding = Enum.Parse(binding.GetType(), EnumValue);
                    }
                    catch (ArgumentException exception)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            string.Format(
                                "EnumRadioButton [{0}]: " +
                                "EnumBinding = {1}, " +
                                "EnumValue = {2}, " +
                                "ArgumentException {3}",
                                Name,
                                EnumBinding,
                                EnumValue,
                                exception));

                        throw;
                    }
                }
            }
        }

        private void SetChecked()
        {
            object binding = EnumBinding;

            if ((binding is Enum) && (EnumValue != null))
            {
                try
                {
                    object value = Enum.Parse(binding.GetType(), EnumValue);

                    IsChecked = ((Enum)binding).CompareTo(value) == 0;
                }
                catch (ArgumentException exception)
                {
                    System.Diagnostics.Debug.WriteLine(
                        string.Format(
                            "EnumRadioButton [{0}]: " +
                            "EnumBinding = {1}, " +
                            "EnumValue = {2}, " +
                            "ArgumentException {3}",
                            Name,
                            EnumBinding,
                            EnumValue,
                            exception));

                    throw;
                }
            }
        }

        static EnumRadioButton()
        {
            FrameworkPropertyMetadata enum_binding_metadata = new FrameworkPropertyMetadata();

            enum_binding_metadata.BindsTwoWayByDefault = true;
            enum_binding_metadata.PropertyChangedCallback = OnEnumBindingChanged;

            EnumBindingProperty = DependencyProperty.Register("EnumBinding",
                                                               typeof(object),
                                                               typeof(EnumRadioButton),
                                                               enum_binding_metadata);

            EnumValueProperty = DependencyProperty.Register("EnumValue",
                                                               typeof(string),
                                                               typeof(EnumRadioButton));
        }

        public static readonly DependencyProperty EnumBindingProperty;

        private static void OnEnumBindingChanged(DependencyObject dependency_object,
                                                 DependencyPropertyChangedEventArgs event_arguments)
        {
            if (dependency_object is EnumRadioButton)
            {
                ((EnumRadioButton)dependency_object).SetChecked();
            }
        }

        public object EnumBinding
        {
            set
            {
                SetValue(EnumBindingProperty, value);
            }

            get
            {
                return (object)GetValue(EnumBindingProperty);
            }
        }

        public static readonly DependencyProperty EnumValueProperty;

        public string EnumValue
        {
            set
            {
                SetValue(EnumValueProperty, value);
            }

            get
            {
                return (string)GetValue(EnumValueProperty);
            }
        }
    }
}
