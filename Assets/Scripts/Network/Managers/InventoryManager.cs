using Mirror;
using SLRemake.InventorySystem;
using SLRemake.InventorySystem.Items;
using SLRemake.Network.Behaviours;

namespace SLRemake.Network.Managers
{
    public class InventoryManager : PlayerBehaviour
    {
        public readonly SyncHashSet<ItemBase> Items = new();
        public readonly SyncDictionary<ItemType, ushort> ReserveAmmo = new();
    }
}