using System;
using System.Collections.Generic;
using System.Linq;


/*/// <summary>
/// 体上限
/// </summary>
public int HpMax;
/// <summary>
/// 体当前
/// </summary>
public int Hp;

/// <summary>
/// 速上限
/// </summary>
public int SpeedMax;
/// <summary>
/// 速当前
/// </summary>
public int Speed;

/// <summary>
/// 力上限
/// </summary>
public int StrengthMax;
/// <summary>
/// 力当前
/// </summary>
public int Strength;

/// <summary>
/// 防上限
/// </summary>
public int PreventMax;
/// <summary>
/// 防当前
/// </summary>
public int Prevent;

/// <summary>
/// 技上限
/// </summary>
public int TechniqueMax;
/// <summary>
/// 技当前
/// </summary>
public int Technique;

/// <summary>
/// 破上限
/// </summary>
public int ResistMax;
/// <summary>
/// 破当前
/// </summary>
public int Resist;

/// <summary>
/// 巧上限
/// </summary>
public int CleverMax;
/// <summary>
/// 巧当前
/// </summary>
public int Clever;
*/
    
/*public int GangQiMax;
public int GangQi;

public int XuanQiMax;
public int XuanQi;

public int KeyMax;*/

public class BattleProperty : IModel
{
    private Dictionary<string, int> PropertyMap = new Dictionary<string, int>();

    public Dictionary<BattleKey, int> KeyMap = new Dictionary<BattleKey, int>();
    public void Init(Character character)
    {
        PropertyMap["HpMax"] = character.Hp;
        PropertyMap["SpeedMax"] = character.Speed;
        PropertyMap["StrengthMax"] = character.Strength;
        PropertyMap["PreventMax"] = character.Prevent;
        PropertyMap["TechniqueMax"] = character.Technique;
        PropertyMap["ResistMax"] = character.Resist;
        PropertyMap["CleverMax"] = character.Clever;
        PropertyMap["GangQiMax"] = character.GangQi;
        PropertyMap["XuanQiMax"] = character.XuanQi;
        PropertyMap["KeyMax"] = BattleConst.KeyMax;
        
        PropertyMap["Hp"] = character.Hp;
        PropertyMap["Speed"] = character.Speed;
        PropertyMap["Strength"] = character.Strength;
        PropertyMap["Prevent"] = character.Prevent;
        PropertyMap["Technique"] = character.Technique;
        PropertyMap["Resist"] = character.Resist;
        PropertyMap["Clever"] = character.Clever;
        PropertyMap["GangQi"] = character.GangQi;
        PropertyMap["XuanQi"] = character.XuanQi;
    }
    
    public bool ChangeProperty(string propName, int propValue)
    {
        if (!PropertyMap.TryAdd(propName, propValue))
        {
            PropertyMap[propName] += propValue;
        }

        return true;
    }

    public bool SetProperty(string propName, int propValue)
    {
        PropertyMap[propName] = propValue;
        return true;
    }

    public int GetProperty(string propName)
    {
        return PropertyMap.GetValueOrDefault(propName, 0);
    }

    public bool AddKey(BattleKey key, int count = 1)
    {
        var now = KeyMap.Sum(v => v.Value);
        var max = PropertyMap["KeyMax"];
        if (now >= max)
        {
            return false;
        }

        var addCount = Math.Min(max - now, count);
        KeyMap[key] += addCount;
        return true;
    }
    
    public bool CostKey(BattleKey key, int count = 1)
    {
        if (KeyMap[key] < count)
            return false;
        KeyMap[key] -= count;
        return true;
    }
}
