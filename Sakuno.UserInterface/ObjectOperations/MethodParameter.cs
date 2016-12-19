using System.Windows;

namespace Sakuno.UserInterface.ObjectOperations
{
    public class MethodParameter : Freezable
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(object), typeof(MethodParameter),
            new PropertyMetadata(null));
        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        protected override Freezable CreateInstanceCore() => new MethodParameter();
    }
}
