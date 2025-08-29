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
        public static HashSet<Player> AllPlayers { get; private set; } = new();
        public uint Id;

        public PlayerRoleManager RoleManager;
        public InventoryManager InventoryManager;
        public PlayerController PlayerController;
        public InputController InputController;

        private void Awake()
        {
            AllPlayers.Add(this);
            if (NetworkServer.active)
            {
                Id = netId;
                PlayerExtensions.PlayerByIds.Add(Id, this);
            }
            
        }
        private void OnDestroy()
        {
            AllPlayers.Remove(this);
            PlayerExtensions.PlayerByIds.Remove(Id);
        }

        public override void OnStartServer()
        {
            StartCoroutine(WaitForActualClientStart());
            
        }

        IEnumerator WaitForActualClientStart()
        {
            yield return new WaitForSeconds(0.2f);
            RoleManager.ServerSetRole(RoleTypeId.ClassD);
        }
    }

}