﻿using System.Collections.Generic;
using JetBrains.Annotations;
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
