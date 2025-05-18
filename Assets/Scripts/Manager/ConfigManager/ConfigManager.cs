using System.Collections;
using System.Collections.Generic;
using System.IO;
using cfg;
using SimpleJSON;
using UnityEngine;

public class ConfigManager : IManager
{
    private static Tables _tables;

    public IEnumerator Init()
    {
        string gameConfDir = Application.streamingAssetsPath + "/Luban"; // 替换为gen.bat中outputDataDir指向的目录
        _tables = new Tables(file => JSON.Parse(File.ReadAllText($"{gameConfDir}/{file}.json")));
        yield break;
    }

    public static Dictionary<int, BattleBuff> GetBuffMap()
    {
        return _tables.TbBattleBuff.DataMap;
    }

    public static BattleBuff GetBuff(int buffID)
    {
        return _tables.TbBattleBuff.DataMap[buffID];
    }
    
    public static Dictionary<int, BattleSkill> GetSkillMap()
    {
        return _tables.TbBattleSkill.DataMap;
    }

    public static BattleSkill GetSkill(int skillID)
    {
        return _tables.TbBattleSkill.DataMap[skillID];
    }

    public static Dictionary<int, HeartMethod> GeHeartMethodMap()
    {
        return _tables.TbHeartMethod.DataMap;
    }

    public static HeartMethod GetHeartMethod(int heartMethodID)
    {
        return _tables.TbHeartMethod.DataMap[heartMethodID];
    }
    
    public static Dictionary<int, Treasure> GeTreasureMap()
    {
        return _tables.TbTreasure.DataMap;
    }

    public static Treasure GetTreasure(int treasureID)
    {
        return _tables.TbTreasure.DataMap[treasureID];
    }
}
