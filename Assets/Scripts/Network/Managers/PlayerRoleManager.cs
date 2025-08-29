using Mirror;
using SLRemake.Loaders;
using SLRemake.Network.Behaviours;
using SLRemake.Roles;
using System;
using UnityEngine;

namespace SLRemake.Network.Managers
{
    public class PlayerRoleManager : PlayerBehaviour
    {
        public static Action<Player, RoleTypeId> OnServerRoleSet;

        private bool _anySet;

        private BaseRole role;

        public MeshRenderer CapsuleRender;
        public Material MaterialTemplate;

        public BaseRole CurrentRole
        {
            get
            {
                if (!_anySet)
                    ServerSetRole(RoleTypeId.None);
                return role;
            }
            set
            {
                role = value;
                _anySet = true;
            }
        }

        [Server]
        public void ServerSetRole(RoleTypeId roleTypeId)
        {
            InitializeNewRole(roleTypeId);
            OnServerRoleSet?.Invoke(Player, roleTypeId);  
        }

        public void InitializeNewRole(RoleTypeId roleTypeId)
        {
            if (!RoleLoader.TryGetItem(roleTypeId, out BaseRole baseRole))
                throw new Exception($"{roleTypeId} not found!");

            Transform transform = baseRole.transform;
            transform.parent = base.transform;
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            CurrentRole = baseRole;
            CurrentRole.Init(Player);
            CapsuleRender.sharedMaterial = new(MaterialTemplate)
            {
                color = CurrentRole.RoleColor
            };
            NetworkServer.RebuildObservers(netIdentity, false);
        }
    }

}