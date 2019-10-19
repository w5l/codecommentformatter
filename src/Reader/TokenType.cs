namespace CodeCommentFormatting.Reader
{
    public enum TokenType
    {
        Word,
        Whitespace,

        /// <summary>
        /// CR, LF or both.
        /// </summary>
        NewLine,

        //XmlTagStart,
        //XmlAttribute,
        //XmlTagEnd,

        // Single character items

        /// <summary>
        /// Equals =
        /// </summary>
        CharEquals = 10,

        /// <summary>
        /// GreaterThan &gt;
        /// </summary>
        CharGT,

        /// <summary>
        /// LessThan &lt;
        /// </summary>
        CharLT,

        /// <summary>
        /// Double quote "
        /// </summary>
        CharDQ,

        /// <summary>
        /// Forward slash /
        /// </summary>
        CharSL,
    }
}