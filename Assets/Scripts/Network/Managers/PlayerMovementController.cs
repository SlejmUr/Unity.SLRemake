using Mirror;
using SLRemake.Network.Behaviours;
using UnityEngine;

namespace SLRemake.Network.Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : PlayerBehaviour
    {
        public static Vector3 DefaultGravity => new(0f, -19.6f, 0f);
        [SyncVar]
        public Vector3 Gravity = DefaultGravity;

        [SyncVar]
        public float SneakSpeed = 1.6f;
        [SyncVar]
        public float WalkSpeed = 3.9f;
        [SyncVar]
        public float SprintSpeed = 5.4f;
        [SyncVar]
        public float JumpSpeed = 4.9f;

        public CharacterController characterController;
        public PlayerInputController InputController;

        [ReadOnly]
        private Vector3 velocity = Vector3.zero;

        public Vector3 Velocity => velocity;

        [HideInInspector]
        public bool CanMove = true;

        private void Update()
        {
            if (!isLocalPlayer)
                return;
            UpdateMovement();
        }

        private Vector3 WorldDirection()
        {
            Vector2 move = InputController.Move;
            return CachedTransform.TransformDirection(new Vector3(move.x, 0, move.y)).normalized;
        }

        private void HandleJump()
        {
            if (!characterController.isGrounded)
            {
                velocity += 0.5f * Time.deltaTime * Gravity;
            }
            else
            {
                velocity.y = -0.5f;

                if (InputController.IsJumping)
                {
                    velocity.y = JumpSpeed;
                }
            }
        }

        [Command]
        void UpdateMovement()
        {
            float speed = WalkSpeed;

            if (InputController.IsSprinting)
                speed = SprintSpeed;

            if (InputController.IsCrouching)
                speed = SneakSpeed;

            Vector3 world = WorldDirection();
            velocity.x = world.x * speed;
            velocity.z = world.z * speed;

            HandleJump();

            MoveVelocity(velocity * Time.deltaTime);
        }

        
        private void MoveVelocity(Vector3 vector3)
        {
            characterController.Move(vector3);
        }

        [Command]
        public void Teleport(Vector3 pos)
        {
            CachedTransform.position = pos;
        }
    }
}
