using System;
using System.Windows;

namespace Sakuno.UserInterface
{
    public class Accent
    {
        public static Accent Blue { get; } = new Accent("Blue", new Uri("pack://application:,,,/Sakuno.UserInterface;component/Themes/Accents/Blue.xaml"));
        public static Accent Brown { get; } = new Accent("Brown", new Uri("pack://application:,,,/Sakuno.UserInterface;component/Themes/Accents/Brown.xaml"));

        public string Name { get; }

        public ResourceDictionary ResourceDictionary { get; }

        public Accent(string rpName, Uri rpUri)
        {
            Name = rpName;

            ResourceDictionary = new ResourceDictionary() { Source = rpUri };
        }
    }
}
