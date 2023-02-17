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

        protected void Awake()
        {
            PlayerPositionController controller = GetComponentInParent<PlayerPositionController>();
            Debug.Assert(controller != null, "PlayerCameraController must be nested inside of a PlayerPositionController");
            controller?.OnChangeFacing.AddListener(HandleFacing);
            HandleFacing(controller.Facing);
        }
    }
}