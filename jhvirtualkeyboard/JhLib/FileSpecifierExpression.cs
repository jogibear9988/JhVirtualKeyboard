using System;


namespace JhLib
{
    /// <summary>
    /// This class comprises a file-specification wildcard expression, such as "*.txt" which would denote all files that end in ".txt".
    /// It provides for multiple wildcard expressions separated by semicolons or commas.
    /// </summary>
    public class FileSpecifierExpression
    {
        #region constructor
        /// <summary>
        /// Create a new FileSpecifierExpression object based upon the given wildcard-expression string,
        /// which may contain multiple wildcard expressions separated by semicolons or commas.
        /// </summary>
        /// <param name="fileSpec">the wildcard-expression string</param>
        public FileSpecifierExpression(string fileSpec)
        {
            _fileSpec = fileSpec.Replace(';', ',').Trim().ToLower();
            if (_fileSpec.Equals("*") || _fileSpec.Equals("*.*"))
            {
                _isNonSpecific = true;
            }
            else
            {
                int numberOfCommas = _fileSpec.NumberOf(',');
                if (numberOfCommas > 0)
                {
                    _hasMultipleSpecs = true;
                }
            }
        }
        #endregion

        #region FileSpec
        /// <summary>
        /// Get the wildcard-expression that this FileSpecifierExpression is based upon, as a string.
        /// </summary>
        public string FileSpec
        {
            get { return _fileSpec; }
        }
        #endregion

        #region FileSpecMatchesPath
        /// <summary>
        /// Return true if the given pathname matches against the fileSpec - which may include wildcard characters * or ?.
        /// fileSpec may also contain multiple specs, separated by commas or semicolons. The checking is not case-sensitive.
        /// </summary>
        /// <param name="fileSpec">the File-Specification (filespec) to test the pathname against. An ArgumentNullException is thrown if this is null.</param>
        /// <param name="pathnameToTest">the file pathname to test against the file-spec. An ArgumentNullException is thrown if this is null.</param>
        /// <returns>true if the pathnameToTest matches against the fileSpec</returns>
        public static bool FileSpecMatchesPath(string fileSpec, string pathnameToTest)
        {
            if (fileSpec == null)
            {
                throw new ArgumentNullException("fileSpec");
            }
            if (pathnameToTest == null)
            {
                throw new ArgumentNullException("pathnameToTest");
            }
            if (fileSpec == "*" || fileSpec == "*.*")
            {
                return true;
            }
            string filespec = fileSpec.Replace(';', ',').Trim().ToLower();
            string path = pathnameToTest.Trim().ToLower();
            int lengthOfFileSpec = filespec.Length;

            // *.cs, *.xaml
            int numberOfCommas = filespec.NumberOf(',');

            if (numberOfCommas > 0)
            {
                string[] filespecs = filespec.Split(new[] { ',' });
                foreach (string spec in filespecs)
                {
                    if (FileSpecMatchesPath(spec, path))
                    {
                        return true;
                    }
                }
                return false;
            }
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
            int indexOfQ = fileSpec.IndexOf('?');
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

        #region IsNonSpecific
        /// <summary>
        /// Get whether this expression specifies nothing specific - that is, if it is "*.*" or just "*".
        /// </summary>
        public bool IsNonSpecific
        {
            get { return _isNonSpecific; }
        }
        #endregion

        #region HasMultipleSpecs
        /// <summary>
        /// Get whether this FileSpecifier has a FileSpec that is composed of multiple distinct patterns (which are logically ORd)
        /// such as "*.cs, *.xaml" for example.
        /// </summary>
        public bool HasMultipleSpecs
        {
            get { return _hasMultipleSpecs; }
        }
        #endregion

        #region MatchesPath
        /// <summary>
        /// Return true if the given pathname matches against the FileSpec that this object is based upon.
        /// The checking is not case-sensitive.
        /// </summary>
        /// <param name="pathnameToTest">the file pathname to test against the file-spec. An ArgumentNullException is thrown if this is null.</param>
        /// <returns>true if the pathnameToTest matches against the FileSpec of this object</returns>
        public bool MatchesPath(string pathnameToTest)
        {
            return FileSpecifierExpression.FileSpecMatchesPath(_fileSpec, pathnameToTest);
        }
        #endregion

        #region fields

        /// <summary>
        /// This string is the wildcard-expression that this FileSpecifierExpression is based upon.
        /// </summary>
        private readonly string _fileSpec;
        /// <summary>
        /// This flag indicates whether this FileSpecifier has a FileSpec that is composed of multiple distinct patterns
        /// (which are logically ORd) such as "*.cs, *.xaml".
        /// </summary>
        private readonly bool _hasMultipleSpecs;
        /// <summary>
        /// This flag indicates whether this expression specifies nothing specific - that is, if it is "*.*" or just "*".
        /// </summary>
        private readonly bool _isNonSpecific;

        #endregion fields
    }
}
