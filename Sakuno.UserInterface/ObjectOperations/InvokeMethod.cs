using Sakuno.Reflection;
using System;
using System.Windows;

namespace Sakuno.UserInterface.ObjectOperations
{
    public class InvokeMethod : ObjectOperation
    {
        public static readonly DependencyProperty MethodProperty = DependencyProperty.Register(nameof(Method), typeof(string), typeof(InvokeMethod),
            new PropertyMetadata(null));
        public string Method
        {
            get { return (string)GetValue(MethodProperty); }
            set { SetValue(MethodProperty, value); }
        }

        Type r_TargetType;
        MethodInvoker r_MethodInvoker;

        public override void Invoke()
        {
            var rTarget = Target;
            if (rTarget == null || Method == null)
                return;

            var rTargetType = rTarget.GetType();
            if (r_TargetType != rTargetType)
            {
                var rMethodInfo = rTargetType.GetMethod(Method);
                if (rMethodInfo == null)
                    throw new InvalidOperationException();

                r_TargetType = rTargetType;
                r_MethodInvoker = ReflectionCache.GetMethodInvoker(rMethodInfo);
            }

            r_MethodInvoker.Invoke(rTarget);
        }

        protected override Freezable CreateInstanceCore() => new InvokeMethod();
    }
}
