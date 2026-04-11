using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2040 : BattleSkillBase
{
    //刚炁+20，本回合下次术杀式威力增加15的百分比 //todo 本回合下次术杀式威力增加15的百分比
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 20, BattleSource.Skill);
        DoAddBuff(Subject, 90010, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
}