using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace QuoridorDelta.View
{
    public static class NullValidatorExtension
    {
        public static void ValidateNull([NotNull] this MonoBehaviour behaviour, [NotNull] params Object[] objects)
        {
            int counter = objects.Count(obj => obj is null);

            if (counter > 0)
            {
                Debug.LogError($"{behaviour.GetType().Name} has {counter} unassigned field{(counter == 1 ? "" : "s")}");
            }
        }
    }
}
