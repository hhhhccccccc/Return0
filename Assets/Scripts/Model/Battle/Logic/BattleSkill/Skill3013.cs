using System.Collections.Generic;
using Zenject;

public class Skill3013 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2300005 - ChangeSkillGangQiCostByUnitRes
        Subject.GetSkill()?.SetGangQiCost(Math.Min(Subject.GetProperty(BattlePropertyType.GangQi) * 0.6, 60));
        // 效果: 112001101 - AddBuff
        DoAddBuff(Subject, 20011, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122002103 - AddBuff
        if (Target != null) DoAddBuff(Target, 20021, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

}