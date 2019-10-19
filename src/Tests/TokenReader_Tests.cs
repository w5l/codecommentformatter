using CodeCommentFormatting.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeCommentFormatting.Tests
{
    [TestClass]
    public class TokenReader_Tests
    {
        [TestMethod]
        public void Reader_Tests_Basic_Word()
        {
            var result = this.ReadAll("Lorem");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TokenType.Word, result[0].Type);
            Assert.AreEqual("Lorem", result[0].Value);
        }

        [TestMethod]
        public void Reader_Tests_Basic_Whitespace_Space()
        {
            var result = this.ReadAll(" ");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TokenType.Whitespace, result[0].Type);
            Assert.AreEqual(" ", result[0].Value);
        }

        [TestMethod]
        public void Reader_Tests_Basic_Whitespace_Tab()
        {
            var result = this.ReadAll("\t");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TokenType.Whitespace, result[0].Type);
            Assert.AreEqual("\t", result[0].Value);
        }

        [TestMethod]
        public void Reader_Tests_Basic_Whitespace_TabAndSpace()
        {
            var result = this.ReadAll("\t ");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TokenType.Whitespace, result[0].Type);
            Assert.AreEqual("\t ", result[0].Value);
        }

        [TestMethod]
        public void Reader_Tests_Basic_NewLine()
        {
            var result = this.ReadAll("\r\n");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(TokenType.NewLine, result[0].Type);
            Assert.AreEqual("\r\n", result[0].Value);
        }

        [TestMethod]
        public void Reader_Tests_Basic_NewLine_MultipleLF()
        {
            var result = this.ReadAll("\n\n");
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(TokenType.NewLine, result[0].Type);
            Assert.AreEqual(TokenType.NewLine, result[1].Type);
            Assert.AreEqual("\n", result[0].Value);
            Assert.AreEqual("\n", result[1].Value);
        }

        [TestMethod]
        public void Reader_Tests_Basic_NewLine_MultipleCR()
        {
            var result = this.ReadAll("\r\r");
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(TokenType.NewLine, result[0].Type);
            Assert.AreEqual(TokenType.NewLine, result[1].Type);
            Assert.AreEqual("\r", result[0].Value);
            Assert.AreEqual("\r", result[1].Value);
        }

        [TestMethod]
        public void Reader_Tests_Basic_NewLine_MultipleCRLF()
        {
            var result = this.ReadAll("\r\n\r\n");
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(TokenType.NewLine, result[0].Type);
            Assert.AreEqual(TokenType.NewLine, result[1].Type);
            Assert.AreEqual("\r\n", result[0].Value);
            Assert.AreEqual("\r\n", result[1].Value);
        }

        [TestMethod]
        public void Reader_Tests_Combinations_WordsSpace()
        {
            var result = this.ReadAll("Lorem ipsum");
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(TokenType.Word, result[0].Type);
            Assert.AreEqual(TokenType.Whitespace, result[1].Type);
            Assert.AreEqual(TokenType.Word, result[2].Type);
        }

        [TestMethod]
        public void Reader_Tests_Combinations_WordsNewline()
        {
            var result = this.ReadAll("Lorem\nipsum");
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(TokenType.Word, result[0].Type);
            Assert.AreEqual(TokenType.NewLine, result[1].Type);
            Assert.AreEqual(TokenType.Word, result[2].Type);
        }

        [TestMethod]
        public void Reader_Tests_Xml_Split_Start()
        {
            var result = this.ReadAll("Lorem<ipsum");
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(TokenType.Word, result[0].Type);
            Assert.AreEqual(TokenType.CharLT, result[1].Type);
            Assert.AreEqual(TokenType.Word, result[2].Type);
        }

        [TestMethod]
        public void Reader_Tests_Xml_Split_End()
        {
            var result = this.ReadAll("Lorem>ipsum");
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(TokenType.Word, result[0].Type);
            Assert.AreEqual(TokenType.CharGT, result[1].Type);
            Assert.AreEqual(TokenType.Word, result[2].Type);
        }

        [TestMethod]
        public void Reader_Tests_Xml_Split()
        {
            var result = this.ReadAll("Lorem<ipsum>dolor");
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(TokenType.Word, result[0].Type);
            Assert.AreEqual(TokenType.CharLT, result[1].Type);
            Assert.AreEqual(TokenType.Word, result[2].Type);
            Assert.AreEqual(TokenType.CharGT, result[3].Type);
            Assert.AreEqual(TokenType.Word, result[4].Type);
        }

        [TestMethod]
        public void Reader_Tests_Xml_Nested()
        {
            var result = this.ReadAll("Lorem<ipsum><sit>dolor</sit>amet</ipsum>");
            Assert.AreEqual(17, result.Count);
        }

        private IList<Token> ReadAll(string input)
        {
            var result = Read(input).ToList();

            // Ensure that we can exactly reproduce the original input from the reader results.
            Assert.AreEqual(input, result.Aggregate(string.Empty, (a, b) => a + b.Value));
            return result;
        }

        private IEnumerable<Token> Read(string input)
        {
            var reader = new TokenReader(new StringReader(input));
            while (reader.Read())
                yield return reader.Current;
        }
    }
}
