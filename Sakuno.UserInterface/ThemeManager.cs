using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Threading;

namespace Sakuno.UserInterface
{
    public class ThemeManager : ModelBase
    {
        static Regex r_AccentUriRegex = new Regex(@"Accents/\w+\.xaml");

        public static ThemeManager Instance { get; } = new ThemeManager();

        Dispatcher r_Dispatcher;

        public Accent Accent { get; private set; }

        ResourceDictionary r_AccentResourceDictionary;

        ThemeManager() { }

        public void Initialize(Application rpApplication, Accent rpAccent)
        {
            r_Dispatcher = rpApplication.Dispatcher;

            var rResources = rpApplication.Resources;

            var rAllDictionaries = EnumerateAllDictionaries(rResources).ToArray();

            var rResourceDictionary = rpAccent.ResourceDictionary;

            r_AccentResourceDictionary = rAllDictionaries.FirstOrDefault(r => r_AccentUriRegex.IsMatch(r.Source.ToString()));
            if (r_AccentResourceDictionary == null)
            {
                r_AccentResourceDictionary = rpAccent.ResourceDictionary;
                rResources.MergedDictionaries.Add(r_AccentResourceDictionary);
            }
            else if (r_AccentResourceDictionary.Source != rResourceDictionary.Source)
                OverwriteResourceDictioanry(r_AccentResourceDictionary, rResourceDictionary);

            Accent = rpAccent;
        }
        IEnumerable<ResourceDictionary> EnumerateAllDictionaries(ResourceDictionary rpResourceDictionary)
        {
            if (rpResourceDictionary.Count == 0)
                yield break;

            foreach (var rMergedDictionary in rpResourceDictionary.MergedDictionaries)
            {
                yield return rMergedDictionary;

                foreach (var rSubDictionary in EnumerateAllDictionaries(rMergedDictionary))
                    yield return rSubDictionary;
            }
        }

        public void ChangeAccent(Accent rpAccent)
        {
            if (Accent == rpAccent)
                return;

            r_Dispatcher.Invoke(() =>
            {
                OverwriteResourceDictioanry(r_AccentResourceDictionary, rpAccent.ResourceDictionary);

                Accent = rpAccent;
                OnPropertyChanged(nameof(Accent));
            });
        }

        void OverwriteResourceDictioanry(ResourceDictionary rpSource, ResourceDictionary rpTarget)
        {
            foreach (var rKey in rpSource.Keys.OfType<string>().Intersect(rpTarget.Keys.OfType<string>()))
                rpSource[rKey] = rpTarget[rKey];
        }
    }
}
