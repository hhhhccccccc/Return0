using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4074 : BattleSkillBase
{
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        return 6;
    }
    
    //全部友方获得50%技的护体状态和1层缓速
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var tech = Subject.GetProperty(BattlePropertyType.Tech);
        var shieldValue = tech * 0.5f;
        var teamList = BattleManager.GetAllTeamUnit(Subject.EntityID, true);
        foreach (var unit in teamList)
        {
            DoAddBuff(unit, GameConst.Battle.ShieldBuffID, Subject, (int)shieldValue, null, BattleMomentType.ReleaseSkillAction);
            DoAddBuff(unit, GameConst.Battle.BuffHuanSu, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
        }
    }
}