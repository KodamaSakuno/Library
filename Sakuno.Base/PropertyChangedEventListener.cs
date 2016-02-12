using System;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Sakuno
{
    public sealed class PropertyChangedEventListener : EventListener<PropertyChangedEventHandler>
    {
        static ConcurrentDictionary<INotifyPropertyChanged, PropertyChangedEventListener> r_Listeners = new ConcurrentDictionary<INotifyPropertyChanged, PropertyChangedEventListener>();

        WeakReference<INotifyPropertyChanged> r_Source;
        ConcurrentDictionary<string, ConcurrentBag<PropertyChangedEventHandler>> r_HandlerDictionary = new ConcurrentDictionary<string, ConcurrentBag<PropertyChangedEventHandler>>();

        public PropertyChangedEventListener(INotifyPropertyChanged rpSource)
        {
            if (rpSource == null)
                throw new ArgumentNullException(nameof(rpSource));

            r_Source = new WeakReference<INotifyPropertyChanged>(rpSource);

            Initialize(r => rpSource.PropertyChanged += r, r => rpSource.PropertyChanged -= r, (_, e) => { RaiseHandler(e); });
        }

        public static PropertyChangedEventListener FromSource(INotifyPropertyChanged rpSource) => r_Listeners.GetOrAdd(rpSource, r => new PropertyChangedEventListener(r));

        public void Add(string rpPropertyName, PropertyChangedEventHandler rpHandler)
        {
            var rHandlerList = r_HandlerDictionary.GetOrAdd(rpPropertyName, _ => new ConcurrentBag<PropertyChangedEventHandler>());
            rHandlerList.Add(rpHandler);
        }

        void RaiseHandler(PropertyChangedEventArgs e)
        {
            INotifyPropertyChanged rSource;
            if (!r_Source.TryGetTarget(out rSource))
                return;

            ConcurrentBag<PropertyChangedEventHandler> rHandlerList;
            if (!r_HandlerDictionary.TryGetValue(e.PropertyName, out rHandlerList))
                return;

            foreach (var rHandler in rHandlerList)
                rHandler(rSource, e);
        }

        protected override void DisposeManagedResources()
        {
            INotifyPropertyChanged rSource;
            if (r_Source.TryGetTarget(out rSource))
            {
                PropertyChangedEventListener rListener;
                r_Listeners.TryRemove(rSource, out rListener);
            }

            base.DisposeManagedResources();
        }
    }
}
