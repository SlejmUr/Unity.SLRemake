using UnityEngine;

namespace SLRemake.Roles
{
    public class NoneRole : BaseRole
    {
        public override RoleTypeId RoleType => RoleTypeId.None;
        public override Color RoleColor { get; } = Color.white;
    }

}