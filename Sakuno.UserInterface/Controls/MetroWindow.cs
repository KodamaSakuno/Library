using Sakuno.UserInterface.Behaviors;
using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Shell;

namespace Sakuno.UserInterface.Controls
{
    public class MetroWindow : Window
    {
        public static readonly DependencyProperty GlowWindowBehaviorProperty = DependencyProperty.Register(nameof(GlowWindowBehavior), typeof(GlowWindowBehavior), typeof(MetroWindow),
            new UIPropertyMetadata(null, OnGlowWindowBehaviorChanged));
        public GlowWindowBehavior GlowWindowBehavior
        {
            get { return (GlowWindowBehavior)GetValue(GlowWindowBehaviorProperty); }
            set { SetValue(GlowWindowBehaviorProperty, value); }
        }

        public static readonly DependencyProperty IsCaptionBarProperty = DependencyProperty.RegisterAttached("IsCaptionBar", typeof(bool), typeof(MetroWindow),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, OnIsCaptionBarChanged));
        public static void SetIsCaptionBar(FrameworkElement rpElement, Boolean rpValue) => rpElement.SetValue(IsCaptionBarProperty, rpValue);
        public static bool GetIsCaptionBar(FrameworkElement rpElement) => (bool)rpElement.GetValue(IsCaptionBarProperty);

        FrameworkElement r_CaptionBar;

        static MetroWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroWindow), new FrameworkPropertyMetadata(typeof(MetroWindow)));
            UseLayoutRoundingProperty.OverrideMetadata(typeof(MetroWindow), new FrameworkPropertyMetadata(BooleanUtil.True));
        }

        static void OnGlowWindowBehaviorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rWindow = (MetroWindow)d;
            var rOldBehavior = (GlowWindowBehavior)e.OldValue;
            var rNewBehavior = (GlowWindowBehavior)e.NewValue;

            if (rOldBehavior == rNewBehavior)
                return;

            var rBehaviors = Interaction.GetBehaviors(rWindow);
            if (rOldBehavior != null)
                rBehaviors.Remove(rOldBehavior);
            if (rNewBehavior != null)
                rBehaviors.Add(rNewBehavior);
        }
        static void OnIsCaptionBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rElement = d as FrameworkElement;
            if (rElement == null)
                return;
            var rWindow = GetWindow(rElement) as MetroWindow;
            if (rWindow == null)
                return;

            rWindow.r_CaptionBar = (bool)e.NewValue ? rElement : null;

            rElement.Loaded += delegate
            {
                var rChrome = WindowChrome.GetWindowChrome(rWindow);
                if (rChrome != null)
                    rChrome.CaptionHeight = rElement.ActualHeight;
            };
        }
    }
}
