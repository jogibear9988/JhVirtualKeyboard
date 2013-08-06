using System;
using System.Collections.Generic;
using System.Management;
using Microsoft.Win32;


namespace JhLib
{
	public class RegistryKeyChange : RegistryChangeBase
	{
		#region Constants

		private const string queryString = "SELECT * FROM RegistryKeyChangeEvent WHERE Hive = '{0}' AND ({1})";

		#endregion Constants

		#region Static Fields

		private static string HiveLocation = "KeyPath";

		#endregion Static Fields

		#region Constructors

        public RegistryKeyChange(string Hive, string KeyPath)
            : this(Hive, new List<string>(new string[] { KeyPath }))
        { }

		public RegistryKeyChange(RegistryKey Hive, string KeyPath)
			: this(Hive.Name, KeyPath)
		{
		}

		public RegistryKeyChange(string Hive, List<string> KeyPathCollection)
			: base(Hive, KeyPathCollection)
		{
			this.Query.QueryString = BuildQueryString(Hive, KeyPathCollection);

			this.EventArrived += new EventArrivedEventHandler(RegistryKeyChange_EventArrived);
		}
        
		public RegistryKeyChange(RegistryKey Hive, List<string> KeyPathCollection)
			: this(Hive.Name, KeyPathCollection)
		{
		}

		#endregion Constructors

		#region Private Methods

		private string BuildQueryString(string Hive, List<string> KeyPathCollection)
		{
			string ORString = RegistryChangeBase.BuildOrString(KeyPathCollection);
			string FormattedOR = string.Format(ORString, HiveLocation);
			return string.Format(queryString, Hive, FormattedOR);
		}

        private void RegistryKeyChange_EventArrived(object sender, EventArrivedEventArgs e)
		{
			RegistryKeyChangeEvent RegValueChange = new RegistryKeyChangeEvent(e.NewEvent);

            OnRegistryKeyChanged(RegValueChange);
		}

        protected virtual void OnRegistryKeyChanged(RegistryKeyChangeEvent RegValueChange)
        {
            if (RegistryKeyChanged != null)
            {
                RegistryKeyChanged(this, new RegistryKeyChangedEventArgs(RegValueChange));
            }
        }

		#endregion Private Methods

		#region Events

		public event EventHandler<RegistryKeyChangedEventArgs> RegistryKeyChanged;

		#endregion Events
	}

    public class RegistryKeyChangedEventArgs : EventArgs
    {
        private RegistryKeyChangeEvent data;

        public RegistryKeyChangeEvent RegistryKeyChangeData
        {
            get
            {
                return data;
            }
        }

        public RegistryKeyChangedEventArgs(RegistryKeyChangeEvent Data)
        {
            data = Data;
        }
    }
}