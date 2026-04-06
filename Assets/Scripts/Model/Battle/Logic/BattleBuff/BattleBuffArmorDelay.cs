using System.Collections.Generic;
using System.Linq;
using cfg;

public class BattleBuffArmorDelay : BattleBuffBase
{
    private float DelayArmorValue;
    public override int AddLayerCount(int layerCount)
    {
        base.AddLayerCount(layerCount);
        if (ParamList.Count > 0)
        {
            DelayArmorValue += ParamList[0];
        }

        return LayerCount;
    }

    protected override void OnBattleStart()
    {
        TryAddArmor(BattleMomentType.BattleStart);
    }
    
    protected override void OnRoundStart()
    {
        TryAddArmor(BattleMomentType.RoundStart);
    }

    protected override void OnDoDesitionAction(bool isPreDesition)
    {
        TryAddArmor(BattleMomentType.DoDesitionAction);
    }

    protected override void OnSelfActionWheelStart()
    {
        TryAddArmor(BattleMomentType.SelfActionWheelStart);
    }

    protected override void OnBeforeAction()
    {
        TryAddArmor(BattleMomentType.BeforeAction);
    }

    protected override void OnBeforeUnderAction()
    {
        TryAddArmor(BattleMomentType.BeforeUnderAction);
    }

    protected override void OnBeforeClash(MomentParamModel paramModel)
    {
        TryAddArmor(BattleMomentType.BeforeClash);
    }

    protected override void OnAfterClash(MomentParamModel paramModel)
    {
        TryAddArmor(BattleMomentType.AfterClash);
    }

    protected override void OnReleaseSkillAction(MomentParamModel paramModel)
    {
        TryAddArmor(BattleMomentType.ReleaseSkillAction);
    }

    protected override void OnAfterUnderAction(MomentParamModel paramModel)
    {
        TryAddArmor(BattleMomentType.AfterUnderAction);
    }

    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        TryAddArmor(BattleMomentType.AfterAction);
    }

    protected override void OnRoundEnd()
    {
        TryAddArmor(BattleMomentType.RoundEnd);
    }

    private void TryAddArmor(BattleMomentType momentType)
    {
        if (Config.ParamEx.Any(o => o.ToInt() == (int)momentType))
        {
            if (DelayArmorValue > 0)
            {
                DoAddBuff(Subject, GameConst.Battle.ArmorBuffID, Subject, DelayArmorValue.ToInt(), null, momentType);
            }
            
            ClearLayerCount();
        }
    }

    protected override void ReduceLayerCountByMoment(BattleMomentType momentType, MomentParamModel paramModel = null) { }
    
    protected override void OnBuffRecycle()
    {
        DelayArmorValue = 0;
    }
}
