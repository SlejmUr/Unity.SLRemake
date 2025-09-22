using Mirror;
using SLRemake.Extensions;
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
            if (itemType is ItemType.None)
                return;
            Items.Add(InventoryExtension.CreateItem(itemType, serial));
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
        public void CmdRequestDropItem(int index, bool throwing)
        {
            if (Items.Count < index)
            {
                Debug.Log("Items.Count < index | " + index);
                return;
            }
            DropItem(index, throwing);
        }

        [Command]
        public void CmdRequestDropCurrentItem(bool throwing)
        {
            if (CurrentItem == null)
                return;
            DropItem(Items.IndexOf(CurrentItem), throwing);
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
            ChangeViewModel();
            item.OnEquipped();
            OnCurrentItemChangedEvent?.Invoke(Player, prevItem, CurrentItem);
            prevItem = CurrentItem;
        }

        private void DropItem(int index, bool throwing)
        {
            if (Items.Count < index)
                return;
            if (CurrentItem != null && Items.IndexOf(CurrentItem) == index)
            {
                // Deselect current item.
                SelectItem(-1);
            }
            ItemBase item = Items[index];
            ChangeViewModel();
            var pickup = InventoryExtension.CreateItemPickup(item, Player.transform.position + Player.transform.forward, Player.transform.rotation * item.PickupBase.transform.rotation);
            if (throwing)
            {
                pickup.Rb.Rb.AddForce(Player.transform.forward);
            }
            Items.Remove(item);
            InventoryExtension.RemoveItem(item, pickup);
        }


        private void ChangeViewModel()
        {
            if (CurrentViewModel != null)
                Destroy(CurrentViewModel);
            if (CurrentItem == null)
            {
                CurrentViewModel = null;
                return;
            }
            /*
            CurrentViewModel = Instantiate(CurrentItem.ViewModelBase, Player.InputController.PlayerCamera.transform);
            CurrentViewModel.InitLocal(CurrentItem);
            CurrentViewModel.OnEquipped();
            */
        }

        private void Items_OnAdded(int index)
        {
            var item = Items[index];
            item.Owner = Player;
            item.transform.SetParent(Player.transform, false);
        }
    }
}