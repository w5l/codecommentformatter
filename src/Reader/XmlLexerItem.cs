using System;
using System.Linq;

namespace CodeCommentFormatting.Reader
{
    public class XmlLexerItem : LexerItem
    {
        public XmlLexerItem(Token[] tokens) : base(tokens)
        {
            if (tokens.Length < 3)
                throw new InvalidOperationException($"Cannot create {nameof(XmlLexerItem)} from this token collection.");
            if (tokens[0].Type != TokenType.CharLT)
                throw new InvalidOperationException($"Token collection for {nameof(XmlLexerItem)} must start with {TokenType.CharLT:g}.");
            if (tokens[tokens.Length - 1].Type != TokenType.CharGT)
                throw new InvalidOperationException($"Token collection for {nameof(XmlLexerItem)} must end with {TokenType.CharGT:g}.");

            this.TagName = this.tokens.First(t => t.Type == TokenType.Word).Value;

            // If the XML item starts with a word (instead of a slash), it's an opening tag.
            this.IsOpenTag = this.tokens[1].Type == TokenType.Word;

            // If it starts with a slash or ends with a slash, it's a closing tag.
            this.IsCloseTag = this.tokens[1].Type == TokenType.CharSL || this.tokens[tokens.Length - 2].Type == TokenType.CharSL;
        }

        public bool IsCloseTag { get; }

        public bool IsOpenTag { get; }

        public string TagName { get; }
    }
}
