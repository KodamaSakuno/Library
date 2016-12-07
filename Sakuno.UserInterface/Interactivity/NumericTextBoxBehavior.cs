using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Sakuno.UserInterface.Interactivity
{
    public class NumericTextBoxBehavior : Behavior<TextBox>
    {
        static Regex r_Regex = new Regex(@"^\d+$");

        protected override void OnAttached()
        {
            AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
            AssociatedObject.PreviewTextInput += OnPreviewTextInput;
            DataObject.AddPastingHandler(AssociatedObject, OnClipboardPaste);

            InputMethod.SetIsInputMethodSuspended(AssociatedObject, true);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
            AssociatedObject.PreviewTextInput -= OnPreviewTextInput;
            DataObject.RemovePastingHandler(AssociatedObject, OnClipboardPaste);

            InputMethod.SetIsInputMethodSuspended(AssociatedObject, false);
        }

        void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                SystemSounds.Beep.Play();
                e.Handled = true;
            }
        }
        void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.IsNullOrEmpty() || !IsValid(e.Text))
            {
                SystemSounds.Beep.Play();
                e.Handled = true;
            }
        }

        void OnClipboardPaste(object sender, DataObjectPastingEventArgs e)
        {
            var rText = e.SourceDataObject.GetData(e.FormatToApply) as string;
            if (rText.IsNullOrEmpty() || !IsValid(rText))
            {
                SystemSounds.Beep.Play();
                e.CancelCommand();
            }
        }

        bool IsValid(string rpText) => r_Regex.IsMatch(rpText);
    }
}
