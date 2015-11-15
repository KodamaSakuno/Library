using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives;

namespace Sakuno.UserInterface.Controls
{
    class DragableTabPanel : Panel
    {
        Dictionary<DragableTabItem, ItemPositionInfo> r_SiblingItemOriginalPositions;

        public DragableTabPanel()
        {
            AddHandler(DragableTabItem.DragStartedEvent, new DragableTabDragStartedEventHandler(ItemDragStarted));
            AddHandler(DragableTabItem.DragDeltaEvent, new DragableTabDragDeltaEventHandler(ItemDragDelta));
            AddHandler(DragableTabItem.DragCompletedEvent, new DragableTabDragCompletedEventHandler(ItemDragCompleted));
        }

        protected override Size ArrangeOverride(Size rpFinalSize)
        {
            var rRect = new Rect(0, 0, rpFinalSize.Width, rpFinalSize.Height);

            foreach (var rItem in InternalChildren.OfType<DragableTabItem>())
            {
                var rOriginalX = rRect.X;
                rRect.Width = rItem.DesiredSize.Width;
                rRect.X = rItem.Position;
                rItem.Arrange(rRect);
            }

            return rpFinalSize;
        }
        protected override Size MeasureOverride(Size rpAvailableSize)
        {
            var rWidth = .0;
            var rHeight = .0;

            foreach (var rItem in InternalChildren.OfType<DragableTabItem>().OrderBy(r => r.PositionAfterMerged == 0.0 ? r.Position : r.PositionAfterMerged))
            {
                rItem.Measure(SizeUtil.Infinity);
                rItem.Position = rItem.PositionAfterMerged == 0.0 ? rWidth : rItem.PositionAfterMerged;
                rWidth += rItem.DesiredSize.Width;
                rHeight = Math.Max(rHeight, rItem.DesiredSize.Height);
            }

            var rFinalWidth = rpAvailableSize.Width.IsInfinity() ? rWidth : rpAvailableSize.Width;
            var rFinalHeight = rpAvailableSize.Height.IsInfinity() ? rHeight : rpAvailableSize.Height;

            return new Size(rFinalWidth, rFinalHeight);
        }

        void ItemDragStarted(object sender, DragableTabDragEventArgs<DragStartedEventArgs> e)
        {
            r_SiblingItemOriginalPositions = InternalChildren.OfType<DragableTabItem>().Except(new[] { e.Item }).Select(r => new ItemPositionInfo(r)).ToDictionary(r => r.Item);

            e.Handled = true;
        }
        void ItemDragDelta(object sender, DragableTabDragDeltaEventArgs e)
        {
            e.Item.Position = Math.Max(e.Item.Position + e.DragEventArgs.HorizontalChange, -2.0);

            var rItems = r_SiblingItemOriginalPositions.Values.Concat(new[] { new ItemPositionInfo(e.Item) })
                   .OrderBy(r => r.Item == e.Item ? r.Start : r_SiblingItemOriginalPositions[r.Item].Start).Select(r => r.Item);

            var rPosition = .0;
            foreach (var rItem in rItems)
            {
                if (rItem != e.Item && rItem.Position != rPosition)
                    SetItemPosition(rItem, rPosition);
                rPosition += rItem.DesiredSize.Width;

                Panel.SetZIndex(rItem, 0);
            }

            Panel.SetZIndex(e.Item, int.MaxValue);
        }
        void ItemDragCompleted(object sender, DragableTabDragEventArgs<DragCompletedEventArgs> e)
        {
            var rItems = r_SiblingItemOriginalPositions.Values.Concat(new[] { new ItemPositionInfo(e.Item) })
                   .OrderBy(r => r.Item == e.Item ? r.Start : r_SiblingItemOriginalPositions[r.Item].Start).Select(r => r.Item).ToList();

            var rPosition = .0;
            foreach (var rItem in rItems)
            {
                if (rItem.Position != rPosition)
                    SetItemPosition(rItem, rPosition);
                rPosition += rItem.DesiredSize.Width;

                rItem.PositionAfterMerged = .0;
            }

            e.Handled = true;
        }

        void SetItemPosition(DragableTabItem rpItem, double rpPosition)
        {
            var rAnimation = new DoubleAnimation(rpPosition, new Duration(TimeSpan.FromMilliseconds(200.0)), FillBehavior.Stop) { EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut } };
            rAnimation.WhenComplete(() => rpItem.Position = rpPosition);

            rpItem.BeginAnimation(DragableTabItem.PositionProperty, rAnimation);
        }

        struct ItemPositionInfo
        {
            public DragableTabItem Item { get; }

            public double Size { get; }
            public double Start { get; }
            public double Middle { get; }
            public double End { get; }

            public ItemPositionInfo(DragableTabItem rpItem)
            {
                Item = rpItem;

                Size = rpItem.DesiredSize.Width;
                Start = rpItem.Position;
                End = Start + Size;
                Middle = End / 2.0;
            }
        }
    }
}
