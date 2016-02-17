using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Sakuno.UserInterface.Controls
{
    public class AdvancedTabHeaderItemsControl : ItemsControl
    {
        static AdvancedTabHeaderItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedTabHeaderItemsControl), new FrameworkPropertyMetadata(typeof(AdvancedTabHeaderItemsControl)));
        }

        public AdvancedTabHeaderItemsControl()
        {
            ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
        }

        protected override DependencyObject GetContainerForItemOverride() => new AdvancedTabItem();
        protected override bool IsItemItsOwnContainerOverride(object rpItem) => rpItem is AdvancedTabItem;
        protected override void ClearContainerForItemOverride(DependencyObject rpElement, object rpItem)
        {
            base.ClearContainerForItemOverride(rpElement, rpItem);

            Dispatcher.BeginInvoke(new Action(InvalidateMeasure));
        }

        internal IEnumerable<AdvancedTabItem> GetItems()
        {
            var rItemCount = ItemContainerGenerator.Items.Count;
            for (var i = 0; i < rItemCount; i++)
            {
                var rItem = ItemContainerGenerator.ContainerFromIndex(i) as AdvancedTabItem;
                if (rItem != null)
                    yield return rItem;
            }
        }

        void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                Dispatcher.BeginInvoke(new Action(InvalidateMeasure));
        }
        protected override Size MeasureOverride(Size rpConstraint)
        {
            var rWidth = .0;
            var rHeight = .0;

            foreach (var rItem in GetItems())
            {
                rItem.Measure(SizeUtil.Infinity);
                rItem.SetCurrentValue(AdvancedTabItem.LeftProperty, rWidth);

                rWidth += rItem.DesiredSize.Width;
                rHeight = Math.Max(rHeight, rItem.DesiredSize.Height);
            }

            var rFinalWidth = rpConstraint.Width.IsInfinity() ? rWidth : rpConstraint.Width;
            var rFinalHeight = rpConstraint.Height.IsInfinity() ? rHeight : rpConstraint.Height;

            return new Size(rFinalWidth, rFinalHeight);
        }
    }
}
