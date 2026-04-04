using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4015 : BattleSkillBase
{
    //本回合使用过术杀式则玄炁+30
    public override void DoDesitionAction(bool isPreDesition)
    {
        if (CheckRoundUsedSkillType(Subject, BattleLogicStateManager.Round, SkillType.ArtKilling))
        {
            DoChangeProperty(Subject, BattlePropertyType.XuanQi, 30, BattleSource.Skill);
        }
    }

    //刚炁+15
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 15, BattleSource.Skill);
    }
}