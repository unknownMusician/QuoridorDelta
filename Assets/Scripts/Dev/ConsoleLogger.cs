#if UNITY_EDITOR
using UnityEngine;
#else
using System;
#endif

namespace Dev
{
    public static class ConsoleLogger
    {
        public static void Log(object msg)
        {
        #if UNITY_EDITOR
            Debug.Log(msg);
        #else
            Console.WriteLine(msg);
        #endif
        }
    }
}
