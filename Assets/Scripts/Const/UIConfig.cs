using System;
using System.Collections.Generic;

public class SingleUIConfig
{
    public string UIName;
    public PanelLayerType LayerType;
    public string PrefabPath;
}

public static class UIConfig
{
    private static Dictionary<string, SingleUIConfig> UIMap = new Dictionary<string, SingleUIConfig>
    {
        ["UIBattle"] = new SingleUIConfig
        {
            UIName = "UIBattle",
            LayerType = PanelLayerType.Background,
            PrefabPath = "Assets/Prefab/Battle/UIBattle.prefab",
        } 
    };

    public static SingleUIConfig GetUIConfig(string uiName)
    {
        return UIMap[uiName];
    }
}
