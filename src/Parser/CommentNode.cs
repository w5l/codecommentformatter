using CodeCommentFormatting.Reader;
using System.Collections.Generic;
using System.Linq;

namespace CodeCommentFormatting.Parser
{
    public class CommentNode
    {
        private readonly List<CommentNode> children;

        public CommentNode()
        {
            this.children = new List<CommentNode>();
        }

        public CommentNode(LexerItem value) : this()
        {
            this.Value = value;
        }

        public IEnumerable<CommentNode> Children => this.children;

        public bool IsFirst => this.Previous == null;

        public bool IsLast => this.Next == null;

        public bool IsRoot => this.Parent == null;

        public CommentNode Next { get; private set; }

        public CommentNode Parent { get; private set; }

        public CommentNode Previous { get; private set; }

        public CommentNodeType Type { get; }

        public LexerItem Value { get; }

        internal CommentNode Add(CommentNode node)
        {
            node.Parent = this;
            var last = this.children.LastOrDefault();
            if (last != null)
            {
                last.Next = node;
                node.Previous = last;
            }
            this.children.Add(node);
            return node;
        }
    }
}
