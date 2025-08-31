using System.Collections.Generic;
using cfg;

public interface IConfigManager : IManager
{
     Dictionary<int, BattleBuffConfig> GetBattleBuffMap();
     BattleBuffConfig GetBattleBuff(int buffID);
     Dictionary<int, BattleBuffRelationConfig> GetBattleBuffRelationMap();
     BattleBuffRelationConfig GetBattleBuffRelation(int buffID);
     Dictionary<int, BattleSkillConfig> GetBattleSkillMap();
     BattleSkillConfig GetBattleSkill(int skillID);
     Dictionary<int, HeartMethodConfig> GetHeartMethodMap();
     HeartMethodConfig GetHeartMethod(int heartMethodID);
     Dictionary<int, TreasureConfig> GetTreasureMap();
     TreasureConfig GetTreasure(int treasureID);
     Dictionary<int, BattleMomentConfig> GetBattleMomentMap();
     BattleMomentConfig GetBattleMoment(int battleMomentID);
     Dictionary<int, BattleMomentConditionConfig> GetBattleMomentConditionMap();
     BattleMomentConditionConfig GetBattleMomentCondition(int battleMomentConditionID);
     Dictionary<int, BattleMomentEffectConfig> GetBattleMomentEffectMap();
     BattleMomentEffectConfig GetBattleMomentEffect(int battleMomentEffectID);
}
