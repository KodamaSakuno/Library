using System.Windows;
using System.Windows.Data;

namespace Sakuno.UserInterface.ObjectOperations
{
    public abstract class ObjectOperation : Freezable
    {
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(nameof(Target), typeof(object), typeof(ObjectOperation),
            new PropertyMetadata(null));
        public object Target
        {
            get { return GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        public ObjectOperation()
        {
            BindingOperations.SetBinding(this, TargetProperty, new Binding());
        }

        public abstract void Invoke();
    }
}
