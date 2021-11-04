using System;
using JetBrains.Annotations;
using QuoridorDelta.Model;
using UnityEngine;
using UnityEngine.UI;

namespace QuoridorDelta.View
{
    public sealed class WinnerWindow : MonoBehaviour
    {
        [SerializeField] private GameObject _windowUI = default!;
        [SerializeField] private Text _winnerText = default!;

        [NotNull] private const string WinnerFormatText = "Player {0} is a Winner!";

        public void Show(PlayerNumber playerNumber)
        {
            _winnerText.text = string.Format(WinnerFormatText, ToInt(playerNumber));
            _windowUI.SetActive(true);
        }
        
        private static int ToInt(PlayerNumber playerNumber) => playerNumber switch
        {
            PlayerNumber.First => 1,
            PlayerNumber.Second => 2,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
