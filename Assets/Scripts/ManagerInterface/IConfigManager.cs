using System.Collections.Generic;
using cfg;

public interface IConfigManager : IManager
{
     Dictionary<int, BattleBuff> GetBuffMap();
     BattleBuff GetBuff(int buffID);
     Dictionary<int, BattleSkill> GetSkillMap();
     BattleSkill GetSkill(int skillID);
     Dictionary<int, HeartMethod> GeHeartMethodMap();
     HeartMethod GetHeartMethod(int heartMethodID);
     Dictionary<int, Treasure> GeTreasureMap();
     Treasure GetTreasure(int treasureID);
}
