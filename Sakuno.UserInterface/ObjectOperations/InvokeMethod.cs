using Sakuno.Reflection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace Sakuno.UserInterface.ObjectOperations
{
    [ContentProperty(nameof(Parameters))]
    public class InvokeMethod : ObjectOperation
    {
        public static readonly DependencyProperty MethodProperty = DependencyProperty.Register(nameof(Method), typeof(string), typeof(InvokeMethod),
            new PropertyMetadata(null));
        public string Method
        {
            get { return (string)GetValue(MethodProperty); }
            set { SetValue(MethodProperty, value); }
        }

        public static readonly DependencyProperty ParametersProperty = DependencyProperty.Register(nameof(Parameters), typeof(MethodParameterCollection), typeof(InvokeMethod),
            new PropertyMetadata(null));
        public MethodParameterCollection Parameters
        {
            get
            {
                var rResult = (MethodParameterCollection)GetValue(ParametersProperty);
                if (rResult == null)
                {
                    rResult = new MethodParameterCollection();
                    Parameters = rResult;
                }

                return rResult;
            }
            set { SetValue(ParametersProperty, value); }
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

            var rParameters = (GetValue(ParametersProperty) as MethodParameterCollection)?.Select(r => r.Value).ToArray();

            r_MethodInvoker.Invoke(rTarget, rParameters);
        }

        protected override Freezable CreateInstanceCore() => new InvokeMethod();
    }
}
