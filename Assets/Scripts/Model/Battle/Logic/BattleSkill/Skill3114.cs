using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3114 : BattleSkillBase
{
    //获得2层力增
    protected override void OnSelfActionWheelStart()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffLiZeng, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    //消耗目标1个键
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoRemoveRandomKey(Target, 1, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
    }
    
    private bool CanAddWelly { get; set; }

    public override void DoDesitionAction(bool isPreDesition)
    {
        if (CheckSkillLastClashState(Subject, SkillID, false))
        {
            CanAddWelly = true;
        }
    }

    public override float GetWellyRateEx(int skillGuid)
    {
        if (CanAddWelly)
        {
            return 0.8f;
        }

        return 0;
    }

    public override void ClearTempData()
    {
        CanAddWelly = false;
    }

    protected override void OnSkillRecycle()
    {
        CanAddWelly = false;
    }
    
    //todo 在下1息友方首次以敌方作为行动目标行动后对其行动目标重复此招式
}