using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace QuoridorDelta.View
{
    public sealed class Ghostable : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _changedMaterial;
        private Material _defaultMaterial;

        private void Awake() => _defaultMaterial = _renderer.material;

        public void Change(bool ghosted) => _renderer.material = ghosted ? _changedMaterial : _defaultMaterial;
    }
}
