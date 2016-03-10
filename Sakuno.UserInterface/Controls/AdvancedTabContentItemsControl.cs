using System.Windows;
using System.Windows.Controls;

namespace Sakuno.UserInterface.Controls
{
    public class AdvancedTabContentItemsControl : ItemsControl
    {
        static readonly DependencyPropertyKey SelectedIndexPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SelectedIndex), typeof(int), typeof(AdvancedTabContentItemsControl),
            new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.None, (s, e)=> ((AdvancedTabContentItemsControl)s).Panel?.InvalidateMeasure()));
        public static readonly DependencyProperty SelectedIndexProperty = SelectedIndexPropertyKey.DependencyProperty;
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            internal set { SetValue(SelectedIndexPropertyKey, value); }
        }

        internal AdvancedTabControl Owner { get; set; }
        internal AdvancedTabContentPanel Panel { get; set; }
    }
}
