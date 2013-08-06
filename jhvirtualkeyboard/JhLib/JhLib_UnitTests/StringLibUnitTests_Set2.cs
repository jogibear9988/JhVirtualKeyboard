using System;
using JhLib;
using NUnit.Framework;


namespace JHLib_UnitTests {

	[TestFixture, Timeout(60000)]
	public class StringLibUnitTests_Set2 {

		#region ConstainsDigit

		[Test]
		public void ContainsDigits_NullArg_ThrowsException()
		{
			string digitFound;
			int indexOfDigit;
			Assert.Throws(typeof(ArgumentNullException), () => StringLib.ContainsDigit(null, out digitFound, out indexOfDigit));
		}

		[Test]
		public void ContainsDigit_EmptyArg_ReturnsFalse() {
			string inputText = String.Empty;
			string digitFound;
			int indexOfDigit;
			bool actualOutput = inputText.ContainsDigit(out digitFound, out indexOfDigit);
			Assert.AreEqual(false, actualOutput);
		}

		[Test]
		public void ContainsDigit_ArgHasOneLetter_ReturnsFalse() {
			string inputText = "A";
			string digitFound;
			int indexOfDigit;
			bool actualOutput = inputText.ContainsDigit(out digitFound, out indexOfDigit);
			Assert.AreEqual(false, actualOutput);
		}

		[Test]
		public void ContainsDigit_ArgHas2NonDigitChars_ReturnsFalse() {
			string inputText = "Zo";
			string digitFound;
			int indexOfDigit;
			bool actualOutput = inputText.ContainsDigit(out digitFound, out indexOfDigit);
			Assert.AreEqual(false, actualOutput);
		}

		[TestCase("0")]
		[TestCase("1")]
		[TestCase("2")]
		[TestCase("3")]
		[TestCase("4")]
		[TestCase("5")]
		[TestCase("6")]
		[TestCase("7")]
		[TestCase("8")]
		[TestCase("9")]
		public void ContainsDigit_ArgHasOneDigit_ReturnsTrue(string inputText) {
			string digitFound;
			int indexOfDigit;
			bool actualOutput = inputText.ContainsDigit(out digitFound, out indexOfDigit);
			Assert.AreEqual(true, actualOutput);
			Assert.AreEqual(inputText, digitFound);
			Assert.AreEqual(0, indexOfDigit);
		}

		[TestCase("A0","0")]
		[TestCase(" 1","1")]
		[TestCase("<2","2")]
		[TestCase("?3","3")]
		[TestCase("x4","4")]
		[TestCase("q5","5")]
		[TestCase("Z6","6")]
		[TestCase("_7","7")]
		[TestCase("o8","8")]
		[TestCase("$9","9")]
		public void ContainsDigit_ArgHasOneNonDigitThenDigit_ReturnsTrue(string inputText, string expectedDigitFound) {
			string digitFound;
			int indexOfDigit;
			bool actualOutput = inputText.ContainsDigit(out digitFound, out indexOfDigit);
			Assert.AreEqual(true, actualOutput);
			Assert.AreEqual(expectedDigitFound, digitFound);
			Assert.AreEqual(1, indexOfDigit);
		}

		[TestCase("A0 ", "0")]
		[TestCase(" 1z", "1")]
		[TestCase("<2b", "2")]
		[TestCase("?35", "3")]
		[TestCase("x41", "4")]
		[TestCase("q5F", "5")]
		[TestCase("Z69", "6")]
		[TestCase("_7 ", "7")]
		[TestCase("o8o", "8")]
		[TestCase("$92", "9")]
		public void ContainsDigit_ArgHasOneNonDigitThenDigitThenMore_ReturnsCorrectResult(string inputText, string expectedDigitFound) {
			string digitFound;
			int indexOfDigit;
			bool actualOutput = inputText.ContainsDigit(out digitFound, out indexOfDigit);
			Assert.AreEqual(true, actualOutput);
			Assert.AreEqual(expectedDigitFound, digitFound);
			Assert.AreEqual(1, indexOfDigit);
		}

		#endregion ConstainsDigit

		#region ToPlural

		[Test]
		public void ToPlural_NullArgument_ThrowsException() {
			Assert.Throws(typeof(ArgumentNullException), () => StringLib.ToPlural(null, null));
		}

		#endregion ToPlural

		#region Matches

		[Test]
		public void Matches_TwoNullArguments() {
			string source = null;
			string pattern = null;
			bool actualOutput = StringLib.Matches(source, pattern);
			Assert.AreEqual(true, actualOutput);
		}

		#endregion Matches

		#region NumberOfLeadingSpaces

		[Test]
		public void NumberOfLeadingSpaces_NullArgument() {
			string s = null;
			int actualOutput = s.NumberOfLeadingSpaces();
			Assert.AreEqual(0, actualOutput);
		}

		[Test]
		public void NumberOfLeadingSpaces_EmptyArgument() {
			string s = "";
			int actualOutput = s.NumberOfLeadingSpaces();
			Assert.AreEqual(0, actualOutput);
		}

		[Test]
		public void NumberOfLeadingSpaces_OneSpaceArgument() {
			string s = " ";
			int actualOutput = s.NumberOfLeadingSpaces();
			Assert.AreEqual(1, actualOutput);
		}

		[Test]
		public void NumberOfLeadingSpaces_TwoSpaceArgument() {
			string s = "  ";
			int actualOutput = s.NumberOfLeadingSpaces();
			Assert.AreEqual(2, actualOutput);
		}

		[Test]
		public void NumberOfLeadingSpaces_OneTabArgument() {
			string s = "\t";
			int actualOutput = s.NumberOfLeadingSpaces();
			Assert.AreEqual(0, actualOutput);
		}

		[Test]
		public void NumberOfLeadingSpaces_OneSpaceBeforeCharArgument() {
			string s = " a";
			int actualOutput = s.NumberOfLeadingSpaces();
			Assert.AreEqual(1, actualOutput);
		}

		#endregion NumberOfLeadingSpaces

		#region WithoutAtStart

		[Test]
		public void WithoutAtStart_ValueFound() {
			string s = "xFiles";
			string textToRemoveAtStart = "x";
			string actualOutput = s.WithoutAtStart(textToRemoveAtStart);
			Assert.AreEqual("Files", actualOutput);
		}

		[Test]
		public void WithoutAtStart_ArgHasLeadingSpace() {
			string s = "xFiles";
			string textToRemoveAtStart = " x";
			string actualOutput = s.WithoutAtStart(textToRemoveAtStart);
			Assert.AreEqual("xFiles", actualOutput);
		}

		[Test]
		public void WithoutAtStart_ValueNotFound() {
			string s = "xFiles";
			string textToRemoveAtStart = "a";
			string actualOutput = s.WithoutAtStart(textToRemoveAtStart);
			Assert.AreEqual("xFiles", actualOutput);
		}

		[Test]
		public void WithoutAtStart_ValueLargerThanThis() {
			string s = "SmallStuff";
			string textToRemoveAtStart = "MoreThanSmallStuff";
			string actualOutput = s.WithoutAtStart(textToRemoveAtStart);
			Assert.AreEqual("SmallStuff", actualOutput);
		}

		[Test]
		public void WithoutAtStart_EmptyThisArg() {
			string s = "";
			string textToRemoveAtStart = "a";
			string actualOutput = s.WithoutAtStart(textToRemoveAtStart);
			Assert.AreEqual("", actualOutput);
		}

		[Test]
		public void WithoutAtStart_EmptyTextToRemoveArg() {
			string s = "Something";
			string textToRemoveAtStart = "";
			string actualOutput = s.WithoutAtStart(textToRemoveAtStart);
			Assert.AreEqual("Something", actualOutput);
		}

		[Test]
		public void WithoutAtStart_NullThisArg() {
			string s = null;
			string textToRemoveAtStart = "something to remove";
			Assert.Throws(typeof(ArgumentNullException), () => StringLib.WithoutAtStart(s, textToRemoveAtStart));
		}

		[Test]
		public void WithoutAtStart_NullTextToRemoveArg() {
			string s = "Something";
			string textToRemoveAtStart = null;
			string actualOutput = s.WithoutAtStart(textToRemoveAtStart);
			Assert.AreEqual("Something", actualOutput);
		}

		#endregion WithoutAtStart

		#region IsAVowel

		[TestCase('a', true)]
		[TestCase('A', true)]
		[TestCase('b', false)]
		[TestCase('B', false)]
		[TestCase('c', false)]
		[TestCase('C', false)]
		[TestCase('d', false)]
		[TestCase('D', false)]
		[TestCase('e', true)]
		[TestCase('E', true)]
		[TestCase('f', false)]
		[TestCase('F', false)]
		[TestCase('g', false)]
		[TestCase('G', false)]
		[TestCase('h', false)]
		[TestCase('H', false)]
		[TestCase('i', true)]
		[TestCase('I', true)]
		[TestCase('j', false)]
		[TestCase('J', false)]
		[TestCase('k', false)]
		[TestCase('K', false)]
		[TestCase('l', false)]
		[TestCase('L', false)]
		[TestCase('m', false)]
		[TestCase('M', false)]
		[TestCase('n', false)]
		[TestCase('N', false)]
		[TestCase('o', true)]
		[TestCase('O', true)]
		[TestCase('p', false)]
		[TestCase('P', false)]
		[TestCase('q', false)]
		[TestCase('Q', false)]
		[TestCase('r', false)]
		[TestCase('R', false)]
		[TestCase('s', false)]
		[TestCase('S', false)]
		[TestCase('t', false)]
		[TestCase('T', false)]
		[TestCase('u', true)]
		[TestCase('U', true)]
		[TestCase('v', false)]
		[TestCase('V', false)]
		[TestCase('w', false)]
		[TestCase('W', false)]
		[TestCase('x', false)]
		[TestCase('X', false)]
		[TestCase('y', false)]
		[TestCase('Y', false)]
		[TestCase('z', false)]
		[TestCase('Z', false)]
		[TestCase('<', false)]
		[TestCase('>', false)]
		public void IsAVowel_VariousArgs(char charToCheck, bool expectedResult) {
			bool actualOutput = charToCheck.IsAVowel();
			Assert.AreEqual(expectedResult, actualOutput);
		}
		#endregion IsAVowel

		#region CharacterDecription

		[TestCase('`', "grave accent")]
		[TestCase('~', "tilde")]
		[TestCase('!', "exclamation-mark")]
		[TestCase('@', "at-sign")]
		[TestCase('#', "pound-sign")]
		[TestCase('$', "dollar-sign")]
		[TestCase('%', "percentage-sign")]
		[TestCase('^', "caret or circumflex accent")]
		[TestCase('&', "ampersand")]
		[TestCase('*', "asterisk")]
		[TestCase('(', "left parenthesis")]
		[TestCase(')', "right parenthesis")]
		[TestCase('-', "hyphen")]
		[TestCase('_', "underscore")]
		[TestCase('=', "equal-sign")]
		[TestCase('+', "plus-sign")]
		[TestCase('\t', "tab")]
		[TestCase('[', "left bracket")]
		[TestCase('{', "left brace")]
		[TestCase(']', "right bracket")]
		[TestCase('}', "right brace")]
		[TestCase('\\', "back-slash")]
		[TestCase('|', "vertical bar")]
		[TestCase(';', "semicolon")]
		[TestCase(':', "colon")]
		[TestCase('\'', "single-quote")]
		[TestCase('"', "double-quote")]
		[TestCase(',', "comma")]
		[TestCase('<', "less-than")]
		[TestCase('.', "period")]
		[TestCase('>', "greater-than")]
		[TestCase('/', "forward-slash")]
		[TestCase('?', "question-mark")]
		[TestCase(' ', "space")]
		[TestCase('1', "digit one")]
		[TestCase('2', "digit two")]
		[TestCase('3', "digit three")]
		[TestCase('4', "digit four")]
		[TestCase('5', "digit five")]
		[TestCase('6', "digit six")]
		[TestCase('7', "digit seven")]
		[TestCase('8', "digit eight")]
		[TestCase('9', "digit nine")]
		[TestCase('0', "digit zero")]
		[TestCase('a', "letter \"a\"")]
		[TestCase('A', "letter \"A\"")]
		public void CharacterDecription_MyriadArgs(char ch, string expectedName) {
			string actualOutput = ch.CharacterDecription();
			Assert.AreEqual(expectedName, actualOutput);
		}

		[Test]
		public void CharacterDecription_NonKeyboardChar() {
			int n = 129;
			char ch = (char)n;
			string actualOutput = ch.CharacterDecription();
			Assert.AreEqual("(UNICODE codepoint 129)", actualOutput);
		}

		[Test]
		public void CharacterDecription_NonKeyboardAsciiChar() {
			int n = 2;
			char ch = (char)n;
			string actualOutput = ch.CharacterDecription();
			Assert.AreEqual("(ASCII control-code 2)", actualOutput);
		}

		#endregion CharacterDecription

		#region ToCardinal

		[TestCase('0', "zero")]
		[TestCase('1', "one")]
		[TestCase('2', "two")]
		[TestCase('3', "three")]
		[TestCase('4', "four")]
		[TestCase('5', "five")]
		[TestCase('6', "six")]
		[TestCase('7', "seven")]
		[TestCase('8', "eight")]
		[TestCase('9', "nine")]
		public void Char_ToCardinal(char charToCheck, string expectedOutput) {
			string actualOutput = charToCheck.ToCardinal();
			Assert.AreEqual(expectedOutput, actualOutput);
		}

		[Test]
		public void Char_ToCardinal_NonDigitArg_ThrowsException() {
			Char ch = 'A';
			Assert.Throws(typeof(ArgumentException), () => ch.ToCardinal());
		}

		#endregion ToCardinal

        #region Titlecase

        [TestCase]
        public void Titlecase01_EmptyInput_YieldsEmptyOutput()
        {
            string inputText = "";
            string actualOutput = StringLib.Titlecase(inputText);
            Assert.AreEqual("", actualOutput);
        }

        [TestCase]
        public void Titlecase02_NullInput_YieldsEmptyOutput()
        {
            string inputText = null;
            string actualOutput = StringLib.Titlecase(inputText);
            Assert.AreEqual("", actualOutput);
        }

        [TestCase]
        public void Titlecase03_OneUppercaseLetterInput_YieldsSameAsOutput()
        {
            string inputText = "A";
            string actualOutput = StringLib.Titlecase(inputText);
            Assert.AreEqual("A", actualOutput);
        }

        [TestCase]
        public void Titlecase04_OneLowercaseLetterInput_YieldsUppercaseLetterOutput()
        {
            string inputText = "b";
            string actualOutput = StringLib.Titlecase(inputText);
            Assert.AreEqual("B", actualOutput);
        }

        [TestCase]
        public void Titlecase05_TwoLowercaseLetters_CorrectOutput()
        {
            string inputText = "xy";
            string actualOutput = StringLib.Titlecase(inputText);
            Assert.AreEqual("Xy", actualOutput);
        }

        [TestCase]
        public void Titlecase06_TwoUppercaseLetters_CorrectOutput()
        {
            string inputText = "BC";
            string actualOutput = StringLib.Titlecase(inputText);
            Assert.AreEqual("Bc", actualOutput);
        }

        [TestCase]
        public void Titlecase07_UcLcLettersInput_CorrectOutput()
        {
            string inputText = "Cd";
            string actualOutput = StringLib.Titlecase(inputText);
            Assert.AreEqual("Cd", actualOutput);
        }

        [TestCase]
        public void Titlecase08_LcLetterUcLetter_CorrectOutput()
        {
            string inputText = "eF";
            string actualOutput = StringLib.Titlecase(inputText);
            Assert.AreEqual("Ef", actualOutput);
        }

        [TestCase]
        public void Titlecase09_1Digit_SameOutput()
        {
            string inputText = "1";
            string actualOutput = StringLib.Titlecase(inputText);
            Assert.AreEqual("1", actualOutput);
        }

        [TestCase]
        public void Titlecase10_2Digits_SameOutput()
        {
            string inputText = "23";
            string actualOutput = StringLib.Titlecase(inputText);
            Assert.AreEqual("23", actualOutput);
        }

        [TestCase]
        public void Titlecase11_LcLetter2Digits_CorrectOutput()
        {
            string inputText = "a45";
            string actualOutput = StringLib.Titlecase(inputText);
            Assert.AreEqual("A45", actualOutput);
        }

        [TestCase]
        public void Titlecase12_DigitsEmbeddedUcLetter_CorrectOutput()
        {
            string inputText = "6X7";
            string actualOutput = StringLib.Titlecase(inputText);
            Assert.AreEqual("6x7", actualOutput);
        }

        #endregion Titlecase
    }
}
