using UnityEngine;

namespace PakistanStreetSimulator.CameraSystems
{
    /// <summary>
    /// Smooth third-person orbital camera with mouse pitch/yaw controls,
    /// cursor lock/unlock handling, and follow target offset.
    /// </summary>
    public class ThirdPersonCamera : MonoBehaviour
    {
        [Header("Target")]
        [Tooltip("Target transform to follow (typically the Player).")]
        [SerializeField] private Transform target;

        [Header("Offset & Distance")]
        [Tooltip("Height offset above the target pivot (e.g. eye level).")]
        [SerializeField] private Vector3 targetOffset = new Vector3(0f, 1.5f, 0f);

        [Tooltip("Distance behind the target.")]
        [SerializeField] private float distance = 3.8f;

        [Header("Sensitivity & Limits")]
        [SerializeField] private float mouseSensitivityX = 180.0f;
        [SerializeField] private float mouseSensitivityY = 120.0f;
        [SerializeField] private float minPitch = -25.0f;
        [SerializeField] private float maxPitch = 60.0f;

        [Header("Smoothing")]
        [SerializeField] private float positionSmoothTime = 0.05f;

        // Rotation & Physics state
        private float currentYaw;
        private float currentPitch = 15.0f;
        private Vector3 currentVelocity;

        private void Start()
        {
            // Lock cursor inside the game view by default
            LockCursor();

            if (target != null)
            {
                currentYaw = target.eulerAngles.y;
            }
        }

        private void LateUpdate()
        {
            if (target == null) return;

            HandleCursorInput();
            HandleMouseOrbit();
            UpdateCameraTransform();
        }

        private void HandleCursorInput()
        {
            // Press Escape to free cursor, click anywhere in Game view to re-lock
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnlockCursor();
            }
            else if (Input.GetMouseButtonDown(0) && Cursor.lockState != CursorLockMode.Locked)
            {
                LockCursor();
            }
        }

        private void HandleMouseOrbit()
        {
            if (Cursor.lockState != CursorLockMode.Locked) return;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;

            currentYaw += mouseX;
            currentPitch -= mouseY;
            currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
        }

        private void UpdateCameraTransform()
        {
            Quaternion targetRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
            Vector3 targetPivot = target.position + targetOffset;
            Vector3 desiredPosition = targetPivot - (targetRotation * Vector3.forward * distance);

            // Smooth position dampening
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, positionSmoothTime);
            transform.rotation = targetRotation;
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
