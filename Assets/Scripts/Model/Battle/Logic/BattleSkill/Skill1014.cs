using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1014 : BattleSkillBase
{
    protected override int DontBeCounter(MomentParamModel paramModel)
    {
        if (CheckProperty(Subject, BattlePropertyType.GangQi, DataType.Int, 50, DataRelation.DaYuDengYu))
        {
            return 1;
        }
        return 0;
    }
    //将自身至多2个随机异常状态转为等量随机1层增益状态（迅速/刚聚/力增/武增）
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // ParamList: [1, 200001, 2] → 自己，200001(增益池)，2(转换2层)
        DoConvertBuffAbnormalToGain(Subject, 200001, 2, BattleMomentType.ReleaseSkillAction);
    }

    //获得6个不同的键，获得2层心眼
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        DoAddRandomKey(Subject, 6, ChangeKeyReason.SkillEffect);
        DoAddBuff(Subject, GameConst.Battle.BuffXinYan, Subject, 2, null, BattleMomentType.AfterAction);
    }
}