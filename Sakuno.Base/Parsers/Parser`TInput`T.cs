namespace Sakuno.Parsers
{
    public delegate IResult<TInput, T> Parser<TInput, out T>(TInput rpInput) where TInput : IInput;
}
