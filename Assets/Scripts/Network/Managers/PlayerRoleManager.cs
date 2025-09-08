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
        public static Action<Player, RoleTypeId> OnRoleSet;
        public static Action<Player, RoleTypeId, RoleTypeId> OnRoleChanged;

        [SyncVar(hook = nameof(OnRoleTypeIdChanged))]
        public RoleTypeId RoleType;

        private BaseRole role;

        public MeshRenderer CapsuleRender;
        public Material MaterialTemplate;

        public BaseRole CurrentRole
        {
            get
            {
                if (role == null)
                    RoleType = RoleTypeId.None;
                return role;
            }
        }

        private void OnRoleTypeIdChanged(RoleTypeId old, RoleTypeId latest)
        {
            if (!RoleLoader.TryGetItem(latest, out BaseRole baseRole))
                throw new Exception($"{latest} not found!");
            if (role != null)
                Destroy(role.gameObject);
            BaseRole createdRole = Instantiate(baseRole, Vector3.zero, Quaternion.identity, transform);
            role = createdRole;
            CurrentRole.Init(Player);
            CapsuleRender.sharedMaterial = new(MaterialTemplate)
            {
                color = CurrentRole.RoleColor
            };
            NetworkServer.RebuildObservers(netIdentity, false);
            OnRoleSet?.Invoke(Player, latest);
            OnRoleChanged?.Invoke(Player, old, latest);
        }
    }

}