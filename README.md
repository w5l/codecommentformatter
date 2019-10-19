
# Code Comment Formatting

An attempt to replace my old regex based comment formatting found in [CodeMaid](https://github.com/codecadwallader/codemaid). 

## Aims
The original regular expression based solution works is very inflexible and does not have a good responsibility separation. It's getting increasingly difficult to add new requested features and fix reported bugs. It uses the system XML parsing under the hood, which is very inflexible with broken XML. 

Last but not least, rebuilding from scratch is always more fun.

This version attempts to bring the following changes:

- Use the [Roslyn ](https://github.com/dotnet/roslyn) compiler to locate comments.
- Parse comment text into a tree representation rather than pattern scanning with regular expressions.
- Allow more configuration of output style.
- Use the [`System.Span<T>` ](https://docs.microsoft.com/en-us/dotnet/api/system.span-1) struct to optimize performance (is this even compatible with a Visual Studio plugin?).
- Make a separate CLI version than can be run outside of Visual Studio.

## Methods

The project is still in a very early development stage. Everything is likely to change around many times before getting anywhere near production ready.

### Locating comments in code
Not thought out yet. Should use Roslyn, it already knows which parts of a file are comments.

### Reading comment text
1. The `TokenReader` turns the input string is turned into a stream of `Token`s. Tokens denote special characters and words.
2. The `Lexer` turns the stream of `Token`s into building blocks. So far, it recognizes `Xml` and `Text`. It should be able to deal gracefully with malformed XML tags.
3. The `Parser` turns the building blocks from the `Lexer` into a tree representation of the comment.

At this point, we have a tree representation of the comment without any loss of information, i.e. the original comment can be 100% reconstructed from the tree.

### Formatting the comment
Not thought out yet.

### Writing back formatted comment
Not thought out yet.

## Conclusions

There's work to do! Maybe this turns into something, maybe it doesn't. Feel free to comment, clone, copy, or all of the above.
