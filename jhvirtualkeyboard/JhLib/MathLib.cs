// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Globalization;
using System.Windows;


namespace JhLib
{
    /// <summary>
    /// A static class for holding a few misc mathematical utilities
    /// </summary>
    public static class MathLib
    {
        #region IsValidNumericInput
        /// <summary>
        /// Return true if the given text represents a valid number, in the current culture.
        /// </summary>
        /// <param name="text">the string input to test</param>
        /// <returns>true only if the given text expresses a number</returns>
        public static bool IsValidNumericInput(this string text)
        {
            string str = text.Trim();
            if (str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.NegativeSign |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.NumberGroupSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentDecimalSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentGroupSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentSymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PerMilleSymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PositiveSign)
            {
                return true;
            }
            foreach (Char c in str)
            {
                if (!Char.IsNumber(c))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region IsInRange
        /// <summary>
        /// Return true if this value (x) is within the range denoted by the two parameters X1,x2 regardless of their order or whether they're negative.
        /// </summary>
        /// <param name="x">this numeric value to compare against the range limits</param>
        /// <param name="x1">one range limit (may be greater or less than the other limit)</param>
        /// <param name="x2">the other range limit</param>
        /// <returns></returns>
        public static bool IsInRange(this double x, double x1, double x2)
        {
            double lowerLimit = Math.Min(x1, x2);
            double upperLimit = Math.Max(x1, x2);
            return (x >= lowerLimit && x <= upperLimit);
        }
        #endregion

        #region IsEssentiallyEqual

        public static bool IsEssentiallyEqual(Size sizeValue1, Size sizeValue2)
        {
            bool bResult = false;
            // If both are Empty, then consider them equal
            if (sizeValue1.IsEmpty)
            {
                bResult = sizeValue2.IsEmpty;
            }
            else if (sizeValue2.IsEmpty)
            {
                bResult = sizeValue1.IsEmpty;
            }
            else
            {
                bResult = IsEssentiallyEqual(sizeValue1.Width, sizeValue2.Width) && IsEssentiallyEqual(sizeValue1.Height, sizeValue2.Height);
            }
            return bResult;
        }

        /// <summary>
        /// Return true if the two numeric values are essentially equal in value,
        /// ie within about Double.Epsilon of each other.
        /// </summary>
        /// <param name="x">one value to compare</param>
        /// <param name="y">the other value to compare against it</param>
        /// <returns>true if they're within Double.Epsilon of each other</returns>
        public static bool IsEssentiallyEqual(this double x, double y)
        {
            return Math.Abs(x - y) <= Double.Epsilon;
            //double epsilon = 0.0001;
            //return (y - epsilon) < x && x < (y + epsilon);
        }
        #endregion

        #region IsEssentiallyEqualToOrLessThan
        /// <summary>
        /// Return true if the first numeric values is less than the second, or they're essentially equal,
        /// ie within about 0.0001 of each other.
        /// </summary>
        /// <param name="x">one value to compare</param>
        /// <param name="y">the other value to compare against it</param>
        /// <returns>true if x is equal to or less than y</returns>
        public static bool IsEssentiallyEqualToOrLessThan(this double x, double y)
        {
            double epsilon = 0.0001;
            return x < (y + epsilon);
        }
        #endregion

        #region IsEssentiallyEqualToOrGreaterThan
        /// <summary>
        /// Return true if the first numeric values is greater than the second, or they're essentially equal,
        /// ie within about 0.0001 of each other.
        /// </summary>
        /// <param name="x">one value to compare</param>
        /// <param name="y">the other value to compare against it</param>
        /// <returns>true if x is equal to or greater than y</returns>
        public static bool IsEssentiallyEqualToOrGreaterThan(this double x, double y)
        {
            double epsilon = 0.0001;
            return x > (y - epsilon);
        }
        #endregion

        #region GetDecimalPlaces
        /// <summary>
        /// An extension method that counts and returns the number of decimal places in this numeric value
        /// </summary>
        public static int GetDecimalPlaces(this double value)
        {
            const int MAX_DECIMAL_PLACES = 10;
            double THRESHOLD = Math.Pow(0.1, 10);
            if (value == 0.0)
            {
                return 0;
            }
            int nDecimal = 0;
            while (value - Math.Floor(value) > THRESHOLD && nDecimal < MAX_DECIMAL_PLACES)
            {
                value *= 10.0;
                nDecimal++;
            }
            return nDecimal;
        }
        #endregion

        #region RoundUpToNearestUnit, RoundDownToNearestUnit
        /// <summary>
        /// An extension method that rounds this numeric value up to the nearest value in unitQuantity steps.
        /// </summary>
        /// <param name="unitQuantity">the step-size to round to</param>
        /// <returns>value rounded up to the nearest unitQuantity step</returns>
        public static double RoundUpToNearestUnit(this double value, double unitQuantity)
        {
            const int ROUND_DP = 10;
            double valueAsMeasuredInUnits = value / unitQuantity;
            valueAsMeasuredInUnits = Math.Round(valueAsMeasuredInUnits, ROUND_DP);
            valueAsMeasuredInUnits = Math.Ceiling(valueAsMeasuredInUnits);
            return (valueAsMeasuredInUnits * unitQuantity);
        }

        /// <summary>
        /// An extension method that rounds this double value down to the nearest value in unitQuantity steps.
        /// </summary>
        /// <param name="unitQuantity">the step-size to round to</param>
        /// <returns>value rounded down to the nearest unitQuantity step</returns>
        public static double RoundDownToNearestUnit(this double value, double unitQuantity)
        {
            const int ROUND_DP = 10;
            double valueAsMeasuredInUnits = value / unitQuantity;
            valueAsMeasuredInUnits = Math.Round(valueAsMeasuredInUnits, ROUND_DP);
            valueAsMeasuredInUnits = Math.Floor(valueAsMeasuredInUnits);
            return (valueAsMeasuredInUnits * unitQuantity);
        }
        #endregion

        #region WithPeriodDelimiters
        /// <summary>
        /// Given a numeric value, return it as a string with the periods (groups of three digits)
        /// delimited by commas.
        /// </summary>
        /// <param name="value">the numeric value to convert to a string</param>
        /// <returns>a string representation of that numeric value</returns>
        public static string WithPeriodDelimiters(double value)
        {
            // Compute the nbr of decimal places needed.
            int nDecimalPlaces = value.GetDecimalPlaces();
            string formatString = "N" + nDecimalPlaces.ToString();
            return value.ToString(formatString, new CultureInfo("en-US"));
        }
        #endregion
    }
}
