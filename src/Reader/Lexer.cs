using System;
using System.Collections;
using System.Collections.Generic;

namespace CodeCommentFormatting.Reader
{
    /// <summary>
    /// Read tokens into items.
    /// </summary>
    internal class Lexer : IEnumerable<LexerItem>
    {
        private readonly TokenReader reader;
        private readonly TextLexer textLexer;
        private readonly Queue<Token> tokens;
        private readonly XmlLexer xmlLexer;
        private LexerItem nextItem;
        private bool skipXml;

        public Lexer(TokenReader reader)
        {
            this.reader = reader;
            this.tokens = new Queue<Token>();
            this.xmlLexer = new XmlLexer();
            this.textLexer = new TextLexer();
            this.nextItem = null;
            this.skipXml = false;
        }

        public LexerItem Current { get; private set; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator<LexerItem> IEnumerable<LexerItem>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public LexerEnumerator GetEnumerator()
        {
            return new LexerEnumerator(this);
        }

        public bool Read()
        {
            while (true)
            {
                if (this.nextItem != null)
                {
                    this.Current = this.nextItem;
                    this.nextItem = null;
                    return true;
                }

                Token token;
                while ((token = this.ReadNextToken()) != null)
                {
                    if (!this.skipXml)
                    {
                        if (this.xmlLexer.TryAppend(token))
                        {
                            if (this.xmlLexer.IsValid)
                            {
                                // Done reading a complete valid XML item.
                                this.nextItem = this.xmlLexer.GetValue();
                                this.xmlLexer.Clear();

                                // If the plain text reader also has a valid item, it should be
                                // output first. Otherwise, we can directly output the XML item.
                                if (this.textLexer.IsValid)
                                {
                                    this.Current = this.textLexer.GetValue();
                                    this.textLexer.Clear();
                                }
                                else
                                {
                                    this.Current = this.nextItem;
                                    this.nextItem = null;
                                }
                                return true;
                            }
                            continue;
                        }

                        if (!this.xmlLexer.IsEmpty)
                        {
                            // Append failed XML tokens to token queue.
                            foreach (var item in this.xmlLexer.Tokens)
                                this.tokens.Enqueue(item);
                            this.xmlLexer.Clear();

                            // Append current token to queue.
                            this.tokens.Enqueue(token);

                            // Restart loop, and this time skip XML parsing.
                            this.skipXml = true;
                            continue;
                        }
                    }

                    if (this.textLexer.TryAppend(token))
                    {
                        // Continue with next token, reinstate XML handling.
                        this.skipXml = false;
                        continue;
                    }

                    if (this.textLexer.IsValid)
                    {
                        // Output paragraph.
                        this.Current = this.textLexer.GetValue();
                        this.textLexer.Clear();
                        this.tokens.Enqueue(token);
                        return true;
                    }
                }

                // No more tokens to read, output whatever was stored in XML and Text lexers.
                if (!this.xmlLexer.IsEmpty)
                {
                    // Get tokens from unfinished XML tag.
                    foreach (var item in this.xmlLexer.Tokens)
                    {
                        this.tokens.Enqueue(item);
                    }
                    this.xmlLexer.Clear();

                    // Restart loop and skip XML parsing.
                    this.skipXml = true;
                    continue;
                }

                if (this.textLexer.IsValid)
                {
                    this.Current = this.textLexer.GetValue();
                    this.textLexer.Clear();
                    return true;
                }

                return false;
            }
        }

        private Token ReadNextToken()
        {
            if (this.tokens.Count != 0)
                return this.tokens.Dequeue();

            if (this.reader.Read())
                return this.reader.Current;

            return null;
        }

        public class LexerEnumerator : IEnumerator<LexerItem>
        {
            private readonly Lexer lexer;

            public LexerEnumerator(Lexer lexer)
            {
                this.lexer = lexer;
            }

            public LexerItem Current => this.lexer.Current;

            object IEnumerator.Current => this.Current;

            public void Dispose()
            {
            }

            public bool MoveNext() => this.lexer.Read();

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
