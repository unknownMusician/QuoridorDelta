using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class Animations : MonoBehaviour
    {
        [SerializeField] private float _speed = 0.5f;

        public void Move(
            [NotNull] Transform movable,
            Vector3 startPosition,
            Vector3 endPosition,
            [NotNull] Action finHandler
        )
        {
            void LerpConsumer(float t) => movable.position = Vector3.Lerp(startPosition, endPosition, t);

            StartCoroutine(Lerp(_speed, LerpConsumer, finHandler));
        }

        public static IEnumerator Lerp(float time, [NotNull] Action<float> tConsumer, [NotNull] Action finHandler)
        {
            yield return Lerp(time, tConsumer);

            finHandler();
        }

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
    }
}
