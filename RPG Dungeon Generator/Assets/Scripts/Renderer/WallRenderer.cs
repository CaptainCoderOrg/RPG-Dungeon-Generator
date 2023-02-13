using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaptainCoder.Dungeoneering
{
    public class WallRenderer : MonoBehaviour
    {
        [SerializeField]
        private Material _material;
        public Material Material 
        { 
            get => _material; 
            private set
            {
                _material = value;
                Renderer.sharedMaterial = _material;
            } 
        }
        private MeshRenderer _renderer;
        private MeshRenderer Renderer 
        { 
            get
            {
                if (_renderer == null)
                {
                    _renderer = GetComponent<MeshRenderer>();
                }
                return _renderer;
            }
        }
        private void Awake() {
            _renderer = Renderer;
        }

        private void OnValidate() {
            // Note: This is called everytime scripts compile, everytime the component changes,
            // and every time UNDO/REDO is used on this gameobject / parent.
            // Could be major performance hit. If things seem slow, make a custom editor might be
            // the way to go.
            if (Renderer.sharedMaterial == _material) { return; }
            Material = _material;
        }
        
    }
}