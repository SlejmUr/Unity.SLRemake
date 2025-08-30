using Mirror;
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

        public Rigidbody Rb;

        // This need testing for real.
        public bool IsFrozen
        {
            get
            {
                if (Rb.isKinematic)
                    return Rb.IsSleeping();
                return false;
            }
            set
            {
                Rb.isKinematic = value;
                if (!value)
                    Rb.Sleep();
                else
                    Rb.WakeUp();
            }
        }

        private void OnWeightChanged(float _, float newWeight)
        {
            Rb.mass = Mathf.Max(0.001f, newWeight);
        }
    }
}