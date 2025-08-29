using Mirror;
using SLRemake.InventorySystem.Items.Pickups;
using SLRemake.InventorySystem.Items.ViewModel;
using SLRemake.Network;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SLRemake.InventorySystem.Items
{
    public abstract class ItemBase : NetworkBehaviour
    {
        public static readonly HashSet<ItemBase> Instances = new();

        public ItemType ItemTypeId;
        public ushort ItemSerial { get; internal set; }
        public ItemPickupBase PickupBase;
        public ViewModelBase ViewModelBase;

        public static event Action<ItemBase> OnItemAdded;
        public static event Action<ItemBase> OnItemRemoved;

        public Player Owner { get; internal set; }
        public abstract float Weight { get; }
        protected virtual void Start()
        {
            OnItemAdded?.Invoke(this);
            Debug.Log("Added Instance");
            Instances.Add(this);
        }

        protected virtual void OnDestroy()
        {
            OnItemRemoved?.Invoke(this);
            Debug.Log("Destroyed Instance");
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

        public virtual void OnEquipped()
        {

        }

        public virtual void OnHolstered()
        {

        }

    }

}