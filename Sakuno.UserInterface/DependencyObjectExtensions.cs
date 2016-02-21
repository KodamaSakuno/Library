using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Sakuno.UserInterface
{
    public static class DependencyObjectExtensions
    {
        public static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject rpObject)
        {
            if (rpObject is Visual)
            {
                var rCount = VisualTreeHelper.GetChildrenCount(rpObject);
                for (var i = 0; i < rCount; i++)
                    yield return VisualTreeHelper.GetChild(rpObject, i);
            }
        }

        public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject rpObject)
        {
            while (true)
            {
                rpObject = VisualTreeHelper.GetParent(rpObject);

                if (rpObject != null)
                    yield return rpObject;
                else
                    yield break;
            }
        }

        public static bool IsAncestorContained<T>(this DependencyObject rpObject) where T : DependencyObject
        {
            while (rpObject != null)
            {
                rpObject = VisualTreeHelper.GetParent(rpObject);

                if (rpObject is T)
                    return true;
            }

            return false;
        }
    }
}
