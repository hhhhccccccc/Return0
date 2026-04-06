using System.Collections.Generic;
using cfg;

public class BattleTreasure10025 : BattleTreasureBase
{
    private float DamageValue => GetConfigParamFloat(0) + GetConfigParamFloat(1) * Subject.Gr;
    private bool CanTrigger { get; set; }
    public override void Init(int treasureID, BattleUnit subject)
    {
        base.Init(treasureID, subject);
        CanTrigger = false;
    }

    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            if (!CanTrigger)
            {
                if (model.GetSelfSkillType(Subject.EntityID) == SkillType.SpellFormula)
                {
                    CanTrigger = true;
                }
            }
            else
            {
                if (model.GetSelfSkillType(Subject.EntityID) == SkillType.PowerKilling)
                {
                    CanTrigger = false;
                }
            }
        }
    }

    protected override void OnAddDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        if (CanTrigger)
        {
            dict.Add(GetSymbol, DamageValue);
        }
    }

    protected override void OnTreasureRecycle()
    {
        CanTrigger = false;
    }
}
