using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpriteManager : ManagerBase, ISpriteManager
{
    [Inject] private IResourceManager ResourceManager { get; set; }
    [Inject] private IPoolManager PoolManager { get; set; }
    private string SpriteConfigPath = "Assets/GameResource/Config/UIRawConfig/UIRawConfig.prefab";
    private readonly Dictionary<string, string> _spriteNameToPath = new();
    
    private readonly Dictionary<string, Sprite> _spriteMap = new();
    protected override IEnumerator OnInit()
    {
        var spriteConfig = this.ResourceManager.Load<GameObject>(SpriteConfigPath);
        var imgDict = spriteConfig.GetComponent<ImageDictBehaviour>().GetImageDictionary();
        foreach (var kv in imgDict)
        {
            _spriteNameToPath[kv.Key] = kv.Value;
        }
        yield break;
    }
    // 根据名称获取图片路径
    public string GetImagePath(string imageName)
    {
        return _spriteNameToPath.TryGetValue(imageName, out string path) ? path : null;
    }

    public Sprite GetSprite(string spriteName)
    {
        if (_spriteNameToPath.TryGetValue(spriteName, out string path))
        {
            if (!_spriteMap.TryGetValue(path, out var sprite))
            {
                sprite = ResourceManager.Load<Sprite>(path);
                _spriteMap.Add(path, sprite);
            }
            return _spriteMap[path];
        }

        return null;
    }
}
