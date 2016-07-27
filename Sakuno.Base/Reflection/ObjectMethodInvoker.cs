using System;

namespace Sakuno.Reflection
{
    public class ObjectMethodInvoker
    {
        Type r_InstanceType;
        string r_MethodName;

        MethodInvoker r_Invoker;

        public void Invoke(object rpInstance, string rpMethodName)
        {
            var rInstanceType = rpInstance.GetType();

            if (r_InstanceType == rInstanceType && r_MethodName == rpMethodName)
            {
                r_Invoker.Invoke(rpInstance);
                return;
            }

            r_InstanceType = rInstanceType;
            r_MethodName = rpMethodName;

            var rMethod = r_InstanceType.GetMethod(r_MethodName);

            r_Invoker = ReflectionCache.GetMethodInvoker(rMethod);
            r_Invoker.Invoke(rpInstance);
        }
    }
}
