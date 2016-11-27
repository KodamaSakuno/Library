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

            e.Handled = true;
        }
        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            var rTranslation = e.CumulativeManipulation.Translation.X;

            if ((rTranslation > .0 && Owner.SelectedIndex == 0) || (rTranslation < .0 && Owner.SelectedIndex == Owner.Items.Count - 1))
            {
                var rSquareRootOfTranslation = Math.Sqrt(Math.Abs(rTranslation));
                if (rTranslation < .0)
                    rSquareRootOfTranslation *= -1.0;

                Panel.ViewportOffset = rSquareRootOfTranslation;
            }
            else
            {
                Panel.ViewportOffset = rTranslation;
            }

            e.Handled = true;
        }
        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
            if (e.TotalManipulation.Translation == default(Vector) && e.FinalVelocities.LinearVelocity == default(Vector))
                e.Cancel();
            else
                Complete(e.TotalManipulation.Translation.X, e.FinalVelocities.LinearVelocity);

            e.Handled = true;
        }

        void Complete(double rpOffset, Vector rpVelocity)
        {
            if (Panel == null)
                return;

            var rIndex = SelectedIndex;

            var rRatio = Math.Abs(rpOffset) / Panel.ActualWidth;
            if (rRatio < .5 && Math.Abs(rpVelocity.X) < 1.0)
                Panel.SetViewportOffset(.0);
            else if (rpOffset > .0)
            {
                if (rIndex == 0)
                    Panel.SetViewportOffset(.0);
                else
                    Panel.SetViewportOffset(Panel.ActualWidth, () => Owner.SelectedIndex--);
            }
            else
            {
                if (rIndex == Owner.Items.Count - 1)
                    Panel?.SetViewportOffset(.0);
                else
                    Panel.SetViewportOffset(-Panel.ActualWidth, () => Owner.SelectedIndex++);
            }
        }
    }
}
