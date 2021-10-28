#nullable enable

using System;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace PaperWorks.Common
{
    public static class Assertions
    {
        public static void AssertNotNull<TNullable>(
            this TNullable? nullable, string? name = ""
        ) where TNullable : class
        {
            if (nullable != null)
            {
                return;
            }

            string valueName = string.IsNullOrEmpty(name) ? "value" : name!;

            throw new ArgumentNullException($"{valueName} should not be null");
        }

        public static void AssertNotNull<TBehaviour>(
            this TBehaviour behaviour, params object?[] nullables
        )
    #if UNITY_EDITOR
            where TBehaviour : MonoBehaviour
    #endif
        {
            int i = 0;

            foreach (object? nullable in nullables)
            {
                nullable.AssertNotNull($"Value #{i} in {behaviour.GetType().Name}");

                i++;
            }
        }
    }
}
