using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1011 : BattleSkillBase
{
    private const float ReduceValue = 5f;
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        var useCount = Target.PreUseSkillDataManager.GetSkillUseCount(SkillGuid);
        DoChangeProperty(Subject, BattlePropertyType.GangQi, Config.ParamEx[0] - useCount * ReduceValue);
    }
}