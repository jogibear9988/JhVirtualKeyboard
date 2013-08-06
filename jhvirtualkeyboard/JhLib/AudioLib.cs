// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;


namespace JhLib
{
    /// <summary>
    /// Helper for playing wav files, and System Events stored under HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default
    /// </summary>
    public static class AudioLib
    {
        //TODO: This article describes disposing of unmanaged resources and avoiding crashing the Win32 api.
        // http://www.daniweb.com/software-development/csharp/code/253704
        // I should do a series of unit-tests to check for that, and implement it if necessary.


        #region PlaySoundFile(string sFilename)

        /// <summary>
        /// Plays the given wav file asynchronously.
        /// </summary>
        /// <param name="sFilename">Pathname that points to the wave file</param>
        public static void PlaySoundFile(string sFilename)
        {
            PlaySoundFile(sFilename, false);
        }

        /// <summary>
        /// Plays the given wav file, syncronously or asyncronously depending upon the value of the 2nd parameter.
        /// </summary>
        /// <param name="sFilename">Pathname that points to the wave file</param>
        /// <param name="bSyncronously">flag that indicates whether to wait until the previous sound is finished (default is false)</param>
        public static void PlaySoundFile(string sFilename, bool bSyncronously)
        {
            string path;
            if (String.IsNullOrEmpty(DefaultSoundFileLocation))
            {
                path = sFilename;
            }
            else
            {
                path = Path.Combine(DefaultSoundFileLocation, sFilename);
                if (!File.Exists(path))
                {
                    path = sFilename;
                }
            }
            if (File.Exists(path))
            {
                if (bSyncronously)
                {
                    PlaySound(path, 0, (int)(SND.SND_SYNC | SND.SND_FILENAME | SND.SND_NOSTOP));
                }
                else
                {
                    PlaySound(path, 0, (int)(SND.SND_ASYNC | SND.SND_FILENAME | SND.SND_NOWAIT));
                }
            }
            else
            {
                string sMessage = "in AudioLib.PlaySoundFile(" + sFilename + "), the file neither of itself nor in the default path " + DefaultSoundFileLocation + " is found.";
                Console.WriteLine(sMessage);
            }
        }
        #endregion

        #region PlaySoundResource
        /// <summary>
        /// Play asyncronously an audio file which is embedded into this assembly (JhLib) as a resource.
        /// Caustion: This uses SoundPlayer, which in the case of .WAV files -- only plays PCM-recorded WAV files, which many .WAV files are not.
        /// </summary>
        /// <param name="unmanagedMemoryStream">An UnmanagedMemoryStream that refers to an embedded audio file. For example, global::JhLib.Properties.Resources.Incorrect</param>
        public static void PlaySoundResource(UnmanagedMemoryStream unmanagedMemoryStream)
        {
            try
            {
                // http://msdn.microsoft.com/en-us/library/3w5b27z4.aspx
                SoundPlayer player = new SoundPlayer();
                unmanagedMemoryStream.Seek(0, SeekOrigin.Begin);
                player.Stream = unmanagedMemoryStream;
                player.Play();
            }
            catch (Exception x)
            {
                Console.WriteLine("PlaySoundResource: Error trying to access or play the resource: " + x.Message);
            }
        }
        #endregion

        #region PlaySoundResourceEvenIfNotPCM
        /// <summary>
        /// Play an audio file which is embedded into this assembly (JhLib) as a resource.
        /// You can use this for .WAV files that are not encoded as PCM, as for example when they contain MP3 encodings
        /// for which PlaySoundResource does not work.
        /// </summary>
        /// <param name="unmanagedMemoryStream">An UnmanagedMemoryStream that refers to an embedded audio file. For example, global::JhLib.Properties.Resources.Incorrect</param>
        public static void PlaySoundResourceEvenIfNotPCM(UnmanagedMemoryStream unmanagedMemoryStream)
        {
            try
            {
                long n = unmanagedMemoryStream.Length;
                byte[] waveData = new byte[n];
                unmanagedMemoryStream.Read(waveData, 0, (int)n);
                // Note: I did not use SND_ASYNC here, because of the reasons described in this article: http://blogs.msdn.com/b/larryosterman/archive/2009/02/19/playsound-xxx-snd-memory-snd-async-is-almost-always-a-bad-idea.aspx
                PlaySound(waveData, 0, (int)(SND.SND_MEMORY | SND.SND_NOWAIT));
            }
            catch (Exception x)
            {
                Console.WriteLine("PlaySoundResourceEvenIfNotPCM: Error trying to access or play the resource: " + x.Message);
            }
        }
        #endregion

        #region PlaySoundEvent(string sSound)
        /// <summary>
        /// Plays the System Event stored under HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default
        /// </summary>
        /// <param name="sSound">SystemEvent Verb</param>
        public static void PlaySoundEvent(string sSound)
        {
            // See Chris Richner's excellent article: http://www.codeproject.com/Articles/2740/Play-Windows-sound-events-defined-in-system-Contro
            PlaySound(sSound, 0, (int)(SND.SND_ASYNC | SND.SND_ALIAS | SND.SND_NOWAIT));
        }
        #endregion

        #region DefaultSoundFileLocation
        /// <summary>
        /// Get or set the path (relative or absolute) to where the audio files will be assumed to be located,
        /// when unspecified. Default is "Sounds";
        /// </summary>
        public static string DefaultSoundFileLocation
        {
            get { return _defaultSoundFileLocation; }
            set { _defaultSoundFileLocation = value; }
        }
        #endregion

        #region Internal Implementation

        /// <summary>
        /// API Parameter Flags for Play method
        /// </summary>
        [Flags]
        public enum SND
        {
            SND_SYNC = 0x0000,/* play synchronously (default) */
            SND_ASYNC = 0x0001, /* play asynchronously */
            SND_NODEFAULT = 0x0002, /* silence (!default) if sound not found */
            SND_MEMORY = 0x0004, /* pszSound points to a memory file */
            SND_LOOP = 0x0008, /* loop the sound until next sndPlaySound */
            SND_NOSTOP = 0x0010, /* don't stop any currently playing sound */
            SND_NOWAIT = 0x00002000, /* don't wait if the driver is busy */
            SND_ALIAS = 0x00010000,/* name is a registry alias */
            SND_ALIAS_ID = 0x00110000, /* alias is a pre d ID */
            SND_FILENAME = 0x00020000, /* name is file name */
            SND_RESOURCE = 0x00040004, /* name is resource name or atom */
            SND_PURGE = 0x0040,  /* purge non-static events for task */
            SND_APPLICATION = 0x0080  /* look for application specific association */
        }

        [DllImport("winmm.dll", EntryPoint = "PlaySound", CharSet = CharSet.Auto)]
        private static extern int PlaySound(string sSound, int hmod, int flags);

        [DllImport("winmm.dll", EntryPoint = "PlaySound", CharSet = CharSet.Auto)]
        private static extern int PlaySound(byte[] waveData, int hmod, int flags);

        private static string _defaultSoundFileLocation = "Sounds";

        //[DllImport("winmm.dll")]
        //private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);


        //public static void PlayUsingMci(string filename)
        //{
        //    mciSendString("open \"" + filename + "\" type mpegvideo alias MediaFile", null, 0, IntPtr.Zero);
        //    mciSendString("play MediaFile", null, 0, IntPtr.Zero);
        //}

        //private static void StopPlayingUsingMci()
        //{
        //    mciSendString("close MediaFile", null, 0, IntPtr.Zero);
        //}


        #endregion Internal Implementation
    }
}
