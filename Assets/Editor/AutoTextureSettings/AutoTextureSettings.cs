// 文件路径：Assets/Editor/AutoTextureSettings.cs
using UnityEditor;
using UnityEngine;

public class AutoTextureSettings : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        TextureImporter importer = (TextureImporter)assetImporter;
        
        // ========== 根据路径自动识别类型 ==========
            // 角色/道具精灵图
        if (assetPath.Contains("UIRaw"))
        {
            importer.mipmapEnabled = false;      // 大多数2D图片关闭mipmap
            importer.filterMode = FilterMode.Bilinear;
            importer.maxTextureSize = 2048;
            importer.textureCompression = TextureImporterCompression.Compressed;
            importer.textureType = TextureImporterType.Sprite;
            importer.spritePixelsPerUnit = 16;
            //importer.spriteMeshType = SpriteMeshType.Tight;
            // Android设置
            TextureImporterPlatformSettings androidSettings = importer.GetPlatformTextureSettings("Android");
            androidSettings.overridden = true;
            androidSettings.format = TextureImporterFormat.ASTC_6x6;
            androidSettings.maxTextureSize = importer.maxTextureSize;
            importer.SetPlatformTextureSettings(androidSettings);
        
            // iOS设置
            TextureImporterPlatformSettings iosSettings = importer.GetPlatformTextureSettings("iPhone");
            iosSettings.overridden = true;
            iosSettings.format = TextureImporterFormat.ASTC_6x6;
            iosSettings.maxTextureSize = importer.maxTextureSize;
            importer.SetPlatformTextureSettings(iosSettings);
        
            // WebGL设置
            TextureImporterPlatformSettings webglSettings = importer.GetPlatformTextureSettings("WebGL");
            webglSettings.overridden = true;
            webglSettings.format = TextureImporterFormat.DXT5;
            webglSettings.maxTextureSize = importer.maxTextureSize;
            importer.SetPlatformTextureSettings(webglSettings);
        }
    }
}