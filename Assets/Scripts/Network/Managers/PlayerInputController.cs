using Mirror;
using SLRemake.Extensions;
using SLRemake.Network.Behaviours;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SLRemake.Network.Controllers
{
    public class PlayerInputController : PlayerBehaviour, PlayerInputAction.IPlayerActions
    {
        private PlayerInputAction m_Actions;
        private PlayerInputAction.PlayerActions m_Player;

        public PlayerInput PlayerInput;

        [SyncVar]
        public Vector2 Move = Vector2.zero;
        //[SyncVar]
        public Vector2 Look;
        [SyncVar]
        public bool IsSprinting;
        [SyncVar]
        public bool IsCrouching;
        [SyncVar]
        public bool IsAiming;
        [SyncVar]
        public bool IsFiring;
        [SyncVar]
        public bool IsThrowing;
        [SyncVar]
        public bool IsCancelled;
        [SyncVar]
        public bool IsJumping;

        public override void OnStartAuthority()
        {
            if (!isLocalPlayer)
                return;
            PlayerInput.enabled = true;
            m_Actions = new();
            m_Player = m_Actions.Player;
            m_Player.AddCallbacks(this);
            m_Player.Enable();

            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            if (!isLocalPlayer)
                return;

            Vector2 temp = m_Player.Move.ReadValue<Vector2>();
            if (Move != temp)
                Move = temp;
            temp = m_Player.Look.ReadValue<Vector2>();
            if (Look != temp)
                Look = temp;
        }

        public void OnMove(InputAction.CallbackContext context)
        {

        }

        public void OnLook(InputAction.CallbackContext context)
        {

        }

        public void OnFire(InputAction.CallbackContext context)
        {
            IsFiring = context.ReadValueAsButton();
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            IsThrowing = context.ReadValueAsButton();
        }

        public void OnSelect(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            var key = context.control.name;

            if (byte.TryParse(key, out var value))
            {
                // TODO check and stuff.
                //Player.InventoryManager.CmdRequestSelectItem(value);
            }
        }

        public void OnSelectScroll(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            var scroll = context.ReadValue<Vector2>();
            switch (scroll.y)
            {
                case -1:
                    {

                    }
                    return;
                case 1:
                    {

                    }
                    return;
                default:
                    return;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            IsJumping = context.ReadValueAsButton();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            IsSprinting = context.ReadValueAsButton();
        }

        public void OnSneak(InputAction.CallbackContext context)
        {
            IsCrouching = context.ReadValueAsButton();
        }
    }
}

