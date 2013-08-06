// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Collections.Generic;
using System.Text;


namespace JhLib
{
    /// <summary>
    /// This class is sort of like a nullable boolean, but is intended
    /// to be suitable as a view-model for a WPF or Silverlight ComboBox or for saving in the Windows Registry.
    /// </summary>
    public struct TriStateBoolean : IEquatable<TriStateBoolean>
    {
        #region constructor
        /// <summary>
        /// Create a TriStateBoolean struct based upon the given numericValue.
        /// </summary>
        /// <param name="numericValue">The number that will be this object's NumericValue</param>
        private TriStateBoolean(int numericValue)
        {
            _numericValue = numericValue;
        }
        #endregion

        #region AllValues
        /// <summary>
        /// Get a List of the three possible instances of this class,
        /// as for binding to the ItemsSource property of a ComboBox.
        /// </summary>
        public static List<TriStateBoolean> AllValues
        {
            get
            {
                if (_allValues == null)
                {
                    _allValues = new List<TriStateBoolean>();
                    _allValues.Add(NoValue);
                    _allValues.Add(OverrideTrue);
                    _allValues.Add(OverrideFalse);
                }
                return _allValues;
            }
        }
        #endregion

        #region From
        /// <summary>
        /// Given a number, return the TriStateBoolean which has that NumericValue.
        /// </summary>
        /// <param name="id">The NumericValue - which hopefully is 0, 1, or 2</param>
        /// <returns>the TriStateBoolean which has that NumericValue</returns>
        public static TriStateBoolean From(int id)
        {
            switch (id)
            {
                case 1:
                    return OverrideTrue;
                case 2:
                    return OverrideFalse;
                default:
                    return NoValue;
            }
        }

        /// <summary>
        /// Given a nullable boolean value, return the TriStateBoolean that corresponds to it.
        /// </summary>
        /// <param name="nullableBooleanValue">The value to create the new TriStateBoolean object from</param>
        /// <returns>NoValue if the argument is null, OverrideTrue if true, OverrideFalse if false</returns>
        public static TriStateBoolean From(bool? nullableBooleanValue)
        {
            if (nullableBooleanValue.HasValue)
            {
                if (nullableBooleanValue.Value == true)
                {
                    return OverrideTrue;
                }
                else
                {
                    return OverrideFalse;
                }
            }
            else
            {
                return NoValue;
            }
        }
        #endregion

        #region HasValue
        /// <summary>
        /// Get whether this object as an explicit value, as opposed to being TriStateBoolean.NoValue.
        /// </summary>
        public bool HasValue
        {
            get { return NumericValue != 0; }
        }
        #endregion

        #region IsTrue
        /// <summary>
        /// Get whether this object represents an explicitly-set value of True.
        /// </summary>
        public bool IsTrue
        {
            get
            {
                return NumericValue == 1;
            }
        }
        #endregion

        #region IsFalse
        /// <summary>
        /// Get whether this object represents an explicitly-set value of False.
        /// </summary>
        public bool IsFalse
        {
            get
            {
                return NumericValue == 2;
            }
        }
        #endregion

        #region Name
        /// <summary>
        /// Get the display-text associated with this object, which is "No Override", "Yes", or "No".
        /// </summary>
        public string Name
        {
            get
            {
                string name;
                if (_numericValue == 1)
                {
                    name = _displayStringForTrue;
                }
                else if (_numericValue == 2)
                {
                    name = _displayStringForFalse;
                }
                else
                {
                    name = _displayStringForNull;
                }
                return name;
            }
        }
        #endregion

        #region NumericValue
        /// <summary>
        /// Get the number that identifies this object, which can only be 0, 1, or 2.
        /// </summary>
        public int NumericValue
        {
            get { return _numericValue; }
        }
        #endregion

        #region NullableValue
        /// <summary>
        /// Get or set the nullable-boolean value that this object represents.
        /// </summary>
        public bool? NullableValue
        {
            get
            {
                if (NumericValue == 1)
                {
                    return true;
                }
                else if (NumericValue == 2)
                {
                    return false;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region SetDisplayStrings
        /// <summary>
        /// Set what the names are for the three possible values, that is - what to display in a ComboBox
        /// if that is bound to the AllValues property.
        /// </summary>
        /// <param name="forFalse">what to display for the False value</param>
        /// <param name="forTrue">what to display for the True value</param>
        /// <param name="forNull">what to display for the Null value</param>
        public void SetDisplayStrings(string forFalse, string forTrue, string forNull)
        {
            _displayStringForFalse = forFalse;
            _displayStringForTrue = forTrue;
            _displayStringForNull = forNull;
        }
        #endregion

        #region ToString
        /// <summary>
        /// Override the ToString method to render a more useful display of this object.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder("TriStateBoolean(");
            sb.Append("Name=");
            sb.Append(this.Name);
            sb.Append(",#=");
            sb.Append(NumericValue);
            sb.Append(",");
            if (_numericValue == 2)
            {
                sb.Append("False");
            }
            else if (_numericValue == 1)
            {
                sb.Append("True");
            }
            else
            {
                sb.Append("null");
            }
            sb.Append(")");
            return sb.ToString();
        }
        #endregion

        #region Implementation of IEquatable

        public bool Equals(TriStateBoolean otherValue)
        {
            // http://www.infoq.com/articles/Equality-Overloading-DotNET
            return this._numericValue == otherValue._numericValue;
        }

        public override bool Equals(object obj)
        {
            if (obj is TriStateBoolean)
            {
                return this.Equals((TriStateBoolean)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _numericValue.GetHashCode();
        }

        public static bool operator ==(TriStateBoolean value1, TriStateBoolean value2)
        {
            return value1.Equals(value2);
        }

        public static bool operator !=(TriStateBoolean value1, TriStateBoolean value2)
        {
            return !value1.Equals(value2);
        }

        #endregion Implementation of IEquatable

        #region fields

        private readonly int _numericValue;
        private static string _displayStringForFalse = "No";
        private static string _displayStringForTrue = "Yes";
        private static string _displayStringForNull = "No Value";
        public static TriStateBoolean NoValue = new TriStateBoolean(0);
        public static TriStateBoolean OverrideTrue = new TriStateBoolean(1);
        public static TriStateBoolean OverrideFalse = new TriStateBoolean(2);
        private static List<TriStateBoolean> _allValues;

        #endregion fields
    }
}
