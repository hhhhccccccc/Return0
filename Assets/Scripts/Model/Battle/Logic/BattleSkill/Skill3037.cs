using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3037 : BattleSkillBase
{
    //获得1层御敌式
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffYuDiShi, Subject, 1, null, BattleMomentType.DoDesitionAction);
    }
}