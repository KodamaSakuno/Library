using System;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Sakuno
{
    public sealed class PropertyChangedEventListener : EventListener<PropertyChangedEventHandler>
    {
        static ConcurrentDictionary<INotifyPropertyChanged, PropertyChangedEventListener> r_Listeners = new ConcurrentDictionary<INotifyPropertyChanged, PropertyChangedEventListener>();

        WeakReference r_Source;
        ConcurrentDictionary<string, ConcurrentBag<PropertyChangedEventHandler>> r_HandlerDictionary = new ConcurrentDictionary<string, ConcurrentBag<PropertyChangedEventHandler>>();

        public PropertyChangedEventListener(INotifyPropertyChanged rpSource)
        {
            if (rpSource == null)
                throw new ArgumentNullException(nameof(rpSource));

            r_Source = new WeakReference(rpSource);

            Initialize(r => rpSource.PropertyChanged += r, r => rpSource.PropertyChanged -= r, (_, e) => RaiseHandler(e));
        }

        public static PropertyChangedEventListener FromSource(INotifyPropertyChanged rpSource)
        {
            return r_Listeners.GetOrAdd(rpSource, r => new PropertyChangedEventListener(r));
        }

        public void Add(string rpPropertyName, PropertyChangedEventHandler rpHandler)
        {
            r_HandlerDictionary.GetOrAdd(rpPropertyName, _ => new ConcurrentBag<PropertyChangedEventHandler>()).Add(rpHandler);
        }

        void RaiseHandler(PropertyChangedEventArgs e)
        {
            var rSource = r_Source.Target as INotifyPropertyChanged;
            if (rSource == null)
                return;

            ConcurrentBag<PropertyChangedEventHandler> rHandlerList;
            if (!r_HandlerDictionary.TryGetValue(e.PropertyName, out rHandlerList))
                return;

            foreach (var rHandler in rHandlerList)
                rHandler(rSource, e);
        }

        protected override void DisposeManagedResources()
        {
            var rSource = r_Source.Target as INotifyPropertyChanged;
            if (rSource != null)
            {
                PropertyChangedEventListener rListener;
                r_Listeners.TryRemove(rSource, out rListener);
            }

            base.DisposeManagedResources();
        }
    }
}
