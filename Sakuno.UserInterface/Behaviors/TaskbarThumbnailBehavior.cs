using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Shell;

namespace Sakuno.UserInterface.Behaviors
{
    public class TaskbarThumbnailBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.Unloaded += AssociatedObject_Unloaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            UodateThumbnail();

            AssociatedObject.LayoutUpdated += AssociatedObject_LayoutUpdated;
        }
        void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            ClearThumbnail();

            AssociatedObject.LayoutUpdated -= AssociatedObject_LayoutUpdated;
        }
        void AssociatedObject_LayoutUpdated(object sender, EventArgs e) => UodateThumbnail();

        void UodateThumbnail()
        {
            var rElement = AssociatedObject;
            if (PresentationSource.FromVisual(rElement) == null)
                return;

            var rWindow = Window.GetWindow(rElement);
            if (rWindow == null)
                return;

            if (rWindow.TaskbarItemInfo == null)
                rWindow.TaskbarItemInfo = new TaskbarItemInfo();

            var rPosition = rElement.PointToScreen(default(Point));
            rPosition = rWindow.PointFromScreen(rPosition);

            rWindow.TaskbarItemInfo.ThumbnailClipMargin = new Thickness(rPosition.X, rPosition.Y, rWindow.ActualWidth - rPosition.X - rElement.ActualWidth, rWindow.ActualHeight - rPosition.Y - rElement.ActualHeight);
        }
        void ClearThumbnail()
        {
            var rWindow = Window.GetWindow(AssociatedObject);
            if (rWindow == null || rWindow.TaskbarItemInfo == null)
                return;

            rWindow.TaskbarItemInfo.ThumbnailClipMargin = new Thickness();
        }
    }
}
