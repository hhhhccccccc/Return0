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
        ["UIBattlePanel"] = new()
        {
            UIName = "UIBattlePanel",
            LayerType = PanelLayerType.Background,
            PrefabPath = "Assets/Prefab/UI/Battle/UIBattlePanel.prefab"
        } 
    };

    public static SingleUIConfig GetUIConfig(string uiName)
    {
        return UIMap[uiName];
    }
}
