using System;
using System.Collections.Generic;

namespace CodeCommentFormatting.Reader
{
    internal class XmlLexer
    {
        private readonly Queue<Token> tokens;
        private XmlLexerPosition position = XmlLexerPosition.None;
        private bool requireWhiteSpace = false;

        public XmlLexer()
        {
            this.tokens = new Queue<Token>();
        }

        private enum XmlLexerPosition
        {
            None,
            Open,
            TagName,
            AttributeName,
            AttributeEquals,
            AttributeValue,
            Close
        }

        public bool IsEmpty => this.tokens.Count == 0;

        public bool IsValid => this.position == XmlLexerPosition.Close;

        public IEnumerable<Token> Tokens => this.tokens;

        public void Clear()
        {
            this.tokens.Clear();
            this.position = XmlLexerPosition.None;
            this.requireWhiteSpace = false;
        }

        public XmlLexerItem GetValue()
        {
            if (this.position != XmlLexerPosition.Close)
            {
                throw new InvalidOperationException($"Invalid lexer state '{this.position:g}'.");
            }

            return new XmlLexerItem(this.tokens.ToArray());
        }

        public bool TryAppend(Token token)
        {
            this.position = this.TryAppendInternal(token);
            if (this.position != XmlLexerPosition.None)
            {
                this.tokens.Enqueue(token);
                return true;
            }
            return false;
        }

        private bool AllowCloseTag()
        {
            if (this.position == XmlLexerPosition.TagName)
                return true;
            if (this.position == XmlLexerPosition.AttributeValue)
                return true;
            return false;
        }

        private bool AllowForwardSlash()
        {
            if (this.position == XmlLexerPosition.Open)
                return true;
            if (this.AllowCloseTag())
                return true;
            return false;
        }

        private bool IsValidXmlTagName(string value)
        {
            // Using the following XML naming rules:
            // - A Name is a token beginning with a letter or one of a few punctuation characters,
            // and continuing with letters, digits, hyphens, underscores, colons, or full stops,
            // together known as name characters.

            // TODO: Use own validation.
            try
            {
                System.Xml.XmlConvert.VerifyName(value);
                return true;
            }
            catch (System.Xml.XmlException) { }
            return false;
        }

        private XmlLexerPosition TryAppendInternal(Token token)
        {
            // Open sign can only be the first character.
            if (this.position == XmlLexerPosition.None)
            {
                if (token.Type == TokenType.CharLT)
                {
                    return XmlLexerPosition.Open;
                }
                return XmlLexerPosition.None;
            }

            // Cannot add anything after closing.
            if (this.position == XmlLexerPosition.Close)
            {
                return XmlLexerPosition.None;
            }

            // Close sign can be added after tag name or after attribute value, and doesn't care
            // about whitespace requirements.
            if (token.Type == TokenType.CharGT && this.AllowCloseTag())
            {
                return XmlLexerPosition.Close;
            }

            // Handle forward slash (for self closing tags).
            if (token.Type == TokenType.CharSL)
            {
                if (this.AllowForwardSlash())
                {
                    return this.position;
                }

                return XmlLexerPosition.None;
            }

            // Whitespace and new lines can always be added after the tag name.
            if (token.Type == TokenType.Whitespace || token.Type == TokenType.NewLine)
            {
                if (this.position >= XmlLexerPosition.TagName)
                {
                    requireWhiteSpace = false;
                    return this.position;
                }

                return XmlLexerPosition.None;
            }

            // If we still require whitespace here, something is wrong.
            if (requireWhiteSpace)
            {
                return XmlLexerPosition.None;
            }

            // Equals sign can only be added between attribute name and attribute value.
            if (token.Type == TokenType.CharEquals && this.position == XmlLexerPosition.AttributeName)
            {
                return XmlLexerPosition.AttributeEquals;
            }

            if (token.Type == TokenType.Word)
            {
                if (this.position == XmlLexerPosition.Open && this.IsValidXmlTagName(token.Value))
                {
                    this.requireWhiteSpace = true;
                    return XmlLexerPosition.TagName;
                }
                if (this.position == XmlLexerPosition.TagName && this.IsValidXmlTagName(token.Value))
                {
                    return XmlLexerPosition.AttributeName;
                }

                if (this.position == XmlLexerPosition.AttributeEquals)
                {
                    this.requireWhiteSpace = true;
                    return XmlLexerPosition.AttributeValue;
                }

                return XmlLexerPosition.None;
            }

            return XmlLexerPosition.None;
        }
    }
}
