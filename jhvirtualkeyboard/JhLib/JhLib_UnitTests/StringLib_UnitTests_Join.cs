using System;
using JhLib;
using NUnit.Framework;


namespace JHLib_UnitTests
{
    [TestFixture, Timeout(60000)]
    class StringLib_UnitTests_Join
    {
        #region Join

        [TestCase]
        public void T01_Join_NullGlue_ThrowsException()
        {
            string glueText = null;
            Assert.Throws<ArgumentNullException>(() => StringLib.Join2(glueText, "left", "right"));
        }

        [TestCase]
        public void T02_Join_EmptyGlue_AppendsArgs()
        {
            string glueText = String.Empty;
            string leftText = "left";
            string rightText = "right";

            string actualOutput = StringLib.Join2(glueText, leftText, rightText);

            Assert.AreEqual("leftright", actualOutput);
        }

        [TestCase]
        public void T03_Join_EmptyGlueNullLeft_ResultIsRight()
        {
            string glueText = String.Empty;
            string leftText = null;
            string rightText = "right";

            string actualOutput = StringLib.Join2(glueText, leftText, rightText);

            Assert.AreEqual("right", actualOutput);
        }

        [TestCase]
        public void T04_Join_EmptyGlueNullRight_ResultIsLeft()
        {
            string glueText = String.Empty;
            string leftText = "A";
            string rightText = null;

            string actualOutput = StringLib.Join2(glueText, leftText, rightText);

            Assert.AreEqual(leftText, actualOutput);
        }

        [TestCase]
        public void T05_Join_EmptyGlueNullLeftNullRight_ResultIsEmpty()
        {
            string glueText = String.Empty;
            string leftText = null;
            string rightText = null;

            string actualOutput = StringLib.Join2(glueText, leftText, rightText);

            Assert.AreEqual(String.Empty, actualOutput);
        }

        [TestCase]
        public void T06_Join_EmptyGlueEmptyLeft_ResultIsRight()
        {
            string glueText = String.Empty;
            string leftText = String.Empty;
            string rightText = "Right";

            string actualOutput = StringLib.Join2(glueText, leftText, rightText);

            Assert.AreEqual(rightText, actualOutput);
        }

        [TestCase]
        public void T07_Join_EmptyGlueEmptyRight_ResultIsLeft()
        {
            string glueText = String.Empty;
            string leftText = "Left";
            string rightText = String.Empty;

            string actualOutput = StringLib.Join2(glueText, leftText, rightText);

            Assert.AreEqual(leftText, actualOutput);
        }

        [TestCase]
        public void T08_Join_EmptyGlueEmptyLeftEmptyRight_ResultIsEmpty()
        {
            string glueText = String.Empty;
            string leftText = String.Empty;
            string rightText = String.Empty;

            string actualOutput = StringLib.Join2(glueText, leftText, rightText);

            Assert.AreEqual(String.Empty, actualOutput);
        }

        [TestCase]
        public void T09_Join_EmptyLeft_ResultIsRight()
        {
            string glueText = "Glue";
            string leftText = String.Empty;
            string rightText = "Right";

            string actualOutput = StringLib.Join2(glueText, leftText, rightText);

            Assert.AreEqual(rightText, actualOutput);
        }

        [TestCase]
        public void T10_Join_EmptyRight_ResultIsLeft()
        {
            string glueText = "Glue";
            string leftText = "Left";
            string rightText = String.Empty;

            string actualOutput = StringLib.Join2(glueText, leftText, rightText);

            Assert.AreEqual(leftText, actualOutput);
        }

        [TestCase]
        public void T11_Join_NonEmptyArgs_CorrentResult()
        {
            string glueText = "Glue";
            string leftText = "Left";
            string rightText = "Right";

            string actualOutput = StringLib.Join2(glueText, leftText, rightText);

            Assert.AreEqual("LeftGlueRight", actualOutput);
        }

        [TestCase]
        public void T12_Join_GlueIsSpace_CorrentResult()
        {
            string glueText = " ";
            string leftText = "Left";
            string rightText = "Right";

            string actualOutput = StringLib.Join2(glueText, leftText, rightText);

            Assert.AreEqual("Left Right", actualOutput);
        }

        #endregion Join
    }
}
