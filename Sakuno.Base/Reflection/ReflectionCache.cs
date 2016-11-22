using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Sakuno.Reflection
{
    public static class ReflectionCache
    {
        static ConcurrentDictionary<ConstructorInfo, Lazy<ConstructorInvoker>> r_ConstructorInvokers = new ConcurrentDictionary<ConstructorInfo, Lazy<ConstructorInvoker>>();
        static ConcurrentDictionary<MethodInfo, Lazy<MethodInvoker>> r_MethodInvokers = new ConcurrentDictionary<MethodInfo, Lazy<MethodInvoker>>();

        static ConcurrentDictionary<FieldInfo, Lazy<FieldAccessor>> r_FieldAccessors = new ConcurrentDictionary<FieldInfo, Lazy<FieldAccessor>>();
        static ConcurrentDictionary<PropertyInfo, Lazy<PropertyAccessor>> r_PropertyAccessors = new ConcurrentDictionary<PropertyInfo, Lazy<PropertyAccessor>>();

        internal static ConcurrentDictionary<Type, Lazy<IEnumerable<Attribute>>> CustomAttributes { get; } = new ConcurrentDictionary<Type, Lazy<IEnumerable<Attribute>>>();

        public static ConstructorInvoker GetConstructorInvoker(ConstructorInfo rpConstructor) =>
            r_ConstructorInvokers.GetOrAdd(rpConstructor, r => new Lazy<ConstructorInvoker>(() => new ConstructorInvoker(r))).Value;
        public static MethodInvoker GetMethodInvoker(MethodInfo rpMethod) =>
            r_MethodInvokers.GetOrAdd(rpMethod, r => new Lazy<MethodInvoker>(() => new MethodInvoker(r))).Value;

        public static FieldAccessor GetFieldAccessor(FieldInfo rpField) =>
            r_FieldAccessors.GetOrAdd(rpField, r => new Lazy<FieldAccessor>(() => new FieldAccessor(r))).Value;
        public static PropertyAccessor GetPropertyAccessor(PropertyInfo rpProperty) =>
            r_PropertyAccessors.GetOrAdd(rpProperty, r => new Lazy<PropertyAccessor>(() => new PropertyAccessor(r))).Value;
    }
}
