using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2072 : BattleSkillBase
{
    //下回合不会自然恢复炁
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, 90016, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
}