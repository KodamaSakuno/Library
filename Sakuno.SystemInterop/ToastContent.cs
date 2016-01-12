using System;

namespace Sakuno.SystemInterop
{
    public class ToastContent
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string BodySecondLine { get; set; }

        public bool ShowTitleInTwoLines { get; set; }

        public string ImagePath { get; set; }

        public ToastAudio Audio { get; set; }

        public event Action Activated = () => { };

        internal void InvokeActivated() => Activated();
    }
}
