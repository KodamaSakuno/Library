using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Sakuno.Reflection
{
    public class FieldAccessor
    {
        public FieldInfo Field { get; }

        Func<object, object> r_Getter;
        Action<object, object> r_Setter;

        public FieldAccessor(FieldInfo rpField)
        {
            if (rpField == null)
                throw new ArgumentNullException(nameof(rpField));

            Field = rpField;

            r_Getter = CreateGetter(rpField);
            r_Setter = CreateSetter(rpField);
        }

        public object GetValue(object rpInstance) => r_Getter(rpInstance);
        public void SetValue(object rpInstance, object rpValue) => r_Setter(rpInstance, rpValue);

        static Func<object, object> CreateGetter(FieldInfo rpField)
        {
            var rInstanceParameter = Expression.Parameter(typeof(object), "rpInstance");

            var rCastInstance = !rpField.IsStatic ? Expression.Convert(rInstanceParameter, rpField.ReflectedType) : null;

            var rResult = Expression.Field(rCastInstance, rpField);
            var rCastResult = Expression.Convert(rResult, typeof(object));

            return Expression.Lambda<Func<object, object>>(rCastResult, rInstanceParameter).Compile();
        }
        static Action<object, object> CreateSetter(FieldInfo rpField)
        {
            var rInstanceParameter = Expression.Parameter(typeof(object), "rpInstance");
            var rValueParameter = Expression.Parameter(typeof(object), "rpValue");

            var rCastInstance = !rpField.IsStatic ? Expression.Convert(rInstanceParameter, rpField.ReflectedType) : null;
            var rCastValue = Expression.Convert(rValueParameter, rpField.FieldType);

            var rField = Expression.Field(rCastInstance, rpField);
            var rAssign = Expression.Assign(rField, rCastValue);

            return Expression.Lambda<Action<object, object>>(rAssign, rInstanceParameter, rValueParameter).Compile();
        }
    }
}
