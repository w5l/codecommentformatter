using System.Collections.Generic;

namespace CodeCommentFormatting.Reader
{
    public interface ILexerItem
    {
        IEnumerable<Token> Tokens { get; }
    }
}
