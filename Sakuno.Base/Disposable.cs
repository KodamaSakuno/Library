using System;
using System.Threading;

namespace Sakuno
{
    public static class Disposable
    {
        public static IDisposable Create(Action rpAction)
        {
            if (rpAction == null)
                throw new ArgumentNullException(nameof(rpAction));

            return new AnonymousDisposable(rpAction);
        }

        sealed class AnonymousDisposable : IDisposable
        {
            Action r_Action;

            public AnonymousDisposable(Action rpAction)
            {
                r_Action = rpAction;
            }

            public void Dispose() => Interlocked.Exchange(ref r_Action, null)?.Invoke();
        }
    }
}
