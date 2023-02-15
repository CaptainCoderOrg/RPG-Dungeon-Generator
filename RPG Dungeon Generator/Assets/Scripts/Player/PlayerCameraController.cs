using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaptainCoder.Dungeoneering
{
    
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField]
        private Facing _onDirection = Facing.North;
        
        public void HandleFacing(Facing f) => gameObject.SetActive(f == _onDirection);
    }
}