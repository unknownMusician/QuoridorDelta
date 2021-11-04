using System.Linq;
using UnityEngine;

namespace QuoridorDelta.View
{
    public static class NullValidatorExtension
    {
        public static void ValidateNull(this MonoBehaviour behaviour, params Object?[] objects)
        {
            int counter = objects.Count(obj => obj is null);

            if (counter > 0)
            {
                Debug.LogError($"{behaviour.GetType().Name} has {counter} unassigned field{(counter == 1 ? "" : "s")}");
            }
        }
    }
}
