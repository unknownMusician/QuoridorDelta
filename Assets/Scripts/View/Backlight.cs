using System;
using System.Collections.Generic;
using UnityEngine;
using QuoridorDelta.Model;

namespace QuoridorDelta.View
{
    public sealed class Backlight
    {
        private const int MaxPossibleLightsCount = 6;
        private GameObject _lightPrefab;
        private CoordsConverter _coordsConverter;
        private GameObject _lightsParent;

        private List<RectTransform> _lights = new List<RectTransform>();


        public Backlight(CoordsConverter coordsConverter, GameObject lightPrefab, GameObject lightsParent)
        {
            _coordsConverter = coordsConverter;
            _lightPrefab = lightPrefab;
            _lightsParent = lightsParent;
            for (int i = 0; i < MaxPossibleLightsCount; i++)
            {
                _lights.Add(GameObject.Instantiate(_lightPrefab, _lightsParent.transform).GetComponent<RectTransform>());
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
