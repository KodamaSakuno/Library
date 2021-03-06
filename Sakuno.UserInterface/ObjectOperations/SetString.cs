﻿using Sakuno.Reflection;
using System;
using System.Windows;

namespace Sakuno.UserInterface.ObjectOperations
{
    public class SetString : ObjectOperation
    {
        public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register(nameof(Property), typeof(string), typeof(SetBoolean),
            new PropertyMetadata(null));
        public string Property
        {
            get { return (string)GetValue(PropertyProperty); }
            set { SetValue(PropertyProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(string), typeof(SetBoolean),
            new PropertyMetadata(null));
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
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

            r_PropertyAccessor.SetValue(rTarget, GetValue(ValueProperty).ToString());
        }

        protected override Freezable CreateInstanceCore() => new SetString();
    }
}
