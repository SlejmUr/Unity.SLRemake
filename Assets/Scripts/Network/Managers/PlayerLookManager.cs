using Mirror;
using SLRemake.Network.Behaviours;
using UnityEngine;

namespace SLRemake.Network.Managers
{
    public class PlayerLookManager : PlayerBehaviour
    {
        public const float LookVerticalLimit = 45.0f;
        public Camera PlayerCamera;
        public float Sensitivity = 0.1f;
        public float Smoothing = 40f;
        /// <summary>
        /// Camera movement smoothing 
        /// </summary>
        public bool EnableSmoothing = true;
        private float verticalRotation;
        private Vector2 currentLook, smoothLookVelocity;
        public override void OnStartAuthority()
        {
            if (!isLocalPlayer)
                return;
            PlayerCamera.enabled = true;
            LockCursor(true);
        }

        private void LockCursor(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
        }

        void Update()
        {
            if (!isLocalPlayer) 
                return;
            // TODO: move curor out for real!
            // Toggle cursor lock with Escape
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LockCursor(Cursor.lockState != CursorLockMode.Locked);
            }
        }

        private void LateUpdate()
        {
            if (!isLocalPlayer || !PlayerCamera.enabled || Cursor.visible) 
                return;

            var targetLook = Player.InputManager.Look;
            if (EnableSmoothing)
                currentLook = Vector2.SmoothDamp(currentLook, targetLook, ref smoothLookVelocity, 1f / Smoothing);
            else
                currentLook = targetLook;
            float mouseX = Sensitivity * currentLook.x;
            float mouseY = Sensitivity * currentLook.y;
            verticalRotation = Mathf.Clamp(verticalRotation - mouseY, -LookVerticalLimit, LookVerticalLimit);

            CachedTransform.Rotate(0, mouseX, 0);
            PlayerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }
    }
}