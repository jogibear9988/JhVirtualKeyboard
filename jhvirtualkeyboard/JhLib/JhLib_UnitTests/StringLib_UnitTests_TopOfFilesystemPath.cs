using System;
using JhLib;
using NUnit.Framework;


namespace JHLib_UnitTests
{
    [TestFixture, Timeout(60000)]
    public class StringLib_UnitTests_TopOfFilesystemPath
    {
        [Description("Ensure a null returns null and sets remainder to null.")]
        [Test]
        public void TopOfFilesystemPath_Null_CorrectResult()
        {
            string remainder;
            string result = StringLib.TopOfFilesystemPath(null, out remainder);
            Assert.IsNull(result);
            Assert.IsNull(remainder);
        }

        [Description("Ensure an empty string returns null and sets remainder to null.")]
        [Test]
        public void TopOfFilesystemPath_EmptyString_CorrectResult()
        {
            string remainder;
            string result = StringLib.TopOfFilesystemPath(String.Empty, out remainder);
            Assert.IsNull(result);
            Assert.IsNull(remainder);
        }

        [Description("Ensure a single slash returns null and sets remainder to null.")]
        [Test]
        public void TopOfFilesystemPath_Slash_CorrectResult()
        {
            string remainder;
            string result = StringLib.TopOfFilesystemPath(@"\", out remainder);
            Assert.IsNull(result);
            Assert.IsNull(remainder);
        }

        [Description("Ensure a single space returns nulls.")]
        [Test]
        public void TopOfFilesystemPath_SingleSpace_CorrectResult()
        {
            string remainder;
            string result = StringLib.TopOfFilesystemPath(" ", out remainder);
            Assert.IsNull(result);
            Assert.IsNull(remainder);
        }

        [Description("Ensure a path with a drive-spec returns just the drive-spec.")]
        [Test]
        public void TopOfFilesystemPath_DriveAndFolders_CorrectResult()
        {
            string remainder;
            string result = StringLib.TopOfFilesystemPath(@"C:\Folder1\Folder2", out remainder);
            Assert.AreEqual(@"C:\", result);
            Assert.AreEqual(@"Folder1\Folder2", remainder);
        }

        [Description("Ensure a path with 'C:' returns the drive-spec with slash and sets remainder to null.")]
        [Test]
        public void TopOfFilesystemPath_Drive_CorrectResult()
        {
            string remainder;
            string result = StringLib.TopOfFilesystemPath(@"C:", out remainder);
            Assert.AreEqual(@"C:\", result);
            Assert.IsNull(remainder);
        }

        [Description("Ensure a path with 'C:\' returns the drive-spec with slash and sets remainder to null.")]
        [Test]
        public void TopOfFilesystemPath_DriveAndSlash_CorrectResult()
        {
            string remainder;
            string result = StringLib.TopOfFilesystemPath(@"C:\", out remainder);
            Assert.AreEqual(@"C:\", result);
            Assert.IsNull(remainder);
        }

        [Description(@"Ensure Folder1\Folder2 returns Folder1")]
        [Test]
        public void TopOfFilesystemPath_2Folders_CorrectResult()
        {
            string remainder;
            string result = StringLib.TopOfFilesystemPath(@"Folder1\Folder2", out remainder);
            Assert.AreEqual("Folder1", result);
            Assert.AreEqual("Folder2", remainder);
        }

        [Description(@"Ensure ' Folder1\Folder2' ignores the leading space and returns Folder1")]
        [Test]
        public void TopOfFilesystemPath_2FoldersLeadingSpace_CorrectResult()
        {
            string remainder;
            string result = StringLib.TopOfFilesystemPath(@" Folder1\Folder2", out remainder);
            Assert.AreEqual("Folder1", result);
            Assert.AreEqual("Folder2", remainder);
        }

        [Description("Ensure a leading slash makes no difference")]
        [Test]
        public void TopOfFilesystemPath_Slash2Folders_CorrectResult()
        {
            string remainder;
            string result = StringLib.TopOfFilesystemPath(@"\Folder1\Folder2", out remainder);
            Assert.AreEqual("Folder1", result);
            Assert.AreEqual("Folder2", remainder);
        }

        [Description(@"Ensure 'Folder1\Folder2 ' ignores the leading space and returns Folder1")]
        [Test]
        public void TopOfFilesystemPath_2FoldersTrailingSpace_CorrectResult()
        {
            string remainder;
            string result = StringLib.TopOfFilesystemPath(@"Folder1\Folder2  ", out remainder);
            Assert.AreEqual("Folder1", result);
            Assert.AreEqual("Folder2", remainder);
        }

        [Description(@"Ensure 'Folder1\Folder2\ ' ignores the leading space and returns Folder1")]
        [Test]
        public void TopOfFilesystemPath_2FoldersTrailingSlashSpace_CorrectResult()
        {
            string remainder;
            string result = StringLib.TopOfFilesystemPath(@"Folder1\Folder2\  ", out remainder);
            Assert.AreEqual("Folder1", result);
            Assert.AreEqual("Folder2", remainder);
        }

        [Description("Ensure a trailing slash makes no difference")]
        [Test]
        public void TopOfFilesystemPath_2FoldersSlash_CorrectResult()
        {
            string remainder;
            string result = StringLib.TopOfFilesystemPath(@"Folder1\Folder2\", out remainder);
            Assert.AreEqual("Folder1", result);
            Assert.AreEqual("Folder2", remainder);
        }

        [Description("Ensure both a leading slash and trailing slash makes no difference")]
        [Test]
        public void TopOfFilesystemPath_Slash2FoldersSlash_CorrectResult()
        {
            string remainder;
            string result = StringLib.TopOfFilesystemPath(@"\Folder1\Folder2\", out remainder);
            Assert.AreEqual("Folder1", result);
            Assert.AreEqual("Folder2", remainder);
        }

        [Description("Folder1 returns Folder1")]
        [Test]
        public void TopOfFilesystemPath_1Folder_CorrectResult()
        {
            string remainder;
            string result = StringLib.TopOfFilesystemPath("Folder1", out remainder);
            Assert.AreEqual("Folder1", result);
            Assert.IsNull(remainder);
        }

        [Description(@"\Folder1 returns Folder1")]
        [Test]
        public void TopOfFilesystemPath_SlashFolder_CorrectResult()
        {
            string remainder;
            string result = StringLib.TopOfFilesystemPath(@"\Folder1", out remainder);
            Assert.AreEqual(@"Folder1", result);
            Assert.IsNull(remainder);
        }

        [Description(@"Folder1\Folder2\Folder3 returns Folder1")]
        [Test]
        public void TopOfFilesystemPath_3Folders_CorrectResult()
        {
            string remainder;
            string result = StringLib.TopOfFilesystemPath(@"Folder1\Folder2\Folder3", out remainder);
            Assert.AreEqual("Folder1", result);
            Assert.AreEqual(@"Folder2\Folder3", remainder);
        }

    }
}
