using Mirror;
using SLRemake.Network;
using System;
using UnityEngine;

namespace SLRemake.InventorySystem.Items.Pickups
{
    public abstract class ItemPickupBase : NetworkBehaviour
    {
        [SyncVar]
        public ItemType ItemType;
        [SyncVar]
        public ushort Serial;
        [SyncVar]
        public float Weight;
    }

}