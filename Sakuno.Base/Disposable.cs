using System;
using System.Threading;

namespace Sakuno
{
    public static class Disposable
    {
        public static IDisposable Create(Action rpAction) => new AnonymousDisposable(rpAction);

        sealed class AnonymousDisposable : IDisposable
        {
            volatile Action r_Action;

            public AnonymousDisposable(Action rpAction)
            {
                r_Action = rpAction;
            }

            public void Dispose()
            {
                var rAction = Interlocked.Exchange(ref r_Action, null);
                if (rAction != null)
                    rAction();
            }
        }
    }
}
