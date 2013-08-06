using System;
using JhLib;
using NUnit.Framework;


namespace JHLib_UnitTests
{
    [TestFixture, Timeout(60000)]
    class StringLib_UnitTests_ExpandTo
    {
        #region ExpandTo

        [TestCase]
        public void T01_ExpandTo_NullArgument_ThrowsException()
        {
            string inputText = null;
            Assert.Throws<ArgumentNullException>(() => StringLib.ExpandTo(inputText, 20));
        }

        [TestCase]
        public void T02_ExpandTo_EmptyString_ThrowsException()
        {
            string inputText = "";
            Assert.Throws<ArgumentException>(() => StringLib.ExpandTo(inputText, 20));
        }

        [TestCase]
        public void T03_ExpandTo_InputIsOneSpace_ThrowsException()
        {
            string inputText = " ";
            Assert.Throws<ArgumentException>(() => StringLib.ExpandTo(inputText, 20));
        }

        [TestCase]
        public void T04_ExpandTo_NegativeTargetLength_ThrowsException()
        {
            string inputText = "Anyway";
            int targetLength = -1;
            Assert.Throws<ArgumentOutOfRangeException>(() => StringLib.ExpandTo(inputText, targetLength));
        }

        [TestCase]
        public void T05_ExpandTo_ZeroTargetLength_ReturnsEmptyString()
        {
            string inputText = "Anything";
            int targetLength = 0;
            string expectedOutput = String.Empty;

            string actualOutput = StringLib.ExpandTo(inputText, targetLength);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestCase]
        public void T06_ExpandTo_StringIs1CharAndTargetLengthIs1_CorrectResult()
        {
            string inputText = "A";
            int targetLength = 1;
            string expectedOutput = "A";

            string actualOutput = StringLib.ExpandTo(inputText, targetLength);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestCase]
        public void T07_ExpandTo_StringIs2Char2AndTargetLengthIs1_CorrectResult()
        {
            string inputText = "XY";
            int targetLength = 1;
            string expectedOutput = "X";

            string actualOutput = StringLib.ExpandTo(inputText, targetLength);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestCase]
        public void T08_ExpandTo_StringIs2Char2AndTargetLengthIs2_CorrectResult()
        {
            string inputText = "AB";
            int targetLength = 2;
            string expectedOutput = "AB";

            string actualOutput = StringLib.ExpandTo(inputText, targetLength);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestCase]
        public void T09_ExpandTo_StringIs2Char2AndTargetLengthIs3_CorrectResult()
        {
            string inputText = "AB";
            int targetLength = 3;
            string expectedOutput = "ABA";

            string actualOutput = StringLib.ExpandTo(inputText, targetLength);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestCase]
        public void T10_ExpandTo_StringIs2Char2AndTargetLengthIs4_CorrectResult()
        {
            string inputText = "AB";
            int targetLength = 4;
            string expectedOutput = "ABAB";

            string actualOutput = StringLib.ExpandTo(inputText, targetLength);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestCase]
        public void T11_ExpandTo_StringIs2Char2AndTargetLengthIs5_CorrectResult()
        {
            string inputText = "AB";
            int targetLength = 5;
            string expectedOutput = "ABABA";

            string actualOutput = StringLib.ExpandTo(inputText, targetLength);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        #endregion ExpandTo
    }
}
