using CodeCommentFormatting.Reader;
using System;
using System.Collections.Generic;

namespace CodeCommentFormatting.Parser
{
    public class CommentParser
    {
        public CommentNode Parse(IEnumerable<LexerItem> items)
        {
            return this.Parse(items.GetEnumerator());
        }

        public CommentNode Parse(IEnumerator<LexerItem> items)
        {
            var root = new CommentNode();
            this.Parse(root, items);
            return root;
        }

        private void Parse(CommentNode root, IEnumerator<LexerItem> items)
        {
            this.Parse(root, items, _ => false);
        }

        private void Parse(CommentNode root, IEnumerator<LexerItem> items, Predicate<XmlLexerItem> shouldCloseAfter)
        {
            while (items.MoveNext())
            {
                var item = items.Current;
                if (item is XmlLexerItem xmlItem)
                {
                    root.Add(this.Parse(xmlItem, items));

                    if (shouldCloseAfter(xmlItem))
                        return;
                }
                else if (item is TextLexerItem textItem)
                {
                    root.Add(new CommentNode(textItem));
                }
                else
                {
                    throw new InvalidOperationException($"Unhandled lexer item type '{item.GetType().Name}'.");
                }
            }
        }

        private CommentNode Parse(XmlLexerItem item, IEnumerator<LexerItem> items)
        {
            var node = new CommentNode(item);

            if (!item.IsCloseTag)
            {
                this.Parse(node, items, xml => !xml.IsOpenTag && xml.IsCloseTag && xml.TagName == item.TagName);
            }

            return node;
        }
    }
}
