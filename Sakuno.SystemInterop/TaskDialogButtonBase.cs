using System.Threading;

namespace Sakuno.SystemInterop
{
    public abstract class TaskDialogButtonBase
    {
        static int r_IDForNextButton = 19;

        public int ID { get; }

        public string Text { get; }

        public bool IsDefault { get; set; }

        protected TaskDialogButtonBase(string rpText)
        {
            ID = Interlocked.Increment(ref r_IDForNextButton) % 1024 + 19;

            Text = rpText;
        }
        protected TaskDialogButtonBase(int rpID, string rpText)
        {
            ID = rpID;
            Text = rpText;
        }
        protected TaskDialogButtonBase(TaskDialogCommonButton rpID, string rpText) : this((int)rpID, rpText) { }

        public override string ToString() => $"ID = {ID}, Text = {Text}";
    }
}
