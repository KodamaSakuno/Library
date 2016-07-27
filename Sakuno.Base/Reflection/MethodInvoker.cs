using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Sakuno.Reflection
{
    public class MethodInvoker
    {
        public MethodInfo Method { get; }

        Func<object, object[], object> r_Invoker;

        public MethodInvoker(MethodInfo rpMethod)
        {
            if (rpMethod == null)
                throw new ArgumentNullException(nameof(rpMethod));

            Method = rpMethod;

            r_Invoker = CreateInvokerCore(rpMethod);
        }

        public object Invoke(object rpInstance, params object[] rpParameters) => r_Invoker(rpInstance, rpParameters);

        static Func<object, object[], object> CreateInvokerCore(MethodInfo rpMethod)
        {
            var rInstanceParameter = Expression.Parameter(typeof(object), "rpInstance");
            var rParametersParameter = Expression.Parameter(typeof(object[]), "rpParameters");

            var rParameterExpressions = rpMethod.GetParameters().Select((r, i) =>
            {
                var rParameterValue = Expression.ArrayIndex(rParametersParameter, Expression.Constant(i));

                return Expression.Convert(rParameterValue, r.ParameterType);
            }).ToArray();

            var rCastInstance = !rpMethod.IsStatic ? Expression.Convert(rInstanceParameter, rpMethod.ReflectedType) : null;

            var rCall = Expression.Call(rCastInstance, rpMethod, rParameterExpressions);

            if (rCall.Type == typeof(void))
            {
                var rFunc = Expression.Lambda<Action<object, object[]>>(rCall, rInstanceParameter, rParametersParameter).Compile();

                return (rpInstance, rpParameters) =>
                {
                    rFunc(rpInstance, rpParameters);

                    return null;
                };
            }
            else
            {
                var rCastReturnValue = Expression.Convert(rCall, typeof(object));

                return Expression.Lambda<Func<object, object[], object>>(rCastReturnValue, rInstanceParameter, rParametersParameter).Compile();
            }
        }
    }
}
