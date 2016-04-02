using System.Windows;
using System.Windows.Controls.Primitives;

namespace Sakuno.UserInterface.Controls
{
    public abstract class AdvancedTabDragEventArgs<T> : RoutedEventArgs where T : RoutedEventArgs
    {
        public AdvancedTabItem Item { get; }
        public T DragEventArgs { get; }

        protected AdvancedTabDragEventArgs(RoutedEvent rpEvent, AdvancedTabItem rpItem, T rpThumbDragEventArgs) : base(rpEvent, rpItem)
        {
            Item = rpItem;
            DragEventArgs = rpThumbDragEventArgs;
        }
    }

    delegate void AdvancedTabDragStartedEventHandler(object sender, AdvancedTabDragStartedEventArgs e);
    public class AdvancedTabDragStartedEventArgs : AdvancedTabDragEventArgs<DragStartedEventArgs>
    {
        public bool Cancel { get; set; }

        public AdvancedTabDragStartedEventArgs(RoutedEvent rpEvent, AdvancedTabItem rpItem, DragStartedEventArgs rpThumbDragEventArgs) : base(rpEvent, rpItem, rpThumbDragEventArgs) { }
    }

    delegate void AdvancedTabDragDeltaEventHandler(object sender, AdvancedTabDragDeltaEventArgs e);
    public class AdvancedTabDragDeltaEventArgs : AdvancedTabDragEventArgs<DragDeltaEventArgs>
    {
        public bool Cancel { get; set; }

        public AdvancedTabDragDeltaEventArgs(RoutedEvent rpEvent, AdvancedTabItem rpItem, DragDeltaEventArgs rpThumbDragEventArgs) : base(rpEvent, rpItem, rpThumbDragEventArgs) { }
    }

    delegate void AdvancedTabDragCompletedEventHandler(object sender, AdvancedTabDragCompletedEventArgs e);
    public class AdvancedTabDragCompletedEventArgs : AdvancedTabDragEventArgs<DragCompletedEventArgs>
    {
        public bool Cancel { get; set; }

        public AdvancedTabDragCompletedEventArgs(RoutedEvent rpEvent, AdvancedTabItem rpItem, DragCompletedEventArgs rpThumbDragEventArgs) : base(rpEvent, rpItem, rpThumbDragEventArgs) { }
    }
}
