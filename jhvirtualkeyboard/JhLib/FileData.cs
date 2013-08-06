using System;
using System.IO;
using System.Security.Cryptography;


namespace DLib
{
    #region class FileData
    /// <summary>
    /// Contains information about a file returned by the 
    /// <see cref="FastDirectoryEnumerator"/> class.
    /// </summary>
    [Serializable]
    public class FileData : IComparable<FileData>
    {
        #region Constructors

        public FileData()
        {
        }

        /// <summary>
        /// copy-constructor
        /// </summary>
        /// <param name="otherFile">The FileData to make a copy of</param>
        public FileData(FileData otherFile)
        {
            this._sName = otherFile._sName;
            this._path = otherFile._path;
            this.Attributes = otherFile.Attributes;
            this.CreationTimeUtc = otherFile.CreationTimeUtc;
            this.LastAccessTimeUtc = otherFile.LastAccessTimeUtc;
            this.LastWriteTimeUtc = otherFile.LastWriteTimeUtc;
            this._size = otherFile._size;
            this._MD5Hash = otherFile._MD5Hash;
            this._isToBeAvoided = otherFile._isToBeAvoided;
            this._isUnableToAccess = otherFile._isUnableToAccess;
            if (otherFile._note != null)
            {
                this._note = otherFile._note;
            }
        }
        #endregion

        #region Name

        private string _sName;

        /// <summary>
        /// Get the Name of the file
        /// </summary>
        public string Name
        {
            get
            {
                if (_sName == null)
                {
                    _sName = "";
                }
                return _sName;
            }
        }
        #endregion

        #region Note

        private string _note;

        /// <summary>
        /// Get/set a string note (which may be anything) that was given to the file
        /// </summary>
        public string Note
        {
            get
            {
                if (_note == null)
                {
                    _note = String.Empty;
                }
                return _note;
            }
            set { _note = value; }
        }
        #endregion

        #region Path
        /// <summary>
        /// Full path to the file.
        /// </summary>
        private string _path;

        public string Path
        {
            get { return _path; }
        }
        #endregion

        #region Directory
        /// <summary>
        /// Get the Pathname for this file but without the filename itself (ie, the folder).
        /// </summary>
        public string Directory
        {
            get
            {
                string sDirectory = this.Path.WithoutAtEnd(this.Name);
                return sDirectory;
            }
        }
        #endregion

        #region IsDirectory
        /// <summary>
        /// Get whether this FileData represents a directory (ie., a "folder") as opposed to a file.
        /// </summary>
        public bool IsDirectory
        {
            get { return (this.Attributes & FileAttributes.Directory) != 0; }
        }
        #endregion

        #region IsHiddenOrSystemOrTemporary
        /// <summary>
        /// Get true if this file is either Hidden, System, or Temporary.
        /// </summary>
        public bool IsHiddenOrSystemOrTemporary
        {
            get { return ((this.Attributes & FileAttributes.Hidden) != 0 || (this.Attributes & FileAttributes.System) != 0 || (this.Attributes & FileAttributes.Temporary) != 0); }
        }
        #endregion

        #region IsNormal
        /// <summary>
        /// Get whether this file is "Normal" and has no other attributes set.
        /// </summary>
        public bool IsNormal
        {
            get { return (this.Attributes & FileAttributes.Normal) != 0; }
        }
        #endregion

        #region Extension
        /// <summary>
        /// Get the filename extension of this file, without the period (as opposed to FileInfo.Extension which does include the period).
        /// </summary>
        public string Extension
        {
            get
            {
                string sExtension = new FileInfo(this.Path).Extension;
                // Remove any leading period from the extension to be returned.
                if (sExtension.Length > 1 && sExtension.StartsWith("."))
                {
                    return sExtension.Substring(1);
                }
                return sExtension;
            }
        }
        #endregion

        #region Attributes
        /// <summary>
        /// Attributes of the file.
        /// </summary>
        public FileAttributes Attributes;
        #endregion

        #region time-stamps

        public DateTime CreationTime
        {
            get { return this.CreationTimeUtc.ToLocalTime(); }
        }

        /// <summary>
        /// File creation time in UTC
        /// </summary>
        public readonly DateTime CreationTimeUtc;

        /// <summary>
        /// Gets the last access time in local time.
        /// </summary>
        public DateTime LastAccesTime
        {
            get { return this.LastAccessTimeUtc.ToLocalTime(); }
        }

        /// <summary>
        /// File last access time in UTC
        /// </summary>
        public readonly DateTime LastAccessTimeUtc;

        /// <summary>
        /// Gets the last access time in local time.
        /// </summary>
        public DateTime LastWriteTime
        {
            get { return this.LastWriteTimeUtc.ToLocalTime(); }
        }

        /// <summary>
        /// File last write time in UTC
        /// </summary>
        public DateTime LastWriteTimeUtc;

        #endregion time-stamps

        #region Size
        /// <summary>
        /// Size of the file in bytes
        /// </summary>
        public readonly long _size;

        public long Size
        {
            get { return _size; }
        }
        #endregion

        #region MD5Hash

        // Used for lazy instantiation
        private byte[] _MD5Hash;

        public byte[] MD5Hash
        {
            get
            {
                if (!IsToBeAvoided)
                {
                    if (_MD5Hash == null)
                    {
                        try
                        {
                            using (FileStream fs = File.OpenRead(this.Path))
                            {
                                // Convert the input string to a byte array and compute the hash.
                                _MD5Hash = _md5Hasher.ComputeHash(fs);
                            }
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine("IOException: " + ex.Message);
                            this.IsToBeAvoided = true;
                            _MD5Hash = null;
                        }
                    }
                }
                return _MD5Hash;
            }
        }

        private static MD5 _md5Hasher = MD5.Create();

        #endregion

        #region IsToBeAvoided

        private bool _isToBeAvoided;

        /// <summary>
        /// Get/set a flag that indicates whether this file threw an exception when we attempted to access it's contents.
        /// </summary>
        public bool IsToBeAvoided
        {
            get { return _isToBeAvoided; }
            set
            {
                if (value != _isToBeAvoided)
                {
                    _isToBeAvoided = value;
                }
            }
        }
        #endregion

        #region IsUnableToAccess

        private bool _isUnableToAccess;

        /// <summary>
        /// Get/set whether we were unable to access this file, as from a security violation.
        /// </summary>
        public bool IsUnableToAccess
        {
            get { return _isUnableToAccess; }
            set
            {
                if (value != _isUnableToAccess)
                {
                    _isUnableToAccess = value;
                }
            }
        }
        #endregion

        #region IsSameAs
        /// <summary>
        /// Determines whether this file has the exact same content as the given file, based upon the file size and the computed MD5 cryptographic hash of the content.
        /// </summary>
        /// <param name="otherFile">The file to compare against</param>
        /// <returns>True if the two files have the exact same content and can be read without any problem, barring an MD5 hash collision</returns>
        public bool IsSameAs(FileData otherFile)
        {
            // If either file gave us a problem before in trying to read it, just return false.
            if (!this.IsToBeAvoided && !otherFile.IsToBeAvoided)
            {
                // Only if they're the same size..
                if (this.Size == otherFile.Size)
                {
                    // Get my own MD5 has.
                    byte[] thisHash = this.MD5Hash;
                    // Only continue if I read that ok.
                    if (thisHash != null)
                    {
                        // Get the other file's MD5 hash.
                        byte[] otherHash = otherFile.MD5Hash;
                        // Only continue if I read that ok.
                        if (otherHash != null)
                        {
                            // Loop through the array of bytes within the hash, and return true only if all turn out to be the same.
                            int thisLength = thisHash.Length;
                            if (thisLength == otherHash.Length)
                            {
                                for (int i = 0; i < thisLength; i++)
                                {
                                    if (thisHash[i] != otherHash[i])
                                    {
                                        return false;
                                    }
                                }
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        #region IComparable implementation
        /// <summary>
        /// Compare based upon the size, in reverse order (since we want to show the biggest files at top of the ListView).
        /// </summary>
        /// <param name="otherFileInstance">The FileInstance to compare against</param>
        /// <returns>-1 if larger than otherFileInstance, 1 if smaller, 0 if same size</returns>
        public int CompareTo(FileData otherFileInstance)
        {
            if (this.Size > otherFileInstance.Size)
            {
                return -1;
            }
            else if (this.Size < otherFileInstance.Size)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region Internal Implementation

        /// <summary>
        /// Initializes a new instance of the <see cref="FileData"/> class.
        /// </summary>
        /// <param name="dir">The directory that the file is stored at</param>
        /// <param name="findData">WIN32_FIND_DATA structure that this
        /// object wraps.</param>
        internal FileData(string dir, WIN32_FIND_DATA findData, bool isUnableToAccess = false, string note = null)
        {
            this.Attributes = findData.dwFileAttributes;

            this.CreationTimeUtc = ConvertDateTime(findData.ftCreationTime_dwHighDateTime,
                                                findData.ftCreationTime_dwLowDateTime);

            this.LastAccessTimeUtc = ConvertDateTime(findData.ftLastAccessTime_dwHighDateTime,
                                                findData.ftLastAccessTime_dwLowDateTime);

            this.LastWriteTimeUtc = ConvertDateTime(findData.ftLastWriteTime_dwHighDateTime,
                                                findData.ftLastWriteTime_dwLowDateTime);

            this._size = CombineHighLowInts(findData.nFileSizeHigh, findData.nFileSizeLow);

            this._sName = findData.cFileName;
            this._path = System.IO.Path.Combine(dir, findData.cFileName);
            this._isUnableToAccess = isUnableToAccess;
            this._note = note;
        }

        private static long CombineHighLowInts(uint high, uint low)
        {
            return (((long)high) << 0x20) | low;
        }

        private static DateTime ConvertDateTime(uint high, uint low)
        {
            long fileTime = CombineHighLowInts(high, low);
            return DateTime.FromFileTimeUtc(fileTime);
        }
        #endregion Internal Implementation
    }
    #endregion class FileData
}
