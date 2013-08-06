using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace DLib
{
    public class FilesystemSearchOptions
    {
        public FilesystemSearchOptions()
        {
            this.IsIncludingHiddenFiles = false;
            this.IsRecursiveSearch = false;
            this.IsFilesOnly = true;
            this.FileSpecifier = new FileSpecifier("*");
        }

        public FilesystemSearchOptions(FilesystemSearchOptions source)
        {
            this.IsIncludingHiddenFiles = source.IsIncludingHiddenFiles;
            this.IsRecursiveSearch = source.IsRecursiveSearch;
            this.IsFilesOnly = source.IsFilesOnly;
            this.FileSpecifier = source.FileSpecifier;
            this.PathExclusions = source.PathExclusions;
        }

        /// <summary>
        /// Get/set the FileSpecifier object that contains the file pathname, or wildcard representation of what to include as matching filenames (such as *.txt, for example).
        /// </summary>
        public FileSpecifier FileSpecifier
        {
            get { return _fileSpecifier; }
            set { _fileSpecifier = value; }
        }

        /// <summary>
        /// If true - recurse into lower-level subdirectories; if false - only return file-objects within the one directory.
        /// This defaults to true.
        /// </summary>
        public bool IsRecursiveSearch { get; set; }

        /// <summary>
        /// If true, this means do not return file-objects that correspond to directories.
        /// This defaults to true.
        /// </summary>
        public bool IsFilesOnly { get; set; }

        /// <summary>
        /// Get/set whether to include hidden and system files within our search.
        /// </summary>
        public bool IsIncludingHiddenFiles { get; set; }

        /// <summary>
        /// Get/set the collection of filesystem paths to exclude from searches.
        /// </summary>
        public string[] PathExclusions
        {
            get
            {
                 return _pathExclusions;
            }
            set { _pathExclusions = value; }
        }

        /// <summary>
        /// Return true if the given file pathname passes our criteria of not being a hidden/system/temporary file,
        /// if this WorkParameters object is set to exclude those.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool PassesFile(FileData fileData)
        {
            //TODO: Does it match our filespec?
            bool r = true;
            try
            {
                // If we are excluding hidden files, then check for that..
                if (!IsIncludingHiddenFiles)
                {
                    if (fileData.IsHiddenOrSystemOrTemporary)
                    {
                        return false;
                    }
                }

            }
            catch (Exception x)
            {
                Console.WriteLine("Exception is FilesystemSearchOptions.PassesFile(" + fileData.Path + "): " + x.Message);
                r = false;
            }
            return r;
        }

        #region ToString
        /// <summary>
        /// Override the ToString method to provide a more complete indication of the state of this object.
        /// </summary>
        /// <returns>A string representation of this thing</returns>
        public override string ToString()
        {
            var sb = new StringBuilder("FilesystemSearchOptions(FileSpec=");
            sb.Append(this.FileSpecifier.FileSpec);
            if (this.IsIncludingHiddenFiles)
            {
                sb.Append(",IsIncludingHiddenFiles");
            }
            if (IsRecursiveSearch)
            {
                sb.Append(",IsRecursiveSearch");
                if (_pathExclusions != null)
                {
                    if (_pathExclusions.Length > 0)
                    {
                        sb.Append(",PathExclusions=");
                        sb.Append(_pathExclusions.Length.ToString());
                    }
                }
            }
            sb.Append(")");
            return sb.ToString();
        }
        #endregion

        #region fields

        private FileSpecifier _fileSpecifier;
        private string[] _pathExclusions;

        #endregion fields
    }
}
