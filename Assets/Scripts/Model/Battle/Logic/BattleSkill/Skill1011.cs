using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1011 : BattleSkillBase
{
    private const float ReduceValue = 5f;
    //行动延迟2息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, -2);
    }

    //刚炁+50 每次使用后该招式减少2玄炁消耗减少5刚炁增加
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        var useCount = Target.PreUseSkillDataManager.GetSkillUseCount(SkillGuid);
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 50 - useCount * ReduceValue, BattleSource.Skill);
    }
}