using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2024 : BattleSkillBase
{
    
    //施加3层刚屏合3层玄屏
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffGangPing, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Target, GameConst.Battle.BuffXuanPing, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }
    
    private bool DontBeCounter = false;
    
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetClashUnit(paramModel);
        if (CheckMutualGoal(Subject, clashUnit) && !BattleBuffManager.CheckTargetHasDownSkillBuff(clashUnit.EntityID))
        {
            DontBeCounter = true;
        }
    }
      
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        if (DontBeCounter)
        {
            return 1;
        }

        return 0;
    }
    
    public override void ClearTempData()
    {
        DontBeCounter = false;
    }

    protected override void OnSkillRecycle()
    {
        DontBeCounter = false;
    }
}

