namespace Sakuno.SystemInterop
{
    public static class TaskDialogExtensions
    {
        public static TaskDialogResult ShowAndDispose(this TaskDialog rpTaskDialog)
        {
            using (rpTaskDialog)
                return rpTaskDialog.Show();
        }
    }
}
