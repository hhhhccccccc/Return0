using System.Collections.Generic;
using System.Linq;
using cfg;

public class BattleBuffShieldDelay : BattleBuffBase
{
    private float DelayShieldValue;
    public override void AddLayerCount(int layerCount)
    {
        base.AddLayerCount(layerCount);
        if (ParamList.Count > 0)
        {
            DelayShieldValue += ParamList[0];
        }
    }

    protected override void OnBattleStart()
    {
        base.OnBattleStart();
        TryAddShield(BattleMomentType.BattleStart);
    }

    protected override void OnRoundStart()
    {
        base.OnRoundStart();
        TryAddShield(BattleMomentType.RoundStart);
    }

    protected override void OnDoDesitionAction(bool isPreDesition)
    {
        base.OnDoDesitionAction(isPreDesition);
        TryAddShield(BattleMomentType.DoDesitionAction);
    }

    protected override void OnSelfActionWheelStart()
    {
        base.OnSelfActionWheelStart();
        TryAddShield(BattleMomentType.SelfActionWheelStart);
    }

    protected override void OnBeforeAction()
    {
        base.OnBeforeAction();
        TryAddShield(BattleMomentType.BeforeAction);
    }

    protected override void OnBeforeUnderAction()
    {
        base.OnBeforeUnderAction();
        TryAddShield(BattleMomentType.BeforeUnderAction);
    }

    protected override void OnBeforeClash(MomentParamModel paramModel)
    {
        base.OnBeforeClash(paramModel);
        TryAddShield(BattleMomentType.BeforeClash);
    }

    protected override void OnAfterClash(MomentParamModel paramModel)
    {
        base.OnAfterClash(paramModel);
        TryAddShield(BattleMomentType.AfterClash);
    }

    protected override void OnReleaseSkillAction(MomentParamModel paramModel)
    {
        base.OnReleaseSkillAction(paramModel);
        TryAddShield(BattleMomentType.ReleaseSkillAction);
    }

    protected override void OnAfterUnderAction(MomentParamModel paramModel)
    {
        base.OnAfterUnderAction(paramModel);
        TryAddShield(BattleMomentType.AfterUnderAction);
    }

    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        base.OnAfterAction(paramModel);
        TryAddShield(BattleMomentType.AfterAction);
    }

    protected override void OnRoundEnd()
    {
        base.OnRoundEnd();
        TryAddShield(BattleMomentType.RoundEnd);
    }

    private void TryAddShield(BattleMomentType momentType)
    {
        if (Config.ParamEx.Any(o => o.ToInt() == (int)momentType))
        {
            if (DelayShieldValue > 0)
            {
                BattleBuffManager.AddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject, DelayShieldValue.ToInt(), null, momentType);
            }
            
            ClearLayerCount();
        }
    }

    protected override void ReduceLayerCountByMoment(BattleMomentType momentType, MomentParamModel paramModel = null) {}
    
    protected override void OnBuffRecycle()
    {
        DelayShieldValue = 0;
    }
}
