using System.Collections;
using System.Collections.Generic;
using System.IO;
using cfg;
using SimpleJSON;
using UnityEngine;

public class ConfigManager : ManagerBase, IConfigManager
{
    private Tables _tables;

    protected override IEnumerator OnInit()
    {
        string gameConfDir = Application.streamingAssetsPath + "/Luban"; // 替换为gen.bat中outputDataDir指向的目录
        _tables = new Tables(file => JSON.Parse(File.ReadAllText($"{gameConfDir}/{file}.json")));
        yield break;
    }

    public Dictionary<int, BattleBuff> GetBuffMap()
    {
        return _tables.TbBattleBuff.DataMap;
    }

    public BattleBuff GetBuff(int buffID)
    {
        return _tables.TbBattleBuff.DataMap[buffID];
    }
    
    public Dictionary<int, BattleSkill> GetSkillMap()
    {
        return _tables.TbBattleSkill.DataMap;
    }

    public BattleSkill GetSkill(int skillID)
    {
        return _tables.TbBattleSkill.DataMap[skillID];
    }

    public Dictionary<int, HeartMethod> GeHeartMethodMap()
    {
        return _tables.TbHeartMethod.DataMap;
    }

    public HeartMethod GetHeartMethod(int heartMethodID)
    {
        return _tables.TbHeartMethod.DataMap[heartMethodID];
    }
    
    public Dictionary<int, Treasure> GeTreasureMap()
    {
        return _tables.TbTreasure.DataMap;
    }

    public Treasure GetTreasure(int treasureID)
    {
        return _tables.TbTreasure.DataMap[treasureID];
    }
}
