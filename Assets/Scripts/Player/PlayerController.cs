using UnityEngine;

namespace PakistanStreetSimulator.Player
{
    /// <summary>
    /// Handles 3D third-person character movement, sprinting, jumping, and ground physics
    /// using Unity's built-in CharacterController component.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Speeds")]
        [Tooltip("Walking speed in meters per second.")]
        [SerializeField] private float walkSpeed = 4.5f;

        [Tooltip("Running speed when holding Left Shift.")]
        [SerializeField] private float sprintSpeed = 8.0f;

        [Tooltip("Smooth rotation turning time towards movement direction.")]
        [SerializeField] private float rotationSmoothTime = 0.1f;

        [Header("Jumping & Gravity")]
        [Tooltip("Jump height in meters.")]
        [SerializeField] private float jumpHeight = 1.2f;

        [Tooltip("Gravity acceleration multiplier (standard realistic gravity: -18.0f).")]
        [SerializeField] private float gravity = -18.0f;

        [Header("Ground Check")]
        [Tooltip("Transform placed at the feet of the player for ground detection.")]
        [SerializeField] private Transform groundCheck;

        [Tooltip("Radius of the ground check sphere.")]
        [SerializeField] private float groundDistance = 0.3f;

        [Tooltip("Layers considered as ground.")]
        [SerializeField] private LayerMask groundMask;

        // Component References
        private CharacterController characterController;
        private Transform mainCameraTransform;

        // Movement Physics State
        private Vector3 verticalVelocity;
        private bool isGrounded;
        private float currentTurnVelocity;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();

            if (Camera.main != null)
            {
                mainCameraTransform = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning("[PlayerController] Main Camera not found at Awake! Ensure MainCamera has tag 'MainCamera'.");
            }
        }

        private void Update()
        {
            // If camera reference was missed during awake, try to re-acquire
            if (mainCameraTransform == null && Camera.main != null)
            {
                mainCameraTransform = Camera.main.transform;
            }

            CheckGroundedStatus();
            HandleMovement();
            HandleGravityAndJump();
        }

        private void CheckGroundedStatus()
        {
            if (groundCheck != null)
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            }
            else
            {
                isGrounded = characterController.isGrounded;
            }

            // Keep the character grounded smoothly without accumulating negative gravity
            if (isGrounded && verticalVelocity.y < 0)
            {
                verticalVelocity.y = -2.0f;
            }
        }

        private void HandleMovement()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

            if (inputDirection.magnitude >= 0.1f)
            {
                float cameraYaw = (mainCameraTransform != null) ? mainCameraTransform.eulerAngles.y : 0f;
                float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraYaw;
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentTurnVelocity, rotationSmoothTime);

                // Rotate player towards movement direction
                transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

                // Move in rotated direction
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

                characterController.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
            }
        }

        private void HandleGravityAndJump()
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                // Velocity formula: v = sqrt(height * -2 * gravity)
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            }

            // Apply gravity over time
            verticalVelocity.y += gravity * Time.deltaTime;

            // Move character vertically
            characterController.Move(verticalVelocity * Time.deltaTime);
        }

        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = isGrounded ? Color.green : Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
            }
        }
    }
}
