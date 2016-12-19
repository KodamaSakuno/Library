using System;
using System.Windows;
using System.Windows.Interactivity;

namespace Sakuno.UserInterface.Interactivity
{
    public class DisposeDataContextAction : TriggerAction<FrameworkElement>
    {
        protected override void Invoke(object rpParameter) => (AssociatedObject.DataContext as IDisposable)?.Dispose();
    }
}
