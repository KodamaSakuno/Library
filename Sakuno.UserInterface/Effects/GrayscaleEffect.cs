using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Sakuno.UserInterface.Effects
{
    public class GrayscaleEffect : ShaderEffect
    {
        static PixelShader r_PixelShader = new PixelShader() { UriSource = new Uri(@"pack://application:,,,/Sakuno.UserInterface;component/Effects/GrayscaleEffect.ps") };

        public static readonly DependencyProperty InputProperty = RegisterPixelShaderSamplerProperty(nameof(Input), typeof(GrayscaleEffect), 0);
        public Brush Input
        {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty DesaturationFactorProperty = DependencyProperty.Register(nameof(DesaturationFactor), typeof(double), typeof(GrayscaleEffect),
            new UIPropertyMetadata(DoubleUtil.Zero, PixelShaderConstantCallback(0), CoerceDesaturationFactor));
        public double DesaturationFactor
        {
            get { return (double)GetValue(DesaturationFactorProperty); }
            set { SetValue(DesaturationFactorProperty, value); }
        }

        public GrayscaleEffect()
        {
            PixelShader = r_PixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(DesaturationFactorProperty);
        }

        protected static object CoerceDesaturationFactor(DependencyObject d, object value)
        {
            var rEffect = (GrayscaleEffect)d;
            var rValue = (double)value;

            return rValue.Clamp(0.0, 1.0);
        }
    }
}
