using System;
using System.IO;

namespace CodeCommentFormatting.Reader
{
    /// <summary>
    /// Read text into tokens.
    /// </summary>
    public class TokenReader
    {
        private readonly TokenBuilder builder;
        private readonly TextReader reader;

        public TokenReader(TextReader reader)
        {
            this.reader = reader;
            this.builder = new TokenBuilder();
        }

        public Token Current { get; private set; }

        public bool Read()
        {
            int charCode;
            while ((charCode = this.reader.Read()) != -1)
            {
                var currentChar = (char)charCode;
                var currentCharType = TokenBuilder.GetCharType(currentChar);

                if (!this.builder.TryAppend(currentChar, currentCharType))
                {
                    if (!this.builder.IsEmpty)
                        this.Current = this.builder.GetValue();

                    if (!this.builder.TryAppend(currentChar, currentCharType))
                        throw new InvalidOperationException("Should be able to append to builder at this point, but cannot. This shouldn't happen ever.");

                    if (!this.builder.IsEmpty)
                        return true;
                }
            }

            if (!this.builder.IsEmpty)
            {
                this.Current = this.builder.GetValue();
                return true;
            }

            return false;
        }
    }
}