using Mirror;
using SLRemake.InventorySystem;
using SLRemake.InventorySystem.Items;
using SLRemake.InventorySystem.Items.Pickups;
using SLRemake.Loaders;
using UnityEngine;

namespace SLRemake.Extensions
{
    public static class InventoryExtension
    {
        public static ItemBase CreateItem(ItemType itemType, ushort? serial = null, bool active = false)
        {
            if (!NetworkServer.active)
                throw new System.Exception("Server Only");
            if (!ItemLoader.TryGetItem(itemType, out ItemBase itemBase))
                throw new System.Exception($"{itemType} not found!");
            return CreateItem(itemBase, serial, active);
        }

        public static ItemBase CreateItem(ItemBase template, ushort? serial = null, bool active = false)
        {
            if (!NetworkServer.active)
                throw new System.Exception("Server Only");
            if (!serial.HasValue || serial == 0)
                serial = ItemSerialGenerator.GenerateNext();
            ItemBase item = Object.Instantiate(template);
            item.ItemSerial = serial.Value;
            NetworkServer.Spawn(item.gameObject);
            item.gameObject.SetActive(active);
            return item;
        }

        public static ItemPickupBase CreateItemPickup(ItemType itemType, ushort? serial = null, Vector3? position = null, Quaternion? rotation = null)
        {
            if (!NetworkServer.active)
                throw new System.Exception("Server Only");
            if (!ItemLoader.TryGetItem(itemType, out ItemBase itemBase))
                throw new System.Exception($"{itemType} not found!");
            return CreateItemPickup(itemBase.PickupBase, itemBase.Weight, serial, position, rotation);
        }

        public static ItemPickupBase CreateItemPickup(ItemBase itemBase, Vector3? position = null, Quaternion? rotation = null)
        {
            if (!NetworkServer.active)
                throw new System.Exception("Server Only");
            return CreateItemPickup(itemBase.PickupBase, itemBase.Weight, itemBase.ItemSerial, position, rotation);
        }

        public static ItemPickupBase CreateItemPickup(ItemPickupBase template, float weight, ushort? serial = null, Vector3? position = null, Quaternion? rotation = null)
        {
            if (!NetworkServer.active)
                throw new System.Exception("Server Only");
            if (!serial.HasValue || serial == 0)
                serial = ItemSerialGenerator.GenerateNext();
            if (position == null)
                position = Vector3.zero;
            if (rotation == null)
                rotation = Quaternion.identity;
            ItemPickupBase pickupBase = Object.Instantiate(template, position.Value, rotation.Value);
            NetworkServer.Spawn(pickupBase.gameObject);
            //item.ItemType = itemBase.ItemTypeId;
            pickupBase.Weight = weight;
            pickupBase.Serial = serial.Value;
            pickupBase.gameObject.GetComponent<Rigidbody>().WakeUp();
            return pickupBase;
        }

        public static void RemoveItem(ItemBase itemBase, ItemPickupBase pickupBase = null)
        {
            if (itemBase.gameObject == null)
                return;
            itemBase.OnRemoved(pickupBase);
            NetworkServer.Destroy(itemBase.gameObject);
        }
    }
}