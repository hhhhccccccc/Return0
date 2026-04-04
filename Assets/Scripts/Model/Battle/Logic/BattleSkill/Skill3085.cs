using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3085 : BattleSkillBase
{
    //todo 对全部敌手造成100%力的伤害，施加2层失持状态，本回合扣除1次行动次数
    
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        return 1;
    }
    
    //消耗敌手2个键
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetOtherUnit(paramModel);
        DoRemoveRandomKey(clashUnit, 2, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
    }
}