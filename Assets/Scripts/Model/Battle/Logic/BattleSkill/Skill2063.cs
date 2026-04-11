using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2063 : BattleSkillBase
{
    private bool TriggerInClash;
    private bool CanAddWelly;
    //随机获得3个键
    protected override void OnSelfActionWheelStart()
    {
        DoAddRandomKey(Subject, 3, ChangeKeyReason.SkillEffect);
    }

    public override void BeforeClash(MomentParamModel paramModel)
    {
        var selfKeyCount = Subject.GetAllKeyCount();
        var clashUnit = GetOtherUnit(paramModel);
        var otherKeyCount = clashUnit.GetAllKeyCount();
        if (selfKeyCount > otherKeyCount)
        {
            CanAddWelly = true;
            TriggerInClash = true;
            DoStealBuff(Subject, clashUnit, BuffType.Gain, 1, BattleMomentType.BeforeClash);
        }
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (!TriggerInClash)
        {
            DoStealBuff(Subject, Target, BuffType.Gain, 1, BattleMomentType.BeforeClash);
        }
    }

    public override float GetWellyRateEx(int skillGuid)
    {
        if (CanAddWelly)
        {
            return 0.1f;
        }

        return 0;
    }

    public override void ClearTempData()
    {
        CanAddWelly = false;
        TriggerInClash = false;
    }

    protected override void OnSkillRecycle()
    {
        CanAddWelly = false;
        TriggerInClash = false;
    }
}