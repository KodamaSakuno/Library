using System;

namespace Sakuno.Parsers
{
    public static class ResultExtensions
    {
        public static IResult<TInput, T> OnSuccess<TInput, T>(this IResult<TInput, T> rpResult, Func<IResult<TInput, T>, IResult<TInput, T>> rpContinuation) where TInput : IInput =>
            rpResult.Success ? rpContinuation(rpResult) : Result.Failure<TInput, T>(rpResult.Rest, rpResult.Expectation);
        public static IResult<TInput, T> OnFailure<TInput, T>(this IResult<TInput, T> rpResult, Func<IResult<TInput, T>, IResult<TInput, T>> rpContinuation) where TInput : IInput =>
            rpResult.Success ? rpResult : rpContinuation(rpResult);
    }
}
