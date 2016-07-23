using System;

namespace Sakuno.Parsers
{
    public interface IResult<TInput, out T> where TInput : IInput
    {
        bool Success { get; }

        T Value { get; }
        TInput Rest { get; }

        string Expectation { get; }
    }
    public struct Result<TInput, T> : IResult<TInput, T> where TInput : IInput
    {
        public bool Success { get; }

        T r_Value;
        public T Value
        {
            get
            {
                if (!Success)
                    throw new InvalidOperationException("Value is not available.");

                return r_Value;
            }
        }

        public TInput Rest { get; }

        public string Expectation { get; }

        public Result(T rpValue, TInput rpRest)
        {
            Success = true;

            r_Value = rpValue;
            Rest = rpRest;

            Expectation = null;
        }

        public Result(TInput rpRest, string rpExpectation)
        {
            Success = false;

            r_Value = default(T);
            Rest = rpRest;

            Expectation = rpExpectation;
        }

        public override string ToString()
        {
            if (Success)
                return "Success: " + Value;

            return $"Failure: Expected {Expectation}";
        }
    }
}
