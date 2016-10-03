using System.Windows;
using System.Windows.Controls;

namespace Sakuno.UserInterface.Controls
{
    public class TreeListView : TreeView
    {
        GridViewColumnCollection r_Columns;
        public GridViewColumnCollection Columns
        {
            get
            {
                if (r_Columns == null)
                    r_Columns = new GridViewColumnCollection();

                return r_Columns;
            }
        }

        static TreeListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeListView), new FrameworkPropertyMetadata(typeof(TreeListView)));
        }

        protected override DependencyObject GetContainerForItemOverride() => new TreeListViewItem();
        protected override bool IsItemItsOwnContainerOverride(object rpItem) => rpItem is TreeListViewItem;
    }
}
