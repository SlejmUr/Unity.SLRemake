using SLRemake.Network;
using SLRemake.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SLRemake.Loaders;

public static class RoleLoader
{
#if !UNITY_EDITOR
    private static AssetBundle bundle;
#endif
    private static readonly Dictionary<RoleTypeId, BaseRole> _loaded = new();
    public static bool IsLoaded { get; private set; } = false;

    public static Dictionary<RoleTypeId, BaseRole> Available
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

    public static bool TryGetItem<T>(RoleTypeId type, out T result) where T : BaseRole
    {
        if (!Available.TryGetValue(type, out var value) || value is not T val)
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
        CustomNetworkManager.OnClientConnected += ForceReload;
    }
    public static void ForceReload()
    {
        try
        {
            BaseRole[] Roles = new BaseRole[0];
#if UNITY_EDITOR
            Roles = Resources.LoadAll<BaseRole>("Roles");
#else
            _loaded.Clear();
            if (bundle != null)
                bundle.Unload(true);

            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "AssetBundles");
            filePath = System.IO.Path.Combine(filePath, "roles");
            bundle = AssetBundle.LoadFromFile(filePath);
            if (bundle == null)
                throw new Exception("Not found 'Roles' AssetBundle");
            GameObject[] objects = bundle.LoadAllAssets<GameObject>();
            Roles = objects.Where(x => x.TryGetComponent(out BaseRole _)).Select(x => x.GetComponent<BaseRole>()).ToArray();
#endif

            foreach (var item in Roles)
            {
                Debug.Log(item.RoleType);
                _loaded.TryAdd(item.RoleType, item);
            }
            IsLoaded = true;
        }
        catch (Exception exception)
        {
            Debug.LogError("Error while loading items from the resources folder. ");
            Debug.LogException(exception);
            IsLoaded = false;
        }
    }
}