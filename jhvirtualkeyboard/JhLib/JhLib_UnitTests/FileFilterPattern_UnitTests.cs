using JhLib;
using NUnit.Framework;


namespace JHLib_UnitTests
{
    [TestFixture, Timeout(60000)]
    public class FileFilterPattern_UnitTests
    {
        [TestCase("A.B")]
        [TestCase(".")]
        [TestCase("shit")]
        public void FilespecWildcard_Include_Matches(string path)
        {
            var f = new FileFilterPattern(true, "*", FilesystemMatchType.FilesOnly);
            Assert.True(f.MatchesPath(path));
            bool? r = f.IncludesPath(path);
            Assert.True(r.HasValue);
            Assert.IsNotNull(r);
            Assert.True(r.Value);
        }

        [TestCase("shit")]
        public void FilespecWildcard_Exclude_Matches(string path)
        {
            var f = new FileFilterPattern(false, "*", FilesystemMatchType.FilesOnly);
            Assert.True(f.MatchesPath(path));
            bool? r = f.IncludesPath(path);
            Assert.True(r.HasValue);
            Assert.IsNotNull(r);
            Assert.False(r.Value);
        }

        [TestCase("Able.OBJ")]
        [TestCase("X.obj")]
        public void FilespecStarDotObj_Include_Matches(string path)
        {
            var f = new FileFilterPattern(true, "*.obj", FilesystemMatchType.FilesOnly);
            Assert.True(f.MatchesPath(path));
            bool? r = f.IncludesPath(path);
            Assert.True(r.HasValue);
            Assert.IsNotNull(r);
            Assert.True(r.Value);
        }

        [TestCase("Able")]
        [TestCase("X.objX")]
        [TestCase("OBJ")]
        public void FilespecStarpObj_Include_DoesNotMatch(string path)
        {
            var f = new FileFilterPattern(true, "*.obj", FilesystemMatchType.FilesOnly);
            Assert.False(f.MatchesPath(path));
            bool? r = f.IncludesPath(path);
            Assert.False(r.HasValue);
            Assert.IsNull(r);
        }

        [TestCase("A.BA")]
        [TestCase("a.bac")]
        [TestCase("A.BACD")]
        [TestCase("A.BA.C")]
        public void FilespecApBAStar_DoesMatch(string path)
        {
            var f = new FileFilterPattern(true, "A.BA*", FilesystemMatchType.FilesOnly);
            Assert.True(f.MatchesPath(path));
            bool? r1 = f.IncludesPath(path);
            Assert.True(r1.HasValue);
            Assert.IsNotNull(r1);
            Assert.IsTrue(r1.Value);
            bool? r2 = f.ExcludesPath(path);
            Assert.True(r2.HasValue);
            Assert.IsNotNull(r2);
            Assert.IsFalse(r2.Value);
        }

        [TestCase("A.BA")]
        [TestCase("a.bac")]
        [TestCase("A.BACD")]
        [TestCase("A.BA.C")]
        [TestCase("ba.ba")]
        [TestCase("A.B")]
        public void FilespecApBCStar_DoesNotMatch(string path)
        {
            var f = new FileFilterPattern(false, "A.BC*", FilesystemMatchType.FilesOnly);
            Assert.False(f.MatchesPath(path));
            bool? r1 = f.IncludesPath(path);
            Assert.False(r1.HasValue);
            Assert.IsNull(r1);
            bool? r2 = f.ExcludesPath(path);
            Assert.False(r2.HasValue);
            Assert.IsNull(r2);
        }

        [TestCase("ABC.XYZ")]
        [TestCase("ABCD.XYZ")]
        public void FilespecABCQpXYZ_DoesMatch(string path)
        {
            var f = new FileFilterPattern(true, "ABC?.XYZ", FilesystemMatchType.FilesOnly);
            Assert.True(f.MatchesPath(path));
            bool? r1 = f.IncludesPath(path);
            Assert.True(r1.HasValue);
            Assert.IsNotNull(r1);
            Assert.IsTrue(r1.Value);
            bool? r2 = f.ExcludesPath(path);
            Assert.True(r2.HasValue);
            Assert.IsNotNull(r2);
            Assert.IsFalse(r2.Value);
        }

        [TestCase("AB.XYZ")]
        [TestCase("ZABC.XYZ")]
        [TestCase("ABCD")]
        [TestCase("ABC.X")]
        [TestCase("ABC.XY")]
        [TestCase("ABC.XYZM")]
        public void FilespecABCQpXYZ_DoesNotMatch(string path)
        {
            var f = new FileFilterPattern(true, "ABC?.XYZ", FilesystemMatchType.FilesOnly);
            Assert.False(f.MatchesPath(path));
            bool? r1 = f.IncludesPath(path);
            Assert.False(r1.HasValue);
            Assert.IsNull(r1);
            bool? r2 = f.ExcludesPath(path);
            Assert.False(r2.HasValue);
            Assert.IsNull(r2);
        }

    }
}
