namespace Sakuno.Parsers
{
    public static class Parsers
    {
        public static Parser<TInput, T> Return<TInput, T>(T rpValue) where TInput : IInput => rpInput => Result.Success(rpValue, rpInput);

        public static Parser<TInput, T> EndOfInput<TInput, T>() where TInput : IInput =>
            rpInput =>
            {
                if (rpInput.EndOfInput)
                    return Result.Success<TInput, T>(default(T), rpInput);

                return Result.Failure<TInput, T>(rpInput, "end of input");
            };
    }
}
