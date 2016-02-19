using System.Windows;
using System.Windows.Controls;

namespace Sakuno.UserInterface.Controls
{
    public class DockAdorner : Control
    {
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(nameof(Direction), typeof(DockDirection), typeof(DockAdorner), new UIPropertyMetadata(DockDirection.Fill));
        public DockDirection Direction
        {
            get { return (DockDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        static DockAdorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DockAdorner), new FrameworkPropertyMetadata(typeof(DockAdorner)));
        }
    }
}
