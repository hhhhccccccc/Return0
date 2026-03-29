using System.Collections.Generic;
using Zenject;

public class Skill4022 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        return 5;
    }
    
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111007105 - AddBuff
        DoAddBuff(Subject, 10071, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 111008105 - AddBuff
        DoAddBuff(Subject, 10081, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 3400001 - AddActionTimes
        Subject.AddActionTimes(1);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102015 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 9500);
    }

}