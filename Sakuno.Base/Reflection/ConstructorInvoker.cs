using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Sakuno.Reflection
{
    public class ConstructorInvoker
    {
        public ConstructorInfo Constructor { get; }

        Func<object[], object> r_Invoker;

        public ConstructorInvoker(ConstructorInfo rpConstructor)
        {
            if (rpConstructor == null)
                throw new ArgumentNullException(nameof(rpConstructor));

            Constructor = rpConstructor;

            r_Invoker = CreateInvokerCore(rpConstructor);
        }

        public object Invoke(params object[] rpParameters) => r_Invoker(rpParameters);

        static Func<object[], object> CreateInvokerCore(ConstructorInfo rpConstructor)
        {
            var rParametersParameter = Expression.Parameter(typeof(object[]), "rpParameters");

            var rParameterExpressions = rpConstructor.GetParameters().Select((r, i) =>
            {
                var rParameterValue = Expression.ArrayIndex(rParametersParameter, Expression.Constant(i));

                return Expression.Convert(rParameterValue, r.ParameterType);
            }).ToArray();

            var rResult = Expression.New(rpConstructor, rParameterExpressions);
            var rCastResult = Expression.Convert(rResult, typeof(object));

            return Expression.Lambda<Func<object[], object>>(rCastResult, rParametersParameter).Compile();
        }
    }
}
