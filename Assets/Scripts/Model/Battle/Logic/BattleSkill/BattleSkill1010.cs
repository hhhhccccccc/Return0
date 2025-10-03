using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleSkill1010 : BattleSkillBase
{
    private const float ReduceValue = 5f;
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        var useCount = Target.PreUseSkillDataManager.GetSkillUseCount(SkillID);
        Subject.ChangeProperty(BattlePropertyType.XuanQi, Config.ParamEx[0] - useCount * ReduceValue, BattleSource.Skill);
    }
}