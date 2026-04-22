using UnityEngine;
using UnityEditor;
using Spine.Unity;
using System.IO;
using Spine;

public class SpineToPrefabTool : EditorWindow
{
    private static readonly string PrefabSavePath = "Assets/GameResource/Prefab/SpinePrefab";

    [MenuItem("Assets/创建Spine UI预制体", false, 30)]
    private static void CreateSpineUIPrefab()
    {
        // 获取选中的资源
        Object selectedObject = Selection.activeObject;
        string assetPath = AssetDatabase.GetAssetPath(selectedObject);
        
        // 检查是否是Spine资源（SkeletonDataAsset）
        SkeletonDataAsset skeletonDataAsset = AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>(assetPath);
        
        if (skeletonDataAsset == null)
        {
            EditorUtility.DisplayDialog("错误", "请选择一个Spine的SkeletonDataAsset资源文件！", "确定");
            return;
        }
        // 获取Spine资产上一级目录的名称
        string parentDirectoryName = GetParentDirectoryName(assetPath);

        // 确保保存路径存在
        if (!Directory.Exists(PrefabSavePath))
        {
            Directory.CreateDirectory(PrefabSavePath);
            AssetDatabase.Refresh();
        }
        
        // 获取第一个动画名称
        string firstAnimationName = GetFirstAnimationName(skeletonDataAsset);

        var savePath1 = $"{PrefabSavePath}/{parentDirectoryName}";
        // 确保保存路径存在
        if (!Directory.Exists(savePath1))
        {
            Directory.CreateDirectory(savePath1);
            AssetDatabase.Refresh();
        }
        // 构建完整的保存路径
        string savePath2 = Path.Combine(savePath1, $"{parentDirectoryName}.prefab");
        savePath2 = AssetDatabase.GenerateUniqueAssetPath(savePath2);
        
        // 创建预制体
        CreatePrefabWithSpineUI(skeletonDataAsset, savePath2, firstAnimationName);
    }
    
    /// <summary>
    /// 获取Spine的第一个动画名称
    /// </summary>
    private static string GetFirstAnimationName(SkeletonDataAsset skeletonDataAsset)
    {
        if (skeletonDataAsset == null)
            return null;
        
        // 清除并重新加载SkeletonData，确保获取最新的动画数据
        skeletonDataAsset.Clear();
        SkeletonData skeletonData = skeletonDataAsset.GetSkeletonData(true);
        
        if (skeletonData == null)
        {
            Debug.LogError("无法加载SkeletonData");
            return null;
        }
        
        // 获取所有动画
        var animations = skeletonData.Animations;
        
        if (animations == null || animations.Count == 0)
        {
            Debug.LogWarning("该Spine资源没有动画");
            return null;
        }
        
        // 返回第一个动画的名称
        string firstAnimationName = animations.Items[0].Name;
        
        return firstAnimationName;
    }
    
        /// <summary>
        /// 获取资源的上一级目录名称
        /// </summary>
        private static string GetParentDirectoryName(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
                return "Unknown";
            
            // 获取文件所在目录
            string directory = Path.GetDirectoryName(assetPath);
            
            if (string.IsNullOrEmpty(directory))
                return "Unknown";
            
            // 获取目录名称（最后一级文件夹名）
            string parentDirectoryName = Path.GetFileName(directory);
            
            return parentDirectoryName;
        }
    
    [MenuItem("Assets/创建Spine UI预制体", true)]
    private static bool ValidateCreateSpineUIPrefab()
    {
        // 只在选中SkeletonDataAsset时显示菜单
        if (Selection.activeObject == null)
            return false;
            
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        return AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>(path) != null;
    }
    
    private static void CreatePrefabWithSpineUI(SkeletonDataAsset skeletonDataAsset, string savePath, string firstAnimationName)
    {
        try
        {
            // 创建一个临时GameObject
            GameObject tempGO = new GameObject(skeletonDataAsset.name + "_SpineUI");
            
            // 添加RectTransform（用于UI）
            RectTransform rectTransform = tempGO.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100, 100);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector3.one;
            
            // 添加SkeletonGraphic组件（Spine的UI组件）
            SkeletonGraphic skeletonGraphic = tempGO.AddComponent<SkeletonGraphic>();
            skeletonGraphic.skeletonDataAsset = skeletonDataAsset;
            
            // 设置默认材质（使用Spine的UI材质）
            Material defaultMaterial = Resources.Load<Material>("Spine/SkeletonGraphicDefault");
            if (defaultMaterial != null)
            {
                skeletonGraphic.material = defaultMaterial;
            }
            
            // 设置动画（使用第一个动画名称）
            if (!string.IsNullOrEmpty(firstAnimationName))
            {
                skeletonGraphic.startingAnimation = firstAnimationName;
                skeletonGraphic.startingLoop = true;
                Debug.Log($"已设置默认动画: {firstAnimationName}");
            }
            else
            {
                skeletonGraphic.startingAnimation = "";
                skeletonGraphic.startingLoop = false;
                Debug.LogWarning("未设置默认动画（Spine资源无动画）");
            }
            
            // 设置默认属性
            skeletonGraphic.startingLoop = true;
            skeletonGraphic.initialFlipX = false;
            skeletonGraphic.initialFlipY = false;
            skeletonGraphic.color = Color.white;
            skeletonGraphic.raycastTarget = true;
            
            // 保存预制体
            PrefabUtility.SaveAsPrefabAsset(tempGO, savePath);
            
            // 清理临时对象
            DestroyImmediate(tempGO);
            
            // 刷新资源数据库
            AssetDatabase.Refresh();
            
            // 显示成功消息
            Debug.Log($"✅ Spine预制体创建成功！\n保存路径：{savePath}");
            EditorUtility.DisplayDialog("成功", $"预制体已创建成功！\n保存路径：{savePath}", "确定");
            
            // 在Project窗口中高亮显示创建的预制体
            Object prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(savePath);
            if (prefabAsset != null)
            {
                EditorGUIUtility.PingObject(prefabAsset);
                Selection.activeObject = prefabAsset;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"创建预制体失败：{e.Message}");
            EditorUtility.DisplayDialog("错误", $"创建预制体失败：{e.Message}", "确定");
        }
    }
}