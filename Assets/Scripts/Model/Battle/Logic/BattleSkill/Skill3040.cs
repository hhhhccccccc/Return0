using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3040 : BattleSkillBase
{
    //todo 在下一息重复该行动，至多重复1次
    public override BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null)
    {
        return new BattleSkillRepeatData
        {
            SkillID = SkillID,
            VariantID = VariantID,
            TargetID = Target.EntityID,
            MaxRepeatCount = 1,
            IfLostChangeToOther = false
        };
    }

    private bool CanAddWelly { get; set; }
    public override void SelfActionWheelStart()
    {
        if (Subject.RoundAlreadyActionTimes == 0)
        {
            CanAddWelly = true;
        }
    }

    public override float GetWellyRateEx(int skillGuid)
    {
        if (CanAddWelly)
        {
            return 0.35f;
        }

        return 0;
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        CanAddWelly = false;
    }

    protected override void OnSkillRecycle()
    {
        CanAddWelly = false;
    }
}