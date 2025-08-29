using SLRemake.Network;
using UnityEngine;

namespace SLRemake.Roles
{
    public abstract class BaseRole : MonoBehaviour
    {
        private Player lastPlayer;
        public abstract RoleTypeId RoleType { get; }
        public abstract Color RoleColor { get; }

        internal virtual void Init(Player player)
        {
            lastPlayer = player;
        }

        public bool TryGetOwner(out Player player)
        {
            player = lastPlayer;
            return player != null;
        }
    }

}