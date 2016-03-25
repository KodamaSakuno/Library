using System.Windows;
using System.Windows.Controls;

namespace Sakuno.UserInterface.Controls.Docking
{
    public class DockAdorner : Control
    {
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(nameof(Direction), typeof(DockDirection), typeof(DockAdorner), new UIPropertyMetadata(DockDirection.Fill));
        public DockDirection Direction
        {
            get { return (DockDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        static readonly DependencyPropertyKey IsHighlightedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsHighlighted), typeof(bool), typeof(DockAdorner), new UIPropertyMetadata(BooleanUtil.False));
        public static readonly DependencyProperty IsHighlightedProperty = IsHighlightedPropertyKey.DependencyProperty;
        public bool IsHighlighted
        {
            get { return (bool)GetValue(IsHighlightedProperty); }
            internal set { SetValue(IsHighlightedPropertyKey, BooleanUtil.GetBoxed(value)); }
        }

        static DockAdorner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DockAdorner), new FrameworkPropertyMetadata(typeof(DockAdorner)));
        }
    }
}
