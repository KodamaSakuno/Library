using System;
using System.ComponentModel;

namespace Sakuno
{
    public static class PropertyChangedExtensions
    {
        public static IDisposable Subscribe(this INotifyPropertyChanged rpSource, PropertyChangedEventHandler rpHandler) => new PropertyChangedEventListener(rpSource);
    }
}
