#nullable enable

#if UNITY_EDITOR
using UnityEngine;
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
