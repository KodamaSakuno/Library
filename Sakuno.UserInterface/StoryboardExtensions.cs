using System;
using System.Windows.Media.Animation;

namespace Sakuno.UserInterface
{
    public static class StoryboardExtensions
    {
        public static void WhenComplete(this Storyboard rpStoryboard, Action<Storyboard> rpContinuation)
        {
            EventHandler rHandler = null;
            rHandler = (s, e) =>
            {
                rpStoryboard.Completed -= rHandler;
                rpContinuation(rpStoryboard);
            };

            rpStoryboard.Completed += rHandler;
        }
    }
}
