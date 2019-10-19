using CodeCommentFormatting.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace CodeCommentFormatting.Tests
{
    [TestClass]
    public class XmlLexerItem_Tests
    {
        [TestMethod]
        public void XmlLexerItem_Tests_Attribute()
        {
            var item = this.Create("<test a=b>");

            Assert.IsTrue(item.IsOpenTag);
            Assert.IsFalse(item.IsCloseTag);
            Assert.AreEqual(item.TagName, "test");
        }

        [TestMethod]
        public void XmlLexerItem_Tests_Close()
        {
            var item = this.Create("</test>");

            Assert.IsFalse(item.IsOpenTag);
            Assert.IsTrue(item.IsCloseTag);
            Assert.AreEqual(item.TagName, "test");
        }

        [TestMethod]
        public void XmlLexerItem_Tests_Open()
        {
            var item = this.Create("<test>");

            Assert.IsTrue(item.IsOpenTag);
            Assert.IsFalse(item.IsCloseTag);
            Assert.AreEqual(item.TagName, "test");
        }

        [TestMethod]
        public void XmlLexerItem_Tests_SelfClose()
        {
            var item = this.Create("<test/>");

            Assert.IsTrue(item.IsOpenTag);
            Assert.IsTrue(item.IsCloseTag);
            Assert.AreEqual(item.TagName, "test");
        }

        [TestMethod]
        public void XmlLexerItem_Tests_SelfCloseSpace()
        {
            var item = this.Create("<test />");

            Assert.IsTrue(item.IsOpenTag);
            Assert.IsTrue(item.IsCloseTag);
            Assert.AreEqual(item.TagName, "test");
        }

        private XmlLexerItem Create(string value)
        {
            var tokens = new List<Token>();
            var reader = new TokenReader(new StringReader(value));
            while (reader.Read())
                tokens.Add(reader.Current);
            return new XmlLexerItem(tokens.ToArray());
        }
    }
}
