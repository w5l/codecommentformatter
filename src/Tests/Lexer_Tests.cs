using CodeCommentFormatting.Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeCommentFormatting.Tests
{
    [TestClass]
    public abstract class Lexer_Tests_Base
    {
        protected IEnumerable<ILexerItem> Read(string input)
        {
            var lexer = new Lexer(new TokenReader(new StringReader(input)));

            while (lexer.Read())
                yield return lexer.Current;
        }

        protected IList<ILexerItem> ReadAll(string input)
        {
            var result = this.Read(input).ToList();

            // Ensure that we can exactly reproduce the original input from the lexer results.
            Assert.AreEqual(input, result.Aggregate(string.Empty, (a, b) => a + b.ToString()));
            return result;
        }
    }
}
