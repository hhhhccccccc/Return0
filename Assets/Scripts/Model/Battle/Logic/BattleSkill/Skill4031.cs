using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4031 : BattleSkillBase
{
    //随机获得7个键
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 7, ChangeKeyReason.SkillEffect);
    }
}