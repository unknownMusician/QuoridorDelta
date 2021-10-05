using System.Collections.Generic;
using QuoridorDelta.Model;
using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class Backlight
    {
        private const int MaxPossibleLightsCount = 6;
        private readonly CoordsConverter _coordsConverter;

        private readonly List<RectTransform> _lights = new List<RectTransform>();


        public Backlight(CoordsConverter coordsConverter, GameObject lightPrefab, Transform lightsParent)
        {
            _coordsConverter = coordsConverter;

            for (int i = 0; i < MaxPossibleLightsCount; i++)
            {
                _lights.Add(Object.Instantiate(lightPrefab, lightsParent).GetComponent<RectTransform>());
            }
        }

        public void TurnOnLightOnCells(IEnumerable<Coords> possibleMoves)
        {
            int currentLightObjectIndex = 0;

            foreach (Coords coords in possibleMoves)
            {
                _lights[currentLightObjectIndex].position = _coordsConverter.ToVector3(coords);
                _lights[currentLightObjectIndex].gameObject.SetActive(true);
                currentLightObjectIndex++;
            }
        }

        public void TurnOffLights()
        {
            foreach (RectTransform lightObject in _lights)
            {
                lightObject.gameObject.SetActive(false);
            }
        }
    }
}