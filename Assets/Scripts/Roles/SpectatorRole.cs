using UnityEngine;

namespace SLRemake.Roles
{
    public class SpectatorRole : BaseRole
    {
        public override Color RoleColor { get; } = Color.white;
        public override RoleTypeId RoleType =>  RoleTypeId.Spectator;
    }

}