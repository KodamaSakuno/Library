using Sakuno.UserInterface.Internal;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Sakuno.UserInterface.Controls
{
    public class AdvancedTabContentItemsControl : ItemsControl
    {
        static readonly DependencyPropertyKey SelectedIndexPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SelectedIndex), typeof(int), typeof(AdvancedTabContentItemsControl),
            new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.None, (s, e) => ((AdvancedTabContentItemsControl)s).OnSelectedIndexChanged((int)e.NewValue)));
        public static readonly DependencyProperty SelectedIndexProperty = SelectedIndexPropertyKey.DependencyProperty;
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            internal set { SetValue(SelectedIndexPropertyKey, value); }
        }

        internal AdvancedTabControl Owner { get; set; }
        internal AdvancedTabContentPanel Panel { get; set; }

        static AdvancedTabContentItemsControl()
        {
            BackgroundProperty.OverrideMetadata(typeof(AdvancedTabContentItemsControl), new FrameworkPropertyMetadata(Brushes.Transparent));
            IsManipulationEnabledProperty.OverrideMetadata(typeof(AdvancedTabContentItemsControl), new PropertyMetadata(BooleanUtil.True, IsManipulationEnabledProperty.GetMetadata(typeof(UIElement)).PropertyChangedCallback));
        }

        void OnSelectedIndexChanged(int rpNewValue)
        {
            if (Panel == null)
                return;

            Panel.SelectedItemIndex = rpNewValue;
            Panel.InvalidateMeasure();
        }

        protected override void OnManipulationStarting(ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = this;
            e.Mode = ManipulationModes.TranslateX;
        }
        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            var rDelta = e.DeltaManipulation.Translation.X;
            if (rDelta == .0)
                return;

            if ((rDelta > .0 && Owner.SelectedIndex == 0) || (rDelta < .0 && Owner.SelectedIndex == Owner.Items.Count - 1))
            {
                var rOffset = Math.Sqrt(Math.Abs(rDelta));
                if (rDelta < .0)
                    rOffset *= -1.0;

                Panel.ViewportOffset = rOffset;
            }
            else
            {
                Panel.SwipeDirection = rDelta > .0 ? SwipeDirection.Backward : SwipeDirection.Forward;
                Panel.ViewportOffset = rDelta;
            }
        }
        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
            Complete(e.TotalManipulation.Translation.X);
        }

        void Complete(double rpOffset)
        {
            if (Panel == null)
                return;

            var rIndex = SelectedIndex;

            var rRatio = Math.Abs(rpOffset) / Panel.ActualWidth;
            if (rRatio < .5)
                Panel?.SetViewportOffset(.0);
            else if (rpOffset > .0)
            {
                if (rIndex > 0)
                    Panel.SetViewportOffset(Panel.ActualWidth, () => Owner.SelectedIndex--);
                else
                    Panel?.SetViewportOffset(.0);
            }
            else
            {
                if (rIndex < Owner.Items.Count - 1)
                    Panel.SetViewportOffset(-Panel.ActualWidth, () => Owner.SelectedIndex++);
                else
                    Panel?.SetViewportOffset(.0);
            }
        }
    }
}
