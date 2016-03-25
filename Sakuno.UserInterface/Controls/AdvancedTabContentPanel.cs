using Sakuno.UserInterface.Internal;
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
            new FrameworkPropertyMetadata(DoubleUtil.Zero, FrameworkPropertyMetadataOptions.AffectsArrange));
        internal double ViewportOffset
        {
            get { return (double)GetValue(ViewportOffsetProperty); }
            set { SetValue(ViewportOffsetProperty, value); }
        }

        internal static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(nameof(SwipeDirection), typeof(SwipeDirection), typeof(AdvancedTabContentPanel),
            new FrameworkPropertyMetadata(SwipeDirection.None, FrameworkPropertyMetadataOptions.AffectsMeasure, (s, e) => ((AdvancedTabContentPanel)s).OnDirectionChanged((SwipeDirection)e.OldValue, (SwipeDirection)e.NewValue)));
        internal SwipeDirection SwipeDirection
        {
            get { return (SwipeDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        internal int SelectedItemIndex { get; set; }

        void OnDirectionChanged(SwipeDirection rpOldValue, SwipeDirection rpNewValue)
        {
            if (InternalChildren.Count == 2 && (rpNewValue == SwipeDirection.None || (rpOldValue == SwipeDirection.Forward && rpNewValue == SwipeDirection.Backward) || (rpOldValue == SwipeDirection.Backward && rpNewValue == SwipeDirection.Forward)))
                Cleanup(1);

            switch (rpNewValue)
            {
                case SwipeDirection.Forward:
                    Generate(DesiredSize, SelectedItemIndex + 1, GeneratorDirection.Forward);
                    break;

                case SwipeDirection.Backward:
                    Generate(DesiredSize, SelectedItemIndex - 1, GeneratorDirection.Backward);
                    break;
            }
        }

        protected override Size MeasureOverride(Size rpAvailableSize)
        {
            if (rpAvailableSize.IsEmpty || rpAvailableSize.Width.IsInfinity() || rpAvailableSize.Width.IsInfinity())
                throw new Exception();

            var rOwner = ItemsControl.GetItemsOwner(this) as AdvancedTabContentItemsControl;
            if (rOwner == null)
                return rpAvailableSize;

            if (rOwner.Panel == null || rOwner.Panel != this)
                rOwner.Panel = this;

            var rChildren = InternalChildren;
            var rGenerator = ItemContainerGenerator;

            if (rChildren.Count < 2)
            {
                var rPosition = new GeneratorPosition(0, 0);
                var rItemIndex = ItemContainerGenerator.IndexFromGeneratorPosition(rPosition);
                if (rItemIndex != SelectedItemIndex)
                    Cleanup(0);
                Generate(rpAvailableSize, SelectedItemIndex, GeneratorDirection.Forward);
            }

            foreach (UIElement rChild in Children)
                rChild.Measure(rpAvailableSize);

            return rpAvailableSize;
        }

        void Cleanup(int rpIndex)
        {
            var rPosition = new GeneratorPosition(rpIndex, 0);
            var rItemIndex = ItemContainerGenerator.IndexFromGeneratorPosition(rPosition);
            if (rItemIndex != -1)
                ItemContainerGenerator.Remove(rPosition, 1);
            RemoveInternalChildRange(rpIndex, 1);
        }

        void Generate(Size rpAvailableSize, int rpIndex, GeneratorDirection rpDirection)
        {
            var rPosition = ItemContainerGenerator.GeneratorPositionFromIndex(rpIndex);
            using (ItemContainerGenerator.StartAt(rPosition, rpDirection, true))
            {
                bool rIsNewlyRealized;
                var rChild = (UIElement)ItemContainerGenerator.GenerateNext(out rIsNewlyRealized);

                if (rIsNewlyRealized)
                {
                    AddInternalChild(rChild);
                    ItemContainerGenerator.PrepareItemContainer(rChild);
                }

                rChild?.Measure(rpAvailableSize);
            }
        }

        protected override Size ArrangeOverride(Size rpFinalSize)
        {
            if (InternalChildren.Count == 0 || SelectedItemIndex < 0)
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
                SwipeDirection = SwipeDirection.None;
                rpContinuationAction?.Invoke();
            });

            BeginAnimation(ViewportOffsetProperty, rAnimation);
        }
    }
}
