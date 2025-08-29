using Mirror;
using SLRemake.Network;
using UnityEngine;

namespace SLRemake.InventorySystem.Items.ViewModel
{
    public class ViewModelBase : MonoBehaviour
    {
        public Player Owner { get; protected set; }
        public ItemBase ParentItem { get; protected set; }

        public bool IsLocal { get; private set; }

        public bool IsSpectator { get; private set; }
        public void InitLocal(ItemBase itemBase)
        {
            ParentItem = itemBase;
            Owner = itemBase.Owner;
            IsLocal = true;
            IsSpectator = false;
        }
    }

}