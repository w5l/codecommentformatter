using CodeCommentFormatting.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCommentFormatting.Tests
{
    [TestClass]
    public class Lexer_Tests_Xml : Lexer_Tests_Base
    {
        [TestMethod]
        public void Lexer_Tests_Xml_OpenTag()
        {
            var result = this.ReadAll("<ipsum>");
            Assert.AreEqual(1, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(XmlLexerItem));
        }

        [TestMethod]
        public void Lexer_Tests_Xml_CloseTag()
        {
            var result = this.ReadAll("</ipsum>");
            Assert.AreEqual(1, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(XmlLexerItem));
        }

        [TestMethod]
        public void Lexer_Tests_Xml_SelfCloseTag()
        {
            var result = this.ReadAll("<ipsum/>");
            Assert.AreEqual(1, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(XmlLexerItem));
        }

        [TestMethod]
        public void Lexer_Tests_Xml_OpenAndCloseTag()
        {
            var result = this.ReadAll("<ipsum></ipsum>");
            Assert.AreEqual(2, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(XmlLexerItem));
            Assert.IsInstanceOfType(result[1], typeof(XmlLexerItem));
        }

        [TestMethod]
        public void Lexer_Tests_Xml_OpenAndCloseTagWithContent()
        {
            var result = this.ReadAll("<ipsum>lorem</ipsum>");
            Assert.AreEqual(3, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(XmlLexerItem));
            Assert.IsInstanceOfType(result[1], typeof(TextLexerItem));
            Assert.IsInstanceOfType(result[2], typeof(XmlLexerItem));
        }

        [TestMethod]
        public void Lexer_Tests_Xml_Nested()
        {
            var result = this.ReadAll("<ipsum>lorem<dolor>sit amet</dolor></ipsum>");
            Assert.AreEqual(6, result.Count);
        }
    }
}
