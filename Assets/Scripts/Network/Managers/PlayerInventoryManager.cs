using Mirror;
using SLRemake.InventorySystem;
using SLRemake.InventorySystem.Items;
using SLRemake.InventorySystem.Items.Pickups;
using SLRemake.InventorySystem.Items.ViewModel;
using SLRemake.Loaders;
using SLRemake.Network.Behaviours;
using System.Collections;
using UnityEngine;

namespace SLRemake.Network.Managers
{
    public class PlayerInventoryManager : PlayerBehaviour
    {
        public readonly SyncList<ItemBase> Items = new();
        public readonly SyncDictionary<ItemType, ushort> ReserveAmmo = new();

        public static event System.Action<Player, ItemBase, ItemBase> OnCurrentItemChangedEvent;

        [SyncVar]
        public ItemBase CurrentItem;

        private ViewModelBase CurrentViewModel;
        private ItemBase prevItem;

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
            if (Items.Count < index && index != -1)
            {
                Debug.Log("Items.Count < index || index != -1 | " + index);
                return;
            }
            SelectItem(index);
        }

        [Command]
        public void CmdRequestDropItem(int index)
        {
            if (Items.Count < index)
            {
                Debug.Log("Items.Count < index | " + index);
                return;
            }
            DropItem(index);
        }

        [Command]
        public void CmdRequestDropCurrentItem()
        {
            if (CurrentItem == null)
                return;
            DropItem(Items.IndexOf(CurrentItem));
        }

        private void SelectItem(int index)
        {
            if (CurrentItem != null)
                CurrentItem.gameObject.SetActive(false);
            if (index == -1)
            {
                CurrentItem = null;
                return;
            }
            Debug.Log(Items.Count);
            Debug.Log(index);
            ItemBase item = Items[index];
            item.gameObject.SetActive(true);
            item.OnHolstered();
            CurrentItem = item;
            if (CurrentViewModel != null)
                Destroy(CurrentViewModel);
            CurrentViewModel = Instantiate(CurrentItem.ViewModelBase, Player.InputController.PlayerCamera.transform);
            CurrentViewModel.InitLocal(CurrentItem);
            CurrentViewModel.OnEquipped();
            item.OnEquipped();
            OnCurrentItemChangedEvent?.Invoke(Player, prevItem, CurrentItem);
            prevItem = CurrentItem;
        }

        private void DropItem(int index)
        {
            if (Items.Count < index)
                return;
            if (CurrentItem != null && Items.IndexOf(CurrentItem) == index)
            {
                SelectItem(index);
            }
            ItemBase item = Items[index];
            if (CurrentViewModel != null)
                Destroy(CurrentViewModel);
            CurrentViewModel = null;
            var pickup = CreateItemPickup(item);
            pickup.transform.SetPositionAndRotation(Player.transform.position + Player.transform.forward, Player.transform.rotation * item.PickupBase.transform.rotation);
            Items.Remove(item);
            item.OnRemoved(pickup);
            NetworkServer.Destroy(item.gameObject);
        }

        private void Items_OnAdded(int index)
        {
            var item = Items[index];
            item.Owner = Player;
            item.transform.SetParent(Player.transform, false);
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
            item.gameObject.SetActive(false);
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