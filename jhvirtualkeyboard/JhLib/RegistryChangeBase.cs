using System;
using System.Collections.Generic;
using System.Management;
using System.Text;


namespace JhLib
{
	public abstract class RegistryChangeBase : ManagementEventWatcher
	{
		#region constructors

		static RegistryChangeBase()
		{
            //TODO: Why did this not include HKEY_CURRENT_USER originally?  jh
            // And why, when I do include HKEY_CURRENT_USER, and employ that setting, does it throw an Invalid Query error?
			string[] validHives = new string[] { "HKEY_LOCAL_MACHINE", "HKEY_USERS", "HKEY_CURRENT_CONFIG", "HKEY_CURRENT_USER" };
			_validHiveList.AddRange(validHives);
		}

		protected RegistryChangeBase(string hive, List<string> keyPathCollection)
		{
			ValidateHive(hive);

            ValidateKeyPathList(keyPathCollection);

			this.Query.QueryLanguage = "WQL";

            this.Scope.Path.NamespacePath = @"root\default";
		}

        protected RegistryChangeBase(string hive, string keyPath)
            : this(hive, new List<string>(new string[] { keyPath }))
        { }

		#endregion constructors

        #region Protected Methods

        protected static string BuildOrString(List<string> values)
        {
            var sb = new StringBuilder();
            foreach (string item in values)
            {
                sb.Append(string.Format(_format, item));
            }
            sb.Remove(sb.Length - 4, 4);
            return sb.ToString();
        }

        #endregion Protected Methods

        #region Private Methods

		private void ValidateHive(string Hive)
		{
			if (!_validHiveList.Contains(Hive.ToUpper()))
			{
			    if (string.IsNullOrEmpty(Hive))
			        throw new ArgumentNullException("Hive", "Hive cannot be null");
			    else
			        throw new ArgumentException("Incorrect value", "Hive");
			}
		}

		private void ValidateKeyPathList(List<string> keyPathCollection)
		{
            if (keyPathCollection == null)
            {
                throw new ArgumentNullException("keyPathCollection");
            }
            if (keyPathCollection.Count == 0)
            {
                throw new ArgumentException("keyPathCollection must have something in it.", "keyPathCollection");
            }

		    int i = 0;
            foreach (string item in keyPathCollection)
            {
                if (string.IsNullOrEmpty(item))
                {
                    throw new ArgumentException(i.ToCardinal() + " element cannot be empty.", "keyPathCollection");
                }
                i++;
            }
		}

		#endregion Private Methods

        #region fields

        private static List<string> _validHiveList = new List<string>();
        private static string _format = "{{0}} = '{0}' OR ";

        #endregion fields
    }
}