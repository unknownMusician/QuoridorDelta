using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace QuoridorDelta.View
{
    public static class Animations
    {
        public static IEnumerator Lerp(float time, [NotNull] Action<float> tConsumer)
        {
            float t = 0.0f;

            while (t < 1.0f)
            {
                t += Time.deltaTime / time;
                tConsumer(t);

                yield return null;
            }

            tConsumer(1.0f);
        }

        public static IEnumerator Lerp(float time, [NotNull] Action<float> tConsumer, [NotNull] Action finHandler)
        {
            yield return Animations.Lerp(time, tConsumer);

            finHandler();
        }
        
        
        public static IEnumerator Wait(float time, [NotNull] Action finHandler)
        {
            float t = 0.0f;

            while (t < 1.0f)
            {
                t += Time.deltaTime / time;

                yield return null;
            }

            finHandler();
        }
    }
}
