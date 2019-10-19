using System.Diagnostics;

namespace CodeCommentFormatting.Reader
{
    [DebuggerDisplay("[{Type,nq}] {Value}")]
    public class Token
    {
        internal Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public virtual TokenType Type { get; }

        public string Value { get; }
    }
}