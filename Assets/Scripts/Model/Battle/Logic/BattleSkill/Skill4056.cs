using System.Collections.Generic;
using Zenject;

public class Skill4056 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 102012 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 25);
        // 效果: 111009102 - AddBuff
        DoAddBuff(Subject, 10091, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 101011 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 25);
        // 效果: 111010102 - AddBuff
        DoAddBuff(Subject, 10101, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 111009105 - AddBuff
        DoAddBuff(Subject, 10091, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 111010105 - AddBuff
        DoAddBuff(Subject, 10101, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
    }

}