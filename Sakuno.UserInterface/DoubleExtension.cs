using System;
using System.Windows.Markup;

namespace Sakuno.UserInterface
{
    public sealed class DoubleExtension : MarkupExtension
    {
        public double Value { get; set; }

        public DoubleExtension(double rpValue)
        {
            Value = rpValue;
        }

        public override object ProvideValue(IServiceProvider rpServiceProvider)
        {
            if (DoubleUtil.IsZero(Value))
                return DoubleUtil.Zero;
            else if (DoubleUtil.IsOne(Value))
                return DoubleUtil.One;
            else if (Value.IsNaN())
                return DoubleUtil.NaN;

            return Value;
        }
    }
}
