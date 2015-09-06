using System.Windows;
using System.Windows.Controls.Primitives;

namespace Sakuno.UserInterface.Controls
{
    public abstract class DragableTabDragEventArgs<T> : RoutedEventArgs
        where T : RoutedEventArgs
    {
        public DragableTabItem Item { get; }
        public T DragEventArgs { get; }

        internal protected DragableTabDragEventArgs(RoutedEvent rpEvent, DragableTabItem rpItem, T rpThumbDragEventArgs)
            : base(rpEvent, rpItem)
        {
            Item = rpItem;
            DragEventArgs = rpThumbDragEventArgs;
        }
    }

    delegate void DragableTabDragStartedEventHandler(object sender, DragableTabDragStartedEventArgs e);
    public class DragableTabDragStartedEventArgs : DragableTabDragEventArgs<DragStartedEventArgs>
    {
        public bool Cancel { get; set; }

        public DragableTabDragStartedEventArgs(RoutedEvent rpEvent, DragableTabItem rpItem, DragStartedEventArgs rpThumbDragEventArgs)
            : base(rpEvent, rpItem, rpThumbDragEventArgs) { }
    }

    delegate void DragableTabDragDeltaEventHandler(object sender, DragableTabDragDeltaEventArgs e);
    public class DragableTabDragDeltaEventArgs : DragableTabDragEventArgs<DragDeltaEventArgs>
    {
        public bool Cancel { get; set; }

        public DragableTabDragDeltaEventArgs(RoutedEvent rpEvent, DragableTabItem rpItem, DragDeltaEventArgs rpThumbDragEventArgs)
            : base(rpEvent, rpItem, rpThumbDragEventArgs) { }
    }

    delegate void DragableTabDragCompletedEventHandler(object sender, DragableTabDragCompletedEventArgs e);
    public class DragableTabDragCompletedEventArgs : DragableTabDragEventArgs<DragCompletedEventArgs>
    {
        public bool Cancel { get; set; }

        public DragableTabDragCompletedEventArgs(RoutedEvent rpEvent, DragableTabItem rpItem, DragCompletedEventArgs rpThumbDragEventArgs)
            : base(rpEvent, rpItem, rpThumbDragEventArgs) { }
    }
}
