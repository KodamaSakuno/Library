using System;
using System.Collections.Generic;
using System.Linq;

namespace Sakuno.Parsers
{
    public static class StringParsers
    {
        public static Parser<IInput<char>, char> AnyChar { get; } = Char(_ => true, "any char");
        public static Parser<IInput<char>, char> WhiteSpace { get; } = Char(char.IsWhiteSpace, "whitespace");
        public static Parser<IInput<char>, char> Letter { get; } = Char(char.IsLetter, "letter");
        public static Parser<IInput<char>, char> Digit { get; } = Char(char.IsDigit, "digit");
        public static Parser<IInput<char>, char> LetterOrDigit { get; } = Char(char.IsLetterOrDigit, "letter or digit");
        public static Parser<IInput<char>, char> Numeric { get; } = Char(char.IsNumber, "numeric character");
        public static Parser<IInput<char>, char> Lower { get; } = Char(char.IsLower, "lowercase letter");
        public static Parser<IInput<char>, char> Upper { get; } = Char(char.IsUpper, "uppercase letter");

        public static Parser<IInput<char>, char> Char(Predicate<char> rpPredicate, string rpDescription) =>
            rpInput =>
            {
                if (rpInput.EndOfInput)
                    return Result.Failure<IInput<char>, char>(rpInput, rpDescription);

                if (rpPredicate(rpInput.Current))
                    return Result.Success(rpInput.Current, rpInput.MoveNext());

                return Result.Failure<IInput<char>, char>(rpInput, rpDescription);
            };

        public static Parser<IInput<char>, char> Char(char rpChar) => Char(r => rpChar == r, char.ToString(rpChar));

        public static Parser<IInput<char>, char> Chars(char[] rpChars) => Char(rpChars.Contains, $"[{string.Join(string.Empty, rpChars)}]");
        public static Parser<IInput<char>, char> Chars(string rpChars) => Char(rpChars.ToCharArray().Contains, $"[{rpChars}]");

        public static Parser<IInput<char>, char> CharExcept(Predicate<char> rpPredicate, string rpDescription) => Char(r => !rpPredicate(r), rpDescription);
        public static Parser<IInput<char>, char> CharExcept(char rpChar) => CharExcept(r => rpChar == r, char.ToString(rpChar));
        public static Parser<IInput<char>, char> CharsExcept(char[] rpChars) => CharExcept(rpChars.Contains, $"^[{string.Join(string.Empty, rpChars)}]");
        public static Parser<IInput<char>, char> CharsExcept(string rpChars) => CharExcept(rpChars.ToCharArray().Contains, $"^[{rpChars}]");

        public static Parser<IInput<char>, T> Token<T>(this Parser<IInput<char>, T> rpParser) =>
            from rLeading in WhiteSpace.Many()
            from rResult in rpParser
            from rTrailing in WhiteSpace.Many().LeftIfPartiallyParsed(Parsers.EndOfInput<IInput<char>, IEnumerable<char>>())
            select rResult;

        public static Parser<IInput<char>, string> AsString(this Parser<IInput<char>, IEnumerable<char>> rpCharacters) => rpCharacters.Select(rpChars => new string(rpChars.ToArray()));
        public static Parser<IInput<char>, int> AsInt32(this Parser<IInput<char>, IEnumerable<char>> rpCharacters) => rpCharacters.AsString().Select(int.Parse);
        public static Parser<IInput<char>, long> AsInt64(this Parser<IInput<char>, IEnumerable<char>> rpCharacters) => rpCharacters.AsString().Select(long.Parse);

        public static Parser<IInput<char>, IEnumerable<char>> String(string rpString) => rpString.Select(r => Char(r).Once()).Aggregate(ParserCombinatorExtensions.Concat).AddDescription(rpString);

        public static IResult<IInput<char>, T> Parse<T>(this Parser<IInput<char>, T> rpParser, string rpInput) => rpParser(new StringInput(rpInput));
    }
}
