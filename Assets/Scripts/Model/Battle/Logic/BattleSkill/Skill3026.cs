using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3026 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffLeiXing, Subject, 3, null, BattleMomentType.DoDesitionAction);
    }
}