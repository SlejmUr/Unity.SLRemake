using Mirror;
using UnityEngine;

namespace SLRemake.Network.Controllers
{
   
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : NetworkBehaviour
    {
        public static Vector3 DefaultGravity => new(0f, -19.6f, 0f);
        [SyncVar]
        public Vector3 Gravity = DefaultGravity;

        [SyncVar]
        public float CrouchSpeed = 0f;
        [SyncVar]
        public float SneakSpeed = 1.6f;
        [SyncVar]
        public float WalkSpeed = 3.9f;
        [SyncVar]
        public float SprintSpeed = 5.4f;
        [SyncVar]
        public float JumpSpeed = 4.9f;

        public CharacterController characterController;
        Vector3 moveDirection = Vector3.zero;

        [HideInInspector]
        public bool CanMove = true;

        void Update()
        {
            if (!isLocalPlayer)
                return;

            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            // Press Left Shift to run
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = CanMove ? (isRunning ? SprintSpeed : WalkSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = CanMove ? (isRunning ? SprintSpeed : WalkSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && CanMove && characterController.isGrounded)
            {
                moveDirection.y = JumpSpeed;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            if (!characterController.isGrounded)
            {
                moveDirection += 0.5f * Time.deltaTime * Gravity;
            }

            characterController.Move(moveDirection * Time.deltaTime);
        }
    }
}
