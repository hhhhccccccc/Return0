using System;
using cfg;

public class BattleBuff30401 : BattleBuffBase
{
    private bool IsTrigger { get; set; }
    protected override void OnDoDesitionAction(bool isPreDesition)
    {
        if (Subject.GetSkillType() == SkillType.PowerKilling)
        {
            IsTrigger = true;
        }
    }

    protected override void OnTrySetWellyRateEx(int skillGuid, ref float value)
    {
        value = Math.Max(value, LayerCount * GetConfigParamFloat(0));
    }

    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoReduceHp(Subject, (GetConfigParamFloat(1) + GetConfigParamFloat(2) * Subject.Gr) * LayerCount, DamageType.InDirect, SpellCaster, BattleSource.Buff);
        IsTrigger = false;
        DoAddBuffLayerCount(Subject, BuffID, LayerCount);
    }
    protected override void OnBuffRecycle()
    {
        IsTrigger = false;
    }
}
