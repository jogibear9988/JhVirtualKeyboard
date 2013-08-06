using System;
using JhLib;
using NUnit.Framework;


namespace JHLib_UnitTests
{
    [TestFixture, Timeout(60000)]
    public class StringLib_UnitTests_NumberOf
    {
        [Test]
        public void NumberOf_2_ValidResult()
        {
            string sSource = "ABCA";
            char characterToTestFor = 'A';
            int n = StringLib.NumberOf(sSource, characterToTestFor);
            Assert.AreEqual(2, n);
        }

        [Test]
        public void NumberOf_1_ValidResult()
        {
            string sSource = "ABCD";
            char characterToTestFor = 'A';
            int n = StringLib.NumberOf(sSource, characterToTestFor);
            Assert.AreEqual(1, n);
        }

        [Test]
        public void NumberOf_EmptySource_ReturnZero()
        {
            string sSource = "";
            char characterToTestFor = 'A';
            int n = StringLib.NumberOf(sSource, characterToTestFor);
            Assert.AreEqual(0, n);
        }

        [Test]
        public void NumberOf_A_Return1()
        {
            string sSource = "A";
            char characterToTestFor = 'A';
            int n = StringLib.NumberOf(sSource, characterToTestFor);
            Assert.AreEqual(1, n);
        }

        [Test]
        public void NumberOf_AA_Return2()
        {
            string sSource = "Aa";
            char characterToTestFor = 'A';
            int n = StringLib.NumberOf(sSource, characterToTestFor);
            Assert.AreEqual(2, n);
        }

        [Test]
        public void NumberOf_AAA_Return3()
        {
            string sSource = "AAA";
            char characterToTestFor = 'A';
            int n = StringLib.NumberOf(sSource, characterToTestFor);
            Assert.AreEqual(3, n);
        }

        [Test]
        public void NumberOf_X_Return0()
        {
            string sSource = "X";
            char characterToTestFor = 'A';
            int n = StringLib.NumberOf(sSource, characterToTestFor);
            Assert.AreEqual(0, n);
        }

        [Test]
        public void NumberOf_XCommaY_Semicolon_Return0()
        {
            string sSource = "X, Y";
            char characterToTestFor = ';';
            int n = StringLib.NumberOf(sSource, characterToTestFor);
            Assert.AreEqual(0, n);
        }

        [Test]
        public void NumberOf_XCommaY_Comma_Return1()
        {
            string sSource = "X, Y";
            char characterToTestFor = ',';
            int n = StringLib.NumberOf(sSource, characterToTestFor);
            Assert.AreEqual(1, n);
        }

        [Test]
        public void NumberOf_NullSource_ThrowsException()
        {
            string sSource = null;
            char characterToTestFor = 'X';
            Assert.Throws(typeof(ArgumentNullException), () => StringLib.NumberOf(sSource, characterToTestFor));
        }

    }
}
