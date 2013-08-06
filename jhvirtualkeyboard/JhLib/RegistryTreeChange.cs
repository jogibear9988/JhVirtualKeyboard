using System;
using System.Collections.Generic;
using System.Management;
using Microsoft.Win32;


namespace JhLib
{
	public class RegistryTreeChange : RegistryChangeBase
	{
        #region constructors

        public RegistryTreeChange(string hive, string rootPath)
            : this(hive, new List<string>(new string[] { rootPath }))
        { }

        public RegistryTreeChange(RegistryKey hive, string rootPath)
            : this(hive.Name, rootPath)
        {
        }

        public RegistryTreeChange(string hive, List<string> rootPathCollection)
            : base(hive, rootPathCollection)
        {
            this.Query.QueryString = BuildQueryString(hive, rootPathCollection);

            this.EventArrived += new EventArrivedEventHandler(RegistryTreeChange_EventArrived);
        }

        public RegistryTreeChange(RegistryKey hive, List<string> rootPathCollection)
            : this(hive.Name, rootPathCollection)
        {
        }

        #endregion constructors

		#region Private Methods

		private string BuildQueryString(string Hive, List<string> keyPathCollection)
		{
			string oRString = RegistryChangeBase.BuildOrString(keyPathCollection);
			string formattedOR = string.Format(oRString, _hiveLocation);
			return string.Format(_queryString, Hive, formattedOR);
		}

        private void RegistryTreeChange_EventArrived(object sender, EventArrivedEventArgs e)
		{
            var registryTreeChangeEvent = new RegistryTreeChangeEvent(e.NewEvent);
            OnRegistryTreeChanged(registryTreeChangeEvent);
		}

        protected virtual void OnRegistryTreeChanged(RegistryTreeChangeEvent registryTreeChangeEvent)
        {
            if (RegistryTreeChanged != null)
            {
                RegistryTreeChanged(this, new RegistryTreeChangedEventArgs(registryTreeChangeEvent));
            }
        }

		#endregion Private Methods

		#region Events

		public event EventHandler<RegistryTreeChangedEventArgs> RegistryTreeChanged;

		#endregion Events

        #region fields

        private const string _queryString = "SELECT * FROM RegistryTreeChangeEvent WHERE Hive = '{0}' AND ({1})";
        private static string _hiveLocation = "RootPath";

        #endregion fields
    }

    public class RegistryTreeChangedEventArgs : EventArgs
    {
        public RegistryTreeChangeEvent RegistryTreeChangeData
        {
            get { return _data; }
        }

        public RegistryTreeChangedEventArgs(RegistryTreeChangeEvent Data)
        {
            _data = Data;
        }

        private RegistryTreeChangeEvent _data;
    }
}