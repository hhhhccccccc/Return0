// 附加到预制体上的脚本组件

using System.Collections.Generic;
using UnityEngine;

public class ImageDictBehaviour : MonoBehaviour
{
    [SerializeField]
    private List<string> imageNames = new List<string>();
    [SerializeField]
    private List<string> imagePaths = new List<string>();

    // 运行时使用的字典
    private Dictionary<string, string> imageDictionary;

    // 编辑器初始化方法
    public void Initialize(Dictionary<string, string> dict)
    {
        imageDictionary = new Dictionary<string, string>(dict);
        
        // 序列化存储
        imageNames.Clear();
        imagePaths.Clear();
        foreach (var kvp in dict)
        {
            imageNames.Add(kvp.Key);
            imagePaths.Add(kvp.Value);
        }
    }

    // 运行时访问字典
    public Dictionary<string, string> GetImageDictionary()
    {
        if (imageDictionary == null || imageDictionary.Count == 0)
        {
            BuildRuntimeDictionary();
        }
        return imageDictionary;
    }

    private void BuildRuntimeDictionary()
    {
        imageDictionary = new Dictionary<string, string>();
        for (int i = 0; i < imageNames.Count && i < imagePaths.Count; i++)
        {
            imageDictionary[imageNames[i]] = imagePaths[i];
        }
    }

    // 根据名称获取图片路径
    public string GetImagePath(string imageName)
    {
        if (imageDictionary == null) BuildRuntimeDictionary();
        return imageDictionary.TryGetValue(imageName, out string path) ? path : null;
    }

    // 获取所有图片名称
    public List<string> GetAllImageNames()
    {
        if (imageNames.Count == 0 && imageDictionary != null)
        {
            return new List<string>(imageDictionary.Keys);
        }
        return imageNames;
    }
}