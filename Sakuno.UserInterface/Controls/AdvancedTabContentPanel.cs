using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace Sakuno.UserInterface.Controls
{
    public class AdvancedTabContentPanel : VirtualizingPanel
    {
        internal static readonly DependencyProperty ViewportOffsetProperty = DependencyProperty.Register(nameof(ViewportOffset), typeof(double), typeof(AdvancedTabContentPanel),
            new FrameworkPropertyMetadata(DoubleUtil.Zero, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
        internal double ViewportOffset
        {
            get { return (double)GetValue(ViewportOffsetProperty); }
            set { SetValue(ViewportOffsetProperty, value); }
        }

        int r_SelectedItemIndex = -1;

        protected override Size MeasureOverride(Size rpAvailableSize)
        {
            if (rpAvailableSize.IsEmpty || rpAvailableSize.Width.IsInfinity() || rpAvailableSize.Width.IsInfinity())
                throw new Exception();

            var rOwner = ItemsControl.GetItemsOwner(this) as AdvancedTabContentItemsControl;
            if (rOwner == null)
                return rpAvailableSize;

            if (rOwner.Panel == null || rOwner.Panel != this)
                rOwner.Panel = this;

            r_SelectedItemIndex = rOwner.SelectedIndex;

            var rChildren = InternalChildren;
            var rGenerator = ItemContainerGenerator;

            if (rChildren.Count < 2 || ViewportOffset == .0)
            {
                Cleanup(rChildren, rGenerator);
                Generate(rpAvailableSize, rGenerator);
            }

            return rpAvailableSize;
        }

        void Cleanup(UIElementCollection rpChildren, IItemContainerGenerator rpGenerator)
        {
            for (var i = rpChildren.Count - 1; i >= 0; i--)
            {
                var rPosition = new GeneratorPosition(i, 0);
                var rItemIndex = rpGenerator.IndexFromGeneratorPosition(rPosition);
                if (rItemIndex != -1)
                    rpGenerator.Remove(rPosition, 1);
                RemoveInternalChildRange(i, 1);
            }
        }
        void Generate(Size rpAvailableSize, IItemContainerGenerator rpGenerator)
        {
            var rPosition = rpGenerator.GeneratorPositionFromIndex(r_SelectedItemIndex);
            var rDirection = ViewportOffset < .0 ? GeneratorDirection.Forward : GeneratorDirection.Backward;
            using (rpGenerator.StartAt(rPosition, rDirection, true))
            {
                GenerateNextElement(rpAvailableSize, rpGenerator);

                if (Math.Abs(ViewportOffset) > .0)
                    GenerateNextElement(rpAvailableSize, rpGenerator);
            }
        }
        void GenerateNextElement(Size rpAvailableSize, IItemContainerGenerator rpGenerator)
        {
            bool rIsNewlyRealized;
            var rChild = (UIElement)rpGenerator.GenerateNext(out rIsNewlyRealized);

            if (rIsNewlyRealized)
            {
                AddInternalChild(rChild);
                rpGenerator.PrepareItemContainer(rChild);
            }

            rChild?.Measure(rpAvailableSize);
        }

        protected override Size ArrangeOverride(Size rpFinalSize)
        {
            if (InternalChildren.Count == 0 || r_SelectedItemIndex < 0)
                return rpFinalSize;

            var rRect = new Rect(ViewportOffset, .0, rpFinalSize.Width, rpFinalSize.Height);
            InternalChildren[0].Arrange(rRect);

            if (InternalChildren.Count > 1)
            {
                if (ViewportOffset < .0)
                    rRect.X = rpFinalSize.Width + ViewportOffset;
                else
                    rRect.X = -rpFinalSize.Width + ViewportOffset;

                InternalChildren[1].Arrange(rRect);
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
