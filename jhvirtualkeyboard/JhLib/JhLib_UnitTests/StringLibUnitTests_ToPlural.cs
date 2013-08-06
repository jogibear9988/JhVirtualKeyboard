using System;
using JhLib;
using NUnit.Framework;


namespace JHLib_UnitTests {

	[TestFixture, Timeout(60000)]
	public class StringLibUnitTests_ToPlural {

		[Test]
		public void ToPlural_NullArgument_ThrowsException() {
			Assert.Throws(typeof(ArgumentNullException), () => StringLib.ToPlural(null, null));
		}

		[TestCase("red", "reds")]
		[TestCase("boy", "boys")]
		[TestCase("bed", "beds")]
		[TestCase("book", "books")]
		[TestCase("pencil", "pencils")]
		[TestCase("day", "days")]
		public void ToPlural_RegularNouns_S(string singularWord, string expectedPluralWord) {
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(expectedPluralWord, actualOutput);
		}

		[TestCase("box", "boxes")]
		[TestCase("horse", "horses")]
		[TestCase("edge", "edges")]
		[TestCase("patch", "patches")]
		[TestCase("prize", "prizes")]
		public void ToPlural_RegularNouns_ES(string singularWord, string expectedPluralWord) {
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(expectedPluralWord, actualOutput);
		}

		[TestCase("spy", "spies")]
		[TestCase("poppy", "poppies")]
		[TestCase("penny", "pennies")]
		public void ToPlural_RegularNouns_YBecomesIES(string singularWord, string expectedPluralWord) {
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(expectedPluralWord, actualOutput);
		}

		[TestCase("osprey", "ospreys")]
		[TestCase("bay", "bays")]
		[TestCase("Germany", "Germanys")]
		public void ToPlural_RegularNouns_YBecomesYS(string singularWord, string expectedPluralWord) {
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(expectedPluralWord, actualOutput);
		}

		[TestCase("calf", "calves")]
		[TestCase("elf", "elves")]
		[TestCase("half", "halves")]
		[TestCase("hoof", "hooves")]
		[TestCase("knife", "knives")]
		[TestCase("leaf", "leaves")]
		[TestCase("life", "lives")]
		[TestCase("loaf", "loaves")]
		[TestCase("scarf", "scarves")]
		[TestCase("self", "selves")]
		[TestCase("sheaf", "sheaves")]
		[TestCase("shelf", "shelves")]
		[TestCase("thief", "thieves")]
		[TestCase("wife", "wives")]
		[TestCase("wolf", "wolves")]
		public void ToPlural_IrregularNouns_ForFEbecomesVES(string singularWord, string expectedPluralWord) {
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(expectedPluralWord, actualOutput);
		}

		[TestCase("man", "men")]
		[TestCase("fireman", "firemen")]
		[TestCase("foot", "feet")]
		[TestCase("goose", "geese")]
		[TestCase("louse", "lice")]
		[TestCase("tooth", "teeth")]
		[TestCase("mouse", "mice")]
		[TestCase("woman", "women")]
		[TestCase("child", "children")]
		[TestCase("ox", "oxen")]
		public void ToPlural_IrregularNouns_ChangeInVowelSound(string singularWord, string expectedPluralWord) {
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(expectedPluralWord, actualOutput);
		}

		[TestCase("Cod")]
		[TestCase("deer")]
		[TestCase("dice")]
		[TestCase("FISH")]
		[TestCase("offspring")]
		[TestCase("Perch")]
		[TestCase("SHEEP")]
		[TestCase("trout")]
		public void ToPlural_NounsThatDoNotChange(string singularWord) {
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(singularWord, actualOutput);
		}

		[TestCase("barracks")]
		[TestCase("crossroads")]
		[TestCase("dice")]
		[TestCase("gallows")]
		[TestCase("headquarters")]
		[TestCase("means")]
		[TestCase("series")]
		[TestCase("SPECIES")]
		public void ToPlural_NounsThatTakeAPluralFormForSingular(string singularWord) {
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(singularWord, actualOutput);
		}

		[TestCase("alga", "algae")]
		[TestCase("amoeba", "amoebae")]
		[TestCase("antenna", "antennae")]
		[TestCase("formula", "formulae")]
		[TestCase("larva", "larvae")]
		[TestCase("nebula", "nebulae")]
		[TestCase("vertebra", "vertebrae")]
		[TestCase("corpus", "corpora")]
		[TestCase("genus", "genera")]
		public void ToPlural_IrregularNouns_More(string singularWord, string expectedPluralWord) {
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(expectedPluralWord, actualOutput);
		}

		[TestCase("alumnus", "alumni")]
		[TestCase("bacillus", "bacilli")]
		[TestCase("cactus", "cacti")]
		[TestCase("focus", "foci")]
		[TestCase("fungus", "fungi")]
		[TestCase("nucleus", "nuclei")]
		[TestCase("radius", "radii")]
		[TestCase("stimulus", "stimuli")]
		[TestCase("syllabus", "syllabi")]
		[TestCase("terminus", "termini")]
		public void ToPlural_NounsThatEndInUSthatChangeToI(string singularWord, string expectedPluralWord) {
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(expectedPluralWord, actualOutput);
		}

		[TestCase("business", "businesses")]
		public void ToPlural_NounsThatEndInUSthatChangeToUSES(string singularWord, string expectedPluralWord) {
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(expectedPluralWord, actualOutput);
		}

		[TestCase("addendum", "addenda")]
		[TestCase("bacterium", "bacteria")]
		[TestCase("curriculum", "curricula")]
		[TestCase("datum", "data")]
		[TestCase("erratum", "errata")]
		[TestCase("medium", "media")]
		[TestCase("memorandum", "memoranda")]
		[TestCase("ovum", "ova")]
		[TestCase("stratum", "strata")]
		[TestCase("symposium", "symposia")]
		public void ToPlural_NounsThatEndInUMthatChangeToA(string singularWord, string expectedPluralWord) {
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(expectedPluralWord, actualOutput);
		}

		[TestCase("apex", "apices")]
		[TestCase("appendix", "appendices")]
		[TestCase("cervix", "cervices")]
		[TestCase("index", "indices")]
		[TestCase("matrix", "matrices")]
		[TestCase("vortex", "vortices")]
		public void ToPlural_NounsThatEndInExOrIXthatChangeToICES(string singularWord, string expectedPluralWord) {
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(expectedPluralWord, actualOutput);
		}

		[TestCase("analysis", "analyses")]
		[TestCase("axis", "axes")]
		[TestCase("basis", "bases")]
		[TestCase("crisis", "crises")]
		[TestCase("diagnosis", "diagnoses")]
		[TestCase("emphasis", "emphases")]
		[TestCase("hypothesis", "hypotheses")]
		[TestCase("neurosis", "neuroses")]
		[TestCase("oasis", "oases")]
		[TestCase("parenthesis", "parentheses")]
		[TestCase("synopsis", "synopses")]
		[TestCase("thesis", "theses")]
		public void ToPlural_NounsThatEndInISthatChangeToES(string singularWord, string expectedPluralWord) {
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(expectedPluralWord, actualOutput);
		}

		[TestCase("criterion", "criteria")]
		[TestCase("phenomenon", "phenomena")]
		[TestCase("automaton", "automata")]
		public void ToPlural_NounsThatEndInONthatChangeToA(string singularWord, string expectedPluralWord) {
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(expectedPluralWord, actualOutput);
		}

		[TestCase("tempo", "tempi")]
		[TestCase("libretto", "libretti")]
		[TestCase("virtuoso", "virtuosi")]
		[TestCase("cherub", "cherubim")]
		[TestCase("seraph", "seraphim")]
		[TestCase("schema", "schemata")]
		public void ToPlural_OtherIrregularNounsRetainedFromOtherLanguages(string singularWord, string expectedPluralWord) {
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(expectedPluralWord, actualOutput);
		}

		[TestCase("abyss", "abysses")]
		[TestCase("alumnus", "alumni")]
		[TestCase("analysis", "analyses")]
		[TestCase("aquarium", "aquaria")]
		[TestCase("arch", "arches")]
		[TestCase("atlas", "atlases")]
		[TestCase("axe", "axes")]
		[TestCase("baby", "babies")]
		[TestCase("bacterium", "bacteria")]
		[TestCase("batch", "batches")]
		[TestCase("beach", "beaches")]
		[TestCase("brush", "brushes")]
		[TestCase("bus", "buses")]
		[TestCase("calf", "calves")]
		[TestCase("chateau", "chateaux")]
		[TestCase("cherry", "cherries")]
		[TestCase("child", "children")]
		[TestCase("church", "churches")]
		[TestCase("circus", "circuses")]
		[TestCase("city", "cities")]
		[TestCase("cod", "cod")]
		[TestCase("copy", "copies")]
		[TestCase("crisis", "crises")]
		[TestCase("curriculum", "curricula")]
		[TestCase("deer", "deer")]
		[TestCase("dictionary", "dictionaries")]
		[TestCase("domino", "dominoes")]
		[TestCase("dwarf", "dwarves")]
		[TestCase("echo", "echoes")]
		[TestCase("elf", "elves")]
		[TestCase("emphasis", "emphases")]
		[TestCase("family", "families")]
		[TestCase("fax", "faxes")]
		[TestCase("fish", "fish")]
		[TestCase("flush", "flushes")]
		[TestCase("fly", "flies")]
		[TestCase("foot", "feet")]
		[TestCase("fungus", "fungi")]
		[TestCase("half", "halves")]
		[TestCase("hero", "heroes")]
		[TestCase("hippopotamus", "hippopotami")]
		[TestCase("hoax", "hoaxes")]
		[TestCase("hoof", "hooves")]
		[TestCase("index", "indices")]
		[TestCase("iris", "irises")]
		[TestCase("kiss", "kisses")]
		[TestCase("knife", "knives")]
		[TestCase("lady", "ladies")]
		[TestCase("leaf", "leaves")]
		[TestCase("life", "lives")]
		[TestCase("loaf", "loaves")]
		[TestCase("man", "men")]
		[TestCase("mango", "mangoes")]
		[TestCase("memorandum", "memoranda")]
		[TestCase("mess", "messes")]
		[TestCase("moose", "moose")]
		[TestCase("motto", "mottoes")]
		[TestCase("mouse", "mice")]
		[TestCase("runner-up", "runners-up")]
		[TestCase("story", "stories")]
		[TestCase("syllabus", "syllabi")]
		[TestCase("tax", "taxes")]
		[TestCase("thesis", "theses")]
		[TestCase("thief", "thieves")]
		[TestCase("tomato", "tomatoes")]
		[TestCase("tooth", "teeth")]
		[TestCase("tornado", "tornadoes")]
		[TestCase("try", "tries")]
		[TestCase("volcano", "volcanoes")]
		[TestCase("waltz", "waltzes")]
		[TestCase("wash", "washes")]
		[TestCase("watch", "watches")]
		[TestCase("wharf", "wharves")]
		[TestCase("wife", "wives")]
		[TestCase("woman", "women")]
		public void ToPlural_SomeMoreIrregularNouns_FromScribd(string singularWord, string expectedPluralWord) {
			// This list comes from: http://www.scribd.com/doc/3271143/List-of-100-Irregular-Plural-Nouns-in-English
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(expectedPluralWord, actualOutput, String.Format("expected {0} to result in output of {1} but got {2},",singularWord,expectedPluralWord,actualOutput));
		}

		[TestCase("beau", "beaux")]
		[TestCase("bureau", "bureaux")]
		[TestCase("ellipsis", "ellipses")]
		[TestCase("paralysis", "paralyses")]
		[TestCase("sheep", "sheep")]
		[TestCase("species", "species")]
		[TestCase("synthesis", "syntheses")]
		[TestCase("tableau", "tableaux")]
		[TestCase("vita", "vitae")]
		public void ToPlural_SomeMoreIrregularNouns_FromGSU(string singularWord, string expectedPluralWord) {
			// This list comes from: http://www2.gsu.edu/~wwwesl/egw/pluralsl.htm
			// I only added those that weren't already being tested for.
			string actualOutput = singularWord.ToPlural();
			Assert.AreEqual(expectedPluralWord, actualOutput, String.Format("expected {0} to result in output of {1} but got {2},", singularWord, expectedPluralWord, actualOutput));
		}

		[Test]
		public void WithPlurality_NullUnitName_ThrowsException() {
			int n = 1;
			Assert.Throws(typeof(ArgumentNullException), () => n.WithPlurality(null));
		}

		[Test]
		public void WithPlurality_EmptyUnitName_ThrowsException() {
			int n = 1;
			Assert.Throws(typeof(ArgumentException), () => n.WithPlurality(""));
		}

		[Test]
		public void WithPlurality_JustWhitespaceInUnitName_ThrowsException() {
			int n = 1;
			Assert.Throws(typeof(ArgumentException), () => n.WithPlurality(" "));
		}

		[Test]
		public void WithPlurality_NullForBothArguments_ThrowsException() {
			int n = 1;
			Assert.Throws(typeof(ArgumentNullException), () => n.WithPlurality(null, null));
		}

		[Test]
		public void WithPlurality_NullFor2ndArgument_ThrowsException() {
			int n = 1;
			Assert.Throws(typeof(ArgumentNullException), () => n.WithPlurality("woman", null));
		}

		[Test]
		public void WithPlurality_Empty2ndArgument_ThrowsException() {
			int n = 1;
			Assert.Throws(typeof(ArgumentException), () => n.WithPlurality("woman", ""));
		}

		[Test]
		public void WithPlurality_JustWhitespaceFor2ndArgument_ThrowsException() {
			int n = 1;
			Assert.Throws(typeof(ArgumentException), () => n.WithPlurality("woman", " "));
		}

		[Test]
		public void WithPlurality_NegativeQty_ThrowsException() {
			int n = -1;
			string actualOutput = n.WithPlurality("woman");
			Assert.AreEqual("-1 women", actualOutput);
		}

		[Test]
		public void WithPlurality_ZeroQty_ThrowsException() {
			int n = 0;
			string actualOutput = n.WithPlurality("woman");
			Assert.AreEqual("no women", actualOutput);
		}

		[Test]
		public void WithPlurality_QtyOf1_ThrowsException() {
			int n = 1;
			string actualOutput = n.WithPlurality("woman");
			Assert.AreEqual("one woman", actualOutput);
		}

		[Test]
		public void WithPlurality_QtyOf2_ThrowsException() {
			int n = 2;
			string actualOutput = n.WithPlurality("box");
			Assert.AreEqual("2 boxes", actualOutput);
		}

        [TestCase("minimum", "minima")]
        [TestCase("premium", "premiums")]
        [TestCase("pudendum", "pudenda")]
        [TestCase("colloquium", "colloquia")]
        [TestCase("auditorium", "auditoriums")]
        [TestCase("stadium", "stadiums")]
        [TestCase("octopus", "octopuses")]
        [TestCase("ignoramus", "ignoramuses")]
        [TestCase("fait accompli", "faits accomplis")]
        [TestCase("force majeure", "forces majeures")]
        [TestCase("feme sole", "femes sole")]
        [TestCase("feme covert", "femes covert")]
        [TestCase("femme couverte", "femmes couvertes")]
        [TestCase("embargo", "embargoes")]
        [TestCase("photo", "photos")]
        [TestCase("kilo", "kilos")]
        [TestCase("potato", "potatoes")]
        [TestCase("ratio", "ratios")]
        [TestCase("veto", "vetoes")]
        [TestCase("embryo", "embryos")]
        [TestCase("insured", "insureds")]
        [TestCase("court martial", "courts martial")]
        [TestCase("heir presumptive", "heirs presumptive")]
        [TestCase("lungful", "lungfuls")]
        [TestCase("spoonful", "spoonfuls")]
        [TestCase("handful", "handfuls")]
        [TestCase("ply", "plies")]
        public void ToPlural_FromGarnersDictOfLegalUsage(string singularWord, string expectedPluralWord)
        {
            // These come from p684
            string actualOutput = singularWord.ToPlural();
            Assert.AreEqual(expectedPluralWord, actualOutput, String.Format("expected {0} to result in output of {1} but got {2},", singularWord, expectedPluralWord, actualOutput));
        }
    }
}
