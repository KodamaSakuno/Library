using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Sakuno.UserInterface.Controls
{
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    public class SplitButton : HeaderedContentControl
    {
        public static readonly DependencyProperty CommandProperty;
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty;
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandTargetProperty;
        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        Popup r_Popup;

        static SplitButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitButton), new FrameworkPropertyMetadata(typeof(SplitButton)));

            CommandProperty = Button.CommandProperty.AddOwner(typeof(SplitButton));
            CommandParameterProperty = Button.CommandParameterProperty.AddOwner(typeof(SplitButton));
            CommandTargetProperty = Button.CommandTargetProperty.AddOwner(typeof(SplitButton));
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
