using Sakuno.Reflection;
using System;
using System.Windows;

namespace Sakuno.UserInterface.ObjectOperations
{
    public class SetBoolean : ObjectOperation
    {
        public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register(nameof(Property), typeof(string), typeof(SetBoolean),
            new PropertyMetadata(null));
        public string Property
        {
            get { return (string)GetValue(PropertyProperty); }
            set { SetValue(PropertyProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(bool), typeof(SetBoolean),
            new PropertyMetadata(BooleanUtil.True));
        public bool Value
        {
            get { return (bool)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        Type r_TargetType;
        PropertyAccessor r_PropertyAccessor;

        public override void Invoke()
        {
            var rTarget = Target;
            if (rTarget == null || Property == null)
                return;

            var rTargetType = rTarget.GetType();
            if (r_TargetType != rTargetType)
            {
                var rPropertyInfo = rTargetType.GetProperty(Property);
                if (rPropertyInfo == null)
                    throw new InvalidOperationException();

                r_TargetType = rTargetType;
                r_PropertyAccessor = ReflectionCache.GetPropertyAccessor(rPropertyInfo);
            }

            r_PropertyAccessor.SetValue(rTarget, GetValue(ValueProperty));
        }

        protected override Freezable CreateInstanceCore() => new SetBoolean();
    }
}
