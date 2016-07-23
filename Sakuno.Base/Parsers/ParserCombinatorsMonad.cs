using System;

namespace Sakuno.Parsers
{
    public static class ParserCombinatorsMonad
    {
        public static Parser<TInput, T2> Select<TInput, T1, T2>(this Parser<TInput, T1> rpParser, Func<T1, T2> rpSelector) where TInput : IInput =>
            rpInput =>
            {
                var rResult = rpParser(rpInput);
                if (!rResult.Success)
                    return Result.Failure<TInput, T2>(rResult.Rest, rResult.Expectation);

                return Result.Success(rpSelector(rResult.Value), rResult.Rest);
            };

        public static Parser<TInput, T2> SelectMany<TInput, T1, TIntermediate, T2>(this Parser<TInput, T1> rpParser, Func<T1, Parser<TInput, TIntermediate>> rpSelector, Func<T1, TIntermediate, T2> rpProjector) where TInput : IInput =>
            rpInput =>
            {
                var rResult = rpParser(rpInput);
                if (!rResult.Success)
                    return Result.Failure<TInput, T2>(rResult.Rest, rResult.Expectation);

                var rValue = rResult.Value;

                var rResult2 = rpSelector(rValue)(rResult.Rest);
                if (!rResult2.Success)
                    return Result.Failure<TInput, T2>(rResult2.Rest, rResult2.Expectation);

                return Result.Success(rpProjector(rValue, rResult2.Value), rResult2.Rest);
            };

        public static Parser<TInput, T> Where<TInput, T>(this Parser<TInput, T> rpParser, Func<T, bool> rpPredicate) where TInput : IInput =>
            rpInput =>
            {
                var rResult = rpParser(rpInput);
                if (!rResult.Success || !rpPredicate(rResult.Value))
                    return Result.Failure<TInput, T>(rResult.Rest, rResult.Expectation);

                return Result.Success(rResult.Value, rResult.Rest);
            };
    }
}
