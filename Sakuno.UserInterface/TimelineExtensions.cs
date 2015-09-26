using System;
using System.Windows.Media.Animation;

namespace Sakuno.UserInterface
{
    public static class TimelineExtensions
    {
        public static void WhenComplete(this Timeline rpTimeline, Action rpContinuation)
        {
            EventHandler rHandler = null;
            rHandler = (s, e) =>
            {
                rpTimeline.Completed -= rHandler;
                rpContinuation();
            };

            rpTimeline.Completed += rHandler;
        }
    }
}
