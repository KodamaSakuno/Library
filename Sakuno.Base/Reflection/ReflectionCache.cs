using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Sakuno.Reflection
{
    public static class ReflectionCache
    {
        static ConcurrentDictionary<ConstructorInfo, ConstructorInvoker> r_ConstructorInvokers = new ConcurrentDictionary<ConstructorInfo, ConstructorInvoker>();
        static ConcurrentDictionary<MethodInfo, MethodInvoker> r_MethodInvokers = new ConcurrentDictionary<MethodInfo, MethodInvoker>();

        static ConcurrentDictionary<FieldInfo, FieldAccessor> r_FieldAccessors = new ConcurrentDictionary<FieldInfo, FieldAccessor>();
        static ConcurrentDictionary<PropertyInfo, PropertyAccessor> r_PropertyAccessors = new ConcurrentDictionary<PropertyInfo, PropertyAccessor>();

        internal static ConcurrentDictionary<Type, IEnumerable<Attribute>> CustomAttributes { get; } = new ConcurrentDictionary<Type, IEnumerable<Attribute>>();

        public static ConstructorInvoker GetConstructorInvoker(ConstructorInfo rpConstructor) => r_ConstructorInvokers.GetOrAdd(rpConstructor, r => new ConstructorInvoker(r));
        public static MethodInvoker GetMethodInvoker(MethodInfo rpMethod) => r_MethodInvokers.GetOrAdd(rpMethod, r => new MethodInvoker(r));

        public static FieldAccessor GetFieldAccessor(FieldInfo rpField) => r_FieldAccessors.GetOrAdd(rpField, r => new FieldAccessor(r));
        public static PropertyAccessor GetPropertyAccessor(PropertyInfo rpProperty) => r_PropertyAccessors.GetOrAdd(rpProperty, r => new PropertyAccessor(r));
    }
}
