using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Sakuno.UserInterface.Controls
{
    [TemplatePart(Name = "PART_ToggleButton", Type = typeof(ToggleButton))]
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

        public static readonly DependencyProperty PopupAutoCloseProperty = DependencyProperty.Register(nameof(PopupAutoClose), typeof(bool), typeof(DropDownButton),
            new FrameworkPropertyMetadata(BooleanUtil.True));
        public bool PopupAutoClose
        {
            get { return (bool)GetValue(PopupAutoCloseProperty); }
            set { SetValue(PopupAutoCloseProperty, value); }
        }

        ToggleButton r_ToggleButton;
        Popup r_Popup;

        static DropDownButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownButton), new FrameworkPropertyMetadata(typeof(DropDownButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            r_ToggleButton = Template.FindName("PART_ToggleButton", this) as ToggleButton;

            r_Popup = Template.FindName("PART_Popup", this) as Popup;
            if (r_Popup != null)
                if (r_Popup != null)
                {
                    r_Popup.CustomPopupPlacementCallback = PopupPlacementCallback;

                    r_Popup.PreviewMouseUp += Popup_PreviewMouseUp;
                }
        }

        void Popup_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (PopupAutoClose)
            {
                if (r_ToggleButton != null)
                    r_ToggleButton.IsChecked = false;
            }
        }

        CustomPopupPlacement[] PopupPlacementCallback(Size rpPopupSize, Size rpTargetSize, Point rpOffset)
        {
            return new[] { new CustomPopupPlacement(new Point(rpOffset.X * DpiUtil.ScaleX, (rpTargetSize.Height + rpOffset.Y) * DpiUtil.ScaleY), PopupPrimaryAxis.Horizontal) };
        }
    }
}
