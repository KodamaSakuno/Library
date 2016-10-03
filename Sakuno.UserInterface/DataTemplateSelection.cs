using System;
using System.Windows;
using System.Windows.Markup;

namespace Sakuno.UserInterface
{
    [ContentProperty("DataTemplate")]
    public class DataTemplateSelection
    {
        public object EqualsTo { get; set; }

        public Type DataType { get; set; }

        public DataTemplate DataTemplate { get; set; }
    }
}
