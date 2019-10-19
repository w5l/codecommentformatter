using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCommentFormatting.Tests
{
    [TestClass]
    public class Lexer_Tests_Text : Lexer_Tests_Base
    {
        [TestMethod]
        public void Lexer_Tests_Text_Single()
        {
            var result = this.ReadAll("Lorem ipsum dolor");
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Lexer_Tests_Text_SingleWithNewLine()
        {
            var result = this.ReadAll("Lorem ipsum dolor\r\nsit amet.");
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Lexer_Tests_Text_SingleWithNewLines()
        {
            var result = this.ReadAll("Lorem ipsum dolor\r\nsit amet\r\nLorem ipsum dolor\r\nsit amet\r\n.");
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Lexer_Tests_Text_SplitOnDoubleNewLine()
        {
            // Should split on the double newline and leave one empty line.
            var result = this.ReadAll("Lorem ipsum dolor\r\n\r\nSit amet.");
            Assert.AreEqual(3, result.Count);
        }
    }
}
