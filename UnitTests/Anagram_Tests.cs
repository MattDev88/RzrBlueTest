using Task_1;

namespace Task1_Tests
{
    public class Anagram_Tests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("asdfqw","kjhbiuh", false)]
        [TestCase("asdfqw", "asdwqf", true)]
        [TestCase("a", "", false)]
        [TestCase("if your happy and you know it clap you hands !!!1!!1/?-_|", "hands you clap if your happy and you know it", true)]
        [TestCase("if your happy and you know it clap you hands !!!1!!1", "hands", false)]
        public void Test1(string word1, string word2, bool expectedResult)
        {
            Anagram anagram = new();

            bool res = anagram.Execute(word1, word2);

            if (res == expectedResult) { Assert.Pass(); }

            Assert.Fail("expected result did not match");

        }
    }
}