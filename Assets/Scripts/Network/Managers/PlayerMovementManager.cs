using Mirror;
using SLRemake.Network.Behaviours;
using UnityEngine;

namespace SLRemake.Network.Managers
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementManager : PlayerBehaviour
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

        //[SyncVar]
        private Vector3 velocity = Vector3.zero;

        private Vector2 serverMove;
        private bool serverJump;
        private bool serverSprint;
        private bool serverCrouch;

        [HideInInspector]
        public bool CanMove = true;

        private void Update()
        {
            if (!NetworkClient.active)
                return;
            if (!isLocalPlayer)
                return;
            serverMove = Player.InputManager.Move;
            serverJump = Player.InputManager.IsJumping;
            serverSprint = Player.InputManager.IsSprinting;
            serverCrouch = Player.InputManager.IsCrouching;
            HandleMovement();
        }

        private Vector3 WorldDirection()
        {
            Vector2 move = serverMove;
            return CachedTransform.TransformDirection(new Vector3(move.x, 0, move.y)).normalized;
        }

        private void HandleJump()
        {
            if (!characterController.isGrounded)
            {
                velocity += Time.deltaTime * Gravity;
            }
            else
            {
                if (serverJump)
                {
                    velocity.y = JumpSpeed;
                }
            }
        }

        void HandleMovement()
        {
            float speed = WalkSpeed;

            if (serverSprint)
                speed = SprintSpeed;

            if (serverCrouch)
                speed = SneakSpeed;

            Vector3 world = WorldDirection();
            velocity.x = world.x * speed;
            velocity.z = world.z * speed;

            HandleJump();

            characterController.Move(velocity * Time.deltaTime);
        }


        [Command]
        public void Teleport(Vector3 pos)
        {
            CachedTransform.position = pos;
        }
    }
}
