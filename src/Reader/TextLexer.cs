using System.Collections.Generic;

namespace CodeCommentFormatting.Reader
{
    internal class TextLexer
    {
        private readonly Queue<Token> tokens;
        private bool allowNewLine;

        public TextLexer()
        {
            this.tokens = new Queue<Token>();
            this.allowNewLine = true;
        }

        public bool IsEmpty => this.tokens.Count == 0;

        public bool IsValid => !this.IsEmpty;

        public void Clear()
        {
            this.tokens.Clear();
            this.allowNewLine = true;
        }

        public TextLexerItem GetValue()
        {
            return new TextLexerItem(this.tokens.ToArray());
        }

        public bool TryAppend(Token token)
        {
            // If text item starts with a newline, it cannot add any other items and must remain an
            // empty line.
            if (this.allowNewLine == false && this.tokens.Count == 1)
            {
                return false;
            }

            // Cannot add two consecutive newlines.
            if (token.Type == TokenType.NewLine)
            {
                if (this.allowNewLine)
                {
                    this.allowNewLine = false;
                    this.tokens.Enqueue(token);
                    return true;
                }
                return false;
            }

            if (token.Type != TokenType.Whitespace)
            {
                this.allowNewLine = true;
            }

            this.tokens.Enqueue(token);
            return true;
        }
    }
}
