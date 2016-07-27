using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Sakuno.Reflection
{
    public class PropertyAccessor
    {
        public PropertyInfo Property { get; }

        Func<object, object> r_Getter;
        Action<object, object> r_Setter;

        public PropertyAccessor(PropertyInfo rpProperty)
        {
            if (rpProperty == null)
                throw new ArgumentNullException(nameof(rpProperty));

            Property = rpProperty;

            r_Getter = CreateGetter(rpProperty);
            r_Setter = CreateSetter(rpProperty);
        }

        public object GetValue(object rpInstance)
        {
            if (r_Getter == null)
                throw new NotSupportedException("Getter is not defined.");

            return r_Getter(rpInstance);
        }
        public void SetValue(object rpInstance, object rpValue)
        {
            if (r_Setter == null)
                throw new NotSupportedException("Setter is not defined.");

            r_Setter(rpInstance, rpValue);
        }

        static Func<object, object> CreateGetter(PropertyInfo rpProperty)
        {
            if (!rpProperty.CanRead)
                return null;

            var rInstanceParameter = Expression.Parameter(typeof(object), "rpInstance");

            var rCastInstance = !rpProperty.GetMethod.IsStatic ? Expression.Convert(rInstanceParameter, rpProperty.ReflectedType) : null;

            var rResult = Expression.Property(rCastInstance, rpProperty);
            var rCastResult = Expression.Convert(rResult, typeof(object));

            return Expression.Lambda<Func<object, object>>(rCastResult, rInstanceParameter).Compile();
        }
        static Action<object, object> CreateSetter(PropertyInfo rpProperty)
        {
            if (!rpProperty.CanWrite)
                return null;

            var rInstanceParameter = Expression.Parameter(typeof(object), "rpInstance");
            var rValueParameter = Expression.Parameter(typeof(object), "rpValue");

            var rCastInstance = !rpProperty.GetMethod.IsStatic ? Expression.Convert(rInstanceParameter, rpProperty.ReflectedType) : null;
            var rCastValue = Expression.Convert(rValueParameter, rpProperty.PropertyType);

            var rProperty = Expression.Property(rCastInstance, rpProperty);
            var rAssign = Expression.Assign(rProperty, rCastValue);

            return Expression.Lambda<Action<object, object>>(rAssign, rInstanceParameter, rValueParameter).Compile();
        }
    }
}
