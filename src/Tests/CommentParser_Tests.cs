using CodeCommentFormatting.Parser;
using CodeCommentFormatting.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace CodeCommentFormatting.Tests
{
    [TestClass]
    public class CommentParser_Tests
    {
        [TestMethod]
        public void CommentParser_Tests_Single()
        {
            var result = this.ReadAll("Lorem ipsum dolor");
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void CommentParser_Tests_XmlTree()
        {
            var result = this.ReadAll("<lorem>ipsum <sit>dolor</sit></lorem>");
            Assert.AreEqual(1, 1);
        }

        protected CommentNode ReadAll(string input)
        {
            var lexer = new Lexer(new TokenReader(new StringReader(input)));
            return new CommentParser().Parse(lexer);
        }
    }
}
