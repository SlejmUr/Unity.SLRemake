using Mirror;
using SLRemake.Extensions;
using SLRemake.Network.Behaviours;
using UnityEngine;

namespace SLRemake.Network.Controllers
{
    public class InputController : PlayerBehaviour
    {
        public Camera playerCamera;
        public float lookSpeed = 2.0f;
        public float lookXLimit = 45.0f;
        float rotationX = 0;

        [HideInInspector]
        public bool CanMove = true;

        public override void OnStartAuthority()
        {
            if (!isLocalPlayer)
                return;

            playerCamera.enabled = true;
            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log(Player == null);
        }

        void Update()
        {
            if (!isLocalPlayer)
                return;

            if (!CanMove)
                return;

            if (Input.GetKey(KeyCode.Escape))
            {
                Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = !Cursor.visible;
            }

            if (Cursor.visible)
                return;

            if (Input.GetKey(KeyCode.Q))
            {
                Player.InventoryManager.CmdRequestDropCurrentItem();
            }

            if (Input.GetKey(KeyCode.F))
            {
                int index = Player.InventoryManager.Items.RandomIndex();
                Player.InventoryManager.CmdRequestSelectItem(index);
            }

            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}

