using System;
using System.Diagnostics;
using System.Threading;

namespace Sakuno.SystemInterop
{
    public static class CodeTimer
    {
        public static void Initialize()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            Evaluate(string.Empty, 1, () => { });
        }

        public static void Evaluate(string rpName, int rpIteration, Action rpAction)
        {
            if (rpName.IsNullOrEmpty())
                return;

            var rOldForegroundColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(rpName);

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

            var rGCCounts = new int[GC.MaxGeneration + 1];
            for (var i = 0; i <= GC.MaxGeneration; i++)
                rGCCounts[i] = GC.CollectionCount(i);

            var rStopwatch = Stopwatch.StartNew();
            var rCycleCount = GetCycleCount();

            for (var i = 0; i < rpIteration; i++)
                rpAction();

            rCycleCount = GetCycleCount() - rCycleCount;
            rStopwatch.Stop();

            Console.ForegroundColor = rOldForegroundColor;
            Console.WriteLine($"\tTime Elapsed:\t{rStopwatch.ElapsedMilliseconds:N0}ms");
            Console.WriteLine($"\tCPU Cycles:\t{rCycleCount:N0}");

            for (var i = 0; i <= GC.MaxGeneration; i++)
            {
                var rGCCountDiff = GC.CollectionCount(i) - rGCCounts[i];

                Console.WriteLine($"\tGen {i}: \t\t{rGCCountDiff}");
            }

            Console.WriteLine();
        }

        static ulong GetCycleCount()
        {
            ulong rResult;
            NativeMethods.Kernel32.QueryThreadCycleTime(NativeMethods.Kernel32.GetCurrentThread(), out rResult);
            return rResult;
        }
    }
}
