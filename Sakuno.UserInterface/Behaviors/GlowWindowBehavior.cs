using Sakuno.UserInterface.Controls;
using Sakuno.UserInterface.Internal;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Sakuno.UserInterface.Behaviors
{
    public class GlowWindowBehavior : Behavior<Window>
    {
        public static readonly DependencyProperty GlowBrushProperty = DependencyProperty.Register(nameof(GlowBrush), typeof(Brush), typeof(GlowWindowBehavior),
            new UIPropertyMetadata(Brushes.Transparent));
        public Brush GlowBrush
        {
            get { return (Brush)GetValue(GlowBrushProperty); }
            set { SetValue(GlowBrushProperty, value); }
        }

        public static readonly DependencyProperty InactiveGlowBrushProperty = DependencyProperty.Register(nameof(InactiveGlowBrush), typeof(Brush), typeof(GlowWindowBehavior),
            new UIPropertyMetadata(Brushes.Transparent));
        public Brush InactiveGlowBrush
        {
            get { return (Brush)GetValue(InactiveGlowBrushProperty); }
            set { SetValue(InactiveGlowBrushProperty, value); }
        }

        PropertyChangedEventListener r_ThemeManagerPCEL = new PropertyChangedEventListener(ThemeManager.Instance);

        GlowWindow r_Left, r_Top, r_Right, r_Bottom;

        public Window Window => AssociatedObject;

        protected override void OnAttached()
        {
            GlowBrush = Application.Current.TryFindResource("AccentBrushKey") as Brush;

            r_ThemeManagerPCEL.Add(nameof(ThemeManager.Instance.Accent), (s, e) => GlowBrush = Application.Current.TryFindResource("AccentBrushKey") as Brush);

            r_Left = new GlowWindow(this, new GlowWindowProcessorLeft());
            r_Top = new GlowWindow(this, new GlowWindowProcessorTop());
            r_Right = new GlowWindow(this, new GlowWindowProcessorRight());
            r_Bottom = new GlowWindow(this, new GlowWindowProcessorBottom());
        }
        protected override void OnDetaching() => r_ThemeManagerPCEL.Dispose();

        void Update()
        {
            r_Left?.Update();
            r_Top?.Update();
            r_Right?.Update();
            r_Bottom?.Update();
        }
    }
}
