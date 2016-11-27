using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Sakuno.UserInterface.Controls
{
    public class AdvancedTabContentPanel : Panel
    {
        internal static readonly DependencyProperty ViewportOffsetProperty = DependencyProperty.Register(nameof(ViewportOffset), typeof(double), typeof(AdvancedTabContentPanel),
            new FrameworkPropertyMetadata(DoubleUtil.Zero, FrameworkPropertyMetadataOptions.AffectsArrange));
        internal double ViewportOffset
        {
            get { return (double)GetValue(ViewportOffsetProperty); }
            set { SetValue(ViewportOffsetProperty, value); }
        }

        internal int SelectedItemIndex { get; set; }

        protected override Size MeasureOverride(Size rpAvailableSize)
        {
            if (rpAvailableSize.IsEmpty || rpAvailableSize.Width.IsInfinity() || rpAvailableSize.Width.IsInfinity())
                throw new Exception();

            var rOwner = ItemsControl.GetItemsOwner(this) as AdvancedTabContentItemsControl;
            if (rOwner == null)
                return rpAvailableSize;

            if (rOwner.Panel == null || rOwner.Panel != this)
                rOwner.Panel = this;

            foreach (UIElement rChild in InternalChildren)
                rChild.Measure(rpAvailableSize);

            return rpAvailableSize;
        }

        protected override Size ArrangeOverride(Size rpFinalSize)
        {
            if (InternalChildren.Count == 0 || SelectedItemIndex < 0)
                return rpFinalSize;

            var rRect = new Rect(-rpFinalSize.Width * SelectedItemIndex + ViewportOffset, .0, rpFinalSize.Width, rpFinalSize.Height);

            foreach (UIElement rElement in InternalChildren)
            {
                rElement.Arrange(rRect);
                rRect.X += rpFinalSize.Width;
            }

            return rpFinalSize;
        }

        internal void SetViewportOffset(double rpOffset, Action rpContinuationAction = null)
        {
            var rAnimation = new DoubleAnimation(rpOffset, new Duration(TimeSpan.FromMilliseconds(200.0)), FillBehavior.Stop) { EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut } };
            rAnimation.WhenComplete(() =>
            {
                ViewportOffset = .0;
                rpContinuationAction?.Invoke();
            });

            BeginAnimation(ViewportOffsetProperty, rAnimation);
        }
    }
}
