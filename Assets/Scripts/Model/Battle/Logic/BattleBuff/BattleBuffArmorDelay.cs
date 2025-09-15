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

    public override void BattleStart()
    {
        base.BattleStart();
        TryAddArmor(BattleMomentType.BattleStart);
    }

    public override void RoundStart()
    {
        base.RoundStart();
        TryAddArmor(BattleMomentType.RoundStart);
    }

    public override void DoDesitionAction()
    {
        base.DoDesitionAction();
        TryAddArmor(BattleMomentType.DoDesitionAction);
    }

    public override void ActionWheelStart()
    {
        base.ActionWheelStart();
        TryAddArmor(BattleMomentType.ActionWheelStart);
    }

    public override void BeforeAction()
    {
        base.BeforeAction();
        TryAddArmor(BattleMomentType.BeforeAction);
    }

    public override void BeforeUnderAction()
    {
        base.BeforeUnderAction();
        TryAddArmor(BattleMomentType.BeforeUnderAction);
    }

    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        TryAddArmor(BattleMomentType.BeforeClash);
    }

    public override void AfterClash(MomentParamModel paramModel)
    {
        base.AfterClash(paramModel);
        TryAddArmor(BattleMomentType.AfterClash);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        TryAddArmor(BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        base.AfterUnderAction(paramModel);
        TryAddArmor(BattleMomentType.AfterUnderAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        TryAddArmor(BattleMomentType.AfterAction);
    }

    public override void RoundEnd()
    {
        base.RoundEnd();
        TryAddArmor(BattleMomentType.RoundEnd);
    }

    private void TryAddArmor(BattleMomentType momentType)
    {
        if (Config.ParamEx.Any(o => o.ToInt() == (int)momentType))
        {
            if (DelayArmorValue > 0)
            {
                BattleBuffManager.AddBuff(Subject, GameConst.Battle.ArmorBuffID, Subject, 1,
                    new List<float> { DelayArmorValue });
            }
            
            ClearLayerCount();
        }
    }

    public override void Recycle()
    {
        base.Recycle();
        DelayArmorValue = 0;
    }
}
