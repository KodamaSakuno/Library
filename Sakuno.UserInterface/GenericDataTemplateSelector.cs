using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Sakuno.UserInterface
{
    [ContentProperty("Selection")]
    public class GenericDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Default { get; set; }

        Collection<DataTemplateSelection> r_Selection;
        public Collection<DataTemplateSelection> Selection
        {
            get
            {
                if (r_Selection == null)
                    r_Selection = new Collection<DataTemplateSelection>();
                return r_Selection;
            }
        }

        public override DataTemplate SelectTemplate(object rpItem, DependencyObject rpContainer)
        {
            if (r_Selection != null && rpItem != null)
            {
                var rType = rpItem.GetType();

                foreach (var rSelection in r_Selection)
                    if (rpItem == rSelection.EqualsTo || rType == rSelection.DataType)
                        return rSelection.DataTemplate;
            }

            return Default ?? base.SelectTemplate(rpItem, rpContainer);
        }
    }
}
