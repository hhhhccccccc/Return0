using System.Collections.Generic;
using System.Linq;
using cfg;

public class BattleBuffShieldDelay : BattleBuffBase
{
    private float DelayShieldValue;
    public override int AddLayerCount(int layerCount)
    {
        base.AddLayerCount(layerCount);
        if (ParamList.Count > 0)
        {
            DelayShieldValue += ParamList[0];
        }

        return LayerCount;
    }

    protected override void OnBattleStart()
    {
        TryAddShield(BattleMomentType.BattleStart);
    }

    protected override void OnRoundStart()
    {
        TryAddShield(BattleMomentType.RoundStart);
    }

    protected override void OnDoDesitionAction(bool isPreDesition)
    {
        TryAddShield(BattleMomentType.DoDesitionAction);
    }

    protected override void OnSelfActionWheelStart()
    {
        TryAddShield(BattleMomentType.SelfActionWheelStart);
    }

    protected override void OnBeforeAction()
    {
        TryAddShield(BattleMomentType.BeforeAction);
    }

    protected override void OnBeforeUnderAction()
    {
        TryAddShield(BattleMomentType.BeforeUnderAction);
    }

    protected override void OnBeforeClash(MomentParamModel paramModel)
    {
        TryAddShield(BattleMomentType.BeforeClash);
    }

    protected override void OnAfterClash(MomentParamModel paramModel)
    {
        TryAddShield(BattleMomentType.AfterClash);
    }

    protected override void OnReleaseSkillAction(MomentParamModel paramModel)
    {
        TryAddShield(BattleMomentType.ReleaseSkillAction);
    }

    protected override void OnAfterUnderAction(MomentParamModel paramModel)
    {
        TryAddShield(BattleMomentType.AfterUnderAction);
    }

    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        TryAddShield(BattleMomentType.AfterAction);
    }

    protected override void OnRoundEnd()
    {
        TryAddShield(BattleMomentType.RoundEnd);
    }

    private void TryAddShield(BattleMomentType momentType)
    {
        if (Config.ParamEx.Any(o => o.ToInt() == (int)momentType))
        {
            if (DelayShieldValue > 0)
            {
                DoAddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject, DelayShieldValue.ToInt(), null, momentType);
            }

            DoClearBuff(Subject, BuffID);
        }
    }

    protected override void ReduceLayerCountByMoment(BattleMomentType momentType, MomentParamModel paramModel = null) {}
    
    protected override void OnBuffRecycle()
    {
        DelayShieldValue = 0;
    }
}
