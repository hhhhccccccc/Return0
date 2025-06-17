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

    public Dictionary<int, BattleBuffConfig> GetBattleBuffMap()
    {
        return _tables.TbBattleBuffConfig.DataMap;
    }

    public BattleBuffConfig GetBattleBuff(int buffID)
    {
        return _tables.TbBattleBuffConfig.DataMap[buffID];
    }
    
    public Dictionary<int, BattleSkillConfig> GetBattleSkillMap()
    {
        return _tables.TbBattleSkillConfig.DataMap;
    }

    public BattleSkillConfig GetBattleSkill(int skillID)
    {
        return _tables.TbBattleSkillConfig.DataMap[skillID];
    }

    public Dictionary<int, HeartMethodConfig> GetHeartMethodMap()
    {
        return _tables.TbHeartMethodConfig.DataMap;
    }

    public HeartMethodConfig GetHeartMethod(int heartMethodID)
    {
        return _tables.TbHeartMethodConfig.DataMap[heartMethodID];
    }
    
    public Dictionary<int, TreasureConfig> GetTreasureMap()
    {
        return _tables.TbTreasureConfig.DataMap;
    }

    public TreasureConfig GetTreasure(int treasureID)
    {
        return _tables.TbTreasureConfig.DataMap[treasureID];
    }

    public Dictionary<int, BattleMomentConfig> GetBattleMomentMap()
    {
        return  _tables.TbBattleMomentConfig.DataMap;
    }

    public BattleMomentConfig GetBattleMoment(int battleMomentID)
    {
        return  _tables.TbBattleMomentConfig.DataMap[battleMomentID];
    }

    public Dictionary<int, BattleMomentConditionConfig> GetBattleMomentConditionMap()
    {
        return  _tables.TbBattleMomentConditionConfig.DataMap;
    }

    public BattleMomentConditionConfig GetBattleMomentCondition(int battleMomentConditionID)
    {
        return  _tables.TbBattleMomentConditionConfig.DataMap[battleMomentConditionID];
    }

    public Dictionary<int, BattleMomentEffectConfig> GetBattleMomentEffectMap()
    {
        return   _tables.TbBattleMomentEffectConfig.DataMap;
    }

    public BattleMomentEffectConfig GetBattleMomentEffect(int battleMomentEffectID)
    {
        return _tables.TbBattleMomentEffectConfig.DataMap[battleMomentEffectID];
    }
}
