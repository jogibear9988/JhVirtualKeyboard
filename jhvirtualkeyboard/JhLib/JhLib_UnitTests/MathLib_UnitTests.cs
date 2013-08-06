using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JhLib;
using NUnit.Framework;


namespace JHLib_UnitTests
{
    [TestFixture, Timeout(60000)]
    public class MathLib_UnitTests
    {
        [Test]
        public void WithPeriodDelimiters_0_CorrectResult()
        {
            string result = MathLib.WithPeriodDelimiters(0);
            Assert.AreEqual("0", result);
        }

        [Test]
        public void WithPeriodDelimiters_1_CorrectResult()
        {
            string result = MathLib.WithPeriodDelimiters(1);
            Assert.AreEqual("1", result);
        }

        [Test]
        public void WithPeriodDelimiters_100_CorrectResult()
        {
            string result = MathLib.WithPeriodDelimiters(100);
            Assert.AreEqual("100", result);
        }

        [Test]
        public void WithPeriodDelimiters_1000_CorrectResult()
        {
            string result = MathLib.WithPeriodDelimiters(1000);
            Assert.AreEqual("1,000", result);
        }

        [Test]
        public void WithPeriodDelimiters_10000_CorrectResult()
        {
            string result = MathLib.WithPeriodDelimiters(10000);
            Assert.AreEqual("10,000", result);
        }

        [Test]
        public void WithPeriodDelimiters_1222333444_CorrectResult()
        {
            string result = MathLib.WithPeriodDelimiters(1222333444);
            Assert.AreEqual("1,222,333,444", result);
        }

        [Test]
        public void WithPeriodDelimiters_1p1_CorrectResult()
        {
            string result = MathLib.WithPeriodDelimiters(1.1);
            Assert.AreEqual("1.1", result);
        }

        [Test]
        public void WithPeriodDelimiters_1001p2_CorrectResult()
        {
            string result = MathLib.WithPeriodDelimiters(1001.2);
            Assert.AreEqual("1,001.2", result);
        }

        [Test]
        public void WithPeriodDelimiters_10002p03_CorrectResult()
        {
            string result = MathLib.WithPeriodDelimiters(10002.03);
            Assert.AreEqual("10,002.03", result);
        }

        [Test]
        public void WithPeriodDelimiters_201000p123_CorrectResult()
        {
            string result = MathLib.WithPeriodDelimiters(201000.123);
            Assert.AreEqual("201,000.123", result);
        }

        [Test]
        public void WithPeriodDelimiters_1000p1234_CorrectResult()
        {
            string result = MathLib.WithPeriodDelimiters(1000.1234);
            Assert.AreEqual("1,000.1234", result);
        }

        [Test]
        public void WithPeriodDelimiters_3p1415926_CorrectResult()
        {
            string result = MathLib.WithPeriodDelimiters(3.1415926);
            Assert.AreEqual("3.1415926", result);
        }

        [Test]
        public void WithPeriodDelimiters_Negative1_CorrectResult()
        {
            string result = MathLib.WithPeriodDelimiters(-1);
            Assert.AreEqual("-1", result);
        }

        [Test]
        public void WithPeriodDelimiters_Negative1234_CorrectResult()
        {
            string result = MathLib.WithPeriodDelimiters(-1234);
            Assert.AreEqual("-1,234", result);
        }
    }
}
