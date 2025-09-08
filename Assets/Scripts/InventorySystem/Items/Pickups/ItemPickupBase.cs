using Mirror;
using SLRemake.Network.Behaviours;
using UnityEngine;

namespace SLRemake.InventorySystem.Items.Pickups
{
    public abstract class ItemPickupBase : NetworkBehaviour
    {
        [SyncVar]
        public ItemType ItemType;
        [SyncVar]
        public ushort Serial;
        [SyncVar(hook = nameof(OnWeightChanged))]
        public float Weight;

        public RigidbodyNetworkSync Rb;

        private void OnWeightChanged(float _, float newWeight)
        {
            Rb.Rb.mass = Mathf.Max(0.001f, newWeight);
        }
    }
}