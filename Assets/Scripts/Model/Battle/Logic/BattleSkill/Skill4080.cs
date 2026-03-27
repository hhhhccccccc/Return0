using System.Collections.Generic;
using Zenject;

public class Skill4080 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 101020 - ChangeProperty
        Target.ChangeProperty_Abs(BattlePropertyType.GangQi, 5);
        // 效果: 102018 - ChangeProperty
        Target.ChangeProperty_Abs(BattlePropertyType.XuanQi, 5);
        // 效果: 121022103 - AddBuff
        if (Target != null) DoAddBuff(Target, 10221, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

}