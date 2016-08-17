using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Sakuno.UserInterface.Controls
{
    [TemplatePart(Name = "PART_ToggleButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    public class ButtonWithPopup : HeaderedContentControl
    {
        public static readonly DependencyProperty PopupAutoCloseProperty = DependencyProperty.Register(nameof(PopupAutoClose), typeof(bool), typeof(ButtonWithPopup),
            new FrameworkPropertyMetadata(BooleanUtil.True));
        public bool PopupAutoClose
        {
            get { return (bool)GetValue(PopupAutoCloseProperty); }
            set { SetValue(PopupAutoCloseProperty, value); }
        }

        public static readonly DependencyProperty PopupVerticalOffsetProperty = DependencyProperty.Register(nameof(PopupVerticalOffset), typeof(double), typeof(ButtonWithPopup),
            new FrameworkPropertyMetadata(DoubleUtil.Zero));
        public double PopupVerticalOffset
        {
            get { return (double)GetValue(PopupVerticalOffsetProperty); }
            set { SetValue(PopupVerticalOffsetProperty, value); }
        }
        public static readonly DependencyProperty PopupHorizontalOffsetProperty = DependencyProperty.Register(nameof(PopupHorizontalOffset), typeof(double), typeof(ButtonWithPopup),
            new FrameworkPropertyMetadata(DoubleUtil.Zero));
        public double PopupHorizontalOffset
        {
            get { return (double)GetValue(PopupHorizontalOffsetProperty); }
            set { SetValue(PopupHorizontalOffsetProperty, value); }
        }

        ToggleButton r_ToggleButton;
        Popup r_Popup;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            r_ToggleButton = Template.FindName("PART_ToggleButton", this) as ToggleButton;

            r_Popup = Template.FindName("PART_Popup", this) as Popup;
            if (r_Popup != null)
            {
                r_Popup.CustomPopupPlacementCallback = PopupPlacementCallback;
                r_Popup.PreviewMouseUp += Popup_PreviewMouseUp;
            }
        }

        void Popup_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (PopupAutoClose && r_ToggleButton != null)
                r_ToggleButton.IsChecked = false;
        }

        CustomPopupPlacement[] PopupPlacementCallback(Size rpPopupSize, Size rpTargetSize, Point rpOffset)
        {
            var rLeft = -rpPopupSize.Width + rpTargetSize.Width - rpOffset.X;
            var rTop = rpTargetSize.Height + rpOffset.Y;

            return new[]
            {
                new CustomPopupPlacement(new Point(rLeft, rTop), PopupPrimaryAxis.Horizontal),
                new CustomPopupPlacement(new Point(rpOffset.X, rTop), PopupPrimaryAxis.Horizontal),
                new CustomPopupPlacement(new Point(rLeft, -rpPopupSize.Height - rpOffset.Y), PopupPrimaryAxis.Horizontal),
            };
        }
    }
}
