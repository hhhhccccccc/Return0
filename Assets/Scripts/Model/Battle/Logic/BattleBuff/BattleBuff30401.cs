using System;
using cfg;

public class BattleBuff30401 : BattleBuffBase
{
    private bool IsTrigger { get; set; }

    protected override void OnDoDesitionAction()
    {
        base.OnDoDesitionAction();
        if (Subject.GetSkillType() == SkillType.PowerKilling)
        {
            IsTrigger = true;
        }
    }

    protected override void OnTrySetAddWellyRate(int skillGuid, ref float value)
    {
        value = Math.Max(value, LayerCount * Config.ParamEx[0]);
    }

    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        Subject.ReduceHp((Config.ParamEx[1] + Config.ParamEx[2] * Subject.Gr) * LayerCount, DamageType.InDirect, SpellCaster.EntityID, source: BattleSource.Buff);
        IsTrigger = false;
        AddLayerCount(LayerCount);
        base.OnAfterAction(paramModel);
    }

    public override void Recycle()
    {
        IsTrigger = false;
        base.Recycle();
    }
}
