using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3091 : BattleSkillBase
{
    //招式的刚炁消耗转为当前50%，至多50
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.GangQi, 0.5f, 50);
    }
    
    //若时段为昼则威力增加10的百分比
    public override float GetWellyRateEx(int skillGuid)
    {
        if (BattleLogicStateManager.BattleChronoType == ChronoType.Morning ||
            BattleLogicStateManager.BattleChronoType == ChronoType.Sunrise)
        {
            return 0.1f;
        }

        return 0;
    }
}