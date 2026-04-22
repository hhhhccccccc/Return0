 using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class ImageDictToPrefab : EditorWindow
{
    private static string sourceFolder = "Assets/UIRaw"; // 图片源文件夹
    private static string prefabFolder = "Assets/GameResource/Config/UIRawConfig"; // 预制体输出文件夹
    private static string prefabName = "UIRawConfig"; // 预制体名称
 
    [MenuItem("Tools/一键生成图片字典预制体")]
    private static void GeneratePrefabWithDict()
    {
        // 1. 确保文件夹存在
        if (!Directory.Exists(sourceFolder))
        {
            Debug.LogError($"源文件夹不存在: {sourceFolder}");
            return;
        }

        if (!Directory.Exists(prefabFolder))
        {
            Directory.CreateDirectory(prefabFolder);
        }

        // 2. 获取所有图片（支持常见格式）
        string[] supportedExtensions = { ".png", ".jpg", ".jpeg", ".tga", ".bmp", ".psd", ".gif" };
        List<string> imagePaths = new List<string>();

        foreach (string ext in supportedExtensions)
        {
            string[] files = Directory.GetFiles(sourceFolder, "*" + ext, SearchOption.AllDirectories);
            imagePaths.AddRange(files);
        }

        if (imagePaths.Count == 0)
        {
            Debug.LogWarning($"在 {sourceFolder} 中没有找到图片文件");
            return;
        }

        // 3. 构建字典数据（相对路径用于Unity）
        Dictionary<string, string> imageDict = new Dictionary<string, string>();
        foreach (string fullPath in imagePaths)
        {
            string relativePath = GetRelativePath(fullPath);
            string fileName = Path.GetFileNameWithoutExtension(fullPath);
            imageDict[fileName] = relativePath;
        }

        // 4. 创建临时GameObject用于保存数据
        GameObject tempGO = new GameObject(prefabName);
        ImageDictBehaviour behaviour = tempGO.AddComponent<ImageDictBehaviour>();
        behaviour.Initialize(imageDict);

        // 直接覆盖保存预制体
        string prefabPath = Path.Combine(prefabFolder, prefabName + ".prefab");
        
        // 如果预制体已存在，直接覆盖
        if (File.Exists(prefabPath))
        {
            // 删除旧的预制体文件
            AssetDatabase.DeleteAsset(prefabPath);
        }
        
        PrefabUtility.SaveAsPrefabAsset(tempGO, prefabPath);
        DestroyImmediate(tempGO);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"预制体已生成: {prefabPath}，包含 {imageDict.Count} 张图片信息");
        
        // 可选：输出到控制台查看
        foreach (var kvp in imageDict)
        {
            Debug.Log($"图片: {kvp.Key} -> {kvp.Value}");
        }
    }

    private static string GetRelativePath(string fullPath)
    {
        // 转换为Unity相对路径 (Assets/...)
        string dataPath = Application.dataPath;
        if (fullPath.StartsWith(dataPath))
        {
            return "Assets" + fullPath.Substring(dataPath.Length);
        }

        var replace = fullPath.Replace('\\', '/');
        return replace;
    }
}