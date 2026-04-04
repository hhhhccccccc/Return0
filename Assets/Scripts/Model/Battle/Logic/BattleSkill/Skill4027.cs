using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4027 : BattleSkillBase
{
    //刚炁+20，玄炁+20，清除自身2个异常状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 20, BattleSource.Skill);
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 20, BattleSource.Skill);
        DoClearBuffByType(Subject, BuffType.Abnormal, 2);
    }
}