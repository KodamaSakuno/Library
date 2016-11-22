using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sakuno.Reflection
{
    public static class ReflectionExtensions
    {
        public static object FastInvoke(this ConstructorInfo rpConstruction, params object[] rpParameters) =>
            ReflectionCache.GetConstructorInvoker(rpConstruction).Invoke(rpParameters);
        public static object FastInvoke(this MethodInfo rpMethod, object rpInstance, params object[] rpParameters) =>
            ReflectionCache.GetMethodInvoker(rpMethod).Invoke(rpInstance, rpParameters);

        public static object FastGetValue(this FieldInfo rpField, object rpInstance) =>
            ReflectionCache.GetFieldAccessor(rpField).GetValue(rpInstance);
        public static void FastGetValue(this FieldInfo rpField, object rpInstance, object rpValue) =>
            ReflectionCache.GetFieldAccessor(rpField).SetValue(rpInstance, rpValue);

        public static object FastGetValue(this PropertyInfo rpProperty, object rpInstance) =>
            ReflectionCache.GetPropertyAccessor(rpProperty).GetValue(rpInstance);
        public static void FastSetValue(this PropertyInfo rpProperty, object rpInstance, object rpValue) =>
            ReflectionCache.GetPropertyAccessor(rpProperty).SetValue(rpInstance, rpValue);

        public static IEnumerable<T> FastGetCustomAttributes<T>(this Type rpType) where T : Attribute
        {
            return (IEnumerable<T>)ReflectionCache.CustomAttributes.GetOrAdd(rpType, r => new Lazy<IEnumerable<Attribute>>(() =>
            {
                if (!r.IsDefined(typeof(T)))
                    return ArrayUtil.Empty<T>();

                return CustomAttributeData.GetCustomAttributes(r).Where(rpData => rpData.Constructor.DeclaringType == typeof(T)).Select(rpData =>
                {
                    var rResult = rpData.Constructor.FastInvoke(rpData.ConstructorArguments.Select(rpArgument => rpArgument.Value).ToArray());

                    foreach (var rNamedArgument in rpData.NamedArguments)
                    {
                        var rProperty = (PropertyInfo)rNamedArgument.MemberInfo;

                        rProperty.FastSetValue(rResult, rNamedArgument.TypedValue.Value);
                    }

                    return (T)rResult;
                }).ToArray();
            })).Value;
        }
        public static T FastGetCustomAttribute<T>(this Type rpType) where T : Attribute
        {
            var rAttributes = (T[])rpType.FastGetCustomAttributes<T>();
            if (rAttributes.Length == 0)
                return null;

            return rAttributes[0];
        }
    }
}
