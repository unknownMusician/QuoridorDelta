using System.Collections.Generic;
using UnityEngine;

namespace QuoridorDelta.View
{
    public sealed class Highlightable : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer = default!;
        [SerializeField] private Material _changedMaterial = default!;

        private Material[]? _defaultMaterials;
        private Material[]? _changedMaterials;

        private void Awake()
        {
            _defaultMaterials = _renderer.materials;
            _changedMaterials = new List<Material>(_renderer.materials) { _changedMaterial }.ToArray();
        }

        public void Change(bool highlited) => _renderer.materials = highlited ? _changedMaterials : _defaultMaterials;
    }
}
