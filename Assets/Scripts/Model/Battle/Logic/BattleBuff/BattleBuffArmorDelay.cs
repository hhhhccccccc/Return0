using System.Collections.Generic;
using System.Linq;
using cfg;

public class BattleBuffArmorDelay : BattleBuffBase
{
    private float DelayArmorValue;
    public override void AddLayerCount(int layerCount)
    {
        base.AddLayerCount(layerCount);
        if (ParamList.Count > 0)
        {
            DelayArmorValue += ParamList[0];
        }
    }

    protected override void OnBattleStart()
    {
        base.OnBattleStart();
        TryAddArmor(BattleMomentType.BattleStart);
    }
    
    protected override void OnRoundStart()
    {
        base.OnRoundStart();
        TryAddArmor(BattleMomentType.RoundStart);
    }

    protected override void OnDoDesitionAction(bool isPreDesition)
    {
        base.OnDoDesitionAction(isPreDesition);
        TryAddArmor(BattleMomentType.DoDesitionAction);
    }

    protected override void OnSelfActionWheelStart()
    {
        base.OnSelfActionWheelStart();
        TryAddArmor(BattleMomentType.ActionWheelStart);
    }

    protected override void OnBeforeAction()
    {
        base.OnBeforeAction();
        TryAddArmor(BattleMomentType.BeforeAction);
    }

    protected override void OnBeforeUnderAction()
    {
        base.OnBeforeUnderAction();
        TryAddArmor(BattleMomentType.BeforeUnderAction);
    }

    protected override void OnBeforeClash(MomentParamModel paramModel)
    {
        base.OnBeforeClash(paramModel);
        TryAddArmor(BattleMomentType.BeforeClash);
    }

    protected override void OnAfterClash(MomentParamModel paramModel)
    {
        base.OnAfterClash(paramModel);
        TryAddArmor(BattleMomentType.AfterClash);
    }

    protected override void OnReleaseSkillAction(MomentParamModel paramModel)
    {
        base.OnReleaseSkillAction(paramModel);
        TryAddArmor(BattleMomentType.ReleaseSkillAction);
    }

    protected override void OnAfterUnderAction(MomentParamModel paramModel)
    {
        base.OnAfterUnderAction(paramModel);
        TryAddArmor(BattleMomentType.AfterUnderAction);
    }

    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        base.OnAfterAction(paramModel);
        TryAddArmor(BattleMomentType.AfterAction);
    }

    protected override void OnRoundEnd()
    {
        base.OnRoundEnd();
        TryAddArmor(BattleMomentType.RoundEnd);
    }

    private void TryAddArmor(BattleMomentType momentType)
    {
        if (Config.ParamEx.Any(o => o.ToInt() == (int)momentType))
        {
            if (DelayArmorValue > 0)
            {
                BattleBuffManager.AddBuff(Subject, GameConst.Battle.ArmorBuffID, Subject, DelayArmorValue.ToInt(), null, momentType);
            }
            
            ClearLayerCount();
        }
    }

    protected override void ReduceLayerCountByMoment(BattleMomentType momentType, MomentParamModel paramModel = null) { }
    
    protected override void OnRecycle()
    {
        DelayArmorValue = 0;
    }
}
