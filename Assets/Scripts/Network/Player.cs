using Mirror;
using SLRemake.Extensions;
using SLRemake.Network.Controllers;
using SLRemake.Network.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLRemake.Network
{
    public class Player : NetworkBehaviour
    {
        private static int id;
        public static HashSet<Player> AllPlayers { get; private set; } = new();

        [SyncVar, ReadOnly]
        public int Id;

        public PlayerRoleManager RoleManager;
        public PlayerInventoryManager InventoryManager;
        public PlayerMovementController MovementController;
        public PlayerInputController InputController;
        public PlayerLookManager LookManager;

        private void Awake()
        {
            AllPlayers.Add(this);
            PlayerExtensions.PlayerByGameObject[gameObject] = this;
            if (NetworkServer.active)
            {
                Id = ++id;
            }
        }

        private void OnDestroy()
        {
            AllPlayers.Remove(this);
            PlayerExtensions.PlayerByGameObject.Remove(gameObject);
        }
    }

}