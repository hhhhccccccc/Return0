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

    public override void BattleStart()
    {
        base.BattleStart();
        TryAddShield(BattleMomentType.BattleStart);
    }

    public override void RoundStart()
    {
        base.RoundStart();
        TryAddShield(BattleMomentType.RoundStart);
    }

    public override void DoDesitionAction()
    {
        base.DoDesitionAction();
        TryAddShield(BattleMomentType.DoDesitionAction);
    }

    public override void ActionWheelStart()
    {
        base.ActionWheelStart();
        TryAddShield(BattleMomentType.ActionWheelStart);
    }

    public override void BeforeAction()
    {
        base.BeforeAction();
        TryAddShield(BattleMomentType.BeforeAction);
    }

    public override void BeforeUnderAction()
    {
        base.BeforeUnderAction();
        TryAddShield(BattleMomentType.BeforeUnderAction);
    }

    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        TryAddShield(BattleMomentType.BeforeClash);
    }

    public override void AfterClash(MomentParamModel paramModel)
    {
        base.AfterClash(paramModel);
        TryAddShield(BattleMomentType.AfterClash);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        TryAddShield(BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        base.AfterUnderAction(paramModel);
        TryAddShield(BattleMomentType.AfterUnderAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        TryAddShield(BattleMomentType.AfterAction);
    }

    public override void RoundEnd()
    {
        base.RoundEnd();
        TryAddShield(BattleMomentType.RoundEnd);
    }

    private void TryAddShield(BattleMomentType momentType)
    {
        if (Config.ParamEx.Any(o => o.ToInt() == (int)momentType))
        {
            if (DelayShieldValue > 0)
            {
                BattleBuffManager.AddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject, 1,
                    new List<float> { DelayShieldValue });
            }
            
            ClearLayerCount();
        }
    }

    public override void Recycle()
    {
        base.Recycle();
        DelayShieldValue = 0;
    }
}
