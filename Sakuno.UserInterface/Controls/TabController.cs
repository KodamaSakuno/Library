using System.Windows;

namespace Sakuno.UserInterface.Controls
{
    public class TabController : DependencyObject
    {
        public static readonly DependencyProperty TearOffControllerProperty = DependencyProperty.Register(nameof(TearOffController), typeof(ITabTearOffController), typeof(TabController), new UIPropertyMetadata(null));
        public ITabTearOffController TearOffController
        {
            get { return (ITabTearOffController)GetValue(TearOffControllerProperty); }
            set { SetValue(TearOffControllerProperty, value); }
        }

        public static readonly DependencyProperty PartitionProperty = DependencyProperty.Register(nameof(Partition), typeof(string), typeof(TabController), new UIPropertyMetadata(null));
        public string Partition
        {
            get { return (string)GetValue(PartitionProperty); }
            set { SetValue(PartitionProperty, value); }
        }
    }
}
