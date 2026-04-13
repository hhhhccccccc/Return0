using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3050 : BattleSkillBase
{
     //玄炁+25，下一息开始获得1层避殃状态
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 25, BattleSource.Skill);
        DoAddBuff(Subject, 90015, Subject, 1, null, BattleMomentType.AfterAction);
    }
}