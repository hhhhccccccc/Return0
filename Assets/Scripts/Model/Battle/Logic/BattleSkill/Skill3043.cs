using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3043 : BattleSkillBase
{
    //未产生交锋则恢复伤害等量的体
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel { BattleClashType: BattleClashType.SingleAction } model)
        {
            var hpValue = model.GetSelfAttackHpValue(Subject.EntityID);
            Subject.HealHp(hpValue, BattleSource.Skill);
        }
    }
    
    private bool CanAddWelly { get; set; }

    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetOtherUnit(paramModel);
        if (!BattleBuffManager.CheckTargetHasUpSkillBuff(clashUnit.EntityID))
        {
            CanAddWelly = true;
        }
    }

    public override float GetWellyRateEx(int skillGuid)
    {
        if (CanAddWelly)
        {
            return 0.5f;
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

}