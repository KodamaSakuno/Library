using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Sakuno.UserInterface.Controls
{
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    public class DropDownButton : HeaderedContentControl
    {
        public static readonly DependencyProperty ShowDropDownMarkerProperty = DependencyProperty.Register(nameof(ShowDropDownMarker), typeof(bool), typeof(DropDownButton),
            new FrameworkPropertyMetadata(BooleanUtil.True));
        public bool ShowDropDownMarker
        {
            get { return (bool)GetValue(ShowDropDownMarkerProperty); }
            set { SetValue(ShowDropDownMarkerProperty, value); }
        }

        Popup r_Popup;

        static DropDownButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownButton), new FrameworkPropertyMetadata(typeof(DropDownButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            r_Popup = Template.FindName("PART_Popup", this) as Popup;
            if (r_Popup != null)
                r_Popup.CustomPopupPlacementCallback = PopupPlacementCallback;
        }

        CustomPopupPlacement[] PopupPlacementCallback(Size rpPopupSize, Size rpTargetSize, Point rpOffset)
        {
            return new[] { new CustomPopupPlacement(new Point(rpOffset.X * DpiUtil.ScaleX, (rpTargetSize.Height + rpOffset.Y) * DpiUtil.ScaleY), PopupPrimaryAxis.Horizontal) };
        }
    }
}
