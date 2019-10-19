using System.Text;

namespace CodeCommentFormatting.Reader
{
    /// <summary>
    /// Create tokens from characters.
    /// </summary>
    internal class TokenBuilder
    {
        private const char CHAR_CR = '\r';
        private const char CHAR_DQ = '"';
        private const char CHAR_EQ = '=';
        private const char CHAR_GT = '>';
        private const char CHAR_LF = '\n';
        private const char CHAR_LT = '<';
        private const char CHAR_SL = '/';
        private const char CHAR_US = '_';

        private readonly StringBuilder builder;
        private TokenType builderType;

        public TokenBuilder()
        {
            this.builder = new StringBuilder();
        }

        public bool IsEmpty => this.builder.Length == 0;

        public static TokenType GetCharType(char value)
        {
            if (value == CHAR_LF || value == CHAR_CR)
            {
                return TokenType.NewLine;
            }

            if (char.IsWhiteSpace(value))
            {
                return TokenType.Whitespace;
            }

            if (value == CHAR_EQ)
            {
                return TokenType.CharEquals;
            }

            if (value == CHAR_GT)
            {
                return TokenType.CharGT;
            }

            if (value == CHAR_LT)
            {
                return TokenType.CharLT;
            }

            if (value == CHAR_DQ)
            {
                return TokenType.CharDQ;
            }

            if (value == CHAR_SL)
            {
                return TokenType.CharSL;
            }

            return TokenType.Word;
        }

        public Token GetValue()
        {
            var t = new Token(this.builderType, this.builder.ToString());
            this.builder.Clear();
            return t;
        }

        public bool TryAppend(char value, TokenType type)
        {
            if (this.CanAppend(value, type))
            {
                this.Append(value, type);
                return true;
            }
            return false;
        }

        private void Append(char value, TokenType type)
        {
            this.builder.Append(value);
            this.builderType = type;
        }

        private bool CanAppend(char value, TokenType type)
        {
            // Can always append to empty builder.
            if (this.builder.Length == 0)
            {
                return true;
            }

            // Can only append characters of same type.
            if (type != this.builderType)
            {
                return false;
            }

            // The specific character types must be single.
            if (type >= TokenType.CharEquals)
            {
                return false;
            }

            // Each newline must be it's own item, but a CR and LF can be together.
            if (this.IsConsecutiveNewLine(value))
            {
                return false;
            }

            return true;
        }

        private bool IsConsecutiveNewLine(char currentChar)
        {
            if (this.builderType != TokenType.NewLine)
            {
                return false;
            }

            if (this.builder.Length == 0)
            {
                return false;
            }

            // Bit hacky but there are only two possible newline characters.
            return
                this.builder[0] == currentChar ||
                (this.builder.Length == 2 && this.builder[1] == currentChar);
        }
    }
}
