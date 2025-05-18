using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;
using Object = UnityEngine.Object;

public class ResourceManager : IManager
{
    private ResourcePackage Package;

    private Dictionary<string, GameObject> Resources = new();
    
    public T Load<T>(string path) where T : Object
    {
        return YooAssets.LoadAssetSync<T>(path).GetAssetObject<T>();
    }

    public IEnumerator<T> LoadParallel<T>(string path) where T : Object
    {
        var handle = YooAssets.LoadAssetAsync<T>(path);
        yield return handle.AssetObject as T;
    }

    public async Task<T> LoadAsync<T>(string path) where T : Object
    {
        var handle = YooAssets.LoadAssetAsync<T>(path);
        await handle.Task;
        return handle.AssetObject as T;
    }

    public void Unload(string packageName)
    {
        var package = YooAssets.TryGetPackage(packageName);
        package?.UnloadUnusedAssetsAsync();
    }

    public IEnumerator Init()
    {
        yield break;
    }
}
