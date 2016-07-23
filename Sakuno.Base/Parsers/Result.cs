namespace Sakuno.Parsers
{
    public static class Result
    {
        public static Result<TInput, T> Success<TInput, T>(T rpValue, TInput rpRest) where TInput : IInput => new Result<TInput, T>(rpValue, rpRest);
        public static Result<TInput, T> Failure<TInput, T>(TInput rpRest, string rpExpectation) where TInput : IInput => new Result<TInput, T>(rpRest, rpExpectation);
    }
}
