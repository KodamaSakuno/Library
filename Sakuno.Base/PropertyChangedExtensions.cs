using System;
using System.ComponentModel;

namespace Sakuno
{
    public static class PropertyChangedExtensions
    {
        public static IDisposable Subscribe(this INotifyPropertyChanged rpSource, string rpPropertyName, PropertyChangedEventHandler rpHandler)
        {
            var rResult = new PropertyChangedEventListener(rpSource);
            rResult.Add(rpPropertyName, rpHandler);

            return rResult;
        }
    }
}
