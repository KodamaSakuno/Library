using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Sakuno.UserInterface.Controls
{
    [TemplatePart(Name = "PART_Thumb", Type = typeof(Thumb))]
    public class DragableTabItem : TabItem
    {
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof(Position), typeof(double), typeof(DragableTabItem),
            new FrameworkPropertyMetadata(DoubleUtil.Zero, FrameworkPropertyMetadataOptions.AffectsParentArrange));
        public double Position
        {
            get { return (double)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        internal static readonly RoutedEvent DragStartedEvent = EventManager.RegisterRoutedEvent(nameof(DragStarted), RoutingStrategy.Bubble, typeof(DragableTabDragStartedEventHandler), typeof(DragableTabItem));
        internal event DragableTabDragStartedEventHandler DragStarted
        {
            add { AddHandler(DragStartedEvent, value); }
            remove { RemoveHandler(DragStartedEvent, value); }
        }
        internal static readonly RoutedEvent PreviewDragDeltaEvent = EventManager.RegisterRoutedEvent(nameof(PreviewDragDelta), RoutingStrategy.Tunnel, typeof(DragableTabDragDeltaEventHandler), typeof(DragableTabItem));
        internal event DragableTabDragDeltaEventHandler PreviewDragDelta
        {
            add { AddHandler(PreviewDragDeltaEvent, value); }
            remove { RemoveHandler(PreviewDragDeltaEvent, value); }
        }
        internal static readonly RoutedEvent DragDeltaEvent = EventManager.RegisterRoutedEvent(nameof(DragDelta), RoutingStrategy.Bubble, typeof(DragableTabDragDeltaEventHandler), typeof(DragableTabItem));
        internal event DragableTabDragDeltaEventHandler DragDelta
        {
            add { AddHandler(DragDeltaEvent, value); }
            remove { RemoveHandler(DragDeltaEvent, value); }
        }
        internal static readonly RoutedEvent DragCompletedEvent = EventManager.RegisterRoutedEvent(nameof(DragCompleted), RoutingStrategy.Bubble, typeof(DragableTabDragCompletedEventHandler), typeof(DragableTabItem));
        internal event DragableTabDragCompletedEventHandler DragCompleted
        {
            add { AddHandler(DragCompletedEvent, value); }
            remove { RemoveHandler(DragCompletedEvent, value); }
        }

        internal Thumb r_Thumb;
        bool r_CaptureMouseAfterApplyingTemplate;

        internal double PositionAfterMerged { get; set; }

        static DragableTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DragableTabItem), new FrameworkPropertyMetadata(typeof(DragableTabItem)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            r_Thumb = Template.FindName("PART_Thumb", this) as Thumb;
            if (r_Thumb != null)
            {
                r_Thumb.DragStarted += Thumb_DragStarted;
                r_Thumb.DragDelta += Thumb_DragDelta;
                r_Thumb.DragCompleted += Thumb_DragCompleted;

                if (r_CaptureMouseAfterApplyingTemplate)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                        r_Thumb.RaiseEvent(new MouseButtonEventArgs(InputManager.Current.PrimaryMouseDevice, 0, MouseButton.Left) { RoutedEvent = UIElement.MouseLeftButtonDownEvent })
                    ));

                    r_CaptureMouseAfterApplyingTemplate = false;
                }
            }
        }

        void Thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            IsSelected = true;
            RaiseEvent(new DragableTabDragStartedEventArgs(DragStartedEvent, this, e));
        }
        void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var rPreviewEventArgs = new DragableTabDragDeltaEventArgs(PreviewDragDeltaEvent, this, e);
            RaiseEvent(rPreviewEventArgs);
            if (rPreviewEventArgs.Cancel)
                r_Thumb.CancelDrag();

            if (!rPreviewEventArgs.Handled)
            {
                var rEventArgs = new DragableTabDragDeltaEventArgs(DragDeltaEvent, this, e);
                RaiseEvent(rEventArgs);
                if (rEventArgs.Cancel)
                    r_Thumb.CancelDrag();
            }
        }
        void Thumb_DragCompleted(object sender, DragCompletedEventArgs e) => RaiseEvent(new DragableTabDragCompletedEventArgs(DragCompletedEvent, this, e));

        internal void ReceiveDrag()
        {
            if (r_Thumb != null)
            {
                r_Thumb.CaptureMouse();
                return;
            }
            r_CaptureMouseAfterApplyingTemplate = true;
        }
    }
}
