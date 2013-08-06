// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhVirtualKeyboard</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Diagnostics;  // IValueConverter
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace JhVirtualKeyboard
{
    #region KvmToTextConverter
    //public class KvmToTextConverter : IValueConverter
    //{
    //    public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
    //    {
    //        Console.WriteLine("enter KvmToTextConverter.Convert");
    //        //if (targetType != typeof(string))
    //        //{
    //        //   throw new InvalidOperationException("The target for a CapsTextConverter must be a String!");
    //        //}
    //        string r = "?";
    //        if (value == null)
    //        {
    //            r = "null";
    //            Console.WriteLine("value is null.");
    //        }
    //        else
    //        {
    //            KeyViewModel kvm = value as KeyViewModel;
    //            if (kvm == null)
    //            {
    //                Console.WriteLine("unabled to cast value to KeyViewModel.");
    //            }
    //            else
    //            {
    //                if (String.IsNullOrEmpty(_keyChar))
    //                {
    //                    r = kvm.TextUnshifted;
    //                    Console.WriteLine("empty KeyChar, using TextUnshifted value of " + r);
    //                }
    //                else if (_keyChar.Equals("Shifted"))
    //                {
    //                    r = kvm.TextShifted;
    //                    Console.WriteLine("using TextShifted value of " + r);
    //                }
    //                else if (_keyChar.Equals("AltGr"))
    //                {
    //                    r = kvm.TextAltGr;
    //                    Console.WriteLine("using TextAltGr value of " + r);
    //                }
    //                else
    //                {
    //                    r = kvm.TextUnshifted;
    //                    Console.WriteLine("Unrecognized KeyChar, using TextUnshifted value of " + r);
    //                }
    //            }
    //        }
    //        if (r == null)
    //        {
    //            return DependencyProperty.UnsetValue;
    //            // or else return Binding.DoNothing. What's the difference?
    //            // see http://msdn.microsoft.com/en-us/library/system.widnows.data.ivalueconverter.convert.aspx
    //        }
    //        else
    //        {
    //            return r;
    //        }
    //    }

    //    public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotSupportedException();
    //    }

    //    #region KeyChar property

    //    public string KeyChar
    //    {
    //        get { return _keyChar; }
    //        set { _keyChar = value; }
    //    }
    //    private string _keyChar;

    //    #endregion
    //}
    #endregion KvmToTextConverter

    #region CapsLockToTextConverter
    /// <summary>
    /// Converts a boolean to a string that expresses the state of the Caps Lock button
    /// </summary>
    /// <example>
    /// If CapsLock is on => returns "Caps ON" otherwise just "Caps"
    /// <code>
    /// Text="{Binding IsCapsLock, Converter=CapsLockToTextConverter}"
    /// </code>
    /// </example>
    public class CapsLockToTextConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            //if (targetType != typeof(string))
            //{
            //   throw new InvalidOperationException("The target for a CapsTextConverter must be a String!");
            //}
            bool bCapsLockOn = (bool)value;
            // For Spanish (see http://en.wikipedia.org/wiki/Keyboard_layout#Spanish_.28Spain.29.2C_aka_Spanish_.28International_sort.29
            // Caps Lock would be  "Bloq Mayús" 
            if (bCapsLockOn)
            {
                return "Caps ON";
            }
            else
            {
                return "Caps";
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    #endregion CapsTextConverter

    #region ShiftLockToTextConverter
    /// <summary>
    /// Converts a boolean to a string that expresses the state of the Shift Lock button
    /// </summary>
    /// <example>
    /// If ShiftLock is on => returns "Shift ON" otherwise just "Shift"
    /// <code>
    /// Text="{Binding IsShiftLock, Converter=ShiftLockToTextConverter}"
    /// </code>
    /// </example>
    public class ShiftLockToTextConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            bool bShiftLockOn = (bool)value;
            if (bShiftLockOn)
            {
                return "Shift ON";
            }
            else
            {
                return "Shift";
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    #endregion ShiftLockToTextConverter

    #region WhichKBLayoutToStringConverter
    /// <summary>
    /// Converts between a WhichKeyboardLayout enum value and it's string equivalent
    /// </summary>
    public class WhichKBLayoutToStringConverter : IValueConverter
    {
        #region Convert
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string sValue = "?";
            try
            {
                //Console.WriteLine("in WhichKBLayoutToStringConverter, targetType is " + targetType.ToString() + ", value.ToString yields " + value.ToString());
                sValue = value.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an exception within WhichKBLayoutToStringConverter: " + e.Message);
            }
            return sValue;
        }
        #endregion

        #region ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                Debug.WriteLine("In WhichKBLayoutToStringConverter.ConvertBack, value is null.");
                return null;
            }
            WhichKeyboardLayout keyboardLayout = WhichKeyboardLayout.Unknown;
            if (!Enum.TryParse((String)value, out keyboardLayout))
            {
                Console.Write("in WhichLanguageToStringConverter.ConvertBack, failed to parse \"");
                Console.WriteLine(value.ToString() + "\".");
            }
            else
            {
                Debug.WriteLine("in WhichKBLayoutToStringConverter.ConvertBack, converted to " + keyboardLayout);
            }
            return keyboardLayout;
        }
        #endregion
    }
    #endregion

    #region BoolToBoldConverter
    public class BoolToBoldConverter : IValueConverter
    {
        #region Convert
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FontWeight fontWeight = FontWeights.Normal;
            try
            {
                bool bValue = true;
                if (bool.TryParse(value.ToString(), out bValue))
                {
                    if (bValue)
                    {
                        fontWeight = FontWeights.ExtraBold;
                    }
                }
            }
            catch (Exception x)
            {
                Debug.WriteLine("There was an exception within BoolToBoldConverter: " + x.Message);
            }
            return fontWeight;
        }
        #endregion

        #region ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
        #endregion
    }
    #endregion
}
