using System.Windows;
using System.Windows.Controls;

namespace Sakuno.UserInterface.Controls.Docking
{
    [TemplatePart(Name = "PART_FirstItemContentPresenter", Type = typeof(ContentPresenter))]
    [TemplatePart(Name = "PART_SecondItemContentPresenter", Type = typeof(ContentPresenter))]
    public class DockGroup : Control
    {
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(DockGroup), new UIPropertyMetadata(Orientation.Horizontal));
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty FirstItemProperty = DependencyProperty.Register(nameof(FirstItem), typeof(object), typeof(DockGroup),
            new UIPropertyMetadata(null, (s, e) => ((DockGroup)s).IsFirstItemGeneralElement = !(e.NewValue is DockGroup || e.NewValue is AdvancedTabControl)));
        public object FirstItem
        {
            get { return GetValue(FirstItemProperty); }
            set { SetValue(FirstItemProperty, value); }
        }
        public static readonly DependencyProperty FirstItemLengthProperty = DependencyProperty.Register(nameof(FirstItemLength), typeof(GridLength), typeof(DockGroup),
            new FrameworkPropertyMetadata(new GridLength(1.0, GridUnitType.Star), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public GridLength FirstItemLength
        {
            get { return (GridLength)GetValue(FirstItemLengthProperty); }
            set { SetValue(FirstItemLengthProperty, value); }
        }

        public static readonly DependencyProperty SecondItemProperty = DependencyProperty.Register(nameof(SecondItem), typeof(object), typeof(DockGroup),
            new UIPropertyMetadata(null, (s, e) => ((DockGroup)s).IsSecondItemGeneralElement = !(e.NewValue is DockGroup || e.NewValue is AdvancedTabControl)));
        public object SecondItem
        {
            get { return GetValue(SecondItemProperty); }
            set { SetValue(SecondItemProperty, value); }
        }
        public static readonly DependencyProperty SecondItemLengthProperty = DependencyProperty.Register(nameof(SecondItemLength), typeof(GridLength), typeof(DockGroup),
            new FrameworkPropertyMetadata(new GridLength(1.0, GridUnitType.Star), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public GridLength SecondItemLength
        {
            get { return (GridLength)GetValue(SecondItemLengthProperty); }
            set { SetValue(SecondItemLengthProperty, value); }
        }

        static readonly DependencyPropertyKey IsFirstItemGeneralElementPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsFirstItemGeneralElement), typeof(bool), typeof(DockGroup), new UIPropertyMetadata(BooleanUtil.False));
        public static DependencyProperty IsFirstItemGeneralElementProperty = IsFirstItemGeneralElementPropertyKey.DependencyProperty;
        public bool IsFirstItemGeneralElement
        {
            get { return (bool)GetValue(IsFirstItemGeneralElementProperty); }
            private set { SetValue(IsFirstItemGeneralElementPropertyKey, BooleanUtil.GetBoxed(value)); }
        }
        static readonly DependencyPropertyKey IsSecondItemGeneralElementPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsSecondItemGeneralElement), typeof(bool), typeof(DockGroup), new UIPropertyMetadata(BooleanUtil.False));
        public static DependencyProperty IsSecondItemGeneralElementProperty = IsSecondItemGeneralElementPropertyKey.DependencyProperty;
        public bool IsSecondItemGeneralElement
        {
            get { return (bool)GetValue(IsSecondItemGeneralElementProperty); }
            private set { SetValue(IsSecondItemGeneralElementPropertyKey, BooleanUtil.GetBoxed(value)); }
        }

        internal ContentPresenter FirstItemContentPresenter { get; private set; }
        internal ContentPresenter SecondItemContentPresenter { get; private set; }

        static DockGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DockGroup), new FrameworkPropertyMetadata(typeof(DockGroup)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            FirstItemContentPresenter = Template.FindName("PART_FirstItemContentPresenter", this) as ContentPresenter;
            SecondItemContentPresenter = Template.FindName("PART_SecondItemContentPresenter", this) as ContentPresenter;
        }
    }
}
