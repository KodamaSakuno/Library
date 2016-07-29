using Sakuno.Collections;
using Sakuno.UserInterface.Internal;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace Sakuno.UserInterface.Controls
{
    [ContentProperty("BBCode")]
    public class BBCodeBlock : TextBlock
    {
        public static readonly DependencyProperty BBCodeProperty = DependencyProperty.Register(nameof(BBCode), typeof(string), typeof(BBCodeBlock),
            new PropertyMetadata((s, e) => ((BBCodeBlock)s).Update(true)));
        public string BBCode
        {
            get { return (string)GetValue(BBCodeProperty); }
            set { SetValue(BBCodeProperty, value); }
        }

        internal static IDictionary<string, Func<Inline, Inline>> CustomTags { get; } = new HybridDictionary<string, Func<Inline, Inline>>(StringComparer.OrdinalIgnoreCase);

        bool r_IsUpdating;

        public BBCodeBlock()
        {
            AddHandler(LoadedEvent, new RoutedEventHandler((s, e) => Update(false)));
        }
        void Update(bool rpForceUpdate)
        {
            if (rpForceUpdate)
                r_IsUpdating = true;

            if (!IsLoaded || !r_IsUpdating)
                return;

            Inlines.Clear();
            Inlines.Add(BBCodeParsers.Parse(BBCode));

            r_IsUpdating = false;
        }

        public static void AddCustomTag(string rpTag, Func<Inline, Inline> rpInlineConstructor) => CustomTags.Add(rpTag, rpInlineConstructor);
    }
}
