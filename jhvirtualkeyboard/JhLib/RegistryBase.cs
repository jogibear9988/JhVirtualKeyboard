// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Text;
using System.Windows;
using Microsoft.Win32;


namespace JhLib
{
    /// <summary>
    /// This provides a somewhat simplified access to the Windows Registry.
    /// In general you will sub-class this and provide your own override of the BaseKeyPath property
    /// in your application, plus type-safe accessors to retrieve the values from the Registry.
    /// </summary>
    public class RegistryBase
    {
        // This factory property is no longer needed, if you provide it within your sub-class to return your specific type.
        //public static RegistryBase The
        //{
        //    get
        //    {
        //        if (_theRegistry == null)
        //        {
        //            _theRegistry = new RegistryBase();
        //        }
        //        return _theRegistry;
        //    }
        //}

        #region BaseKeyPath
        /// <summary>
        /// Base registry key that your subclass should override to provide the full path of keys/subkeys to access the key-values of your application,
        /// unless you want to accept the default that this provides.
        /// For example, @"Software\DesignForge\MyApplication".
        /// </summary>
        public virtual string BaseKeyPath
        {
            get
            {
                if (m_baseKeyPath == null)
                {
                    IApp thisApplication = Application.Current as IApp;
                    if (thisApplication != null)
                    {
                        // Since this is an IApp, we can derive the value.
                        string productName = thisApplication.ProductNameShort;
                        string companyName = thisApplication.VendorName;
                        m_baseKeyPath = @"Software\" + companyName + @"\" + productName;
                    }
                    else
                    {
                        throw new ApplicationException("You must override the RegistryBase.BaseKeyPath method, or else your App must subclass IApp!");
                    }
                }
                return m_baseKeyPath;
            }
        }
        #endregion

        protected string m_baseKeyPath;

        #region DeleteKey method
        /// <summary>
        /// Remove the given sub-key (under BaseKeyPath), if it exists, from the Windows Registry .
        /// </summary>
        /// <param name="subkeyPath">the Registry subkey (under BaseKeyPath) to remove</param>
        /// <remarks>
        /// This operates recursively, deleting the entire subkey-tree under the given subkey if there is any.
        /// </remarks>
        protected void DeleteKey(string subkeyPath)
        {
            if (String.IsNullOrWhiteSpace(subkeyPath))
            {
                throw new ArgumentException("RegistryBase.DeleteKey argument subkeyPath must not be empty.");
            }
            if (IsKeyPresentUnderBasePath(subkeyPath))
            {
                string fullSubkeyPath = BaseKeyPath + @"\" + subkeyPath;
                if (IsSpecificToUser)
                {
                    Registry.CurrentUser.DeleteSubKeyTree(fullSubkeyPath);
                }
                else
                {
                    Registry.LocalMachine.DeleteSubKeyTree(fullSubkeyPath);
                }
            }
        }
        #endregion

        #region DeleteValue method
        /// <summary>
        /// Remove the indicated Value from the Windows Registry from under BaseKeyPath,
        /// and under subkeyPath if that is not null.
        /// </summary>
        /// <param name="valueName">the value-name to delete</param>
        /// <param name="subkeyPath">the sub-key (under BaseKeyPath) to delete the value from. If null - then it's directly under BaseKeyPath.</param>
        protected void DeleteValue(string valueName, string subkeyPath = null)
        {
            if (valueName == null)
            {
                throw new ArgumentNullException("DeleteValue argument valueName must not be null!");
            }
            using (var registryKey = OpenSubkeyUnderBasekey(true, subkeyPath))
            {
                if (registryKey != null)
                {
                    registryKey.DeleteValue(valueName, false);
                }
            }
        }
        #endregion

        #region EnsureExists methods
        /// <summary>
        /// Check that the given key path exists under the BaseKeyPath
        /// and create it if it does not already exist.
        /// </summary>
        /// <param name="subkeyUnderBaseKeyPath">The sub-key underneath the BaseKeyPath to ensure the existence of</param>
        public void EnsureExists(string subkeyUnderBasekeyPath)
        {
            if (String.IsNullOrWhiteSpace(subkeyUnderBasekeyPath))
            {
                throw new ArgumentException("EnsureExists argument subkeyUnderBasekeyPath must not be empty.");
            }
            RegistryKey registryKey = null;
            try
            {
                registryKey = CreateSubkeyUnderBasekey(subkeyUnderBasekeyPath);
                if (registryKey == null)
                {
                    Console.WriteLine("in RegistryBase.EnsureExists(" + subkeyUnderBasekeyPath + "), CreateSubKey returned null.");
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.GetType() + " in RegistryBase.EnsureExists(" + subkeyUnderBasekeyPath + "): " + x.Message);
                throw;
            }
            finally
            {
                if (registryKey != null)
                {
                    registryKey.Close();
                    registryKey.Dispose();
                    registryKey = null;
                }
            }
        }

        /// <summary>
        /// Check that the Registry key BaseKeyPath exists, and create it if it does not.
        /// </summary>
        public void EnsureBaseKeyPathExists()
        {
            RegistryKey registryKey = null;
            try
            {
                if (this.IsSpecificToUser)
                {
                    registryKey = Registry.CurrentUser.OpenSubKey(BaseKeyPath);
                }
                else
                {
                    registryKey = Registry.LocalMachine.OpenSubKey(BaseKeyPath);
                }
                if (registryKey == null)
                {
                    Console.WriteLine("in RegistryBase.EnsureBaseKeyPathExists(with BaseKeyPath=" + BaseKeyPath + "), CreateSubKey returned null.");
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.GetType() + " in RegistryBase.EnsureExists(with BaseKeyPath=" + BaseKeyPath + "): " + x.Message);
                throw;
            }
            finally
            {
                if (registryKey != null)
                {
                    registryKey.Close();
                    registryKey.Dispose();
                    registryKey = null;
                }
            }
        }
        #endregion

        #region Get methods

        #region GetString
        /// <summary>
        /// Retrieve the indicated string value from the Windows Registry, from under BaseKeyPath (and under subkeyPath if that is non-null).
        /// If the retrieval fails then the given default value is returned.
        /// </summary>
        /// <param name="valueName">the name of the Registry value to retrieve it from</param>
        /// <param name="stringDefaultValue">the default value to return if the retrieval fails</param>
        /// <param name="subkeyPath">the Registry subkey (under BaseKeyPath) under which to retrieve it from, if non-null. Default is null.</param>
        /// <returns>The value from the Registry, or the default value if there's a failure</returns>
        protected string GetString(string valueName, string stringDefaultValue, string subkeyPath = null)
        {
            string result = stringDefaultValue;
            using (var registryKey = this.OpenSubkeyUnderBasekey(false, subkeyPath))
            {
                if (registryKey != null)
                {
                    result = (string)registryKey.GetValue(valueName, stringDefaultValue);
                }
                else
                {
                    Console.WriteLine("in RegistryBase.GetString(" + valueName + ",," + StringLib.AsNonNullString(subkeyPath) + "), access failed so returning the default value.");
                }
            }
            return result;
        }
        #endregion

        #region GetInteger
        /// <summary>
        /// Retrieve the indicated integer value from the Windows Registry, from under BaseKeyPath (and under subkeyPath if that is non-null).
        /// If the retrieval fails then the provided default value is returned.
        /// </summary>
        /// <param name="valueName">the name of the Registry value to retrieve it from</param>
        /// <param name="integerDefaultValue">the default value to return if the retrieval fails</param>
        /// <param name="subkeyPath">the Registry subkey (under BaseKeyPath) under which to retrieve it from, if non-null. Default is null.</param>
        /// <returns>The value from the Registry, or the default value if there's a failure</returns>
        protected int GetInteger(string valueName, int integerDefaultValue, string subkeyPath = null)
        {
            int result = integerDefaultValue;
            using (var registryKey = this.OpenSubkeyUnderBasekey(false, subkeyPath))
            {
                if (registryKey != null)
                {
                    result = (int)registryKey.GetValue(valueName, integerDefaultValue);
                }
                else
                {
                    Console.WriteLine("in RegistryBase.GetInteger(" + valueName + ",, " + StringLib.AsNonNullString(subkeyPath) + ",), access failed so returning the default value.");
                }
            }
            return result;
        }
        #endregion

        #region GetAndEnsureIsInteger
        /// <summary>
        /// Retrieve the indicated integer value from the Windows Registry, from under BaseKeyPath (and under subkeyPath if that is non-null).
        /// If the value that is in the Registry is a STRING, delete that and replace it with a DWORD value containing the given integerDefaultValue.
        /// If the retrieval fails then the provided default value is returned.
        /// </summary>
        /// <param name="valueName">the name of the Registry value to retrieve it from</param>
        /// <param name="integerDefaultValue">the default value to return if the retrieval fails</param>
        /// <param name="subkeyPath">the Registry subkey (under BaseKeyPath) under which to retrieve it from, if non-null. Default is null.</param>
        /// <returns>The value from the Registry, or the default value if there's a failure</returns>
        protected int GetAndEnsureIsInteger(string valueName, int integerDefaultValue, string subkeyPath = null)
        {
            int result = integerDefaultValue;
            if (String.IsNullOrWhiteSpace(valueName))
            {
                throw new ArgumentException("for GetAndEnsureIsInteger, argument valueName must not be empty.");
            }
            //TODO: Actually, should only need write access if we need to replace it.
            using (var registryKey = this.OpenSubkeyUnderBasekey(true, subkeyPath))
            {
                if (registryKey != null)
                {
                    // First check to see whether a value is there.
                    object o = registryKey.GetValue(valueName, null);
                    if (o != null)
                    {
                        // There's a value. Find out whether it's a DWORD or a STRING.
                        RegistryValueKind kind = registryKey.GetValueKind(valueName);
                        if (kind == RegistryValueKind.DWord)
                        {
                            //TODO: We shouldn't have to do multipile accesses of the Registry. jh
                            result = (int)registryKey.GetValue(valueName, integerDefaultValue);
                        }
                        else if (kind == RegistryValueKind.String)
                        {
                            // It's a STRING, so replace it with a DWORD value containing the default value.
                            try
                            {
                                DeleteValue(valueName, subkeyPath);
                            }
                            catch (UnauthorizedAccessException)
                            {
                                Console.WriteLine("in RegistryBase.GetAndEnsureIsInteger(" + valueName + "," + integerDefaultValue + "," + StringLib.AsNonNullString(subkeyPath) + "), UnauthorizedAccessException trying to delete the string value.");
                            }
                            SetValue(valueName, integerDefaultValue, subkeyPath);
                        }
                        else
                        {
                            Console.WriteLine("in RegistryBase.GetAndEnsureIsInteger(" + valueName + "," + integerDefaultValue + "," + StringLib.AsNonNullString(subkeyPath) + "), the RegistryValueKind is " + kind);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("in RegistryBase.GetAndEnsureIsInteger(" + valueName + "," + integerDefaultValue + "," + StringLib.AsNonNullString(subkeyPath) + "), the key was not found.");
                }
            }
            return result;
        }
        #endregion

        #region GetInt64
        /// <summary>
        /// Retrieve the indicated 64-bit integer value from the Windows Registry, from under BaseKeyPath (and under subkeyPath if that is non-null).
        /// If the retrieval fails then the provided default value is returned.
        /// </summary>
        /// <param name="valueName">the name of the Registry value to retrieve it from</param>
        /// <param name="int64DefaultValue">the default value to return if the retrieval fails</param>
        /// <param name="subkeyPath">the Registry subkey (under BaseKeyPath) under which to retrieve it from, if non-null. Default is null.</param>
        /// <returns>The value from the Registry, or the default value if there's a failure</returns>
        protected Int64 GetInt64(string valueName, Int64 int64DefaultValue, string subkeyPath = null)
        {
            Int64 result = int64DefaultValue;
            using (var registryKey = this.OpenSubkeyUnderBasekey(false, subkeyPath))
            {
                if (registryKey != null)
                {
                    result = (Int64)registryKey.GetValue(valueName, int64DefaultValue);
                }
                else
                {
                    Console.WriteLine("in RegistryBase.GetInt64(" + valueName + ",," + StringLib.AsNonNullString(subkeyPath) + "), access failed so returning the default value.");
                }
            }
            return result;
        }
        #endregion

        #region GetDouble
        /// <summary>
        /// Retrieve the indicated double-precision numeric value from the Windows Registry, from under BaseKeyPath (and under subkeyPath if that is non-null).
        /// If the retrieval fails then the given default value is returned.
        /// </summary>
        /// <param name="valueName">the name of the Registry value to retrieve it from</param>
        /// <param name="doubleDefaultValue">the default value to return if the retrieval fails</param>
        /// <param name="subkeyPath">the Registry subkey (under BaseKeyPath) under which to retrieve it from, if non-null. Default is null.</param>
        /// <returns>The value from the Registry, or the default value if there's a failure</returns>
        protected double GetDouble(string valueName, double doubleDefaultValue, string subkeyPath = null)
        {
            double result = doubleDefaultValue;
            using (var registryKey = this.OpenSubkeyUnderBasekey(false, subkeyPath))
            {
                if (registryKey != null)
                {
                    string stringValue = (string)registryKey.GetValue(valueName, String.Empty);
                    if (!String.IsNullOrEmpty(stringValue))
                    {
                        if (!Double.TryParse(stringValue, out result))
                        {
                            // Looks like an invalid string. Let's whine about this.
                            Console.WriteLine("in RegistryBase.GetDouble, invalid value found: \"" + stringValue + "\"");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("in RegistryBase.GetDouble(" + valueName + ",," + StringLib.AsNonNullString(subkeyPath) + "), access failed so returning the default value.");
                }
            }
            return result;
        }
        #endregion

        #region GetBoolean
        /// <summary>
        /// Retrieve the indicated Boolean value from the Windows Registry, from under BaseKeyPath.
        /// This can be stored in either a DWORD or a STRING value.
        /// If the retrieval fails then the given default value is returned.
        /// </summary>
        /// <param name="valueName">the name of the Registry value to retrieve it from</param>
        /// <param name="booleanDefaultValue">the default value to return if the retrieval fails</param>
        /// <param name="subkeyPath">the sub-key under BaseKeyPath to get the value from, or directly under BaseKeyPath if null</param>
        /// <returns>The value from the Registry, or the default value if there's a failure</returns>
        protected bool GetBoolean(string valueName, bool booleanDefaultValue, string subkeyPath = null)
        {
            int iDefaultValue = (booleanDefaultValue ? 1 : 0);
            bool returnValue = booleanDefaultValue;
            using (var registryKey = this.OpenSubkeyUnderBasekey(false, subkeyPath))
            {
                if (registryKey != null)
                {
                    // First check to see whether a value is there.
                    object o = registryKey.GetValue(valueName, null);
                    if (o != null)
                    {
                        // There's a value. Find out whether it's a DWORD or a STRING.
                        RegistryValueKind kind = registryKey.GetValueKind(valueName);
                        if (kind == RegistryValueKind.DWord)
                        {
                            //TODO: We shouldn't have to do 2, or 3 accesses of the Registry. jh
                            // It's a DWORD, so interpret it in the simplist way - one means True, zero means False.
                            int iValue = (int)registryKey.GetValue(valueName, iDefaultValue);
                            returnValue = (iValue == 0 ? false : true);
                        }
                        else if (kind == RegistryValueKind.String)
                        {
                            // It's a STRING - so use ParseLib to accept any reasonable expression of True vs False.
                            string stringDefaultValue = (booleanDefaultValue ? "True" : "False");
                            string stringValue = (string)registryKey.GetValue(valueName, stringDefaultValue);
                            if (!String.IsNullOrWhiteSpace(stringValue))
                            {
                                string x = stringValue.Trim().ToLower();
                                if (x == "0" || x == "no" || x == "false" || x == "off" || x == "zero")
                                {
                                    returnValue = false;
                                }
                                else if (x == "1" || x == "yes" || x == "true" || x == "on" || x == "one")
                                {
                                    returnValue = true;
                                }
                                else
                                {
                                    Console.WriteLine("in RegistryBase.GetBoolean(" + valueName + ",) unexpected string value: " + stringValue);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("in RegistryBase.GetBoolean(" + valueName + ",) the RegistryValueKind is " + kind);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("in RegistryBase.GetBoolean(" + valueName + ",) the key was not found.");
                }
            }
            return returnValue;
        }
        #endregion GetBoolean

        #region GetTriStateBoolean
        /// <summary>
        /// Retrieve the indicated integer value from the Windows Registry, from under subkeyPath (which is under BaseKeyPath).
        /// If the retrieval fails then the provided default value is returned.
        /// </summary>
        /// <param name="valueName">the name of the Registry value to retrieve it from</param>
        /// <param name="defaultValue">the default value to return if the retrieval fails</param>
        /// <param name="subkeyPath">the Registry subkey (under BaseKeyPath) under which to retrieve it from</param>
        /// <returns>The value from the Registry, or the default value if there's a failure</returns>
        protected TriStateBoolean GetTriStateBoolean(string valueName, bool? defaultValue, string subkeyPath = null)
        {
            TriStateBoolean result = TriStateBoolean.From(defaultValue);
            int integerDefaultValue = result.NumericValue;
            using (var registryKey = this.OpenSubkeyUnderBasekey(false, subkeyPath))
            {
                if (registryKey != null)
                {
                    int integerValue = (int)registryKey.GetValue(valueName, integerDefaultValue);
                    result = TriStateBoolean.From(integerValue);
                }
                else
                {
                    Console.WriteLine("in RegistryBase.GetTriStateBoolean(" + valueName + ",), access failed so returning the default value.");
                }
            }
            return result;
        }
        #endregion

        #region string[] GetSubkeyNames
        /// <summary>
        /// Retrieve all of the sub-key names from under the indicated subkey (under BaseKeyPath) in the Windows Registry.
        /// If there are none or the retrieval fails then null is returned.
        /// </summary>
        /// <param name="subkeyPath">the Registry subkey (under BaseKeyPath) under which to retrieve the key names from</param>
        /// <returns>an array of all the sub-key names, or null if none</returns>
        protected string[] GetSubkeyNames(string subkeyPath)
        {
            string[] aryResult = null;
            using (var registryKey = OpenSubkeyUnderBasekey(false, subkeyPath))
            {
                if (registryKey != null)
                {
                    aryResult = registryKey.GetSubKeyNames();
                }
                else
                {
                    Console.WriteLine("in RegistryBase.GetSubkeyNames, subkey " + subkeyPath + " not found.");
                }
            }
            return aryResult;
        }
        #endregion

        #region string[] GetValueNames
        /// <summary>
        /// Retrieve all of the value names from under the indicated subkey (under BaseKeyPath) in the Windows Registry.
        /// If there are none or the retrieval fails then null is returned.
        /// </summary>
        /// <param name="subkeyPath">the Registry subkey under which to retrieve the value names from</param>
        /// <returns>an array of all the value names, or null if none</returns>
        protected string[] GetValueNames(string subkeyPath)
        {
            string[] aryResult = null;
            using (var registryKey = OpenSubkeyUnderBasekey(false, subkeyPath))
            {
                if (registryKey != null)
                {
                    aryResult = registryKey.GetValueNames();
                }
                else
                {
                    Console.WriteLine("in RegistryBase.GetValueNames, subkey " + subkeyPath + " not found.");
                }
            }
            return aryResult;
        }
        #endregion

        #endregion Get methods

        #region IsSpecificToUser
        /// <summary>
        /// Get or set whether the registry values are accessed from that section of the Registry that is specific to the current user (ie, HKEY_CURRENT_USER)
        /// or accessible across all users on the local box (ie, HKEY_LOCAL_MACHINE).
        /// This needs to be set before the first Registry access if it is to be other than the default (which is true).
        /// </summary>
        public bool IsSpecificToUser
        {
            get { return m_isSpecificToUser; }
            set { m_isSpecificToUser = value; }
        }
        #endregion

        #region IsKeyPresentUnderBasePath
        /// <summary>
        /// Return true if the given subkey is present within the Windows Registry under the BaseKeyPath.
        /// If subkeyPath is left null (it's default argument-value) then the existing of the BaseKeyPath itself is tested.
        /// </summary>
        /// <param name="subkeyPath">a sub-key path, which would come under the BaseKeyPath. Null indicates we're only interested in the BaseKeyPath.</param>
        /// <returns>true if the given subkey does exist in the Registry</returns>
        public bool IsKeyPresentUnderBasePath(string subkeyPath = null)
        {
            bool result = false;
            RegistryKey registryKey = null;
            try
            {
                if (String.IsNullOrWhiteSpace(subkeyPath))
                {
                    if (this.IsSpecificToUser)
                    {
                        registryKey = Registry.CurrentUser.OpenSubKey(this.BaseKeyPath);
                    }
                    else
                    {
                        registryKey = Registry.LocalMachine.OpenSubKey(this.BaseKeyPath);
                    }
                }
                else
                {
                    registryKey = OpenSubkeyUnderBasekey(false, subkeyPath);
                }
                if (registryKey != null)
                {
                    result = true;
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x.GetType().ToString() + " in IsKeyPresentUnderBasePath(" + subkeyPath + ": " + x.Message);
            }
            finally
            {
                if (registryKey != null)
                {
                    registryKey.Dispose();
                }
            }
            return result;
        }
        #endregion

        #region IsRegistry32Bit
        /// <summary>
        /// If true, then the user is wanting to access the 32-bit view of the Registry instead of the 64-bit view.
        /// This only applies for 64-bit processes running on a 64-bit Windows host.
        /// </summary>
        public bool IsRegistry32Bit
        {
            get { return m_isRegistry32Bit; }
            set { m_isRegistry32Bit = value; }
        }
        #endregion

        #region ParentOfKeyPath
        /// <summary>
        /// Given a string that represents a Registry-key path (like "Key1\Key2\Key3"),
        /// return the parent of that key path (like "Key1\Key2").
        /// If the string is empty or has no back-slash character, then return an empty string.
        /// </summary>
        /// <param name="keyPath">the key-path to get the parent of</param>
        /// <returns>a string that represents the parent key of the given subkey</returns>
        public static string ParentOfKeyPath(string keyPath)
        {
            if (!String.IsNullOrWhiteSpace(keyPath))
            {
                int i = keyPath.LastIndexOf('\\');
                if (i != -1)
                {
                    return keyPath.Substring(0, i);
                }
            }
            return String.Empty;
        }
        #endregion

        #region SetKey method
        /// <summary>
        /// Store the given key (keyName) in the Windows Registry under BaseKeyPath (under subkeyNameUnderThat if that is non-null).
        /// </summary>
        /// <param name="keyName">the Registry key to set under BaseKeyPath</param>
        /// <param name="subkeyNameUnderThat">the Registry subkey under keyName to store it in, if non-null. Default is null.</param>
        protected void SetKey(string keyName, string subkeyNameUnderThat = null)
        {
            if (String.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentException("SetKey argument keyName must not be empty.");
            }
            RegistryKey registryKey = null;
            try
            {
                string subkeyPathToUse;
                if (String.IsNullOrWhiteSpace(subkeyNameUnderThat))
                {
                    subkeyPathToUse = keyName;
                }
                else
                {
                    subkeyPathToUse = keyName + @"\" + subkeyNameUnderThat;
                }
                registryKey = this.CreateSubkeyUnderBasekey(subkeyPathToUse);
            }
            catch (Exception x)
            {
                Console.WriteLine(x.GetType().ToString() + " in LogNutRegistry.SetKey(" + keyName + "," + StringLib.AsNonNullString(subkeyNameUnderThat) + "): " + x.Message);
            }
            finally
            {
                if (registryKey != null)
                {
                    registryKey.Dispose();
                }
            }
        }
        #endregion

        #region SetValue methods

        #region SetValue(,bool,)
        /// <summary>
        /// Store the given boolean value in the Windows Registry
        /// </summary>
        /// <param name="valueName">the Registry value-name to store it in</param>
        /// <param name="booleanValue">the boolean value to store</param>
        /// <param name="subkeyPath">the Registry subkey to store it under, underneath BaseKeyPath, if non-null</param>
        protected void SetValue(string valueName, bool booleanValue, string subkeyPath = null)
        {
            int iValue = (booleanValue ? 1 : 0);
            SetValue(valueName, iValue, subkeyPath);
        }
        #endregion

        #region SetValue(,int,)
        /// <summary>
        /// Store the given integer value in the Windows Registry as a DWORD value under the given subkeyPath (which is under BaseKeyPath).
        /// </summary>
        /// <param name="valueName">the Registry value-name to store it in</param>
        /// <param name="integerValue">the value to store, which will be as a DWORD</param>
        /// <param name="subkeyPath">the Registry subkey to store it under (with BaseKeyPath prefixing it) if non-null</param>
        protected void SetValue(string valueName, int integerValue, string subkeyPath = null)
        {
            if (String.IsNullOrWhiteSpace(valueName))
            {
                throw new ArgumentNullException("SetValue argument valueName must not be empty.");
            }
            using (var registryKey = this.CreateSubkeyUnderBasekey(subkeyPath))
            {
                if (registryKey != null)
                {
                    registryKey.SetValue(valueName, integerValue);
                }
                else // didn't work!
                {
                    Console.WriteLine("in RegistryBase.SetValue(" + valueName + "," + integerValue.ToString() + "," + StringLib.AsNonNullString(subkeyPath) + "), CreateSubkeyUnderBasekey(" + StringLib.AsNonNullString(subkeyPath) + ") failed!");
                }
            }
            //RegistryKey registryKey = null;
            //try
            //{
            //    string sKeyPath = BaseKeyPath + @"\" + subkeyPath;
            //    if (this.IsSpecificToUser)
            //    {
            //        registryKey = Registry.CurrentUser.CreateSubKey(sKeyPath);
            //    }
            //    else
            //    {
            //        registryKey = Registry.LocalMachine.CreateSubKey(sKeyPath);
            //    }
            //    if (registryKey != null)
            //    {
            //        registryKey.SetValue(valueName, integerValue);
            //    }
            //    else // didn't work!
            //    {
            //        Console.WriteLine("in RegistryBase.SetValue, CreateSubKey of " + sKeyPath + " failed!");
            //    }
            //}
            //catch (Exception x)
            //{
            //    Console.WriteLine(x.GetType().ToString() + " in RegistryBase.SetValue: " + x.Message);
            //}
            //finally
            //{
            //    if (registryKey != null)
            //    {
            //        registryKey.Dispose();
            //        registryKey = null;
            //    }
            //}
        }
        #endregion

        #region SetValue(,Int64,)
        /// <summary>
        /// Store the given 64-bit integer value in the Windows Registry as a DWORD value under the given subkeyPath (which is under BaseKeyPath).
        /// </summary>
        /// <param name="valueName">the Registry value-name to store it in</param>
        /// <param name="integer64BitValue">the value to store, which will be as a DWORD</param>
        /// <param name="subkeyPath">the Registry subkey to store it under, under BaseKeyPath, if non-null</param>
        protected void SetValue(string valueName, Int64 integer64BitValue, string subkeyPath = null)
        {
            if (String.IsNullOrWhiteSpace(valueName))
            {
                throw new ArgumentNullException("SetValue argument valueName must not be empty.");
            }
            using (var registryKey = this.CreateSubkeyUnderBasekey(subkeyPath))
            {
                if (registryKey != null)
                {
                    registryKey.SetValue(valueName, integer64BitValue, RegistryValueKind.QWord);
                }
                else // didn't work!
                {
                    Console.WriteLine("in RegistryBase.SetValue, CreateSubkeyUnderBasekey(" + StringLib.AsNonNullString(subkeyPath) + ") failed!");
                }
            }
        }
        #endregion

        #region SetValue(,double,)
        /// <summary>
        /// Store a double-precision numeric value (numericValue) in the Registry with the given name under BaseKeyPath,
        /// under subkeyPath if that is non-null.
        /// </summary>
        /// <param name="valueName">the Registry value-name to store it in</param>
        /// <param name="numericValue">the value to store</param>
        /// <param name="subkeyPath">the Registry subkey to store it under, under BaseKeyPath, if non-null</param>
        protected void SetValue(string valueName, double numericValue, string subkeyPath = null)
        {
            // Let's test this with some limit values!  cbl
            string sValue = numericValue.ToString();
            SetValue(valueName, sValue, subkeyPath);
        }
        #endregion

        #region SetValue(,string,)
        /// <summary>
        /// Store a string value in the Registry with the given name under BaseKeyPath (and furthermore under subkeyPath if that is non-null).
        /// If stringValue is an empty or null string then the value is removed from the Registry.
        /// </summary>
        /// <param name="valueName">the Registry value-name to store it in</param>
        /// <param name="stringValue">the value to store</param>
        /// <param name="subkeyPath">the Registry subkey to store it under (with BaseKeyPath prefixing it)</param>
        protected void SetValue(string valueName, string stringValue, string subkeyPath = null)
        {
            if (String.IsNullOrWhiteSpace(valueName))
            {
                throw new ArgumentException("SetValue argument valueName must not be empty.");
            }
            if (String.IsNullOrWhiteSpace(stringValue))
            {
                DeleteValue(valueName, subkeyPath);
            }
            else
            {
                using (var registryKey = this.CreateSubkeyUnderBasekey(subkeyPath))
                {
                    registryKey.SetValue(valueName, stringValue);
                }
            }
        }
        #endregion

        #endregion SetValue methods

        #region internal implementation

        //TODO: The bit-ness yet needs to be addressed.
        // See this article regarding the bit-ness: http://www.rhyous.com/2011/01/24/how-read-the-64-bit-registry-from-a-32-bit-application-or-vice-versa/

        protected RegistryBase()
        {
        }

        #region OpenSubkeyUnderBasekey
        /// <summary>
        /// Helper method for retrieving the correct RegistryKey, under the BaseKeyPath and optionally under a sub-key.
        /// </summary>
        /// <param name="withWriteAccess">Set this True if you need write access to the key.</param>
        /// <param name="subkeyPath">A subkey, which comes under the BaseKeyPath. If null = then directly under the BaseKeyPath.</param>
        /// <returns>a RegistryKey that is returned from calling OpenSubKey on that complete key-path</returns>
        protected RegistryKey OpenSubkeyUnderBasekey(bool withWriteAccess, string subkeyPath = null)
        {
            if (String.IsNullOrWhiteSpace(BaseKeyPath))
            {
                throw new ArgumentException("BaseKeyPath must be overridden to return a non-empty value.");
            }
            RegistryKey registryKey;
            if (subkeyPath == null)
            {
                if (this.IsSpecificToUser)
                {
                    registryKey = Registry.CurrentUser.OpenSubKey(BaseKeyPath, withWriteAccess);
                }
                else
                {
                    registryKey = Registry.LocalMachine.OpenSubKey(BaseKeyPath, withWriteAccess);
                }
            }
            else
            {
                string fullKeyPath = BaseKeyPath + @"\" + subkeyPath;
                if (this.IsSpecificToUser)
                {
                    registryKey = Registry.CurrentUser.OpenSubKey(fullKeyPath, withWriteAccess);
                }
                else
                {
                    registryKey = Registry.LocalMachine.OpenSubKey(fullKeyPath, withWriteAccess);
                }
            }
            return registryKey;
        }
        #endregion

        #region CreateSubkeyUnderBasekey
        /// <summary>
        /// Helper method for creating and retrieving a RegistryKey under the BaseKeyPath,
        /// This is needed whenever you need modify-access to the open key object.
        /// </summary>
        /// <param name="subkeyPath">A subkey, which comes under the BaseKeyPath - inapplicable if null. Default is null.</param>
        /// <returns>a RegistryKey that is returned from calling CreateSubKey on the complete key-path</returns>
        protected RegistryKey CreateSubkeyUnderBasekey(string subkeyPath = null)
        {
            if (String.IsNullOrWhiteSpace(BaseKeyPath))
            {
                throw new ArgumentException("BaseKeyPath must be overridden to return a non-empty value.");
            }
            RegistryKey registryKey;
            if (subkeyPath == null)
            {
                if (this.IsSpecificToUser)
                {
                    registryKey = Registry.CurrentUser.CreateSubKey(BaseKeyPath);
                }
                else
                {
                    try
                    {
                        registryKey = Registry.LocalMachine.CreateSubKey(BaseKeyPath);
                    }
                    catch (UnauthorizedAccessException uax1)
                    {
                        string msg1 = "You threw an UnauthorizedAccessException while attempting to access key " + BaseKeyPath + " under HKLM. You may need to invoke this program with Run As Administrator.";
                        string msg2;
                        Console.WriteLine("Hey!! " + msg1);
                        // Now test to see whether CurrentUser would yield the same problem, just to provide a hint to the developer.
                        try
                        {
                            registryKey = Registry.CurrentUser.CreateSubKey(BaseKeyPath);
                            msg2 = "Note: accessing it using CreateSubKey under CurrentUser yielded no exception.";
                        }
                        catch (UnauthorizedAccessException)
                        {
                            msg2 = "Attempting to access it using CreateSubKey under CurrentUser also resulted in an UnauthorizedAccessException.";
                        }
                        Console.WriteLine(msg2);
                        throw new UnauthorizedAccessException(msg1 + Environment.NewLine + msg2, uax1);
                    }
                }
            }
            else
            {
                string fullKeyPath = BaseKeyPath + @"\" + subkeyPath;
                if (this.IsSpecificToUser)
                {
                    registryKey = Registry.CurrentUser.CreateSubKey(fullKeyPath);
                }
                else
                {
                    registryKey = Registry.LocalMachine.CreateSubKey(fullKeyPath);
                }
            }
            return registryKey;
        }
        #endregion

        #region fields

        /// <summary>
        /// This dictates whether the registry values are accessed from that section of the Registry that is specific to the current user (ie, HKEY_CURRENT_USER)
        /// or accessible across all users on the local box (ie, HKEY_CURRENT_USER).
        /// This needs to be set before the first Registry access if it is to be other than the default (which is false).
        /// </summary>
        protected bool m_isSpecificToUser;

        /// <summary>
        /// If true, then the user is wanting to access the 32-bit view of the Registry instead of the 64-bit view.
        /// This only applies for 64-bit processes running on a 64-bit Windows host.
        /// </summary>
        protected bool m_isRegistry32Bit;

        #endregion fields

        #endregion internal implementation
    }
}
