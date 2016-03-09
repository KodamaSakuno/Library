using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

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
                return base.MeasureOverride(rpAvailableSize);

            if (rOwner.Panel == null || rOwner.Panel != this)
                rOwner.Panel = this;

            r_SelectedItemIndex = rOwner.SelectedIndex;

            var rChildren = InternalChildren;
            var rGenerator = ItemContainerGenerator;

            Cleanup(rChildren, rGenerator);
            Generate(rpAvailableSize, rGenerator);

            return rpAvailableSize;
        }

        void Cleanup(UIElementCollection rpChildren, IItemContainerGenerator rpGenerator)
        {
            for (var i = rpChildren.Count - 1; i >= 0; i--)
            {
                var rPosition = new GeneratorPosition(i, 0);
                var rItemIndex = rpGenerator.IndexFromGeneratorPosition(rPosition);

                rpGenerator.Remove(rPosition, 1);
                RemoveInternalChildRange(i, 1);
            }
        }
        void Generate(Size rpAvailableSize, IItemContainerGenerator rpGenerator)
        {
            var rPosition = rpGenerator.GeneratorPositionFromIndex(r_SelectedItemIndex);
            using (rpGenerator.StartAt(rPosition, GeneratorDirection.Forward, true))
            {
                bool rIsNewlyRealized;
                var rChild = (UIElement)rpGenerator.GenerateNext(out rIsNewlyRealized);

                if (rIsNewlyRealized)
                {
                    AddInternalChild(rChild);
                    rpGenerator.PrepareItemContainer(rChild);
                }

                rChild.Measure(rpAvailableSize);
            }
        }

        protected override Size ArrangeOverride(Size rpFinalSize)
        {
            if (InternalChildren.Count == 0 || r_SelectedItemIndex < 0)
                return rpFinalSize;

            var rRect = new Rect(.0, .0, rpFinalSize.Width, rpFinalSize.Height);
            InternalChildren[0].Arrange(rRect);

            return rpFinalSize;
        }
    }
}
