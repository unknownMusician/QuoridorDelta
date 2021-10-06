using UnityEngine;

namespace QuoridorDelta.View
{
    public class WallGameObject
    {
        public GameObject GameObject { get; private set; }
        public readonly Vector3 StartPosition;
        //public bool AtStartPosition { get; private set; }

        public WallGameObject(GameObject gameObject, Vector3 startPosition)
        {
            GameObject = gameObject;
            StartPosition = startPosition;
            //AtStartPosition = true;
        }
        public void ResetToStartPosition()
        {
            GameObject.transform.position = StartPosition;
            GameObject.transform.rotation = Quaternion.identity;
            //AtStartPosition = true;
        }
        public void PlaceWallGameObject(Vector3 newPosition, Quaternion newRotation)
        {
            GameObject.transform.position = newPosition;
            GameObject.transform.rotation = newRotation;
        }
    }
}
