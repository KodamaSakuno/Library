namespace Sakuno.Parsers
{
    public interface IInput
    {
        int Position { get; }

        bool EndOfInput { get; }
    }
    public interface IInput<out T> : IInput
    {
        T Current { get; }

        IInput<T> MoveNext();
    }
}
