using Mirror;
using SLRemake.InventorySystem;
using SLRemake.InventorySystem.Items;
using SLRemake.InventorySystem.Items.Pickups;
using SLRemake.Loaders;
using SLRemake.Network.Behaviours;
using System.Collections;
using UnityEngine;

namespace SLRemake.Network.Managers
{
    public class InventoryManager : PlayerBehaviour
    {
        public readonly SyncList<ItemBase> Items = new();
        public readonly SyncDictionary<ItemType, ushort> ReserveAmmo = new();

        [SyncVar]
        public ItemBase CurrentItem;

        public override void OnStartAuthority()
        {
            Items.OnAdd += Items_OnAdded;
            StartCoroutine(WaitThenAddItem());
        }

        IEnumerator WaitThenAddItem()
        {
            yield return new WaitForSeconds(2f);
            if (!NetworkServer.active)
                yield break;
            CmdRequestItem(ItemType.Test, 0);

        }

        [Command]
        public void CmdRequestItem(ItemType itemType, ushort serial)
        {
            Items.Add(CreateItem(itemType, serial));
        }

        [Command]
        public void CmdRequestSelectItem(int index)
        {
            if (Items.Count > index)
                return;
            SelectItem(index);
        }

        [Command]
        public void CmdRequestDropItem(int index)
        {
            if (Items.Count > index)
                return;
            DropItem(Items[index]);
        }

        [Command]
        public void CmdRequestDropCurrentItem()
        {
            if (CurrentItem == null)
                return;
            DropItem(CurrentItem);
        }

        private void SelectItem(int index)
        {
            var item = Items[index];
            item.OnEquipped();
            CurrentItem = item;
        }

        private void DropItem(ItemBase item)
        {
            if (item == null)
                return;
            CurrentItem = null;
            var pickup = CreateItemPickup(item);
            pickup.transform.SetPositionAndRotation(Player.transform.position, Player.transform.rotation * item.PickupBase.transform.rotation);
            Items.Remove(item);
        }

        private void Items_OnAdded(int index)
        {
            Items[index].Owner = Player;
        }

        // TODO: Move these outside!

        public ItemBase CreateItem(ItemType itemType, ushort? serial = null)
        {
            if (!NetworkServer.active)
                throw new System.Exception("Server Only");
            if (!serial.HasValue || serial == 0)
                serial = ItemSerialGenerator.GenerateNext();
            if (!ItemLoader.TryGetItem(itemType, out ItemBase itemBase))
                throw new System.Exception($"{itemType} not found!");
            ItemBase item = Instantiate(itemBase);
            item.ItemSerial = serial.Value;
            NetworkServer.Spawn(item.gameObject);
            return item;
        }

        public ItemPickupBase CreateItemPickup(ItemType itemType, ushort? serial = null)
        {
            if (!NetworkServer.active)
                throw new System.Exception("Server Only");
            if (!serial.HasValue || serial == 0)
                serial = ItemSerialGenerator.GenerateNext();
            if (!ItemLoader.TryGetItem(itemType, out ItemBase itemBase))
                throw new System.Exception($"{itemType} not found!");
            var item = Instantiate(itemBase.PickupBase);
            item.ItemType = itemBase.ItemTypeId;
            item.Weight = itemBase.Weight;
            item.Serial = serial.Value;
            NetworkServer.Spawn(item.gameObject);
            return item;
        }

        public ItemPickupBase CreateItemPickup(ItemBase itemBase)
        {
            if (!NetworkServer.active)
                throw new System.Exception("Server Only");
            var item = Instantiate(itemBase.PickupBase);
            item.ItemType = itemBase.ItemTypeId;
            item.Weight = itemBase.Weight;
            item.Serial = itemBase.ItemSerial;
            NetworkServer.Spawn(item.gameObject);
            item.gameObject.GetComponent<Rigidbody>().WakeUp();
            return item;
        }
    }
}