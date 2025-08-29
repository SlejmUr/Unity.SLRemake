using UnityEngine;

namespace SLRemake.Roles
{
    public class HumanRole : BaseRole
    {
        [SerializeField]
        private RoleTypeId roleType;
        [SerializeField]
        private Color roleColor;
        public override RoleTypeId RoleType => roleType;
        public override Color RoleColor => roleColor;
    }
}