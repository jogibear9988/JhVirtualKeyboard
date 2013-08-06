// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace JhLib
{
    /// <summary>
    /// StringLib is our library of purely string-related utilities.
    /// </summary>
    public static class StringLib
    {
        #region AsExponentialIfLargeOrSmall
        /// <summary>
        /// Formats the given numeric value as a string and returns it,
        /// formatting it as exponential if it's very small or very large.
        /// </summary>
        /// <param name="fValue">The numeric value to format as a string</param>
        /// <returns>a string representation of the given numeric value</returns>
        static public string AsExponentialIfLargeOrSmall(double fValue)
        {
            string sValueAsString;
            // If the offset is between .001 and 1,000,000, inclusive
            // then we probably don't need to show it in exponential format.
            if (fValue >= 0.001 && fValue <= 1000000)
            {
                sValueAsString = String.Format("{0}", fValue.ToString("F4"));
            }
            else
            {
                sValueAsString = String.Format("{0}", fValue.ToString("E"));
            }
            return sValueAsString;
        }
        #endregion

        #region AsNonNullString
        /// <summary>
        /// This is an extension method that simply returns the given thing converted to a string, in general using ToString
        /// but in a little more robust way.
        /// It produces a useful result even if the given object is null, an array, or a nullable type.
        /// This is used where you want to display what the object is, but don't want your code to choke on a null value.
        /// </summary>
        /// <param name="someObject">The given object to return expressed as a string</param>
        /// <returns>the given object expressed as a string</returns>
        public static string AsNonNullString(this object someObject)
        {
            string resultString;
            if (someObject == null)
            {
                resultString = "null";
            }
            else
            {
                string stringObject = someObject as String;
                if (stringObject == null)
                {
                    Type thisType = someObject.GetType();
                    if (thisType.IsArray)
                    {
                        System.Array systemArray = someObject as System.Array;
                        int len = systemArray.Length;
                        var sb = new StringBuilder();
                        string typeName = thisType.Name;
                        string elementTypeAsString = AsInnerType(someObject);
                        sb.Append(elementTypeAsString);
                        sb.Append("[");
                        sb.Append(len);
                        sb.Append("]");
                        if (len > 0)
                        {
                            sb.Append(" = {");
                            for (int i = 0; i < len; i++)
                            {
                                object o = systemArray.GetValue(i);
                                if (o == null)
                                {
                                    sb.Append("null");
                                }
                                else
                                {
                                    sb.Append(o.ToString());
                                }
                                if (i < len - 1)
                                {
                                    sb.Append(", ");
                                }
                            }
                            sb.Append("}");
                        }
                        resultString = sb.ToString();
                    }
                    else
                    {
                        resultString = someObject.ToString();
                    }
                }
                else if (stringObject == String.Empty)
                {
                    resultString = "Empty";
                }
                else if (String.IsNullOrWhiteSpace(stringObject))
                {
                    resultString = "whitespace";
                }
                else
                {
                    resultString = stringObject.WithinDoubleQuotes();
                }
            }
            return resultString;
        }
        #endregion

        #region AsSimplerTypeString
        /// <summary>
        /// Return a string representation of the given C# Type, shortened in a few ways.
        /// </summary>
        /// <param name="aType"></param>
        /// <returns></returns>
        public static string AsSimplerTypeString(Type aType)
        {
            string s = aType.ToString();
            if (s.StartsWith("System."))
            {
                s = s.Substring(7);
            }
            if (s.Equals("Boolean"))
            {
                s = "bool";
            }
            if (s.Equals("Int32"))
            {
                s = "int";
            }
            return s;
        }
        #endregion

        #region AsInnerType
        /// <summary>
        /// Return a somewhat-simplifed string representation of the type of the given object.
        /// "System" is stripped, Boolean is "bool", Int32 is "int", and Nullables are represented with "?".
        /// </summary>
        /// <param name="thisObject">the Object to give the type of</param>
        /// <returns>a simplifed representation of the type of the given object</returns>
        public static string AsInnerType(Object thisObject)
        {
            Type thisType = thisObject.GetType();
            Type elementType = thisType.GetElementType();
            string typeName = elementType.Name;
            string s = typeName;
            if (s.StartsWith("Nullable"))
            {
                Type underlyingType = Nullable.GetUnderlyingType(elementType);
                if (underlyingType != null)
                {
                    s = AsSimplerTypeString(underlyingType) + "?";
                }
            }
            else
            {
                s = AsSimplerTypeString(elementType);
            }
            return s;
        }
        #endregion

        #region Titlecase
        /// <summary>
        /// This is an extension-method that returns the given string with the first character in uppercase, and the rest in lowercase.
        /// </summary>
        /// <param name="sText">the string to return as first-character-capitalized</param>
        /// <returns>the given string with only the first character capitalized</returns>
        /// <remarks>
        /// This method is named after the equivalent string function in Python.
        /// This IS completely unit-tested.
        /// </remarks>
        public static string Titlecase(this string sText)
        {
            string sResult = "";
            if (!String.IsNullOrEmpty(sText))
            {
                string sFirstChar = sText.Substring(0, 1).ToUpper();
                string sRest = sText.Substring(1).ToLower();
                sResult = sFirstChar + sRest;
            }
            return sResult;
        }
        #endregion

        #region CharacterDecription
        /// <summary>
        /// Provides a spelled-out name for the given character, if possible,
        /// otherwise expresses it as the Unicode codepoint value.
        ///  The intent is to identify it in an obvious human-readable form.
        /// </summary>
        /// <param name="thisCharacter">The character to supply a name for</param>
        /// <returns>The name of the given character if known, otherwise the UNICODE codepoint</returns>
        /// <remarks>This is intended for responding to the user concerning something he/she has typed into an English keyboard,
        /// thus the non-English characters are not accounted for here. Other alphabets yet need to be implemented.</remarks>
        public static string CharacterDecription(this Char thisCharacter)
        {
            string sName = "?";
            if (Char.IsDigit(thisCharacter))
            {
                sName = "digit " + thisCharacter.ToCardinal();
            }
            else if (Char.IsLetter(thisCharacter))
            {
                sName = "letter \"" + thisCharacter + "\"";
            }
            else
            {
                // These are presented in (roughly) the order as they appear on my keyboard, top-to-bottom, left-to-right.
                switch (thisCharacter)
                {
                    case '`':
                        return "grave accent";
                    case '~':
                        return "tilde";
                    case '!':
                        return "exclamation-mark";
                    case '@':
                        return "at-sign";
                    case '#':
                        return "pound-sign";
                    case '$':
                        return "dollar-sign";
                    case '%':
                        return "percentage-sign";
                    case '^':
                        return "caret or circumflex accent";
                    case '&':
                        return "ampersand";
                    case '*':
                        return "asterisk";
                    case '(':
                        return "left parenthesis";
                    case ')':
                        return "right parenthesis";
                    case '-':
                        return "hyphen";
                    case '_':
                        return "underscore";
                    case '=':
                        return "equal-sign";
                    case '+':
                        return "plus-sign";
                    case '\t':
                        return "tab";
                    case '[':
                        return "left bracket";
                    case '{':
                        return "left brace";
                    case ']':
                        return "right bracket";
                    case '}':
                        return "right brace";
                    case '\\':
                        return "back-slash";
                    case '|':
                        return "vertical bar";
                    case ';':
                        return "semicolon";
                    case ':':
                        return "colon";
                    case '\'':
                        return "single-quote";
                    case '\"':
                        return "double-quote";
                    case '\r':
                        return "carriage-return";
                    case '\n':
                        return "newline";
                    case ',':
                        return "comma";
                    case '<':
                        return "less-than";
                    case '.':
                        return "period";
                    case '>':
                        return "greater-than";
                    case '/':
                        return "forward-slash";
                    case '?':
                        return "question-mark";
                    case ' ':
                        return "space";
                    default:
                        int n = Convert.ToInt32(thisCharacter);
                        if (n > 127)
                        {
                            sName = "(UNICODE codepoint " + n.ToString() + ")";
                        }
                        else
                        {
                            if (Char.IsPunctuation(thisCharacter))
                            {
                                sName = "punctuation-mark \"" + thisCharacter + "\"";
                            }
                            else if (Char.IsControl(thisCharacter))
                            {
                                sName = "(ASCII control-code " + n.ToString() + ")";
                            }
                            else
                            {
                                sName = "(ASCII code " + n.ToString() + ")";
                            }
                        }
                        break;
                }
            }
            return sName;
        }
        #endregion CharacterDecription

        #region CharacterDecriptions
        /// <summary>
        /// Given an arbitrary string, return another string that expresses the characters of that given string
        /// in plain English, with the Unicode code-points for non-obvious characters.
        /// </summary>
        /// <param name="s">The string to express</param>
        /// <returns>A string detailing the content of the given string</returns>
        /// <remarks>This is intended for giving feedback to a user regarding what he typed into a field,
        /// so it's only concerned with the normal keyboard-characters at this point.</remarks>
        static public string CharacterDecriptions(this string s)
        {
            string sR;
            if (!String.IsNullOrEmpty(s))
            {
                if (s.Length == 1)
                {
                    sR = "{ " + CharacterDecription(s[0]) + " }";
                }
                else
                {
                    int n = s.Length;
                    var sb = new StringBuilder("{");
                    for (int i = 0; i < n; i++)
                    {
                        sb.Append(CharacterDecription(s[i]));
                        if (i >= 0 && i < n - 1)
                        {
                            sb.Append(", ");
                        }
                    }
                    sb.Append("}");
                    sR = sb.ToString();
                }
            }
            else
            {
                sR = "(empty-string)";
            }
            return sR;
        }
        #endregion

        #region ConcatLeftToRight
        /// <summary>
        /// Concatenate this string with another string, in a left-to-right FlowDirection.
        /// The need for this arises due to the fact that the normal C# string-concatenation methods insist upon falling back to a different order
        /// for, for example, Arabic characters.
        /// </summary>
        /// <param name="sLeft">the string to append to</param>
        /// <param name="sRight">the string to be appended</param>
        /// <returns></returns>
        public static string ConcatLeftToRight(this string sLeft, string sRight)
        {
            // We'll do this on a character-by-character basis.
            int nLeftLength = sLeft.Length;
            int nRightLength = sRight.Length;
            int nNewLength = nLeftLength + nRightLength;
            char[] aryResult = new char[nNewLength];
            for (int i = 0; i < nNewLength; i++)
            {
                if (i < nLeftLength)
                {
                    aryResult[i] = sLeft[i];
                }
                else
                {
                    aryResult[i] = sRight[i - nLeftLength];
                }
            }
            return new string(aryResult);
        }
        #endregion

        #region ContainsDigits
        public static Tuple<bool, string, int> ContainsDigit(this string thisText)
        {
            if (thisText == null)
            {
                throw new ArgumentNullException("thisText must not be null");
            }
            bool foundADigit = false;
            string whatWasFound = String.Empty;
            int indexOfDigit = -1;
            int n = thisText.Length;
            for (int i = 0; i < n; i++)
            {
                char c = thisText[i];
                if (Char.IsDigit(c))
                {
                    foundADigit = true;
                    whatWasFound = c.ToString();
                    indexOfDigit = i;
                    break;
                }
            }
            return Tuple.Create(foundADigit, whatWasFound, indexOfDigit);
        }
        /// <summary>
        /// Returns true if the given string contains any ASCII digits (that is, '0'..'9') and also puts a string representation of that in digitFound.
        /// </summary>
        /// <param name="thisText">The given string to check for digits</param>
        /// <param name="digitFound">If any digits are found, a string representation of the first digit found is placed into this</param>
        /// <param name="indexOfDigit">If any digits are found, the zero-based index into the given text is placed into this</param>
        /// <returns>false if the given string has no digits, true if it does</returns>
        /// <exception cref="ArgumentNullException"/>
        public static bool ContainsDigit(this string thisText, out string digitFound, out int indexOfDigit)
        {
            if (thisText == null)
            {
                throw new ArgumentNullException("thisText must not be null");
            }
            int n = thisText.Length;
            for (int i = 0; i < n; i++)
            {
                char c = thisText[i];
                if (Char.IsDigit(c))
                {
                    digitFound = c.ToString();
                    indexOfDigit = i;
                    return true;
                }
            }
            digitFound = String.Empty;
            indexOfDigit = -1;
            return false;
        }
        #endregion

        #region ContainsRoot
        /// <summary>
        /// Indicate whether the string Contains the given word, ignoring case,
        /// or the simple plural of that word - doing a whole-word match.
        /// For now, the plural is assumed to end with "s".
        /// </summary>
        /// <param name="sThis">The text to test against</param>
        /// <param name="sWordSingular">The singular form of the word to match against</param>
        /// <returns>true if the string contains the given word or it's plural</returns>
        public static bool ContainsRoot(this string sThis, string sWordSingular)
        {
            string sWordFound;
            return ContainsRoot(sThis, sWordSingular, out sWordFound);
        }

        /// <summary>
        /// Indicate whether the string Contains the given word, ignoring case,
        /// or the simple plural of that word - doing a whole-word match.
        /// For now, the plural is assumed to end with "s".
        /// </summary>
        /// <param name="sThis">The text to test against</param>
        /// <param name="sWordSingular">The singular form of the word to match against</param>
        /// <param name="sWordFound">The actual word that was found, if any (as lowercase).</param>
        /// <returns>true if the string contains the given word or it's plural</returns>
        public static bool ContainsRoot(this string sThis, string sWordSingular, out string sWordFound)
        {
            // TODO: This also needs to guard against embedded, incidental pattern matches.
            sWordFound = String.Empty;
            string sPatternSingular = sWordSingular.ToLower();
            bool bFound = false;
            int LENp = sPatternSingular.Length;
            int LENme = sThis.Length;
            int i = sThis.IndexOf(sPatternSingular);
            if (i >= 0)
            {
                bFound = true;
                // Check the preceding boundary.
                if (i > 0)
                {
                    if (Char.IsLetter(sThis[i - 1]))
                    {
                        // There is a letter immediately preceding the start of this word, so that was a false match.
                        bFound = false;
                    }
                }
                else if (i + LENp < LENme && Char.IsLetter(sThis[i + LENp]))
                {
                    // There is a letter immediately following, so that was a false match.
                    bFound = false;
                }
                // We found the word as specified.
                if (bFound)
                {
                    sWordFound = sWordSingular;
                }
            }
            if (!bFound)
            {
                // TODO  This is the part that needs to be globalized.
                string sPatternPlural = sPatternSingular + "s";
                LENp = sPatternPlural.Length;
                i = sThis.IndexOf(sPatternPlural);
                if (i >= 0)
                {
                    bFound = true;
                    // Check the preceding boundary.
                    if (i > 0)
                    {
                        if (Char.IsLetter(sThis[i - 1]))
                        {
                            // There is a letter immediately preceding the start of this word, so that was a false match.
                            bFound = false;
                        }
                    }
                    else if (i + LENp < LENme && Char.IsLetter(sThis[i + LENp]))
                    {
                        // There is a letter immediately following, so that was a false match.
                        bFound = false;
                    }
                    // We found the plural form of the given word.
                    if (bFound)
                    {
                        sWordFound = sPatternPlural;
                    }
                }
            }
            return bFound;
        }
        #endregion

        #region DoubleQuoted and SingleQuoted
        /// <summary>
        /// Returns the given string enclosed within proper typographic Double-Quotes
        /// (ie, within Unicode characters for 201C and 201D).
        /// </summary>
        /// <param name="sText">The string you want to enclose</param>
        /// <returns>sText with quotes added</returns>
        public static string DoubleQuoted(this string sText)
        {
            return "\u201C" + sText + "\u201D";
        }

        /// <summary>
        /// Returns the given string enclosed within proper typographic single-quotes
        /// (ie, within Unicode characters for 2018 and 2019).
        /// </summary>
        /// <param name="sText">The string you want to enclose</param>
        /// <returns>sText with quotes added</returns>
        public static string SingleQuoted(this string sText)
        {
            return "\u2018" + sText + "\u2019";
        }
        #endregion

#if SILVERLIGHT
        #region EndsWith
        /// <summary>
        /// Return true if the final portion of thisString is the same as suffix.
        /// This is built-in with the full .NET Framework, but unavailable with Silverlight.
        /// </summary>
        /// <param name="thisString">The string in question</param>
        /// <param name="suffix">A string that we want to test whether is the same as the end of thisString</param>
        /// <param name="isIgnoringCase">Dictates whether to ignore capitalization in the comparison</param>
        /// <returns>true if thisString includes suffix at it's end</returns>
        public static bool EndsWith(this string thisString, string suffix, bool isIgnoringCase)
        {
            // The full .NET Framework has EndsWith, and we could simply return, for example: thisString.EndsWith(suffix, true, CultureInfo.InvariantCulture)
#if DEBUG
            if (thisString == null)
            {
                throw new ArgumentNullException("thisString");
            }
            if (suffix == null)
            {
                throw new ArgumentNullException("suffix");
            }
#endif
            int lengthOfThisString = thisString.Length;
            int lengthOfSuffix = suffix.Length;

            if (lengthOfSuffix > lengthOfThisString)
            {
                return false;
            }

            string pertinentPartOfThisString;
            if (lengthOfSuffix == lengthOfThisString)
            {
                pertinentPartOfThisString = thisString;
            }
            else
            {
                pertinentPartOfThisString = thisString.Substring(lengthOfThisString - lengthOfSuffix);
            }
            // At this point we know that pertinentPartOfThisString is the same length as prefix.
            if (isIgnoringCase)
            {
                return suffix.Equals(pertinentPartOfThisString, StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                return suffix.Equals(pertinentPartOfThisString, StringComparison.InvariantCulture);
            }
        }
        #endregion StartsWith
#endif

        #region ExpandTo
        /// <summary>
        /// Given a piece of text, expand it to the desired length by simply repeating the text, or shortening it as necessary.
        /// </summary>
        /// <param name="originalText">the original text-pattern to expand or shorten</param>
        /// <param name="desiredLength">the length of the result</param>
        /// <returns>a new string that is the original expanded or shortened to the specified length</returns>
        public static string ExpandTo(this string originalText, int desiredLength)
        {
            // Note: This is intended for things like running unit-tests.
            // In fact, I wrote this using pure TDD: I wrote a comprehensive series of unit-tests, then composed this method to pass those tests.
            if (originalText == null)
            {
                throw new ArgumentNullException("originalText");
            }
            if (String.IsNullOrWhiteSpace(originalText))
            {
                throw new ArgumentException("originalText must not be empty");
            }
            if (desiredLength < 0)
            {
                throw new ArgumentOutOfRangeException("desiredLength must not be negative!");
            }
            else if (desiredLength == 0)
            {
                return String.Empty;
            }
            else if (desiredLength == originalText.Length)
            {
                return originalText;
            }
            else if (desiredLength < originalText.Length)
            {
                return originalText.Substring(0, desiredLength);
            }
            int len = originalText.Length;
            int numberOfTimesItFits = desiredLength / len;
            var sb = new StringBuilder();
            for (int i = 0; i < numberOfTimesItFits; i++)
            {
                sb.Append(originalText);
            }
            int remainderLength = desiredLength % len;
            // Add any partial-pattern..
            if (remainderLength > 0)
            {
                string remainder = originalText.Substring(0, remainderLength);
                sb.Append(remainder);
            }
            return sb.ToString();
        }
        #endregion ExpandTo

        #region filesystem - related

        #region AsFileSize
        /// <summary>
        /// Return a string denoting the numeric value as a metric in Bytes, KB (Kilobytes), MB (Megabytes), etc.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="includeUnits"></param>
        /// <returns></returns>
        public static string AsFileSize(this long bytes, bool includeUnits)
        {
            // Note: Found some examples at http://sharpertutorials.com/pretty-format-bytes-kb-mb-gb/
            //                         also http://stackoverflow.com/questions/128618/c-file-size-format-provider
            // If I wanted greater capacity, could use “YB”, “ZB”, “EB”, “PB”, “TB”, “GB”, “MB”, “KB”, “Bytes”.
            const int scale = 1024;
            var orders = new string[] { "TB", "GB", "MB", "KB", "Bytes" };
            long max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (var order in orders)
            {
                if (bytes > max)
                    return String.Format("{0:##.##} {1}", Decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 Bytes";
        }
        #endregion

        #region FilenameWithoutExtension
        /// <summary>
        /// Return the given filesystem pathname without the extension,
        /// which is the portion that comes after the final period.
        /// The string that is returned does not include that final period.
        /// </summary>
        /// <param name="pathname">a string representing a filesystem-path, that we want to get a portion of</param>
        /// <returns>pathname without the extension nor the final period</returns>
        public static string FilenameWithoutExtension(this string pathname)
        {
            if (String.IsNullOrWhiteSpace(pathname))
            {
                throw new ArgumentNullException("The argument to StringLib.FilenameWithoutExtension must not be null!");
            }
            int indexOfLastPeriod = pathname.LastIndexOf('.');
            if (indexOfLastPeriod >= 0)
            {
                return pathname.Substring(0, indexOfLastPeriod);
            }
            else
            {
                return pathname;
            }
        }
        #endregion

        #region FilenameExtensionWithoutPeriod
        public static string FilenameExtensionWithoutPeriod(this string pathname)
        {
            if (String.IsNullOrWhiteSpace(pathname))
            {
                throw new ArgumentNullException("The argument to StringLib.FilenameExtensionWithoutPeriod must not be null!");
            }
            int indexOfLastPeriod = pathname.LastIndexOf('.');
            if (indexOfLastPeriod >= 0 && indexOfLastPeriod < pathname.Length - 1)
            {
                return pathname.Substring(indexOfLastPeriod + 1);
            }
            else
            {
                return String.Empty;
            }
        }
        #endregion

        #region IsTheSameFilesystemPath
        /// <summary>
        /// Compare two filesystem pathnames and return true if they equate to the same thing, ignoring any drive-letter that either may have.
        /// </summary>
        /// <param name="pathReference">one pathname to compare</param>
        /// <param name="pathToTest">the other pathname to compare</param>
        /// <param name="isAbsolutePathRequired">false indicates a simple folder-name may match an absolute path, true means they do have to exactly match</param>
        /// <returns>true if both paths potentially refer to the same filesystem object, ignoring drive-letters</returns>
        /// <remarks>The argument for isAbsolutePathRequired controls whether a simple directory-name
        ///   may match against an absolute path.
        ///   For example, (if true) then "obj" matches "C:\Proj\obj".
        /// </remarks>
        public static bool IsTheSameFilesystemPath(this string pathReference, string pathToTest, bool isAbsolutePathRequired)
        {
            if (String.IsNullOrWhiteSpace(pathReference))
            {
                throw new ArgumentNullException("argument pathReference to method StringLib.IsTheSameFilesystemPath must not be empty.");
            }
            if (String.IsNullOrWhiteSpace(pathToTest))
            {
                throw new ArgumentNullException("argument pathToTest to method StringLib.IsTheSameFilesystemPath must not be empty.");
            }
            // Normalize the inputs by trimming space, and converting to uppercase.
            string pathReference2 = pathReference.Trim().ToUpper();
            string pathToTest2 = pathToTest.Trim().ToUpper();
            int n1 = pathReference2.Length;
            int n2 = pathToTest2.Length;
            // Remove any drive-letter, and the colon that follows it.
            if (n1 >= 2 && pathReference2[1] == ':' && pathReference2[0].IsEnglishAlphabetLetter())
            {
                pathReference2 = pathReference2.Substring(2);
            }
            if (n2 >= 2 && pathToTest2[1] == ':' && pathToTest2[0].IsEnglishAlphabetLetter())
            {
                pathToTest2 = pathToTest2.Substring(2);
            }
            // Remove any leading slashes.
            //if (pathReference2[0] == '\\')
            //{
            //    pathReference2 = pathReference2.Substring(1);
            //}
            //if (pathToTest2[0] == '\\')
            //{
            //    pathToTest2 = pathToTest2.Substring(1);
            //}
            // Remove any trailing slashes.
            n1 = pathReference2.Length;
            if (pathReference2[n1 - 1] == '\\')
            {
                pathReference2 = pathReference2.Substring(0, n1 - 1);
            }
            n2 = pathToTest2.Length;
            if (pathToTest2[n2 - 1] == '\\')
            {
                pathToTest2 = pathToTest2.Substring(0, n2 - 1);
            }
            // Finally, compare the normalized paths.
            if (pathToTest2.Equals(pathReference2))
            {
                return true;
            }
            else // need to consider 
            {
                if (isAbsolutePathRequired)
                {
                    return false;
                }
                else
                {
                    // See if one is an absolute path, and the other is a simple folder-name (with no slashes in it).
                    bool isReferencePathAbsolute = pathReference2[0] == '\\';
                    bool isTestPathAbsolute = pathToTest2[0] == '\\';
                    if (isReferencePathAbsolute ^ isTestPathAbsolute)
                    {
                        if (isReferencePathAbsolute)
                        {
                            // Extract the string that comes after the last slash.
                            string lastFolderOfReferencePath = pathReference2.WhatComesAfterLastSlash();
                            return pathToTest2.Equals(lastFolderOfReferencePath);
                        }
                        else
                        {
                            // Extract the string that comes after the last slash.
                            string lastFolderOfTestPath = pathToTest2.WhatComesAfterLastSlash();
                            return pathReference2.Equals(lastFolderOfTestPath);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        #endregion

        #region IsPathnameCharacterValid
        /// <summary>
        /// Return true if the given character is valid to use within a Windows pathname. This permits non-English UNICODE characters.
        /// </summary>
        /// <param name="pathnameCharacter">The character to evaluate for validity</param>
        /// <returns>true if the character may be used within a pathname</returns>
        public static bool IsPathnameCharacterValid(char pathnameCharacter)
        {
            // Most characters can be used in naming files.
            // These cannot: < > : " / \ | ? * although the fore and back slashes are valid in pathnames.
            bool isValid;
            // Note: I exclude the colon from this list of invalid chars because it can be used to denote the drive letter, but only there.
            switch (pathnameCharacter)
            {
                case '<':
                case '>':
                //case ':':  
                case '\"':
                case '|':
                case '?':
                case '*':
                    isValid = false;
                    break;
                default:
                    isValid = true;
                    break;
            }
            return isValid;
        }
        #endregion

        #region TopOfFilesystemPath
        /// <summary>
        /// Given a string that represents a filesystem directory (such as "Folder1\Folder2\Folder3"),
        /// return the topmost folder (like "Folder1") and put the remainder into pathRemainder (e.g. "Folder2\Folder3").
        /// If the string is empty then return null.
        /// If there is no path-separator (ie, there is only the one folder) then that folder is returned and pathRemainder is set to null.
        /// If path begins with a disk-drive in the form "C:\Folder1\Folder2", for example,
        /// then this returns "C:\" and pathRemainder is set to "Folder1\Folder2".
        /// </summary>
        /// <param name="filesystemFolder">the filesystem directory from which to get the top-most folder</param>
        /// <param name="pathRemainder">what remainds of path after extracting the topmost folder</param>
        /// <returns>the name of the folder that appears uppermost on the given path, or null if there is none</returns>
        public static string TopOfFilesystemPath(string filesystemFolder, out string pathRemainder)
        {
            string result = null;
            pathRemainder = null;
            if (!String.IsNullOrWhiteSpace(filesystemFolder))
            {
                string path = filesystemFolder.Trim();
                if (!String.IsNullOrWhiteSpace(path))
                {
                    // If it starts with a slash, remove that..
                    bool doesStartWithSlash = path[0] == Path.DirectorySeparatorChar;
                    if (doesStartWithSlash)
                    {
                        if (path.Length == 1)
                        {
                            result = null;
                            return result;
                        }
                        path = path.Substring(1);
                    }
                    if (path.StartsWithDrive())
                    {
                        if (path.Length > 2 && path[2] != Path.DirectorySeparatorChar)
                        {
                            result = path.Substring(0, 2) + Path.DirectorySeparatorChar;
                            pathRemainder = path.Substring(2);
                        }
                        else if (path.Length == 2)
                        {
                            result = path.Substring(0, 2) + Path.DirectorySeparatorChar;
                            // The length is 2 so there is no string left.
                            pathRemainder = null;
                        }
                        else // Length is > 2 and the 3rd character is the slash
                        {
                            result = Path.GetPathRoot(path);
                            if (path.Length > 3)
                            {
                                pathRemainder = path.Substring(3);
                            }
                            else
                            {
                                pathRemainder = null;
                            }
                        }
                        return result;
                    }
                    int i = path.IndexOf(Path.DirectorySeparatorChar);
                    if (i == -1)
                    {
                        return path;
                    }
                    else
                    {
                        result = path.Substring(0, i);
                        if (path.Length > i + 1)
                        {
                            pathRemainder = path.Substring(i + 1).WithoutAtEnd(Path.DirectorySeparatorChar);
                        }
                    }
                }
            }
            return result;
        }
        #endregion

        #endregion filesystem - related

        #region IsEnglishAlphabetLetter
        /// <summary>
        /// Return true if the given character is a letter of the English alphabet (ie, A through Z), regardless of whether it's upper or lower case.
        /// </summary>
        /// <param name="chr">The character to test</param>
        /// <returns>True if it is a letter of the English alphabet</returns>
        public static bool IsEnglishAlphabetLetter(this char chr)
        {
            Regex regex = new Regex(@"[a-zA-Z]");
            string testInput = chr.ToString();
            return regex.IsMatch(testInput);
        }
        #endregion

        #region EnglishAlphabetLetterToIndex and IndexToEnglishAlphabetLetter

        public static int EnglishAlphabetLetterToIndex(string sString)
        {
            if (String.IsNullOrEmpty(sString))
                return -1;
            else
                return EnglishAlphabetLetterToIndex(sString[0]);
        }
        /// <summary>
        /// Given a letter character, return it's index from A
        /// ie, for A return zero, B = 1, C = 2, etc.
        /// </summary>
        /// <param name="chr">The character to find an index of</param>
        /// <returns>The distance from letter A as an integer, or -1 if some unexpected value</returns>
        public static int EnglishAlphabetLetterToIndex(char chr)
        {
            int i = -1;
            // Make it uppercase..
            string sAsString = chr.ToString();
            string sUppercase = sAsString.ToUpper();
            char chrUppercase = sUppercase[0];
            switch (chrUppercase)
            {
                case 'A':
                    i = 0;
                    break;
                case 'B':
                    i = 1;
                    break;
                case 'C':
                    i = 2;
                    break;
                case 'D':
                    i = 3;
                    break;
                case 'E':
                    i = 4;
                    break;
                case 'F':
                    i = 5;
                    break;
                case 'G':
                    i = 6;
                    break;
                case 'H':
                    i = 7;
                    break;
                default:
                    break;
            }
            return i;
        }

        /// <summary>
        /// Given a low-valued integer, return the corresponding letter (as a string)
        /// with zero = A, 1 = B, 2 = C, etc.
        /// </summary>
        /// <param name="i">The integer value to convert</param>
        /// <returns>A string containing the corresponding letter, or a question-mark if it's an unanticipated value</returns>
        static public string IndexToEnglishAlphabetLetter(int i)
        {
            string s;
            switch (i)
            {
                case 0:
                    s = "A";
                    break;
                case 1:
                    s = "B";
                    break;
                case 2:
                    s = "C";
                    break;
                case 3:
                    s = "D";
                    break;
                case 4:
                    s = "E";
                    break;
                case 5:
                    s = "F";
                    break;
                default:
                    s = "?";
                    break;
            }
            return s;
        }
        #endregion

        #region IsAllDigits
        /// <summary>
        /// Indicates whether the given string is composed only of numeric digits
        /// </summary>
        /// <param name="sInput">The string to check</param>
        /// <returns>true if the input IS composed only of digits</returns>
        public static bool IsAllDigits(this string sInput)
        {
            bool r = true;
            if (sInput.Length > 0)
            {
                foreach (char ch in sInput)
                {
                    if (!Char.IsDigit(ch))
                    {
                        r = false;
                        break;
                    }
                }
            }
            return r;
        }
        #endregion

        #region IsAllHexDigits
        /// <summary>
        /// Indicates whether the given string is composed only of numeric digits or A..F
        /// </summary>
        /// <param name="sInput">The string to check</param>
        /// <returns>true if the input IS composed only of digits</returns>
        public static bool IsAllHexDigits(this string sInput)
        {
            bool r = true;
            if (sInput.Length > 0)
            {
                string sInputUpperCase = sInput.ToUpper();
                foreach (char ch in sInputUpperCase)
                {
                    if (!(Char.IsDigit(ch) || ch == 'A' || ch == 'B' || ch == 'C' || ch == 'D' || ch == 'E' || ch == 'F'))
                    {
                        r = false;
                        break;
                    }
                }
            }
            return r;
        }
        #endregion

        #region IsAllLowercase
        /// <summary>
        /// Indicate whether the given string is composed of only lowercase letters (or no letters at all)
        /// </summary>
        /// <param name="sWhat">The given string to check</param>
        /// <returns>true if there are no uppercase letters in the given string</returns>
        /// <exception cref="ArgumentException"/>
        public static bool IsAllLowercase(this string sWhat)
        {
            if (String.IsNullOrWhiteSpace(sWhat))
            {
                throw new ArgumentException("sWhat must not be empty");
            }
            return (sWhat == sWhat.ToLower());
        }
        #endregion

        #region IsAllUppercase
        /// <summary>
        /// Indicate whether the given string is composed of only uppercase letters (or no letters at all)
        /// </summary>
        /// <param name="sWhat">The given string to check</param>
        /// <returns>true if there are no lowercase letters in the given string</returns>
        /// <exception cref="ArgumentException"/>
        public static bool IsAllUppercase(this string sWhat)
        {
            if (String.IsNullOrWhiteSpace(sWhat))
            {
                throw new ArgumentException("sWhat must not be empty");
            }
            return (sWhat == sWhat.ToUpper());
        }
        #endregion

        #region IsAllMoneyChars
        //TODO: Presently this is USD only!
        // Ignores the string "USD"
        /// <summary>
        /// Indicates whether the given input string is composed only of characters appropriate
        /// for a monetary value.
        /// </summary>
        /// <param name="sInput">The input string to check</param>
        /// <param name="sReason">If input is not valid then this gets set to the reason</param>
        /// <returns>true if input string has only appropriate characters</returns>
        public static bool IsAllMoneyChars(this string sInput, out string sReason)
        {
            sReason = "";
            bool r = true;
            if (!String.IsNullOrEmpty(sInput))
            {
                string s = sInput.Trim().ToLower();
                if (s.Length > 0)
                {
                    int i = 0;
                    // Account for starting or ending in USD
                    if (s.StartsWith("usd"))
                    {
                        s = s.Substring(3).TrimStart();
                        i = 3;
                    }
                    if (s.Length > 0)
                    {
                        if (s.Length >= 3)
                        {
                            if (s.EndsWith("usd"))
                            {
                                int n = s.Length;
                                if (n < 4)
                                {
                                    return r;
                                }
                                s = s.Substring(0, n - 3);
                            }
                        }
                        for (; i < s.Length; i++)
                        {
                            char ch = s[i];
                            if (!(Char.IsDigit(ch) || ch == '-' || ch == '+' || ch == '$' || ch == ',' || ch == '.'))
                            {
                                r = false;
                                sReason = "The " + i.ToOrdinal(true) + " character is not expected in a numeric value";
                                break;
                            }
                        }
                    }
                }
            }
            return r;
        }
        #endregion end IsAllMoneyChars

        #region IsACurrencyWord
        /// <summary>
        /// Given a single word, this returns true if it could commonly represent a
        /// term of currency, such as "dollar", "buck", etc.
        /// If the currency can be determined, it's returned as the std 3-character code
        /// in sCurrency (which defaults to an empty string otherwise).
        /// If the word indicates multiple units, such as "Benjamin" or "G",
        /// then iUnits is set to, eg. 100 or 1000 accordingly.
        /// </summary>
        /// <param name="sWord">The word to examine</param>
        /// <param name="sCurrency">std 3-character currency field, if it's explicit in the input</param>
        /// <param name="iUnits">Normally will be 1, unless the input represents a multiple.</param>
        /// <returns></returns>
        public static bool IsACurrencyWord(this string sWord, out string sCurrency, out int iUnits)
        {
            sCurrency = "";
            iUnits = 1;
            bool bYes = false;
            string s = sWord.Trim().ToLower();
            switch (s)
            {
                case "buck":
                case "bucks":
                case "froghide":
                case "froghides":
                case "dollar":
                case "dollars":
                case "smacker":
                case "smackers":
                    bYes = true;
                    sCurrency = "USD";
                    break;
                default:
                    break;
            }
            return bYes;
        }
        #endregion

        #region IsADayOfTheWeek
        /// <summary>
        /// Returns true if the given sInput represents a valid (English!) day of the week,
        /// and sets eValue to what day-of-the-week that input represents.
        /// </summary>
        /// <param name="what">Some text that we will check to see whether it is a day-of-the-week</param>
        /// <param name="eValue">The System.DayOfWeek value that the input represents, or Sunday by default</param>
        /// <returns>true if the input is indeed a valid DayOfWeek value</returns>
        public static bool IsADayOfTheWeek(this string what, out DayOfWeek eValue)
        {
            eValue = DayOfWeek.Sunday;
            string s = what.ToLower();
            switch (s)
            {
                case "sun":
                case "sunday":
                    eValue = DayOfWeek.Sunday;
                    return true;
                case "mon":
                case "monday":
                    eValue = DayOfWeek.Monday;
                    return true;
                case "tue":
                case "tues":
                case "tuesday":
                case "tueday":
                    eValue = DayOfWeek.Tuesday;
                    return true;
                case "wed":
                case "wednesday":
                case "wedsday":
                case "wensday":
                    eValue = DayOfWeek.Wednesday;
                    return true;
                case "thursday":
                case "thurday":
                case "thurs":
                case "thur":
                    eValue = DayOfWeek.Thursday;
                    return true;
                case "friday":
                case "fri":
                    eValue = DayOfWeek.Friday;
                    return true;
                case "saturday":
                case "sat":
                case "satday":
                    eValue = DayOfWeek.Saturday;
                    return true;
                default:
                    return false;
            }
        }
        #endregion IsDayOfWeek

        #region IsANumberWord
        /// <summary>
        /// Indicates whether the given word can be recognized as a number value, and gives the value
        /// </summary>
        /// <param name="sWord">The given word</param>
        /// <param name="niValue">Sets this to the numeric value of the word, or null</param>
        /// <param name="bIsOrdinal">This gets set to true if the given sWord is an ordinal number (eg, "first")</param>
        /// <returns>true if it is a number, false if not</returns>
        public static bool IsANumberWord(this string sWord, out int? niValue, out bool bIsOrdinal)
        {
            bIsOrdinal = false;
            bool bYes = true;
            niValue = 0;
            // Handle empty inputs.
            if (String.IsNullOrEmpty(sWord))
            {
                niValue = null;
                return false;
            }
            string s = sWord.Trim().ToLower();
            // Check for a "st", "nd", "th" or "rd" suffix.
            bool bHasOrdinalSuffix = false;
            if (s != "first" && s.EndsWith("st"))
            {
                // Remove the ordinal suffix, but remember that it was there.
                bHasOrdinalSuffix = true;
                s = s.Substring(0, s.Length - 2);
            }
            else if (s != "second" && s.EndsWith("nd"))
            {
                // Remove the ordinal suffix, but remember that it was there.
                bHasOrdinalSuffix = true;
                s = s.Substring(0, s.Length - 2);
            }
            else if (s.EndsWith("th") && s != "fifth" && s != "eighth" && s != "ninth")
            {
                // Remove the ordinal suffix, but remember that it was there.
                bHasOrdinalSuffix = true;
                s = s.Substring(0, s.Length - 2);
            }
            else if (s.EndsWith("rd") && s != "third")
            {
                // Remove the ordinal suffix, but remember that it was there.
                bHasOrdinalSuffix = true;
                s = s.Substring(0, s.Length - 2);
            }
            if (bHasOrdinalSuffix)
            {
                bIsOrdinal = true;
            }
            switch (s)
            {
                case "nothing":
                case "nuthin":
                case "nada":
                case "zip":
                case "unknown":
                case "unsure":
                case "unspecified":
                case "dunno":
                    niValue = null;
                    break;
                case "zero":
                case "o":
                    niValue = 0;
                    break;
                case "one":
                    niValue = 1;
                    break;
                case "two":
                    niValue = 2;
                    break;
                case "three":
                    niValue = 3;
                    break;
                case "four":
                    niValue = 4;
                    break;
                case "five":
                    niValue = 5;
                    break;
                case "six":
                    niValue = 6;
                    break;
                case "seven":
                    niValue = 7;
                    break;
                case "eight":
                    niValue = 8;
                    break;
                case "nine":
                    niValue = 9;
                    break;
                case "ten":
                    niValue = 10;
                    break;
                case "eleven":
                    niValue = 11;
                    break;
                case "twelve":
                    niValue = 12;
                    break;
                case "thirteen":
                    niValue = 13;
                    break;
                case "fourteen":
                case "forteen":
                    niValue = 14;
                    break;
                case "fifteen":
                    niValue = 15;
                    break;
                case "sixteen":
                    niValue = 16;
                    break;
                case "seventeen":
                    niValue = 17;
                    break;
                case "eighteen":
                    niValue = 18;
                    break;
                case "nineteen":
                case "ninteen":
                    niValue = 19;
                    break;
                case "twenty":
                    niValue = 20;
                    break;
                case "thirty":
                    niValue = 30;
                    break;
                case "forty":
                case "fourty":
                    niValue = 40;
                    break;
                case "fifty":
                    niValue = 50;
                    break;
                case "sixty":
                    niValue = 60;
                    break;
                case "seventy":
                    niValue = 70;
                    break;
                case "eighty":
                    niValue = 80;
                    break;
                case "ninty":
                case "ninety":
                    niValue = 90;
                    break;
                case "first":
                    bIsOrdinal = true;
                    niValue = 1;
                    break;
                case "second":
                    bIsOrdinal = true;
                    niValue = 2;
                    break;
                case "third":
                    bIsOrdinal = true;
                    niValue = 3;
                    break;
                // "fourth" already handled
                case "fifth":
                    bIsOrdinal = true;
                    niValue = 5;
                    break;
                // "sixth" already handled
                // "seventh" already handled
                case "eighth":
                    bIsOrdinal = true;
                    niValue = 8;
                    break;
                case "ninth":
                    bIsOrdinal = true;
                    niValue = 9;
                    break;
                default:
                    bYes = false;
                    break;
            }
            return bYes;
        }
        #endregion IsANumberWord

        #region IsAProperNoun
        public static bool IsAProperNoun(this string word)
        {
            //TODO: This absolutely does need to use the language-database! It's here just to help with unit-tests for the moment.
            string s = word.ToLower();
            switch (s)
            {
                case "germany":
                    return true;
                default:
                    return false;
            }
        }
        #endregion

        #region IsAVowel
        /// <summary>
        /// Return true if the given character is a vowel. This does not return true for the letter 'y' since that's not always a vowel.
        /// This is applicable ONLY to English.
        /// </summary>
        /// <param name="characterToCheck">The character to check for vowel-ness</param>
        /// <returns>true if it's always a vowel (returns false for y)</returns>
        public static bool IsAVowel(this char characterToCheck)
        {
            // We do not account here for y, which is "sometimes" a vowel. That'll have to be context-dependent.
            switch (characterToCheck)
            {
                case 'A':
                case 'a':
                case 'E':
                case 'e':
                case 'I':
                case 'i':
                case 'O':
                case 'o':
                case 'U':
                case 'u':
                    return true;
                default:
                    return false;
            }
        }
        #endregion

        #region IsInTitlecase
        /// <summary>
        /// Indicates whether the given string has the initial letter in uppercase and the remainder all lowercase.
        /// </summary>
        /// <param name="sWhat">The given string to check</param>
        /// <returns>true if only the first letter is in uppercase</returns>
        /// <exception cref="ArgumentException"/>
        /// <remarks>
        /// This method is named thus in order to reflect the naming of the titlecase function in Python.
        /// </remarks>
        public static bool IsInTitlecase(this string sWhat)
        {
            if (String.IsNullOrWhiteSpace(sWhat))
            {
                throw new ArgumentException("sWhat must not be empty");
            }
            bool bAnswer = false;
            string sFirstCharacter = sWhat.Substring(0, 1);
            if (sFirstCharacter == sFirstCharacter.ToUpper())
            {
                string sRestOfIt = sWhat.Substring(1);
                if (sRestOfIt == sRestOfIt.ToLower())
                {
                    bAnswer = true;
                }
            }
            return bAnswer;
        }
        #endregion

        #region IsComposedOfCharactersFrom
        /// <summary>
        /// Indicate whether the given character is found within the array of valid characters.
        /// </summary>
        /// <param name="theGivenCharacter">The Character to test against the valid characters</param>
        /// <param name="allValidCharacters">The array of characters that we consider to be valid in this instance</param>
        /// <returns>true if the given character is present in allValidCharacters</returns>
        public static bool IsComposedOfCharactersFrom(this Char theGivenCharacter, char[] allValidCharacters)
        {
            bool isValid = false;
            foreach (char thisValidChar in allValidCharacters)
            {
                if (theGivenCharacter == thisValidChar)
                {
                    isValid = true;
                    break;
                }
            }
            return isValid;
        }
        #endregion

        #region IsEqualIgnoringCase
        /// <summary>
        /// Extension method that provides a slightly simpler way to do a case-insensitive test of two strings for equality
        /// when you don't want to consider case and beginning-or-trailing whitespace.
        /// </summary>
        /// <param name="sThis">one string to compare</param>
        /// <param name="sToWhat">the other string to compare against</param>
        /// <returns>true if they're equal regardless of case</returns>
        static public bool IsEqualIgnoringCase(this string thisString, string toWhat)
        {
            string sThisTrimmed = thisString.Trim();
            string sToWhatTrimmed = toWhat.Trim();
#if SILVERLIGHT
            return String.Compare(sThisTrimmed, sToWhatTrimmed, StringComparison.InvariantCultureIgnoreCase) == 0;
#else
            return String.Compare(sThisTrimmed, sToWhatTrimmed, true) == 0;
#endif
        }
        #endregion

        #region IsExpressedInHexadecimal
        /// <summary>
        /// Indicate whether the string seems to be an integer value that is expressed in hexadecimal,
        /// by detecting the presense of a prefix or suffix, or the inclusion of A..F as digits.
        /// </summary>
        /// <param name="text">the string to test</param>
        /// <returns>True if it looks like it's being expressed as hex</returns>
        public static bool IsExpressedInHexadecimal(this string text)
        {
            bool r = false;
            if (!String.IsNullOrEmpty(text))
            {
                string trimmedText = text.Trim();
#if SILVERLIGHT
                if (trimmedText.StartsWith("0x", true)
                    || trimmedText.StartsWith("&h", true)
                    || trimmedText.StartsWith("h", true)
                    || trimmedText.EndsWith("h", true)
                    || trimmedText.EndsWith("hex", true)
                    || trimmedText.EndsWith("x", true))
#else
                if (trimmedText.StartsWith("0x", true, CultureInfo.InvariantCulture)
                    || trimmedText.StartsWith("&h", true, CultureInfo.InvariantCulture)
                    || trimmedText.StartsWith("h", true, CultureInfo.InvariantCulture)
                    || trimmedText.EndsWith("h", true, CultureInfo.InvariantCulture)
                    || trimmedText.EndsWith("hex", true, CultureInfo.InvariantCulture)
                    || trimmedText.EndsWith("x", true, CultureInfo.InvariantCulture))
#endif
                {
                    return true;
                }
                var regex = new Regex(@"[a-fA-F]+");
                if (regex.IsMatch(trimmedText))
                {
                    return true;
                }
            }
            return r;
        }
        #endregion

        #region IsInArray
        /// <summary>
        /// Return true if the given string does not match any of the strings within the given array-of-strings, ignoring case.
        /// </summary>
        /// <param name="what">the given string value which we want to whether the array contains</param>
        /// <param name="arrayOfStrings">the array of strings that we want to check for containing what</param>
        /// <returns>true if arrayOfStrings has a copy (without regard to case) of the value what</returns>
        public static bool IsInArray(this string what, string[] arrayOfStrings)
        {
            if (what == null)
            {
                throw new ArgumentNullException("argument what in method IsInArray must not be null.");
            }
            if (arrayOfStrings != null)
            {
                foreach (string element in arrayOfStrings)
                {
                    if (what.Equals(element, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region IsNumeric
        /// <summary>
        /// Validates a string to see if it can be converted into a numeric value.
        /// Models the same function in VB.Net
        /// </summary>
        public static bool IsNumeric(this string stringToCheck)
        {
            double dd;
            return Double.TryParse(stringToCheck, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out dd);
        }
        #endregion

        #region IsOnlyDigitsAndPeriod
        /// <summary>
        /// Indicates whether the given string is composed only of numeric digits and an optional decimal-point
        /// </summary>
        /// <param name="sInput">The string to check</param>
        /// <returns>true if the input IS composed only of digits and an optional decimal-point</returns>
        public static bool IsOnlyDigitsAndPeriod(this string sInput)
        {
            bool r = true;
            bool bDecPoint = false;
            if (sInput.Length > 0)
            {
                foreach (char ch in sInput)
                {
                    if (!Char.IsDigit(ch))
                    {
                        if (ch == '.')
                        {
                            if (bDecPoint)
                            {
                                // Too many decimal-points! fail.
                                r = false;
                                break;
                            }
                            else
                            {
                                // Flag that we've already seen the decimal-point
                                bDecPoint = true;
                            }
                        }
                        else
                        {
                            r = false;
                            break;
                        }
                    }
                }
            }
            return r;
        }
        #endregion

        #region IsOnlyDigitsPeriodsAndCommas
        /// <summary>
        /// Indicates whether the given string is composed only of numeric digits
        /// and possibly commas and/or decimal-points
        /// </summary>
        /// <param name="sInput">The string to check</param>
        /// <returns>true if the input IS composed only of digits, periods, and commas</returns>
        public static bool IsOnlyDigitsPeriodsAndCommas(this string sInput)
        {
            bool r = true;
            //bool bDecPoint = false;
            //bool bComma = false;
            if (sInput.Length > 0)
            {
                foreach (char ch in sInput)
                {
                    if (!Char.IsDigit(ch))
                    {
                        if (ch == '.')
                        {
                            // Flag that we've already seen the decimal-point
                            //bDecPoint = true;
                        }
                        else if (ch == ',')
                        {
                            //bComma = true;
                        }
                        else
                        {
                            r = false;
                            break;
                        }
                    }
                }
            }
            return r;
        }
        #endregion

        #region Join
        /// <summary>
        /// Given a bit of "glue text", append the left and right strings together with the glue-text in-between.
        /// For example, " ".Join("Left", "Right) returns "Left Right".
        /// </summary>
        /// <param name="sGlue">the text to insert between the other parts. This may be an empty string, but not null.</param>
        /// <param name="sLeft">the left-string to concatenate on the left</param>
        /// <param name="sRight">the right-string, to concatenate to the right of sGlue</param>
        /// <returns>sLeft concatenated with sGlue concatenated with sRight</returns>
        /// <exception cref="ArgumentNullException">
        /// Throws an ArgumentNullException if sGlue is null.
        /// </exception>
        /// <remarks>
        /// If sGlue is an empty string, then sLeft is simply concatenated with sRight.
        /// If sLeft is null or empty, then sRight is returned and sGlue is ignored.
        /// If sRight is null or empty, then sLeft is returned and sGlue is ignored.
        /// </remarks>
        public static string Join2(this string sGlue, string sLeft, string sRight)
        {
            //CBL: This method may be entirely bogus -- the String class already has a Join method.
            if (sGlue == null)
            {
                throw new ArgumentNullException("sGlue");
            }
            if (String.IsNullOrEmpty(sRight))
            {
                if (String.IsNullOrEmpty(sLeft))
                {
                    return String.Empty;
                }
                else
                {
                    return sLeft;
                }
            }
            else if (String.IsNullOrEmpty(sLeft))
            {
                if (String.IsNullOrEmpty(sRight))
                {
                    return String.Empty;
                }
                else
                {
                    return sRight;
                }
            }
            return sLeft + sGlue + sRight;
        }
        #endregion

        #region Matches
        /// <summary>
        /// Indicate whether the given string (1st parameter) matches the given pattern (2nd parameter),
        /// ignoring case and common syntactical differences in spelling.
        /// </summary>
        /// <param name="sSource">The given string to match the pattern against</param>
        /// <param name="sPattern">The pattern string to test our source string against</param>
        /// <returns>true if they match, false otherwise</returns>
        public static bool Matches(this string sSource, string sPattern)
        {
            if (sPattern == null)
            {
                if (sSource == null)
                {
                    return true;
                }
                return false;
            }
            if (sSource.Equals(sPattern, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                string sSource2 = sSource.Trim().Replace("-", "");
                string sPattern2 = sPattern.Trim().Replace("-", "");
                if (sSource2.Equals(sPattern2, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
                else if (sSource2.Equals(sPattern2 + "s", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
                else
                {
                    // Match 0 against 0.0, 0F, 0.0F, and "zero"
                    if (sPattern.Equals("0"))
                    {
                        if (sSource.Equals("0") || sSource.Equals("0.0") || sSource.Equals("0.0F") || sSource.Equals("0F") || sSource.Equals("0M"))
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Indicate whether the given string (1st parameter) matches either of the given patterns (2nd and 3rd parameters),
        /// ignoring case and common syntactical differences in spelling.
        /// </summary>
        /// <param name="sSource">The given string to match against the patterns</param>
        /// <param name="sPattern1">a pattern string to test our source against</param>
        /// <param name="sPattern2">another pattern string to test our source against</param>
        /// <returns>true if there's a match, false otherwise</returns>
        public static bool Matches(this string sSource, string sPattern1, string sPattern2)
        {
            return Matches(sSource, sPattern1) || Matches(sSource, sPattern2);
        }

        /// <summary>
        /// Indicate whether the given string matches either of the given patterns (the subsequent),
        /// ignoring case and common syntactical differences in spelling.
        /// </summary>
        /// <param name="sSource">The given string to match against the patterns</param>
        /// <param name="sPattern1">a pattern string to test our source against</param>
        /// <param name="sPattern2">another pattern string to test our source against</param>
        /// <param name="sPattern3">another pattern string to test our source against</param>
        /// <returns>true if there's a match, false otherwise</returns>
        public static bool Matches(this string sSource, string sPattern1, string sPattern2, string sPattern3)
        {
            return Matches(sSource, sPattern1, sPattern2) || Matches(sSource, sPattern3);
        }

        /// <summary>
        /// Indicate whether the given string matches either of the given patterns (the subsequent),
        /// ignoring case and common syntactical differences in spelling.
        /// </summary>
        /// <param name="sSource">The given string to match against the patterns</param>
        /// <param name="sPattern1">a pattern string to test our source against</param>
        /// <param name="sPattern2">another pattern string to test our source against</param>
        /// <param name="sPattern3">another pattern string to test our source against</param>
        /// <param name="sPattern4">another pattern string to test our source against</param>
        /// <returns>true if there's a match, false otherwise</returns>
        public static bool Matches(this string sSource, string sPattern1, string sPattern2, string sPattern3, string sPattern4)
        {
            return Matches(sSource, sPattern1, sPattern2) || Matches(sSource, sPattern3, sPattern4);
        }
        #endregion

        #region NumberOf
        /// <summary>
        /// Return the number of ocurrances of the given character.
        /// </summary>
        /// <param name="sSource">The source-string to test for the given character</param>
        /// <param name="characterToTestFor">The character to scan the source-string for</param>
        /// <returns>the number of ocurrances of that character within the given string</returns>
        public static int NumberOf(this string sSource, char characterToTestFor)
        {
            if (!String.IsNullOrWhiteSpace(sSource))
            {
                string sourceLowercase = sSource.ToLower();
                char charToTestForLowercase = Char.ToLower(characterToTestFor);
                int n = 0;
                for (int i = 0; i < sourceLowercase.Length; i++)
                {
                    if (sourceLowercase[i] == charToTestForLowercase)
                    {
                        n++;
                    }
                }
                return n;
            }
            else
            {
                if (sSource == null)
                {
                    throw new ArgumentNullException("The sSource argument to NumberOf should not be null.");
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion

        #region NumberOfLeadingSpaces
        /// <summary>
        /// Return the number of space characters that begin the given string, if any.
        /// </summary>
        /// <param name="sSource">The source-string to test for spaces</param>
        /// <returns>the number of leading spaces</returns>
        public static int NumberOfLeadingSpaces(this string sSource)
        {
            if (!String.IsNullOrEmpty(sSource))
            {
                int n = 0;
                for (int i = 0; i < sSource.Length; i++)
                {
                    if (sSource[i] == ' ')
                    {
                        n = i + 1;
                    }
                    else
                    {
                        break;
                    }
                }
                return n;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region RemoveAll
        /// <summary>
        /// Return this string with all ocurrences of the given character removed.
        /// </summary>
        /// <param name="sThis">this string, from which we want to remove characters</param>
        /// <param name="charToRemove">the character to remove</param>
        /// <returns>a copy of the given string with all ocurrences of the indicated character removed</returns>
        static public string RemoveAll(this string sThis, char charToRemove)
        {
            //public static string RemoveSpecialCharacters(string str)
            if (sThis.IndexOf(charToRemove) > -1)
            {
                var sb = new StringBuilder();
                foreach (char c in sThis)
                {
                    //if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') | || (c >= 'a' && c <= 'z') | c == '.' || c == '_')
                    if (c != charToRemove)
                    {
                        sb.Append(c);
                    }
                }
                return sb.ToString();
            }
            else
            {
                return sThis;
            }
        }

        static public string RemoveAll(this string sThis, char[] charactersToRemove)
        {
            if (sThis.IndexOfAny(charactersToRemove) > -1)
            {
                var sb = new StringBuilder();
                foreach (char c in sThis)
                {
                    if (!charactersToRemove.Contains(c))
                    {
                        sb.Append(c);
                    }
                }
                return sb.ToString();
            }
            else
            {
                return sThis;
            }
        }
        #endregion RemoveAll

        #region Shortened
        /// <summary>
        /// Return the given string limited (if necessary) to the indicated length.
        /// If it's longer than that, then it's shortened and an ellipsis suffixed to it.
        /// </summary>
        /// <param name="sText">The text to limit in length</param>
        /// <param name="iTargetLength">The maximum length that we want it to be</param>
        /// <returns>The input text (sText) appropriately shortened (and the ellipsis applied if need be)</returns>
        public static string Shortened(this string sText, int iTargetLength)
        {
            string sResult = "";
            if (!String.IsNullOrEmpty(sText))
            {
                string sInput = sText.Trim().Replace('\n', ' ').Replace('\r', ' ');
                int iLength = sInput.Length;
                if (iLength <= iTargetLength)
                {
                    sResult = sInput;
                }
                else
                {
                    if (iTargetLength > 2)
                    {
                        string sPartToShow = sInput.Substring(0, iTargetLength - 2);
                        sResult = sPartToShow + "..";
                    }
                    else
                    {
                        sResult = sInput.Substring(0, iTargetLength);
                    }
                }
            }
            return sResult;
        }
        #endregion

        #region SplitQuoted

        //TODO: This goes away when we move up to a less ancient .NET version.
        public static string[] SplitQuoted(string sText)
        {
            return SplitQuoted(sText, " ", null);
        }

        /// <summary>
        /// Split a string, dealing correctly with quoted items.
        /// The quotes parameter is the character pair used to quote strings
        /// (default is "", the double-quote).
        /// You can also use a character pair (eg "{}") if the opening and closing quotes are different.
        /// </summary>
        /// <remarks>
        /// For example, you can split this:
        /// string[] fields = SplitQuoted("[one,two],three,[four,five]",,"[]")
        /// into 3 items, because commas inside [] are not taken into account.
        /// Multiple separators are ignored, so splitting "a,,b" using a comma as the separator
        /// will return two fields, not three. To get this behavior, you could use ", " (comma and space)
        /// as separators and default quotes,
        /// then set the string to something like 'a,"",b' to get the empty field.
        /// You could also use comma as *only separator and put a space to get a space field like'a, ,b'.
        /// </remarks>
        /// <param name="sText">text to split</param>
        /// <param name="sSeparators">The separator char(s) as a string</param>
        /// <param name="sQuotes">The char pair used to quote a string</param>
        /// <returns>An array of strings</returns>
        /// <exception cref="ArgumentNullException"/>
        public static string[] SplitQuoted(string sText, string sSeparators, string sQuotes)
        {
            // Default separators is a space and tab (e.g. "\t")
            // All separators not inside quote pair are ignored
            // Default quotes pair is two double quotes (e.g. '""""')
            if (sText == null)
            {
                throw new ArgumentNullException("text", "sText is null");
            }
            if (sSeparators == null || sSeparators.Length < 1)
            {
                sSeparators = "\t";
            }
            if (sQuotes == null || sQuotes.Length < 1)
            {
                sQuotes = "\"\"";
            }
            List<string> res = new List<string>();

            // Get the open and close chars, escape them for use in regular expressions.
            string openChar = Regex.Escape(sQuotes[0].ToString());
            string closeChar = Regex.Escape(sQuotes[sQuotes.Length - 1].ToString());

            // Build the pattern that searches for both quoted and unquoted elements.
            // Notice that the quoted element is defined by group #2
            // and the unquoted elment is defined by group #3.
            string sPattern = @"\s*(" + openChar + "([^" + closeChar + "]*)" +
                    closeChar + @"|([^" + sSeparators + @"]+))\s*";

            // Search the string.
            foreach (Match m in Regex.Matches(sText, sPattern))
            {
                string g3 = m.Groups[3].Value;
                if (g3 != null && g3.Length > 0)
                    res.Add(g3);
                else
                {
                    // Get the quoted string, but without the quotes.
                    res.Add(m.Groups[2].Value);
                }
            }
            return res.ToArray();
        }
        #endregion

#if SILVERLIGHT
        #region StartsWith
        /// <summary>
        /// Return true if the first portion of thisString is the same as prefix.
        /// This is built-in with the full .NET Framework, but unavailable with Silverlight.
        /// </summary>
        /// <param name="thisString">The string in question</param>
        /// <param name="prefix">A string that we want to test whether is the same as the beginning of thisString</param>
        /// <param name="isIgnoringCase">Dictates whether to ignore capitalization in the comparison</param>
        /// <returns>true if thisString includes prefix in it's beginning</returns>
        public static bool StartsWith(this string thisString, string prefix, bool isIgnoringCase)
        {
            // The full .NET Framework has StartsWith, and we could simply return, for example: trimmedText.EndsWith("hex", true, CultureInfo.InvariantCulture)
#if DEBUG
            if (thisString == null)
            {
                throw new ArgumentNullException("thisString");
            }
            if (prefix == null)
            {
                throw new ArgumentNullException("prefix");
            }
#endif
            int lengthOfThisString = thisString.Length;
            int lengthOfPrefix = prefix.Length;

            if (lengthOfPrefix > lengthOfThisString)
            {
                return false;
            }

            string pertinentPartOfThisString;
            if (lengthOfPrefix == lengthOfThisString)
            {
                pertinentPartOfThisString = thisString;
            }
            else
            {
                pertinentPartOfThisString = thisString.Substring(0, lengthOfPrefix);
            }
            // At this point we know that pertinentPartOfThisString is the same length as prefix.
            if (isIgnoringCase)
            {
                return prefix.Equals(pertinentPartOfThisString, StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                return prefix.Equals(pertinentPartOfThisString, StringComparison.InvariantCulture);
            }
        }
        #endregion StartsWith
#endif

        #region StartsWithDrive
        /// <summary>
        /// Give a filesystem path, return true if it starts with a drive-letter and colon (with any whitespace trimmed).
        /// </summary>
        /// <param name="filesystemPath">the path to check</param>
        /// <returns>true if the path starts with a disk-drive spec of the form "C:"</returns>
        public static bool StartsWithDrive(this string filesystemPath)
        {
            if (!String.IsNullOrWhiteSpace(filesystemPath))
            {
                string path = filesystemPath.Trim();
                if (path.Length >= 2 && StringLib.IsEnglishAlphabetLetter(path[0]) && path[1] == ':')
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region TestStringOfLength
        /// <summary>
        /// Return a string consisting of digits 0 through 9, repeated as many times as needed to make it the specified length.
        /// </summary>
        /// <remarks>
        /// This is envisioned as being for testing utilities that take a string input.
        /// </remarks>
        /// <param name="length">The length (in characters) of the string to return</param>
        /// <returns>A string of the given length, consisting of repeating digits</returns>
        public static string TestStringOfLength(int length)
        {
            var sb = new StringBuilder(length);
            int digit = 0;
            for (int i = 0; i < length; i++)
            {
                sb.Append(digit.ToString());
                digit++;
                if (digit > 9)
                {
                    digit = 0;
                }
            }
            return sb.ToString();
        }
        #endregion

        #region TestIfContainsAndRemove
        /// <summary>
        /// Check whether the given string contains the given word or it's plural, returning true if it does and also returning it with the test-pattern removed.
        /// The test is case-insensitive, and requires word-boundaries (ie, a word embedded within other letters is not matched).
        /// </summary>
        /// <param name="sThis">The given string to test against</param>
        /// <param name="sWord1">The pattern to check for</param>
        /// <param name="sWithout">If the pattern (or it's plural) is found within the given string, that string is returned in this argument with the pattern removed</param>
        /// <returns>true if the pattern (or it's plural) is found</returns>
        static public bool TestIfContainsAndRemove(this string sThis, string sWord1, out string sWithout)
        {
            return TestIfContainsAndRemove(sThis, sWord1, null, null, out sWithout);
        }

        /// <summary>
        /// Check whether the given string contains either of the given words or their plural, returning true if it does and also returning it with the test-pattern removed.
        /// The test is case-insensitive, and requires word-boundaries (ie, a word embedded within other letters is not matched).
        /// </summary>
        /// <param name="sThis">The given string to test against</param>
        /// <param name="sWord1">The first pattern to check for</param>
        /// <param name="sWord2">The second pattern to check for</param>
        /// <param name="sWithout">If the patterns(or their plural) are found within the given string, that string is returned in this argument with the pattern removed</param>
        /// <returns>true if either of the patterns (or plural forms) is found</returns>
        static public bool TestIfContainsAndRemove(this string sThis, string sWord1, string sWord2, out string sWithout)
        {
            return TestIfContainsAndRemove(sThis, sWord1, sWord2, null, out sWithout);
        }

        /// <summary>
        /// Check whether the given string contains any of the given words or their plural, returning true if it does and also returning it with the test-pattern removed.
        /// The test is case-insensitive, and requires word-boundaries (ie, a word embedded within other letters is not matched).
        /// </summary>
        /// <param name="sThis">The given string to test against</param>
        /// <param name="sWord1">The first pattern to check for</param>
        /// <param name="sWord2">The second pattern to check for</param>
        /// <param name="sWord3">The third pattern to check for</param>
        /// <param name="sWithout">If the patterns(or their plural) are found within the given string, that string is returned in this argument with the pattern removed from the end</param>
        /// <returns>true if any of the patterns (or plural forms) is found</returns>
        static public bool TestIfContainsAndRemove(this string sThis, string sWord1, string sWord2, string sWord3, out string sWithout)
        {
            bool bFound = false;
            string sWordFound;
            if (sThis.ContainsRoot(sWord1, out sWordFound))
            {
                bFound = true;
                sWithout = sThis.WithoutAtEnd(sWordFound);
            }
            else if (!String.IsNullOrEmpty(sWord2) && sThis.ContainsRoot(sWord2, out sWordFound))
            {
                bFound = true;
                sWithout = sThis.WithoutAtEnd(sWordFound);
            }
            else if (!String.IsNullOrEmpty(sWord3) && sThis.ContainsRoot(sWord3, out sWordFound))
            {
                bFound = true;
                sWithout = sThis.WithoutAtEnd(sWordFound);
            }
            else
            {
                sWithout = sThis;
            }
            return bFound;
        }
        #endregion // TestIfContainsAndRemove

        #region ToCardinal
        /// <summary>
        /// Given a number, express it in English words.
        /// </summary>
        /// <param name="inputNumber">The number to express</param>
        /// <returns>An expression of that given value</returns>
        public static string ToCardinal(this int inputNumber)
        {
            // This article contributed the basic algorithm, but it had errors and warranted unit-testiing.
            // http://www.hotblue.com/article0001.aspx
            int dig1, dig2, dig3, level = 0, lasttwo, threeDigits;

            string retval = "";
            string x = "";
            string[] ones ={
                "zero",
                "one",
                "two",
                "three",
                "four",
                "five",
                "six",
                "seven",
                "eight",
                "nine",
                "ten",
                "eleven",
                "twelve",
                "thirteen",
                "fourteen",
                "fifteen",
                "sixteen",
                "seventeen",
                "eighteen",
                "nineteen"
              };
            string[] tens ={
                "zero",
                "ten",
                "twenty",
                "thirty",
                "forty",
                "fifty",
                "sixty",
                "seventy",
                "eighty",
                "ninety"
              };
            string[] thou ={
                "",
                "thousand",
                "million",
                "billion",
                "trillion",
                "quadrillion",
                "quintillion"
              };

            bool isNegative = false;
            if (inputNumber < 0)
            {
                isNegative = true;
                inputNumber *= -1;
            }

            if (inputNumber == 0)
                return ("zero");

            string s = inputNumber.ToString();

            while (s.Length > 0)
            {
                // Get the three rightmost characters
                x = (s.Length < 3) ? s : s.Substring(s.Length - 3, 3);

                // Separate the three digits
                threeDigits = Int32.Parse(x);
                lasttwo = threeDigits % 100;
                dig1 = threeDigits / 100;
                dig2 = lasttwo / 10;
                dig3 = (threeDigits % 10);

                // append a "thousand" where appropriate
                if (level > 0 && dig1 + dig2 + dig3 > 0)
                {
                    retval = thou[level] + " " + retval;
                    retval = retval.Trim();
                }

                // check that the last two digits is not a zero
                if (lasttwo > 0)
                {
                    // if less than 20, use "ones" only
                    if (lasttwo < 20)
                    {
                        retval = ones[lasttwo] + " " + retval;
                    }
                    else // otherwise, use both "tens" and "ones" array
                    {
                        if (dig3 > 0)
                        {
                            retval = tens[dig2] + " " + ones[dig3] + " " + retval;
                        }
                        else // dig3 is zero
                        {
                            retval = tens[dig2] + " " + retval;
                        }
                    }
                }

                // if a hundreds part is there, translate it
                if (dig1 > 0)
                    retval = ones[dig1] + " hundred " + retval;

                s = (s.Length - 3) > 0 ? s.Substring(0, s.Length - 3) : "";
                level++;
            }

            while (retval.IndexOf("  ") > 0)
                retval = retval.Replace("  ", " ");

            retval = retval.Trim();

            if (isNegative)
                retval = "negative " + retval;

            return (retval);
        }

        /// <summary>
        /// Get the (English) word for the integer value that the given character represents, assuming it is the ASCII character for digit 0..9.
        /// </summary>
        /// <param name="fromWhatCharacter">The Char to get the integer value of</param>
        /// <returns>"one" for the '1' character, "two" for '2', ..</returns>
        /// <exception cref="ArgumentException"/>
        public static string ToCardinal(this Char fromWhatCharacter)
        {
            if (!Char.IsDigit(fromWhatCharacter))
            {
                throw new ArgumentException("fromWhatCharacter must be a digit!");
            }
            int asciiIndex = (int)fromWhatCharacter;
            int integerValue = asciiIndex - 48;
            return integerValue.ToCardinal();
        }
        #endregion

        #region ToOrdinal
        /// <summary>
        /// Extension method that returns a string that expresses the given integer as an ordinal (eg, "1st" or "first").
        /// </summary>
        /// <param name="fromNumber">The integer to be expressed as an ordinal</param>
        /// <returns>The ordinal expression, eg 1st (or first if bVerbose is true)</returns>
        /// <exception cref="ArgumentException"/>
        public static string ToOrdinal(this int fromNumber)
        {
            // This is provided for VS2008 since that does not support default parameters.
            return ToOrdinal(fromNumber, false);
        }

        /// <summary>
        /// Extension method that returns a string that expresses the given integer as an ordinal (eg, "1st" or "first").
        /// </summary>
        /// <param name="fromNumber">The integer to be expressed as an ordinal</param>
        /// <param name="beVerbose">If this flag is true, first is output instead of 1st. Default is false.</param>
        /// <returns>The ordinal expression, eg 1st (or first if bVerbose is true)</returns>
        /// <exception cref="ArgumentException"/>
        public static string ToOrdinal(this int fromNumber, bool beVerbose)
        {
            //TODO: This could use more thorough unit-testing.
            if (fromNumber < 0)
            {
                throw new ArgumentException("fromNumber argument must not be negative!");
            }
            string sResult = "";
            if (beVerbose)
            {
                switch (fromNumber)
                {
                    case 0:
                        sResult = "zeroth";
                        break;
                    case 1:
                        sResult = "first";
                        break;
                    case 2:
                        sResult = "second";
                        break;
                    case 3:
                        sResult = "third";
                        break;
                    case 4:
                        sResult = "fourth";
                        break;
                    case 5:
                        sResult = "fifth";
                        break;
                    case 6:
                        sResult = "sixth";
                        break;
                    case 7:
                        sResult = "seventh";
                        break;
                    case 8:
                        sResult = "eighth";
                        break;
                    case 9:
                        sResult = "ninth";
                        break;
                    case 10:
                        sResult = "tenth";
                        break;
                    case 11:
                        sResult = "eleventh";
                        break;
                    case 12:
                        sResult = "twelveth";
                        break;
                    case 20:
                        sResult = "twentieth";
                        break;
                    case 30:
                        sResult = "thirtieth";
                        break;
                    case 40:
                        sResult = "fortieth";
                        break;
                    case 50:
                        sResult = "fiftieth";
                        break;
                    case 60:
                        sResult = "sixtieth";
                        break;
                    case 70:
                        sResult = "seventieth";
                        break;
                    case 80:
                        sResult = "eightieth";
                        break;
                    case 90:
                        sResult = "ninetieth";
                        break;
                    case 100:
                        sResult = "hundredth";
                        break;
                    case 1000:
                        sResult = "thousandth";
                        break;
                    case 1000000:
                        sResult = "millionth";
                        break;
                    default:
                        if (fromNumber < 20)
                        {
                            sResult = fromNumber.ToCardinal() + "th";
                        }
                        else if (fromNumber < 100)
                        {
                            int tensDigit = fromNumber / 10;
                            int tensQuantity = tensDigit * 10;
                            int onesDigit = fromNumber % 10;
                            sResult = tensQuantity.ToCardinal() + " " + onesDigit.ToOrdinal(true);
                        }
                        else
                        {
                            int hundredsDigit = fromNumber / 100;
                            int hundredsValue = hundredsDigit * 100;
                            int subHundredValue = fromNumber % 100;
                            int tensDigit = subHundredValue / 10;
                            int tensValue = tensDigit * 10;
                            int onesDigit = subHundredValue % 10;
                            if (fromNumber < 1000 && tensDigit == 0)
                            {
                                if (hundredsValue == 100)
                                {
                                    sResult = "hundred and " + onesDigit.ToOrdinal(true);
                                }
                                else
                                {
                                    sResult = hundredsValue.ToCardinal() + " and " + onesDigit.ToOrdinal(true);
                                }
                            }
                            else
                            {
                                sResult = fromNumber.ToCardinal() + "th";
                            }
                        }
                        break;
                }
            }
            else // not verbose
            {
                string numberText = fromNumber.ToString();
                string positionText;
                if (fromNumber >= 10 && numberText[numberText.Length - 2] == '1')
                {
                    // teen numbers always end in 'th'
                    positionText = "th";
                }
                else
                {
                    switch (numberText[numberText.Length - 1])
                    {
                        case '0':
                            positionText = "th";
                            break;
                        case '1':
                            positionText = "st";
                            break;
                        case '2':
                            positionText = "nd";
                            break;
                        case '3':
                            positionText = "rd";
                            break;
                        default:
                            positionText = "th";
                            break;
                    }
                }
                sResult = numberText + positionText;
            }
            return sResult;
        }
        #endregion

        #region ToPlural
        /// <summary>
        /// Get a plural form of the given word. Use WithPlurality if you want a method that will return to you the singular or plural word depending upon a number.
        /// </summary>
        /// <param name="singularWord">a human-language word of the language indicated by cultureInfo</param>
        /// <returns>the plural form of singularWord, in lower-case, if it can find it</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static string ToPlural(this string singularWord)
        {
            // This is provided for VS2008 since that does not support default parameter values.
            return ToPlural(singularWord, null);
        }

        /// <summary>
        /// Get a plural form of the given word within the context of the given CultureInfo. Use WithPlurality if you want a method that will return to you the singular or plural word depending upon a number.
        /// </summary>
        /// <param name="singularWord">a human-language word of the language indicated by cultureInfo</param>
        /// <param name="cultureInfo">The culture that the word is assumed to be in. If this is null (the default value) then en is assumed.</param>
        /// <returns>the plural form of singularWord, in lower-case, if it can find it</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static string ToPlural(this string singularWord, CultureInfo cultureInfo)
        {
            if (singularWord == null)
            {
                throw new ArgumentNullException("singularWord must not be null");
            }
            if (singularWord.Length < 2)
            {
                throw new ArgumentException("singularWord must have at least two characters for this to be useful. ?");
            }
            TypeOfCasing capitalization = singularWord.GetTypeOfCasing();
            string lowercaseSingularWord = singularWord.ToLower();
            string pluralFormOfWord = lowercaseSingularWord.ToPluralLowercase(cultureInfo);
            return pluralFormOfWord.PutIntoTypeOfCasing(capitalization);
        }

        public static string ToPluralLowercase(this string singularWord)
        {
            // This is provided for VS2008 since that does not support default parameter values.
            return ToPluralLowercase(singularWord, null);
        }

        public static string ToPluralLowercase(this string singularWord, CultureInfo cultureInfo)
        {
            if (cultureInfo == null || cultureInfo.TwoLetterISOLanguageName == "en")
            {
                // TODO: English-only, at present. You've a bit of work to do on this.
                // This is a prime candidate for unit-testing.
                // This will be moved into the linguistic-database once that's up and running for .net
                // Much of the grammatical-rules came from: http://www2.gsu.edu/~wwwesl/egw/crump.htm

                // Some irregular forms that need to be dealt with first.
                string s = singularWord.ToLower();
                switch (s)
                {
                    case "abyss":
                        return "abysses";
                    case "child":
                        return "children";
                    case "court martial":
                        return "courts martial";
                    case "die":
                        return "dice";
                    case "fait accompli":
                        return "faits accomplis";
                    case "feme covert":
                        return "femes covert";
                    case "femme couverte":
                        return "femmes couvertes";
                    case "feme sole":
                        return "femes sole";
                    case "fireman":
                        return "firemen";
                    case "foot":
                        return "feet";
                    case "force majeure":
                        return "forces majeures";
                    case "goose":
                        return "geese";
                    case "heir presumptive":
                        return "heirs presumptive";
                    case "louse":
                        return "lice";
                    case "man":
                        return "men";
                    case "mouse":
                        return "mice";
                    case "ox":
                        return "oxen";
                    case "runner-up":
                        return "runners-up";
                    case "tooth":
                        return "teeth";
                    case "woman":
                        return "women";
                    // Some Italian that are retained into the English language
                    case "tempo":
                        return "tempi";
                    case "libretto":
                        return "libretti";
                    case "virtuoso":
                        return "virtuosi";
                    // Hebrew
                    case "cherub":
                        return "cherubim";
                    case "seraph":
                        return "seraphim";
                    // Greek
                    case "schema":
                        return "schemata";
                }
                // Rule: some nouns don't change at all.
                switch (s)
                {
                    case "barracks":
                    case "cod":
                    case "crossroads":
                    case "dice":
                    case "deer":
                    case "fish":
                    case "gallows":
                    case "headquarters":
                    case "means":
                    case "moose":
                    case "offspring":
                    case "perch":
                    case "series":
                    case "sheep":
                    case "species":
                    case "trout":
                        return s;
                }
                // Rule: nouns ending in -eau, take the plural form -eaux
                if (s.EndsWith("eau"))
                {
                    return s + "x";
                }
                // Rule: nouns ending in -ex, -ix taking the plural form -ices
                if (s.EndsWith("ex") || s.EndsWith("ix"))
                {
                    if (s.EndsWith("ex"))
                    {
                        return s.WithoutAtEnd("ex") + "ices";
                    }
                    else
                    {
                        return s.WithoutAtEnd("ix") + "ices";
                    }
                }
                // Rule: add -es to those that end in a sibilant sound (/s/, /z/, /ts/, /dz/).
                if (s.EndsWith("ch") || s.EndsWith("x") || s.EndsWith("z") || s.EndsWith("sh") || s.EndsWith("as") || s.EndsWith("ess") || s.EndsWith("iss"))
                {
                    return s + "es";
                }
                // Rule: If the noun ends with -y and the -y is not preceded by a vowel (or is not a proper noun, such as a name) the -y changes to -i and the plural is then -es.
                if (s.Length > 2 && s.EndsWith("y"))
                {
                    int i = s.LastIndexOf('y');
                    if (!IsAVowel(s[i - 1]))
                    {
                        if (!s.IsAProperNoun())
                        {
                            string firstPart = s.Substring(0, i);
                            return firstPart + "ies";
                        }
                    }
                }
                // Rule: some nouns that end in -f or -fe change to -ves in the plural.
                if (s.Length > 2 && (s.EndsWith("f") | s.EndsWith("fe")))
                {
                    int i = s.LastIndexOf('f');
                    string firstPart = s.Substring(0, i);
                    return firstPart + "ves";
                }
                // Rule: some nouns ending in -o take -s for the plural, while others take -es
                if (s.EndsWith("o"))
                {
                    switch (s)
                    {
                        case "echo":
                        case "domino":
                        case "embargo":
                        case "hero":
                        case "mango":
                        case "motto":
                        case "potato":
                        case "tomato":
                        case "tornado":
                        case "torpedo":
                        case "veto":
                        case "volcano":
                        case "zero":
                            return s + "es";
                        default:
                            return s + "s";
                    }
                }
                // Rule: a few that end in -us take a plural form with -a (only in technical use), while others take the plural form -i
                if (s.EndsWith("us"))
                {
                    switch (s)
                    {
                        case "circus":
                            return "circuses";
                        case "corpus":
                            return "corpora";
                        case "genus":
                            return "genera";
                        case "bus":
                            return "buses";
                        case "ignoramus":
                            return "ignoramuses";
                        case "octopus":
                            return "octopuses";
                        default:
                            return s.WithoutAtEnd("us") + "i";
                    }
                }
                // Rule: nouns ending in -um that take the plural form -a, with a few exceptions
                if (s.EndsWith("um"))
                {
                    if (s == "premium" || s == "auditorium" || s == "stadium")
                    {
                        return s + "s";
                    }
                    else
                    {
                        return s.WithoutAtEnd("um") + "a";
                    }
                }
                // Rule: nouns ending in -is that become -es in the plural form.
                if (s.EndsWith("is"))
                {
                    switch (s)
                    {
                        case "iris":
                            return "irises";
                    }
                    return s.WithoutAtEnd("is") + "es";
                }
                // Rule: nouns ending in -on that become -a
                if (s.EndsWith("on"))
                {
                    return s.WithoutAtEnd("on") + "a";
                }
                // Rule: some nouns that retain foreign plurals, ending in -a take a plural form that ends with 'ae
                if (s.EndsWith("a"))
                {
                    return s + "e";
                }
                // With have fallen through to our final, all-around-default rule. Just append the letter 's'.
                return singularWord + "s";
            }
            else
            {
                return singularWord;
            }
        }
        #endregion

        #region ToStringWithoutNuls
        /// <summary>
        /// Convert the given array of Chars, into a string - leaving off any zero characters.
        /// </summary>
        public static string ToStringWithoutNuls(this char[] sourceCharArray)
        {
            var newArray = (from c in sourceCharArray where c != 0 select c).ToArray();
            string stringResult = new string(newArray);
            return stringResult;
        }
        #endregion

        #region TypeOfCasing, methods GetTypeOfCasing and PutIntoTypeOfCasing

        public enum TypeOfCasing { Unknown, Mixed, AllLowercase, AllUppercase, Titlecased };

        /// <summary>
        /// Get an enum type value that indicates the capitalization scheme of the given string,
        /// that is, whether it's all uppercase letters, all lowercase letters, mixed, titlecased, or unknown.
        /// </summary>
        /// <param name="ofWhat">The text that we want to know the capitalization of</param>
        /// <returns>One of the enum TypeOfCasing values</returns>
        /// <exception cref="ArgumentException"/>
        /// <remarks>
        /// "Titlecase" means with only the initial letter in uppercase and the rest in lowercase,
        /// as it means in the Python language.
        /// </remarks>
        public static TypeOfCasing GetTypeOfCasing(this string ofWhat)
        {
            if (String.IsNullOrWhiteSpace(ofWhat))
            {
                throw new ArgumentException("ofWhat must not be empty");
            }
            TypeOfCasing capitalization = TypeOfCasing.Unknown;
            if (ofWhat.IsAllLowercase())
            {
                capitalization = TypeOfCasing.AllLowercase;
            }
            else if (ofWhat.IsAllUppercase())
            {
                capitalization = TypeOfCasing.AllUppercase;
            }
            else if (ofWhat.IsInTitlecase())
            {
                capitalization = TypeOfCasing.Titlecased;
            }
            else
            {
                capitalization = TypeOfCasing.Mixed;
            }
            return capitalization;
        }

        /// <summary>
        /// Given a string and an enum type value that indicates the capitalization scheme desired for that string,
        /// return a string that is that value changed to match that capitalization scheme.
        /// </summary>
        /// <param name="what">the string that we want to return a (properly-cased) copy of</param>
        /// <param name="how">A TypeOfCasing value that indicates how we want to change the capitalization</param>
        /// <returns>a copy of the string cast into the given TypeOfCasing</returns>
        /// <exception cref="ArgumentException"/>
        public static string PutIntoTypeOfCasing(this string what, TypeOfCasing how)
        {
            if (String.IsNullOrWhiteSpace(what))
            {
                throw new ArgumentException("what must not be empty");
            }
            switch (how)
            {
                case TypeOfCasing.AllLowercase:
                    return what.ToLower();
                case TypeOfCasing.AllUppercase:
                    return what.ToUpper();
                case TypeOfCasing.Titlecased:
                    return what.Titlecase();
                default:
                    return what;
            }
        }
        #endregion The enum type TypeOfCasing, methods GetTypeOfCasing and PutIntoTypeOfCasing

        #region WhatComesAfterLastSlash
        /// <summary>
        /// If the given filesystem-path contains any path-separators (either a forward or a back-slash),
        /// return the part that comes after the final path-seperator, otherwise just return the argument unchanged.
        /// </summary>
        /// <param name="path">a string that denotes the given filesystem-path</param>
        /// <returns>the part of the path that comes after the last filesystem-separator</returns>
        public static string WhatComesAfterLastSlash(this string path)
        {
            int indexOfLastSlash = path.LastIndexOf('\\');
            if (indexOfLastSlash == -1)
            {
                indexOfLastSlash = path.LastIndexOf('/');
            }
            if (indexOfLastSlash == -1)
            {
                return path;
            }
            else
            {
                return path.Substring(indexOfLastSlash + 1);
            }
        }
        #endregion

        #region WithFirstLetterCapitalized
        /// <summary>
        /// Extension-method that returns the given string with the first character in uppercase, and the remainder left as-is.
        /// </summary>
        /// <param name="sText">The string to return as first-character-capitalized</param>
        /// <returns>The given string with only the first character made to be uppercase</returns>
        public static string WithFirstLetterCapitalized(this string sText)
        {
            string sResult = "";
            if (!String.IsNullOrEmpty(sText))
            {
                string sFirstChar = sText.Substring(0, 1).ToUpper();
                string sRest = sText.Substring(1);
                sResult = sFirstChar + sRest;
            }
            return sResult;
        }
        #endregion

        #region WithFirstLetterInLowercase
        /// <summary>
        /// Extension-method that returns the given string with the first character in lowercase, and the remainder left as-is.
        /// </summary>
        /// <param name="sText">The string to return a copy of with it's first letter in lowercase</param>
        /// <returns>The given string with only the first character made to be lowercase</returns>
        public static string WithFirstLetterInLowercase(this string sText)
        {
            string sResult = "";
            if (!String.IsNullOrEmpty(sText))
            {
                string sFirstChar = sText.Substring(0, 1).ToLower();
                string sRest = sText.Substring(1);
                sResult = sFirstChar + sRest;
            }
            return sResult;
        }
        #endregion

        #region WithFlowDocumentOuterTag  and  WithoutFlowDocumentOuterTag

        public static string WithoutFlowDocumentOuterTag(this string sXAML)
        {
            if (sXAML.StartsWith(s_StandardOuterTagStart) && sXAML.EndsWith(s_StandardOuterTagEnd))
            {
                return sXAML.WithoutAtStart(s_StandardOuterTagStart).WithoutAtEnd(s_StandardOuterTagEnd);
            }
            else
            {
                return sXAML;
            }
        }

        public static string WithFlowDocumentOuterTag(this string sXAML)
        {
            string sResult;
            if (!sXAML.StartsWith(s_StandardOuterTagStart))
            {
                sResult = s_StandardOuterTagStart + sXAML;
            }
            else
            {
                sResult = sXAML;
            }
            if (!sXAML.EndsWith(s_StandardOuterTagEnd))
            {
                sResult += s_StandardOuterTagEnd;
            }
            return sResult;
        }

        static string s_StandardOuterTagStart = "<FlowDocument PagePadding=\"5,0,5,0\" AllowDrop=\"True\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">";
        static string s_StandardOuterTagEnd = "</FlowDocument>";

        #endregion

        #region WithinDoubleQuotes
        /// <summary>
        /// Return the given string surrounded by double-quotes. If what is null, represent it as "null".
        /// </summary>
        /// <param name="what">the string to return surrounded by double-quotes</param>
        /// <returns>the given string surrounded by double-quotes</returns>
        public static string WithinDoubleQuotes(this string what)
        {
            if (what == null)
            {
                return "\"null\"";
            }
            else
            {
                return "\"" + what + "\"";
            }
        }
        #endregion

        #region WithoutDoubleQuotes
        /// <summary>
        /// Return the given string, without the surrounding double-quotes if present, otherwise unchanged.
        /// </summary>
        /// <param name="what">the string to return</param>
        /// <returns>the given string without any double-quotes</returns>
        public static string WithoutDoubleQuotes(this string what)
        {
            if (what == null)
            {
                return String.Empty;
            }
            else
            {
                int len = what.Length;
                if (len == 2 && what[0] == '\"' && what[1] == '\"')
                {
                    return String.Empty;
                }
                else if (len > 2 && what[0] == '\"' && what[len - 1] == '\"')
                {
                    return what.Substring(1, len - 2);
                }
                else
                {
                    return what;
                }
            }
        }
        #endregion

        #region WithinParentheses
        /// <summary>
        /// Return the given string surrounded by parentheses. If what is null, represent it as "null".
        /// </summary>
        /// <param name="what">the string to return surrounded by parentheses</param>
        /// <returns>the given string surrounded by parentheses</returns>
        public static string WithinParentheses(this string what)
        {
            if (what == null)
            {
                return "(null)";
            }
            else
            {
                return "(" + what + ")";
            }
        }
        #endregion

        #region WithoutAtStart
        /// <summary>
        /// Return this string with the given text removed from the beginning of it, if it's present.
        /// </summary>
        /// <param name="charToRemoveAtStart">the character to remove</param>
        /// <returns>the given string without that character if it was present, otherwise just returns itself unchanged</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <remarks>
        /// The arguments do NOT get stripped of leading-space, and the text-comparison is case-sensitive.
        /// </remarks>
        public static string WithoutAtStart(this string thisString, Char charToRemoveAtStart)
        {
            if (thisString == null)
            {
                throw new ArgumentNullException("thisString");
            }
            if (thisString.Length > 0)
            {
                if (thisString[0] == charToRemoveAtStart)
                {
                    return thisString.Substring(1);
                }
            }
            return thisString;
        }

        /// <summary>
        /// Return this string with the given text removed from the beginning of it, if it's present.
        /// </summary>
        /// <param name="textToRemoveAtStart">the text to remove</param>
        /// <returns>the string without the given text at the beginning if it was present, otherwise just returns itself unchanged</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <remarks>
        /// The arguments do NOT get stripped of leading-space, and the text-comparison is case-sensitive.
        /// </remarks>
        public static string WithoutAtStart(this string thisString, string textToRemoveAtStart)
        {
            if (thisString == null)
            {
                throw new ArgumentNullException("thisString");
            }
            if (!String.IsNullOrEmpty(textToRemoveAtStart))
            {
                int n = textToRemoveAtStart.Length;
                if (thisString.Length >= n)
                {
                    if (thisString.StartsWith(textToRemoveAtStart))
                    {
                        return thisString.Substring(n);
                    }
                }
            }
            return thisString;
        }

        /// <summary>
        /// Return this string with the given text removed from the beginning of it, if it's present.
        /// </summary>
        /// <param name="textToRemoveAtStart">The text to remove</param>
        /// <param name="ignoreCase">Indicates whether to ignore whether the prefix to remove appears as uppercase or lower (it gets removed regardless)</param>
        /// <returns>This, without the given text at the beginning, if it was present, otherwise just returns itself unchanged</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <remarks>
        /// The arguments do NOT get stripped of leading-space, and the text-comparison is case-sensitive only if ignoreCase is false.
        /// </remarks>
        public static string WithoutAtStart(this string thisString, string textToRemoveAtStart, bool ignoreCase)
        {
            if (thisString == null)
            {
                throw new ArgumentNullException("thisString must not be null.");
            }
            if (!String.IsNullOrEmpty(textToRemoveAtStart))
            {
                int n = textToRemoveAtStart.Length;
                if (thisString.Length >= n)
                {
#if SILVERLIGHT
                    if (thisString.StartsWith(textToRemoveAtStart, ignoreCase))
#else
                    if (thisString.StartsWith(textToRemoveAtStart, ignoreCase, CultureInfo.InvariantCulture))
#endif
                    {
                        return thisString.Substring(n);
                    }
                }
            }
            return thisString;
        }
        #endregion

        #region WithoutAtEnd
        /// <summary>
        /// Return this string with the given character removed from the end, if it's present.
        /// </summary>
        /// <param name="thisString"></param>
        /// <param name="characterToRemoveFromEnd">The char to remove from the end of it, if it is present</param>
        /// <returns>thisString, without the given char at the end, if it was present, otherwise just returns itself unchanged</returns>
        public static string WithoutAtEnd(this string thisString, char characterToRemoveFromEnd)
        {
            if (String.IsNullOrWhiteSpace(thisString))
            {
                return String.Empty;
            }
            else
            {
                int n = thisString.Length;
                if (thisString[n - 1] == characterToRemoveFromEnd)
                {
                    return thisString.Substring(0, n - 1);
                }
                return thisString;
            }
        }

        /// <summary>
        /// Return this string with the given text removed from the end, if it's present.
        /// </summary>
        /// <param name="thisString"></param>
        /// <param name="textToRemoveAtEnd">The text to remove from the end of it, if it is present</param>
        /// <returns>thisString, without the given text at the end, if it was present, otherwise just returns itself unchanged</returns>
        public static string WithoutAtEnd(this string thisString, string textToRemoveAtEnd)
        {
            if (!String.IsNullOrEmpty(textToRemoveAtEnd))
            {
                int nToRemove = textToRemoveAtEnd.Length;
                int n = thisString.Length;
                if (n >= nToRemove)
                {
                    if (thisString.EndsWith(textToRemoveAtEnd))
                    {
                        return thisString.Substring(0, n - nToRemove);
                    }
                }
            }
            return thisString;
        }

        /// <summary>
        /// Return this string with the given text removed from the end, if it's present.
        /// </summary>
        /// <param name="thisString"></param>
        /// <param name="textToRemoveAtEnd">The text to remove from the end of it, if it is present</param>
        /// <param name="ignoreCase">Indicates whether to ignore whether the suffix to remove appears as uppercase or lower</param>
        /// <returns>thisString, without the given text at the end, if it was present, otherwise just returns itself unchanged</returns>
        public static string WithoutAtEnd(this string thisString, string textToRemoveAtEnd, bool ignoreCase)
        {
            if (!String.IsNullOrEmpty(textToRemoveAtEnd))
            {
                int nToRemove = textToRemoveAtEnd.Length;
                int n = thisString.Length;
                if (n >= nToRemove)
                {
#if SILVERLIGHT
                    if (thisString.EndsWith(textToRemoveAtEnd, ignoreCase))
#else
                    if (thisString.EndsWith(textToRemoveAtEnd, ignoreCase, CultureInfo.InvariantCulture))
#endif
                    {
                        return thisString.Substring(0, n - nToRemove);
                    }
                }
            }
            return thisString;
        }
        #endregion

        #region WithoutTrailingLineEndCharacters
        /// <summary>
        /// Return the given inputText with any of the (common patterns of trailing carriage-return or new-line characters) removed.
        /// </summary>
        /// <param name="inputText">the input string to strip off any trailing CR/LF from</param>
        /// <returns>a copy of the input string without the trailing 1 or 2 characters if they are CR, LF, CR+LF or LF+CR</returns>
        public static string WithoutTrailingLineEndCharacters(this string inputText)
        {
            // Remove from the end either CR, LF, CR+LF, or LF+CR.
            if (inputText == null)
            {
                throw new ArgumentNullException("argument to method WithoutTrailingLineEndCharacters must not be null.");
            }
            if (String.IsNullOrWhiteSpace(inputText))
            {
                return String.Empty;
            }
            int iLastChar = inputText.Length - 1;
            if (inputText.Length >= 2)
            {
                if (inputText[iLastChar - 1] == '\r' && inputText[iLastChar] == '\n')
                {
                    return inputText.Substring(0, iLastChar - 1);
                }
                else if (inputText[iLastChar - 1] == '\n' && inputText[iLastChar] == '\r')
                {
                    return inputText.Substring(0, iLastChar - 1);
                }
            }
            if (inputText[iLastChar] == '\n')
            {
                return inputText.Substring(0, iLastChar);
            }
            else if (inputText[iLastChar] == '\r')
            {
                return inputText.Substring(0, iLastChar);
            }
            return inputText;
        }
        #endregion

        #region WithoutLeadingLineEndCharacters
        /// <summary>
        /// Return the given inputText with any of the (common patterns of leading carriage-return or new-line characters) removed.
        /// </summary>
        /// <param name="inputText">the input string to strip off any leading CR/LF from</param>
        /// <returns>a copy of the input string without the leading 1 or 2 characters if they are CR, LF, CR+LF or LF+CR</returns>
        public static string WithoutLeadingLineEndCharacters(this string inputText)
        {
            // Remove from the beginning either CR, LF, CR+LF, or LF+CR.
            if (inputText == null)
            {
                throw new ArgumentNullException("argument to method WithoutLeadingLineEndCharacters must not be null.");
            }
            if (String.IsNullOrWhiteSpace(inputText))
            {
                return String.Empty;
            }
            if (inputText.Length >= 2)
            {
                if (inputText[0] == '\r' && inputText[1] == '\n')
                {
                    return inputText.Substring(2);
                }
                else if (inputText[0] == '\n' && inputText[1] == '\r')
                {
                    return inputText.Substring(2);
                }
            }
            if (inputText[0] == '\n')
            {
                return inputText.Substring(1);
            }
            else if (inputText[0] == '\r')
            {
                return inputText.Substring(1);
            }
            return inputText;
        }
        #endregion

        #region WithoutTrailingZeros
        /// <summary>
        /// Given a string that presumably represents a decimal number,
        /// return that string without any excess trailing zeros after the decimal-point.
        /// </summary>
        /// <param name="sNumberString">The source string to (potentially) trim zeros from</param>
        /// <returns>A copy of the source string, trimmed</returns>
        public static string WithoutTrailingZeros(this string sNumberString)
        {
            if (String.IsNullOrEmpty(sNumberString))
            {
                return String.Empty;
            }
            else
            {
                int n = sNumberString.Length;
                // Get the position of the decimal point (English culture),
                // or whichever symbol the current culture uses for the decimal-point.
                NumberFormatInfo formatInfo = CultureInfo.CurrentCulture.NumberFormat;
                string sDecimalSymbol = formatInfo.NumberDecimalSeparator;
                // Determine what we should be using for a comma-separator.
                string sGroupSeparator = formatInfo.NumberGroupSeparator;
                char cGroupSeparator = ',';
                if (!String.IsNullOrEmpty(sGroupSeparator))
                {
                    cGroupSeparator = sGroupSeparator[0];
                }
                int iPositionOfDecimal = sNumberString.IndexOf(sDecimalSymbol);
                if (iPositionOfDecimal < 0)
                {
                    // No decimal-point, so just return the string unchanged.
                    return sNumberString;
                }
                else // there is a decimal-point.
                {
                    // Are trailing zeros?
                    int iPositionOfLastCharToKeep = n - 1;
                    for (int i = n - 1; i >= iPositionOfDecimal; i--)
                    {
                        if (sNumberString[i] == '0')
                        {
                            iPositionOfLastCharToKeep = i - 1;
                        }
                        else if (i == iPositionOfDecimal)
                        {
                            // There are only zeros (or nothing) beyond the decimal-point,
                            // so remove the decimal-point itself.
                            iPositionOfLastCharToKeep = iPositionOfDecimal - 1;
                            break;
                        }
                        else if (sNumberString[i] != cGroupSeparator)
                        {
                            // This character is not a zero, and it's not a decimal - so we're done.
                            break;
                        }
                    }
                    return sNumberString.Substring(0, iPositionOfLastCharToKeep + 1);
                }
            }
        }
        #endregion

        #region WithPlurality
        /// <summary>
        /// Given the name of a countable unit, make a phrase that denotes the quantity of that unit in proper grammar for the current thread's CurrentCulture setting.
        /// For example, WithPlurality(2,"meter") would return "2 meters".
        /// If the plural form is formed by other than simply appending "s", you need to use the overloading of this method that includes
        /// the plural as a third parameter.
        /// </summary>
        /// <param name="nQuantity">The count of these things to express</param>
        /// <param name="sUnitName">The (singular) name of the unit/noun/whatever that you want to express a quantity of</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        static public string WithPlurality(this int nQuantity, string sUnitName)
        {
            if (sUnitName == null)
            {
                throw new ArgumentNullException("sUnitName must not be null");
            }
            return WithPlurality(nQuantity, sUnitName, sUnitName.ToPlural());
        }

        /// <summary>
        /// Given the name of a countable unit, make a phrase that denotes the quantity of that unit in proper English grammar.
        /// For example, WithPlurality(2,"inch","inches") would return "2 inches".
        /// If the plural form is simply the singular + "s", then you can omit the 2nd parameter.
        /// </summary>
        /// <param name="nQuantity">The count of these things to express</param>
        /// <param name="sUnitNameSingular">The (singular) name of the unit/noun/whatever that you want to express a quantity of</param>
        /// <param name="sUnitNamePlural">The (plural) name of the unit/noun/whatever that you want to express a quantity of</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        static public string WithPlurality(this int nQuantity, string sUnitNameSingular, string sUnitNamePlural)
        {
            if (sUnitNameSingular == null)
            {
                throw new ArgumentNullException("sUnitNameSingular must not be null");
            }
            if (sUnitNamePlural == null)
            {
                throw new ArgumentNullException("sUnitNamePlural must not be null");
            }
            if (String.IsNullOrWhiteSpace(sUnitNamePlural))
            {
                throw new ArgumentException("sUnitNamePlural must not be empty");
            }
            string r;
            if (nQuantity == 0)
            {
                r = "no " + sUnitNamePlural;
            }
            else if (nQuantity == 1)
            {
                r = "one " + sUnitNameSingular;
            }
            else
            {
                r = nQuantity.ToString() + " " + sUnitNamePlural;
            }
            return r;
        }
        #endregion

        #region WithSlashOnEnd
        /// <summary>
        /// Return the given string with a back-slash on the end, adding one if it doesn't already have one.
        /// </summary>
        /// <param name="inputText">the string to add a back-slash to</param>
        /// <returns>the inputText with a back-slash appended if necessary</returns>
        public static string WithSlashOnEnd(this string inputText)
        {
            if (inputText == null)
            {
                throw new ArgumentNullException("argument to method WithSlashOnEnd must not be null.");
            }
            if (String.IsNullOrWhiteSpace(inputText))
            {
                return @"\";
            }
            int n = inputText.Length;
            if (inputText[n - 1] != '\\')
            {
                return inputText + @"\";
            }
            else
            {
                return inputText;
            }
        }
        #endregion
    }
}
