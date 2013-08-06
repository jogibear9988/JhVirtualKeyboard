// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
#if !SILVERLIGHT
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
#endif


namespace JhLib
{
    /// <summary>
    /// A (minimal) base class for a something that implements INotifyPropertyChanged.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Subclasses can override this method to do stuff after a property value is set. The base implementation does nothing.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        protected virtual void AfterPropertyChanged(string propertyName)
        {
        }

        /// <summary>
        /// Get an instance of a PropertyChangedEventArgs object, by taking it from our private cache if it is there - otherwise creating it.
        /// </summary>
        /// <param name="propertyName">The name of the property that the PropertyChangedEventArgs object is constructed from</param>
        /// <returns>An instance of PropertyChangedEventArgs constructed from the given propertyName</returns>
        public static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName)
        {
            if (String.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArithmeticException("propertyName cannot be null or empty.");
            }

            PropertyChangedEventArgs args;

            // Get the event args from the cache, creating and adding them to the cache only if necessary.
            lock (_lockObject)
            {
                bool isCached = _eventArgCache.ContainsKey(propertyName);
                if (!isCached)
                {
                    _eventArgCache.Add(propertyName, new PropertyChangedEventArgs(propertyName));
                }
                args = _eventArgCache[propertyName];
            }
            return args;
        }

        private static readonly Dictionary<string, PropertyChangedEventArgs> _eventArgCache = new Dictionary<string, PropertyChangedEventArgs>();
        private static readonly object _lockObject = new object();

        /// <summary>
        /// Check that the given property exists on this type.
        /// </summary>
        /// <param name="propertyName">The name of the property to check</param>
        [Conditional("DEBUG")]
        private void VerifyProperty(string propertyName)
        {
            Type type = this.GetType();
            // Look for a public property with the specified name.
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            // The property couldn't be found, so get upset.
            Debug.Assert(propertyInfo != null, String.Format("Unable to verify that type {0} has property {1} !", type.FullName, propertyName));
        }

        #region INotifyPropertyChanged

        // This is after reading a few articles online, such as Jeremy Likness' article at http://www.codeproject.com/KB/silverlight/mvvm-explained.aspx

        /// <summary>
        /// Raise the PropertyChanged event, with the given propertyName as the argument.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed in value. The spelling must be correct.</param>
        public void Notify(string propertyName)
        {
            this.VerifyProperty(propertyName);

            // Make a copy of the PropertyChanged event first, before testing it for null,
            // because another thread might change it between the two statements.
            var copyOfPropertyChangedEvent = PropertyChanged;

            if (copyOfPropertyChangedEvent != null)
            { 
                // Get the cached event-arg.
                var args = BaseViewModel.GetPropertyChangedEventArgs(propertyName);
                copyOfPropertyChangedEvent(this, args);
            }

            this.AfterPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged

        #region IsInDesignMode
        /// <summary>
        /// Get whether the control is in design mode (running in Blend or Cider).
        /// </summary>
        public static bool IsInDesignModeStatic
        {
            get
            {
                // Thanks to Laurent Bugnion for the this.
                // http://geekswithblogs.net/lbugnion/archive/2009/09/05/detecting-design-time-mode-in-wpf-and-silverlight.aspx
                // except, it would've been rather nice if it had actually been correct!  :>(
                if (!_isInDesignMode.HasValue)
                {
#if SILVERLIGHT
                    _isInDesignMode = System.ComponentModel.DesignerProperties.IsInDesignTool;
#else
#if WIN8
                    _isInDesignMode = Windows.ApplicationModel.DesignMode.DesignModeEnabled;
#else
                    _isInDesignMode = DesignerProperties.IsInDesignMode;
                    //TODO: Why does this not compile?
                    //var prop = DesignerProperties.IsInDesignModeProperty;
                    //_isInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
#endif
#endif
                }
                return _isInDesignMode.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is in design mode (running under Blend or Cider).
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Non static member needed for data binding")]
        public bool IsInDesignMode
        {
            get { return IsInDesignModeStatic; }
        }

        private static bool? _isInDesignMode;

        #endregion IsInDesignMode
    }
}
