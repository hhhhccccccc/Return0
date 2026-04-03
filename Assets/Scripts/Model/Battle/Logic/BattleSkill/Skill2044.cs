using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2044 : BattleSkillBase
{
    //若目标的刚炁低于40则施加1层技式禁状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (CheckProperty(Target, BattlePropertyType.GangQi, DataType.Int, 40, DataRelation.XiaoYu))
        {
            DoAddBuff(Target, GameConst.Battle.BuffJiShiJin, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
        }
    }
}