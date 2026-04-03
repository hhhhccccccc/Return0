using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class Skill2061 : BattleSkillBase
{
    //施加3层刚屏合3层玄屏
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
       DoAddBuff(Target, GameConst.Battle.BuffGangPing, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
       DoAddBuff(Target, GameConst.Battle.BuffXuanPing, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }
    
    private bool DontBeCounter = false;

    //若互为目标且目标招式未带有↓键则本次行动不会被破招
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetClashUnit(paramModel);
        var skill = clashUnit.GetSkill();
        if (CheckMutualGoal(Subject, clashUnit) && skill.GetKeyCostList.All(o => o != (int)BattleKeyType.KeyDown))
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