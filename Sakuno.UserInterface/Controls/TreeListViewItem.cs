using System.Windows;
using System.Windows.Controls;

namespace Sakuno.UserInterface.Controls
{
    public class TreeListViewItem : TreeViewItem
    {
        public int Level
        {
            get
            {
                var rParent = ItemsControlFromItemContainer(this) as TreeListViewItem;

                return rParent != null ? rParent.Level + 1 : 0;
            }
        }

        static TreeListViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeListViewItem), new FrameworkPropertyMetadata(typeof(TreeListViewItem)));
        }

        protected override DependencyObject GetContainerForItemOverride() => new TreeListViewItem();
        protected override bool IsItemItsOwnContainerOverride(object rpItem) => rpItem is TreeListViewItem;
    }
}
