using System.Collections.Generic;
using QuoridorDelta.Model;
using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class Backlight : MonoBehaviour
    {
        [SerializeField] private GameObject _lightPrefab = default!;
        [SerializeField] private Transform _lightsParent = default!;
        [SerializeField] private CoordsConverter _coordsConverter = default!;
        
        private const int MaxPossibleLightsCount = 5;

        private readonly List<RectTransform> _lights = new List<RectTransform>();

        private void Awake()
        {
            for (int i = 0; i < Backlight.MaxPossibleLightsCount; i++)
            {
                _lights.Add(Object.Instantiate(_lightPrefab, _lightsParent).GetComponent<RectTransform>());
            }
        }

        public void TurnOnLightOnCells(IEnumerable<Coords> possibleMoves)
        {
            int currentLightObjectIndex = 0;

            foreach (Coords coords in possibleMoves)
            {
                RectTransform lightTransform = _lights[currentLightObjectIndex];

                lightTransform.position = _coordsConverter.ToVector3(coords);
                lightTransform.gameObject.SetActive(true);
                currentLightObjectIndex++;
            }
        }

        public void TurnOffLights() => _lights.ForEach(lightTransform => lightTransform!.gameObject.SetActive(false));
    }
}
