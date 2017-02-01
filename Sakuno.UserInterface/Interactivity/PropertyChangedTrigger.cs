using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace Sakuno.UserInterface.Interactivity
{
    public class PropertyChangedTrigger : TriggerBase<DependencyObject>
    {
        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register(nameof(Binding), typeof(object), typeof(PropertyChangedTrigger),
            new PropertyMetadata((s, e) => ((PropertyChangedTrigger)s).OnBindingChanged(e)));
        public object Binding
        {
            get { return GetValue(BindingProperty); }
            set { SetValue(BindingProperty, value); }
        }

        public static readonly DependencyProperty IgnoreInitialValueProperty = DependencyProperty.Register(nameof(IgnoreInitialValue), typeof(bool), typeof(PropertyChangedTrigger),
            new PropertyMetadata(BooleanUtil.True));
        public bool IgnoreInitialValue
        {
            get { return (bool)GetValue(IgnoreInitialValueProperty); }
            set { SetValue(IgnoreInitialValueProperty, value); }
        }

        public static readonly DependencyProperty ThrottleDueTimeProperty = DependencyProperty.Register(nameof(ThrottleDueTime), typeof(TimeSpan), typeof(PropertyChangedTrigger),
            new PropertyMetadata(TimeSpan.Zero));
        public TimeSpan ThrottleDueTime
        {
            get { return (TimeSpan)GetValue(ThrottleDueTimeProperty); }
            set { SetValue(ThrottleDueTimeProperty, value); }
        }

        bool r_IsInitialValueHandled;

        int r_Count;

        void OnBindingChanged(DependencyPropertyChangedEventArgs e)
        {
            if (!r_IsInitialValueHandled)
            {
                r_IsInitialValueHandled = true;

                if (IgnoreInitialValue)
                    return;
            }

            var rThrottleDueTime = ThrottleDueTime;
            if (rThrottleDueTime == TimeSpan.Zero)
                InvokeActions(e);
            else
                ThrottleCore(rThrottleDueTime, e);
        }
        async void ThrottleCore(TimeSpan rpDueTime, DependencyPropertyChangedEventArgs e)
        {
            Interlocked.Increment(ref r_Count);

            await Task.Delay(rpDueTime);

            if (Interlocked.Decrement(ref r_Count) == 0)
                InvokeActions(e);
        }
    }
}
