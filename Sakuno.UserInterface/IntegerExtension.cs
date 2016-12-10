using System;
using System.Windows.Markup;

namespace Sakuno.UserInterface
{
    public sealed class IntegerExtension : MarkupExtension
    {
        public int Value { get; set; }

        public IntegerExtension(int rpValue)
        {
            Value = rpValue;
        }

        public override object ProvideValue(IServiceProvider rpServiceProvider)
        {
            switch (Value)
            {
                case 0: return Int32Util.Zero;

                default: return Value;
            }
        }
    }
}
