using System.Collections.Generic;
using JetBrains.Annotations;
using QuoridorDelta.Model;
using UnityEngine;

namespace QuoridorDelta.View.Refactor
{
    public sealed class Backlight : MonoBehaviour
    {
        [SerializeField] private GameObject _lightPrefab;
        [SerializeField] private Transform _lightsParent;
        [SerializeField] private CoordsConverter _coordsConverter;
        
        private const int MaxPossibleLightsCount = 6;

        private readonly List<RectTransform> _lights = new List<RectTransform>();

        private void Awake()
        {
            for (int i = 0; i < MaxPossibleLightsCount; i++)
            {
                _lights.Add(Object.Instantiate(_lightPrefab, _lightsParent).GetComponent<RectTransform>());
            }
        }

        public void TurnOnLightOnCells([NotNull] IEnumerable<Coords> possibleMoves)
        {
            int currentLightObjectIndex = 0;

            foreach (Coords coords in possibleMoves)
            {
                RectTransform light = _lights[currentLightObjectIndex];

                light.position = _coordsConverter.ToVector3(coords);
                light.gameObject.SetActive(true);
                currentLightObjectIndex++;
            }
        }

        public void TurnOffLights() => _lights.ForEach(light => light.gameObject.SetActive(false));
    }
}
