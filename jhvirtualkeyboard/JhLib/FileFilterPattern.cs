// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhVirtualKeyboardDemoApp</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Serialization;


namespace JhLib
{
    public enum FilesystemMatchType { FilesAndFolders, FilesOnly, FoldersOnly }

    public enum InclusivenessType { Include, Exclude }

    /// <summary>
    /// A FileFilterPattern is simply a file-spec and a flag that indicates whether that file-spec indicates what to include,
    /// or what to exclude.
    /// </summary>
    public class FileFilterPattern
    {
        #region Constructors
        /// <summary>
        /// Constructor that takes arguments to specify every part of it's state.
        /// </summary>
        /// <param name="include">denotes whether this FileFilterPattern specifies files to include, vs to exclude</param>
        /// <param name="what">what to include or exclude (ie, a file-spec)</param>
        /// <param name="matchType">what kind of filesystem-object this pattern can match</param>
        public FileFilterPattern(bool include, string what, FilesystemMatchType matchType)
        {
            _isInclusion = include;
            _fileSpec = what;
            _matchType = matchType;
        }

        public FileFilterPattern()
        {
            _fileSpec = String.Empty;
        }
        #endregion

        #region MatchesPath
        /// <summary>
        /// Return true if the given pathname matches against the fileSpec - which may include wildcard characters * or ?.
        /// fileSpec may also contain multiple specs, separated by commas or semicolons. The checking is not case-sensitive.
        /// </summary>
        /// <param name="pathnameToTest">The file pathname to test against the file-spec. An ArgumentNullException is thrown if this is null.</param>
        /// <returns>True if the given path matches this file-spec, regardless of the state of IsInclusion</returns>
        public bool MatchesPath(string pathnameToTest)
        {
            if (pathnameToTest == null)
            {
                throw new ArgumentNullException("pathnameToTest");
            }
            if (String.IsNullOrWhiteSpace(pathnameToTest))
            {
                throw new ArgumentException("pathnameToTest should not be empty");
            }
            // FileSpecs of *, or *.*, match everything.
            if (FileSpec == "*" || FileSpec == "*.*")
            {
                return true;
            }
            string filespec = _fileSpec.ToLower();
            string path = pathnameToTest.Trim().ToLower();
            int lengthOfFileSpec = filespec.Length;

            // A.BA*
            int indexOfLastDot = filespec.LastIndexOf('.');
            int indexOfAsterisk = filespec.IndexOf('*');
            if (indexOfAsterisk == lengthOfFileSpec - 1)
            //   if (indexOfLastDot >= 0 && indexOfAsterisk > indexOfLastDot && indexOfAsterisk == lengthOfFileSpec - 1)
            {
                string partBeforeAsterisk = filespec.Substring(0, indexOfAsterisk);
                return path.StartsWith(partBeforeAsterisk);
                //string regularExpression = partBeforeAsterisk + @"([A-Za-z0-9\-\.])+.";
                //Match match = Regex.Match(pathnameToTest, regularExpression, RegexOptions.IgnoreCase);
                //return match.Success;
            }
            // *.cs or Z*.EXE
            int numberOfAsterisks = filespec.NumberOf('*');
            if (numberOfAsterisks == 1)
            {
                if (indexOfAsterisk == indexOfLastDot - 1)
                {
                    int indexOfLastDotWithinPath = path.LastIndexOf('.');
                    if (indexOfLastDotWithinPath > 0)
                    {
                        string partBeforeAsterisk = filespec.Substring(0, indexOfAsterisk);
                        if (path.StartsWith(partBeforeAsterisk))
                        {
                            string partAfterAsterisk = filespec.Substring(indexOfAsterisk + 1);
                            return path.EndsWith(partAfterAsterisk);
                        }
                    }
                }
            }
            // ABC?.BAT
            int indexOfQ = filespec.IndexOf('?');
            // If there is a ?
            if (indexOfQ >= 0)
            {
                // If the ? is not the first character, and it immediately precedes the period,
                if (indexOfQ > 0 && indexOfLastDot == indexOfQ + 1)
                {
                    string partBeforeQ = filespec.Substring(0, indexOfQ);
                    if (path.StartsWith(partBeforeQ))
                    {
                        string partAfterQ = filespec.Substring(indexOfLastDot);
                        return path.EndsWith(partAfterQ);
                    }

                }
            }
            // Simple case of A == B ?
            if (path.Equals(filespec))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region ExcludesPath
        /// <summary>
        /// Return true if the given path is matched against this filespec and this is an exclusioin,
        /// false if it matches and this is an inclusion, or null if it does not match.
        /// </summary>
        /// <param name="pathnameToTest">The file pathname to test against the file-spec.</param>
        /// <returns>null if no match, otherwise false if IsInclusion or true if IsExclusion</returns>
        public bool? ExcludesPath(string pathnameToTest)
        {
            if (this.MatchesPath(pathnameToTest))
            {
                return IsExclusion;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region IncludesPath
        /// <summary>
        /// Return true if the given path is matched against this filespec and this is an inclusioin,
        /// false if it matches and this is an exclusion, or null if it does not match.
        /// </summary>
        /// <param name="pathnameToTest">The file pathname to test against the file-spec.</param>
        /// <returns>null if no match, otherwise true if IsInclusion or false if IsExclusion</returns>
        public bool? IncludesPath(string pathnameToTest)
        {
            if (this.MatchesPath(pathnameToTest))
            {
                return _isInclusion;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region FileSpec
        /// <summary>
        /// Get or set what to include or exclude. This may be a filename, or else a file-spec such as "*.obj".
        /// </summary>
        public string FileSpec
        {
            get { return _fileSpec; }
            set { _fileSpec = value; }
        }
        #endregion

        #region IsInclusion
        /// <summary>
        /// Get or set whether this FileFilterPattern specifies files to include, as opposed to files to exclude.
        /// </summary>
        [XmlIgnore]
        public bool IsInclusion
        {
            get { return _isInclusion; }
            set { _isInclusion = value; }
        }
        #endregion

        #region IsExclusion
        /// <summary>
        /// Get or set whether this FileFilterPattern specifies files to exclude, as opposed to files to include.
        /// </summary>
        [XmlIgnore]
        public bool IsExclusion
        {
            get { return !_isInclusion; }
            set { _isInclusion = !value; }
        }
        #endregion

        #region Inclusiveness
        /// <summary>
        /// Get or set whether this FileFilterPattern specifies files to include, as opposed to files to exclude.
        /// </summary>
        public InclusivenessType Inclusiveness
        {
            get
            {
                if (_isInclusion)
                    return InclusivenessType.Include;
                else
                    return InclusivenessType.Exclude;
            }
            set
            {
                //bool previousValue = _isInclusion;
                if (value == InclusivenessType.Include)
                {
                    _isInclusion = true;
                }
                else
                {
                    _isInclusion = false;
                }
                //if (_isInclusion != previousValue)
                //{
                //    Notify("Inclusiveness");
                //}
            }
        }
        #endregion

        #region MatchType
        /// <summary>
        /// Get or set what kind of filesystem-object this pattern can match. Defaults to FilesAndFolders.
        /// </summary>
        public FilesystemMatchType MatchType
        {
            get { return _matchType; }
            set { _matchType = value; }
        }
        #endregion

        #region ToString
        /// <summary>
        /// Override the ToString method to provide a more useful description of this object.
        /// </summary>
        /// <returns>An indication of it's state</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("FileFilterPattern(");
            if (IsExclusion)
            {
                sb.Append("exclusion,");
            }
            sb.Append(MatchType.ToString());
            sb.Append(",");
            if (String.IsNullOrWhiteSpace(_fileSpec))
            {
                sb.Append("no spec");
            }
            else
            {
                sb.Append(FileSpec);
            }
            sb.Append(")");
            return sb.ToString();
        }
        #endregion

        #region ToShortString
        /// <summary>
        /// Return a shorter version of what ToString would yield, as would be useful for a list to be displayed.
        /// </summary>
        /// <returns></returns>
        public string ToShortString()
        {
            var sb = new StringBuilder();
            if (IsExclusion)
            {
                sb.Append("exclude-");
            }
            if (String.IsNullOrWhiteSpace(_fileSpec))
            {
                sb.Append("(no-spec)");
            }
            else
            {
                sb.Append(FileSpec);
            }
            return sb.ToString();
        }
        #endregion

        #region fields

        /// <summary>
        /// This denotes what to include or exclude, and may be a filename or else a file-spec such as "*.obj"
        /// </summary>
        private string _fileSpec;

        /// <summary>
        /// This determines what kind of filesystem-object this pattern can match.
        /// </summary>
        private FilesystemMatchType _matchType = FilesystemMatchType.FilesAndFolders;

        /// <summary>
        /// If true then this FileFilterPattern specifies files to include, if false - to exclude.
        /// </summary>
        private bool _isInclusion;

        //private InclusivenessType _inclusiveness;

        #endregion fields
    }

    public class FileFilterPatterns : ObservableCollection<FileFilterPattern>
    {
        #region ExcludesPath
        /// <summary>
        /// Return true if the given path is matched against this filespec and this is an exclusioin,
        /// false if it matches and this is an inclusion, or null if it does not match.
        /// </summary>
        /// <param name="pathnameToTest">The file pathname to test against the file-spec.</param>
        /// <returns>null if no match, otherwise false if IsInclusion or true if IsExclusion</returns>
        public bool ExcludesPath(string pathnameToTest)
        {
            foreach (var fileFilterPattern in this)
            {
                if (fileFilterPattern.ExcludesPath(pathnameToTest) == true)
                {
                    return true;
                }
            }
            // Since none of the patterns excluded this file, consider it as NOT excluded.
            return false;
        }
        #endregion

        #region IncludesPath
        /// <summary>
        /// Return true if the given path is matched against this filespec and this is an inclusioin,
        /// false if it matches and this is an exclusion, or null if it does not match.
        /// </summary>
        /// <param name="pathnameToTest">The file pathname to test against the file-spec.</param>
        /// <returns>null if no match, otherwise true if IsInclusion or false if IsExclusion</returns>
        public bool IncludesPath(string pathnameToTest)
        {
            foreach (var fileFilterPattern in this)
            {
                if (fileFilterPattern.ExcludesPath(pathnameToTest) == true)
                {
                    return false;
                }
            }
            // Since none of the patterns excluded this file, consider it included.
            return true;
        }
        #endregion
    }
}
