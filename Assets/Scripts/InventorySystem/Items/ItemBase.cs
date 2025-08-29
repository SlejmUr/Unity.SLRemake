using Mirror;
using SLRemake.InventorySystem.Items.Pickups;
using System;
using System.Collections.Generic;

namespace SLRemake.InventorySystem.Items
{
    public abstract class ItemBase : NetworkBehaviour
    {
        public static readonly HashSet<ItemBase> Instances = new();

        public ItemType ItemTypeId;
        public ushort ItemSerial { get; internal set; }
        public ItemPickupBase PickupBase;

        public static event Action<ItemBase> OnItemAdded;
        public static event Action<ItemBase> OnItemRemoved;
        protected virtual void Start()
        {
            OnItemAdded?.Invoke(this);
            Instances.Add(this);
        }

        protected virtual void OnDestroy()
        {
            OnItemRemoved?.Invoke(this);
            Instances.Remove(this);
        }

        public virtual void OnAdded(ItemPickupBase pickup)
        {
        }

        public virtual void OnRemoved(ItemPickupBase pickup)
        {
        }

        public virtual void OnTemplateReloaded(bool loaded)
        {
            
        }
    }

}