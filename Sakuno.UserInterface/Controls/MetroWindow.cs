using System;
using System.Windows;
using System.Windows.Shell;

namespace Sakuno.UserInterface.Controls
{
    public class MetroWindow : Window
    {
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
