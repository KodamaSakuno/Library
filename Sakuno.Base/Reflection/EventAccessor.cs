using System;
using System.Reflection;

namespace Sakuno.Reflection
{
    public class EventAccessor
    {
        public EventInfo Event { get; }

        MethodInvoker r_Adder, r_Remover;

        public EventAccessor(EventInfo rpEvent)
        {
            if (rpEvent == null)
                throw new ArgumentNullException(nameof(rpEvent));

            Event = rpEvent;

            r_Adder = ReflectionCache.GetMethodInvoker(rpEvent.AddMethod);
            r_Remover = ReflectionCache.GetMethodInvoker(rpEvent.RemoveMethod);
        }

        public void AddHandler(object rpInstance, Delegate rpEventHandler) => r_Adder.Invoke(rpInstance, rpEventHandler);
        public void RemoveHandler(object rpInstance, Delegate rpEventHandler) => r_Remover.Invoke(rpInstance, rpEventHandler);
    }
}
