using Mirror;
using UnityEngine;

namespace SLRemake.InventorySystem.Items
{
    public class TestItem : ItemBase
    {
        [SyncVar]
        public float MainWeight;
        public override float Weight => MainWeight;
        public override void OnEquipped()
        {
            base.OnEquipped();
            Debug.Log("AAAA TEST ITEM EQUIPEDDEDDDED!");
        }
    }

}