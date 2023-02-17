using UnityEngine;
using Cinemachine;
using static UnityEngine.InputSystem.InputAction;

namespace CaptainCoder.Dungeoneering
{
    [RequireComponent(typeof(CinemachineFreeLook))]
    public class CameraGamepadController : MonoBehaviour
    {
        public CinemachineFreeLook FreeLookCamera { get; private set; }
        public Vector2 InputVector { get; private set; } = Vector2.zero;
        [field: SerializeField]
        public float RotationSpeed { get; private set; } = 100f;
        [field: SerializeField]
        public float TiltSpeed { get; private set; } = 3f;

        protected void Update()
        {
            // FreeLookCamera.m_XAxis.Value += InputVector.x * Time.deltaTime * RotationSpeed; // * (CameraControls.InvertXAxis ? -1 : 1);
            // FreeLookCamera.m_YAxis.Value += InputVector.y * Time.deltaTime * TiltSpeed; // * (CameraControls.InvertYAxis ? -1 : 1);
        }

        protected void Awake()
        {
            FreeLookCamera = GetComponent<CinemachineFreeLook>();
            // CameraZoom = GetComponent<CameraZoom>();
            // _gameCamera = GetComponent<GameCamera>();
        }

        protected void OnEnable() {
            Controls.CameraActions.FreeLook.performed += HandleCameraRotation;
            Controls.CameraActions.FreeLook.canceled += StopCameraRotation;
        }

        protected void OnDisable() {
            Controls.CameraActions.FreeLook.performed -= HandleCameraRotation;
            Controls.CameraActions.FreeLook.canceled -= StopCameraRotation;
        }

        // private void StartZoomIn(CallbackContext context) => _zoomIn = -1;
        // private void StopZoomIn(CallbackContext context) => _zoomIn = 0;
        // private void StartZoomOut(CallbackContext context) => _zoomOut = 1;
        // private void StopZoomOut(CallbackContext context) => _zoomOut = 0;
        private void HandleCameraRotation(CallbackContext context)
        {
            InputVector = context.ReadValue<Vector2>();
            // _gameCamera.IsMoving = true;
        }
        private void StopCameraRotation(CallbackContext context)
        {
            InputVector = Vector2.zero;
            // _gameCamera.IsMoving = false;
        }
    }
}