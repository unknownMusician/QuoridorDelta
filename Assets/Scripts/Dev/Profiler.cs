using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Debug = UnityEngine.Debug;

namespace Dev
{
    public static class Profiler
    {
        private static readonly Stack<(string name, Stopwatch watch)> Buffers =
            new Stack<(string name, Stopwatch watch)>();

        public static void Start(string name)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Buffers.Push((name, stopWatch));
        }

        public static void Stop()
        {
            DateTime now = DateTime.Now;

            if (!Buffers.Any())
            {
                throw new InvalidOperationException("Buffer start count does not match stop count");
            }

            (string name, Stopwatch watch) = Buffers.Pop();
            watch.Stop();
            double milliseconds = watch.Elapsed.TotalMilliseconds;

            Debug.Log($"P: {name} - {milliseconds:F5} ms.");
        }
    }
    
    public static class LoopProfiler
    {
        private static readonly Dictionary<string, double> Total =
            new Dictionary<string, double>();
        
        private static readonly Stack<(string name, Stopwatch watch)> Buffers =
            new Stack<(string name, Stopwatch watch)>();

        public static void Start(string name)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Buffers.Push((name, stopWatch));
        }

        public static void Stop()
        {
            DateTime now = DateTime.Now;

            if (!Buffers.Any())
            {
                throw new InvalidOperationException("Buffer start count does not match stop count");
            }

            (string name, Stopwatch watch) = Buffers.Pop();
            watch.Stop();
            double milliseconds = watch.Elapsed.TotalMilliseconds;

            if (Total.TryGetValue(name, out double sum))
            {
                milliseconds += sum;
            }
            
            Total[name] = milliseconds;
        }

        public static void Print()
        {
            foreach (KeyValuePair<string, double> pair in Total)
            {
                Debug.Log($"T: {pair.Key} - {pair.Value:F5} ms.");
            }
            
            Total.Clear();
        }
    }
}
