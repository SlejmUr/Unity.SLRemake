using Mirror;
using SLRemake.InventorySystem;
using SLRemake.InventorySystem.Items;
using SLRemake.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace SLRemake.Loaders;

public static class ItemLoader
{
#if !UNITY_EDITOR
    private static AssetBundle bundle;
#endif
    private static readonly Dictionary<ItemType, ItemBase> _loaded = new();
    public static bool IsLoaded { get; private set; }

    public static Dictionary<ItemType, ItemBase> Available
    {
        get
        {
            if (!IsLoaded)
            {
                ForceReload();
            }
            return _loaded;
        }
    }

    public static bool TryGetItem<T>(ItemType itemType, out T result) where T : ItemBase
    {
        if (!Available.TryGetValue(itemType, out var value) || value is not T val)
        {
            result = null;
            return false;
        }
        result = val;
        return true;
    }


    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        CustomNetworkManager.OnClientConnected += RegisterPrefabs;
    }

    private static void RegisterPrefabs()
    {
        HashSet<GameObject> hashSet = HashSetPool<GameObject>.Get();
        foreach (var value in Available.Values)
        {
            if (value.PickupBase != null && hashSet.Add(value.PickupBase.gameObject))
            {
                NetworkClient.RegisterPrefab(value.PickupBase.gameObject);
            }
        }
        Debug.Log("Successfully registered " + hashSet.Count + " pickups for " + Available.Count + " items.");
        HashSetPool<GameObject>.Release(hashSet);
    }

    public static void ForceReload()
    {
        ItemType itemType = ItemType.None;
        try
        {
            ItemBase[] Items = new ItemBase[0];
#if UNITY_EDITOR
            Items = Resources.LoadAll<ItemBase>("Items");
#else
            _loaded.Clear();
            if (bundle != null)
                bundle.Unload(true);

            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "AssetBundles");
            filePath = System.IO.Path.Combine(filePath, "items");
            bundle = AssetBundle.LoadFromFile(filePath);
            if (bundle == null)
                throw new Exception("Not found 'Items' AssetBundle");
            GameObject[] objects = bundle.LoadAllAssets<GameObject>();
            Items = objects.Where(x => x.TryGetComponent(out ItemBase _)).Select(x => x.GetComponent<ItemBase>()).ToArray();
#endif
            Array.Sort(Items, delegate (ItemBase x, ItemBase y)
            {
                int itemTypeId = (int)x.ItemTypeId;
                return itemTypeId.CompareTo((int)y.ItemTypeId);
            });
            foreach (var itemBase in Items)
            {
                if (itemBase.ItemTypeId != ItemType.None)
                {
                    itemType = itemBase.ItemTypeId;
                    _loaded[itemBase.ItemTypeId] = itemBase;
                    itemBase.OnTemplateReloaded(IsLoaded);
                }
            }
            IsLoaded = true;
        }
        catch (Exception exception)
        {
            Debug.LogError("Error while loading items from the resources folder. Last assigned item: " + itemType);
            Debug.LogException(exception);
            IsLoaded = false;
        }
    }
}
