using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;


namespace DLib
{
    #region class WIN32_FIND_DATA
    /// <summary>
    /// Contains information about the file that is found 
    /// by the FindFirstFile or FindNextFile functions.
    /// </summary>
    [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto), BestFitMapping(false)]
    internal class WIN32_FIND_DATA
    {
        public FileAttributes dwFileAttributes;
        public uint ftCreationTime_dwLowDateTime;
        public uint ftCreationTime_dwHighDateTime;
        public uint ftLastAccessTime_dwLowDateTime;
        public uint ftLastAccessTime_dwHighDateTime;
        public uint ftLastWriteTime_dwLowDateTime;
        public uint ftLastWriteTime_dwHighDateTime;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
        public int dwReserved0;
        public int dwReserved1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string cFileName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public string cAlternateFileName;

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return "File name=" + cFileName;
        }
    }
    #endregion

    #region class FastDirectoryEnumerator
    /// <summary>
    /// A fast enumerator of files in a directory.  Use this if you need to get attributes for 
    /// all files in a directory.
    /// </summary>
    /// <remarks>
    /// This enumerator is substantially faster than using <see cref="Directory.GetFiles(string)"/>
    /// and then creating a new FileInfo object for each path.  Use this version when you 
    /// will need to look at the attibutes of each file returned (for example, you need
    /// to check each file in a directory to see if it was modified after a specific date).
    /// </remarks>
    public static class FastDirectoryEnumerator
    {
        #region method EnumerateFiles
        /// <summary>
        /// Gets <see cref="FileData"/> for all the files in a directory.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <returns>An object that implements <see cref="IEnumerable{FileData}"/> and 
        /// allows you to enumerate the files in the given directory.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is a null reference (Nothing in VB)
        /// </exception>
        public static IEnumerable<FileData> EnumerateFiles(string path)
        {
            var filesystemSearchOptions = new FilesystemSearchOptions();
            filesystemSearchOptions.FileSpecifier = new FileSpecifier("*");
            return FastDirectoryEnumerator.EnumerateFiles(path, filesystemSearchOptions);
        }

        /// <summary>
        /// Gets <see cref="FileData"/> for all the files in a directory that match a 
        /// specific filter.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="searchPattern">The search string to match against files in the path.</param>
        /// <returns>An object that implements <see cref="IEnumerable{FileData}"/> and 
        /// allows you to enumerate the files in the given directory.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is a null reference (Nothing in VB)
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="filter"/> is a null reference (Nothing in VB)
        /// </exception>
        //public static IEnumerable<FileData> EnumerateFiles(string path, string searchPattern)
        //{
        //    return FastDirectoryEnumerator.EnumerateFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
        //}

        /// <summary>
        /// Gets FileData for all the files in a directory that qualify under the given FilesystemSearchOptions.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="filesystemSearchOptions">
        /// An object that specifies search criteria such as a filespec, andwhether the search 
        /// operation should include all subdirectories or only the current directory.
        /// </param>
        /// <returns>An object that implements <see cref="IEnumerable{FileData}"/> and 
        /// allows you to enumerate the files in the given directory.</returns>
        public static IEnumerable<FileData> EnumerateFiles(string path, FilesystemSearchOptions filesystemSearchOptions)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (filesystemSearchOptions.FileSpecifier == null)
            {
                throw new ArgumentNullException("FileSpecifier");
            }
            string fullPath = Path.GetFullPath(path);
            return new FileEnumerable(fullPath, filesystemSearchOptions);
        }
        #endregion

        #region method GetFiles
        /// <summary>
        /// Gets <see cref="FileData"/> for all the files in a directory that match a 
        /// specific filter.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="searchPattern">The search string to match against files in the path.</param>
        /// <returns>An object that implements <see cref="IEnumerable{FileData}"/> and 
        /// allows you to enumerate the files in the given directory.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="path"/> is a null reference (Nothing in VB)
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="filter"/> is a null reference (Nothing in VB)
        /// </exception>
        //public static FileData[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        //{
        //    var fileSystemSearchOptions = new FilesystemSearchOptions();
        //    fileSystemSearchOptions.FileSpecifier = new FileSpecifier(searchPattern);
        //    fileSystemSearchOptions.IsRecursiveSearch = (searchOption == SearchOption.TopDirectoryOnly ? false : true);
        //    IEnumerable<FileData> e = FastDirectoryEnumerator.EnumerateFiles(path, fileSystemSearchOptions);
        //    List<FileData> list = new List<FileData>(e);

        //    FileData[] retval = new FileData[list.Count];
        //    list.CopyTo(retval);

        //    return retval;
        //}
        #endregion

        #region class FileEnumerable
        /// <summary>
        /// Provides the implementation of the 
        /// <see cref="T:System.Collections.Generic.IEnumerable`1"/> interface
        /// </summary>
        private class FileEnumerable : IEnumerable<FileData>
        {
            #region Constructor
            /// <summary>
            /// Initializes a new instance of the <see cref="FileEnumerable"/> class.
            /// </summary>
            /// <param name="path">The path to search.</param>
            /// <param name="filesystemSearchOptions">An object that specifies search criteria such as the pattern to match files against</param>
            public FileEnumerable(string path, FilesystemSearchOptions filesystemSearchOptions)
            {
                _path = path;
                _filesystemSearchOptions = filesystemSearchOptions;
            }
            #endregion

            #region IEnumerable<FileData> Members  (which is GetEnumerator)

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can 
            /// be used to iterate through the collection.
            /// </returns>
            public IEnumerator<FileData> GetEnumerator()
            {
                return new FileEnumerator(_path, _filesystemSearchOptions);
            }

            #endregion

            #region IEnumerable Members  ( which is GetEnumerator )
            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator"/> object that can be 
            /// used to iterate through the collection.
            /// </returns>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return new FileEnumerator(_path, _filesystemSearchOptions);
            }
            #endregion

            #region fields

            private readonly string _path;
            private readonly FilesystemSearchOptions _filesystemSearchOptions;

            #endregion fields
        }
        #endregion

        #region class SafeFindHandle
        /// <summary>
        /// Wraps a FindFirstFile handle.
        /// </summary>
        private sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            [DllImport("kernel32.dll")]
            private static extern bool FindClose(IntPtr handle);

            /// <summary>
            /// Initializes a new instance of the <see cref="SafeFindHandle"/> class.
            /// </summary>
            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            internal SafeFindHandle()
                : base(true)
            {
            }

            /// <summary>
            /// When overridden in a derived class, executes the code required to free the handle.
            /// </summary>
            /// <returns>
            /// true if the handle is released successfully; otherwise, in the 
            /// event of a catastrophic failure, false. In this case, it 
            /// generates a releaseHandleFailed MDA Managed Debugging Assistant.
            /// </returns>
            protected override bool ReleaseHandle()
            {
                return FindClose(base.handle);
            }
        }
        #endregion

        #region class FileEnumerator
        /// <summary>
        /// Provides the implementation of the 
        /// <see cref="T:System.Collections.Generic.IEnumerator`1"/> interface
        /// </summary>
        [System.Security.SuppressUnmanagedCodeSecurity]
        private class FileEnumerator : IEnumerator<FileData>
        {
            #region FileEnumerator Constructor
            /// <summary>
            /// Initializes a new instance of the <see cref="FileEnumerator"/> class.
            /// </summary>
            /// <param name="path">The path to search.</param>
            /// <param name="filter">The search string to match against files in the path.</param>
            /// <param name="searchOption">
            /// One of the SearchOption values that specifies whether the search 
            /// operation should include all subdirectories or only the current directory.
            /// </param>
            public FileEnumerator(string path, FilesystemSearchOptions filesystemSearchOptions)
            {
                _path = path;
                _searchOptions = filesystemSearchOptions;
                _isSelectingWithFileSpec = !_searchOptions.FileSpecifier.IsNonSpecific;
                if (_searchOptions.IsRecursiveSearch)
                {
                    _stackOfHandles = new Stack<SearchContext>();
                }
            }
            #endregion

            #region IEnumerator<FileData> Members

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <value></value>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            /// </returns>
            public FileData Current
            {
                get { return new FileData(_path, _win_find_data, _isUnableToAccess); }
            }

            #endregion

            #region IDisposable Members

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, 
            /// or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if (_hndFindFile != null)
                {
                    if (!_hndFindFile.IsClosed)
                    {
                        _hndFindFile.Dispose();
                    }
                    _hndFindFile = null;
                }
            }

            #endregion

            #region IEnumerator Members

            #region Current property
            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <value></value>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            /// </returns>
            object System.Collections.IEnumerator.Current
            {
                get { return new FileData(_path, _win_find_data, _isUnableToAccess); }
            }
            #endregion

            #region MoveNext method
            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element; 
            /// false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">
            /// The collection was modified after the enumerator was created.
            /// </exception>
            public bool MoveNext()
            {
                bool isNotEmpty = false;

                while (true)
                {
                    _isUnableToAccess = false;
                    // If the handle is null, this is first call to MoveNext in the current 
                    // directory.  In that case, start a new search, starting with the files of the directory..
                    if (_hndFindFile == null)
                    {
                        new FileIOPermission(FileIOPermissionAccess.PathDiscovery, _path).Demand();
                        string searchPath;
                        if (_searchOptions.IsRecursiveSearch)
                        {
                            searchPath = Path.Combine(_path, "*");
                        }
                        else
                        {
                            // If the FileSpec is a multiple (eg, "*.cs, *.xaml" then we cannot use the FindFirestFile api call
                            // to filter the search for us. So, we'll just canvas for all files, and then filter the returns.
                            if (_searchOptions.FileSpecifier.HasMultipleSpecs)
                            {
                                searchPath = Path.Combine(_path, "*.*");
                            }
                            else
                            {
                                searchPath = Path.Combine(_path, _searchOptions.FileSpecifier.FileSpec);
                            }
                        }
                        _hndFindFile = FindFirstFile(searchPath, _win_find_data);
                        isNotEmpty = !_hndFindFile.IsInvalid;
                        // If that call to FindFirstFile yielded absolutely nothing, then let's check it for security violations so that we can alert the user..
                        if (_hndFindFile.IsInvalid)
                        {
                            _isUnableToAccess = true;
                            return true;
                            //try
                            //{
                            //    string currentUser = Environment.UserName;
                            //    NTAccount account = new NTAccount(currentUser);
                            //    SecurityIdentifier securityIdentifier = account.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier;
                            //    Console.WriteLine("for path: " + _path);
                            //    DirectoryInfo directoryInfo = new DirectoryInfo((_path));
                            //    DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();
                            //    AuthorizationRuleCollection rules = directorySecurity.GetAccessRules(true, true, typeof (SecurityIdentifier));
                            //    foreach (FileSystemAccessRule accessRule in rules)
                            //    {
                            //        //accessRule.
                            //        if (securityIdentifier.CompareTo(accessRule.IdentityReference as SecurityIdentifier) == 0)
                            //        {
                            //            Console.WriteLine("  accessRule.FileSystemRights = " + accessRule.FileSystemRights + ", accessFule=" + accessRule.ToString());
                            //        }
                            //    }
                            //}
                            //catch (Exception x)
                            //{
                            //    Console.WriteLine("  Denied access to " + _path + ", " + x.Message);
                            //}
                        }
                    }
                    else // _hndFindFile is not null, so this is not the first call to MoveNext.
                    {
                        // find the next item.
                        isNotEmpty = FindNextFile(_hndFindFile, _win_find_data);
                    }

                    // If the call to FindFirstFile or FindNextFile found something..
                    if (isNotEmpty)
                    {
                        string sFilename = _win_find_data.cFileName;

                        // If we are now on a directory..
                        if (IsWhatIFoundADirectory())
                        {
                            // If we are searching directories, push the current directory onto the
                            // context stack, then restart the search in the sub-directory.
                            // Otherwise, skip the directory and move on to the next file in the current directory.
                            if (_searchOptions.IsRecursiveSearch)
                            {
                                if (IsADirectoryIWantToSkip())
                                {
                                    continue;
                                }
                                else // not a directory we want to skip
                                {
                                    // Found a directory, this is a recursive search, and it's one that we don't want to skip,
                                    // so save off the current context (the parameters of the search for this current directory)
                                    // and start searching anew within the discovered directory..
                                    SearchContext context = new SearchContext(_hndFindFile, _path);
                                    _stackOfHandles.Push((context));
                                    _path = Path.Combine(_path, sFilename);
                                    _hndFindFile = null;
                                    // Return true to indicate we did find something, and the caller can get it via the _path, _win_find_data, and _isUnableToAccess variables.
                                    return true;
                                }
                            }
                            else // not a recursive search, so skip directories and just move on to the next file/folder object.
                            {
                                continue;
                            }
                            //continue;
                        }
                        else // it's a file
                        {
                            // Filter out whatever files we want to skip.
                            if (IsAFileIWantToSkip())
                            {
                                // Loop around to the next file in the current directory.
                                continue;
                            }
                            else // We have a file we want to yield to the caller, so exit the retry-loop.
                            {
                                break;
                            }
                        }
                    }
                    else // Emtpy -- we have exhausted all of the file/folder objects in the current path.
                    {
                        if (_searchOptions.IsRecursiveSearch)
                        {
                            // If there are no more files in this directory and we are 
                            // in a sub directory, pop back up to the parent directory and
                            // continue the search from there.
                            if (_stackOfHandles.Count > 0)
                            {
                                SearchContext context = _stackOfHandles.Pop();
                                _path = context.Path;
                                _hndFindFile = context.Handle;
                                continue;
                            }
                        }
                        break;
                    }
                }
                return isNotEmpty;
            }
            #endregion MoveNext method

            #region Reset method
            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">
            /// The collection was modified after the enumerator was created.
            /// </exception>
            public void Reset()
            {
                _hndFindFile = null;
            }
            #endregion

            #endregion

            #region Internal Implementation

            #region Win32 interface

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern SafeFindHandle FindFirstFile(string fileName,
                [In, Out] WIN32_FIND_DATA data);

            // http://msdn.microsoft.com/en-us/library/windows/desktop/aa364418%28v=vs.85%29.aspx

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            private static extern bool FindNextFile(SafeFindHandle hndFindFile,
                    [In, Out, MarshalAs(UnmanagedType.LPStruct)] WIN32_FIND_DATA lpFindFileData);

            #endregion Win32 interface

            #region private class SearchContext
            /// <summary>
            /// Hold context information about where we current are in the directory search,
            /// which is actually just a handle and path.
            /// </summary>
            private class SearchContext
            {
                public SearchContext(SafeFindHandle handle, string path)
                {
                    this.Handle = handle;
                    this.Path = path;
                }

                public readonly SafeFindHandle Handle;
                public readonly string Path;
            }
            #endregion private class SearchContext

            #region IsWhatIFoundADirectory
            /// <summary>
            /// Return true if the file-or-folder object found (which is reflected in _win_find_data) is a folder.
            /// </summary>
            private bool IsWhatIFoundADirectory()
            {
                FileAttributes fileAttrib = (FileAttributes)_win_find_data.dwFileAttributes;
                return (fileAttrib & FileAttributes.Directory) == FileAttributes.Directory;
            }
            #endregion

            private bool IsAFileIWantToSkip()
            {
                string sFilename = _win_find_data.cFileName;
                if (sFilename == "." || sFilename == "..")
                {
                    return true;
                }
                // Ignore hidden, system, or temporary files.
                if (!_searchOptions.IsIncludingHiddenFiles)
                {
                    FileAttributes fileAttrib = (FileAttributes)_win_find_data.dwFileAttributes;
                    if ((fileAttrib & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        return true;
                    }
                }
                // Match them against the filespec..
                if (_isSelectingWithFileSpec)
                {
                    if (_searchOptions.FileSpecifier.MatchesPath(sFilename))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }

            private bool IsADirectoryIWantToSkip()
            {
                string sFilename = _win_find_data.cFileName;
                if (sFilename == "." || sFilename == "..")
                {
                    return true;
                }
                if (!_searchOptions.IsIncludingHiddenFiles)
                {
                    // Ignore hidden directories.
                    FileAttributes fileAttrib = (FileAttributes)_win_find_data.dwFileAttributes;
                    if ((fileAttrib & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        return true;
                    }
                }
                // Check for PathExclusions..
                if (_searchOptions.PathExclusions != null)
                {
                    int n = _searchOptions.PathExclusions.Length;
                    for (int i = 0; i < n; i++)
                    {
                        string path = _searchOptions.PathExclusions[i];
                        if (path.IsTheSameFilesystemPath(sFilename))
                        {
                            //Console.WriteLine("Skipping path " + sFilename + " because it is within PathExclusions.");
                            return true;
                        }
                    }
                }
                return false;
            }

            #region fields

            private string _path;
            private FilesystemSearchOptions _searchOptions;
            private Stack<SearchContext> _stackOfHandles;
            /// <summary>
            /// This handle is null until the first call to FindFirstFile is made, or after a Reset.
            /// </summary>
            private SafeFindHandle _hndFindFile;
            private WIN32_FIND_DATA _win_find_data = new WIN32_FIND_DATA();
            /// <summary>
            /// This flag indicates whether at the last call to FindFirstFile or FindNextFile we were unable to access the file/folder.
            /// </summary>
            private bool _isUnableToAccess;
            /// <summary>
            /// This is true if the FileSpec is something other than "*" or "*.*".
            /// </summary>
            private bool _isSelectingWithFileSpec;

            #endregion fields

            #endregion Internal Implementation
        }
        #endregion class FileEnumerator
    }
    #endregion class FastDirectoryEnumerator
}
