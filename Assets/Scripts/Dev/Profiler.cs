#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#if UNITY_EDITOR
using Debug = UnityEngine.Debug;
#endif

namespace Dev
{
    // todo
    public static class Profiler
    {
        private static readonly Stack<(string name, Stopwatch watch)> Buffers =
            new Stack<(string name, Stopwatch watch)>();

        public static void Start(string name)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Profiler.Buffers.Push((name, stopWatch));
        }

        public static void Stop()
        {
            if (!Profiler.Buffers.Any())
            {
                throw new InvalidOperationException("Buffer start count does not match stop count");
            }

            (string name, Stopwatch watch) = Profiler.Buffers.Pop();
            watch.Stop();
            double milliseconds = watch.Elapsed.TotalMilliseconds;

            string debugMessage = $"P: {name} - {milliseconds:F5} ms.";

        #if UNITY_EDITOR
            Debug.Log(debugMessage);
        #else
                // Console.WriteLine(debugMessage);
        #endif
        }
    }

    public static class LoopProfiler
    {
        private static readonly Dictionary<string, double> Total = new Dictionary<string, double>();

        private static readonly Stack<(string name, Stopwatch watch)> Buffers =
            new Stack<(string name, Stopwatch watch)>();

        public static void Start(string name)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            LoopProfiler.Buffers.Push((name, stopWatch));
        }

        public static void Stop()
        {
            if (!LoopProfiler.Buffers.Any())
            {
                throw new InvalidOperationException("Buffer start count does not match stop count");
            }

            (string name, Stopwatch watch) = LoopProfiler.Buffers.Pop();
            watch.Stop();
            double milliseconds = watch.Elapsed.TotalMilliseconds;

            if (LoopProfiler.Total.TryGetValue(name, out double sum))
            {
                milliseconds += sum;
            }

            LoopProfiler.Total[name] = milliseconds;
        }

        public static void Print()
        {
            foreach (KeyValuePair<string, double> pair in LoopProfiler.Total)
            {
                string debugMessage = $"T: {pair.Key} - {pair.Value:F5} ms.";
            #if UNITY_EDITOR
                Debug.Log(debugMessage);
            #else
                    // Console.WriteLine(debugMessage);
            #endif
            }

            LoopProfiler.Total.Clear();
        }
    }
}
