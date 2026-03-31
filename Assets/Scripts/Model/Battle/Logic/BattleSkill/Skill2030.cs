using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2030 : BattleSkillBase
{
    //随机获得3个键
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddRandomKey(Subject, 3, ChangeKeyReason.SkillEffect);
    }

    //窃取目标2个增益状态
    //todo 若键的数量大于敌手则威力增加10的百分比，且时窃取目标2个增益状态的效果改为交锋时触发
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoStealBuff(Subject, Target, BuffType.Gain, 2, BattleMomentType.ReleaseSkillAction);
    }
}