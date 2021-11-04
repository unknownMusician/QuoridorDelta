using System;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace PaperWorks.Common
{
    public static class Assertions
    {
        public static void AssertNotNull<TNullable>(this TNullable? nullable, string? name = "")
            where TNullable : class
        {
            if (nullable != null)
            {
                return;
            }

            string valueName = string.IsNullOrEmpty(name) ? "value" : name!;

            throw new ArgumentNullException($"{valueName} should not be null");
        }

    #if UNITY_EDITOR
        public static void AssertNotNull<TBehaviour>(
            this TBehaviour behaviour, params object?[] nullables
        )
            where TBehaviour : MonoBehaviour
        {
            int i = 0;

            foreach (object? nullable in nullables)
            {
                nullable.AssertNotNull($"Value #{i} in {behaviour!.GetType().Name}");

                i++;
            }
        }
    #else
        public static void AssertNotNull<TBehaviour>(params object?[] nullables)
        {
            foreach (object? nullable in nullables)
            {
                nullable.AssertNotNull();
            }
        }
    #endif
    }
}
