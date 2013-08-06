using System;
using JhLib;
using NUnit.Framework;


namespace JHLib_UnitTests
{
    [TestFixture]
    public class StringLib_UnitTests_IsTheSameFilesystemPath
    {
        [Test]
        public void IsTheSameFilesystemPath_NullPathToTest_Exception()
        {
            string pathReference = @"OnePath";
            string pathToTest = null;
            Assert.Throws(typeof(ArgumentNullException), () => StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, false));
        }

        [Test]
        public void IsTheSameFilesystemPath_NullPathRef_Exception()
        {
            string pathReference = null;
            string pathToTest = "Path";
            Assert.Throws(typeof(ArgumentNullException), () => StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, false));
        }

        [Test]
        public void IsTheSameFilesystemPath_BothEqual_CorrrectResult()
        {
            string pathReference = @"C:\Path1";
            string pathToTest = @"C:\Path1";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, true);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsTheSameFilesystemPath_Path1_CorrrectResult()
        {
            string pathReference = @"C:\Path1";
            string pathToTest = "Path1";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, false);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsTheSameFilesystemPath_SlashPath1_CorrrectResult()
        {
            string pathReference = @"C:\Path1";
            string pathToTest = @"\Path1";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, false);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsTheSameFilesystemPath_Path1Slash_CorrrectResult()
        {
            string pathReference = @"C:\Path1";
            string pathToTest = @"Path1\";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, false);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsTheSameFilesystemPath_FullPath1Slash_CorrrectResult()
        {
            string pathReference = @"C:\Path1";
            string pathToTest = @"C:\Path1\";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, true);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsTheSameFilesystemPath_Path1Slash_DrPath1_CorrrectResult()
        {
            string pathReference = @"C:\Path1\";
            string pathToTest = @"C:\Path1";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, true);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsTheSameFilesystemPath_Path1Slash_Path1Equal_CorrrectResult()
        {
            string pathReference = @"C:\Path1\";
            string pathToTest = @"C:\Path1\";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, true);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsTheSameFilesystemPath_Path1Slash_Path1_CorrrectResult()
        {
            string pathReference = @"C:\Path1\";
            string pathToTest = @"Path1";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, false);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsTheSameFilesystemPath_Path1Slash_SlashPath1_CorrrectResult()
        {
            string pathReference = @"C:\Path1\";
            string pathToTest = @"\Path1";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, false);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsTheSameFilesystemPath_Path1Slash_SlashPath1Slash_CorrrectResult()
        {
            string pathReference = @"C:\Path1\";
            string pathToTest = @"\Path1\";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, true);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsTheSameFilesystemPath_Capitalized_Lowercase_CorrrectResult()
        {
            string pathReference = @"OnePath";
            string pathToTest = "onepath";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, false);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsTheSameFilesystemPath_Capitalized_Uppercase_CorrrectResult()
        {
            string pathReference = @"OnePath";
            string pathToTest = "ONEPATH";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, false);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsTheSameFilesystemPath_2Levels_CorrrectResult()
        {
            string pathReference = @"C:\Path1\Path2";
            string pathToTest = @"Path1\Path2";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, false);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsTheSameFilesystemPath_2Levels_EndsWithSlash_CorrrectResult()
        {
            string pathReference = @"C:\Path1\Path2";
            string pathToTest = @"Path1\Path2\";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, false);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsTheSameFilesystemPath_2Levels_Path1_CorrrectResult()
        {
            string pathReference = @"C:\Path1\Path2";
            string pathToTest = @"Path1";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, false);
            Assert.IsFalse(result);
        }

        [Test]
        public void IsTheSameFilesystemPath_2Levels_XPath_CorrrectResult()
        {
            string pathReference = @"C:\Path1\Path2";
            string pathToTest = @"XPath";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, false);
            Assert.IsFalse(result);
        }

        [Test]
        public void IsTheSameFilesystemPath_2Levels_Path2_CorrrectResult()
        {
            string pathReference = @"C:\Path1\Path2";
            string pathToTest = @"Path2";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, false);
            Assert.IsFalse(result);
        }

        [Test]
        public void IsTheSameFilesystemPath_2LevelsNoDr_Path2_CorrrectResult()
        {
            string pathReference = @"Path1\Path2";
            string pathToTest = @"Path2";
            bool result = StringLib.IsTheSameFilesystemPath(pathReference, pathToTest, false);
            Assert.IsFalse(result);
        }

    }
}
