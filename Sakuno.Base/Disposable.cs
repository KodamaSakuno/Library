using System;
using System.Threading;

namespace Sakuno
{
    public static class Disposable
    {
        public static IDisposable Create(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return new AnonymousDisposable(action);
        }

        sealed class AnonymousDisposable : DisposableObject
        {
            Action r_Action;

            public AnonymousDisposable(Action rpAction)
            {
                r_Action = rpAction;
            }

            protected override void DisposeManagedResources() => Interlocked.Exchange(ref r_Action, null)?.Invoke();
        }
    }
}
