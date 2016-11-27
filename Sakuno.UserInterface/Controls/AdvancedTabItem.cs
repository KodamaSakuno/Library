using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Sakuno.UserInterface.Controls
{
    [TemplatePart(Name = "PART_Thumb", Type = typeof(Thumb))]
    public class AdvancedTabItem : ContentControl
    {
        internal static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(AdvancedTabItem),
            new FrameworkPropertyMetadata(BooleanUtil.False, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsParentMeasure));
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            internal set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register(nameof(Left), typeof(double), typeof(AdvancedTabItem),
            new PropertyMetadata(DoubleUtil.Zero, (d, e) => ((AdvancedTabItem)d).RaiseEvent(new RoutedPropertyChangedEventArgs<double>((double)e.OldValue, (double)e.NewValue) { RoutedEvent = LeftChangedEvent })));
        public double Left
        {
            get { return (double)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }
        public static readonly DependencyProperty TopProperty = DependencyProperty.Register(nameof(Top), typeof(double), typeof(AdvancedTabItem),
            new PropertyMetadata(DoubleUtil.Zero, (d, e) => ((AdvancedTabItem)d).RaiseEvent(new RoutedPropertyChangedEventArgs<double>((double)e.OldValue, (double)e.NewValue) { RoutedEvent = TopChangedEvent })));
        public double Top
        {
            get { return (double)GetValue(TopProperty); }
            set { SetValue(TopProperty, value); }
        }

        public static readonly RoutedEvent LeftChangedEvent = EventManager.RegisterRoutedEvent(nameof(LeftChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(AdvancedTabItem));
        public event RoutedPropertyChangedEventHandler<double> LeftChanged
        {
            add { AddHandler(LeftChangedEvent, value); }
            remove { RemoveHandler(LeftChangedEvent, value); }
        }
        public static readonly RoutedEvent TopChangedEvent = EventManager.RegisterRoutedEvent(nameof(TopChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(AdvancedTabItem));
        public event RoutedPropertyChangedEventHandler<double> TopChanged
        {
            add { AddHandler(TopChangedEvent, value); }
            remove { RemoveHandler(TopChangedEvent, value); }
        }

        internal static readonly RoutedEvent DragStartedEvent = EventManager.RegisterRoutedEvent(nameof(DragStarted), RoutingStrategy.Bubble, typeof(AdvancedTabDragStartedEventHandler), typeof(AdvancedTabItem));
        internal event AdvancedTabDragStartedEventHandler DragStarted
        {
            add { AddHandler(DragStartedEvent, value); }
            remove { RemoveHandler(DragStartedEvent, value); }
        }
        internal static readonly RoutedEvent PreviewDragDeltaEvent = EventManager.RegisterRoutedEvent(nameof(PreviewDragDelta), RoutingStrategy.Tunnel, typeof(AdvancedTabDragDeltaEventHandler), typeof(AdvancedTabItem));
        internal event AdvancedTabDragDeltaEventHandler PreviewDragDelta
        {
            add { AddHandler(PreviewDragDeltaEvent, value); }
            remove { RemoveHandler(PreviewDragDeltaEvent, value); }
        }
        internal static readonly RoutedEvent DragDeltaEvent = EventManager.RegisterRoutedEvent(nameof(DragDelta), RoutingStrategy.Bubble, typeof(AdvancedTabDragDeltaEventHandler), typeof(AdvancedTabItem));
        internal event AdvancedTabDragDeltaEventHandler DragDelta
        {
            add { AddHandler(DragDeltaEvent, value); }
            remove { RemoveHandler(DragDeltaEvent, value); }
        }
        internal static readonly RoutedEvent DragCompletedEvent = EventManager.RegisterRoutedEvent(nameof(DragCompleted), RoutingStrategy.Bubble, typeof(AdvancedTabDragCompletedEventHandler), typeof(AdvancedTabItem));
        internal event AdvancedTabDragCompletedEventHandler DragCompleted
        {
            add { AddHandler(DragCompletedEvent, value); }
            remove { RemoveHandler(DragCompletedEvent, value); }
        }

        internal static readonly RoutedEvent ClosingEvent = EventManager.RegisterRoutedEvent(nameof(ClosingEvent), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AdvancedTabItem));
        internal event RoutedEventHandler Closing
        {
            add { AddHandler(ClosingEvent, value); }
            remove { RemoveHandler(ClosingEvent, value); }
        }

        public static readonly RoutedCommand CloseTabCommand = new RoutedUICommand("CloseTab", "CloseTab", typeof(AdvancedTabItem));

        Thumb r_Thumb;

        bool r_ReceiveDragOnApplyTemplate;

        static AdvancedTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedTabItem), new FrameworkPropertyMetadata(typeof(AdvancedTabItem)));

            EventManager.RegisterClassHandler(typeof(AdvancedTabItem), Thumb.DragStartedEvent, new DragStartedEventHandler((s, e) => ((AdvancedTabItem)s).OnDragStarted(s, e)));
            EventManager.RegisterClassHandler(typeof(AdvancedTabItem), Thumb.DragDeltaEvent, new DragDeltaEventHandler((s, e) => ((AdvancedTabItem)s).OnDragDelta(s, e)));
            EventManager.RegisterClassHandler(typeof(AdvancedTabItem), Thumb.DragCompletedEvent, new DragCompletedEventHandler((s, e) => ((AdvancedTabItem)s).OnDragCompleted(s, e)));

            CommandManager.RegisterClassCommandBinding(typeof(AdvancedTabItem), new CommandBinding(CloseTabCommand, CloseTab));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            r_Thumb = Template.FindName("PART_Thumb", this) as Thumb;
            if (r_Thumb != null && r_ReceiveDragOnApplyTemplate)
                Dispatcher.BeginInvoke(new Action(() => r_Thumb.RaiseEvent(new MouseButtonEventArgs(InputManager.Current.PrimaryMouseDevice, 0, MouseButton.Left) { RoutedEvent = MouseLeftButtonDownEvent })));

            r_ReceiveDragOnApplyTemplate = false;
        }

        void OnDragStarted(object sender, DragStartedEventArgs e)
        {
            e.Handled = true;

            IsSelected = true;
            RaiseEvent(new AdvancedTabDragStartedEventArgs(DragStartedEvent, this, e));
        }
        void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            e.Handled = true;

            var rPreviewEventArgs = new AdvancedTabDragDeltaEventArgs(PreviewDragDeltaEvent, this, e);
            RaiseEvent(rPreviewEventArgs);
            if (rPreviewEventArgs.Cancel)
                r_Thumb.CancelDrag();

            if (!rPreviewEventArgs.Handled)
            {
                var rEventArgs = new AdvancedTabDragDeltaEventArgs(DragDeltaEvent, this, e);
                RaiseEvent(rEventArgs);
                if (rEventArgs.Cancel)
                    r_Thumb.CancelDrag();
            }
        }
        void OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            e.Handled = true;

            RaiseEvent(new AdvancedTabDragCompletedEventArgs(DragCompletedEvent, this, e));
        }

        internal void ReceiveDrag()
        {
            var rThumb = Template.FindName("PART_Thumb", this) as Thumb;
            if (rThumb != null)
            {
                rThumb.CaptureMouse();
                return;
            }

            r_ReceiveDragOnApplyTemplate = true;
        }

        static void CloseTab(object sender, ExecutedRoutedEventArgs e)
        {
            var rOriginalSource = e.OriginalSource as DependencyObject;
            if (rOriginalSource == null)
                return;

            var rOwner = rOriginalSource.GetAncestors().OfType<AdvancedTabItem>().FirstOrDefault();
            if (rOwner == null)
                return;

            rOwner.RaiseEvent(new RoutedEventArgs(ClosingEvent, rOwner));
        }
    }
}
