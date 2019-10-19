using System.Collections.Generic;
using System.Linq;

namespace CodeCommentFormatting.Reader
{
    public class LexerItem : ILexerItem
    {
        protected readonly Token[] tokens;

        public LexerItem(Token[] tokens)
        {
            this.tokens = tokens;
        }

        public IEnumerable<Token> Tokens => tokens;

        public override string ToString()
        {
            return this.Tokens.Aggregate(string.Empty, (a, b) => a + b.Value);
        }
    }
}