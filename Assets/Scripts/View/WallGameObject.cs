using QuoridorDelta.Model;
using UnityEngine;

namespace QuoridorDelta.View
{
    public class WallGameObject
    {
        public GameObject GameObject { get; private set; }
        public readonly Vector3 StartPosition;
        public readonly PlayerNumber PlayerNumber;
        public readonly Highlightable Highlightable;
        public bool AtStartPosition { get; set; } = true;

        public WallGameObject(GameObject gameObject, Vector3 startPosition, PlayerNumber playerNumber)
        {
            GameObject = gameObject;
            StartPosition = startPosition;
            PlayerNumber = playerNumber;
            Highlightable = GameObject.GetComponent<Highlightable>();
        }
        public void ResetToStartPosition()
        {
            GameObject.transform.position = StartPosition;
            GameObject.transform.rotation = Quaternion.identity;
            GameObject.layer = PlayerBehaviour.GetWallLayer(PlayerNumber);
            AtStartPosition = true;
        }
        public void PlaceWallGameObject(Vector3 newPosition, Quaternion newRotation)
        {
            GameObject.transform.position = newPosition;
            GameObject.transform.rotation = newRotation;
            GameObject.layer = LayerMask.NameToLayer("Default");
            AtStartPosition = false;
        }
    }
}
