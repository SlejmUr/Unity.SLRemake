using SLRemake.Network.Behaviours;
using SLRemake.Roles;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SLRemake.Network.Managers
{
    public class PlayerInputManager : PlayerBehaviour, PlayerInputAction.IPlayerActions, PlayerInputAction.IHumanActions, PlayerInputAction.ISCPActions
    {
        private PlayerInputAction Actions;
        private PlayerInputAction.PlayerActions playerActions;
        private PlayerInputAction.HumanActions humanActions;
        private PlayerInputAction.SCPActions scpActions;

        public PlayerInput PlayerInput;

        public Vector2 Move = Vector2.zero;
        public Vector2 Look;
        public bool IsSprinting;
        public bool IsCrouching;
        public bool IsAiming;
        public bool IsFiring;
        public bool IsCancelled;
        public bool IsJumping;

        public override void OnStartAuthority()
        {
            if (!isLocalPlayer)
                return;
            PlayerInput.enabled = true;
            Actions = new();
            playerActions = Actions.Player;
            playerActions.AddCallbacks(this);
            playerActions.Enable();
            humanActions = Actions.Human;
            humanActions.AddCallbacks(this);
            scpActions = Actions.SCP;
            scpActions.AddCallbacks(this);

            PlayerRoleManager.OnRoleChanged += RoleChanged;
        }

        private void RoleChanged(Player player, RoleTypeId id1, RoleTypeId id2)
        {
            if (player.RoleManager.CurrentRole is HumanRole)
                humanActions.Enable();
            else
                humanActions.Disable();
            // TODO: SCP roles.
        }

        void Update()
        {
            if (!isLocalPlayer)
                return;

            Vector2 temp = playerActions.Move.ReadValue<Vector2>();
            if (Move != temp)
                Move = temp;
            temp = playerActions.Look.ReadValue<Vector2>();
            if (Look != temp)
                Look = temp;
        }

        public void OnMove(InputAction.CallbackContext context) { }
        public void OnLook(InputAction.CallbackContext context) { }

        public void OnFire(InputAction.CallbackContext context)
        {
            IsFiring = context.ReadValueAsButton();
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            Player.InventoryManager.CmdRequestDropCurrentItem(true);
        }

        public void OnSelect(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            string key = context.control.name;

            if (byte.TryParse(key, out var value))
                Player.InventoryManager.CmdRequestSelectItem(value);
        }

        public void OnSelectScroll(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            Vector2 scroll = context.ReadValue<Vector2>();
            Player.InventoryManager.CmdRequestSelectItemScroll((int)scroll.y);
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

        public void OnDrop(InputAction.CallbackContext context)
        {
            Player.InventoryManager.CmdRequestDropCurrentItem(false);
        }

        public void OnInventory(InputAction.CallbackContext context)
        {
            
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            
        }
    }
}

