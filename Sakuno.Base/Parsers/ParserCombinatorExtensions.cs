using System.Collections.Generic;
using System.Linq;

namespace Sakuno.Parsers
{
    public static class ParserCombinatorExtensions
    {
        public static Parser<TInput, T> Or<TInput, T>(this Parser<TInput, T> x, Parser<TInput, T> y) where TInput : IInput =>
            rpInput =>
            {
                var rFirst = x(rpInput);
                if (!rFirst.Success)
                    return y(rpInput).OnFailure(r => CombineFailures(rFirst, r));

                if (rFirst.Rest.Position == rpInput.Position)
                    return y(rpInput).OnFailure(r => rFirst);

                return rFirst;
            };
        public static Parser<TInput, T> LeftIfPartiallyParsed<TInput, T>(this Parser<TInput, T> x, Parser<TInput, T> y) where TInput : IInput =>
            rpInput =>
            {
                var rFirst = x(rpInput);
                if (!rFirst.Success)
                {
                    if (rFirst.Rest.Position != rpInput.Position)
                        return rFirst;

                    return y(rpInput).OnFailure(r => CombineFailures(rFirst, r));
                }

                if (rFirst.Rest.Position == rpInput.Position)
                    return y(rpInput).OnFailure(r => rFirst);

                return rFirst;
            };

        public static IResult<TInput, T> CombineFailures<TInput, T>(IResult<TInput, T> rpFirst, IResult<TInput, T> rpSecond) where TInput : IInput
        {
            if (rpFirst.Rest.Position == rpSecond.Rest.Position)
                return Result.Failure<TInput, T>(rpFirst.Rest, $"{rpFirst.Expectation} or {rpSecond.Expectation}");

            return rpFirst;
        }

        public static Parser<TInput, T2> And<TInput, T1, T2>(this Parser<TInput, T1> x, Parser<TInput, T2> y) where TInput : IInput =>
            rpInput =>
            {
                var rResult = x(rpInput);
                if (!rResult.Success)
                    return Result.Failure<TInput, T2>(rResult.Rest, rResult.Expectation);

                return y(rResult.Rest);
            };

        public static Parser<TInput, T> EndOfInput<TInput, T>(this Parser<TInput, T> rpParser) where TInput : IInput =>
            rpInput => rpParser(rpInput).OnSuccess(rpResult => rpResult.Rest.EndOfInput ? rpResult : Result.Failure<TInput, T>(rpResult.Rest, "end of input"));

        public static Parser<TInput, T> Not<TInput, T>(this Parser<TInput, T> rpParser) where TInput : IInput =>
            rpInput =>
            {
                var rResult = rpParser(rpInput);
                if (rResult.Success)
                    return Result.Failure<TInput, T>(rResult.Rest, rResult.Expectation);

                return Result.Success(default(T), rpInput);
            };

        public static Parser<TInput, T> Optional<TInput, T>(this Parser<TInput, T> rpParser) where TInput : IInput where T : class =>
            rpInput =>
            {
                var rResult = rpParser(rpInput);

                return rResult.Success ? Result.Success(rResult.Value, rResult.Rest) : Result.Success<TInput, T>(null, rpInput);
            };
        public static Parser<TInput, T?> OptionalOfValueType<TInput, T>(this Parser<TInput, T> rpParser) where TInput : IInput where T : struct =>
            rpInput =>
            {
                var rResult = rpParser(rpInput);

                return rResult.Success ? Result.Success<TInput, T?>(rResult.Value, rResult.Rest) : Result.Success<TInput, T?>(null, rpInput);
            };

        public static Parser<TInput, IEnumerable<T>> Once<TInput, T>(this Parser<TInput, T> rpParser) where TInput : IInput =>
            from rFirst in rpParser
            select new[] { rFirst };

        public static Parser<TInput, IEnumerable<T>> AtLeastOnce<TInput, T>(this Parser<TInput, T> rpParser) where TInput : IInput =>
            from rFirst in rpParser.Once()
            from rRest in rpParser.Many()
            select rFirst.Concat(rRest);

        public static Parser<TInput, IEnumerable<T>> Many<TInput, T>(this Parser<TInput, T> rpParser) where TInput : IInput =>
            rpInput =>
            {
                var rRest = rpInput;
                var rValues = new List<T>();

                if (rpInput.EndOfInput)
                    return Result.Failure<TInput, IEnumerable<T>>(rRest, "end of input");

                var rResult = rpParser(rpInput);
                while (rResult.Success)
                {
                    rValues.Add(rResult.Value);

                    rRest = rResult.Rest;
                    rResult = rpParser(rRest);
                }

                return Result.Success<TInput, IEnumerable<T>>(rValues, rRest);
            };

        public static Parser<TInput, IEnumerable<T>> ManyIfPartiallyParsed<TInput, T>(this Parser<TInput, T> rpParser) where TInput : IInput =>
            from rItems in rpParser.Many()
            from rResult in rpParser.Once().LeftIfPartiallyParsed(Parsers.Return<TInput, IEnumerable<T>>(rItems))
            select rResult;

        public static Parser<TInput, T1> Except<TInput, T1, T2>(this Parser<TInput, T1> rpParser, Parser<TInput, T2> rpExcept) where TInput : IInput =>
            rpInput =>
            {
                var rResult = rpExcept(rpInput);
                if (rResult.Success)
                    return Result.Failure<TInput, T1>(rpInput, "except");

                return rpParser(rpInput);
            };
        public static Parser<TInput, IEnumerable<T1>> Until<TInput, T1, T2>(this Parser<TInput, T1> rpParser, Parser<TInput, T2> rpUntil) where TInput : IInput =>
            rpInput =>
            {
                var rResult = rpParser.Except(rpUntil).Many()(rpInput);
                if (rResult.Success)
                {
                    var rResult2 = rpUntil.LeftIfPartiallyParsed(Parsers.EndOfInput<TInput, T2>()).Select(_ => rResult)(rResult.Rest);
                    if (!rResult2.Success)
                        return Result.Failure<TInput, IEnumerable<T1>>(rpInput, "until");

                    return rResult;
                }

                return Result.Failure<TInput, IEnumerable<T1>>(rpInput, "first");
            };

        public static Parser<TInput, IEnumerable<T1>> DelimitedBy<TInput, T1, T2>(this Parser<TInput, T1> rpParser, Parser<TInput, T2> rpDelimiter) where TInput : IInput =>
            from rFirst in rpParser.Once()
            from rRest in (
                from rSeparator in rpDelimiter
                from rValue in rpParser
                select rValue).Many().LeftIfPartiallyParsed(Parsers.EndOfInput<TInput, IEnumerable<T1>>())
            select rRest == null ? rFirst : rFirst.Concat(rRest);

        public static Parser<TInput, IEnumerable<T1>> StrictDelimitedBy<TInput, T1, T2>(this Parser<TInput, T1> rpParser, Parser<TInput, T2> rpDelimiter) where TInput : IInput =>
            from rFirst in rpParser.Once()
            from rRest in (
                from rSeparator in rpDelimiter
                from rValue in rpParser
                select rValue).Many()
            select rFirst.Concat(rRest);

        public static Parser<TInput, IEnumerable<T>> Concat<TInput, T>(this Parser<TInput, IEnumerable<T>> x, Parser<TInput, IEnumerable<T>> y) where TInput : IInput =>
            from rFirst in x
            from rSecond in y
            select rFirst.Concat(rSecond);

        public static Parser<TInput, T> AddDescription<TInput, T>(this Parser<TInput, T> rpParser, string rpDescription) where TInput : IInput =>
            rpInput => rpParser(rpInput).OnFailure(rpFailure => Result.Failure<TInput, T>(rpFailure.Rest, rpDescription));
    }
}
