using System.Windows;

namespace Sakuno.UserInterface.Controls
{
    public class DropDownButton : ButtonWithPopup
    {
        public static readonly DependencyProperty ShowDropDownMarkerProperty = DependencyProperty.Register(nameof(ShowDropDownMarker), typeof(bool), typeof(DropDownButton),
            new FrameworkPropertyMetadata(BooleanUtil.True));
        public bool ShowDropDownMarker
        {
            get { return (bool)GetValue(ShowDropDownMarkerProperty); }
            set { SetValue(ShowDropDownMarkerProperty, value); }
        }

        static DropDownButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownButton), new FrameworkPropertyMetadata(typeof(DropDownButton)));
        }
    }
}
